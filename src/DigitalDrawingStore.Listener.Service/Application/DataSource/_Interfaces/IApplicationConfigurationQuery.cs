using System.Collections.Generic;

namespace XperiCad.Common.Infrastructure.Application.DataSource
{
    /// <summary>
    /// Provides queries for application configuration. With these methods the properties in the configuration resource could be read.
    /// </summary>
    public interface IApplicationConfigurationQuery
    {
        /// <summary>
        /// Get a single string property by it's name.
        /// </summary>
        /// <param name="propertyName">The name of the property in the configuration resource.</param>
        /// <returns>With the value of property, if exists, otherwise with empty string.</returns>
        string GetStringPropertyByName(string propertyName);

        IEnumerable<string> GetStringCollectionByName(string propertyName);

        /// <summary>
        /// Get a single bool property by it's name.
        /// </summary>
        /// <param name="propertyName">The name of the property in the configuration resource.</param>
        /// <returns>With the value of property, if exists, otherwise with false.</returns>
        bool GetBoolPropertyByName(string propertyName);

        /// <summary>
        /// Get a single int property by it's name.
        /// </summary>
        /// <param name="propertyName">The name of the property in the configuration resource.</param>
        /// <returns>With the value of property, if exists, otherwise with 0.</returns>
        int GetIntPropertyByName(string propertyName);

        /// <summary>
        /// Get a single long property by it's name.
        /// </summary>
        /// <param name="propertyName">The name of the property in the configuration resource.</param>
        /// <returns>With the value of property, if exists, otherwise with 0L.</returns>
        long GetLongPropertyByName(string propertyName);
    }
}
