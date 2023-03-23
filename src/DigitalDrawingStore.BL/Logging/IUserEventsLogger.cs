using XperiCad.DigitalDrawingStore.BL.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Logging
{
    public interface IUserEventsLogger
    {
        Task LogDocumentEventAsync(
            string eventId,
            string userDomainName,
            string machineNumber,
            string sourceIp,
            string documentPath,
            string documentVersion,
            string targetOfUsage,
            string documentDrawingNumber,
            string documentRevisionId,
            string documentTitle,
            string documentTypeOfProductionOnDrawing,
            string documentPrefix,
            IDocumentWatermark documentWatermark);

        Task LogDocumentCreateEventAsync(
            string userDomainName,
            string machineNumber,
            string sourceIp,
            string documentPath,
            string documentVersion,
            string targetOfUsage,
            string documentDrawingNumber,
            string documentRevisionId,
            string documentTitle,
            string documentTypeOfProductionOnDrawing,
            string documentPrefix,
            DateTime dateTimeOfDocumentCreation,
            DateTime dateTimeOfDocumentApprove);

        Task LogDocumentAttributeChangeAsync(
            string userDomainName,
            string machineNumber,
            string sourceIp,
            string documentPath,
            string documentVersion,
            string targetOfUsage,
            string documentDrawingNumber,
            string documentRevisionId,
            string documentTitle,
            string documentTypeOfProductionOnDrawing,
            string documentPrefix,
            string propertyName,
            string oldPropertyValue,
            string newPropertyValue);
    }
}
