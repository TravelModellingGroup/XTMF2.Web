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

namespace XTMF2.Web.Controllers {
	/// <summary>
	/// </summary>
	[Route ("api/[controller]")]
	public class AuthenticationController : Controller {
		private readonly AuthenticationService _authenticationService;

		/// <summary>
		/// </summary>
		/// <param name="authenticationService"></param>
		public AuthenticationController (AuthenticationService authenticationService) {
			_authenticationService = authenticationService;
		}

		/// <summary>
		 ///     Login endpoint.
		 /// </summary>
		 /// <param name="userName">The username to login.</param>
		 [HttpPost]
		 public async Task<IActionResult> Login([FromBody] string userName)
		 {
		     await _authenticationService.SignIn(userName);
		     return new OkObjectResult(User.Identity.IsAuthenticated);
		 } 
	}
}