namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class JsonResponse<T>
    {
        public T ResponseObject { get; }
        public IEnumerable<FeedbackMessage> FeedbackMessages { get; }
        public bool IsOkay { get; }

        public JsonResponse(T responseObject, IEnumerable<FeedbackMessage> feedbackMessages, bool isOkay)
        {
            ResponseObject = responseObject;
            FeedbackMessages = feedbackMessages ?? throw new ArgumentNullException(nameof(feedbackMessages));
            IsOkay = isOkay;
        }
    }
}
