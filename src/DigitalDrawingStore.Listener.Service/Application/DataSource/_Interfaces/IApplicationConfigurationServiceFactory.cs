namespace XperiCad.Common.Infrastructure.Application.DataSource
{
    /// <summary>
    /// Provides an interface to create an application configuration service with specific configuration path.
    /// </summary>
    public interface IApplicationConfigurationServiceFactory
    {
        /// <summary>
        /// Create a new IApplicationConfigurationService object which works with the specified configuration path.
        /// </summary>
        /// <param name="configurationPath">The target path where the IApplicationConfigurationService should work.</param>
        /// <returns>IApplicationConfigurationService which works with the target path.</returns>
        IApplicationConfigurationService CreateApplicationConfigurationService(string configurationPath);
    }
}
