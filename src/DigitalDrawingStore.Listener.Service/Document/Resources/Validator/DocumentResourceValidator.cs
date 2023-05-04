using DigitalDrawingStore.Listener.Service.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Infrastructure.DataSource;

namespace DigitalDrawingStore.Listener.Service.Document.Resources.Validator
{
    internal class DocumentResourceValidator : IDocumentResource
    {
        #region Fields
        private readonly IDocumentResource _documentResource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDataSource _msSqlDataSource;
        private readonly IDictionary<string, string> _sqlTableNames;
        private readonly IApplicationProperties _applicationProperties;
        #endregion

        #region Constructor
        public DocumentResourceValidator(
            IDocumentResource documentResource,
            IDataParameterFactory dataParameterFactory,
            IDataSource msSqlDataSource,
            IDictionary<string, string> sqlTableNames,
            IApplicationProperties applicationProperties)
        {
            _documentResource = documentResource ?? throw new ArgumentNullException(nameof(documentResource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            _applicationProperties = applicationProperties ?? throw new ArgumentNullException(nameof(applicationProperties));
        }
        #endregion

        #region IDocumentResource members
        public void SaveDocuments(IEnumerable<IRawDocument> rawDocuments)
        {
            var documentsToSaveInDataSource = new List<IRawDocument>();
            var feedbackQueue = new List<ValidationFeedback>();

            foreach (var rawDocument in rawDocuments)
            {
                var documentName = Path.GetFileName(rawDocument.DocumentData.DocumentPath);

                if (string.IsNullOrWhiteSpace(documentName))
                {
                    feedbackQueue.Add(new ValidationFeedback(rawDocument, $"Ez az útvonal nem tartalmaz fájlnevet: {rawDocument.DocumentData.DocumentPath}."));
                    continue;
                }

                if (IsDocumentExistsInDatabase(documentName))
                {
                    feedbackQueue.Add(new ValidationFeedback(rawDocument, $"Ez a dokumentum már létezik az adatbázisban: {rawDocument.DocumentData.DocumentPath}."));
                    continue;
                }

                documentsToSaveInDataSource.Add(rawDocument);
            }

            _documentResource.SaveDocuments(documentsToSaveInDataSource);
            TrySendFeedback(feedbackQueue);
        }
        #endregion

        #region Private members
        private bool IsDocumentExistsInDatabase(string documentName)
        {
            var parameters = _dataParameterFactory
                                .ConfigureParameter("@Path", SqlDbType.NVarChar, documentName, -1)
                                .GetConfiguredParameters();

            var sqlScript = $"SELECT Id"
                + $" FROM {_sqlTableNames[Constants.DocumentDatabase.DOCUMENTS_TABLE_NAME_KEY]}"
                + $" WHERE Path LIKE CONCAT('%', @Path, '%')";

            var queryResult = _msSqlDataSource.PerformQuery(sqlScript, parameters);

            return queryResult.Count() > 0;
        }

        private void TrySendFeedback(IEnumerable<ValidationFeedback> validationFeedbacks)
        {
            try
            {
                if (validationFeedbacks != null && validationFeedbacks.Count() > 0)
                {
                    SendFeedbackEmail(validationFeedbacks);
                }
            }
            catch (Exception)
            {
                // TODO: log
            }
        }

        private void SendFeedbackEmail(IEnumerable<ValidationFeedback> validationFeedbacks)
        {
            const string EMAIL_SUBJECT = "Hibás dokumentumok találhatóak a megfigyelt útvonalakon";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient()
            {
                Host = _applicationProperties.FeedbackProperties.SmtpHost,
                Port = _applicationProperties.FeedbackProperties.SmtpPort,
                UseDefaultCredentials = _applicationProperties.FeedbackProperties.IsUseDefaultCredentials,
                Credentials = new NetworkCredential(_applicationProperties.FeedbackProperties.SmtpUsername, _applicationProperties.FeedbackProperties.SmtpPassword),
                EnableSsl = _applicationProperties.FeedbackProperties.IsEnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

            mailMessage.From = new MailAddress(_applicationProperties.FeedbackProperties.SenderEmail);

            var recipients = _applicationProperties.FeedbackProperties.EmailRecipients;
            foreach (var recipient in recipients)
            {
                mailMessage.To.Add(recipient);
            }

            mailMessage.Subject = EMAIL_SUBJECT;
            mailMessage.IsBodyHtml = true;

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"<h2>{validationFeedbacks.Count()} db hibás dokumentum található a következő megfigyelt útvonalakon.</h2>");

            messageBuilder.AppendLine($"<h3>Megfigyelt útvonalak:</h3>");
            foreach (var observedLocation in _applicationProperties.ObservedLocations)
            {
                messageBuilder.AppendLine($"{observedLocation}<br/>");
            }

            messageBuilder.AppendLine($"<h3>A következő hibák történtek a dokumentumok adatbázisba való mentésekor:</h3>");
            foreach (var validationFeedback in validationFeedbacks)
            {
                messageBuilder.AppendLine($"{validationFeedback.Message}<br/>");
            }

            messageBuilder.AppendLine($"<h2>Javaslatok:</h2>");
            messageBuilder.AppendLine($"- kérjük vegye fel a kapcsolatot a rendszer adminisztrátorral a megfelelő beállítások rögzítéséhez");
            messageBuilder.AppendLine($"<br/>- ha hibás működést vélt felfedezni, akkor kérjük továbbítsa az információt a fejlesztői kapcsolattartók számára, hogy növelni tudjuk a felhasználói élményt");

            mailMessage.Body = messageBuilder.ToString();
            smtpClient.Send(mailMessage);
        }
        #endregion
    }
}
