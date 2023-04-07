using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Culture.Resource;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories._Interfaces;
using XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories
{
    public class CultureFactory : ICultureFactory
    {
        #region Fields
        private static readonly IDictionary<string, ICultureResource> _propertyNameTranslations = new Dictionary<string, ICultureResource>()
        {
            ["Name"] = Property.Property_Name,
            ["Document number"] = Property.Property_Document_Number,
            ["Part number"] = Property.Property_Part_Number,
            ["CreatedBy"] = Property.Property_Created_By,
            ["CreatedAt"] = Property.Property_Created_At,
            ["ModifiedBy"] = Property.Property_Modified_By,
            ["ModifiedAt"] = Property.Property_Modified_At,
            ["ModeOfUsage"] = Property.Property_Mode_Of_Usage,
            ["Freetext"] = Property.Property_Freetext
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

        public static ICultureResource? GetPropertyNameTranslationCluture(string propertyName)
        {
            _propertyNameTranslations.TryGetValue(propertyName, out var resource);
            return resource;
        }

        public static string? GetPropertyNameTranslation(string propertyName, string selectedCulture)
        {
            var culture = GetPropertyNameTranslationCluture(propertyName);
            return culture?.GetCultureString(selectedCulture).FirstOrDefault().Value;
        }
        #endregion
    }
}
