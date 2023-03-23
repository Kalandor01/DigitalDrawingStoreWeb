using XperiCad.Common.Infrastructure.Feedback;

namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class FeedbackMessage
    {
        public Severity Severity { get; }
        public string Message { get; }

        public FeedbackMessage(Severity severity, string message)
        {
            Severity = severity;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
