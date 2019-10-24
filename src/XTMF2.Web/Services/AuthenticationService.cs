using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Services
{
    /// <summary>
    /// Authentication service for clients. Associates a session with a backed XTMF2 user account.
    /// </summary>
    public class AuthenticationService
    {

        private readonly UserManager<XtmfUser> _userManager;

        private readonly SignInManager<XtmfUser> _signInManager;

        private readonly ILogger<AuthenticationService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="logger"></param>
        public AuthenticationService(UserManager<XtmfUser> userManager, SignInManager<XtmfUser> signInManager, ILogger<AuthenticationService> logger)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">The username to associate the session with.</param>
        /// <param name="password">Currently unused.</param>
        public async Task SignIn(string userName, string password = null)
        {
            var user = await this._userManager.FindByIdAsync(userName);
            await _signInManager.SignInAsync(user, true, null);
        }

    }
}
