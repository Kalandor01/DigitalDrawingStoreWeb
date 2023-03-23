using System.Security.Claims;
using System.Security.Principal;

namespace XperiCad.DigitalDrawingStore.Web.API.Authorization
{
    public class SecurityFacade : ISecurityFacade
	{

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility",
Justification = "Disabled because WindowsIdentity will be used in a Windows context only")]
        public bool IsInGroup(ClaimsPrincipal user, string groupPolicy)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            if(config == null)
            {
                return false;
            }
            string? groupName;

            switch (groupPolicy)
            {
                case Constants.Authorization.Policies.ADMIN:
                    groupName = config.GetValue<string>("UserGroups:Admin");
                    break;
                default:
                    groupName = config.GetValue<string>("UserGroups:User");
                    break;
            }

            if (user.Identity is not WindowsIdentity winUser || winUser.Groups == null || groupName == null)
            {
                return false;
            }

            foreach (var group in winUser.Groups)
            {
                var groupStr = group.Translate(typeof(NTAccount)).ToString();
                if (groupStr.Contains(groupName))
                {
                    return true;    
                }
            }
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility",
Justification = "Disabled because WindowsIdentity will be used in a Windows context only")]
        public async Task<ClaimsPrincipal> TransformPrincipalAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity is WindowsIdentity windowsIdentity)
            {
                if (windowsIdentity.Groups == null)
                {
                    return await Task.FromResult(principal);
                }

                foreach (var group in windowsIdentity.Groups)
                {
                    var claim = new Claim(windowsIdentity.RoleClaimType, group.Value);
                    windowsIdentity.AddClaim(claim);
                }
            }
            return await Task.FromResult(principal);
        }
    }
}
