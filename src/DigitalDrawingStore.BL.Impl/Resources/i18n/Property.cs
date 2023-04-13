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
    }
}
