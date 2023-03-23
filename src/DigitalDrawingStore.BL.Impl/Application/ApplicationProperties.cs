using XperiCad.Common.Infrastructure.Application;
using XperiCad.DigitalDrawingStore.BL.Application;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application
{
    internal class ApplicationProperties : IApplicationProperties
    {
        #region Properties
        public ICommonApplicationProperties CommonApplicationProperties { get; }
        public IDocumentResourceProperties DocumentResourceProperties { get; }
        public IUserEventLoggingProperties UserEventLoggingProperties { get; }
        public IFeedbackProperties FeedbackProperties { get; }
        #endregion

        #region Constructor
        public ApplicationProperties(
            ICommonApplicationProperties commonApplicationProperties,
            IDocumentResourceProperties documentResourceProperies,
            IUserEventLoggingProperties userEventLoggingProperties,
            IFeedbackProperties feedbackProperties)
        {
            CommonApplicationProperties = commonApplicationProperties ?? throw new ArgumentNullException(nameof(commonApplicationProperties));
            DocumentResourceProperties = documentResourceProperies ?? throw new ArgumentNullException(nameof(documentResourceProperies));
            UserEventLoggingProperties = userEventLoggingProperties ?? throw new ArgumentNullException(nameof(userEventLoggingProperties));
        	FeedbackProperties = feedbackProperties ?? throw new ArgumentNullException(nameof(feedbackProperties));
        }
        #endregion
    }
}
