using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Services.Factories
{
    internal interface IListenerServiceFactory
    {
        IListenerService CreateDocumentListenerService(string applicationConfigurationFilePath, IDictionary<string, string> sqlTableNames);
    }
}
