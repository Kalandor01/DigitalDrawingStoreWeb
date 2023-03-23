using DigitalDrawingStore.Listener.Service.Document.Extractors;
using System.IO;

namespace DigitalDrawingStore.Listener.Service.Document.Factories
{
    internal class RawDocumentFactory : IRawDocumentFactory
    {
        #region IRawDocumentFactory members
        public IRawDocument CreateRawDocument(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new System.ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            if (Path.GetExtension(path).ToLower() == ".pdf")
            {
                return CreatePdfDocument(path);
            }

            return null;
        }
        #endregion

        #region Private members
        private IRawDocument CreatePdfDocument(string path)
        {
            var fileDirectory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var metadataPath = Path.GetFullPath(Path.Combine(fileDirectory, fileName + ".txt"));

            if (File.Exists(metadataPath))
            {
                return new RawDocument(new PdfDocumentExtractor(), new DocumentData(path, metadataPath));
            }

            return null;
        }
        #endregion
    }
}
