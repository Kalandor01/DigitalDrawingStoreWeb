namespace XperiCad.DigitalDrawingStore.BL.Documents
{
    public interface IDocumentCategory
    {
        Guid Id { get; }
        bool IsDesigned { get; }
        void SetDocumentCategoryEntityAsync(Guid entityId);

        Task<IDictionary<string, string>> GetAttributesAsync(string? selectedCulture);
        Task<string> GetDisplayNameAsync();
    }
}
