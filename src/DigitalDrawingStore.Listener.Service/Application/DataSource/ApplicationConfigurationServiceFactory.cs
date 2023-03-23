using XperiCad.Common.Infrastructure.Application.DataSource;

namespace XperiCad.Common.Core.Application.DataSource
{
    internal class ApplicationConfigurationServiceFactory : IApplicationConfigurationServiceFactory
    {
        public IApplicationConfigurationService CreateApplicationConfigurationService(string configurationPath)
        {
            return new ApplicationConfigurationService(
                new XmlApplicationConfigurationQuery(configurationPath),
                new XmlApplicationConfigurationCommand(configurationPath));
        }
    }
}
