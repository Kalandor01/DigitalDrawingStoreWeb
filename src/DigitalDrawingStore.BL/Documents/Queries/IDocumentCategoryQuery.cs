using XperiCad.Common.Infrastructure.Behaviours.Queries;

namespace XperiCad.DigitalDrawingStore.BL.Documents.Queries
{
    public interface IDocumentCategoryQuery
    {
        /// <summary>
        /// Queries all of the documents that have the given searchText in them and categorizes them by technical document category.
        /// </summary>
        /// <returns></returns>
        Task<IPromise<IEnumerable<IDocumentCategory>>> QueryDocumentCategoriesAsync();
    }
}
