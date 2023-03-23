using XperiCad.Common.Infrastructure.Behaviours.Queries;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Commands;
using XperiCad.DigitalDrawingStore.BL.Documents.Queries;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Services
{
    internal class DocumentService : IDocumentService
    {
        #region Fields
        private readonly IDocumentCategoryQuery _documentCategoryQuery;
        private readonly IDocumentQuery _documentQuery;
        private readonly IDocumentMetadataDefinitionQuery _categoryAttributeQuery;
        private readonly IUpdateDocumentCategoryCommand _updateDocumentCategoryCommand;
        private readonly IUpdateDocumentMetadataCommand _updateDocumentMetadataCommand;
        private readonly IUpdateDocumentCategoryEntitiesCommand _updateDocumentCategoryEntitiesCommand;
        private readonly IUpdateDocumentCategoryRelationCommand _updateDocumentCategoryRelationCommand;
        #endregion

        #region ctor
        public DocumentService(
            IDocumentCategoryQuery documentCategoryQuery,
            IDocumentQuery documentQuery,
            IDocumentMetadataDefinitionQuery categoryAttributeQuery,
            IUpdateDocumentCategoryCommand updateDocumentCategoryCommand,
            IUpdateDocumentMetadataCommand updateDocumentMetadataCommand,
            IUpdateDocumentCategoryEntitiesCommand updateDocumentCategoryEntitiesCommand,
            IUpdateDocumentCategoryRelationCommand updateDocumentCategoryRelationCommand)
        {
            _documentCategoryQuery = documentCategoryQuery ?? throw new ArgumentNullException(nameof(documentCategoryQuery));
            _documentQuery = documentQuery ?? throw new ArgumentNullException(nameof(documentQuery));
            _categoryAttributeQuery = categoryAttributeQuery ?? throw new ArgumentNullException(nameof(categoryAttributeQuery));
            _updateDocumentCategoryCommand = updateDocumentCategoryCommand ?? throw new ArgumentNullException(nameof(updateDocumentCategoryCommand));
            _updateDocumentMetadataCommand = updateDocumentMetadataCommand ?? throw new ArgumentNullException(nameof(updateDocumentMetadataCommand));
            _updateDocumentCategoryEntitiesCommand = updateDocumentCategoryEntitiesCommand ?? throw new ArgumentNullException(nameof(_updateDocumentCategoryEntitiesCommand));
            _updateDocumentCategoryRelationCommand = updateDocumentCategoryRelationCommand ?? throw new ArgumentNullException(nameof(_updateDocumentCategoryRelationCommand));
        }

        public DocumentService(
            IDocumentCategoryQuery documentCategoryQuery,
            IDocumentQuery documentQuery)
        {
            _documentCategoryQuery = documentCategoryQuery ?? throw new ArgumentNullException(nameof(documentCategoryQuery));
            _documentQuery = documentQuery ?? throw new ArgumentNullException(nameof(documentQuery));
        }
        #endregion

        #region IDocumentService members
        //TODO: shouldn't this be async as well?
        public Task<IPromise<IEnumerable<IDocumentCategory>>> QueryDocumentCategoriesAsync()
        {
            return _documentCategoryQuery.QueryDocumentCategoriesAsync();
        }

        public async Task<IPromise<IDictionary<string, string>>> QueryDocumentMetadataAsync(Guid documentId)
        {
            return await _documentQuery.QueryDocumentMetadataAsync(documentId);
        }

        public async Task<IPromise<IDictionary<Guid, string>>> QueryDocumentMetadataDefinitionsAsync()
        {
            return await _categoryAttributeQuery.QueryDocumentMetadataDefinitionsAsync();
        }

        public async Task<IPromise<IDictionary<string, string>>> QueryDocumentCategoryEntitiesAsync()
        {
            return await _categoryAttributeQuery.QueryDocumentCategoryEntitiesAsync();
        }

        public async Task<IPromise<IDictionary<string, string>>> QueryDocumentCategoryEntitiesAsync(Guid categoryId)
        {
            return await _categoryAttributeQuery.QueryDocumentCategoryEntitiesAsync(categoryId);
        }

        public async Task<IPromise<IEnumerable<IDocument>>> QueryAllDocumentsAsync()
        {
            return await QueryDocumentsAsync("*");
        }

        public async Task<IPromise<IEnumerable<IDocument>>> QueryDocumentsAsync(string searchText)
        {
            return await _documentQuery.QueryDocumentsAsync(searchText);
        }

        public async Task<IPromise<IEnumerable<IDocument>>> QueryDocumentsAsync(Guid categoryId, string searchText)
        {
            return await _documentQuery.QueryDocumentsAsync(categoryId, searchText);
        }

        public async Task<IPromise<IDictionary<Guid, string>>> QueryAllTargetOfDocumentUsageAsync()
        {
            return await _documentQuery.QueryAllTargetOfDocumentUsageAsync();
        }

        public async Task<bool> UpdateDocumentCategoryAsync(Guid id, string categoryName, bool isDesigned)
        {
            return await _updateDocumentCategoryCommand.UpdateDocumentCategoryAsync(id, categoryName, isDesigned);
        }

        public async Task<bool> UpdateDocumentCategoryRelationAsync(Guid documentId, Guid newCategoryId)
        {
            return await _updateDocumentCategoryRelationCommand.UpdateDocumentCategoryRelationAsync(documentId, newCategoryId);
        }

        public async Task<bool> UpdateDocumentMetadata(Guid documentId, string metadataName, string metadataValue, string oldMetadataName)
        {
            return await _updateDocumentMetadataCommand.UpdateDocumentMetadata(documentId, metadataName, metadataValue, oldMetadataName);
        }

        public async Task<bool> UpdateDocumentCategoryEntitiesAsync(Guid documentCategoryId, IDictionary<string, string> categoryEntities, IDictionary<Guid, string> metadataDefinitions)
        {
            return await _updateDocumentCategoryEntitiesCommand.UpdateDocumentCategoryEntitiesAsync(documentCategoryId, categoryEntities, metadataDefinitions);
        }
        public async Task<IPromise<IDocument>> GetDocumentByIdAsync(Guid documentId)
        {
            return await _documentQuery.GetDocumentByIdAsync(documentId);
        }
        #endregion
    }
}
