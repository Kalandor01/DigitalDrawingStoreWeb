namespace DigitalDrawingStore.Listener.Service.Document.Factories
{
    internal interface IRawDocumentFactory
    {
        IRawDocument CreateRawDocument(string path);
    }
}
