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
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Server.Controllers {
    /// <summary>
    ///     API Controller for project related actions.
    /// </summary>
    [Route ("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase {
        private readonly ILogger<ProjectController> _logger;
        private readonly User _user;
        private readonly XTMFRuntime _xtmfRuntime;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ProjectController (XTMFRuntime runtime, User user, ILogger<ProjectController> logger,
            IMapper mapper) {
            _xtmfRuntime = runtime;
            _user = user;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        ///     Creates a new project from the model passed.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType (StatusCodes.Status201Created)]
        [ProducesResponseType (StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Create (string projectName) {
            var error = default (string);
            if (!_xtmfRuntime.ProjectController.CreateNewProject (_user, projectName,
                    out var session, ref error)) {
                _logger.LogError ($"Unable to create project: {session.Project.Name}\n" +
                    $"Error: {error}");
                return new UnprocessableEntityObjectResult (error);
            }

            _logger.LogInformation ($"New project created: {session.Project.Name}");
            return new CreatedResult (nameof (ProjectController), projectName);
        }

        /// <summary>
        ///     Gets the specified project.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        [HttpGet ("{projectName}")]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status200OK)]
        public ActionResult<ProjectModel> Get (string projectName) {
            string error = default;
            if (!_xtmfRuntime.ProjectController.GetProject (_user.UserName, projectName,
                    out var project, ref error)) {
                return new NotFoundResult ();
            }
            return new OkObjectResult (_mapper.Map<ProjectModel> (project));
        }

        /// <summary>
        ///     Lists all projects by the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType (StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProjectModel>> List () {
            var projects = XTMF2.Controllers.ProjectController.GetProjects (_user);
            return new OkObjectResult (_mapper.Map<List<ProjectModel>> (projects));
        }

        /// <summary>
        /// Deletes a project.
        /// </summary>
        /// <param name="projectName">The name of the project to delete.</param>
        /// <returns></returns>
        [HttpDelete ("{projectName}")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Delete (string projectName) {
            var error = default (string);
            if (!_xtmfRuntime.ProjectController.GetProject (_user, projectName, out var xtmfProject, ref error)) {
                return new NotFoundObjectResult (error);
            }

            if (!_xtmfRuntime.ProjectController.DeleteProject (_user, xtmfProject, ref error)) {
                _logger.LogError ($"Unable to delete project: {projectName}\n" +
                    $"Error: {error}");
                return new UnprocessableEntityObjectResult (error);

            }
            _logger.LogInformation ($"Project deleted: {projectName}");
            return new OkResult ();
        }
    }
}