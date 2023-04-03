using System.Data;
using System.Net.Sockets;
using System.Net;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Documents.Factories;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents
{
    internal class MsSqlPdfDocument : IDocument
    {
        #region Properties
        public Guid Id { get; }
        public string Path { get; }
        public string Directory { get; }
        public string Extension { get; }
        public string Name { get; }
        public string NameWithExtension { get; }
        #endregion

        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        private readonly IDocumentExporter _documentExporter;
        private readonly IDocumentWatermarkProvider _documentWatermarkProvider;
        #endregion

        #region Constructors
        public MsSqlPdfDocument(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            Guid id,
            string path,
            IDocumentExporter documentExporter,
            IDocumentWatermarkProvider documentWatermarkProvider)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"Parameter {nameof(id)} could not be empty.");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            _documentExporter = documentExporter ?? throw new ArgumentNullException(nameof(documentExporter));
            _documentWatermarkProvider = documentWatermarkProvider ?? throw new ArgumentNullException(nameof(documentWatermarkProvider));

            Id = id;
            Path = path;
            Directory = System.IO.Path.GetDirectoryName(path) ?? throw new ArgumentException($"Argument {nameof(path)} is invalid, no directory found on path: {path}.");
            Extension = System.IO.Path.GetExtension(path) ?? throw new ArgumentException($"Argument {nameof(path)} is invalid, no file extension found on path: {path}.");
            Name = System.IO.Path.GetFileNameWithoutExtension(path) ?? throw new ArgumentException($"Argument {nameof(path)} is invalid, no file name found on path: {path}.");
            NameWithExtension = $"{Name}{Extension}";
        }
        #endregion

        #region IDocument members
        public void ExportAs(string targetPath, IDocumentWatermark documentWatermark)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
            {
                throw new ArgumentException($"'{nameof(targetPath)}' cannot be null or whitespace.", nameof(targetPath));
            }
            _ = documentWatermark ?? throw new ArgumentNullException(nameof(documentWatermark));

            _documentExporter.Export(targetPath, documentWatermark);
        }

        public byte[] Download(IEnumerable<IDocumentWatermark> watermarks)
        {
            if (watermarks is null)
            {
                throw new ArgumentNullException(nameof(watermarks));
            }

            return _documentWatermarkProvider.ApplyWatermarksOnDocument(Path, watermarks); 
        }

        public async Task<T?> GetAttribute<T>(string attribute)
        {
            if (string.IsNullOrWhiteSpace(attribute))
            {
                throw new ArgumentException($"'{nameof(attribute)}' cannot be null or whitespace.", nameof(attribute));
            }

            if (!CheckNecessaryTablesAreExists(out var documentTableName, out var documentMetadataTableName, out var documentMetadataDefinitionsTableName))
            {
                return default;
            }

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@DocumentId", SqlDbType.UniqueIdentifier, Id)
                                .ConfigureParameter("@Attribute", SqlDbType.VarChar, attribute, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            var sqlScript = $" SELECT dm.Id, Value"
                + $" FROM {documentMetadataTableName} dm"
                + $" INNER JOIN {documentTableName} d"
                + $" ON d.Id = dm.DocumentId"
                + $" INNER JOIN {documentMetadataDefinitionsTableName} dmd"
                + $" ON dm.DocumentMetadataDefinitionId = dmd.Id"
                + $" WHERE d.Id = @DocumentId"
                + $" AND dmd.ExtractedName = @Attribute";
            var documentMetadataResult = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters, "Value");
            
            
            if (documentMetadataResult.ResponseObject == null)
            {
                return default;
            }

            if (documentMetadataResult.ResponseObject.Count() > 1)
            {
                // TODO: feedback
                throw new InvalidOperationException($"Error occured during attribute reading from pdf document. Multiple metadata record exists to attribute {attribute} in table {Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY} with attribute named {attribute}.");
            }

            var metadata = documentMetadataResult.ResponseObject.FirstOrDefault();

            if (metadata == null)
            {
                return default;
            }

            var isValueFieldExists = metadata.Attributes.TryGetValue("Value", out var value);
            if (!isValueFieldExists)
            {
                // TODO: feedback
                throw new InvalidOperationException($"Error occured during attribute reading from pdf document. SQL field not exists in table {Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY}.");
            }

            try
            {
                // TODO: refactor this:
                if (value != null && typeof(T) == typeof(DateTime))
                {
                    return (T)(object)DateTime.Parse(value.ToString());
                }
                else if (value != null && typeof(T) == typeof(bool))
                {
                    return (T)(object)bool.Parse(value.ToString());
                }
                else if (value != null && typeof(T) == typeof(int))
                {
                    return (T)(object)int.Parse(value.ToString());
                }
                else if (value != null && typeof(T) == typeof(long))
                {
                    return (T)(object)long.Parse(value.ToString());
                }
                else if (value != null)
                {
                    return (T)value;
                }
            }
            catch (Exception)
            {
                return default;
            }

            return default;
        }

        public async Task SetAttribute<T>(string attribute, T value)
        {
            if (string.IsNullOrWhiteSpace(attribute))
            {
                throw new ArgumentException($"'{nameof(attribute)}' cannot be null or whitespace.", nameof(attribute));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!CheckNecessaryTablesAreExists(out _, out var documentMetadataTableName, out _))
            {
                return;
            }

            var oldValue = await GetAttribute<string>(attribute);
            if (string.IsNullOrWhiteSpace(oldValue))
            {
                var isMetadataDefinitionExists = await GetMetadataDefinitionIdByMetadataDefinitionName(attribute) != Guid.Empty;
                if (!isMetadataDefinitionExists)
                {
                    InsertMetadataDefinition(attribute);
                }

                var newMetadataDefinitionId = await GetMetadataDefinitionIdByMetadataDefinitionName(attribute);
                var isMetadataExists = await GetMetadataIdByDocumentIdAndMetadataDefinitionId(Id, newMetadataDefinitionId) != Guid.Empty;
                if (!isMetadataExists)
                {
                    InsertMetadata(newMetadataDefinitionId, value.ToString() ?? string.Empty);
                }
            }

            var metadataDefinitionId = await GetMetadataDefinitionIdByMetadataDefinitionName(attribute);

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@DocumentId", SqlDbType.UniqueIdentifier, Id)
                                .ConfigureParameter("@AttributeId", SqlDbType.UniqueIdentifier, metadataDefinitionId)
                                .ConfigureParameter("@AttributeValue", SqlDbType.VarChar, value.ToString(), SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            _ = _msSqlDataSource.PerformCommand(
                $"   UPDATE {documentMetadataTableName}"
                + $" SET Value = @AttributeValue"
                + $" WHERE DocumentId = @DocumentId"
                + $"   AND DocumentMetadataDefinitionId = @AttributeId",
                parameters);
        }

        private void InsertMetadata(Guid metadataDefinitionId, string value)
        {
            if (CheckNecessaryTablesAreExists(out _, out var documentMetadataTableName, out _))
            {
                var parameters = _dataParameterFactory
                        .ConfigureParameter("@Id", SqlDbType.UniqueIdentifier, Guid.NewGuid())
                        .ConfigureParameter("@DocumentId", SqlDbType.UniqueIdentifier, Id)
                        .ConfigureParameter("@DocumentMetadataDefinitionId", SqlDbType.UniqueIdentifier, metadataDefinitionId)
                        .ConfigureParameter("@AttributeValue", SqlDbType.VarChar, value, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                        .GetConfiguredParameters();

                _ = _msSqlDataSource.PerformCommand(
                $"   INSERT INTO {documentMetadataTableName} (Id, DocumentId, DocumentMetadataDefinitionId, Value)"
                + $" Values(@Id, @DocumentId, @DocumentMetadataDefinitionId, @AttributeValue)",
                parameters);
            }
        }
        #endregion

        #region Private members
        private bool CheckNecessaryTablesAreExists(out string documentTableName, out string documentMetadataTableName, out string documentMetadataDefinitionsTableName)
        {
            var isDocumentTableExists = _sqlTableNames.TryGetValue(Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY, out documentTableName);
            var isDocumentMetadataTableExists = _sqlTableNames.TryGetValue(Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY, out documentMetadataTableName);
            var isDocumentMetadataDefinitionsTableExists = _sqlTableNames.TryGetValue(Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY, out documentMetadataDefinitionsTableName);

            if (!isDocumentTableExists)
            {
                // TODO: feedback
                return false;
            }

            if (!isDocumentMetadataTableExists)
            {
                // TODO: feedback
                return false;
            }

            if (!isDocumentMetadataDefinitionsTableExists)
            {
                // TODO: feedback
                return false;
            }

            return true;
        }

        private async Task<Guid> GetMetadataDefinitionIdByMetadataDefinitionName(string metadataDefinitionName)
        {
            if (CheckNecessaryTablesAreExists(out _, out _, out var documentMetadataDefinitionsTableName))
            {
                var parameters = _dataParameterFactory
                                    .ConfigureParameter("@MetadataDefinitionName", SqlDbType.VarChar, metadataDefinitionName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                    .GetConfiguredParameters();

                var sqlScript = $" SELECT Id FROM {documentMetadataDefinitionsTableName}"
                    + $" WHERE ExtractedName = @MetadataDefinitionName";
                var documentMetadataResult = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters, "Id");

                var id = documentMetadataResult.ResponseObject.FirstOrDefault()?.Id.ToString();

                if (!string.IsNullOrWhiteSpace(id))
                {
                    return Guid.Parse(id);
                }
            }

            return default;
        }

        private void InsertMetadataDefinition(string name)
        {
            if (CheckNecessaryTablesAreExists(out _, out _, out var documentMetadataDefinitionsTableName))
            {
                var documentParameters = _dataParameterFactory
                        .ConfigureParameter("@Id", SqlDbType.UniqueIdentifier, Guid.NewGuid())
                        .ConfigureParameter("@ExtractedName", SqlDbType.VarChar, name, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                        .GetConfiguredParameters();

                _msSqlDataSource.PerformCommand(
                    $"   INSERT INTO {documentMetadataDefinitionsTableName}"
                    + $" (Id, ExtractedName)"
                    + $" VALUES (@Id, @ExtractedName)",
                    documentParameters);
            }
        }

        private async Task<Guid> GetMetadataIdByDocumentIdAndMetadataDefinitionId(Guid documentId, Guid metadataDefinition)
        {
            if (CheckNecessaryTablesAreExists(out _, out var documentMetadataTableName, out _))
            {
                var parameters = _dataParameterFactory
                                    .ConfigureParameter("@DocumentId", SqlDbType.UniqueIdentifier, documentId)
                                    .ConfigureParameter("@DocumentMetadataDefinitionId", SqlDbType.UniqueIdentifier, metadataDefinition)
                                    .GetConfiguredParameters();

                var sqlScript = $" SELECT Id FROM {documentMetadataTableName}"
                    + $" WHERE DocumentId = @DocumentId"
                    + $" AND DocumentMetadataDefinitionId = @DocumentMetadataDefinitionId";
                var documentMetadataResult = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters);

                var id = documentMetadataResult.ResponseObject.FirstOrDefault()?.Id.ToString();

                if (!string.IsNullOrWhiteSpace(id))
                {
                    return Guid.Parse(id);
                }
            }

            return default;
        }
        #endregion
    }
}
