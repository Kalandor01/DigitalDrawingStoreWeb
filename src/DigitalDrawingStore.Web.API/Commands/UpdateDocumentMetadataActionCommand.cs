using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class UpdateDocumentMetadataActionCommand : AActionCommand<bool>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private IDocumentService _documentService;
        private readonly Guid _documentId;
        private readonly string _metadataName;
        private readonly string _metadataValue;
        private readonly string _oldMetadataName;
        #endregion

        #region ctor
        public UpdateDocumentMetadataActionCommand(Guid documentId, string metadataName, string metadataValue, string oldMetadataName)
        {
            if (string.IsNullOrWhiteSpace(metadataName))
            {
                throw new ArgumentException($"'{nameof(metadataName)}' cannot be null or whitespace.", nameof(metadataName));
            }

            if (string.IsNullOrWhiteSpace(metadataValue))
            {
                throw new ArgumentException($"'{nameof(metadataValue)}' cannot be null or whitespace.", nameof(metadataValue));
            }

            if (string.IsNullOrWhiteSpace(oldMetadataName))
            {
                throw new ArgumentException($"'{nameof(oldMetadataName)}' cannot be null or whitespace.", nameof(oldMetadataName));
            }

            var documentServiceFactory = new DocumentServiceFactory(); // TODO: inject
            _documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _documentId = documentId;
            _metadataName = metadataName;
            _metadataValue = metadataValue;
            _oldMetadataName = oldMetadataName;
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var response = false;

            if (_documentId != Guid.Empty)
            {
                response = await _documentService.UpdateDocumentMetadata(_documentId, _metadataName, _metadataValue, _oldMetadataName);
            }

            ResolveAction(response);
        }
        #endregion
    }
}
