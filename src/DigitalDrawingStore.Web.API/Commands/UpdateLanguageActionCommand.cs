using Unity;
using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Culture;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class UpdateLanguageActionCommand : AActionCommand<bool>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private readonly string _languageCodeName;
        private readonly IGeneralApplicationProperties _generalApplicationProperties;
        private readonly ICultureInformationFactory _cultureInformationFactory;

        private static readonly IDictionary<string, string> _languageCodeDictionary = new Dictionary<string, string>
        {
            ["HU"] = "hu-HU",
            ["EN"] = "en",
        };
        #endregion

        #region ctor
        public UpdateLanguageActionCommand(string languageCodeName)
        {

            if (string.IsNullOrWhiteSpace(languageCodeName))
            {
                throw new ArgumentException($"'{nameof(languageCodeName)}' cannot be null or whitespace.", nameof(languageCodeName));
            }

            var container = new ContainerFactory().CreateContainer();
            _generalApplicationProperties = container.Resolve<ICommonApplicationProperties>().GeneralApplicationProperties;
            _cultureInformationFactory = container.Resolve<ICultureInformationFactory>();

            _languageCodeName = languageCodeName;
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var response = false;

            if (_languageCodeDictionary.ContainsKey(_languageCodeName))
            {
                var languageCode = _languageCodeDictionary[_languageCodeName];
                _generalApplicationProperties.SelectedCulture = _cultureInformationFactory.CreateByLanguageCountryCode(languageCode);
            }

            ResolveAction(response);
        }
        #endregion
    }
}
