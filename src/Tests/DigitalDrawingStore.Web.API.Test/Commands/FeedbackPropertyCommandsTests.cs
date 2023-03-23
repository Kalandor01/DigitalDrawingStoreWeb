using Unity;
using XperiCad.Common.Core.Core;
using XperiCad.Common.Core.Exceptions;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark.Factories;
using XperiCad.DigitalDrawingStore.Web.API.Commands;

namespace XperiCad.DigitalDrawingStore.Web.API.Test.Commands
{
    public class FeedbackPropertyCommandsTests
    {
        #region Private Fields
        private readonly IUnityContainer _container;
        private readonly IDataSource _msSqlDataSource;
        #endregion

        #region ctor
        public FeedbackPropertyCommandsTests()
        {
            _container = CommonCoreBootstrapper.ConfigureCommonCore(
                new [] { Common.Core.Constants.Culture.CultureKeys.HUNGARIAN_HUNGARY },
                @"XperiCad\DigitalDrawingStore"); // TODO: constant, inject this container to other levels
            var dataSourceFactory = _container.Resolve<IDataSourceFactory>();
            _msSqlDataSource = dataSourceFactory.CreateMsSqlDataSource(BL.Impl.Constants.Documents.Resources.DEFAULT_CONNECTION_STRING);
        }
        
        #endregion

        #region Tests
        
        [Theory]
        [InlineData(@"Resources\Documents\ReferenceDocuments\TestPdf1.pdf")]
        [InlineData(@"Resources\Documents\ReferenceDocuments\TestPdf2.pdf")]
        [InlineData(@"Resources\Documents\ReferenceDocuments\TestPdf3.pdf")]
        [InlineData(@"Resources\Documents\ReferenceDocuments\LargeSizeTestPdf1.pdf")]
        [InlineData(@"Resources\Documents\ReferenceDocuments\LargeSizeTestPdf2.pdf")]
        public async Task FPCT0001_Given_CorrectValues_When_ApplyWatermark_Then_ReturnWatermarkedByteArray(string pdfPath)
        {
            var document = GetTestDocument(pdfPath);
            var bytes = await File.ReadAllBytesAsync(document.Path);

            var command = GetDownloadPreviewActionCommand(
                document,
                "Test",
                "upperLeftCorner",
                "TestUser",
                "TestMachine",
                "[-.-]", 15);
            
            await GetCommandInvoker(command).ExecuteAllAsync();
            
            var response = command.ActionResponse.ResponseObject;
            var result = Convert.FromBase64String(response);

            Assert.NotNull(result);
            Assert.NotEqual(result, bytes);
        }

        [Theory]
        [MemberData(nameof(GetTestParametersForFPCT0011))]
        public void FPCT0011_Given_NullValues_When_CommandExecuted_Then_ThrowFeedbackException(
            string targetOfDocumentUsage, 
            string sideWatermarkPosition, 
            string clientUsername,
            string clientMachineName,
            string clientIp,
            int nullParam)
        {
            var exception = Record.Exception(() => GetDownloadPreviewActionCommand(null!, targetOfDocumentUsage, sideWatermarkPosition, clientUsername, clientMachineName, clientIp, 16));
                
            switch (nullParam)
            {
                case 0:
                    Assert.IsType<FeedbackException>(exception);                   
                    break;
                case 1:
                    Assert.IsType<FeedbackException>(exception);
                    break;
                case 2:
                    Assert.IsType<FeedbackException>(exception);
                    break;
                case 3:
                    Assert.IsType<FeedbackException>(exception);
                    break;
                case 4:
                    Assert.IsType<FeedbackException>(exception);
                    break;           
            }
        }
        
        #endregion

        #region Private members
        private DownloadPreviewActionCommand GetDownloadPreviewActionCommand(
            IDocument document,
            string targetOfDocumentUsage,
            string sideWatermarkPosition,
            string clientUsername,
            string clientMachineName,
            string clientIp,
            int fontSize)
        {
            var documentWatermarkFactory = new DocumentWatermarkFactory();
            return new DownloadPreviewActionCommand(
                document,
                documentWatermarkFactory,
                40,
                fontSize,
                targetOfDocumentUsage,
                0,
                0,
                sideWatermarkPosition,
                clientUsername,
                clientMachineName,
                clientIp);
        }

        private IDictionary<string, string> GetSqlTableNames()
        {
            return new Dictionary<string, string>
            {
                { BL.Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY, "DocumentCategories" },
                { BL.Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY, "DocumentCategoryEntities" },
                { BL.Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY, "DocumentMetadataDefinitions" },
                { BL.Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY, "DocumentMetadata" },
                { BL.Impl.Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY, "Documents" },
                { BL.Impl.Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_TABLE_NAME_KEY, "ApplicationProperties" },
                { BL.Impl.Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_DICTIONARY_TABLE_NAME_KEY, "ApplicationPropertiesDictionary" },
            };
        }

        private IDocument GetTestDocument(string pdfPath)
        {
            var dataParameterFactory = _container.Resolve<IDataParameterFactory>();
            var watermarkProvider = new PdfDocumentWatermarkProvider();
            var documentExporter = new PdfDocumentExporter();

            return new MsSqlPdfDocument(_msSqlDataSource, dataParameterFactory, GetSqlTableNames(), Guid.NewGuid(), pdfPath, documentExporter, watermarkProvider);
        }

        private IActionCommandInvoker<T> GetCommandInvoker<T>(IActionCommand<T> command)
        {
            var commandInvokerFactory = _container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<T>();
            commandInvoker.AddCommand(command);
            return commandInvoker;
        }
        #endregion

        #region MemberData
        public static IEnumerable<object[]> GetTestParametersForFPCT0011()
        {
            yield return new object[] { "", "Test_sideWatermarkPosition", "Test_clientUsername", "Test_clientMachineName", "Test_clientIp", 0 };
            yield return new object[] { "Test_targetOfDocumentUsage", "", "Test_clientUsername", "Test_clientMachineName", "Test_clientIp", 1 };
            yield return new object[] { "Test_targetOfDocumentUsage", "Test_sideWatermarkPosition", "", "Test_clientMachineName", "Test_clientIp", 2 };
            yield return new object[] { "Test_targetOfDocumentUsage", "Test_sideWatermarkPosition", "Test_clientUsername", "", "Test_clientIp", 3 };
            yield return new object[] { "Test_targetOfDocumentUsage", "Test_sideWatermarkPosition", "Test_clientUsername", "Test_clientMachineName", "", 4 };
        }
        #endregion
    }
}
