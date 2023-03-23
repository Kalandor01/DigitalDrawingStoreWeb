using System.Data;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Core.Exceptions;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents.Commands;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application.Commands
{
    internal class UpdateFeedbackPropertiesCommand : IUpdateFeedbackPropertiesCommand
    {
        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        #endregion

        #region ctor
        public UpdateFeedbackPropertiesCommand(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
        }
        #endregion

        #region IUpdateFeedbackPropertyCommand members
        public async Task<bool> UpdateFeedbackPropertyAsync(string propertyKey, string propertyValue)
        {
            propertyKey = propertyKey ?? throw new ArgumentNullException(nameof(propertyKey));
            propertyValue = propertyValue ?? throw new ArgumentNullException(nameof(propertyValue));

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@PropertyKey", SqlDbType.VarChar, propertyKey, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .ConfigureParameter("@PropertyValue", SqlDbType.VarChar, propertyValue, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            var sqlScript = $"UPDATE {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_TABLE_NAME_KEY]}"
                            + $" SET PropertyValue = @PropertyValue"
                            + $" WHERE PropertyKey = @PropertyKey";

            var rowsChanged = await _msSqlDataSource.PerformCommandAsync(sqlScript, parameters);

            if (rowsChanged == 0)
            {
                throw new FeedbackException("bad", Resources.i18n.Feedback.Fatal_database);
            }
            return rowsChanged > 0;
        }
        #endregion
    }
}
