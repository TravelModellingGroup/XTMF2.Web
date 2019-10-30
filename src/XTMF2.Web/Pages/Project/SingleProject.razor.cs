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

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace XTMF2.Web.Pages.Project {
	/// <summary>
	///     Single project view (page).
	/// </summary>
	public partial class SingleProject {
		[Inject] protected XTMFRuntime XtmfRuntime { get; set; }

		[Inject] protected User XtmfUser { get; set; }

		/// <summary>
		///     Path parameter that specifies the ProjectName
		/// </summary>
		[Microsoft.AspNetCore.Components.Parameter]
		public string ProjectName { get; set; }

		[Inject] protected ILogger<SingleProject> Logger { get; set; }

		/// <summary>
		///     The loaded project.
		/// </summary>
		protected XTMF2.Project Project { get; set; }

		/// <summary>
		///     Initialization function, will attempt to load the referenced project.
		/// </summary>
		protected override void OnInitialized () {
			string error = null;
			if (XtmfRuntime.ProjectController.GetProject (XtmfUser, ProjectName, out var project, ref error)) {
				Project = project;
			} else {
				Logger.LogError ("Unable to load project, or project not found: " + ProjectName);
			}
		}
	}
}