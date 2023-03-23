using XperiCad.DigitalDrawingStore.BL.Application;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories._Interfaces
{
    internal interface IDocumentResourceProperiesFactory
    {
        IDocumentResourceProperties CreateDocumentResourceProperties(string applicationConfigurationFilePath);
    }
}
