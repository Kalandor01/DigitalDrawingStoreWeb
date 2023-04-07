using Unity;
using XperiCad.Common.Infrastructure.Culture.Resource;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories._Interfaces
{
    internal interface ICultureFactory
    {
        public string GetSelectedCulture();
        public string GetSelectedCulture(IUnityContainer container);
    }
}
