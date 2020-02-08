using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace XTMF2.Web.Client.Services {

    /// <summary>
    /// Client authentication state provider.
    /// </summary>
    public class XtmfAuthenticationStateProvider : AuthenticationStateProvider {
        public XtmfAuthenticationStateProvider () {

        }

        /// <summary>
        /// Returns the authentication state of the current context. This method is invokved by Authorization tags 
        /// in several pages.
        /// </summary>
        /// <returns></returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync () {
            var identity = new ClaimsIdentity (new [] {
                new Claim (ClaimTypes.Name, string.Empty),
                }, string.Empty) {

            };
            var user = new ClaimsPrincipal (identity);
            return Task.FromResult (new AuthenticationState (user));
        }
    }
}