using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Document.Scout
{
    internal interface IDocumentScout
    {
        IEnumerable<IRawDocument> FindDocuments();
    }
}
