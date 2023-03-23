namespace XperiCad.DigitalDrawingStore.BL.Documents.Factories
{
    public interface IDocumentWatermarkFactory
    {
        IDocumentWatermark CreateWatermark(
            string text, 
            int fontSize, 
            float opacity, 
            int rotation, 
            WatermarkVerticalPosition verticalPosition, 
            WatermarkHorizontalPosition horizontalPosition);

        IDocumentWatermark CreateWatermark(
            string text,
            int fontSize,
            float opacity,
            int rotation,
            int offsetX,
            int offsetY,
            WatermarkVerticalPosition verticalPosition,
            WatermarkHorizontalPosition horizontalPosition);
    }
}
