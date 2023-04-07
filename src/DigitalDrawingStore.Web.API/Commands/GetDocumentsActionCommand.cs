using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;
using XperiCad.DigitalDrawingStore.Web.API.DTO;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class GetDocumentsActionCommand : AActionCommand<IEnumerable<DocumentWithPath>>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private IDocumentService _documentService;
        private readonly string _searchText;
        #endregion

        #region ctor
        public GetDocumentsActionCommand(string searchText)
        {
            var documentServiceFactory = new DocumentServiceFactory(); // TODO: inject
            _documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _searchText = searchText;
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var response = new List<DocumentWithPath>();

            var documentCategoriesPromise = await _documentService.QueryDocumentCategoriesAsync();
            QueueFeedback(documentCategoriesPromise);

            if (documentCategoriesPromise.IsOkay)
            {
                foreach (var documentCategory in documentCategoriesPromise.ResponseObject)
                {
                    var documentsPromise = await _documentService.QueryDocumentsAsync(documentCategory.Id, _searchText);
                    QueueFeedback(documentsPromise);

                    var simpleDocumentCategory = new SimpleDocumentCategory(documentCategory.Id, await documentCategory.GetDisplayNameAsync(), await documentCategory.GetAttributesAsync(new CultureFactory().GetSelectedCulture()), documentCategory.IsDesigned);

                    var documentDtoCollection = new List<DocumentWithPath>();
                    var documentCategoryAttributes = await documentCategory.GetAttributesAsync(new CultureFactory().GetSelectedCulture());

                    if (documentsPromise.ResponseObject != null)
                    {
                        foreach (var document in documentsPromise.ResponseObject)
                        {
                            var documentAttributes = new Dictionary<string, string?>();
                            foreach (var documentCategoryAttribute in documentCategoryAttributes)
                            {
                                documentAttributes.Add(documentCategoryAttribute.Key, document.GetAttribute<string?>(documentCategoryAttribute.Key).Result);
                            }

                            var documentDto = new DocumentWithPath(document.Id, document.NameWithExtension, document.Path, documentAttributes, simpleDocumentCategory);
                            response.Add(documentDto);
                        }
                    }
                }
            }

            ResolveAction(response);
        }
        #endregion
    }
}
