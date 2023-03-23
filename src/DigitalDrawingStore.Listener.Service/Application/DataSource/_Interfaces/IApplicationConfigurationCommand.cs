namespace XperiCad.Common.Infrastructure.Application.DataSource
{
    /// <summary>
    /// Provides manipulation commands for application configuration. With these methods the configuration resource value could be modified.
    /// </summary>
    public interface IApplicationConfigurationCommand
    {
        /// <summary>
        /// Set a single string property by name.
        /// </summary>
        /// <param name="propertyName">Name of the property in the configuration resource.</param>
        /// <param name="propertyValue">The new value of the property in the configuration resource.</param>
        void SetPropertyByName(string propertyName, string propertyValue);

        /// <summary>
        /// Set a single bool property by name.
        /// </summary>
        /// <param name="propertyName">Name of the property in the configuration resource.</param>
        /// <param name="propertyValue">The new value of the property in the configuration resource.</param>
        void SetPropertyByName(string propertyName, bool propertyValue);

        /// <summary>
        /// Set a single int property by name.
        /// </summary>
        /// <param name="propertyName">Name of the property in the configuration resource.</param>
        /// <param name="propertyValue">The new value of the property in the configuration resource.</param>
        void SetPropertyByName(string propertyName, int propertyValue);

        /// <summary>
        /// Set a single long property by name.
        /// </summary>
        /// <param name="propertyName">Name of the property in the configuration resource.</param>
        /// <param name="propertyValue">The new value of the property in the configuration resource.</param>
        void SetPropertyByName(string propertyName, long propertyValue);
    }
}
