namespace XperiCad.Common.Infrastructure.Application.DataSource
{
    /// <summary>
    /// Provides read and write functions for configuration resource.
    /// </summary>
    public interface IApplicationConfigurationService
    {
        /// <summary>
        /// A query object which can read from the configuration resource.
        /// </summary>
        IApplicationConfigurationQuery Query { get; }

        /// <summary>
        /// A command object which can modify the configuration resource.
        /// </summary>
        IApplicationConfigurationCommand Command { get; }
    }
}
