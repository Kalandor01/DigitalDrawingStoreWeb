using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.Web.API.DTO;

namespace XperiCad.DigitalDrawingStore.Web.API.Extensions
{
    public static class IActionResponseExtensions
    {
        // TODO: implement this in common project
        public static JsonResponse<T> ToJsonResponse<T>(this IActionResponse<T> actionResponse)
        {
            var feedbackMessages = new List<FeedbackMessage>();

            foreach (var feedbackMessage in actionResponse.FeedbackMessages)
            {
                feedbackMessages.Add(new FeedbackMessage(feedbackMessage.Severity, feedbackMessage.GetCulture().Value));
            }

            var result = new JsonResponse<T>(actionResponse.ResponseObject, feedbackMessages, actionResponse.IsOkay);
            return result;
        }
    }
}
