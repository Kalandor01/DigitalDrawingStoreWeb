using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Application
{
    /// <summary>
    /// Gets the global properties of the application.
    /// </summary>
    internal interface IApplicationProperties
    {
        /// <summary>
        /// Gets the document resource properties which is needed to reach the document resource.
        /// </summary>
        IDocumentResourceProperies DocumentResourceProperies { get; }
        IEnumerable<string> ObservedLocations { get; }
        string DocumentDropDirectory { get; }
        IFeedbackProperties FeedbackProperties { get; }
    }
}
