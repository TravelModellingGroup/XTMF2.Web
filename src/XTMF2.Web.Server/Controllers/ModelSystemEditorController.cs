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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Controllers {
    /// <summary>
    ///     API controller for the management of model systems (meta). This controller does not contain endpoints for the
    ///     editing
    ///     of model systems.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ModelSystemEditorController : ControllerBase {
        private XTMFRuntime _xtmfRuntime;
        private ModelSystemSessions _sessions;
        private ProjectSessions _projectSessions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="sessions"></param>
        /// <param name="projectSessions"></param>
        public ModelSystemEditorController(XTMFRuntime runtime, ModelSystemSessions sessions, ProjectSessions projectSessions)
        {
            _xtmfRuntime = runtime;
            _sessions = sessions;
            _projectSessions = projectSessions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/open-session")]
        public ActionResult OpenSession(string projectName, string modelSystemName, [FromServices] UserSession userSession)
        {
            string error = default;
            if (!Utils.XtmfUtils.GetModelSystemHeader(_xtmfRuntime, userSession, _projectSessions, projectName, modelSystemName, out var modelSystemHeader, ref error)) {
                return new NotFoundObjectResult(error);
            }
            if (!Utils.XtmfUtils.GetProjectSession(_xtmfRuntime, userSession, projectName, out var projectSession, _projectSessions, ref error)) {
                return new NotFoundObjectResult(error);
            }
            if (!projectSession.EditModelSystem(userSession.User, modelSystemHeader, out var session, ref error)) {
                return new UnprocessableEntityObjectResult(error);
            }
            return new OkResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/end-session")]
        public ActionResult EndSession(string projectName, string modelSystemName, [FromServices] UserSession userSession)
        {
            string error = default;
            if (!Utils.XtmfUtils.GetModelSystemHeader(_xtmfRuntime, userSession, _projectSessions, projectName, modelSystemName, out var modelSystemHeader, ref error)) {
                return new NotFoundObjectResult(error);
            }
            if (!Utils.XtmfUtils.GetProjectSession(_xtmfRuntime, userSession, projectName, out var projectSession, _projectSessions, ref error)) {
                return new NotFoundObjectResult(error);
            }
            if (!projectSession.EditModelSystem(userSession.User, modelSystemHeader, out var session, ref error)) {
                return new UnprocessableEntityObjectResult(error);
            }
            //dispose of the session
            session.Dispose();
            return new OkResult();
        }
    }
}