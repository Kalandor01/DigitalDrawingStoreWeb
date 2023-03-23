using Moq;
using XperiCad.DigitalDrawingStore.BL.Documents.Queries;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;
using XperiCad.DigitalDrawingStore.BL.Test.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Test.Services
{
    public class DocumentServiceTests : BaseDocumentTests
    {
        #region Tests
        [Theory]
        [MemberData(nameof(DS0011_GetTestParameters))]
        public void DS0011_Given_NullOrEmptyAsArguments_When_CreateDocumentService_Then_ThrowsArgumentNullException(
            IDocumentCategoryQuery documentCategoryQuery,
            IDocumentQuery documentQuery,
            Type expectedExceptionType,
            string expectedExceptionMessage)
        {
            var exception = Record.Exception(() => new DocumentService(documentCategoryQuery, documentQuery));

            Assert.IsType(expectedExceptionType, exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(DS0021_GetTestParameters))]
        public async void DS0021_Given_DocumentCategoriesInDatabase_When_QueryDocumentCategories_Then_ReturnsWithAllDocumentCategories(
            string testNamespace,
            IEnumerable<CategorizedDocument> testDocuments)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, testDocuments);
                var documentService = CreateDocumentService(testNamespace);
                var documentCategoriesCount = testDocuments.GroupBy(d => d.DocumentCategory.Id).Count();

                var result = (await documentService.QueryDocumentCategoriesAsync()).ResponseObject;

                Assert.NotNull(result);
                Assert.Equal(documentCategoriesCount, result.Count());
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
        [MemberData(nameof(DS0031_GetTestParameters))]
        public async void DS0031_Given_TestDocumentsInDatabase_When_QueryAllDocuments_Then_ReturnsWithAllDocuments(
            string testNamespace,
            IEnumerable<CategorizedDocument> testDocuments)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, testDocuments);
                var documentService = CreateDocumentService(testNamespace);

                var result = (await documentService.QueryAllDocumentsAsync()).ResponseObject;

                Assert.NotNull(result);
                Assert.Equal(testDocuments.Count(), result.Count());
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
        [MemberData(nameof(DS0041_GetTestParameters))]
        public async void DS0041_Given_SearchText_When_QueryDocuments_Then_ReturnsWithFilteredDocuments(
            string testNamespace,
            string searchText,
            IEnumerable<object[]> documentMetadataDefinitions,
            IEnumerable<CategorizedDocument> testDocuments,
            IEnumerable<object[]> documentMetadata,
            int expectedDocumentCount)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, documentMetadataDefinitions, testDocuments, documentMetadata);
                var documentService = CreateDocumentService(testNamespace);

                var result = (await documentService.QueryDocumentsAsync(searchText)).ResponseObject;

                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());
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
        [MemberData(nameof(DS0051_GetTestParameters))]
        public async void DS0051_Given_CategoryWithoutSearchText_When_QueryDocuments_Then_ReturnsWithFilteredDocuments(
            string testNamespace,
            Guid documentCategory,
            IEnumerable<CategorizedDocument> testDocuments,
            int expectedDocumentCount)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, testDocuments);
                var documentService = CreateDocumentService(testNamespace);

                var result = (await documentService.QueryDocumentsAsync(documentCategory, null)).ResponseObject;
                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());

                result = (await documentService.QueryDocumentsAsync(documentCategory, "")).ResponseObject;
                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());

                result = (await documentService.QueryDocumentsAsync(documentCategory, " ")).ResponseObject;
                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());

                result = (await documentService.QueryDocumentsAsync(documentCategory, "  ")).ResponseObject;
                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());

                result = (await documentService.QueryDocumentsAsync(documentCategory, "\t")).ResponseObject;
                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());

                result = (await documentService.QueryDocumentsAsync(documentCategory, "*")).ResponseObject;
                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());
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
        [MemberData(nameof(DS0061_GetTestParameters))]
        public async void DS0061_Given_SearchTextWithoutCategory_When_QueryDocuments_Then_ReturnsWithFilteredDocuments(
            string testNamespace,
            string searchText,
            IEnumerable<CategorizedDocument> testDocuments,
            int expectedDocumentCount)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, testDocuments);
                var documentService = CreateDocumentService(testNamespace);

                var result = (await documentService.QueryDocumentsAsync(Guid.Empty, searchText)).ResponseObject;

                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());
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
        [MemberData(nameof(DS0071_GetTestParameters))]
        public async void DS0071_Given_SearchTextWithCategory_When_QueryDocuments_Then_ReturnsWithFilteredDocuments(
            string testNamespace,
            string searchText,
            Guid documentCategory,
            IEnumerable<CategorizedDocument> testDocuments,
            int expectedDocumentCount)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, testDocuments);
                var documentService = CreateDocumentService(testNamespace);

                var result = (await documentService.QueryDocumentsAsync(documentCategory, searchText)).ResponseObject;

                Assert.NotNull(result);
                Assert.Equal(expectedDocumentCount, result.Count());
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
        public static IEnumerable<object[]> DS0011_GetTestParameters()
        {
            yield return new object[] { null, Mock.Of<IDocumentQuery>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'documentCategoryQuery')" };
            yield return new object[] { Mock.Of<IDocumentCategoryQuery>(), null, typeof(ArgumentNullException), "Value cannot be null. (Parameter 'documentQuery')" };
        }

        public static IEnumerable<object[]> DS0021_GetTestParameters()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;
            var testCategoryId = Guid.NewGuid();
            var testCategoryName = "TestCategory3";

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(Guid.NewGuid(), "TestCategory1", documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory2", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf")),
                new CategorizedDocument(testCategoryId, testCategoryName, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Temp\doc.pdf")),
                new CategorizedDocument(testCategoryId, testCategoryName, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"..\doc.pdf"))
            };

            yield return new object[] { testNamespace, documents };
            yield return new object[] { testNamespace, documents };
            yield return new object[] { testNamespace, documents };
        }

        public static IEnumerable<object[]> DS0031_GetTestParameters()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;
            var testCategoryId = Guid.NewGuid();
            var testCategoryName = "TestCategory3";

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(Guid.NewGuid(), "TestCategory1", documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory2", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf")),
                new CategorizedDocument(testCategoryId, testCategoryName, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Temp\doc.pdf")),
                new CategorizedDocument(testCategoryId, testCategoryName, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"..\doc.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory4", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory5", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"E:\Test\Subfolder\Document.pdf")),
            };

            yield return new object[] { testNamespace, documents };
            yield return new object[] { testNamespace, documents };
            yield return new object[] { testNamespace, documents };
        }

        public static IEnumerable<object[]> DS0041_GetTestParameters()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;
            var testCategoryId = Guid.NewGuid();
            var testCategoryName = "TestCategory3";

            var partNumberMetadataDefinitionId = Guid.NewGuid();
            var documentMetadataDefinitions = new List<object[]>()
            {
                new object[] { partNumberMetadataDefinitionId, "PartNumber" },
            };

            var targetDocumentId = Guid.NewGuid();
            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(Guid.NewGuid(), "TestCategory1", documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory2", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf")),
                new CategorizedDocument(testCategoryId, testCategoryName, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Temp\doc.pdf")),
                new CategorizedDocument(testCategoryId, testCategoryName, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"..\doc.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory4", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory5", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"E:\Test\Subfolder\Document.xcf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory6", documentFactory.CreatePdfDocument(targetDocumentId, @"C:\PartNumbered\MyFile.pdf")),
            };

            var documentMetadata = new List<object[]>()
            {
                new object[] { Guid.NewGuid(), partNumberMetadataDefinitionId, targetDocumentId, "03.103.50.07.0" },
            };

            yield return new object[] { testNamespace, "Document.pdf", documentMetadataDefinitions, documents, documentMetadata, 2 };
            yield return new object[] { testNamespace, "Document", documentMetadataDefinitions, documents, documentMetadata, 3 };
            yield return new object[] { testNamespace, "Document.xcf", documentMetadataDefinitions, documents, documentMetadata, 1 };
            yield return new object[] { testNamespace, "Doc", documentMetadataDefinitions, documents, documentMetadata, 5 };
            yield return new object[] { testNamespace, "Doc.pDf", documentMetadataDefinitions, documents, documentMetadata, 2 };
            yield return new object[] { testNamespace, "*", documentMetadataDefinitions, documents, documentMetadata, 7 };
            yield return new object[] { testNamespace, "", documentMetadataDefinitions, documents, documentMetadata, 7 };
            yield return new object[] { testNamespace, " ", documentMetadataDefinitions, documents, documentMetadata, 7 };
            yield return new object[] { testNamespace, "  ", documentMetadataDefinitions, documents, documentMetadata, 7 };
            yield return new object[] { testNamespace, "\t", documentMetadataDefinitions, documents, documentMetadata, 7 };
            yield return new object[] { testNamespace, null, documentMetadataDefinitions, documents, documentMetadata, 7 };
            yield return new object[] { testNamespace, "03.103.50.07.0", documentMetadataDefinitions, documents, documentMetadata, 1 };
            yield return new object[] { testNamespace, "03103.5007.0", documentMetadataDefinitions, documents, documentMetadata, 1 };
            yield return new object[] { testNamespace, "0310350070", documentMetadataDefinitions, documents, documentMetadata, 1 };
        }

        public static IEnumerable<object[]> DS0051_GetTestParameters()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;
            var testCategoryId1 = Guid.NewGuid();
            var testCategoryName1 = "TestCategory3";

            var testCategoryId2 = Guid.NewGuid();
            var testCategoryName2 = "TestCategory3";

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf")),
                new CategorizedDocument(testCategoryId1, testCategoryName1, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Temp\doc.pdf")),
                new CategorizedDocument(testCategoryId1, testCategoryName1, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"..\doc.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"E:\Test\Subfolder\Document.xcf")),
            };

            yield return new object[] { testNamespace, testCategoryId1, documents, 2 };
            yield return new object[] { testNamespace, testCategoryId2, documents, 4 };
            yield return new object[] { testNamespace, Guid.Empty, documents, 6 };
        }

        public static IEnumerable<object[]> DS0061_GetTestParameters()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;
            var testCategoryId1 = Guid.NewGuid();
            var testCategoryName1 = "TestCategory3";

            var testCategoryId2 = Guid.NewGuid();
            var testCategoryName2 = "TestCategory3";

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf")),
                new CategorizedDocument(testCategoryId1, testCategoryName1, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Temp\doc.pdf")),
                new CategorizedDocument(testCategoryId1, testCategoryName1, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"..\doc.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"E:\Test\Subfolder\Document.xcf")),
            };

            yield return new object[] { testNamespace, "Document.pdf", documents, 2 };
            yield return new object[] { testNamespace, "Document", documents, 3 };
            yield return new object[] { testNamespace, "Document.xcf", documents, 1 };
            yield return new object[] { testNamespace, "Doc", documents, 5 };
            yield return new object[] { testNamespace, "Doc.pDf", documents, 2 };
            yield return new object[] { testNamespace, "*", documents, 6 };
            yield return new object[] { testNamespace, "", documents, 6 };
            yield return new object[] { testNamespace, " ", documents, 6 };
            yield return new object[] { testNamespace, "  ", documents, 6 };
            yield return new object[] { testNamespace, "\t", documents, 6 };
            yield return new object[] { testNamespace, null, documents, 6 };
        }

        public static IEnumerable<object[]> DS0071_GetTestParameters()
        {
            var testNamespace = CreateTestNamespace();
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;
            var testCategoryId1 = Guid.NewGuid();
            var testCategoryName1 = "TestCategory3";

            var testCategoryId2 = Guid.NewGuid();
            var testCategoryName2 = "TestCategory3";

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf")),
                new CategorizedDocument(testCategoryId1, testCategoryName1, documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Temp\doc.pdf")),
                new CategorizedDocument(testCategoryId1, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"..\doc.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName2, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf")),
                new CategorizedDocument(testCategoryId2, testCategoryName1, documentFactory.CreatePdfDocument(Guid.NewGuid(), @"E:\Test\Subfolder\Document.xcf")),
            };

            yield return new object[] { testNamespace, "Document.pdf", testCategoryId1, documents, 0 };
            yield return new object[] { testNamespace, "Document", testCategoryId2, documents, 3 };
            yield return new object[] { testNamespace, "Document.xcf", testCategoryId1, documents, 0 };
            yield return new object[] { testNamespace, "Document.xcf", testCategoryId2, documents, 1 };
        }
        #endregion

        #region Private members
        private IDocumentService CreateDocumentService(string testNamespace)
        {
            var documentServiceFactory = new DocumentServiceFactory();
            var sqlTableNames = new Dictionary<string, string>()
            {
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY, $"DocumentCategories{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY, $"DocumentCategoryEntities{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY, $"DocumentMetadataDefinitions{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY, $"DocumentMetadata{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY, $"Documents{testNamespace}" },
            };

            return documentServiceFactory.CreateDocumentService(@".\Preferences\TestApplicationConfiguration.xml", sqlTableNames);
        }
        #endregion
    }
}
