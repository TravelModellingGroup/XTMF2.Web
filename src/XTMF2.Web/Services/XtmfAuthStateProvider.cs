//    Copyright 2017-2019 University of Toronto
// 
//    This file is part of XTMF2.
// 
//    XTMF2 is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    XTMF2 is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace XTMF2.Web.Services
{
    /// <summary>
    /// </summary>
    public class XtmfAuthStateProvider : AuthenticationStateProvider
    {
        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";

        /// <summary>
        /// </summary>
        /// <param name="xtmfUser"></param>
        public XtmfAuthStateProvider(User xtmfUser)
        {
            XtmfUser = xtmfUser;
        }

        private User XtmfUser { get; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, XtmfUser.IsAdmin ? RoleAdmin : RoleUser),
                new Claim(ClaimTypes.Name, XtmfUser.UserName)
            }, "xtmf2");
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}