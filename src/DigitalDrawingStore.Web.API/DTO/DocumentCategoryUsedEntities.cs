using Newtonsoft.Json;

namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class DocumentCategoryUsedEntities
    {
        public Guid CategoryId { get; }
        public string[] Keys { get; }
        public string[] Values { get; }

        [JsonConstructor]
        public DocumentCategoryUsedEntities(Guid categoryId, string[] keys, string[] values)
        {
            CategoryId = categoryId;
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }
    }
}
