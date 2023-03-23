using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Document
{
    internal interface IRawDocument
    {
        IDocumentData DocumentData { get; }
        string GetAttribute(string attributeName);
        IDictionary<string, string> GetAllAttributes();
    }
}
