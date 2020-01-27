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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Server.Controllers
{
    /// <summary>
    ///     API Controller for project related actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<ProjectController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="userManager"></param>
        public ProjectController(XTMFRuntime runtime, IHttpContextAccessor httpContextAccessor,
            ILogger<ProjectController> logger,
            IMapper mapper, UserManager<User> userManager)
        {
            _xtmfRuntime = runtime;
            _logger = logger;
            _mapper = mapper;
            _httpContext = httpContextAccessor;
            _userManager = userManager;
        }

        /// <summary>
        ///     Creates a new project from the model passed. A model representing the newly created
        ///     project is returned to the caller.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProjectModel), StatusCodes.Status201Created)]
        public IActionResult Create(string projectName, [FromServices] User user)
        {
            var error = default(string);
            if (!_xtmfRuntime.ProjectController.CreateNewProject(user, projectName,
                out var session, ref error)) {
                _logger.LogError($"Unable to create project: {session.Project.Name}\n" +
                                 $"Error: {error}");
                return new UnprocessableEntityObjectResult(error);
            }

            _logger.LogInformation($"New project created: {session.Project.Name}");
            return new CreatedResult(nameof(ProjectController), _mapper.Map<ProjectModel>(session.Project));
        }

        /// <summary>
        ///     Gets the specified project.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        [HttpGet("{projectName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProjectModel), StatusCodes.Status201Created)]
        public ActionResult<ProjectModel> Get(string projectName, [FromServices] User user)
        {
            string error = default;
            if (!_xtmfRuntime.ProjectController.GetProject(user.UserName, projectName,
                out var project, ref error)) {
                return new NotFoundResult();
            }

            return new OkObjectResult(_mapper.Map<ProjectModel>(project));
        }

        /// <summary>
        ///     Lists all projects by the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<ProjectModel>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProjectModel>> List([FromServices] User user)
        {
            var projects = XTMF2.Controllers.ProjectController.GetProjects(user);
            return new OkObjectResult(_mapper.Map<List<ProjectModel>>(projects));
        }

        /// <summary>
        ///     Deletes a project.
        /// </summary>
        /// <param name="projectName">The name of the project to delete.</param>
        /// <returns></returns>
        [HttpDelete("{projectName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Delete(string projectName, [FromServices] User user)
        {
            var error = default(string);
            if (!_xtmfRuntime.ProjectController.GetProject(user, projectName, out var xtmfProject, ref error))
            {
                return new NotFoundObjectResult(error);
            }

            if (!_xtmfRuntime.ProjectController.DeleteProject(user, xtmfProject, ref error))
            {
                _logger.LogError($"Unable to delete project: {projectName}\n" +
                                 $"Error: {error}");
                return new UnprocessableEntityObjectResult(error);
            }

            _logger.LogInformation($"Project deleted: {projectName}");
            return new OkResult();
        }
    }
}