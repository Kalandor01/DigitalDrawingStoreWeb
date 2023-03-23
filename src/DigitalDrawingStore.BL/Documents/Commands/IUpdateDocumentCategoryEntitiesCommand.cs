namespace XperiCad.DigitalDrawingStore.BL.Documents.Commands
{
    /// <summary>
    /// This interface is for updating the attributes of a DocumentCategory in the database.
    /// </summary>
    public interface IUpdateDocumentCategoryEntitiesCommand
    {
        /// <summary>
        /// Updates the attributes of a DocumentCategory in the database.
        /// </summary>
        /// <param name="documentCategoryId"></param>
        /// <param name="categoryEntities"></param>
        /// <returns></returns>
        bool UpdateDocumentCategoryEntities(Guid documentCategoryId, IDictionary<string, string> categoryEntities, IDictionary<Guid, string> metadataDefinitions);
    }
}
