using Moq;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;

namespace XperiCad.DigitalDrawingStore.BL.Test.Documents
{
    public class MsSqlDocumentCategoryTests : BaseDocumentTests
    {
        #region Tests
        [Theory]
        [MemberData(nameof(MSDC0011_GetTestParameters))]
        public void MSDC0011_Given_NullOrEmptyArguments_When_CreateDocumentCategory_Then_ThrowsArgumentNullException(
            Guid id,
            IDataSource dataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            Type expectedExceptionType,
            string expectedExceptionMessage)
        {
            var exception = Record.Exception(() => new MsSqlDocumentCategory(id, dataSource, dataParameterFactory, sqlTableNames));

            Assert.IsType(expectedExceptionType, exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(MSDC0021_GetTestParameters))]
        public async Task MSDC0021_Given_ValidDocumentCategories_When_GetDisplayName_Then_ReturnsWithDisplayName(
            string testNamespace,
            IEnumerable<CategoryDTO> documentCategories,
            IEnumerable<CategorizedDocument> documents,
            IDocumentCategory documentCategory,
            string expectedDisplayName)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, documentCategories, documents);

                var result = await documentCategory.GetDisplayNameAsync();

                Assert.NotNull(result);
                Assert.Equal(expectedDisplayName, result);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(testNamespace))
                {
                    DisposeTestEnvironment(testNamespace);
                }
            }
        }

        [Theory(Skip = "Need to implement SetDocumentCategoryEntityAsync in MsSqlDocumentCategory.")]
        [MemberData(nameof(MSDC0031_GetTestParameters))]
        public async Task MSDC0031_Given_ValidDocumentCategories_When_GetAttributes_Then_ReturnsWithValidAttribute(
            string testNamespace,
            IEnumerable<CategoryDTO> documentCategories,
            IEnumerable<CategorizedDocument> documents,
            IDocumentCategory targetDocumentCategory,
            IDocument targetDocument,
            IDictionary<string, string> metadata)
        {
            try
            {
                PrepareTestEnvironment(testNamespace, documentCategories, documents);
                foreach (var documentDTO in documents)
                {
                    foreach (var attribute in metadata)
                    {
                        await documentDTO.Document.SetAttribute(attribute.Key, attribute.Value);
                    }
                }


                Assert.NotEmpty(await targetDocumentCategory.GetAttributesAsync());
                foreach (var attribute in await targetDocumentCategory.GetAttributesAsync())
                {
                    var attributeValue = targetDocument.GetAttribute<string>(attribute.Key).Result;

                    Assert.Equal(metadata[attribute.Key], attributeValue);
                }
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
        public static IEnumerable<object[]> MSDC0011_GetTestParameters()
        {
            yield return new object[] { Guid.Empty, Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), new Dictionary<string, string>(), typeof(ArgumentException), "Argument id could not be an empty Guid." };
            yield return new object[] { Guid.NewGuid(), null, Mock.Of<IDataParameterFactory>(), new Dictionary<string, string>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'msSqlDataSource')" };
            yield return new object[] { Guid.NewGuid(), Mock.Of<IDataSource>(), null, new Dictionary<string, string>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'dataParameterFactory')" };
            yield return new object[] { Guid.NewGuid(), Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), null, typeof(ArgumentNullException), "Value cannot be null. (Parameter 'sqlTableNames')" };
        }

        public static IEnumerable<object[]> MSDC0021_GetTestParameters()
        {
            var testNamespace = CreateTestNamespace();
            var categoriesDTO = new List<CategoryDTO>()
            {
                new CategoryDTO(Guid.NewGuid(), "Változások"),
                new CategoryDTO(Guid.NewGuid(), "Szabványok"),
                new CategoryDTO(Guid.NewGuid(), "TestCategory"),
                new CategoryDTO(Guid.NewGuid(), "MyCategory"),
                new CategoryDTO(Guid.NewGuid(), "CategoryA"),
                new CategoryDTO(Guid.NewGuid(), "CategoryB"),
                new CategoryDTO(Guid.NewGuid(), "CategoryC"),
                new CategoryDTO(Guid.NewGuid(), "Hello World"),
            };

            var categories = new List<IDocumentCategory>()
            {
                CreateDocumentCategory(testNamespace, categoriesDTO[0].CategoryId, categoriesDTO[0].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[1].CategoryId, categoriesDTO[1].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[2].CategoryId, categoriesDTO[2].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[3].CategoryId, categoriesDTO[3].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[4].CategoryId, categoriesDTO[4].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[5].CategoryId, categoriesDTO[5].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[6].CategoryId, categoriesDTO[6].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[7].CategoryId, categoriesDTO[7].DisplayName),
            };

            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(categories[0], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentA.pdf")),
                new CategorizedDocument(categories[1], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentB.pdf")),
                new CategorizedDocument(categories[1], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentC.pdf")),
                new CategorizedDocument(categories[2], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentD.pdf")),
                new CategorizedDocument(categories[2], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentE.pdf")),
                new CategorizedDocument(categories[2], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentF.pdf")),
                new CategorizedDocument(categories[3], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentF.pdf")),
            };

            yield return new object[] { testNamespace, categoriesDTO, documents, categories[0], "Változások" };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[1], "Szabványok" };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[2], "TestCategory" };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[3], "MyCategory" };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[4], "CategoryA" };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[5], "CategoryB" };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[6], "CategoryC" };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[7], "Hello World" };
        }

        public static IEnumerable<object[]> MSDC0031_GetTestParameters()
        {
            var testNamespace = CreateTestNamespace();
            var categoriesDTO = new List<CategoryDTO>()
            {
                new CategoryDTO(Guid.NewGuid(), "Változások"),
                new CategoryDTO(Guid.NewGuid(), "Szabványok"),
                new CategoryDTO(Guid.NewGuid(), "TestCategory"),
                new CategoryDTO(Guid.NewGuid(), "MyCategory"),
                new CategoryDTO(Guid.NewGuid(), "CategoryA"),
                new CategoryDTO(Guid.NewGuid(), "CategoryB"),
                new CategoryDTO(Guid.NewGuid(), "CategoryC"),
                new CategoryDTO(Guid.NewGuid(), "Hello World"),
            };

            var categories = new List<IDocumentCategory>()
            {
                CreateDocumentCategory(testNamespace, categoriesDTO[0].CategoryId, categoriesDTO[0].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[1].CategoryId, categoriesDTO[1].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[2].CategoryId, categoriesDTO[2].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[3].CategoryId, categoriesDTO[3].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[4].CategoryId, categoriesDTO[4].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[5].CategoryId, categoriesDTO[5].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[6].CategoryId, categoriesDTO[6].DisplayName),
                CreateDocumentCategory(testNamespace, categoriesDTO[7].CategoryId, categoriesDTO[7].DisplayName),
            };

            var documentFactory = CreateDocumentFactory(testNamespace).DocumentFactory;

            var documents = new List<CategorizedDocument>()
            {
                new CategorizedDocument(categories[0], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentA.pdf")),
                new CategorizedDocument(categories[1], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentB.pdf")),
                new CategorizedDocument(categories[1], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentC.pdf")),
                new CategorizedDocument(categories[2], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentD.pdf")),
                new CategorizedDocument(categories[2], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentE.pdf")),
                new CategorizedDocument(categories[2], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentF.pdf")),
                new CategorizedDocument(categories[3], documentFactory.CreatePdfDocument(Guid.NewGuid(), @"C:\XperiCad\TestDocumentF.pdf")),
            };

            var attributes = new Dictionary<string, string>()
            {
                { "AttributeA", "ValueA" },
                { "AttributeB", "ValueB" },
                { "AttributeC", "ValueC" },
                { "AttributeD", "ValueD" },
            };

            yield return new object[] { testNamespace, categoriesDTO, documents, categories[0], documents[0].Document, attributes };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[1], documents[1].Document, attributes };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[2], documents[2].Document, attributes };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[3], documents[3].Document, attributes };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[4], documents[4].Document, attributes };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[5], documents[5].Document, attributes };
            yield return new object[] { testNamespace, categoriesDTO, documents, categories[6], documents[6].Document, attributes };
        }
        #endregion
    }
}
