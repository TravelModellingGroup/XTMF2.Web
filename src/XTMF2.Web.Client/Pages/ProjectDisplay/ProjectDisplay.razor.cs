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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Components.Util;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Pages {
    /// <summary>
    ///     Single project view (page).
    /// </summary>
    public partial class ProjectDisplay : ComponentBase {

        private InputRequestDialog _inputDialog;

        [Inject]
        protected UserModel XtmfUser { get; set; }

        /// <summary>
        ///     Path parameter that specifies the ProjectName
        /// </summary>
        [Microsoft.AspNetCore.Components.Parameter]
        public string ProjectName { get; set; }

        [Inject]
        protected ILogger<ProjectDisplay> Logger { get; set; }

        /// <summary>
        ///     Model systems belonging to the project
        /// </summary>
        public IReadOnlyCollection<ModelSystemModel> ModelSystems { get; set; } = new List<ModelSystemModel> ();

        /// <summary>
        ///     The loaded project.
        /// </summary>
        protected ProjectModel Project { get; set; }

        protected void NewModelSystemSubmit (string input) {
            /*
            string error = null;
            if (!_projectSession.CreateNewModelSystem(XtmfUser, input, out var modelSystem, ref error))
            {
                Logger.LogError("Unable to create model system: " + input);
            }
            else
            {
                Logger.LogInformation("Model system created: " + input);
            } */
        }

        /// <summary>
        ///     Callback for deleting the passed model system.
        /// </summary>
        /// <param name="modelSystem"></param>
        protected void DeleteModelSystem (ModelSystemModel modelSystem) {
            /*
            string error = null;
            if (!_projectSession.RemoveModelSystem(XtmfUser, modelSystem, ref error))
            {
                Logger.LogError("Unable to remove model system: " + modelSystem.Name);
            }
            else
            {
                Logger.LogInformation("Model system removed: " + modelSystem.Name);
            } */
        }

        /// <summary>
        ///     Initialization function, will attempt to load the referenced project.
        /// </summary>
        protected override Task OnInitializedAsync () {
            /*
            string error = null;
            if (XtmfRuntime.ProjectController.GetProject(XtmfUser, ProjectName, out var project, ref error))
            {
                Project = project;
                ModelSystems = project.ModelSystems;
                XtmfRuntime.ProjectController.GetProjectSession(XtmfUser, Project, out _projectSession, ref error);
            }
            else
            {
                ModelSystems = new List<ModelSystemHeader>();
                Logger.LogError("Unable to load project, or project not found: " + ProjectName);
            }
 */
            return base.OnInitializedAsync ();
        }
    }
}