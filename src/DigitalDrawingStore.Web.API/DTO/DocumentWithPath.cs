namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class DocumentWithPath
    {
        public Guid Id { get; }
        public string NameWithExtension { get; }
        public string Path { get; }
        public IDictionary<string, string?> Attributes { get; }
        public SimpleDocumentCategory Category { get; }

        public DocumentWithPath(Guid id, string nameWithExtension, string path, IDictionary<string, string?> attributes, SimpleDocumentCategory category)
        {
            Id = id;
            NameWithExtension = nameWithExtension ?? throw new ArgumentNullException(nameof(nameWithExtension));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            Category = category ?? throw new ArgumentNullException(nameof(category)); ;
        }
    }
}
