using XperiCad.Common.Infrastructure.Behaviours.Queries;

namespace XperiCad.DigitalDrawingStore.BL.Documents.Queries
{
    /// <summary>
    /// This interface is for querying documents.
    /// </summary>
    public interface IDocumentQuery
    {
        /// <summary>
        /// Queries all of the documents that have the given searchText in them.
        /// </summary>
        Task<IPromise<IEnumerable<IDocument>>> QueryDocumentsAsync(string searchText);

        /// <summary>
        /// Queries all of the documents in a specified category that have the given searchText in them.
        /// </summary>
        Task<IPromise<IEnumerable<IDocument>>> QueryDocumentsAsync(Guid documentCategoryId, string searchText);
        
        /// <summary>
        /// Queries all metadata of a specified document.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<IPromise<IDictionary<string, string>>> QueryDocumentMetadataAsync(Guid documentId);

        /// <summary>
        /// Queries all target of document usage tag
        /// </summary>
        /// <returns></returns>
        Task<IPromise<IDictionary<Guid, string>>> QueryAllTargetOfDocumentUsageAsync();
        
        /// <summary>
        /// Gets the document by Id.
        /// </summary>
        Task<IPromise<IDocument>> GetDocumentByIdAsync(Guid documentId);
    }
}
