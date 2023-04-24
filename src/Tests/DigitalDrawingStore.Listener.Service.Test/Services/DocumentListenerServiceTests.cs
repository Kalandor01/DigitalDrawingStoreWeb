using DigitalDrawingStore.Listener.Service;
using DigitalDrawingStore.Listener.Service.Services;
using DigitalDrawingStore.Listener.Service.Services.Factories;

namespace XperiCad.DigitalDrawingStore.Listener.Service.Test.Services
{
    public class DocumentListenerServiceTests
    {
        #region Tests
        [Fact]
        public async Task DLS0011_Given_TestDocumentEnvironment_When_StartListening_Then_FindsDocumentsAndInsertItIntoDatabase()
        {
            _ = Directory.CreateDirectory(@".\Resources\TestDocuments\DropLocation");

            // TODO: prepare test environment programatically
            var listenerService = CreateListenerService();

            await listenerService.StartListeningAsync(1, 1);

            // TODO: assert
        }
        #endregion

        #region Private members
        private IListenerService CreateListenerService()
        {
            var sqlTableNames = new Dictionary<string, string>()
            {
                { Constants.DocumentDatabase.DOCUMENTS_TABLE_NAME_KEY, "DocumentsTest" },
                { Constants.DocumentDatabase.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY, "DocumentMetadataDefinitionsTest" },
                { Constants.DocumentDatabase.DOCUMENTS_METADATA_TABLE_NAME_KEY, "DocumentMetadataTest" },
                { Constants.DocumentDatabase.DOCUMENT_CATEGORIES_TABLE_NAME_KEY, "DocumentCategoriesTest" },
                { Constants.DocumentDatabase.APPLICATION_PROPERTIES_TABLE_NAME_KEY, "ApplicationProperties" },
            };

            var listenerServiceFactory = new ListenerServiceFactory();
            var listenerService = listenerServiceFactory
                .CreateDocumentListenerService(@".\Preferences\TestApplicationConfiguration.xml", sqlTableNames);

            return listenerService;
        }
        #endregion
    }
}
