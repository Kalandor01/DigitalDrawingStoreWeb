using XperiCad.Common.Infrastructure.Behaviours.Queries;

namespace XperiCad.DigitalDrawingStore.BL.Services
{
    /// <summary>
    /// This interface is for handling feedback properties.
    /// </summary>
    public interface IFeedbackPropertiesService
    {
        Task<IPromise<string>> QuerySenderEmailAsync();
        Task<IPromise<string>> QueryEmailRecipitentsAsync();
        Task<IPromise<string>> QuerySmtpHostAsync();
        Task<IPromise<string>> QuerySmtpPortAsync();
        Task<IPromise<string>> QuerySmtpUsernameAsync();
        Task<IPromise<string>> QuerySmtpPasswordAsync();
        Task<IPromise<string>> QueryIsUseDefaultCredentialsAsync();
        Task<IPromise<string>> QueryIsEnableSslAsync();

        Task<bool> UpdateSenderEmailAsync(string propertyValue);
        Task<bool> UpdateEmailRecipientsAsync(string propertyValue);
        Task<bool> UpdateSmtpHostAsync(string propertyValue);
        Task<bool> UpdateSmtpPortAsync(string propertyValue);
        Task<bool> UpdateSmtpUsernameAsync(string propertyValue);
        Task<bool> UpdateSmtpPasswordAsync(string propertyValue);
        Task<bool> UpdateIsUseDefaultCredentialsAsync(string propertyValue);
        Task<bool> UpdateIsEnableSslAsync(string propertyValue);
    }
}
