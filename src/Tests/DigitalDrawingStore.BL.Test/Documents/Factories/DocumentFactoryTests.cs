using Moq;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;

namespace XperiCad.DigitalDrawingStore.BL.Test.Documents.Factories
{
    public class DocumentFactoryTests
    {
        #region Tests
        [Theory]
        [MemberData(nameof(GetTestParametersForDF0011))]
        public void DF0011_Given_NullOrEmptyAsParameter_When_ConstructDocumentFactory_Then_ThrowArgumentNullException(
            IDataSource dataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            IEnumerable<IDocumentExporter> documentExporters,
            IDocumentWatermarkProvider documentWatermarkProvider,
            Type expectedExceptionType,
            string expectedExceptionMessage)
        {
            var exception = Record.Exception(() => new DocumentFactory(dataSource, dataParameterFactory, sqlTableNames, documentExporters, documentWatermarkProvider));

            Assert.IsType(expectedExceptionType, exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [InlineData(@".\TestPath\doc.pdf", false, false)]
        [InlineData(null, true, true)]
        [InlineData("", true, true)]
        [InlineData(" ", true, true)]
        [InlineData("  ", true, true)]
        [InlineData("\t", true, true)]
        public void DF0011_Given_NullAsArgument_When_CreatePdfDocument_Then_ThrowsArgumentNullException(string path, bool shouldThrowException, bool isArgumentException)
        {
            var documentFactory = CreateDocumentFactory();

            var exception = Record.Exception(() => documentFactory.CreatePdfDocument(Guid.NewGuid(), path));

            if (shouldThrowException)
            {
                if (isArgumentException)
                {
                    Assert.IsType<ArgumentException>(exception);
                    Assert.Equal("'path' cannot be null or whitespace. (Parameter 'path')", exception.Message);
                }
                else
                {
                    Assert.IsType<ArgumentNullException>(exception);
                    Assert.Equal("No pdf exporters found in registered document exporters.", exception.Message);
                }
            }
            else
            {
                Assert.Null(exception);
            }
        }

        [Theory]
        [InlineData(@".\TestPath\doc.pdf")]
        [InlineData(@"\\NetworkPath\doc.pdf")]
        [InlineData(@"C:\Temp\AbsolutePath\doc.pdf")]
        public void DF0021_Given_EmptyDocumentExporterList_When_CreatePdfDocument_Then_ThrowsArgumentException(string path)
        {
            var documentFactory = CreateDocumentFactory(new List<IDocumentExporter>());

            var exception = Record.Exception(() => documentFactory.CreatePdfDocument(Guid.NewGuid(), path));

            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("No pdf exporters found in registered document exporters.", exception.Message);
        }

        [Theory]
        [InlineData(@".\TestPath\doc.pdf")]
        [InlineData(@"\\NetworkPath\Somewhere\doc.pdf")]
        [InlineData(@"C:\Temp\AbsolutePath\doc.pdf")]
        public void DF0031_Given_ValidDocumentFactory_When_CreatePdfDocument_Then_ReturnsWithPdfDocument(string path)
        {
            var documentFactory = CreateDocumentFactory();

            var document = documentFactory.CreatePdfDocument(Guid.NewGuid(), path);

            Assert.Equal(path, document.Path);
        }
        #endregion

        #region Test parameters
        public static IEnumerable<object[]> GetTestParametersForDF0011()
        {
            yield return new object[] { null, Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Mock.Of<IEnumerable<IDocumentExporter>>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'msSqlDataSource')" };
            yield return new object[] { Mock.Of<IDataSource>(), null, Mock.Of<IDictionary<string, string>>(), Mock.Of<IEnumerable<IDocumentExporter>>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'dataParameterFactory')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), null, Mock.Of<IEnumerable<IDocumentExporter>>(), Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'sqlTableNames')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), null, Mock.Of<IDocumentWatermarkProvider>(), typeof(ArgumentNullException), "Value cannot be null. (Parameter 'documentExporters')" };
            yield return new object[] { Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), Mock.Of<IEnumerable<IDocumentExporter>>(), null, typeof(ArgumentNullException), "Value cannot be null. (Parameter 'documentWatermarkProvider')" };
        }
        #endregion

        #region Private members

        private IDocumentFactory CreateDocumentFactory()
        {
            return CreateDocumentFactory(new List<IDocumentExporter>() { new PdfDocumentExporter() });
        }

        private IDocumentFactory CreateDocumentFactory(IEnumerable<IDocumentExporter> documentExporter)
        {
            return new DocumentFactory(Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), Mock.Of<IDictionary<string, string>>(), documentExporter, Mock.Of<IDocumentWatermarkProvider>());
        }
        #endregion
    }
}
