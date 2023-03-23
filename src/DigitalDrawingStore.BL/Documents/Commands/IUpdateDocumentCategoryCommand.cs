namespace XperiCad.DigitalDrawingStore.BL.Documents.Commands
{
    /// <summary>
    /// This interface is for updating a DocumentCategory in the database.
    /// </summary>
    public interface IUpdateDocumentCategoryCommand
    {
        /// <summary>
        /// Updates a DocumentCategory in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryName"></param>
        /// <param name="isDesigned"></param>
        /// <returns></returns>
        Task<bool> UpdateDocumentCategoryAsync(Guid id, string categoryName, bool isDesigned);
    }
}
