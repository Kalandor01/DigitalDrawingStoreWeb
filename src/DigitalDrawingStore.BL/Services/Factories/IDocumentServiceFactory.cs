namespace XperiCad.DigitalDrawingStore.BL.Services.Factories
{
    public interface IDocumentServiceFactory
    {
        IDocumentService CreateDocumentService(string applicationConfigurationFilePath);
        IDocumentService CreateDocumentService(string applicationConfigurationFilePath, IDictionary<string, string> sqlTableNames);
    }
}
