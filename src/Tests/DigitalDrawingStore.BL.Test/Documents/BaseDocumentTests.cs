using Moq;
using Unity;
using XperiCad.Common.Core.Core;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark;
using XperiCad.DigitalDrawingStore.BL.Test.Documents.Factories;

namespace XperiCad.DigitalDrawingStore.BL.Test.Documents
{
    public abstract class BaseDocumentTests
    {
        #region Properties
        protected static IDataSource MsSqlDataSource => GetMsSqlDataSource();
        protected static IDataParameterFactory DataParameterFactory => GetDataParameterFactory();
        protected static IDictionary<string, string> SqlTableNames => GetSqlTableNames(CreateTestNamespace());
        #endregion

        #region Protected members
        protected string PrepareTestEnvironment(IEnumerable<IDocument> documents)
        {
            var testNamespace = CreateTestNamespace();
            PrepareTestEnvironment(testNamespace, ConvertToCategorizedDocuments(testNamespace, documents));

            return testNamespace;
        }

        protected string PrepareTestEnvironment(IEnumerable<CategorizedDocument> documents)
        {
            var testNamespace = CreateTestNamespace();
            PrepareTestEnvironment(testNamespace, documents);

            return testNamespace;
        }

        protected void PrepareTestEnvironment(string testNamespace, IEnumerable<IDocument> documents)
        {
            PrepareTestEnvironment(testNamespace, ConvertToCategorizedDocuments(testNamespace, documents));
        }

        protected void PrepareTestEnvironment(string testNamespace, IEnumerable<CategoryDTO> categories, IEnumerable<CategorizedDocument> documents)
        {
            CreateTestDatabaseTables(testNamespace, MsSqlDataSource);
            var task = new Task(async () =>
            {
                await FillTestDatabase(testNamespace, MsSqlDataSource, DataParameterFactory, new List<object[]>(), categories, documents, new List<object[]>());
            });

            task.RunSynchronously();
        }

        protected void PrepareTestEnvironment(string testNamespace, IEnumerable<CategorizedDocument> documents)
        {
            CreateTestDatabaseTables(testNamespace, MsSqlDataSource);

            var task = new Task(async () =>
            {
                await FillTestDatabase(testNamespace, MsSqlDataSource, DataParameterFactory, new List<object[]>(), new List<CategoryDTO>(), documents, new List<object[]>());
            });

            task.RunSynchronously();
        }

        protected void PrepareTestEnvironment(string testNamespace, IEnumerable<object[]> documentMetadataDefinitions, IEnumerable<IDocument> documents, IEnumerable<object[]> documentMetadata)
        {
            PrepareTestEnvironment(testNamespace, documentMetadataDefinitions, ConvertToCategorizedDocuments(testNamespace, documents), documentMetadata);
        }

        protected void PrepareTestEnvironment(string testNamespace, IEnumerable<object[]> documentMetadataDefinitions, IEnumerable<CategorizedDocument> documents, IEnumerable<object[]> documentMetadata)
        {
            CreateTestDatabaseTables(testNamespace, MsSqlDataSource);

                var task = new Task(async () =>
                {
                    await FillTestDatabase(testNamespace, MsSqlDataSource, DataParameterFactory, documentMetadataDefinitions, new List<CategoryDTO>(), documents, documentMetadata);
                });

            task.RunSynchronously();
        }

        protected void DisposeTestEnvironment(string testNamespace)
        {
            MsSqlDataSource.PerformCommand(""
                            + $"DROP TABLE DocumentCategoryEntities{testNamespace};"
                            + $"DROP TABLE DocumentMetadata{testNamespace};"
                            + $"DROP TABLE DocumentMetadataDefinitions{testNamespace};"
                            + $"DROP TABLE Documents{testNamespace};"
                            + $"DROP TABLE DocumentCategories{testNamespace};");
        }

        protected static TestDocumentFactory CreateDocumentFactory(string testNamespace)
        {
            return new TestDocumentFactory()
            {
                DocumentFactory = new DocumentFactory(MsSqlDataSource, DataParameterFactory, GetSqlTableNames(testNamespace), new List<IDocumentExporter>() { new PdfDocumentExporter() }, Mock.Of<IDocumentWatermarkProvider>())
            };
        }

        protected static TestDocumentFactory CreateDocumentFactory()
        {
            return new TestDocumentFactory()
            {
                DocumentFactory = new DocumentFactory(MsSqlDataSource, DataParameterFactory, SqlTableNames, new List<IDocumentExporter>() { new PdfDocumentExporter() }, Mock.Of<IDocumentWatermarkProvider>())
            };
        }

        protected static string CreateTestNamespace()
        {
            var id = Guid.NewGuid().ToString();
            var testNamespace = id.Replace("-", "");

            return testNamespace;
        }

        protected static IDocumentCategory CreateDocumentCategory(string testNamespace, Guid id, string displayName)
        {
            return new MsSqlDocumentCategory(id, MsSqlDataSource, DataParameterFactory, GetSqlTableNames(testNamespace));
        }
        #endregion

        #region Private members
        private void CreateTestDatabaseTables(string testNamespace, IDataSource msSqlDataSource)
        {
            msSqlDataSource.PerformCommand(
                $" CREATE TABLE DocumentCategories{testNamespace}"
                + $" ("
                + $"  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),"
                + $"  IsDesigned BIT DEFAULT 0,"
                + $"  DisplayName VARCHAR(MAX)"
                + $");"
                + $"CREATE TABLE Documents{testNamespace}"
                + $"("
                + $"  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),"
                + $"  DocumentCategoryId UNIQUEIDENTIFIER,"
                + $"  Path VARCHAR(MAX),"
                + $"  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategories{testNamespace} (Id)"
                + $");"
                + $"CREATE TABLE DocumentMetadataDefinitions{testNamespace}"
                + $"("
                + $"  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),"
                + $"  ExtractedName VARCHAR(MAX)"
                + $");"
                + $"CREATE TABLE DocumentCategoryEntities{testNamespace}"
                + $"("
                + $"  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),"
                + $"  DocumentCategoryId UNIQUEIDENTIFIER,"
                + $"  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,"
                + $"  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategories{testNamespace} (Id),"
                + $"  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitions{testNamespace} (Id)"
                + $");"
                + $"CREATE TABLE DocumentMetadata{testNamespace}"
                + $"("
                + $"  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),"
                + $"  DocumentId UNIQUEIDENTIFIER,"
                + $"  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,"
                + $"  Value NVARCHAR(MAX),"
                + $"  FOREIGN KEY (DocumentId) REFERENCES Documents{testNamespace} (Id),"
                + $"  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitions{testNamespace} (Id)"
                + $");"
            );
        }

        private async Task FillTestDatabase(
            string testNamespace,
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IEnumerable<object[]> documentMetadataDefinitions,
            IEnumerable<CategoryDTO> documentCategories,
            IEnumerable<CategorizedDocument> categorizedDocuments,
            IEnumerable<object[]> documentMetadata)
        {
            if (documentCategories.Count() == 0)
            {
                var categories = categorizedDocuments.GroupBy(d => d.DocumentCategory.Id);
                foreach (var category in categories)
                {
                    var categoryName = default(string);
                    if (!string.IsNullOrWhiteSpace(await category.FirstOrDefault()?.DocumentCategory.GetDisplayNameAsync()))
                    {
                        categoryName = await category.FirstOrDefault()?.DocumentCategory.GetDisplayNameAsync();
                    }

                    if (!string.IsNullOrWhiteSpace(category.FirstOrDefault()?.DocumentCategoryName))
                    {
                        categoryName = category.FirstOrDefault()?.DocumentCategoryName;
                    }

                    if (string.IsNullOrWhiteSpace(categoryName))
                    {
                        categoryName = "TestCategory";
                    }

                    var documentCategoryParameters = dataParameterFactory
                        .ConfigureParameter("@Id", System.Data.SqlDbType.UniqueIdentifier, category.Key)
                        .ConfigureParameter("@IsDesigned", System.Data.SqlDbType.Bit, 1)
                        .ConfigureParameter("@DisplayName", System.Data.SqlDbType.VarChar, categoryName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                        .GetConfiguredParameters();

                    msSqlDataSource.PerformCommand(
                        $"   INSERT INTO DocumentCategories{testNamespace}"
                        + $" (Id, IsDesigned, DisplayName)"
                        + $" VALUES (@Id, @IsDesigned, @DisplayName)",
                        documentCategoryParameters);
                }
            }
            else
            {
                foreach (var documentCategory in documentCategories)
                {
                    var documentCategoryParameters = dataParameterFactory
                        .ConfigureParameter("@Id", System.Data.SqlDbType.UniqueIdentifier, documentCategory.CategoryId)
                        .ConfigureParameter("@IsDesigned", System.Data.SqlDbType.Bit, 1)
                        .ConfigureParameter("@DisplayName", System.Data.SqlDbType.VarChar, documentCategory.DisplayName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                        .GetConfiguredParameters();

                    msSqlDataSource.PerformCommand(
                        $"   INSERT INTO DocumentCategories{testNamespace}"
                        + $" (Id, IsDesigned, DisplayName)"
                        + $" VALUES (@Id, @IsDesigned, @DisplayName)",
                        documentCategoryParameters);
                }
            }


            foreach (var categorizedDocument in categorizedDocuments)
            {
                var documentParameters = dataParameterFactory
                    .ConfigureParameter("@Id", System.Data.SqlDbType.UniqueIdentifier, categorizedDocument.Document.Id)
                    .ConfigureParameter("@DocumentCategoryId", System.Data.SqlDbType.UniqueIdentifier, categorizedDocument.DocumentCategory.Id)
                    .ConfigureParameter("@Path", System.Data.SqlDbType.VarChar, categorizedDocument.Document.Path, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                    .GetConfiguredParameters();

                msSqlDataSource.PerformCommand(
                    $"   INSERT INTO Documents{testNamespace}"
                    + $" (Id, DocumentCategoryId, Path)"
                    + $" VALUES (@Id, @DocumentCategoryId, @Path)",
                    documentParameters);
            }

            foreach (var metadataDefinition in documentMetadataDefinitions)
            {
                var documentParameters = dataParameterFactory
                    .ConfigureParameter("@Id", System.Data.SqlDbType.UniqueIdentifier, metadataDefinition[0])
                    .ConfigureParameter("@ExtractedName", System.Data.SqlDbType.VarChar, metadataDefinition[1], SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                    .GetConfiguredParameters();

                msSqlDataSource.PerformCommand(
                    $"   INSERT INTO DocumentMetadataDefinitions{testNamespace}"
                    + $" (Id, ExtractedName)"
                    + $" VALUES (@Id, @ExtractedName)",
                    documentParameters);
            }

            foreach (var metadata in documentMetadata)
            {
                var documentParameters = dataParameterFactory
                    .ConfigureParameter("@Id", System.Data.SqlDbType.UniqueIdentifier, metadata[0])
                    .ConfigureParameter("@DocumentMetadataDefinitionId", System.Data.SqlDbType.UniqueIdentifier, metadata[1])
                    .ConfigureParameter("@DocumentId", System.Data.SqlDbType.UniqueIdentifier, metadata[2])
                    .ConfigureParameter("@Value", System.Data.SqlDbType.VarChar, metadata[3], SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                    .GetConfiguredParameters();

                msSqlDataSource.PerformCommand(
                    $"   INSERT INTO DocumentMetadata{testNamespace}"
                    + $" (Id, DocumentId, DocumentMetadataDefinitionId, Value)"
                    + $" VALUES (@Id, @DocumentId, @DocumentMetadataDefinitionId, @Value)",
                    documentParameters);
            }
        }

        private static IDataSource GetMsSqlDataSource()
        {
            var container = CommonCoreBootstrapper.ConfigureCommonCore(new string[] { Common.Core.Constants.Culture.CultureKeys.HUNGARIAN_HUNGARY }, Constants.TEST_NAMESPACE, true);

            return container
                .Resolve<IDataSourceFactory>()
                .CreateMsSqlDataSource(Constants.TEST_DATABASE_CONNECTION_STRING);
        }

        private static IDataParameterFactory GetDataParameterFactory()
        {
            var container = CommonCoreBootstrapper.ConfigureCommonCore(new string[] { Common.Core.Constants.Culture.CultureKeys.HUNGARIAN_HUNGARY }, Constants.TEST_NAMESPACE, true);

            return container.Resolve<IDataParameterFactory>();
        }

        private IEnumerable<CategorizedDocument> ConvertToCategorizedDocuments(string testNamespace, IEnumerable<IDocument> documents)
        {
            var documentCategoryId = Guid.NewGuid();
            var categorizedDocuments = new List<CategorizedDocument>();

            foreach (var document in documents)
            {
                categorizedDocuments.Add(new CategorizedDocument(new MsSqlDocumentCategory(documentCategoryId, MsSqlDataSource, DataParameterFactory, GetSqlTableNames(testNamespace)), document));
            }

            return categorizedDocuments;
        }

        protected static IDictionary<string, string> GetSqlTableNames(string testNamespace)
        {
            var sqlTableNames = new Dictionary<string, string>()
            {
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY, $"DocumentCategories{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY, $"DocumentCategoryEntities{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY, $"DocumentMetadataDefinitions{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY, $"DocumentMetadata{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY, $"Documents{testNamespace}" },
            };

            return sqlTableNames;
        }
        #endregion
    }
}
