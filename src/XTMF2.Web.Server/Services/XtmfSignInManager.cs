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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace XTMF2.Web.Server.Services
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class XtmfSignInManager<TUser> : SignInManager<User> where TUser : class
    {
        /// <summary>
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="claimsFactory"></param>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        /// <param name="schemes"></param>
        /// <param name="confirmation"></param>
        public XtmfSignInManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<User> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor,
            logger, schemes, confirmation)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isPersistent"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        public override Task SignInAsync(User user, bool isPersistent, string authenticationMethod = null)
        {
            return base.SignInAsync(user, isPersistent, authenticationMethod);
        }
    }
}