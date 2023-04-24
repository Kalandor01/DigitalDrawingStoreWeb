using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands.Set
{
    public class UpdateDocumentCategoryAttributesActionCommand : AActionCommand<bool>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private IDocumentService _documentService;
        private readonly Guid _categoryId;
        private readonly string _categoryName;
        private readonly string _isDesigned;
        #endregion

        #region ctor
        public UpdateDocumentCategoryAttributesActionCommand(Guid categoryId, string categoryName, string isDesigned)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException($"'{nameof(categoryName)}' cannot be null or whitespace.", nameof(categoryName));
            }

            if (string.IsNullOrWhiteSpace(isDesigned))
            {
                throw new ArgumentException($"'{nameof(isDesigned)}' cannot be null or whitespace.", nameof(isDesigned));
            }

            var documentServiceFactory = new DocumentServiceFactory(); // TODO: inject
            _documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _categoryId = categoryId;
            _categoryName = categoryName;
            _isDesigned = isDesigned;
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var response = false;

            if (bool.TryParse(_isDesigned, out _))
            {
                response = await _documentService.UpdateDocumentCategoryAsync(_categoryId, _categoryName, Convert.ToBoolean(_isDesigned));
            }

            ResolveAction(response);
        }
        #endregion
    }
}
