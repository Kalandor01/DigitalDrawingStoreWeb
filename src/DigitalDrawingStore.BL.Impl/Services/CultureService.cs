using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Culture.Resource;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;
using ICultureService = XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories._Interfaces.ICultureService;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Services
{
    public class CultureService : ICultureService
    {
        #region Fields
        private static readonly IDictionary<CultureProperty, ICultureResource> _propertyNameTranslations = new Dictionary<CultureProperty, ICultureResource>()
        {
            [CultureProperty.DOCUMENT_NAME] = Property.Property_Name
        };
        #endregion

        #region Public members
        public string GetSelectedCulture()
        {
            return GetSelectedCulture(new ContainerFactory().CreateContainer());
        }

        public string GetSelectedCulture(IUnityContainer container)
        {
            var commonApplicationProperties = container.Resolve<ICommonApplicationProperties>();
            return commonApplicationProperties.GeneralApplicationProperties.SelectedCulture.LanguageCountryCode;
        }

        public static ICultureResource? GetPropertyNameTranslationCluture(CultureProperty property)
        {
            _propertyNameTranslations.TryGetValue(property, out var resource);
            return resource;
        }

        public static string? GetPropertyNameTranslation(CultureProperty property, string selectedCulture)
        {
            var culture = GetPropertyNameTranslationCluture(property);
            return culture?.GetCultureString(selectedCulture).FirstOrDefault().Value;
        }
        #endregion
    }
}
