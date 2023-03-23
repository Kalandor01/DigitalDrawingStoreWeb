namespace XperiCad.DigitalDrawingStore.BL.Application
{
    /// <summary>
    /// This interface is to get and set the connection properties for document resource.
    /// </summary>
    public interface IDocumentResourceProperties
    {
        /// <summary>
        /// The connection string that contains the <server>;<database>;<username>;<password> attributes.
        /// </summary>
        string ResourcePath { get; set; }
    }
}
