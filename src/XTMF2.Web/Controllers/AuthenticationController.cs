using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XTMF2.Web.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XTMF2.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationService _authenticationService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationService"></param>
        public AuthenticationController(AuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        /// <summary>
        /// Login endpoint.
        /// </summary>
        /// <param name="userName">The username to login.</param>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]string userName)
        {
            await this._authenticationService.SignIn(userName);
            return new OkObjectResult(User.Identity.IsAuthenticated);

        }

    }
}
