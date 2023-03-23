using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DigitalDrawingStore.Listener.Service.Document.Extractors
{
    internal class PdfDocumentExtractor : IDocumentExtractor
    {
        public IDictionary<string, string> GetAllAttributes(string sourceDocumentPath, string documentMetadataFilePath)
        {
            const int CHANGE_NUMBER = 1;
            const int DOCUMENT_NUMBER_INDEX = 2;
            const int DOCUMENT_TYPE_INDEX = 3;
            const int DRAWING_NUMBER_INDEX = 4;
            const int LANGUAGE_INDEX = 5;
            const int PREFIX_INDEX = 6;
            const int DOCUMENT_TITLE_INDEX = 7;
            const int DOCUMENT_TITLE_HU_INDEX = 8;
            const int TYPE_OF_PRODUCT_ON_DRAWING_INDEX = 9;

            var result = new Dictionary<string, string>();

            if (File.Exists(documentMetadataFilePath))
            {
                var metadataFileLines = File.ReadAllLines(documentMetadataFilePath, Encoding.UTF8).ToList();
                var documentDirectory = Path.GetDirectoryName(documentMetadataFilePath);
                var documentName = Path.GetFileName(sourceDocumentPath);
                var documentPath = $"{documentDirectory}\\{documentName}";
                metadataFileLines.RemoveAt(0);

                foreach (var metadataLine in metadataFileLines)
                {
                    if (string.IsNullOrWhiteSpace(metadataLine))
                    {
                        continue;
                    }

                    var metadata = metadataLine.Split(';');

                    if (string.IsNullOrWhiteSpace(metadata[DOCUMENT_TYPE_INDEX]))
                    {
                        metadata[DOCUMENT_TYPE_INDEX] = "Általános dokumentum";
                    }

                    result.Add(Constants.RawDocumentAttributeNames.DOCUMENT_PATH, documentPath);
                    result.Add(Constants.RawDocumentAttributeNames.CHANGE_NUMBER, metadata[CHANGE_NUMBER]);
                    result.Add(Constants.RawDocumentAttributeNames.DOCUMENT_NUMBER, metadata[DOCUMENT_NUMBER_INDEX]);
                    result.Add(Constants.RawDocumentAttributeNames.DOCUMENT_CATEGORY, metadata[DOCUMENT_TYPE_INDEX]); // document type
                    result.Add(Constants.RawDocumentAttributeNames.DRAWING_NUMBER, metadata[DRAWING_NUMBER_INDEX]);
                    result.Add(Constants.RawDocumentAttributeNames.LANGUAGE, metadata[LANGUAGE_INDEX]);
                    result.Add(Constants.RawDocumentAttributeNames.PREFIX, metadata[PREFIX_INDEX]);
                    result.Add(Constants.RawDocumentAttributeNames.DOCUMENT_TITLE, $"{metadata[DOCUMENT_TITLE_INDEX]} - {metadata[DOCUMENT_TITLE_HU_INDEX]}");
                    result.Add(Constants.RawDocumentAttributeNames.TYPE_OF_PRODUCT_ON_DRAWING, metadata[TYPE_OF_PRODUCT_ON_DRAWING_INDEX]);
                }

                var documentVersionMatches = Regex.Matches(documentName, "(?<=_)([a-zA-Z]*?)(?=(\\.pdf))");
                if (documentVersionMatches != null && documentVersionMatches.Count != 0)
                {
                    var documentVersion = documentVersionMatches[0].Value;

                    result.Add(Constants.RawDocumentAttributeNames.DOCUMENT_VERSION, documentVersion);
                }
                else
                {
                    result.Add(Constants.RawDocumentAttributeNames.DOCUMENT_VERSION, "0");
                }

                result.Add(Constants.RawDocumentAttributeNames.DOCUMENT_REVISION_ID, string.Empty);

                var fileCreationTime = File.GetCreationTime(documentPath);
                result.Add(Constants.RawDocumentAttributeNames.DATE_TIME_OF_DOCUMENT_CREATION, fileCreationTime.ToString("yyyy-MM-dd HH:mm:ss"));
                result.Add(Constants.RawDocumentAttributeNames.DATE_TIME_OF_DOCUMENT_APPROVE, fileCreationTime.ToString("yyyy-MM-dd HH:mm:ss"));

                var freeText = GetAllFreeText(documentPath);
                result.Add("FreeText", freeText);
            }

            return result;
        }

        private string GetAllFreeText(string filePath)
        {
            return ExtractFreeTextFromPdf(filePath);
        }

        public string ExtractFreeTextFromPdf(string filePath)
        {
            var pageContents = new List<string>();

            using (var pdfReader = new PdfReader(filePath))
            using (var pdfDoc = new PdfDocument(pdfReader))
            {
                for (var pageNumber = 1; pageNumber <= pdfDoc.GetNumberOfPages(); pageNumber++)
                {
                    var textExtractionStrategy = new SimpleTextExtractionStrategy();
                    pageContents.Add(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(pageNumber), textExtractionStrategy));
                }
            }

            var mergedText = string.Join(" ", pageContents);

            return mergedText;
        }
    }
}
