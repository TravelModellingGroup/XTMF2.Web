
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using XTMF2;
using BlazorStrap;
using XTMF2.Editing;
using XTMF2.Web.Components;
using XTMF2.Web.Pages;
using XTMF2.Web;
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
        protected string NewProjectName { get; set; }


        public BSModal LiveDemo;


        public List<XTMF2.Project> Projects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProjectsBase()
        {

        }

        public void NewProjectClicked(System.EventArgs e)
        {
            Console.WriteLine("Button was clicked!");
            Logger.LogDebug("hello debug");
            LiveDemo.Show();
            // Console.WriteLine(LiveDemo.Show());
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {

            Projects = new List<XTMF2.Project>();
            Projects.AddRange(XTMFRuntime.ProjectController.GetProjects(XTMFUser));
            Console.WriteLine("hello");
        }

        public void DeleteProject()
        {
            Logger.LogInformation("Deleting project ");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void ShowNewProjectDialog(EventArgs e)
        {

        }


        protected void NewProjectDialog_Confirm(EventArgs e)
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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void NewProjectDialog_Cancel(EventArgs e)
        {
            Console.WriteLine("cancelled");
        }
    }

}