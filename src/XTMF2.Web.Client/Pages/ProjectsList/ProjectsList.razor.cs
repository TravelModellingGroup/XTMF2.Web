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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BlazorStrap;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Client.Api;
using XTMF2.Web.Components.Util;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Pages {
	/// <summary>
	///     Projects (page) base component. This page lists the currently existing projects
	///     for the current active user. The user can both add and delete projects from this page.
	/// </summary>
	public partial class ProjectsList : ComponentBase {
		/// <summary>
		///     Modal ref for the new project dialog.
		/// </summary>
		public BSModal NewProjectModal;

		/// <summary>
		///     New project form validation model.
		/// </summary>
		private InputRequestDialog _inputRequestDialog;

		[Inject] protected ProjectClient _projectClient { get; set; }

		[Inject] protected UserModel XtmfUser { get; set; }

		[Inject] protected ILogger<ProjectsList> Logger { get; set; }

		[Inject] private NavigationManager NavigationManager { get; set; }

		/// <summary>
		///     List of projects for the active user.
		/// </summary>
		public ReadOnlyObservableCollection<ProjectModel> Projects { get; set; }

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		public void OpenNewProjectDialog (EventArgs e) {
			InputRequestDialog.Show ();
		}

		/// <summary>
		///     Initialization for component.
		/// </summary>
		protected override Task OnInitializedAsync () {

			// Projects =  ProjectController.GetProjects (XtmfUser);
			return base.OnInitializedAsync ();
		}

		/// <summary>
		///     Deletes a project for this user.
		/// </summary>
		public void DeleteProject (ProjectModel project) {
			/* string error = null;
			if (XtmfRuntime.ProjectController.DeleteProject (XtmfUser, project, ref error)) {
				Logger.LogInformation ($"Deleted project: {project.Name}");
			} else {
				Logger.LogError ($"Unable to delete project: {project.Name} - {error}");
			} */
		}

		/// <summary>
		///     Attempts to create a new project on submission of the new project form.
		/// </summary>
		protected void OnNewProjectFormSubmit (string input) {
			/* string error = null;
			if (XtmfRuntime.ProjectController.CreateNewProject (XtmfUser, input,
					out var session, ref error)) {
				Logger.LogInformation ($"New project created: {session.Project.Name}");
				CloseNewProjectDialog ();
			} else {
				Logger.LogError ($"Unable to create new project:  {error}");
			} */
		}

		/// <summary>
		///     Closes the new project dialog/
		/// </summary>
		/// <param name="e"></param>
		protected void CloseNewProjectDialog () {
			NewProjectModal.Hide ();

		}
	}
}