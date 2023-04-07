using XperiCad.Common.Core.Culture.Resource.Json;
using XperiCad.Common.Infrastructure.Culture.Resource;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n
{
    internal class Property
    {
        private const string LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH = "XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n.Documents.Properties.hu_HU.PropertyValues.json";

        #region Document properties
        internal static ICultureResource Property_Name = new JsonCultureResource("Property_Name", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Property_Document_Number = new JsonCultureResource("Property_Document_Number", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Property_Part_Number = new JsonCultureResource("Property_Part_Number", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Property_Created_By = new JsonCultureResource("Property_Created_By", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Property_Created_At = new JsonCultureResource("Property_Created_At", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Property_Modified_By = new JsonCultureResource("Property_Modified_By", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Property_Modified_At = new JsonCultureResource("Property_Modified_At", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Property_Mode_Of_Usage = new JsonCultureResource("Property_Mode_Of_Usage", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);

        internal static ICultureResource Property_Freetext = new JsonCultureResource("Property_Freetext", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);
        #endregion
    }
}
