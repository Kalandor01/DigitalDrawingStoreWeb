using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.Common.Core.Exceptions;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Factories;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class DownloadPreviewActionCommand : AActionCommand<string>
    {
        #region Properties
        public override bool CanExecute => true; // TODO: create conscheck for watermarks
        #endregion

        #region Fields
        private IDocument _document;
        private readonly IDocumentWatermarkFactory _documentWatermarkFactory;
        private readonly float _watermarkOpacityInPercentage;
        private readonly int _fontSize;
        private readonly string _targetOfDocumentUsage;
        private readonly int _centralWatermarkHorizontalOffset;
        private readonly int _centralWatermarkVerticalOffset;
        private readonly string _clientUsername;
        private readonly string _clientMachineName;
        private readonly string _clientIp;
        private readonly string _sideWatermarkPosition; // TODO: investigate, is this needed here?
        #endregion

        #region ctor
        public DownloadPreviewActionCommand(
            IDocument document,
            IDocumentWatermarkFactory documentWatermarkFactory,
            float watermarkOpacityInPercentage,
            int fontSize,
            string targetOfDocumentUsage,
            int centerWatermarkHorizontalOffset,
            int centerWatermarkVerticalOffset,
            string sideWatermarkPosition,
            string clientUsername,
            string clientMachineName,
            string clientIp)
        {
            if (string.IsNullOrWhiteSpace(targetOfDocumentUsage))
            {
                throw new FeedbackException($"'{nameof(targetOfDocumentUsage)}' cannot be null or whitespace.", Resources.i18n.Feedback.Error_TargetOfDocumentUsageIsNull);
            }

            if (string.IsNullOrWhiteSpace(clientUsername))
            {
                throw new FeedbackException($"'{nameof(clientUsername)}' cannot be null or whitespace.", Resources.i18n.Feedback.Error_ClientUsernameIsNull);
            }

            if (string.IsNullOrWhiteSpace(clientMachineName))
            {
                throw new FeedbackException($"'{nameof(clientMachineName)}' cannot be null or whitespace.", Resources.i18n.Feedback.Error_ClientMachineNameIsNull);
            }

            if (string.IsNullOrWhiteSpace(clientIp))
            {
                throw new FeedbackException($"'{nameof(clientIp)}' cannot be null or whitespace.", Resources.i18n.Feedback.Error_ClientIpIsNull);
            }

            if (string.IsNullOrWhiteSpace(sideWatermarkPosition))
            {
                throw new FeedbackException($"'{nameof(sideWatermarkPosition)}' cannot be null or whitespace.", Resources.i18n.Feedback.Error_SideWatermarkPositionIsNull);
            }

            _document = document ?? throw new ArgumentNullException(nameof(document));
            _documentWatermarkFactory = documentWatermarkFactory ?? throw new ArgumentNullException(nameof(documentWatermarkFactory));
            _watermarkOpacityInPercentage = watermarkOpacityInPercentage;
            _fontSize = fontSize;
            _targetOfDocumentUsage = targetOfDocumentUsage;
            _centralWatermarkHorizontalOffset = centerWatermarkHorizontalOffset;
            _centralWatermarkVerticalOffset = centerWatermarkVerticalOffset;
            _clientUsername = clientUsername;
            _clientMachineName = clientMachineName;
            _clientIp = clientIp;
            _sideWatermarkPosition = sideWatermarkPosition;
        }
        #endregion

        #region AActionCommand members
        public override void Execute()
        {
            var sideWatermarkVerticalPosition = WatermarkVerticalPosition.Top;
            var sideWatermarkHorizontalPosition = WatermarkHorizontalPosition.Right;

            switch (_sideWatermarkPosition)
            {
                case "upperLeftCorner":
                    sideWatermarkVerticalPosition = WatermarkVerticalPosition.Top;
                    sideWatermarkHorizontalPosition = WatermarkHorizontalPosition.Left;
                    break;
                case "upperRightCorner":
                    sideWatermarkVerticalPosition = WatermarkVerticalPosition.Top;
                    sideWatermarkHorizontalPosition = WatermarkHorizontalPosition.Right;
                    break;
                case "bottomLeftCorner":
                    sideWatermarkVerticalPosition = WatermarkVerticalPosition.Bottom;
                    sideWatermarkHorizontalPosition = WatermarkHorizontalPosition.Left;
                    break;
                case "bottomRightCorner":
                    sideWatermarkVerticalPosition = WatermarkVerticalPosition.Bottom;
                    sideWatermarkHorizontalPosition = WatermarkHorizontalPosition.Right;
                    break;
            }

            var downloadDateTime = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss");
            var sideWatermarkText = $"{_clientUsername} - {downloadDateTime}";

            var watermarks = new List<IDocumentWatermark>
            {
                _documentWatermarkFactory.CreateWatermark(_targetOfDocumentUsage, _fontSize,
                    _watermarkOpacityInPercentage, 60,
                    _centralWatermarkHorizontalOffset, _centralWatermarkVerticalOffset,
                    WatermarkVerticalPosition.Center, WatermarkHorizontalPosition.Center), // TODO: Implement new requirement

                _documentWatermarkFactory.CreateWatermark(sideWatermarkText, 20,
                    _watermarkOpacityInPercentage, 0,
                    sideWatermarkVerticalPosition, sideWatermarkHorizontalPosition)
            };

            var watermarkedDocument = _document.Download(watermarks);
            ResolveAction(Convert.ToBase64String(watermarkedDocument));
        }

        public override async Task ExecuteAsync()
        {
            await Task.Run(() => Execute());
        }
        #endregion
    }
}
