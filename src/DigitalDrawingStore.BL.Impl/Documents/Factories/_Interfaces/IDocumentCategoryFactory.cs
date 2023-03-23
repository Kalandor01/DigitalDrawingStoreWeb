using XperiCad.DigitalDrawingStore.BL.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories
{
    internal interface IDocumentCategoryFactory
    {
        IDocumentCategory CreateDocumentCategory(Guid id);
        IDocumentCategory CreateDocumentCategory(Guid id, bool isDesigned);
    }
}
