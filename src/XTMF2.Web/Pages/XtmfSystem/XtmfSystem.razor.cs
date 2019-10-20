using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Pages.XtmfSystem
{
    public partial class XtmfSystem
    {

        [Inject]
        public SignInManager<XtmfUser> SignInManager { get; set; }

        [Inject]
        public UserManager<XtmfUser> UserManager { get; set; }

        [Inject]
        protected ILogger<XtmfSystem> Logger { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var user = await this.UserManager.FindByIdAsync("local");
            await SignInManager.SignInAsync(user, true, null);
            Logger.LogDebug("Code after attempting sign in.");
        }


        /// <summary>
        /// 
        /// </summary>
        public async void SignIn()
        {

            return;
        }
    }
}