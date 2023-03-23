namespace XperiCad.DigitalDrawingStore.Web.API.DTO
{
    public class FeedbackEntities
    {
        private static readonly char EMAIL_RECIPIENTS_JOIN_STRING = '\n';

        public string SenderEmail { get; }
        public string EmailRecipients { get; }
        public string SmtpHost { get; }
        public int SmtpPort { get; }
        public string SmtpUsername { get; }
        public string SmtpPassword { get; }
        public bool IsUseDefaultCredentials { get; }
        public bool IsUseSsl { get; }

        public FeedbackEntities(
            string senderEmail,
            IEnumerable<string> emailRecipient,
            string smtpHost,
            int smtpPort,
            string smtpUsername,
            string smtpPassword,
            bool isUseDefaultCredentials,
            bool isUseSsl
        )
        {
            SenderEmail = senderEmail ?? throw new ArgumentNullException(nameof(senderEmail));
            var emailRecipientsList = emailRecipient ?? throw new ArgumentNullException(nameof(emailRecipient));
            EmailRecipients = string.Join(EMAIL_RECIPIENTS_JOIN_STRING, emailRecipientsList);
            SmtpHost = smtpHost ?? throw new ArgumentNullException(nameof(smtpHost));
            SmtpPort = smtpPort;
            SmtpUsername = smtpUsername ?? throw new ArgumentNullException(nameof(smtpUsername));
            SmtpPassword = smtpPassword ?? throw new ArgumentNullException(nameof(smtpPassword));
            IsUseDefaultCredentials = isUseDefaultCredentials;
            IsUseSsl = isUseSsl;
        }
    }
}
