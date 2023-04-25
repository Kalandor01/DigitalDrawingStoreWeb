using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands.Get
{
    public class GetTranslationStringDictionaryActionCommand : AActionCommand<Dictionary<string, string?>>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private readonly IDictionary<string, CultureProperty> _propertyEnumDictionary;
        private readonly string _cultureString;
        #endregion

        #region ctor
        public GetTranslationStringDictionaryActionCommand(IDictionary<string, CultureProperty> propertyDictionary)
        {
            _propertyEnumDictionary = propertyDictionary;
            _cultureString = CultureService.GetSelectedCulture();
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var translations = new Dictionary<string, string?>();
            foreach (var properties in _propertyEnumDictionary)
            {
                var translationString = CultureService.GetPropertyNameTranslation(properties.Value, _cultureString);
                translations.Add(properties.Key, translationString);
            }

            ResolveAction(translations);
        }
        #endregion
    }
}
