using System;
using System.Xml;
using XperiCad.Common.Infrastructure.Application.DataSource;

namespace XperiCad.Common.Core.Application.DataSource
{
    internal class XmlApplicationConfigurationCommand : IApplicationConfigurationCommand
    {
        #region Fields
        private readonly string _applicationConfigurationFilePath;
        #endregion

        #region ctor
        public XmlApplicationConfigurationCommand(string applicationConfigurationFilePath)
        {
            if (string.IsNullOrWhiteSpace(applicationConfigurationFilePath))
            {
                throw new ArgumentException($"'{nameof(applicationConfigurationFilePath)}' cannot be null or whitespace.", nameof(applicationConfigurationFilePath));
            }

            _applicationConfigurationFilePath = applicationConfigurationFilePath;
        }
        #endregion

        #region IApplicationConfigurationCommand members
        public void SetPropertyByName(string propertyName, string propertyValue)
        {
            TrySetPropertyByName(propertyName, propertyValue);
        }

        public void SetPropertyByName(string propertyName, bool propertyValue)
        {
            SetPropertyByName(propertyName, propertyValue.ToString());
        }

        public void SetPropertyByName(string propertyName, int propertyValue)
        {
            SetPropertyByName(propertyName, propertyValue.ToString());
        }

        public void SetPropertyByName(string propertyName, long propertyValue)
        {
            SetPropertyByName(propertyName, propertyValue.ToString());
        }
        #endregion

        #region Private members
        private void SetFirstPropertyValueByName(XmlDocument document, string propertyName, string value)
        {
            var propertyNameTags = document.GetElementsByTagName("Name");
            foreach (XmlNode propertyNameTag in propertyNameTags)
            {
                if (propertyNameTag.FirstChild.Value == propertyName)
                {
                    var siblingNodes = propertyNameTag.ParentNode.ChildNodes;
                    foreach (XmlNode siblingNode in siblingNodes)
                    {
                        if (siblingNode.Name == "Value")
                        {
                            if (siblingNode.FirstChild == null)
                            {
                                siblingNode.AppendChild(document.CreateTextNode(value));
                            }
                            else
                            {
                                siblingNode.FirstChild.Value = value;
                            }
                        }
                    }

                    return;
                }
            }
        }

        private void TrySetPropertyByName(string propertyName, string propertyValue)
        {
            try
            {
                var appConfigXml = new XmlDocument();
                appConfigXml.Load(_applicationConfigurationFilePath);

                SetFirstPropertyValueByName(appConfigXml, propertyName, propertyValue);

                appConfigXml.Save(_applicationConfigurationFilePath);
            }
            catch (Exception)
            {
                // TODO: log or feedback
            }
        }
        #endregion
    }
}
