namespace XperiCad.DigitalDrawingStore.BL.Documents.Exporters
{
    /// <summary>
    /// This interface is for exporting documents.
    /// </summary>
    public interface IDocumentExporter
    {
        /// <summary>
        /// Exports the documents to a target path by it's watermark.
        /// </summary>
        public void Export(string targetPath, IDocumentWatermark documentWatermark);
    }
}
