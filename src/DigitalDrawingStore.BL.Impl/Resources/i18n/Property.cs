using XperiCad.Common.Core.Culture.Resource.Json;
using XperiCad.Common.Infrastructure.Culture.Resource;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n
{
    internal class Property
    {
        private const string LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH = "XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n.Properties.hu_HU.DocumentProperties.json";

        #region Document properties
        internal static ICultureResource Property_Name = new JsonCultureResource("Property_Name", LANGUAGE_DOCUMENT_PRPERTY_RESOURCES_PATH);
        #endregion
    }
}
