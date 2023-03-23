using System.Data;
using XperiCad.Common.Infrastructure.Application.DataSource;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Application;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application
{
    internal class UserEventLoggingProperties : IUserEventLoggingProperties
    {
        #region Fields
        private readonly IApplicationConfigurationService _applicationConfigurationService;
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        #endregion

        #region ctor
        public UserEventLoggingProperties(
            IApplicationConfigurationService applicationConfigurationService,
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames)
        {
            _applicationConfigurationService = applicationConfigurationService ?? throw new ArgumentNullException(nameof(applicationConfigurationService));
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
        }
        #endregion

        #region IUserEventLoggingProperties members
        public async Task<bool> GetIsLoggingEnabledAsync()
        {
            var sqlScript = $"SELECT Id, PropertyValue"
                                + $" FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_TABLE_NAME_KEY]} ap"
                                + $" WHERE PropertyKey = 'IsLoggingEnabled'";
            var response = await _msSqlDataSource.PerformQueryAsync(sqlScript, "PropertyValue");

            if (response != null && response.IsOkay)
            {
                var applicationPropertyEntities = response.ResponseObject;

                if (applicationPropertyEntities != null)
                {
                    foreach (var categoryEntity in applicationPropertyEntities)
                    {
                        if (categoryEntity != null && categoryEntity.Attributes != null)
                        {
                            var isValueCorrect = bool.TryParse(categoryEntity.Attributes["PropertyValue"].ToString(), out var isLoggingEnabled);

                            if (categoryEntity.Id != Guid.Empty && isValueCorrect)
                            {
                                return await Task.FromResult(isLoggingEnabled);
                            }
                        }
                    }
                }
            }

            return await Task.FromResult(false);
        }

        public async Task SetIsLoggingEnabledAsync(bool isLoggingEnabled)
        {
            var sqlScript = $"   UPDATE {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_TABLE_NAME_KEY]}"
                            + $" SET PropertyValue = @isLoggingEnabled"
                            + $" WHERE PropertyKey = 'IsLoggingEnabled'";

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@isLoggingEnabled", SqlDbType.Bit, isLoggingEnabled)
                                .GetConfiguredParameters();

            _ = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters);
        }


        public Task<string> GetResourceDirectoryAsync()
        {
            return Task.FromResult("UserEventLogs");
        }

        public Task<string> GetResourcePathAsync()
        {
            return Task.FromResult(_applicationConfigurationService.Query.GetStringPropertyByName("DocumentDatabaseConnectionString"));
        }
        #endregion
    }
}
