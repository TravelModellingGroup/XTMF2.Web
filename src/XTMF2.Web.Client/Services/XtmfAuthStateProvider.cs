using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace XTMF2.Web.Client.Services {

    public class XtmfAuthStateProvider : AuthenticationStateProvider {
        public XtmfAuthStateProvider () {

        }

        /// <summary>
        /// Returns the authentication state of the current context. This method is invokved by Authorization tags 
        /// in several pages.
        /// </summary>
        /// <returns></returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync () {
            var identity = new ClaimsIdentity (new [] {
                new Claim (ClaimTypes.Name, "mrfibuli"),
                }, string.Empty) {

            };
            var user = new ClaimsPrincipal (identity);
            return Task.FromResult (new AuthenticationState (user));
        }
    }
}