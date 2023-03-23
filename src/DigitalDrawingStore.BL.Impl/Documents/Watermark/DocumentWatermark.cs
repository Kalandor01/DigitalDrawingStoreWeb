using XperiCad.DigitalDrawingStore.BL.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Watermark
{
    internal class DocumentWatermark : IDocumentWatermark
    {
        #region Properties
        public string Text { get; }
        public int FontSizeInPt { get; }
        public float OpacityInPercentage { get; }
        public int RotationInDegree { get; }
        public int OffsetX { get; }
        public int OffsetY { get; }
        public WatermarkVerticalPosition VerticalPosition { get; }
        public WatermarkHorizontalPosition HorizontalPosition { get; }
        #endregion

        #region ctor
        public DocumentWatermark(
            string text,
            int fontSizeInPt,
            float opacityInPercentage,
            int rotationInDegree,
            int offsetX,
            int offsetY,
            WatermarkVerticalPosition verticalPosition,
            WatermarkHorizontalPosition horizontalPosition)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text), "Watermark text could not be null or empty.");
            }

            if (fontSizeInPt <= 0)
            {
                throw new ArgumentNullException(nameof(rotationInDegree), "Watermark font could not be less then or equal to 0.");
            }

            Text = text;
            FontSizeInPt = fontSizeInPt;
            OpacityInPercentage = opacityInPercentage;
            RotationInDegree = rotationInDegree;
            OffsetX = offsetX;
            OffsetY = offsetY;
            VerticalPosition = verticalPosition;
            HorizontalPosition = horizontalPosition;
        }
        #endregion
    }
}
