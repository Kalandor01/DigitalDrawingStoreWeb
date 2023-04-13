using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Culture.Resource;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Services
{
    public class CultureService
    {
        #region Fields
        private static readonly IDictionary<CultureProperty, ICultureResource> _propertyNameTranslations = new Dictionary<CultureProperty, ICultureResource>()
        {
            [CultureProperty.APPLICATION_NAME] = Property.Application_Name,

            [CultureProperty.HOME_PAGE_NAME] = Property.Home_Page_Name,
            [CultureProperty.PRIVACY_PAGE_NAME] = Property.Privacy_Page_Name,
            [CultureProperty.ERROR_PAGE_NAME] = Property.Error_Page_Name,
            [CultureProperty.DOCUMENTS_PAGE_NAME] = Property.Documents_Page_Name,
            [CultureProperty.CATEGORIES_PAGE_NAME] = Property.Categories_Page_Name,
            [CultureProperty.SETTINGS_PAGE_NAME] = Property.Settings_Page_Name,
            [CultureProperty.EVENTS_PAGE_NAME] = Property.Events_Page_Name,

            [CultureProperty.DOCUMENT_NAME_CATEGORY_NAME] = Property.Document_Name_Category_Name
        };
        #endregion

        #region Public members
        public static string GetSelectedCulture()
        {
            return GetSelectedCulture(new ContainerFactory().CreateContainer());
        }

        public static string GetSelectedCulture(IUnityContainer container)
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
