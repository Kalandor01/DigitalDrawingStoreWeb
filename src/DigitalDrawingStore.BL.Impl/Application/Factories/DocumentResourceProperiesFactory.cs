using Unity;
using XperiCad.Common.Infrastructure.Application.DataSource;
using XperiCad.DigitalDrawingStore.BL.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories._Interfaces;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories
{
    public class DocumentResourceProperiesFactory : IDocumentResourceProperiesFactory
    {

        #region 
        public IDocumentResourceProperties CreateDocumentResourceProperties(string applicationConfigurationFilePath)
        {
            if (string.IsNullOrWhiteSpace(applicationConfigurationFilePath))
            {
                throw new ArgumentException($"'{nameof(applicationConfigurationFilePath)}' cannot be null or whitespace.", nameof(applicationConfigurationFilePath));
            }

            applicationConfigurationFilePath = Path.GetFullPath(applicationConfigurationFilePath);

            var container = new ContainerFactory().CreateContainer();

            var applicationConfigurationServiceFactory = container.Resolve<IApplicationConfigurationServiceFactory>();
            var applicationConfigurationService = applicationConfigurationServiceFactory.CreateApplicationConfigurationService(applicationConfigurationFilePath);

            return new DocumentResourceProperies(applicationConfigurationService);
        }
        #endregion
    }
}
