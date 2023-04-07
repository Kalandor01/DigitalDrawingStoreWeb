using System.ComponentModel.Design;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories
{
    internal class DocumentCategoryFactory : IDocumentCategoryFactory
    {
        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        #endregion

        public DocumentCategoryFactory(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
        }

        #region IDocumentCategoryFactory members
        public async Task<IDocumentCategory> CreateDocumentCategoryAsync(Guid id, bool isDesigned)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"Attribute {nameof(id)} could not be empty Guid.");
            }

            return (IDocumentCategory)(await Task.FromResult(new MsSqlDocumentCategory(id, _msSqlDataSource, _dataParameterFactory, _sqlTableNames, isDesigned)));
        }

        public async Task<IDocumentCategory> CreateDocumentCategoryAsync(Guid id)
        {
            return await CreateDocumentCategoryAsync(id, false);
        }
        #endregion
    }
}
