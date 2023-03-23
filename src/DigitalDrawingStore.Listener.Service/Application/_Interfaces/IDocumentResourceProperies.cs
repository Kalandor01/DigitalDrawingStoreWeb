namespace DigitalDrawingStore.Listener.Service.Application
{
    /// <summary>
    /// This interface is for get the connection properties for document resource.
    /// </summary>
    public interface IDocumentResourceProperies
    {
        /// <summary>
        /// Gets the connection string that contains the <server>;<database>;<username>;<password> attributes.
        /// </summary>
        string ResourcePath { get; }
    }
}
