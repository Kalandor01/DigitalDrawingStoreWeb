using Unity;
using XperiCad.Common.Core.Core;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.DigitalDrawingStore.BL.Application;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application
{
    // TODO: this shouldl be a module
    public class ContainerFactory : IContainerFactory
    {
        public IUnityContainer CreateContainer()
        {
            var container = CommonCoreBootstrapper.ConfigureCommonCore(
                new string[] { Common.Core.Constants.Culture.CultureKeys.HUNGARIAN_HUNGARY },
                @"XperiCad\DigitalDrawingStore");

            var commonGeneralApplicationProperties = container.Resolve<IGeneralApplicationProperties>();
            container.RegisterFactory<IGeneralApplicationProperties>(c =>
            {
                return new DdswGeneralApplicationProperties(commonGeneralApplicationProperties);
            });

            return container;
        }
    }
}
