namespace XperiCad.DigitalDrawingStore.BL.Documents
{
    public interface IDocumentWatermarkProvider
    {
        byte[] ApplyWatermarksOnDocument(string documentPath, IEnumerable<IDocumentWatermark> watermarks);
    }
}
