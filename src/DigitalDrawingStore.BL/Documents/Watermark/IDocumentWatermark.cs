namespace XperiCad.DigitalDrawingStore.BL.Documents
{
    /// <summary>
    /// This interface represents the watermark on a document.
    /// </summary>
    public interface IDocumentWatermark
    {
        string Text { get; }
        int FontSizeInPt { get; }
        float OpacityInPercentage { get; }
        int RotationInDegree { get; }
        int OffsetX { get; }
        int OffsetY { get; }
        WatermarkVerticalPosition VerticalPosition { get; }
        WatermarkHorizontalPosition HorizontalPosition { get; }
    }
}
