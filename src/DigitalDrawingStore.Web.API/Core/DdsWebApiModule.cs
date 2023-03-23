using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Culture;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;

namespace XperiCad.DigitalDrawingStore.Web.API.Core
{
    public class DdsWebApiModule
    {
        public void InitializeModule()
        {
            var container = new ContainerFactory().CreateContainer();

            // TODO: get the default culture from common settings
            var appProperties = container.Resolve<ICommonApplicationProperties>();
            appProperties.GeneralApplicationProperties.SelectedCulture = container.Resolve<ICultureInformationFactory>().CreateHungarianHungary();
        }
    }
}
