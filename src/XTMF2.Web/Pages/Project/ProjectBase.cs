using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace XTMF2.Web.Pages.Project
{
    public class ProjectBase : ComponentBase
    {

        [Inject]
        protected XTMF2.XTMFRuntime XTMFRuntime { get; set; }

        [Inject]
        protected XTMF2.User XTMFUser { get; set; }

        [Parameter]
        public string Project { get; set; }
    }
}
