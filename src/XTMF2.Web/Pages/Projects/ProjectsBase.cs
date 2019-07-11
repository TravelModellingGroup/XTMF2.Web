
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Layouts;
using Microsoft.Extensions.Logging;
using XTMF2;
using XTMF2.Editing;
using XTMF2.Web.Components;
using XTMF2.Web.Shared;

namespace XTMF2.Web.Pages
{

    /// <summary>
    ///  Projects (page) base component.
    /// </summary>
    public class ProjectsBase : ComponentBase
    {

        [Inject]
        protected XTMF2.XTMFRuntime XTMFRuntime { get; set; }

        [Inject]
        protected XTMF2.User XTMFUser { get; set; }

        [Inject]
        protected ILogger<ProjectsBase> Logger { get; set; }

        [Parameter]
        protected string NewProjectName {get;set;}

        [CascadingParameter]
        private ContentLayout Layout { get; set; }

        protected Modal newProjectDialog;


        public List<XTMF2.Project> Projects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProjectsBase()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInit()
        {
            Projects = new List<XTMF2.Project>();
            Projects.AddRange(XTMFRuntime.ProjectController.GetProjects(XTMFUser));
        }

        public void DeleteProject(XTMF2.Project project)
        {
            Logger.LogInformation("Deleting project " + project.Name);
            string error = "";
            XTMFRuntime.ProjectController.DeleteProject(XTMFUser, project.Name, ref error);
            Projects.Remove(project);

            LayoutComponentBase b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void ShowNewProjectDialog(UIMouseEventArgs e)
        {
            this.newProjectDialog.Show();
        }


        protected void NewProjectDialog_Confirm(UIEventArgs e)
        {
            ProjectSession session = null;
            string error = "";
            if ((XTMFRuntime.ProjectController.CreateNewProject(XTMFUser, NewProjectName,
            out session, ref error)))
            {
                Projects.Add(session.Project);
                Console.WriteLine("Adding project");

            }
            else
            {
                Console.WriteLine("Error occured");
            }
            Console.WriteLine("confirmed");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void NewProjectDialog_Cancel(UIEventArgs e)
        {
            Console.WriteLine("cancelled");
        }
    }

}