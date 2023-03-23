namespace XperiCad.DigitalDrawingStore.BL.Documents.Commands
{
    /// <summary>
    /// This interface is for updating the metadata of a document in the database.
    /// </summary>
    public interface IUpdateDocumentMetadataCommand
    {
        /// <summary>
        /// Updates a document metadata in the database.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="metadataName"></param>
        /// <param name="metadataValue"></param>
        /// <param name="oldMetadataName"></param>
        /// <param name="isDesigned"></param>
        /// <returns></returns>
        Task<bool> UpdateDocumentMetadata(Guid documentId, string metadataName, string metadataValue, string oldMetadataName);
    }
}
