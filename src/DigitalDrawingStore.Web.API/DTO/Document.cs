namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class Document
    {
        public Guid Id { get; }
        public string NameWithExtension { get; }
        public IDictionary<string, string?> Attributes { get; }

        public Document(Guid id, string nameWithExtension, IDictionary<string, string?> attributes)
        {
            Id = id;
            NameWithExtension = nameWithExtension ?? throw new ArgumentNullException(nameof(nameWithExtension));
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }
    }
}
