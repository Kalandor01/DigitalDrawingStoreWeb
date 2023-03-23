using System;
using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Document.Resources
{
    internal class DocumentResourcePool : IDocumentResource
    {
        #region Fields
        private readonly IEnumerable<IDocumentResource> _documentResources;
        #endregion

        #region Constructor
        public DocumentResourcePool(IEnumerable<IDocumentResource> documentResources)
        {
            _documentResources = documentResources ?? throw new System.ArgumentNullException(nameof(documentResources));
        }
        #endregion

        #region IDocumentResource members
        public void SaveDocuments(IEnumerable<IRawDocument> rawDocuments)
        {
            if (rawDocuments is null)
            {
                throw new ArgumentNullException(nameof(rawDocuments));
            }

            foreach (var documentResource in _documentResources)
            {
                documentResource.SaveDocuments(rawDocuments);
            }
        }
        #endregion
    }
}
