using XperiCad.DigitalDrawingStore.Web.API.Authorization;

namespace DigitalDrawingStore.Web.UI.Views.Shared
{
	public class SharedViewModel
	{
        public ISecurityFacade SecurityFacade { get; }

        public SharedViewModel(ISecurityFacade securityFacade)
		{
			SecurityFacade = securityFacade ?? throw new ArgumentNullException(nameof(securityFacade));
		}
	}
}
