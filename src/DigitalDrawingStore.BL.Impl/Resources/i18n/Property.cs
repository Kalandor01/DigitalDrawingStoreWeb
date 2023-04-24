using XperiCad.Common.Core.Culture.Resource.Json;
using XperiCad.Common.Infrastructure.Culture.Resource;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n
{
    internal class Property
    {
        private const string LANGUAGE_PRPERTY_RESOURCES_PATH = "XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n.Properties.hu_HU.GeneralProperties.json";

        #region Application properties
        internal static ICultureResource Application_Name = new JsonCultureResource("Application_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);
        #endregion

        #region Page properties
        internal static ICultureResource Home_Page_Name = new JsonCultureResource("Home_Page_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Privacy_Page_Name = new JsonCultureResource("Privacy_Page_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Error_Page_Name = new JsonCultureResource("Error_Page_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Documents_Page_Name = new JsonCultureResource("Documents_Page_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Categories_Page_Name = new JsonCultureResource("Categories_Page_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Settings_Page_Name = new JsonCultureResource("Settings_Page_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Events_Page_Name = new JsonCultureResource("Events_Page_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);
        #endregion

        #region Document properties
        internal static ICultureResource Document_Name_Category_Name = new JsonCultureResource("Document_Name_Category_Name", LANGUAGE_PRPERTY_RESOURCES_PATH);
        #endregion

        #region Page text
        internal static ICultureResource Search_Button_Text = new JsonCultureResource("Search_Button_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);
        
        internal static ICultureResource Show_Empty_Categories_Checkbox_Text = new JsonCultureResource("Show_Empty_Categories_Checkbox_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Copyright = new JsonCultureResource("Copyright", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Searchable_Fields_Text_1 = new JsonCultureResource("Searchable_Fields_Text_1", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Searchable_Fields_Text_2 = new JsonCultureResource("Searchable_Fields_Text_2", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Searchable_Fields_Text_3 = new JsonCultureResource("Searchable_Fields_Text_3", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Searchable_Fields_Text_4 = new JsonCultureResource("Searchable_Fields_Text_4", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Empty_Category_Text = new JsonCultureResource("Empty_Category_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Open_Editor_Text = new JsonCultureResource("Open_Editor_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Close_Editor_Text = new JsonCultureResource("Close_Editor_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Outer_Watermark_Position_Text = new JsonCultureResource("Outer_Watermark_Position_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);
        
        internal static ICultureResource Document_Usage_Text = new JsonCultureResource("Document_Usage_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Middle_Watermark_Horizontal_Offset_Text = new JsonCultureResource("Middle_Watermark_Horizontal_Offset_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Middle_Watermark_Vertical_Offset_Text = new JsonCultureResource("Middle_Watermark_Vertical_Offset_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Font_Size_Text = new JsonCultureResource("Font_Size_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Watermark_Transparency_Text = new JsonCultureResource("Watermark_Transparency_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);
        
        internal static ICultureResource Update_Text = new JsonCultureResource("Update_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Top_Left_Watermark_Text = new JsonCultureResource("Top_Left_Watermark_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Top_Right_Watermark_Text = new JsonCultureResource("Top_Right_Watermark_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Bottom_Left_Watermark_Text = new JsonCultureResource("Bottom_Left_Watermark_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Bottom_Right_Watermark_Text = new JsonCultureResource("Bottom_Right_Watermark_Text", LANGUAGE_PRPERTY_RESOURCES_PATH);
        #endregion
    }
}
