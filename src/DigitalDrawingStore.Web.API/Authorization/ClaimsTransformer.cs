using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace XperiCad.DigitalDrawingStore.Web.API.Authorization
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly ISecurityFacade _securityFacade;

        public ClaimsTransformer(ISecurityFacade securityFacade)
        {
            _securityFacade = securityFacade ?? throw new ArgumentNullException(nameof(securityFacade));
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            return await _securityFacade.TransformPrincipalAsync(principal);
        }
    }
}
