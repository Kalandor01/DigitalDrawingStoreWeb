using System;

namespace DigitalDrawingStore.Listener.Service.Document
{
    internal class DocumentData : IDocumentData
    {
        #region Properties
        public string DocumentPath { get; set; }
        public string DocumentMetadataPath { get; }
        #endregion

        #region Constructor
        public DocumentData(string documentPath, string documentMetadataPath)
        {
            if (string.IsNullOrWhiteSpace(documentPath))
            {
                throw new ArgumentException($"'{nameof(documentPath)}' cannot be null or whitespace.", nameof(documentPath));
            }

            if (string.IsNullOrWhiteSpace(documentMetadataPath))
            {
                throw new ArgumentException($"'{nameof(documentMetadataPath)}' cannot be null or whitespace.", nameof(documentMetadataPath));
            }

            DocumentPath = documentPath;
            DocumentMetadataPath = documentMetadataPath;
        }
        #endregion
    }
}
