using Unity;
using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Culture;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands.Set
{
    public class UpdateLanguageActionCommand : AActionCommand<bool>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private readonly string _languageCode;
        private readonly IUnityContainer _container;

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

            _container = new ContainerFactory().CreateContainer();

            _languageCode = _languageCodeDictionary[languageCodeName];
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            CultureService.SetSelectedCulture(_container, _languageCode);

            ResolveAction(true);
        }
        #endregion
    }
}
