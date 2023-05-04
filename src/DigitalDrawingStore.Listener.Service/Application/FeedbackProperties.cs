using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Infrastructure.DataSource;

namespace DigitalDrawingStore.Listener.Service.Application
{
    internal class FeedbackProperties : IFeedbackProperties
    {
        #region Properties
        public string SenderEmail => GetSenderEmail();
        public IEnumerable<string> EmailRecipients => GetEmailRecipitents();
        public string SmtpHost => GetSmtpAddress();
        public int SmtpPort => GetSmtpPort();
        public string SmtpUsername => GetSmtpUsername();
        public string SmtpPassword => GetSmtpPassword();
        public bool IsUseDefaultCredentials => GetIsUseDefaultCredentials();
        public bool IsEnableSsl => GetIsEnableSsl();
        #endregion

        #region Fields
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDataSource _msSqlDataSource;
        private readonly IDictionary<string, string> _sqlTableNames;
        #endregion

        #region ctor
        public FeedbackProperties(
            IDataParameterFactory dataParameterFactory,
            IDataSource msSqlDataSource,
            IDictionary<string, string> sqlTableNames)
        {
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
        }
        #endregion

        #region Private members
        private string GetSenderEmail()
        {
            return GetProperty("SenderEmail");
        }

        private IEnumerable<string> GetEmailRecipitents()
        {
            var emailRecipientsProperty = GetProperty("EmailRecipients");
            if (emailRecipientsProperty != null)
            {
                var emailRecipients = emailRecipientsProperty.Split(';');

                emailRecipients = emailRecipients.Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();

                return emailRecipients;
            }

            return null;
        }

        private string GetSmtpAddress()
        {
            return GetProperty("SmtpHost");
        }

        private int GetSmtpPort()
        {
            var smtpPortProperty = GetProperty("SmtpPort");
            if (smtpPortProperty != null)
            {
                var isSuccessfullyParsed = int.TryParse(smtpPortProperty, out int port);
                if (isSuccessfullyParsed)
                {
                    return port;
                }
            }

            return default;
        }

        private string GetSmtpUsername()
        {
            return GetProperty("SmtpUsername");
        }

        private string GetSmtpPassword()
        {
            return GetProperty("SmtpPassword");
        }

        private bool GetIsUseDefaultCredentials()
        {
            var isUseDefaultSmtpCredentialsProperty = GetProperty("UseDefaultSmtpCredentials");
            if (isUseDefaultSmtpCredentialsProperty != null)
            {
                var isSuccessfullyParsed = bool.TryParse(isUseDefaultSmtpCredentialsProperty, out bool isUseDefaultSmtpCredentials);
                if (isSuccessfullyParsed)
                {
                    return isUseDefaultSmtpCredentials;
                }
            }

            return default;
        }

        private bool GetIsEnableSsl()
        {
            var isEnableSslProperty = GetProperty("EnableSmtpSsl");
            if (isEnableSslProperty != null)
            {
                var isSuccessfullyParsed = bool.TryParse(isEnableSslProperty, out bool isEnableSsl);
                if (isSuccessfullyParsed)
                {
                    return isEnableSsl;
                }
            }

            return default;
        }

        private string GetProperty(string propertyKey)
        {
            var parameters = _dataParameterFactory
                               .ConfigureParameter("@PropertyKey", SqlDbType.NVarChar, propertyKey, -1)
                               .GetConfiguredParameters();

            var sqlScript = $"SELECT Id, PropertyValue"
                + $" FROM {_sqlTableNames[Constants.DocumentDatabase.APPLICATION_PROPERTIES_TABLE_NAME_KEY]}"
                + $" WHERE PropertyKey = @PropertyKey";

            var response = _msSqlDataSource.PerformQuery(sqlScript, parameters, "PropertyValue");
            var property = response.FirstOrDefault();

            if (property != null)
            {
                return property.Attributes["PropertyValue"].ToString();
            }

            return null;
        }
        #endregion
    }
}
