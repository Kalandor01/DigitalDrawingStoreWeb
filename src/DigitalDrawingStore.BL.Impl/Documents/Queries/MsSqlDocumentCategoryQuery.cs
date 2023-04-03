using XperiCad.Common.Core.Behaviours.Queries;
using XperiCad.Common.Infrastructure.Behaviours.Queries;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Queries;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;
using i18n = XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Queries
{
    internal class MsSqlDocumentCategoryQuery : AQuery, IDocumentCategoryQuery
    {
        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        private readonly IDocumentCategoryFactory _documentCategoryFactory;
        private readonly IFeedbackMessageFactory _feedbackMessageFactory;
        #endregion

        #region ctor
        public MsSqlDocumentCategoryQuery(
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

        #region IDocumentCategoryQuery members
        public async Task<IPromise<IEnumerable<IDocumentCategory>>> QueryDocumentCategoriesAsync()
        {
            var feedbackQueue = GetFeedbackQueue();
            var categoryEntities = await PerformQuery(feedbackQueue);

            //TODO: remove after testing
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoTargetOfDocumentUsageFoundInDatabase));
            //return RejectPromise<IEnumerable<IDocumentCategory>>(feedbackQueue);

            if (categoryEntities == null)
            {
                return RejectPromise<IEnumerable<IDocumentCategory>>(feedbackQueue);
            }

            var categories = ConvertCategoryEntitiesToCategories(feedbackQueue, categoryEntities);

            return ResolvePromise(categories, feedbackQueue);
        }
        #endregion

        #region Private members
        private async Task<IEnumerable<IEntity>> PerformQuery(ICollection<IFeedbackMessage> feedbackQueue)
        {
            // TODO: conscheck in feedbackQueue

            //TODO: use this feedback message if a table was not found and reject the promise
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            var sqlScript = $"SELECT Id, IsDesigned"
                + $" FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY]}";

            var response = await _msSqlDataSource.PerformQueryAsync(sqlScript, "IsDesigned");


            return response.ResponseObject;
        }

        private IEnumerable<IDocumentCategory> ConvertCategoryEntitiesToCategories(ICollection<IFeedbackMessage> feedbackQueue, IEnumerable<IEntity> categoryEntities)
        {
            var result = new List<IDocumentCategory>();

            if (categoryEntities != null)
            {
                foreach (var categoryEntity in categoryEntities)
                {
                    if (categoryEntity != null && categoryEntity.Attributes != null)
                    {
                        var isValueCorrect = bool.TryParse(categoryEntity.Attributes["IsDesigned"].ToString(), out var isDesigned);

                        if (categoryEntity.Id != Guid.Empty && isValueCorrect)
                        {
                            var category = _documentCategoryFactory.CreateDocumentCategory(categoryEntity.Id, isDesigned);
                            result.Add(category);
                        }
                        else
                        {
                            feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Warning_DocumentCategoryCouldNotBeProcessed));
                        }
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
