using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace XTMF2.Web.Views.Project
{
    /// <summary>
    /// Single project view (page).
    /// </summary>
    public partial class SingleProject
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
        protected ILogger<SingleProject> Logger { get; set; }

        /// <summary>
        /// The loaded project.
        /// </summary>
        protected XTMF2.Project Project { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SingleProject()
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
