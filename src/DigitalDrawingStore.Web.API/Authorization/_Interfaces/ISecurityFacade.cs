using System.Security.Claims;

namespace XperiCad.DigitalDrawingStore.Web.API.Authorization
{
    /// <summary>
    /// Manages the security mechanim for authenicating the User
    /// </summary>
    public interface ISecurityFacade
    {
        /// <summary>
        /// This method is used on the Razor Pages for authenticating.
        /// </summary>
        /// <param name="user">This is a ClaimPrincipal in the HttpContext.</param>
        /// <param name="groupName">This values is coming from the constants</param>
        /// <returns>Returns the logical result of the authentication</returns>
        bool IsInGroup(ClaimsPrincipal user, string groupName);

        /// <summary>
        /// This method is called by the ClaimsTransformer class.
        /// </summary>
        /// <param name="principal">This parameter is coming from the authorize tag.</param>
        /// <returns></returns>
        Task<ClaimsPrincipal> TransformPrincipalAsync(ClaimsPrincipal principal);
    }
}
