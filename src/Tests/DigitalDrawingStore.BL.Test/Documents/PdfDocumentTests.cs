using Moq;
using Unity;
using XperiCad.Common.Core.Core;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Test.Documents
{
    public class PdfDocumentTests : BaseDocumentTests
    {
        #region Fields
        private IDataSource _msSqlDataSource;
        private IDataParameterFactory _dataParameterFactory;
        #endregion

        #region Constructors
        public PdfDocumentTests()
        {
            var container = CommonCoreBootstrapper.ConfigureCommonCore(new string[] { Common.Core.Constants.Culture.CultureKeys.HUNGARIAN_HUNGARY }, Constants.TEST_NAMESPACE, true);

            _msSqlDataSource = container
                                   .Resolve<IDataSourceFactory>()
                                    .CreateMsSqlDataSource(Constants.TEST_DATABASE_CONNECTION_STRING);
            _dataParameterFactory = container.Resolve<IDataParameterFactory>();
        }
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(GetTestParametersForPD0011))]
        public void PD0011_Given_NullOrEmptyValueAsArgument_When_CreatingNewPdfDocument_Then_ThrowsArgumentNullException(
            IDataSource dataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> tableNames,
            Guid id,
            string path,
            IDocumentExporter documentExporter,
            IDocumentWatermarkProvider documentWatermarkProvider,
            Type expectedExceptionType,
            string expedctedExceptionMessage)
        {
            var exception = Record.Exception(() => new MsSqlPdfDocument(dataSource, dataParameterFactory, tableNames, id, path, documentExporter, documentWatermarkProvider));

            Assert.IsType(expectedExceptionType, exception);
            Assert.Equal(expedctedExceptionMessage, exception.Message);
        }

        [Theory]
        [InlineData(@"C:\Test\Path\542486.pdf", "542486", ".pdf", @"C:\Test\Path")]
        [InlineData(@"D:\Somewhere\In\The\Universe.PDF", "Universe", ".PDF", @"D:\Somewhere\In\The")]
        [InlineData(@"\\This\Is\a\network\path\passwords.PDF", "passwords", ".PDF", @"\\This\Is\a\network\path")]
        public void PD0021_Given_ValidDocumentPaths_When_GetDocumentProperties_Then_ReturnWithCorrectProperties(
            string path,
            string expectedName,
            string expectedExtension,
            string expectedDirectory)
        {
            var doc = CreatePdfDocument(Guid.NewGuid(), path);

            var filePath = doc.Path;
            var fileName = doc.Name;
            var fileExtension = doc.Extension;
            var fileDirectory = doc.Directory;

            Assert.Equal(path, filePath);
            Assert.Equal(expectedName, fileName);
            Assert.Equal(expectedExtension, fileExtension);
            Assert.Equal(expectedDirectory, fileDirectory);
        }

        [Theory]
        [MemberData(nameof(GetTestParametersForPD0031))]
        public void PD0031_Given_NullOrEmptyAsArgument_When_Export_Then_ThrowsArgumentNullException(
            string targetPath,
            IDocumentWatermark watermark,
            Type expectedExceptionType,
            string expectedExceptionMessage)
        {
            var document = CreatePdfDocument(Guid.NewGuid(), @".\Just\A\Document.pdf");

            var exception = Record.Exception(() => document.ExportAs(targetPath, watermark));

            Assert.IsType(expectedExceptionType, exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory(Skip = "Export method is not yet implemented. Will implemented in story 736.")]
        [InlineData(@".\Resources\Documents\ReferenceDocuments\SampleDocument-HelloWorld.pdf", @"XperiCad\Tests\SampleDocument-HelloWorld.pdf")]
        public void PD0041_Given_PdfDocument_When_Export_Then_DocumentExportedSuccessfully(string path, string targetRelativePath)
        {
            var document = CreatePdfDocument(Guid.NewGuid(), path);
            var watermark = Mock.Of<IDocumentWatermark>();
            var targetPath = $"{Path.GetTempPath()}{targetRelativePath}";

            try
            {
                document.ExportAs(targetPath, watermark);
                var isFileExists = File.Exists(targetPath);

                Assert.True(isFileExists);
            }
            finally
            {
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }
        }

        [Theory(Skip = "Export method is not yet implemented. Will implemented in story 736.")]
        [InlineData(@".\Resources\Documents\ReferenceDocuments\SampleDocument-HelloWorld.pdf", @"XperiCad\Tests\SampleDocument-HelloWorld.pdf")]
        public void PD0051_Given_PdfDocument_When_Export_Then_ExportedContentIsCorrect(string path, string targetRelativePath)
        {
            var document = CreatePdfDocument(Guid.NewGuid(), path);
            var watermark = Mock.Of<IDocumentWatermark>();
            var targetPath = $"{Path.GetTempPath()}{targetRelativePath}";

            document.ExportAs(targetPath, watermark);

            // TODO: assert for content
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("\t")]
        public void PD0061_Given_NullOrEmptyAsParameter_When_GetAttribute_Then_ThrowsArgumentNullException(string attribute)
        {
            var document = CreatePdfDocument(Guid.NewGuid(), @".\TestDocument\doc.pdf");

            var exception = Record.ExceptionAsync(async () => await document.GetAttribute<string>(attribute));

            Assert.NotNull(exception.Result);
            Assert.IsType<ArgumentException>(exception.Result);
            Assert.Equal("'attribute' cannot be null or whitespace. (Parameter 'attribute')", exception.Result.Message);
        }

        [Theory]
        [MemberData(nameof(GetTestParametersForPD0071))]
        public async Task PD0071_Given_PdfDocument_When_GetMissingAttribute_Then_ReturnsNull(
            string testNamespace,
            IEnumerable<IDocument> documents,
            IDocument targetDocument,
            string attribute)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, documents);

                var result = await targetDocument.GetAttribute<string>(attribute);

                Assert.Null(result);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(testNamespace))
                {
                    DisposeTestEnvironment(testNamespace);
                }
            }
        }

        // TODO: extend this test to get DateTime, bool, string and int as well
        [Theory]
        [MemberData(nameof(GetTestParametersForPD0081))]
        public void PD0081_Given_PdfDocument_When_GetExistingAttribute_Then_ReturnsWithValue<T>(
            string testNamespace,
            IEnumerable<object[]> documentMetadataDefinitions,
            IEnumerable<IDocument> documents,
            IEnumerable<object[]> documentMetadata,
            IDocument targetDocument,
            string attribute,
            T expectedResult)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, documentMetadataDefinitions, documents, documentMetadata);

                var result = targetDocument.GetAttribute<T>(attribute).Result;

                Assert.NotNull(result);
                Assert.Equal(expectedResult, result);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(testNamespace))
                {
                    DisposeTestEnvironment(testNamespace);
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestParametersForPD0091))]
        public void PD0091_Given_NullOrEmptyAsParameter_When_SetAttribute_Then_ThrowsArgumentNullException(
            string attribute,
            object value,
            Type expectedExceptionType,
            string expectedExceptionMessage)
        {
            var document = CreatePdfDocument(Guid.NewGuid(), @".\TestDocument\doc.pdf");

            var exception = Record.ExceptionAsync(async () => await document.SetAttribute(attribute, value));

            Assert.NotNull(exception.Result);
            Assert.IsType(expectedExceptionType, exception.Result);
            Assert.Equal(expectedExceptionMessage, exception.Result.Message);
        }

        [Theory]
        [MemberData(nameof(GetTestParametersForPD0101))]
        public async Task PD0101_Given_PdfDocumentWithMissingOrExistingAttribute_When_SetAttribute_Then_CreatesNewOneIfNecessaryAndSetsTheAttributeValue<T>(
            string testNamespace,
            IEnumerable<object[]> documentMetadataDefinitions,
            IEnumerable<IDocument> documents,
            IEnumerable<object[]> documentMetadata,
            IDocument targetDocument,
            string attributeName,
            T oldAttributeValue,
            T newAttributeValue)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, documentMetadataDefinitions, documents, documentMetadata);

                var oldResult = await targetDocument.GetAttribute<T>(attributeName);
                
                await targetDocument.SetAttribute(attributeName, newAttributeValue);
                
                var newResult = await targetDocument.GetAttribute<T>(attributeName);

                Assert.Equal(oldAttributeValue, oldResult);
                Assert.Equal(newAttributeValue, newResult);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(testNamespace))
                {
                    DisposeTestEnvironment(testNamespace);
                }
            }
        }
        #endregion

        #region Test parameters
        public static IEnumerable<object[]> GetTestParametersForPD0011()
        {
            yield return new object[] { null, Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Guid.NewGuid(), @".\PathToDocument\doc.pdf", Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'msSqlDataSource')" };
            yield return new object[] { Mock.Of<IDataSource>(), null, Mock.Of<IDictionary<string, string>>(), Guid.NewGuid(), @".\PathToDocument\doc.pdf", Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'dataParameterFactory')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), null, Guid.NewGuid(), @".\PathToDocument\doc.pdf", Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'sqlTableNames')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Guid.Empty, @".\PathToDocument\doc.pdf", Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentException), "Parameter id could not be empty." };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Guid.NewGuid(), null, Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentException), "'path' cannot be null or whitespace. (Parameter 'path')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Guid.NewGuid(), "", Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentException), "'path' cannot be null or whitespace. (Parameter 'path')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Guid.NewGuid(), " ", Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentException), "'path' cannot be null or whitespace. (Parameter 'path')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Guid.NewGuid(), "  ", Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentException), "'path' cannot be null or whitespace. (Parameter 'path')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Guid.NewGuid(), "\t", Mock.Of<IDocumentExporter>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentException), "'path' cannot be null or whitespace. (Parameter 'path')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Guid.NewGuid(), @".\PathToDocument\doc.pdf", null, Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'documentExporter')" };
        }

        public static IEnumerable<object[]> GetTestParametersForPD0031()
        {
            yield return new object[] { null, Mock.Of<IDocumentWatermark>(), typeof(ArgumentException), "'targetPath' cannot be null or whitespace. (Parameter 'targetPath')" };
            yield return new object[] { "", Mock.Of<IDocumentWatermark>(), typeof(ArgumentException), "'targetPath' cannot be null or whitespace. (Parameter 'targetPath')" };
            yield return new object[] { " ", Mock.Of<IDocumentWatermark>(), typeof(ArgumentException), "'targetPath' cannot be null or whitespace. (Parameter 'targetPath')" };
            yield return new object[] { "  ", Mock.Of<IDocumentWatermark>(), typeof(ArgumentException), "'targetPath' cannot be null or whitespace. (Parameter 'targetPath')" };
            yield return new object[] { "\t", Mock.Of<IDocumentWatermark>(), typeof(ArgumentException), "'targetPath' cannot be null or whitespace. (Parameter 'targetPath')" };
            yield return new object[] { "\t\t", Mock.Of<IDocumentWatermark>(), typeof(ArgumentException), "'targetPath' cannot be null or whitespace. (Parameter 'targetPath')" };
            yield return new object[] { @".\TargetPath\hello.pdf", null, typeof(ArgumentNullException), "Value cannot be null. (Parameter 'documentWatermark')" };
        }

        public static IEnumerable<object[]> GetTestParametersForPD0071()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;
            var targetDocument = documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf");

            var documents = new List<IDocument>()
            {
                targetDocument,
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf"),
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf"),
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Temp\doc.pdf"),
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @"..\doc.pdf")
            };

            yield return new object[] { testNamespace, documents, targetDocument, "MissingAttribute" };
            yield return new object[] { testNamespace, documents, targetDocument, "PartNumber" };
            yield return new object[] { testNamespace, documents, targetDocument, "Standard" };
        }

        public static IEnumerable<object[]> GetTestParametersForPD0081()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;

            var standardMetadataDefinitionId = Guid.NewGuid();
            var typoMetadataDefinitionId = Guid.NewGuid();
            var partNumberMetadataDefinitionId = Guid.NewGuid();
            var documentNumberMetadataDefinitionId = Guid.NewGuid();
            var createdAtMetadataDefinitionId = Guid.NewGuid();
            var isItFromGoogleMetadataDefinitionId = Guid.NewGuid();
            var integerMetadataDefinitionId = Guid.NewGuid();
            var longMetadataDefinitionId = Guid.NewGuid();
            var documentMetadataDefinitions = new List<object[]>()
            {
                new object[] { standardMetadataDefinitionId, "Standard" },
                new object[] { typoMetadataDefinitionId, "TypoMetadata" },
                new object[] { partNumberMetadataDefinitionId, "PartNumber" },
                new object[] { documentNumberMetadataDefinitionId, "DocumentNumber" },
                new object[] { createdAtMetadataDefinitionId, "CreatedAt" },
                new object[] { isItFromGoogleMetadataDefinitionId, "IsFromGoogle" },
                new object[] { integerMetadataDefinitionId, "Integer" },
                new object[] { longMetadataDefinitionId, "Long" },
                new object[] { Guid.NewGuid(), "Freetext" },
            };

            var testDoc1Id = Guid.NewGuid();
            var testDoc1 = documentFactory.CreatePdfDocument(testDoc1Id, @".\Document.pdf");
            var testDoc2Id = Guid.NewGuid();
            var testDoc2 = documentFactory.CreatePdfDocument(testDoc2Id, @"C:\Hello\World.pdf");
            var documents = new List<IDocument>()
            {
                testDoc2,
                testDoc1,
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf"),
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Temp\doc.pdf"),
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @"..\doc.pdf")
            };

            var documentMetadata = new List<object[]>()
            {
                new object[] { Guid.NewGuid(), standardMetadataDefinitionId, testDoc1Id, "MSZ-2250" },
                new object[] { Guid.NewGuid(), documentNumberMetadataDefinitionId, testDoc1Id, "DN-ABCD" },
                new object[] { Guid.NewGuid(), createdAtMetadataDefinitionId, testDoc1Id, new DateTime(2022, 12, 10) },
                new object[] { Guid.NewGuid(), standardMetadataDefinitionId, testDoc2Id, "ISO27001" },
                new object[] { Guid.NewGuid(), integerMetadataDefinitionId, testDoc2Id, 123 },
                new object[] { Guid.NewGuid(), longMetadataDefinitionId, testDoc2Id, 256L },
                new object[] { Guid.NewGuid(), typoMetadataDefinitionId, testDoc2Id, "ISO27001-2" },
                new object[] { Guid.NewGuid(), partNumberMetadataDefinitionId, testDoc2Id, "PN-1150" },
                new object[] { Guid.NewGuid(), isItFromGoogleMetadataDefinitionId, testDoc2Id, true },
            };

            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc1, "Standard", "MSZ-2250" };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc1, "DocumentNumber", "DN-ABCD" };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc1, "CreatedAt", new DateTime(2022, 12, 10) };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc2, "Standard", "ISO27001" };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc2, "TypoMetadata", "ISO27001-2" };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc2, "PartNumber", "PN-1150" };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc2, "IsFromGoogle", true };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc2, "Integer", 123 };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDoc2, "Long", 256L };
        }

        public static IEnumerable<object[]> GetTestParametersForPD0091()
        {
            yield return new object[] { null, "TestValue", typeof(ArgumentException), "'attribute' cannot be null or whitespace. (Parameter 'attribute')" };
            yield return new object[] { "", "TestValue", typeof(ArgumentException), "'attribute' cannot be null or whitespace. (Parameter 'attribute')" };
            yield return new object[] { " ", "TestValue", typeof(ArgumentException), "'attribute' cannot be null or whitespace. (Parameter 'attribute')" };
            yield return new object[] { "  ", "TestValue", typeof(ArgumentException), "'attribute' cannot be null or whitespace. (Parameter 'attribute')" };
            yield return new object[] { "\t", "TestValue", typeof(ArgumentException), "'attribute' cannot be null or whitespace. (Parameter 'attribute')" };
            yield return new object[] { "\t\t", "TestValue", typeof(ArgumentException), "'attribute' cannot be null or whitespace. (Parameter 'attribute')" };
            yield return new object[] { @".\TargetPath\hello.pdf", null, typeof(ArgumentNullException), "Value cannot be null. (Parameter 'value')" };
        }

        public static IEnumerable<object[]> GetTestParametersForPD0101()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;

            var stringMetadataDefinitionId = Guid.NewGuid();
            var intMetadataDefinitionId = Guid.NewGuid();
            var longMetadataDefinitionId = Guid.NewGuid();
            var defaultTrueBoolMetadataDefinitionId = Guid.NewGuid();
            var defaultFalseBoolMetadataDefinitionId = Guid.NewGuid();
            var dateTimeMetadataDefinitionId = Guid.NewGuid();
            var documentMetadataDefinitions = new List<object[]>()
            {
                new object[] { stringMetadataDefinitionId, "StringMetadata" },
                new object[] { intMetadataDefinitionId, "IntMetadata" },
                new object[] { longMetadataDefinitionId, "LongMetadata" },
                new object[] { defaultTrueBoolMetadataDefinitionId, "BoolMetadataDefaultTrue" },
                new object[] { defaultFalseBoolMetadataDefinitionId, "BoolMetadataDefaultFalse" },
                new object[] { dateTimeMetadataDefinitionId, "DateTimeMetadata" },
            };

            var testDocumentId = Guid.NewGuid();
            var testDocument = documentFactory.CreatePdfDocument(testDocumentId, @".\Document.pdf");
            var documents = new List<IDocument>()
            {
                testDocument
            };

            var documentMetadata = new List<object[]>()
            {
                new object[] { Guid.NewGuid(), stringMetadataDefinitionId, testDocumentId, "StringMetadataValue" },
                new object[] { Guid.NewGuid(), intMetadataDefinitionId, testDocumentId, 1 },
                new object[] { Guid.NewGuid(), longMetadataDefinitionId, testDocumentId, 1L },
                new object[] { Guid.NewGuid(), defaultTrueBoolMetadataDefinitionId, testDocumentId, true },
                new object[] { Guid.NewGuid(), defaultFalseBoolMetadataDefinitionId, testDocumentId, false },
                new object[] { Guid.NewGuid(), dateTimeMetadataDefinitionId, testDocumentId, new DateTime(2022, 12, 12) },
            };

            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "StringMetadata", "StringMetadataValue", "NewStringMetadataValue" };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "IntMetadata", 1, 2 };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "LongMetadata", 1L, 2L };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "BoolMetadataDefaultTrue", true, false };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "BoolMetadataDefaultFalse", false, true };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "DateTimeMetadata", new DateTime(2022, 12, 12), new DateTime(2022, 12, 13) };

            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "NewStringMetadata", null, "NewlyCreatedStringMetadataValue" };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "NewIntMetadata", 0, 3 };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "NewLongMetadata", 0L, 3L };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "NewBoolMetadataDefaultFalse", false, true };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "NewBoolMetadataDefaultFalse", false, false };
            yield return new object[] { testNamespace, documentMetadataDefinitions, documents, documentMetadata, testDocument, "NewDateTimeMetadata", default(DateTime), new DateTime(2022, 12, 14) };
        }
        #endregion

        #region Private members
        private IDocument CreatePdfDocument(Guid id, string path)
        {
            return CreatePdfDocument("", id, path);
        }

        private IDocument CreatePdfDocument(string testNamespace, Guid id, string path)
        {
            var sqlTableNames = new Dictionary<string, string>()
            {
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY, $"Documents{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY, $"DocumentMetadata{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY, $"DocumentMetadataDefinitions{testNamespace}" },
            };

            return new MsSqlPdfDocument(
                _msSqlDataSource,
                _dataParameterFactory,
                sqlTableNames,
                id,
                path,
                Mock.Of<IDocumentExporter>(),
                Mock.Of<IDocumentWatermarkProvider>());
        }
        #endregion
    }
}
