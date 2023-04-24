using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Documents.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands.Get
{
    public class GetDocumentCategoriesActionCommand : AActionCommand<IEnumerable<DocumentCategory>>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private IDocumentService _documentService;
        private IDocumentWatermarkFactory _documentWatermarkFactory;
        private readonly string _searchText;
        #endregion

        #region ctor
        public GetDocumentCategoriesActionCommand(string searchText)
        {
            var documentServiceFactory = new DocumentServiceFactory(); // TODO: inject
            _documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _documentWatermarkFactory = new DocumentWatermarkFactory();
            _searchText = string.IsNullOrWhiteSpace(searchText) ? "*" : searchText;
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var response = new List<DocumentCategory>();

            var documentCategoriesPromise = await _documentService.QueryDocumentCategoriesAsync();
            QueueFeedback(documentCategoriesPromise);

            if (documentCategoriesPromise.IsOkay)
            {
                var selectedCulture = CultureService.GetSelectedCulture();
                var documentNameAttributeName = CultureService.GetPropertyNameTranslation(CultureProperty.DOCUMENT_NAME_CATEGORY_NAME, selectedCulture) ?? "[Név]";
                foreach (var documentCategory in documentCategoriesPromise.ResponseObject)
                {
                    var documentsPromise = await _documentService.QueryDocumentsAsync(documentCategory.Id, _searchText);
                    QueueFeedback(documentsPromise);

                    var documentDtoCollection = new List<Document>();

                    if (documentsPromise.ResponseObject != null)
                    {
                        foreach (var document in documentsPromise.ResponseObject)
                        {
                            var documentAttributes = new Dictionary<string, string?>();
                            var documentCategoryAttributes = await documentCategory.GetAttributesAsync();
                            foreach (var documentCategoryAttribute in documentCategoryAttributes)
                            {
                                documentAttributes.Add(documentCategoryAttribute.Key, document.GetAttribute<string?>(documentCategoryAttribute.Key).Result);
                            }
                            var documentDto = new Document(document.Id, document.NameWithExtension, documentAttributes);
                            documentDtoCollection.Add(documentDto);
                        }
                    }
                    var attributesList = await documentCategory.GetAttributesAsync();
                    var nameAttributesList = new Dictionary<string, string> { ["nameWithExtension"] = documentNameAttributeName };
                    var fullAttributesList = nameAttributesList.Concat(attributesList).GroupBy(d => d.Key)
                        .ToDictionary(d => d.Key, d => d.First().Value);
                    response.Add(new DocumentCategory(documentCategory.Id, await documentCategory.GetDisplayNameAsync(), fullAttributesList, documentDtoCollection, documentCategory.IsDesigned));
                }
            }

            ResolveAction(response);
        }
        #endregion
    }
}
