using DigitalDrawingStore.Listener.Service.Document.Extractors;
using System;
using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Document
{
    internal class RawDocument : IRawDocument
    {
        #region Properties
        public IDocumentData DocumentData { get; }
        #endregion

        #region Fields
        private readonly IDocumentExtractor _documentExtractor;
        #endregion

        #region Constructor
        public RawDocument(IDocumentExtractor documentExtractor, IDocumentData documentData)
        {
            _documentExtractor = documentExtractor ?? throw new ArgumentNullException(nameof(documentExtractor));
            DocumentData = documentData ?? throw new ArgumentNullException(nameof(documentData));
        }
        #endregion

        #region Object overrides
        public override string ToString()
        {
            return DocumentData.DocumentPath;
        }
        #endregion

        #region IRawDocument members
        public string GetAttribute(string attributeName)
        {
            var allAttributes = GetAllAttributes();
            return allAttributes[attributeName];
        }

        public IDictionary<string, string> GetAllAttributes()
        {
            return _documentExtractor.GetAllAttributes(DocumentData.DocumentPath, DocumentData.DocumentMetadataPath);
        }
        #endregion
    }
}
