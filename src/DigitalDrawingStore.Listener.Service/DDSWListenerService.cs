using DigitalDrawingStore.Listener.Service.Services;
using DigitalDrawingStore.Listener.Service.Services.Factories;
using System.Collections.Generic;
using System.ServiceProcess;

namespace DigitalDrawingStore.Listener.Service
{
    public partial class ddswListener : ServiceBase
    {
        #region Fields
        private readonly IListenerService _listenerService;
        #endregion

        #region Constructor
        public ddswListener()
        {
            InitializeComponent();
            _listenerService = CreateListenerService();
        }
        #endregion

        #region ServiceBase members
        protected override void OnStart(string[] args)
        {
            _listenerService.StartListening();
        }

        protected override void OnStop()
        {
            _listenerService.StopListening();
        }
        #endregion

        #region Private members
        private IListenerService CreateListenerService()
        {
            var sqlTableNames = new Dictionary<string, string>()
            {
                { Constants.DocumentDatabase.DOCUMENTS_TABLE_NAME_KEY, "Documents" },
                { Constants.DocumentDatabase.DOCUMENT_METADATA_DEFINITIONS_TABLE_NAME_KEY, "DocumentMetadataDefinitions" },
                { Constants.DocumentDatabase.DOCUMENT_METADATA_TABLE_NAME_KEY, "DocumentMetadata" },
                { Constants.DocumentDatabase.DOCUMENT_CATEGORIES_TABLE_NAME_KEY, "DocumentCategories" },
                { Constants.DocumentDatabase.APPLICATION_PROPERTIES_TABLE_NAME_KEY, "ApplicationProperties" },
            };

            var listenerServiceFactory = new ListenerServiceFactory();
            var listenerService = listenerServiceFactory
                .CreateDocumentListenerService(@".\Preferences\ApplicationConfiguration.xml", sqlTableNames);

            return listenerService;
        }
        #endregion
    }
}
