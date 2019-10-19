using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Pages.Projects;

namespace XTMF2.Web.Pages
{

    public class XtmfSystemBase : ComponentBase
    {

        [Inject]
        public SignInManager<XtmfUserModel> SignInManager { get; set; }

        [Inject]
        public UserManager<XtmfUserModel> UserManager { get; set; }

        [Inject]
        protected ILogger<XtmfSystemBase> Logger { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public async void SignIn()
        {

            var user = await this.UserManager.FindByIdAsync("local");
            await SignInManager.SignInAsync(null, true, null);

            Logger.LogDebug("Code after attempting sign in.");
            return;
        }
    }
}