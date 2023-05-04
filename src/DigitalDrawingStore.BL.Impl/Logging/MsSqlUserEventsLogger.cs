using System.Data;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Application;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Logging;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Logging
{
    internal class MsSqlUserEventsLogger : IUserEventsLogger
    {
        #region Constants
        private const int SQL_FLOAT_LENGTH = 4;
        private const string DOCUMENT_CREATED_EVENT_ID = "DocumentCreated";
        private const string ATTRIBUTE_CHANGED_EVENT_ID = "DocumentAttributeChanged";
        #endregion

        #region Fields
        private readonly IApplicationProperties _applicationProperties;
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        #endregion

        #region ctor
        public MsSqlUserEventsLogger(
            IApplicationProperties applicationProperties,
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames)
        {
            _applicationProperties = applicationProperties ?? throw new ArgumentNullException(nameof(applicationProperties));
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
        }
        #endregion

        #region IUserEventsLogger members
        public async Task LogDocumentEventAsync(
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
            IDocumentWatermark documentWatermark)
        {
            if (await GetIsLoggingEnabledAsync())
            {
                await InsertRecordIntoLogs(
                    eventId,
                    userDomainName,
                    machineNumber,
                    sourceIp,
                    documentPath,
                    documentVersion,
                    targetOfUsage,
                    documentDrawingNumber,
                    documentRevisionId,
                    documentTitle,
                    documentTypeOfProductionOnDrawing,
                    documentPrefix,
                    documentWatermark.Text,
                    $"Vertical: {documentWatermark.VerticalPosition} - Horizontal: {documentWatermark.HorizontalPosition}",
                    documentWatermark.FontSizeInPt,
                    documentWatermark.RotationInDegree,
                    null,
                    null,
                    null,
                    null,
                    null);
            }
        }

        public async Task LogDocumentCreateEventAsync(
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
            DateTime dateTimeOfDocumentApprove)
        {
            if (await GetIsLoggingEnabledAsync())
            {
                await InsertRecordIntoLogs(
                    DOCUMENT_CREATED_EVENT_ID,
                    userDomainName,
                    machineNumber,
                    sourceIp,
                    documentPath,
                    documentVersion,
                    targetOfUsage,
                    documentDrawingNumber,
                    documentRevisionId,
                    documentTitle,
                    documentTypeOfProductionOnDrawing,
                    documentPrefix,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    dateTimeOfDocumentCreation,
                    dateTimeOfDocumentApprove);
            }
        }

        public async Task LogDocumentAttributeChangeAsync(
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
            string newPropertyValue)
        {
            if (await GetIsLoggingEnabledAsync())
            {
                await InsertRecordIntoLogs(
                    ATTRIBUTE_CHANGED_EVENT_ID,
                    userDomainName,
                    machineNumber,
                    sourceIp,
                    documentPath,
                    documentVersion,
                    targetOfUsage,
                    documentDrawingNumber,
                    documentRevisionId,
                    documentTitle,
                    documentTypeOfProductionOnDrawing,
                    documentPrefix,
                    null,
                    null,
                    null,
                    null,
                    propertyName,
                    oldPropertyValue,
                    newPropertyValue,
                    null,
                    null);
            }
        }
        #endregion

        #region Private members
        private async Task<bool> GetIsLoggingEnabledAsync()
        {
            return await _applicationProperties.UserEventLoggingProperties.GetIsLoggingEnabledAsync();
        }

        private async Task InsertRecordIntoLogs(
            string eventId,
            string? userDomainName,
            string machineNumber,
            string sourceIp,
            string? documentPath,
            string? documentVersion,
            string? targetOfUsage,
            string? documentDrawingNumber,
            string? documentRevisionId,
            string? documentTitle,
            string? documentTypeOfProductionOnDrawing,
            string? documentPrefix,
            string? watermarkText,
            string? watermarkPosition,
            int? watermarkFontSizeInPt,
            float? watermarkRotationAngleInDegree,
            string? propertyName,
            string? oldPropertyValue,
            string? newProtpertyValue,
            DateTime? documentCreatedAt,
            DateTime? documentApprovedAt)
        {
            if (string.IsNullOrWhiteSpace(eventId))
            {
                throw new ArgumentException($"'{nameof(eventId)}' cannot be null or whitespace.", nameof(eventId));
            }

            if (string.IsNullOrWhiteSpace(machineNumber))
            {
                throw new ArgumentException($"'{nameof(machineNumber)}' cannot be null or whitespace.", nameof(machineNumber));
            }

            if (string.IsNullOrWhiteSpace(sourceIp))
            {
                throw new ArgumentException($"'{nameof(sourceIp)}' cannot be null or whitespace.", nameof(sourceIp));
            }

            const string NOT_APPLICABLE = "N/A";

            var sqlScript = $"   INSERT INTO {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.USER_EVENT_LOGS_TABLE_NAME_KEY]}"
                            + $" (Id, EventId, LoggedAt, UserDomainName, MachineNumber, SourceIp, DocumentPath, DocumentVersion,"
                            + $"   TargetOfUsage, DocumentDrawingNumber, DocumentRevId, DocumentTitle, DocumentTypeOfPRoductOnDrawing, DocumentPrefix,"
                            + $"   WatermarkText, WatermarkPosition, WatermarkFontSizeInPt, WatermarkRotationAngleInDegree,"
                            + $"   PropertyName, OldPropertyValue, NewPropertyValue, DocumentCreationAt, DocumentApprovedAt)"
                            + $" VALUES(@Id, @EventId, @LoggedAt, @UserDomainName, @MachineNumber, @SourceIp, @DocumentPath, @DocumentVersion,"
                            + $"   @TargetOfUsage, @DocumentDrawingNumber, @DocumentRevId, @DocumentTitle, @DocumentTypeOfPRoductOnDrawing, @DocumentPrefix,"
                            + $"   @WatermarkText, @WatermarkPosition, @WatermarkFontSizeInPt, @WatermarkRotationAngleInDegree,"
                            + $"   @PropertyName, @OldPropertyValue, @NewPropertyValue, @DocumentCreationAt, @DocumentApprovedAt)";

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@Id", SqlDbType.UniqueIdentifier, Guid.NewGuid())
                                .ConfigureParameter("@EventId", SqlDbType.NVarChar, eventId, -1)
                                .ConfigureParameter("@LoggedAt", SqlDbType.DateTime2, DateTime.UtcNow, 27)
                                .ConfigureParameter("@UserDomainName", SqlDbType.NVarChar, userDomainName ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@MachineNumber", SqlDbType.NVarChar, machineNumber, -1)
                                .ConfigureParameter("@SourceIp", SqlDbType.NVarChar, sourceIp, -1)
                                .ConfigureParameter("@DocumentPath", SqlDbType.NVarChar, documentPath ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@DocumentVersion", SqlDbType.NVarChar, documentVersion ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@TargetOfUsage", SqlDbType.NVarChar, targetOfUsage ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@DocumentDrawingNumber", SqlDbType.NVarChar, documentDrawingNumber ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@DocumentRevId", SqlDbType.NVarChar, documentRevisionId ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@DocumentTitle", SqlDbType.NVarChar, documentTitle ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@DocumentTypeOfPRoductOnDrawing", SqlDbType.NVarChar, documentTypeOfProductionOnDrawing ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@DocumentPrefix", SqlDbType.NVarChar, documentPrefix ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@WatermarkText", SqlDbType.NVarChar, watermarkText ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@WatermarkPosition", SqlDbType.NVarChar, watermarkPosition ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@WatermarkFontSizeInPt", SqlDbType.Int, watermarkFontSizeInPt ?? 0, SqlTypeLengthConstants.INT_MAX_LENGTH)
                                .ConfigureParameter("@WatermarkRotationAngleInDegree", SqlDbType.Float, watermarkRotationAngleInDegree, SQL_FLOAT_LENGTH)
                                .ConfigureParameter("@PropertyName", SqlDbType.NVarChar, propertyName ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@OldPropertyValue", SqlDbType.NVarChar, oldPropertyValue ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@NewPropertyValue", SqlDbType.NVarChar, newProtpertyValue ?? NOT_APPLICABLE, -1)
                                .ConfigureParameter("@DocumentCreationAt", SqlDbType.DateTime2, documentCreatedAt ?? DateTime.MinValue, 27)
                                .ConfigureParameter("@DocumentApprovedAt", SqlDbType.DateTime2, documentApprovedAt ?? DateTime.MinValue, 27)
                                .GetConfiguredParameters();

            _ = await _msSqlDataSource.PerformCommandAsync(sqlScript, parameters);
        }
        #endregion
    }
}
