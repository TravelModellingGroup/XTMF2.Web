
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using BlazorStrap;
using XTMF2.Editing;
namespace XTMF2.Web.Pages.Projects
{

    /// <summary>
    ///  Projects (page) base component. This page lists the currently existing projects
    /// for the current active user. The user can both add and delete projects from this page.
    /// </summary>
    public class ProjectsBase : ComponentBase
    {

        [Inject]
        protected XTMF2.XTMFRuntime XtmfRuntime { get; set; }

        [Inject]
        protected XTMF2.User XtmfUser { get; set; }

        [Inject]
        protected ILogger<ProjectsBase> Logger { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Modal ref for the new project dialog.
        /// </summary>
        public BSModal NewProjectModal;

        /// <summary>
        /// New project form validation model.
        /// </summary>
        protected NewProjectModel NewProjectModel = new NewProjectModel();

        /// <summary>
        /// List of projects for the active user.
        /// </summary>
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
        /// <param name="e"></param>
        public void OpenNewProjectDialog(System.EventArgs e)
        {
            this.NewProjectModel = new NewProjectModel();
            NewProjectModal.Show();
        }


        /// <summary>
        /// Initialization for component.
        /// </summary>
        protected override void OnInitialized()
        {

            Projects = new List<XTMF2.Project>();
            Projects.AddRange(XtmfRuntime.ProjectController.GetProjects(XtmfUser));

        }

        /// <summary>
        /// Deletes a project for this user.
        /// </summary>
        public void DeleteProject(XTMF2.Project project)
        {
            string error = "";
            Projects.Remove(project);
            if (XtmfRuntime.ProjectController.DeleteProject(XtmfUser, project, ref error))
            {
                Logger.LogInformation($"Deleted project: {project.Name}");
            }
            else
            {
                Logger.LogError($"Unable to delete project: {project.Name} - {error}");
            }

        }

        /// <summary>
        /// Attempts to create a new project on submission of the new project form.
        /// </summary>
        protected void OnNewProjectFormSubmit()
        {
            string error = "";
            if ((XtmfRuntime.ProjectController.CreateNewProject(XtmfUser, NewProjectModel.ProjectName,
                out var session, ref error)))
            {
                Projects.Add(session.Project);
                Logger.LogInformation($"New project created: {session.Project.Name}");
                this.CloseNewProjectDialog();

            }
            else
            {
                Logger.LogError($"Unable to create new project:  {error}");
            }
        }

        /// <summary>
        /// Closes the new project dialog/
        /// </summary>
        /// <param name="e"></param>
        protected void CloseNewProjectDialog()
        {
            NewProjectModal.Hide();
        }

    }

}