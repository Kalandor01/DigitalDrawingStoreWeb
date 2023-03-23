using System.Data.SqlClient;
using Unity;
using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories;
using XperiCad.DigitalDrawingStore.Web.API.DTO;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class UpdateDocumentDatabaseConnectionStringActionCommand : AActionCommand<JsonResponse<string>>
    {
        #region Fields
        private readonly IDocumentResourceProperties _documentProperties;
        private readonly string _documentDatabaseConnectionString;
        private readonly string _selectedCulture;
        #endregion

        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region ctor
        public UpdateDocumentDatabaseConnectionStringActionCommand (
            string documentDatabaseConnectionString, 
            string applicationConfigurationFilePath
            )
        {
            _documentProperties = new DocumentResourceProperiesFactory().CreateDocumentResourceProperties(applicationConfigurationFilePath);

            if (string.IsNullOrWhiteSpace(documentDatabaseConnectionString))
            {
                throw new ArgumentException($"'{nameof(documentDatabaseConnectionString)}' cannot be null or whitespace.", nameof(documentDatabaseConnectionString));
            }

            _documentDatabaseConnectionString = documentDatabaseConnectionString;

            var container = new ContainerFactory().CreateContainer();
            var commonApplicationProperties = container.Resolve<ICommonApplicationProperties>();
            _selectedCulture = commonApplicationProperties.GeneralApplicationProperties.SelectedCulture.LanguageCountryCode;
        }
        #endregion

        #region AActionCommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public async override Task ExecuteAsync()
        {
            bool success;
            string response;
            var feedback = new List<FeedbackMessage>();
            if (await IsDatabaseConnectionValid())
            {
                _documentProperties.ResourcePath = _documentDatabaseConnectionString;
                response = "Sikeresen módosítva.";
                feedback.Add(new FeedbackMessage(Severity.Information, "Sikeresen módosítva."));
                success = true;
            }
            else
            {
                var feedbackResource = Resources.i18n.Feedback.Fatal_Wrong_Document_Database_Connection_String;
                var feedbackMessage = feedbackResource.CultureResource.GetCultureString(_selectedCulture).FirstOrDefault().Value;
                feedback.Add(new FeedbackMessage(feedbackResource.Severity, feedbackMessage));
                response = "Nem sikerült módosítani.";
                success = false;
            }

            ResolveAction(new JsonResponse<string>(response, feedback, success));
        }
        #endregion

        #region Private members
        private async Task<bool> IsDatabaseConnectionValid()
        {
            try
            {
                var conn = new SqlConnection(_documentDatabaseConnectionString);
                await conn.OpenAsync();
                await conn.CloseAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
