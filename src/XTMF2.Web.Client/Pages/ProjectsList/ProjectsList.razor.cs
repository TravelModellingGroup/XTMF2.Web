//     Copyright 2017-2020 University of Toronto
// 
//     This file is part of XTMF2.
// 
//     XTMF2 is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     XTMF2 is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorStrap;
using Microsoft.AspNetCore.Components;
using Serilog;
using XTMF2.Web.Client.Services;
using XTMF2.Web.Components.Util;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Client.Pages {
    /// <summary>
    ///     Projects (page) base component. This page lists the currently existing projects
    ///     for the current active user. The user can both add and delete projects from this page.
    /// </summary>
    public partial class ProjectsList : ComponentBase {
        /// <summary>
        ///     New project form validation model.
        /// </summary>
        private InputRequestDialog _inputRequestDialog;

        /// <summary>
        ///     Modal ref for the new project dialog.
        /// </summary>
        public BSModal NewProjectModal;

        [Inject] protected ProjectClient ProjectClient { get; set; }


        protected ILogger Logger { get; set; } = Log.ForContext<ProjectsList> ();

        [Inject] private NavigationManager NavigationManager { get; set; }

        /// <summary>
        ///     List of projects for the active user.
        /// </summary>
        public List<ProjectModel> Projects { get; set; } = new List<ProjectModel> ();

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        public void OpenNewProjectDialog (EventArgs e) {
            _inputRequestDialog.Show ();
        }

        /// <summary>
        ///     Initialization for component.
        /// </summary>
        protected override async Task OnInitializedAsync () {
            Logger.Information ("Projects List loading.");

            var s = await ProjectClient.ListAsync ();
            Projects.AddRange (s);
        }

        /// <summary>
        ///     Deletes a project for this user.
        /// </summary>
        public void DeleteProject (ProjectModel project) {
            throw new NotImplementedException ();
        }

        /// <summary>
        ///     Attempts to create a new project on submission of the new project form.
        /// </summary>
        protected async void OnNewProjectFormSubmit (string input) {
            await ProjectClient.CreateAsync (input);
            Logger.Information ("Project created");

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