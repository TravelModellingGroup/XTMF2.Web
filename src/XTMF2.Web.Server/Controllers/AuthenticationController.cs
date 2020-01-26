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
using Microsoft.AspNetCore.Mvc;
using XTMF2.Web.Services;
using XTMF2.Web.Services.Interfaces;

namespace XTMF2.Web.Server.Controllers {
    /// <summary>
    ///     Primary authentication controller. Manages endpoints for login and logout.
    /// </summary>
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller {

        private readonly XTMFRuntime _xtmf;

        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationService"></param>
        /// <param name="xtmf"></param>
        public AuthenticationController (IAuthenticationService authenticationService, XTMFRuntime xtmf) {
            _authenticationService = authenticationService;
            _xtmf = xtmf;
        }

        /// <summary>
        /// Login endpoint.
        /// </summary>
        /// <param name="userName">The username to login.</param>
        [HttpPost ("authenticate")]
        public async Task<IActionResult> Authenticate (string userName) {
            var tokenString = await _authenticationService.SignIn (userName);
            return Ok (tokenString);
        }

        /// <summary>
        /// Login endpoint.
        /// </summary>
        /// <param name="userName">The username to login.</param>
        [HttpPost ("logout")]
        public async Task<IActionResult> Logout () {
            await _authenticationService.SignOut(null);
            return Ok ();
        }
    }
}