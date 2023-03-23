namespace XperiCad.Common.Infrastructure.DataSource
{
    public interface IDataSourceFactory
    {
        IDataSource CreateMsSqlDataSource(string connectionString);
    }
}
