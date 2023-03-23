using Moq;
using Unity;
using XperiCad.Common.Core.Core;
using XperiCad.Common.Infrastructure.Application.DataSource;
using XperiCad.Common.Infrastructure.Behaviours.Queries;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Application;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Logging;
using XperiCad.DigitalDrawingStore.BL.Impl.Watermark;
using XperiCad.DigitalDrawingStore.BL.Logging;

namespace XperiCad.DigitalDrawingStore.BL.Test.Logging
{
    public class MsSqlUserEventsLoggerTests
    {
        #region Tests

        public static IEnumerable<object[]> MSUEL0011_GetParameterts()
        {
            yield return new object[] { Impl.Constants.Documents.DocumentEventId.Open, "domain\\TestUser", "WORK-PC", "127.0.0.1", "C:\\Somewhere\\Document.pdf", "V1.0.0.0", "Review", "RJ45856", "12376", "Screw", "Type0", "A", new DocumentWatermark("Hello world", 18, 80F, 45, 10, 10, WatermarkVerticalPosition.Center, WatermarkHorizontalPosition.Center) };
            yield return new object[] { Impl.Constants.Documents.DocumentEventId.Download, "domain\\TestUser", "WORK-PC", "127.0.0.1", "C:\\Somewhere\\Document.pdf", "V1.0.0.0", "Review", "RJ45856", "12376", "Screw", "Type0", "A", new DocumentWatermark("Hello world", 18, 80F, 45, 10, 10, WatermarkVerticalPosition.Center, WatermarkHorizontalPosition.Center) };
            yield return new object[] { Impl.Constants.Documents.DocumentEventId.Print, "domain\\TestUser", "WORK-PC", "127.0.0.1", "C:\\Somewhere\\Document.pdf", "V1.0.0.0", "Review", "RJ45856", "12376", "Screw", "Type0", "A", new DocumentWatermark("Hello world", 18, 80F, 45, 10, 10, WatermarkVerticalPosition.Center, WatermarkHorizontalPosition.Center) };
        }
        [Theory]
        [MemberData(nameof(MSUEL0011_GetParameterts))]
        public async Task MSUEL0011_Given_ValidArguments_When_LogDocumentEventAsync_Then_LogsEntryToDatabase(
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
            var testContextId = Guid.NewGuid();

            try
            {
                await PrepareTestEnvironmentAsync(testContextId);
                var userEventsLogger = CreateUserEventsLogger(testContextId);

                await userEventsLogger.LogDocumentEventAsync(
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
                    documentWatermark);

                var insertedRecords = await QueryInsertedRecords(testContextId);

                Assert.True(insertedRecords.IsOkay);

                foreach (var record in insertedRecords.ResponseObject)
                {
                    Assert.Equal(eventId, record.Attributes["EventId"]);
                    Assert.Equal(userDomainName, record.Attributes["UserDomainName"]);
                    Assert.Equal(machineNumber, record.Attributes["MachineNumber"]);
                    Assert.Equal(sourceIp, record.Attributes["SourceIp"]);
                    Assert.Equal(documentPath, record.Attributes["DocumentPath"]);
                    Assert.Equal(documentVersion, record.Attributes["DocumentVersion"]);
                    Assert.Equal(targetOfUsage, record.Attributes["TargetOfUsage"]);
                    Assert.Equal(documentDrawingNumber, record.Attributes["DocumentDrawingNumber"]);
                    Assert.Equal(documentRevisionId, record.Attributes["DocumentRevId"]);
                    Assert.Equal(documentTitle, record.Attributes["DocumentTitle"]);
                    Assert.Equal(documentTypeOfProductionOnDrawing, record.Attributes["DocumentTypeOfPRoductOnDrawing"]);
                    Assert.Equal(documentPrefix, record.Attributes["DocumentPrefix"]);
                    Assert.Equal(documentWatermark.Text, record.Attributes["WatermarkText"]);
                    Assert.Equal("Vertical: Center - Horizontal: Center", record.Attributes["WatermarkPosition"]);
                    Assert.Equal(documentWatermark.FontSizeInPt, int.Parse(record.Attributes["WatermarkFontSizeInPt"].ToString()));
                    Assert.Equal(documentWatermark.RotationInDegree, int.Parse(record.Attributes["WatermarkRotationAngleInDegree"].ToString()));
                    Assert.Equal("N/A", record.Attributes["PropertyName"]);
                    Assert.Equal("N/A", record.Attributes["OldPropertyValue"]);
                    Assert.Equal("N/A", record.Attributes["NewPropertyValue"]);
                    Assert.Equal(DateTime.Parse("0001-01-01T00:00:00.0000000"), DateTime.Parse(record.Attributes["DocumentCreationAt"].ToString()));
                    Assert.Equal(DateTime.Parse("0001-01-01T00:00:00.0000000"), DateTime.Parse(record.Attributes["DocumentApprovedAt"].ToString()));
                }
            }
            finally
            {
                await DisposeTestEnvironment(testContextId);
            }
        }

        // TODO: create tests for all methods
        #endregion

        #region Private members
        private async Task PrepareTestEnvironmentAsync(Guid testContextId)
        {
            var testContext = testContextId.ToString().Replace("-", "");

            var sqlScript = $"CREATE TABLE UserEventLogsTest{testContext}"
                            + $"("
                                + $" Id UNIQUEIDENTIFIER NOT NULL,"
                                + $" EventId VARCHAR(max) NOT NULL,"
                                + $" LoggedAt DateTime2 NOT NULL,"

                                + $" UserDomainName VARCHAR(max) NOT NULL,"
                                + $" MachineNumber VARCHAR(max) NOT NULL,"
                                + $" SourceIp VARCHAR(max) NOT NULL,"
                                + $" DocumentPath VARCHAR(max) NULL,"
                                + $" DocumentVersion VARCHAR(max) NULL,"
                                + $" TargetOfUsage VARCHAR(max) NULL,"
                                + $" DocumentDrawingNumber VARCHAR(max) NULL,"
                                + $" DocumentRevId VARCHAR(max) NULL,"
                                + $" DocumentTitle VARCHAR(max) NULL,"
                                + $" DocumentTypeOfPRoductOnDrawing VARCHAR(max) NULL,"
                                + $" DocumentPrefix VARCHAR(max) NULL,"
                                + $" WatermarkText VARCHAR(max) NULL,"
                                + $" WatermarkPosition VARCHAR(max) NULL,"
                                + $" WatermarkFontSizeInPt VARCHAR(max) NULL,"
                                + $" WatermarkRotationAngleInDegree VARCHAR(max) NULL,"
                                + $" PropertyName VARCHAR(max) NULL,"
                                + $" OldPropertyValue VARCHAR(max) NULL,"
                                + $" NewPropertyValue VARCHAR(max) NULL,"
                                + $" DocumentCreationAt DateTime2 NULL,"
                                + $" DocumentApprovedAt DateTime2 NULL"
                                + $" PRIMARY KEY(Id)"
                            + $");"
                            + $"CREATE TABLE ApplicationPropertiesTest{testContext}"
                            + $"("
                                + $"Id UNIQUEIDENTIFIER NOT NULL,"
                                + $"PropertyKey VARCHAR(MAX) NULL,"
                                + $"PropertyValue VARCHAR(MAX) NULL,"
                                + $"PRIMARY KEY(Id)"
                            + $");"
                            + $"INSERT INTO ApplicationPropertiesTest{testContext} (Id, PropertyKey, PropertyValue) VALUES(NEWID(), 'IsLoggingEnabled', 'True')";

            var dataSource = CreateSqlDataSource();
            _ = await dataSource.PerformCommandAsync(sqlScript);
        }

        private async Task DisposeTestEnvironment(Guid testContextId)
        {
            var testContext = testContextId.ToString().Replace("-", "");

            var sqlScript = $"  DROP TABLE UserEventLogsTest{testContext};"
                            + $"DROP TABLE ApplicationPropertiesTest{testContext}";

            var dataSource = CreateSqlDataSource();
            _ = await dataSource.PerformCommandAsync(sqlScript);
        }

        private async Task<IPromise<IEnumerable<IEntity>>> QueryInsertedRecords(Guid testContextId)
        {
            var testContext = testContextId.ToString().Replace("-", "");

            var sqlScript = $"SELECT * FROM UserEventLogsTest{testContext}";

            var dataSource = CreateSqlDataSource();
            return await dataSource.PerformQueryAsync(sqlScript, "Id", "EventId", "LoggedAt", "UserDomainName", "MachineNumber", "SourceIp", "DocumentPath", "DocumentVersion", "TargetOfUsage", "DocumentDrawingNumber", "DocumentRevId", "DocumentTitle", "DocumentTypeOfPRoductOnDrawing", "DocumentPrefix", "WatermarkText", "WatermarkPosition", "WatermarkFontSizeInPt", "WatermarkRotationAngleInDegree", "PropertyName", "OldPropertyValue", "NewPropertyValue", "DocumentCreationAt", "DocumentApprovedAt");
        }

        private IUserEventsLogger CreateUserEventsLogger(Guid testContextId)
        {
            var testContext = testContextId.ToString().Replace("-", "");

            var coreModule = CommonCoreBootstrapper.ConfigureCommonCore(new string[] { "hu-HU" }, Constants.TEST_NAMESPACE, true);
            var applicationConfigurationServiceFactory = coreModule.Resolve<IApplicationConfigurationServiceFactory>();
            var applicationConfigurationService = applicationConfigurationServiceFactory.CreateApplicationConfigurationService(@"G:\XperiCad\WS\DigitalDrawingStoreWeb\src\Tests\DigitalDrawingStore.BL.Test\Preferences\TestApplicationConfiguration.xml");
            var dataSource = CreateSqlDataSource();
            var dataParameterFactory = coreModule.Resolve<IDataParameterFactory>();
            var sqlTableNames = new Dictionary<string, string>()
            {
                { Impl.Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_TABLE_NAME_KEY, $"ApplicationPropertiesTest{testContext}" },
                { Impl.Constants.Documents.Resources.DatabaseTables.USER_EVENT_LOGS_TABLE_NAME_KEY, $"UserEventLogsTest{testContext}" },
            };
            var userEventLoggingProperties = new UserEventLoggingProperties(applicationConfigurationService, dataSource, dataParameterFactory, sqlTableNames);
            var apllicationPropertiesMock = new Mock<IApplicationProperties>();
            apllicationPropertiesMock.Setup(e => e.UserEventLoggingProperties).Returns(userEventLoggingProperties);

            var userEventLogger = new MsSqlUserEventsLogger(apllicationPropertiesMock.Object, dataSource, dataParameterFactory, sqlTableNames);
            return userEventLogger;
        }

        private IDataSource CreateSqlDataSource()
        {
            var coreModule = CommonCoreBootstrapper.ConfigureCommonCore(new string[] { "hu-HU" }, Constants.TEST_NAMESPACE, true);
            var dataSourceFactory = coreModule.Resolve<IDataSourceFactory>();
            var dataSource = dataSourceFactory.CreateMsSqlDataSource(Constants.TEST_DATABASE_CONNECTION_STRING);

            return dataSource;
        }
        #endregion
    }
}
