namespace DigitalDrawingStore.Listener.Service.Document
{
    internal interface IDocumentData
    {
        string DocumentPath { get; set; }
        string DocumentMetadataPath { get; }
    }
}
