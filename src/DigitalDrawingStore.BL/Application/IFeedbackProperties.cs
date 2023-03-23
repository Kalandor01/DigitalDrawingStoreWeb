using XperiCad.Common.Infrastructure.Behaviours.Queries;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application
{
    public interface IFeedbackProperties
    {
        Task<string> GetSenderEmailAsync();
        Task<IEnumerable<string>> GetEmailRecipientsAsync();
        Task<string> GetSmtpHostAsync();
        Task<int> GetSmtpPortAsync();
        Task<string> GetSmtpUsernameAsync();
        Task<string> GetSmtpPasswordAsync(); // TODO: secure string + encoding
        Task<bool> GetIsUseDefaultCredentialsAsync();
        Task<bool> GetIsEnableSslAsync();
        Task<bool> UpdateSenderEmailAsync(string senderEmail);
        Task<bool> UpdateEmailRecipientsAsync(IEnumerable<string> emailRecipients);
        Task<bool> UpdateSmtpHostAsync(string smtpHost);
        Task<bool> UpdateSmtpPortAsync(int smtpPort);
        Task<bool> UpdateSmtpUsernameAsync(string smtpUsername);
        Task<bool> UpdateSmtpPasswordAsync(string smtpPassword); // TODO: secure string + encoding
        Task<bool> UpdateIsUseDefaultCredentialsAsync(bool isUseDefaultCredentials);
        Task<bool> UpdateIsEnableSslAsync(bool isEnableSsl);
    }
}
