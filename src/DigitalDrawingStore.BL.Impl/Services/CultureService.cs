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
            [CultureProperty.BOTTOM_RIGHT_WATERMARK_TEXT] = Property.Bottom_Right_Watermark_Text,

            [CultureProperty.CATEGORIES_TABLE_NAME_TEXT] = Property.Categories_Table_Name_Text,
            [CultureProperty.DOCUMENT_CATEGORIES_COLUMN_TEXT] = Property.Document_Categories_Column_Text,
            [CultureProperty.IS_DESIGNED_COLUMN_TEXT] = Property.Is_Designed_Column_Text,
            [CultureProperty.TABLE_ACTIONS_COLUMN_TEXT] = Property.Table_Actions_Column_Text,
            [CultureProperty.IS_DESIGNED_YES_TEXT] = Property.Is_Designed_Yes_Text,
            [CultureProperty.IS_DESIGNED_NO_TEXT] = Property.Is_Designed_No_Text,
            [CultureProperty.EDIT_CATEGORY_TEXT] = Property.Edit_Category_Text,
            [CultureProperty.ADDED_METADATA_COLUMN_TEXT] = Property.Added_Metadata_Column_Text,
            [CultureProperty.NOT_ADDED_METADATA_COLUMN_TEXT] = Property.Not_Added_Metadata_Column_Text,
            [CultureProperty.CLOSE_EDITOR_WINDOW_TEXT] = Property.Close_Editor_Window_Text,
            [CultureProperty.SAVE_EDITOR_WINDOW_TEXT] = Property.Save_Editor_Window_Text,

            [CultureProperty.UNKNOWN_ERROR_TITLE_TEXT] = Property.Unknown_Error_Title_Text,
            [CultureProperty.UNKNOWN_ERROR_TEXT] = Property.Unknown_Error_Text,
            [CultureProperty.ERROR_TITLE_TEXT] = Property.Error_Title_Text,
            [CultureProperty.WARNING_TITLE_TEXT] = Property.Warning_Title_Text,
            [CultureProperty.INFORMATION_TITLE_TEXT] = Property.Information_Title_Text,
            [CultureProperty.ATTRIBUTES_NOT_FOUND_TEXT] = Property.Attributes_Not_Found_Text,
            [CultureProperty.CHANGES_WILL_BE_LOST_TEXT] = Property.Changes_Will_Be_Lost_Text,
            [CultureProperty.DIALOG_EDIT_TEXT] = Property.Dialog_Edit_Text,
            [CultureProperty.DIALOG_SAVE_TEXT] = Property.Dialog_Save_Text,
            [CultureProperty.DIALOG_CANCEL_TEXT] = Property.Dialog_Cancel_Text,
            [CultureProperty.DIALOG_YES_CHECK_VALUE] = Property.Dialog_Yes_Check_Value,

            [CultureProperty.DOCUMENTS_TABLE_NAME_TEXT] = Property.Documents_Table_Name_Text,
            [CultureProperty.NAME_TABLE_COLUMN_TEXT] = Property.Name_Table_Column_Text,
            [CultureProperty.CATEGORY_TABLE_COLUMN_TEXT] = Property.Category_Table_Column_Text,
            [CultureProperty.ACTIONS_TABLE_COLUMN_TEXT] = Property.Actions_Table_Column_Text,
            [CultureProperty.EDIT_ACTION_BUTTON_TEXT] = Property.Edit_Action_Button_Text,
            [CultureProperty.CLOSE_BUTTON_TEXT] = Property.Close_Button_Text,
            [CultureProperty.METADATA_NAME_COLUMN_TEXT] = Property.Metadata_Name_Table_Column_Text,
            [CultureProperty.METADATA_VALUE_COLUMN_TEXT] = Property.Metadata_Value_Table_Column_Text,

            [CultureProperty.APP_CONFIGURATION_TEXT] = Property.App_Configuration_Text,
            [CultureProperty.DATABASE_PATH_TEXT] = Property.Database_Path_Text,
            [CultureProperty.SAVE_CHANGES_TEXT] = Property.Save_Changes_Text,
            [CultureProperty.FEEDBACK_SETTINGS_TEXT] = Property.Feedback_Settings_Text,
            [CultureProperty.SENDER_EMAIL_TEXT] = Property.Sender_Email_Text,
            [CultureProperty.EMAIL_RECIPIENTS_TEXT] = Property.Email_Recipients_Text,
            [CultureProperty.SMTP_HOST_TEXT] = Property.Smtp_Host_Text,
            [CultureProperty.SMTP_PORT_TEXT] = Property.Smtp_Port_Text,
            [CultureProperty.SMTP_USERNAME_TEXT] = Property.Smtp_Username_Text,
            [CultureProperty.SMTP_PASSWORD_TEXT] = Property.Smtp_Password_Text,
            [CultureProperty.IS_USE_DEFAULT_CREDENTIALS_TEXT] = Property.Is_Use_Default_Credentials_Text,
            [CultureProperty.IS_ENABLE_SSL_TEXT] = Property.Is_Enable_Ssl_Text,
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
