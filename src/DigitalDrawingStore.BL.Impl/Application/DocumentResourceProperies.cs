using XperiCad.Common.Infrastructure.Application.DataSource;
using XperiCad.DigitalDrawingStore.BL.Application;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application
{
    internal class DocumentResourceProperies : IDocumentResourceProperties
    {
        #region Properties
        public string ResourcePath
        {
            get => _applicationConfigurationService.Query.GetStringPropertyByName("DocumentDatabaseConnectionString");
            set => _applicationConfigurationService.Command.SetPropertyByName("DocumentDatabaseConnectionString", value);
        }
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
