using XperiCad.Common.Infrastructure.Behaviours.Queries;

namespace XperiCad.DigitalDrawingStore.BL.Documents.Queries
{
    /// <summary>
    /// This interface is for querying all attributes to configure which are contained by a given category.
    /// </summary>
    public interface IDocumentMetadataDefinitionQuery
    {
        /// <summary>
        /// Queries all attributes from the database.
        /// </summary>
        /// <returns></returns>
        Task<IPromise<IDictionary<Guid, string>>> QueryDocumentMetadataDefinitionsAsync();
        /// <summary>
        /// Queries all attributes from the database.
        /// </summary>
        /// <returns></returns>
        Task<IPromise<IDictionary<string, string>>> QueryDocumentCategoryEntitiesAsync();

        /// <summary>
        /// Queries all attributes related to a specified category with a given id.
        /// </summary>
        /// <param name="categoryId">The Guid of the selected category.</param>
        /// <returns></returns>
        Task<IPromise<IDictionary<string, string>>> QueryDocumentCategoryEntitiesAsync(Guid categoryId);
    }
}
