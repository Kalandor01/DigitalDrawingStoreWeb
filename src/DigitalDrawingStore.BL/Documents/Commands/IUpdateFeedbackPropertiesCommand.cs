namespace XperiCad.DigitalDrawingStore.BL.Documents.Commands
{
    /// <summary>
    /// This interface is for updating a FeedbackProperty in the database.
    /// </summary>
    public interface IUpdateFeedbackPropertiesCommand
    {
        /// <summary>
        /// Updates a FeedbackProperty in the database.
        /// </summary>
        /// <param name="propertyKey"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        Task<bool> UpdateFeedbackPropertyAsync(string propertyKey, string propertyValue);
    }
}
