using System;
using XperiCad.Common.Infrastructure.Application.DataSource;

namespace DigitalDrawingStore.Listener.Service.Application
{
    internal class DocumentResourceProperies : IDocumentResourceProperies
    {
        #region Properties
        public string ResourcePath => _applicationConfigurationService.Query.GetStringPropertyByName("DocumentDatabaseConnectionString");
        #endregion

        #region Fields
        private readonly IApplicationConfigurationService _applicationConfigurationService;
        #endregion

        #region Constructor
        public DocumentResourceProperies(IApplicationConfigurationService applicationConfigurationService)
        {
            _applicationConfigurationService = applicationConfigurationService ?? throw new ArgumentNullException(nameof(applicationConfigurationService));
        }
        #endregion
    }
}
