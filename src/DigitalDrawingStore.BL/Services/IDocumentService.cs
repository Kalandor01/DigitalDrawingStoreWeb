using XperiCad.Common.Infrastructure.Behaviours.Queries;
using XperiCad.DigitalDrawingStore.BL.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Services
{
    /// <summary>
    /// This interface is for querying documents and categories.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Queries all of the document categories from the datasource.
        /// </summary>
        Task<IPromise<IEnumerable<IDocumentCategory>>> QueryDocumentCategoriesAsync();

        /// <summary>
        /// Queries all metadata related to a document with a given id.
        /// </summary>
        Task<IPromise<IDictionary<string, string>>> QueryDocumentMetadataAsync(Guid documentId);

        /// <summary>
        /// Queries all metadata definitions with their id and name.
        /// </summary>
        Task<IPromise<IDictionary<Guid, string>>> QueryDocumentMetadataDefinitionsAsync();

        /// <summary>
        /// Queries all metadata definitions.
        /// </summary>
        Task<IPromise<IDictionary<string, string>>> QueryDocumentCategoryEntitiesAsync();

        /// <summary>
        /// Queries all attributes related to a specified category with a given id.
        /// </summary>
        Task<IPromise<IDictionary<string, string>>> QueryDocumentCategoryEntitiesAsync(Guid categoryId);
        
        /// <summary>
        /// Queries all of the documents from the datasource.
        /// </summary>
        Task<IPromise<IEnumerable<IDocument>>> QueryAllDocumentsAsync();
        
        /// <summary>
        /// Queries all of the documents that have the given searchText in them.
        /// </summary>
        Task<IPromise<IEnumerable<IDocument>>> QueryDocumentsAsync(string searchText);
        
        /// <summary>
        /// Queries all of the documents in a specified category that have the given searchText in them.
        /// </summary>
        Task<IPromise<IEnumerable<IDocument>>> QueryDocumentsAsync(Guid categoryId, string searchText);
        
        /// <summary>
        /// Queries all target of document usage tag
        /// </summary>
        Task<IPromise<IDictionary<Guid, string>>> QueryAllTargetOfDocumentUsageAsync();

        /// <summary>
        /// Updates a document category with a given id in the database.
        /// </summary>
        Task<bool> UpdateDocumentCategoryAsync(Guid id, string categoryName, bool isDesigned);
        /// <summary>
        /// Updates which category the document belongs to.
        /// </summary>
        Task<bool> UpdateDocumentCategoryRelationAsync(Guid documentId, Guid newCategoryId);
        /// <summary>
        /// Updates the metadata of a document with the given id in the database.
        /// </summary>
        Task<bool> UpdateDocumentMetadata(Guid id, string metadataName, string metadataValue, string oldMetadataName);
        /// <summary>
        /// Updates which attributes are related to a document category with a given id.
        /// </summary>
        Task<bool> UpdateDocumentCategoryEntitiesAsync(Guid id, IDictionary<string, string> categoryEntities, IDictionary<Guid, string> metadataDefinitions);
        
        /// <summary>
        /// Gets the document by Id.
        /// </summary>
        Task<IPromise<IDocument>> GetDocumentByIdAsync(Guid documentId);
    }
}
