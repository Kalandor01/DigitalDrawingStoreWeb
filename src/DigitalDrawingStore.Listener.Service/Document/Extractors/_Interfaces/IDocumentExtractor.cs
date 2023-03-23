using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Document.Extractors
{
    internal interface IDocumentExtractor
    {
        IDictionary<string, string> GetAllAttributes(string sourceDocumentPath, string documentMetadataFilePath);
    }
}
