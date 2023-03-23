using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Document.Resources
{
    internal interface IDocumentResource
    {
        void SaveDocuments(IEnumerable<IRawDocument> rawDocuments);
    }
}
