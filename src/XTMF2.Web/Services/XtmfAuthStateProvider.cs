using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace XTMF2.Web.Services
{
    public class XtmfAuthStateProvider : AuthenticationStateProvider
    {

        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";


        private XTMF2.User XtmfUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xtmfUser"></param>
        public XtmfAuthStateProvider(XTMF2.User xtmfUser)
        {
            this.XtmfUser = xtmfUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim( ClaimTypes.Role, XtmfUser.Admin ? RoleAdmin : RoleUser),
                new Claim(ClaimTypes.Name, XtmfUser.UserName),
            }, "xtmf2");
            var user = new ClaimsPrincipal(identity);
            
            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
