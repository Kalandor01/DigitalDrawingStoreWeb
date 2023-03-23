namespace XperiCad.DigitalDrawingStore.BL.Application
{
    public interface IUserEventLoggingProperties
    {
        Task<bool> GetIsLoggingEnabledAsync();
        Task SetIsLoggingEnabledAsync(bool isLoggingEnabled);

        Task<string> GetResourcePathAsync();
        Task<string> GetResourceDirectoryAsync();
    }
}
