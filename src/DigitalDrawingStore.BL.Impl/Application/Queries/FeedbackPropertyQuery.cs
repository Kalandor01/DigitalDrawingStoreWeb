using System.Data;
using XperiCad.Common.Core.Behaviours.Queries;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Infrastructure.Behaviours.Queries;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Documents.Queries;
using i18n = XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application.Queries
{
    internal class FeedbackPropertyQuery : AQuery, IFeedbackPropertyQuery
    {
        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        private readonly IFeedbackMessageFactory _feedbackMessageFactory;
        #endregion

        #region Constructors
        public FeedbackPropertyQuery(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            IFeedbackMessageFactory feedbackMessageFactory)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            _feedbackMessageFactory = feedbackMessageFactory ?? throw new ArgumentNullException(nameof(feedbackMessageFactory));
        }
        #endregion

        #region IDocumentQuery members
        public async Task<IPromise<string>> QueryFeedbackPropertyAsync(string propertyKey)
        {
            var feedbackQueue = GetFeedbackQueue();

            var propertyEntities = await PerformQuery(feedbackQueue, propertyKey);

            var propertyEntity = propertyEntities.FirstOrDefault();
            string? property;
            if (propertyEntity != null)
            {
                property = propertyEntity.Attributes["PropertyValue"].ToString();
                if (property == null)
                {
                    feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_CouldNotQueryBecauseOfInvalidGuid));
                    property = string.Empty;
                }
            }
            else
            {
                feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_CouldNotQueryBecauseOfInvalidGuid));
                property = string.Empty;
            }
            return ResolvePromise(property, feedbackQueue);
        }
        
        private async Task<IEnumerable<IEntity>> PerformQuery(ICollection<IFeedbackMessage> feedbackQueue, string propertyKey)
        {
            // TODO: Implement actual feedback messages
            // feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Test));
            // TODO: check if necessary tables are exists in feedbackQueue
            // TODO: avoid every exception with validations and send feedback if something is bad

            //TODO: use this feedback message if a table was not found and reject the promise
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            if (string.IsNullOrWhiteSpace(propertyKey))
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            var sqlScript = $"SELECT Id, PropertyValue"
                          + $" FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_TABLE_NAME_KEY]}"
                          + $" WHERE PropertyKey = @PropertyKey";

            var parameters = _dataParameterFactory
                .ConfigureParameter("@PropertyKey", SqlDbType.VarChar, propertyKey, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                .GetConfiguredParameters();

            var result = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters, "PropertyValue");

            return result.ResponseObject;
        }
        #endregion
    }
}
