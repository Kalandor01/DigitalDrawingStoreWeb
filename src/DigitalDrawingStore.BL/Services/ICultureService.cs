using Unity;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories._Interfaces
{
    public interface ICultureService
    {
        public string GetSelectedCulture();
        public string GetSelectedCulture(IUnityContainer container);
    }
}
