﻿using System.Data;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents.Commands;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Commands
{
    internal class MsSqlUpdateDocumentCategoryCommand : IUpdateDocumentCategoryCommand
    {
        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        #endregion

        #region ctor
        public MsSqlUpdateDocumentCategoryCommand(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
        }
        #endregion

        #region IUpdateDocumentCategoryCommand members
        public bool UpdateDocumentCategory(Guid id, string categoryName, bool isDesigned)
        {
            if (Equals(id, Guid.Empty) || string.IsNullOrWhiteSpace(categoryName))
            {
                return false;
            }

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@CategoryId", SqlDbType.UniqueIdentifier, id)
                                .ConfigureParameter("@CategoryName", SqlDbType.VarChar, categoryName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .ConfigureParameter("@IsDesigned", SqlDbType.Bit, isDesigned)
                                .GetConfiguredParameters();

            _ = _msSqlDataSource.PerformCommand(
                $"   UPDATE {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY]}"
                + $" SET DisplayName = @CategoryName ,"
                + $" IsDesigned = @IsDesigned"
                + $" WHERE Id = @CategoryId",
                parameters);

            return true;
        }
        #endregion
    }
}
