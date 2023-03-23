namespace XperiCad.DigitalDrawingStore.BL.Test.Documents
{
    public class CategoryDTO
    {
        #region Properties
        public string DisplayName { get; }
        public Guid CategoryId { get; }
        #endregion

        #region ctor
        public CategoryDTO(Guid categoryId, string displayName)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException($"Argument {nameof(categoryId)} could not be an empty Guid.");
            }

            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            CategoryId = categoryId;
        }
        #endregion
    }
}
