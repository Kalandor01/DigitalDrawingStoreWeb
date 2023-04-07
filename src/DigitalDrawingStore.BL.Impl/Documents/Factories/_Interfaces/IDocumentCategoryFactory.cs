using XperiCad.DigitalDrawingStore.BL.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories
{
    internal interface IDocumentCategoryFactory
    {
        Task<IDocumentCategory> CreateDocumentCategoryAsync(Guid id);
        Task<IDocumentCategory> CreateDocumentCategoryAsync(Guid id, bool isDesigned);
    }
}
