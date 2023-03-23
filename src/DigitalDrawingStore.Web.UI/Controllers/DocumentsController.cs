using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using DocumentsApiController = XperiCad.DigitalDrawingStore.Web.API.Controllers.DocumentsController;

namespace DigitalDrawingStore.Web.UI.Controllers
{
    [Route("Documents")]
    [Authorize(Policy = Constants.Autorization.Policies.USER)]
    public class DocumentsController : Controller
    {
        #region Fields
        private readonly DocumentsApiController _documentsController;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region ctor
        public DocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            // TODO: implement DI for ApiModule
            _documentsController = new DocumentsApiController();
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        #endregion

        #region GET
        [Route("")]
        public async Task<JsonResponse<IEnumerable<DocumentWithPath>>> Documents(string searchText)
        {
            return await _documentsController.Documents(searchText);
        }

        [Route("Metadata")]
        public async Task<JsonResponse<IDictionary<string, string>>> Metadata(Guid documentId)
        {
            return await _documentsController.Metadata(documentId);
        }

        [Route("TargetOfDocumentUsages")]
        public async Task<IActionResponse<IDictionary<Guid, string>>> GetTargetOfDocumentUsages()
        {
            return await _documentsController.GetTargetOfDocumentUsages();
        }

        [Route("DocumentPreview")]
        public async Task<JsonResponse<string>> DocumentPreview(
            Guid id,
            float watermarkOpacity,
            string sideWatermarkPosition,
            int centeredWatermarkHorizontalOffset,
            int centeredWatermarkVerticalOffset,
            string targetOfDocumentUsage,
            int fontSize)
        {
            var username = HttpContext.User.Identity?.Name;
            var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;
            string clientIpStr;
            string clientMachineName;
            if (clientIp != null)
            {
                clientIpStr = clientIp.ToString();
                clientMachineName = Dns.GetHostEntry(clientIp).HostName;
            }
            else
            {
                clientIpStr = "[Ip address is not available]";
                clientMachineName = "[Machine name not found]";
            }

            var targets = await _documentsController.GetTargetOfDocumentUsages();

            var selectedTargetOfDocumentUsage = targets.ResponseObject?.FirstOrDefault() ?? new KeyValuePair<Guid, string>(Guid.Empty, "");
            if (!string.IsNullOrWhiteSpace(targetOfDocumentUsage))
            {
                selectedTargetOfDocumentUsage = targets.ResponseObject?.FirstOrDefault(t => t.Key.ToString() == targetOfDocumentUsage) ?? targets.ResponseObject.FirstOrDefault();
            }

            return await _documentsController.DocumentPreview(
                id,
                watermarkOpacity,
                fontSize,
                sideWatermarkPosition,
                centeredWatermarkHorizontalOffset,
                centeredWatermarkVerticalOffset,
                selectedTargetOfDocumentUsage.Value,
                username,
                clientMachineName,
                clientIpStr);
        }
        #endregion

        #region PUT
        [Route("UpdateMetadata")]
        [Authorize(Policy = Constants.Autorization.Policies.ADMIN)]
        public async Task<IActionResponse<bool>> UpdateMetadata(Guid documentId, string metadataName, string metadataValue, string oldMetadataName)
        {
            return await _documentsController.UpdateMetadata(documentId, metadataName, metadataValue, oldMetadataName);
        }

        [Route("UpdateDocumentCategoryRelation")]
        public async Task<IActionResponse<bool>> UpdateDocumentCategoryRelation(Guid documentId, Guid newCategoryId)
        {
            return await _documentsController.UpdateDocumentCategoryRelation(documentId, newCategoryId);
        }
        #endregion
    }
}
