using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using XperiCad.Common.Core.Exceptions;
using XperiCad.Common.Core.Feedback;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark
{
    internal class PdfDocumentWatermarkProvider : IDocumentWatermarkProvider
    {
        private readonly string FONT_PATH = $"{Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System))}\\Fonts\\times.ttf";

        public byte[] ApplyWatermarksOnDocument(string documentPath, IEnumerable<IDocumentWatermark> watermarks)
        {
            if (!File.Exists(documentPath))
            {
                // TODO: fix parameter issue
                var documentName = System.IO.Path.GetFileName(documentPath);
                var feedbackResource = new FeedbackResource(Feedback.Error_DocumentIsNotExists.Severity, Feedback.Error_DocumentIsNotExists.CultureResource, documentName);
                throw new FeedbackException($"Error during applying watermark on document. The target file is not exists: {documentPath}.", feedbackResource);
            }

            var extension = System.IO.Path.GetExtension(documentPath);

            if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                // TODO: fix parameter issue
                var documentName = System.IO.Path.GetFileName(documentPath);
                var feedbackResource = new FeedbackResource(Feedback.Error_DocumentExtensionIsNotSupported.Severity, Feedback.Error_DocumentExtensionIsNotSupported.CultureResource, documentName);
                throw new FeedbackException($"Error during applying watermark on document. The target file is not pdf format: {documentPath}.", feedbackResource);
            }

            using (var stream = new MemoryStream())
            using (var pdfDoc = new PdfDocument(new PdfReader(documentPath), new PdfWriter(stream)))
            {
                foreach (var watermark in watermarks)
                {
                    AnnotatePdf(pdfDoc, watermark);
                }
                pdfDoc.Close();

                var pdfBytes = stream.ToArray();
                return pdfBytes;
            }
        }

        private void AnnotatePdf(PdfDocument pdfDoc, IDocumentWatermark watermark)
        {
            var text = watermark.Text;
            var fontSize = watermark.FontSizeInPt;
            var opacityInPercentage = watermark.OpacityInPercentage;
            var rotationInDegree = watermark.RotationInDegree;
            var verticalPosition = watermark.VerticalPosition;
            var horizontalPosition = watermark.HorizontalPosition;

            opacityInPercentage /= 100;

            var rotationInRads = rotationInDegree * Math.PI / 180;
            PdfFont pdfFont;
            if (File.Exists(FONT_PATH))
            {
                pdfFont = PdfFontFactory.CreateFont(FONT_PATH, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED, false);
            }
            else
            {
                pdfFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN, PdfEncodings.CP1250, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED, false);
            }

            for (var i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                var ps = page.GetPageSize();

                var formHeight = fontSize;
                var formWidth = CalculateFormWidth(text, fontSize, pdfFont);

                var psWidth = ps.GetWidth();
                var psHeight = ps.GetHeight();
                var yWatermarkPosition = CalculateYPosition(verticalPosition, psHeight, formHeight);
                var xWatermarkPosition = CalculateXPosition(horizontalPosition, psWidth, formWidth);

                var graphicsState = new PdfExtGState().SetFillOpacity(opacityInPercentage);
                var textMatrixTransform = CalculateTextMatrixTransform(watermark, rotationInRads, yWatermarkPosition, xWatermarkPosition, formWidth, formHeight);

                ApplyWatermarkOnCanvas(text, fontSize, pdfFont, page, formWidth, graphicsState, textMatrixTransform);
            }
        }

        private static float CalculateFormWidth(string text, int fontSize, PdfFont font)
        {
            var formWidth = 0F;

            foreach (var ch in text)
            {
                var width = font.GetWidth(ch);
                var fontSizeRate = fontSize / 1000F;
                formWidth += width * fontSizeRate;
            }

            return formWidth;
        }

        private static float CalculateYPosition(WatermarkVerticalPosition verticalPosition, float psHeight, float formHeight)
        {
            float yPos;
            switch (verticalPosition)
            {
                case WatermarkVerticalPosition.Center:
                    yPos = psHeight / 2F - formHeight / 2F;
                    break;
                case WatermarkVerticalPosition.Top:
                    yPos = psHeight - formHeight;
                    break;
                case WatermarkVerticalPosition.Bottom:
                    yPos = 0;
                    break;
                default:
                    throw new InvalidOperationException($"Watermark vertical position is not supported: {verticalPosition}.");
            }

            return yPos;
        }

        private static float CalculateXPosition(WatermarkHorizontalPosition horizontalPosition, float psWidth, float formWidth)
        {
            float xPos;
            switch (horizontalPosition)
            {
                case WatermarkHorizontalPosition.Center:
                    xPos = psWidth / 2F - formWidth / 2F;
                    break;
                case WatermarkHorizontalPosition.Left:
                    xPos = 0;
                    break;
                case WatermarkHorizontalPosition.Right:
                    xPos = psWidth - formWidth;
                    break;
                default:
                    throw new InvalidOperationException($"Watermark vertical position is not supported: {horizontalPosition}.");

            }

            return xPos;
        }

        private static AffineTransform CalculateTextMatrixTransform(IDocumentWatermark watermark, double rotationInRads, float yWatermarkPosition, float xWatermarkPosition, float formWidth, int formHeight)
        {
            var centerX = formWidth / 2;
            var centerY = formHeight / 2;

            var transform = new AffineTransform();
            transform.Translate(xWatermarkPosition + watermark.OffsetX, yWatermarkPosition + watermark.OffsetY);
            transform.Rotate(rotationInRads, centerX, centerY);

            var transformValues = new float[6];
            transform.GetMatrix(transformValues);

            return transform;
        }

        private static void ApplyWatermarkOnCanvas(string text, int fontSize, PdfFont pdfFont, PdfPage page, float formWidth, PdfExtGState graphicsState, AffineTransform textMatrixTransform)
        {
            var canvas = new PdfCanvas(page);

            canvas.SaveState()
                .BeginText().SetColor(new DeviceRgb(128, 128, 128), true).SetExtGState(graphicsState)
                .SetTextMatrix(textMatrixTransform)
                .SetFontAndSize(pdfFont, fontSize)
                .ShowText(text)
                .EndText();

            canvas.RestoreState();
        }
    }
}
