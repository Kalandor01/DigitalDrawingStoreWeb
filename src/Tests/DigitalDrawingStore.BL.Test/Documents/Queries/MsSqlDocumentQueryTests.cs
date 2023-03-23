using Moq;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Queries;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;

namespace XperiCad.DigitalDrawingStore.BL.Test.Documents.Queries
{
    public class MsSqlDocumentQueryTests : BaseDocumentTests
    {
        #region Tests
        [Theory]
        [MemberData(nameof(GetTestParametersForMSDQ0011))]
        public void MSDQ0011_Given_NullOrEmptyAsParameters_When_CreateMsSqlDocumentQueryObject_Then_ThrowsArgumentNullException(
            IDataSource dataSource,
            IDataParameterFactory dataParameterFactory,
            object documentFactoryObject,
            IFeedbackMessageFactory feedbackMessageFactory,
            Type expectedExceptionType,
            string expectedExceptionMessage)
        {
            var documentFactory = (IDocumentFactory)documentFactoryObject;

            var exception = Record.Exception(() => CreateDocumentQuery(dataSource, dataParameterFactory, "N/A", documentFactory, feedbackMessageFactory));

            Assert.IsType(expectedExceptionType, exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(GetTestParametersForMSDQ0021))]
        public async void MSDQ0021_Given_NullOrEmptyOrStarAsSearchTextWithSpecifiedDocumentCategory_When_QueryDocuments_Then_ReturnsWithAllDocumentsInCategory(
            Guid targetDocumentCategoryId,
            string searchText,
            IEnumerable<CategorizedDocument> testDocuments,
            int expectedNumberOfFoundDocuments)
        {
            var testNamespace = default(string);

            try
            {
                testNamespace = PrepareTestEnvironment(testDocuments);
                var documentQuery = CreateDocumentQuery(testNamespace);

                var result = (await documentQuery.QueryDocumentsAsync(targetDocumentCategoryId, searchText)).ResponseObject;

                Assert.Equal(expectedNumberOfFoundDocuments, result.Count());
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
        [MemberData(nameof(GetTestParametersForMSDQ0031))]
        public async void MSDQ0031_Given_NullOrEmptyOrStarAsSearchTextWithUnspecifiedDocumentCategory_When_QueryDocuments_Then_ReturnsWithAllDocuments(
            string searchText,
            IEnumerable<IDocument> documents)
        {
            var testNamespace = default(string);

            try
            {
                testNamespace = PrepareTestEnvironment(documents);
                var documentQuery = CreateDocumentQuery(testNamespace);

                var result = (await documentQuery.QueryDocumentsAsync(searchText)).ResponseObject;

                Assert.Equal(documents.Count(), result.Count());
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
        [MemberData(nameof(GetTestParametersForMSDQ0041))]
        public async void MSDQ0041_Given_ValidSearchTextWithSpecifiedDocumentCategory_When_QueryDocuments_Then_ReturnsWithFilteredDocuments(
            string searchText,
            Guid documentCategoryId,
            IEnumerable<CategorizedDocument> documents,
            int expectedDocumentCount)
        {
            var testNamespace = default(string);

            try
            {
                testNamespace = PrepareTestEnvironment(documents);
                var documentQuery = CreateDocumentQuery(testNamespace);

                var result = (await documentQuery.QueryDocumentsAsync(documentCategoryId, searchText)).ResponseObject;

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
        public static IEnumerable<object[]> GetTestParametersForMSDQ0011()
        {
            yield return new object[] { null, Mock.Of<IDataParameterFactory>(), Mock.Of<IDocumentFactory>(), Mock.Of<IFeedbackMessageFactory>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'msSqlDataSource')" };
            yield return new object[] { Mock.Of<IDataSource>(), null, Mock.Of<IDocumentFactory>(), Mock.Of<IFeedbackMessageFactory>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'dataParameterFactory')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), null, Mock.Of<IFeedbackMessageFactory>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'documentFactory')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDocumentFactory>(), null, typeof(ArgumentNullException), "Value cannot be null. (Parameter 'feedbackMessageFactory')" };
        }

        public static IEnumerable<object[]> GetTestParametersForMSDQ0021()
        {
            var documentFactory = CreateDocumentFactory().DocumentFactory;
            var targetDocumentCategoryId = Guid.NewGuid();

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(targetDocumentCategoryId, "TestCategory", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf")),
                new CategorizedDocument(targetDocumentCategoryId, "TestCategory", documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory2", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory3", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\Document\With\Other\Category\Document.pdf"))
            };

            const int EXPECTED_NUMBER_OF_FOUND_DOCMENTS = 2;

            yield return new object[] { targetDocumentCategoryId, "*", documents, EXPECTED_NUMBER_OF_FOUND_DOCMENTS };
            yield return new object[] { targetDocumentCategoryId, null, documents, EXPECTED_NUMBER_OF_FOUND_DOCMENTS };
            yield return new object[] { targetDocumentCategoryId, " ", documents, EXPECTED_NUMBER_OF_FOUND_DOCMENTS };
            yield return new object[] { targetDocumentCategoryId, "  ", documents, EXPECTED_NUMBER_OF_FOUND_DOCMENTS };
            yield return new object[] { targetDocumentCategoryId, "\t", documents, EXPECTED_NUMBER_OF_FOUND_DOCMENTS };
        }

        public static IEnumerable<object[]> GetTestParametersForMSDQ0031()
        {
            var documentFactory = CreateDocumentFactory().DocumentFactory;

            var documents = new List<IDocument>()
            {
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf"),
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf"),
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf"),
                documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\Document\With\Other\Category\Document.pdf")
            };

            yield return new object[] { "*", documents };
            yield return new object[] { null, documents };
            yield return new object[] { " ", documents };
            yield return new object[] { "  ", documents };
            yield return new object[] { "\t", documents };
        }


        public static IEnumerable<object[]> GetTestParametersForMSDQ0041()
        {
            var documentFactory = CreateDocumentFactory().DocumentFactory;
            var documentCategoryId = Guid.NewGuid();

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(documentCategoryId, "TestCategory", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\Hello\World.pdf")),
                new CategorizedDocument(documentCategoryId, "TestCategory", documentFactory.CreatePdfDocument(Guid.NewGuid(), @".\Document.pdf")),
                new CategorizedDocument(documentCategoryId, "TestCategory", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.pdf")),
                new CategorizedDocument(documentCategoryId, "TestCategory", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\Document.xcf")),
                new CategorizedDocument(Guid.NewGuid(), "TestCategory", documentFactory.CreatePdfDocument(Guid.NewGuid(), @"\\NetworkPath\Somewhere\DocumentInOtherCategory.pdf"))
            };

            yield return new object[] { "Document.pdf", documentCategoryId, documents, 2 };
            yield return new object[] { "Document.pdf", Guid.NewGuid(), documents, 0 };
            yield return new object[] { "Document", documentCategoryId, documents, 3 };
            yield return new object[] { "Document", Guid.NewGuid(), documents, 0 };
            yield return new object[] { "Docume", documentCategoryId, documents, 3 };
            yield return new object[] { "Somew", documentCategoryId, documents, 2 }; // TODO: investigate, is this an acceptable result?
            yield return new object[] { @"C:\Hello\World.pdf", documentCategoryId, documents, 1 };
            yield return new object[] { @"C:\Nowhere\NonExistingFile.pdf", documentCategoryId, documents, 0 };
            // TODO: need a test case to check for other attribute values (e.g. PartNumber, TestAttribute, etc, for all type, bool, string, int, etc)
        }
        #endregion

        #region Private members
        private IDocumentQuery CreateDocumentQuery(string testNamespace)
        {
            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;

            return CreateDocumentQuery(MsSqlDataSource, DataParameterFactory, testNamespace, documentFactory, Mock.Of<IFeedbackMessageFactory>());
        }

        private IDocumentQuery CreateDocumentQuery(
            IDataSource dataSource,
            IDataParameterFactory dataParameterFactory,
            string testNamespace,
            IDocumentFactory documentFactory,
            IFeedbackMessageFactory feedbackMessageFactory)
        {
            var sqlTableNames = new Dictionary<string, string>()
            {
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY, $"Documents{testNamespace}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY, $"DocumentMetadata{testNamespace}" },
            };

            return new MsSqlDocumentQuery(dataSource, dataParameterFactory, sqlTableNames, documentFactory, feedbackMessageFactory);
        }
        #endregion
    }
}
