using DigitalDrawingStore.Listener.Service.Services;
using DigitalDrawingStore.Listener.Service.Services.Factories;
using System.Collections.Generic;
using System.Diagnostics;
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
            // FOR DEBUGGING
            Debugger.Launch();
            // FOR DEBUGGING
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
                { Constants.DocumentDatabase.DOCUMENT_CATEGORIES_TABLE_NAME_KEY, "DocumentCategories" },
                { Constants.DocumentDatabase.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY, "DocumentCategoryEntities" },
                { Constants.DocumentDatabase.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY, "DocumentMetadataDefinitions" },
                { Constants.DocumentDatabase.DOCUMENTS_METADATA_TABLE_NAME_KEY, "DocumentMetadata" },
                { Constants.DocumentDatabase.DOCUMENTS_TABLE_NAME_KEY, "Documents" },
                { Constants.DocumentDatabase.APPLICATION_PROPERTIES_TABLE_NAME_KEY, "ApplicationProperties" },
                { Constants.DocumentDatabase.USER_EVENT_LOGS_TABLE_NAME_KEY, "UserEventLogs" },
                { Constants.DocumentDatabase.APPLICATION_PROPERTIES_DICTIONARY_TABLE_NAME_KEY, "ApplicationPropertiesDictionary" },
            };

            var listenerServiceFactory = new ListenerServiceFactory();
            var listenerService = listenerServiceFactory
                .CreateDocumentListenerService(@".\Preferences\ApplicationConfiguration.xml", sqlTableNames);

            return listenerService;
        }
        #endregion
    }
}
