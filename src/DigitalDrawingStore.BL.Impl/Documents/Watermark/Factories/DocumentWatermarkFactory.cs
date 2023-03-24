using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Watermark;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark.Factories
{
    public class DocumentWatermarkFactory : IDocumentWatermarkFactory
    {
        public IDocumentWatermark CreateWatermark(
            string text,
            int fontSize,
            float opacity,
            int rotation,
            WatermarkVerticalPosition verticalPosition,
            WatermarkHorizontalPosition horizontalPosition)
        {
            return CreateWatermark(text, fontSize, opacity, rotation, 0, 0, verticalPosition, horizontalPosition);
        }

        public IDocumentWatermark CreateWatermark(
            string text,
            int fontSize,
            float opacity,
            int rotation,
            int offsetX,
            int offsetY,
            WatermarkVerticalPosition verticalPosition,
            WatermarkHorizontalPosition horizontalPosition)
        {
            ValidateInput(fontSize, opacity, rotation, offsetX, offsetY);

            return new DocumentWatermark(text, fontSize, opacity, rotation, offsetX, offsetY, verticalPosition, horizontalPosition);
        }

        private static void ValidateInput(int fontSize, float opacity, int rotation, int offsetX, int offsetY)
        {
            if (fontSize < Constants.Documents.Watermark.MIN_FONT_SIZE || fontSize > Constants.Documents.Watermark.MAX_FONT_SIZE)
            {
                throw new ArgumentException($"Watermark font sizes should be between {Constants.Documents.Watermark.MIN_FONT_SIZE} and {Constants.Documents.Watermark.MAX_FONT_SIZE}.");
            }

            if (opacity < Constants.Documents.Watermark.MIN_OPACITY || opacity > Constants.Documents.Watermark.MAX_OPACITY)
            {
                throw new ArgumentException($"Watermark opacity value should be between {Constants.Documents.Watermark.MIN_OPACITY} and {Constants.Documents.Watermark.MAX_OPACITY}.");
            }

            if (rotation < 0 || rotation > 360)
            {
                throw new ArgumentException($"Watermark rotation value should be between 0 and 360.");
            }

            if (offsetX < -100 || offsetX > 100
                || offsetY < -100 || offsetY > 100)
            {
                throw new ArgumentException($"Offset range should be between {Constants.Documents.Watermark.MIN_OFFSET} and {Constants.Documents.Watermark.MAX_OFFSET}.");
            }
        }
    }
}
