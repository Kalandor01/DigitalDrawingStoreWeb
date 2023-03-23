namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class SimpleDocumentCategory
    {
        #region Properties
        public Guid Id { get; }
        public string CategoryName { get; }
        public bool IsDesigned { get; }
        public IDictionary<string, string> Attributes { get; }
        #endregion

        #region ctor
        public SimpleDocumentCategory(
            Guid id,
            string categoryName,
            IDictionary<string, string> attributes,
            bool isDesigned)
        {
            Id = id;
            CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            IsDesigned = isDesigned;
        }
        #endregion
    }
}
