using System;
using System.Collections.Generic;
using System.Xml;
using XperiCad.Common.Infrastructure.Application.DataSource;

namespace XperiCad.Common.Core.Application.DataSource
{
    // TODO: need tests
    internal class XmlApplicationConfigurationQuery : IApplicationConfigurationQuery
    {
        #region Fields
        private readonly string _applicationConfigurationFilePath;
        #endregion

        #region ctor
        public XmlApplicationConfigurationQuery(string applicationConfigurationFilePath)
        {
            if (string.IsNullOrWhiteSpace(applicationConfigurationFilePath))
            {
                throw new System.ArgumentException($"'{nameof(applicationConfigurationFilePath)}' cannot be null or whitespace.", nameof(applicationConfigurationFilePath));
            }

            _applicationConfigurationFilePath = applicationConfigurationFilePath;
        }
        #endregion

        #region IApplicationConfigurationQuery members
        public bool GetBoolPropertyByName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new System.ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            var propertyValue = GetFirstPropertyValueByName(propertyName);
            _ = bool.TryParse(propertyValue, out bool boolPropertyValue);

            return boolPropertyValue;
        }

        public int GetIntPropertyByName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            var propertyValue = GetFirstPropertyValueByName(propertyName);
            _ = int.TryParse(propertyValue, out int intPropertyValue);

            return intPropertyValue;
        }

        public long GetLongPropertyByName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            var propertyValue = GetFirstPropertyValueByName(propertyName);
            _ = long.TryParse(propertyValue, out long longPropertyValue);

            return longPropertyValue;
        }

        public string GetStringPropertyByName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new System.ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            var propertyValue = GetFirstPropertyValueByName(propertyName);

            return propertyValue;
        }

        public IEnumerable<string> GetStringCollectionByName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            return GetPropertyByName(propertyName);
        }
        #endregion

        #region Private members
        private IEnumerable<string> GetPropertyByName(string propertyName)
        {
            try
            {
                var appConfigXml = new XmlDocument();
                appConfigXml.Load(_applicationConfigurationFilePath);

                var property = GetFirstPropertyByName(propertyName, appConfigXml);
                var propertyValue = property == null ? null : GetAllPropertyValue(property);

                return propertyValue;
            }
            catch (Exception)
            {
                // TODO: log or feedback
            }

            return null;
        }

        private string GetFirstPropertyValueByName(string propertyName)
        {
            try
            {
                var appConfigXml = new XmlDocument();
                appConfigXml.Load(_applicationConfigurationFilePath);

                var property = GetFirstPropertyByName(propertyName, appConfigXml);
                var propertyValue = property == null ? string.Empty : GetPropertyValue(property);

                return propertyValue;
            }
            catch (Exception)
            {
                // TODO: log or feedback
            }

            return null;
        }

        private XmlNode GetFirstPropertyByName(string propertyName, XmlDocument appConfigXml)
        {
            var propertyNameTags = appConfigXml.GetElementsByTagName("Name");
            foreach (XmlNode propertyNameTag in propertyNameTags)
            {
                if (propertyNameTag.FirstChild.Value == propertyName)
                {
                    return propertyNameTag.ParentNode;
                }
            }

            return null;
        }

        private string GetPropertyValue(XmlNode propertyNode)
        {
            var childNodes = propertyNode.ChildNodes;

            foreach (XmlNode childNode in childNodes)
            {
                if (childNode.Name == "Value")
                {
                    return childNode?.FirstChild?.Value ?? string.Empty;
                }
            }

            return string.Empty;
        }

        private IEnumerable<string> GetAllPropertyValue(XmlNode propertyNode)
        {
            var result = new List<string>();
            var childNodes = propertyNode.ChildNodes;

            foreach (XmlNode childNode in childNodes)
            {
                if (childNode.Name == "Value")
                {
                    result.Add(childNode?.FirstChild?.Value ?? string.Empty);
                }
            }

            return result;
        }
        #endregion
    }
}
