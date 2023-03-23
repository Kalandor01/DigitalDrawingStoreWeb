using XperiCad.DigitalDrawingStore.BL.Impl.Application;

namespace XperiCad.DigitalDrawingStore.BL.Application
{
    /// <summary>
    /// Gets the global properties of the application.
    /// </summary>
    public interface IApplicationProperties
    {
        /// <summary>
        /// Gets the document resource properties which is needed to reach the document resource.
        /// </summary>
        IDocumentResourceProperties DocumentResourceProperties { get; }

        /// <summary>
        /// Gets the user event logging properties which is needed to track the user events in the system.
        /// </summary>
        IUserEventLoggingProperties UserEventLoggingProperties { get; }
        
        /// <summary>
        /// Manages the application's feedback properties for feedback emails.
        /// </summary>
        IFeedbackProperties FeedbackProperties { get; }
    }
}
