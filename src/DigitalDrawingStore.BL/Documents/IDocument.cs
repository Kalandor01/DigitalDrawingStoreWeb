namespace XperiCad.DigitalDrawingStore.BL.Documents
{
    /// <summary>
    /// This interface collects document properties.
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// Gets the document id in resource.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the path of the document.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the directory where the document is located.
        /// </summary>
        string Directory { get; }

        /// <summary>
        /// Gets the extension of the document.
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Gets the name of the document.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the name of the document with the extension.
        /// </summary>
        string NameWithExtension { get; }

        /// <summary>
        /// Exports the document to the target path with a watermark.
        /// </summary>
        void ExportAs(string targetPath, IDocumentWatermark documentWatermark);

        /// <summary>
        /// Puts on the watermark before download, then returns the filepath.
        /// </summary>
        byte[] Download(IEnumerable<IDocumentWatermark> watermarks);

        /// <summary>
        /// Gets a document attribute from document resource.
        /// </summary>
        /// <param name="attribute">The document attribute if exists, otherwise null.</param>
        Task<T?> GetAttribute<T>(string attribute);

        /// <summary>
        /// Sets a document attribute in resource if the attribute exists.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <param name="value">The value of attribute.</param>
        Task SetAttribute<T>(string attribute, T value);
    }
}
