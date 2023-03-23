using Unity;
using XperiCad.Common.Core.Core;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.Web.API.Commands;

namespace XperiCad.DigitalDrawingStore.Web.API.Test.Controllers
{
    public class AdministrationControllerTests
    {
        #region Constants
        private static readonly string MASTER_DATABASE_NAME = "master";
        #endregion

        #region Fields
        private static string _defaultDatabaseConnaectionString;
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(GetTestParametersForACT0011))]
        public async Task ACT0011_Given_Resource_Property_Values_When_DocumentResourceProperties_Then_Gets_Same_Values_Back(
            string databaseName,
            string documentDatabaseConnectionString
            )
        {
            SaveAppConfigFileDatabaseString();
            CreateDatabase(databaseName);
            try
            {
                var uddcsac = new UpdateDocumentDatabaseConnectionStringActionCommand(documentDatabaseConnectionString, Constants.TEST_APPLICATION_CONFIGURATION_FILE_PATH);
                await uddcsac.ExecuteAsync();

                var drps = GetDocumentResourceProperies();
                Assert.Equal(drps.ResourcePath, documentDatabaseConnectionString);
            }
            finally
            {
                DropDatabase(databaseName);
                await RestoreAppConfigFileDatabaseString();
            }
        }

        [Theory]
        [MemberData(nameof(GetTestParametersForACT0021))]
        public async Task ACT0021_Given_Incorrect_Database_Connection_String_When_DocumentResourceProperties_Then_Gets_Diferent_Value_Back(
            string databaseName, string documentDatabaseConnectionString
            )
        {
            SaveAppConfigFileDatabaseString();
            CreateDatabase(databaseName);
            try
            {
                var uddcsac = new UpdateDocumentDatabaseConnectionStringActionCommand(documentDatabaseConnectionString, Constants.TEST_APPLICATION_CONFIGURATION_FILE_PATH);
                await uddcsac.ExecuteAsync();

                var drps = GetDocumentResourceProperies();
                Assert.NotEqual(drps.ResourcePath, documentDatabaseConnectionString);
            }
            finally
            {
                DropDatabase(databaseName);
                await RestoreAppConfigFileDatabaseString();
            }
        }

        [Theory]
        [MemberData(nameof(GetTestParametersForACT0031))]
        public async Task ACT0031_Given_Feedback_Property_Values_When_FeedbackProperties_Then_Gets_Same_Values_Back(
            string senderEmail,
            IEnumerable<string> emailRecipients,
            string smtpHost,
            int smtpPort,
            string smtpUsername,
            string smtpPassword,
            bool isUseDefaultSmtpCredentials,
            bool isEnableSsl
            )
        {
            var dataSource = GetMsSqlDataSource();
            var testNamespace = CreateTestNamespace();
            CreateApplicationPropertiesTable(testNamespace, dataSource);
            try
            {
                var fpService = new FeedbackPropertiesServiceFactory().CreateFeedbackPropertyService(Constants.TEST_APPLICATION_CONFIGURATION_FILE_PATH);
                var feedbackProperies = new FeedbackProperties(fpService);
                await feedbackProperies.UpdateSenderEmailAsync(senderEmail);
                await feedbackProperies.UpdateEmailRecipientsAsync(emailRecipients);
                await feedbackProperies.UpdateSmtpHostAsync(smtpHost);
                await feedbackProperies.UpdateSmtpPortAsync(smtpPort);
                await feedbackProperies.UpdateSmtpUsernameAsync(smtpUsername);
                await feedbackProperies.UpdateSmtpPasswordAsync(smtpPassword);
                await feedbackProperies.UpdateIsUseDefaultCredentialsAsync(isUseDefaultSmtpCredentials);
                await feedbackProperies.UpdateIsEnableSslAsync(isEnableSsl);

                var nse = (await feedbackProperies.GetSenderEmailAsync()).ResponseObject;
                var ner = await feedbackProperies.GetEmailRecipientsAsync();
                var nsh = (await feedbackProperies.GetSmtpHostAsync()).ResponseObject;
                var nsp = await feedbackProperies.GetSmtpPortAsync();
                var nsu = (await feedbackProperies.GetSmtpUsernameAsync()).ResponseObject;
                var nsa = (await feedbackProperies.GetSmtpPasswordAsync()).ResponseObject;
                var niu = await feedbackProperies.GetIsUseDefaultCredentialsAsync();
                var nie = await feedbackProperies.GetIsEnableSslAsync();

                Assert.Equal(nse, senderEmail);
                Assert.Equal(ner, emailRecipients);
                Assert.Equal(nsh, smtpHost);
                Assert.Equal(nsp, smtpPort);
                Assert.Equal(nsu, smtpUsername);
                Assert.Equal(nsa, smtpPassword);
                Assert.Equal(niu, isUseDefaultSmtpCredentials);
                Assert.Equal(nie, isEnableSsl);
            }
            finally
            {
                DropApplicationPropertiesTable(testNamespace, dataSource);
            }
        }
        #endregion

        #region Private Members
        private IDocumentResourceProperties GetDocumentResourceProperies()
        {
            return new DocumentResourceProperiesFactory().CreateDocumentResourceProperties(Constants.TEST_APPLICATION_CONFIGURATION_FILE_PATH);
        }
        private static IDataSource GetMsSqlDataSource()
        {
            string connectionString;
            connectionString = Constants.TEST_DATABASE_CONNECTION_STRING;
            return GetMsSqlDataSource(connectionString);
        }
        private static IDataSource GetMsSqlDataSource(string connectionString)
        {
            var container = CommonCoreBootstrapper.ConfigureCommonCore(new string[] { Common.Core.Constants.Culture.CultureKeys.HUNGARIAN_HUNGARY }, Constants.TEST_NAMESPACE, true);

            return container
                .Resolve<IDataSourceFactory>()
                .CreateMsSqlDataSource(connectionString);
        }

        private static string GetDatabaseString(string databaseName)
        {
            return $"Server=(localdb)\\MSSQLLocalDB;Database={databaseName};Trusted_Connection=True;";
        }

        private static void CreateDatabase(string databaseName)
        {
            var dataSource = GetMsSqlDataSource(GetDatabaseString(MASTER_DATABASE_NAME));
            dataSource.PerformCommand(
                $"CREATE DATABASE [{databaseName}]"
            );
        }

        private static void DropDatabase(string databaseName)
        {
            var dataSource = GetMsSqlDataSource(GetDatabaseString(MASTER_DATABASE_NAME));
            dataSource.PerformCommand(
                $"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                $"DROP DATABASE [{databaseName}]"
            );
        }
        private static void CreateApplicationPropertiesTable(string testNamespace, IDataSource msSqlDataSource)
        {
            msSqlDataSource.PerformCommand(
                $"CREATE TABLE ApplicationProperties{testNamespace}"
                + $"("
                + $"    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),"
                + $"    PropertyKey VARCHAR(MAX),"
                + $"    PropertyValue VARCHAR(MAX)"
                + $");"
            );
        }

        private static void DropApplicationPropertiesTable(string testNamespace, IDataSource msSqlDataSource)
        {
            msSqlDataSource.PerformCommand(
                $"DROP TABLE ApplicationProperties{testNamespace};"
            );
        }
        protected static string CreateTestNamespace()
        {
            var id = Guid.NewGuid().ToString();
            var testNamespace = id.Replace("-", "");

            return testNamespace;
        }

        private async Task RestoreAppConfigFileDatabaseString()
        {
            var uddcsac = new UpdateDocumentDatabaseConnectionStringActionCommand(_defaultDatabaseConnaectionString, Constants.TEST_APPLICATION_CONFIGURATION_FILE_PATH);
            await uddcsac.ExecuteAsync();
        }

        private void SaveAppConfigFileDatabaseString()
        {
            _defaultDatabaseConnaectionString = GetDocumentResourceProperies().ResourcePath;
        }
        #endregion

        #region MemberData
        public static IEnumerable<object[]> GetTestParametersForACT0011()
        {
            yield return new object[] { "testd1", GetDatabaseString("testd1") };
            yield return new object[] { "testd1515", GetDatabaseString("testd1515") };
        }
        public static IEnumerable<object[]> GetTestParametersForACT0021()
        {
            yield return new object[] { "totaly real database", "totaly database string" };
            yield return new object[] { "nothing", "Server=(localdb)\\MSSQLLocalDB;Database=hmm;Trusted_Connection=True;" };
            yield return new object[] { "l", "Server=(localdlDB;Databam;Trusted_Connection=True;" };
        }
        public static IEnumerable<object[]> GetTestParametersForACT0031()
        {
            yield return new object[] { "sender", new List<string> { "rec1", "rec2", "rec3" }, "local", 1234, "me", "pass", true, false};
            yield return new object[] { "you.you@d", new List<string> { "hehehe", "nem" }, "h", 111111111, "j", "pess", true, true };
            yield return new object[] { "te", new List<string> { "monkey" }, "localhó", 1234, "hfghhf", "trz", false, false };
        }
        #endregion
    }
}
