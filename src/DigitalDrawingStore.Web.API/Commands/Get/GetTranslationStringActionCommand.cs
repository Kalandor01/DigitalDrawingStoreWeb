using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands.Get
{
    public class GetTranslationStringActionCommand : AActionCommand<string?>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private readonly CultureProperty _propertyEnum;
        private readonly string _cultureString;
        #endregion

        #region ctor
        public GetTranslationStringActionCommand(CultureProperty propertyKey)
        {
            _propertyEnum = propertyKey;
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
            var translationString = CultureService.GetPropertyNameTranslation(_propertyEnum, _cultureString);

            ResolveAction(translationString);
        }
        #endregion
    }
}
