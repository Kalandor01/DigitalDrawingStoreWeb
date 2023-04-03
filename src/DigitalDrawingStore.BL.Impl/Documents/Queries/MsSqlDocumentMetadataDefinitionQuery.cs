using XperiCad.Common.Core.Behaviours.Queries;
using XperiCad.Common.Infrastructure.Behaviours.Queries;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Documents.Queries;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;
using i18n = XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Queries
{
    internal class MsSqlDocumentCategoryEntitiesQuery : AQuery, IDocumentMetadataDefinitionQuery
    {
        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDictionary<string, string> _sqlTableNames;
        private readonly IFeedbackMessageFactory _feedbackMessageFactory;
        private readonly IDocumentCategoryFactory _documentCategoryFactory;
        private readonly IDataParameterFactory _dataParameterFactory;
        #endregion

        #region Constructors
        public MsSqlDocumentCategoryEntitiesQuery(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            IDocumentCategoryFactory documentCategoryFactory,
            IFeedbackMessageFactory feedbackMessageFactory)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            _documentCategoryFactory = documentCategoryFactory ?? throw new ArgumentNullException(nameof(documentCategoryFactory));
            _feedbackMessageFactory = feedbackMessageFactory ?? throw new ArgumentNullException(nameof(feedbackMessageFactory));
        }
        #endregion

        #region IDocumentMetadataDefinitionQuery members
        public async Task<IPromise<IDictionary<Guid, string>>> QueryDocumentMetadataDefinitionsAsync()
        {
            var feedbackQueue = GetFeedbackQueue();

            var documentCategoryAttributeEntities = await PerformIdQuery(feedbackQueue);
            if (documentCategoryAttributeEntities == null)
            {
                return RejectPromise<IDictionary<Guid, string>>(feedbackQueue);
            }

            var documentCategoryAttributes = ConvertEntitiesToDocumentMetadataDefinitions(feedbackQueue, documentCategoryAttributeEntities);
            return ResolvePromise(documentCategoryAttributes, feedbackQueue);
        }

        public async Task<IPromise<IDictionary<string, string>>> QueryDocumentCategoryEntitiesAsync()
        {
            var feedbackQueue = GetFeedbackQueue();

            var documentCategoryAttributeEntities = await PerformQuery(feedbackQueue);
            if (documentCategoryAttributeEntities == null)
            {
                return RejectPromise<IDictionary<string, string>>(feedbackQueue);
            }

            var documentCategoryAttributes = ConvertEntitiesToDocumentCategoryEntities(feedbackQueue, documentCategoryAttributeEntities);
            return ResolvePromise(documentCategoryAttributes, feedbackQueue);
        }

        public async Task<IPromise<IDictionary<string, string>>> QueryDocumentCategoryEntitiesAsync(Guid categoryId)
        {
            var feedbackQueue = GetFeedbackQueue();

            if (categoryId == Guid.Empty)
            {
                feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_CouldNotQueryBecauseOfInvalidGuid));
                return RejectPromise<IDictionary<string, string>>(feedbackQueue);
            }

            var documentCategory = _documentCategoryFactory.CreateDocumentCategory(categoryId);

            var documentCategoryAttributeEntities = await documentCategory.GetAttributesAsync();
            if (documentCategoryAttributeEntities == null)
            {
                return RejectPromise<IDictionary<string, string>>(feedbackQueue);
            }

            return ResolvePromise(documentCategoryAttributeEntities, feedbackQueue);
        }
        #endregion

        #region Private methods
        private async Task<IEnumerable<IEntity>> PerformQuery(ICollection<IFeedbackMessage> feedbackQueue)
        {
            // TODO: conscheck for tables

            //TODO: use this feedback message if a table was not found and reject the promise
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            var sqlScript = $" SELECT Id, ExtractedName"
                + $" FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY]}";

            var parameters = _dataParameterFactory.GetConfiguredParameters();

            var documentMetadataDefinitions = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters, "ExtractedName");

            return documentMetadataDefinitions.ResponseObject;
        }

        private async Task<IEnumerable<IEntity>> PerformIdQuery(ICollection<IFeedbackMessage> feedbackQueue)
        {
            // TODO: conscheck for tables

            //TODO: use this feedback message if a table was not found and reject the promise
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            var sqlScript = $" SELECT Id, ExtractedName"
                + $" FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY]}";

            var parameters = _dataParameterFactory.GetConfiguredParameters();

            var documentMetadataDefinitions = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters, "Id", "ExtractedName");

            return documentMetadataDefinitions.ResponseObject;
        }

        private IDictionary<string, string> ConvertEntitiesToDocumentCategoryEntities(ICollection<IFeedbackMessage> feedbackQueue, IEnumerable<IEntity> documentMetadataDefinitions)
        {
            var result = new Dictionary<string, string>();

            if (documentMetadataDefinitions != null)
            {
                foreach (var documentMetadataDefinition in documentMetadataDefinitions)
                {
                    if (documentMetadataDefinition != null && documentMetadataDefinition.Attributes != null)
                    {
                        var extractedName = documentMetadataDefinition?.Attributes["ExtractedName"]?.ToString();

                        if (!string.IsNullOrWhiteSpace(extractedName))
                        {
                            result.Add(extractedName ?? Guid.NewGuid().ToString(), extractedName ?? string.Empty);
                        }
                        else
                        {
                            feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Warning_DocumentMetadataDefinitionCouldNotBeProcessed));
                        }
                    }
                }
            }

            return result;
        }

        private IDictionary<Guid, string> ConvertEntitiesToDocumentMetadataDefinitions(ICollection<IFeedbackMessage> feedbackQueue, IEnumerable<IEntity> documentMetadataDefinitions)
        {
            var result = new Dictionary<Guid, string>();

            if (documentMetadataDefinitions != null)
            {
                foreach (var documentMetadataDefinition in documentMetadataDefinitions)
                {
                    if (documentMetadataDefinition != null && documentMetadataDefinition.Attributes != null)
                    {
                        var isParsed = Guid.TryParse(documentMetadataDefinition?.Attributes["Id"].ToString(), out var id);
                        var extractedName = documentMetadataDefinition?.Attributes["ExtractedName"]?.ToString();

                        if (!string.IsNullOrWhiteSpace(extractedName) && isParsed)
                        {
                            result.Add(id, extractedName ?? string.Empty);
                        }
                        else
                        {
                            feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Warning_DocumentMetadataDefinitionCouldNotBeProcessed));
                        }
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
