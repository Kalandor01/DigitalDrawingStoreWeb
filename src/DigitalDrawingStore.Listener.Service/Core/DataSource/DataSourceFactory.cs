using XperiCad.Common.Infrastructure.DataSource;

namespace XperiCad.Common.Core.DataSource
{
    internal class DataSourceFactory : IDataSourceFactory
    {
        #region IDataSourceFactory members
        public IDataSource CreateMsSqlDataSource(string connectionString)
        {
            return new MsSqlDataSource(connectionString);
        }
        #endregion
    }
}
