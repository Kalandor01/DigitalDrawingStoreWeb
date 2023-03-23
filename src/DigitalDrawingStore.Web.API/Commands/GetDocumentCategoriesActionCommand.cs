﻿using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Documents.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;
using XperiCad.DigitalDrawingStore.Web.API.DTO;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
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

                    response.Add(new DocumentCategory(documentCategory.Id, await documentCategory.GetDisplayNameAsync(), await documentCategory.GetAttributesAsync(), documentDtoCollection, documentCategory.IsDesigned));
                }
            }

            ResolveAction(response);
        }
        #endregion
    }
}
