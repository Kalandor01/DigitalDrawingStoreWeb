namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class DocumentCategoryEntities
    {
        #region Properties
        public IDictionary<string, string> UsedEntities { get; }
        public IDictionary<string, string> UnusedEntities { get; }
        #endregion

        #region ctor
        public DocumentCategoryEntities(
        IDictionary<string, string> usedEntities,
        IDictionary<string, string> unusedEntities)
        {
            UsedEntities = usedEntities ?? throw new ArgumentNullException(nameof(usedEntities));
            UnusedEntities = unusedEntities ?? throw new ArgumentNullException(nameof(unusedEntities));
        }
        #endregion
    }
}
