using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using AdministrationControllerApi = XperiCad.DigitalDrawingStore.Web.API.Controllers.AdministrationController;

namespace DigitalDrawingStore.Web.UI.Controllers
{
    [Authorize(Policy = Constants.Autorization.Policies.ADMIN)]
    public class AdministrationController : BaseController
    {
        #region Fields
        private readonly AdministrationControllerApi _administrationController;
        #endregion

        #region ctor
        public AdministrationController()
        {
            _administrationController = new AdministrationControllerApi();
        }
        #endregion

        #region Public members
        public async Task<JsonResponse<string>> UpdateApplicationConfigurationSettings(
            string? documentDatabaseConnectionString
        )
        {
            return await _administrationController.UpdateApplicationConfiguration(documentDatabaseConnectionString);
        }

        public async Task<JsonResponse<string>> UpdateFeedbackEmailSettings(
            string? senderEmail, string? emailRecipients, string? smtpHost,
            string? smtpPort, string? smtpUsername, string? smtpPassword,
            string? isUseDefaultCredentials, string? isEnableSsl
        )
        {
            var responses = await _administrationController.UpdateFeedbackProperties(
                senderEmail,
                emailRecipients,
                smtpHost,
                smtpPort,
                smtpUsername,
                smtpPassword,
                isUseDefaultCredentials,
                isEnableSsl
            );

            return responses;
        }
        #endregion

        public IActionResult Categories()
        {
            return SharedView();
        }

        public IActionResult Documents()
        {
            return SharedView();
        }

        public IActionResult Settings()
        {
            return SharedView();
        }
    }
}