using XperiCad.DigitalDrawingStore.BL.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories
{
    /// <summary>
    /// This interface helps to convert to an IDocument data type.
    /// </summary>
    internal interface IDocumentFactory
    {
        /// <summary>
        /// Creates an IDocument data type from a given path.
        /// </summary>
        IDocument CreatePdfDocument(Guid id, string path);
    }
}
