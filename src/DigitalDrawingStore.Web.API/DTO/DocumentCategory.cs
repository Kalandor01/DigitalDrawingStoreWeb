namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class DocumentCategory : SimpleDocumentCategory
    {
        #region Properties
        public IEnumerable<Document> Documents { get; }
        #endregion

        #region ctor
        public DocumentCategory(
            Guid id,
            string categoryName,
            IDictionary<string, string> attributes,
            IEnumerable<Document> documents,
            bool isDesigned) : base (
                id,
                categoryName,
                attributes,
                isDesigned)
        {
            Documents = documents ?? throw new ArgumentNullException(nameof(documents));
        }
        #endregion
    }
}
