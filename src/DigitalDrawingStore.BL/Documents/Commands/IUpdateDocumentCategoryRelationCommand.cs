namespace XperiCad.DigitalDrawingStore.BL.Documents.Commands
{
    /// <summary>
    /// This interface is for updating which category the document belongs to.
    /// </summary>
    public interface IUpdateDocumentCategoryRelationCommand
    {
        /// <summary>
        /// Updates which category the document belongs to.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="newCategoryId"></param>
        /// <returns></returns>
        Task<bool> UpdateDocumentCategoryRelationAsync(Guid documentId, Guid newCategoryId);
    }
}
