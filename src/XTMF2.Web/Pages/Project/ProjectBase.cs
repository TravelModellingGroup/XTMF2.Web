using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace XTMF2.Web.Pages.Project
{
    /// <summary>
    /// Single project view (page).
    /// </summary>
    public class ProjectBase : ComponentBase
    {
        [Inject]
        protected XTMF2.XTMFRuntime XtmfRuntime { get; set; }

        [Inject]
        protected XTMF2.User XtmfUser { get; set; }

        /// <summary>
        /// Path parameter that specifies the ProjectName
        /// </summary>
        [Microsoft.AspNetCore.Components.Parameter]
        public string ProjectName { get; set; }


        [Inject]
        protected ILogger<ProjectBase> Logger { get; set; }

        /// <summary>
        /// The loaded project.
        /// </summary>
        protected XTMF2.Project Project { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProjectBase()
        {

        }

        /// <summary>
        /// Initialization function, will attempt to load the referenced project.
        /// </summary>
        protected override void OnInitialized()
        {
            string error = "";
            if (this.XtmfRuntime.ProjectController.GetProject(XtmfUser.UserName, ProjectName, out var project, ref error))
            {
                this.Project = project;
            }
            else
            {
                Logger.LogError("Unable to load project, or project not found: " + ProjectName);
            }

        }
    }
}
