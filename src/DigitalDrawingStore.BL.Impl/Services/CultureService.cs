using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Culture;
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

            [CultureProperty.DOCUMENT_NAME_CATEGORY_NAME] = Property.Document_Name_Category_Name,

            [CultureProperty.SEARCH_BUTTON_TEXT] = Property.Search_Button_Text,
            [CultureProperty.SHOW_EMPTY_CATEGORIES_CHECKBOX_TEXT] = Property.Show_Empty_Categories_Checkbox_Text,
            [CultureProperty.COPYRIGHT] = Property.Copyright,
            [CultureProperty.SEARCHABLE_FIELDS_TEXT_1] = Property.Searchable_Fields_Text_1,
            [CultureProperty.SEARCHABLE_FIELDS_TEXT_2] = Property.Searchable_Fields_Text_2,
            [CultureProperty.SEARCHABLE_FIELDS_TEXT_3] = Property.Searchable_Fields_Text_3,
            [CultureProperty.SEARCHABLE_FIELDS_TEXT_4] = Property.Searchable_Fields_Text_4,
            [CultureProperty.EMPTY_CATEGORY_TEXT] = Property.Empty_Category_Text,

            [CultureProperty.OPEN_EDITOR_TEXT] = Property.Open_Editor_Text,
            [CultureProperty.CLOSE_EDITOR_TEXT] = Property.Close_Editor_Text,
            [CultureProperty.OUTER_WATERMARK_POSITION_TEXT] = Property.Outer_Watermark_Position_Text,
            [CultureProperty.DOCUMENT_USAGE_TEXT] = Property.Document_Usage_Text,
            [CultureProperty.MIDDLE_WATERMARK_HORIZONTAL_OFFSET_TEXT] = Property.Middle_Watermark_Horizontal_Offset_Text,
            [CultureProperty.MIDDLE_WATERMARK_VARTICAL_OFFSET_TEXT] = Property.Middle_Watermark_Vertical_Offset_Text,
            [CultureProperty.FONT_SIZE_TEXT] = Property.Font_Size_Text,
            [CultureProperty.WATERMARK_TRANSPARENCY_TEXT] = Property.Watermark_Transparency_Text,
            [CultureProperty.UPDATE_TEXT] = Property.Update_Text,
            [CultureProperty.TOP_LEFT_WATERMARK_TEXT] = Property.Top_Left_Watermark_Text,
            [CultureProperty.TOP_RIGHT_WATERMARK_TEXT] = Property.Top_Right_Watermark_Text,
            [CultureProperty.BOTTOM_LEFT_WATERMARK_TEXT] = Property.Bottom_Left_Watermark_Text,
            [CultureProperty.BOTTOM_RIGHT_WATERMARK_TEXT] = Property.Bottom_Right_Watermark_Text
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

        public static void SetSelectedCulture(IUnityContainer container, string cultureLanguageCode)
        {
            var cultureInformationFactory = container.Resolve<ICultureInformationFactory>();
            var culture = cultureInformationFactory.CreateByLanguageCountryCode(cultureLanguageCode);
            SetSelectedCulture(container, culture);
        }

        public static void SetSelectedCulture(IUnityContainer container, ICultureInformation culture)
        {
            var commonApplicationProperties = container.Resolve<ICommonApplicationProperties>();
            commonApplicationProperties.GeneralApplicationProperties.SelectedCulture = culture;
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
