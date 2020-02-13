﻿//     Copyright 2017-2020 University of Toronto
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

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Interfaces.Editing;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Data.Types;
using XTMF2.Web.Server.Session;
using XTMF2.Web.Server.Utils;

namespace XTMF2.Web.Server.Controllers
{
    /// <summary>
    ///     API controller for the management of model systems (meta). This controller does not contain endpoints for the
    ///     editing
    ///     of model systems.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ModelSystemEditorController : ControllerBase
    {
        private XTMFRuntime _xtmfRuntime;
        private ProjectSessions _projectSessions;
        private ModelSystemSessions _modelSystemSessions;
        private IMapper _mapper;
        private ILogger<ModelSystemEditorController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="logger"></param>
        /// <param name="projectSessions"></param>
        /// <param name="modelSystemSessions"></param>
        /// <param name="mapper"></param>
        public ModelSystemEditorController(XTMFRuntime runtime, ILogger<ModelSystemEditorController> logger, ProjectSessions projectSessions,
            ModelSystemSessions modelSystemSessions, IMapper mapper)
        {
            _xtmfRuntime = runtime;
            _projectSessions = projectSessions;
            _modelSystemSessions = modelSystemSessions;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("projects/{projectName}/model-systems/{modelSystemName}/")]
        public IActionResult GetModelSystem(string projectName, string modelSystemName, [FromServices] UserSession userSession)
        {
            string error = default;
            if (!Utils.XtmfUtils.GetModelSystemHeader(_xtmfRuntime, userSession, _projectSessions, projectName, modelSystemName, out var modelSystemHeader, ref error))
            {
                return new NotFoundObjectResult(error);
            }
            if (!Utils.XtmfUtils.GetProjectSession(_xtmfRuntime, userSession, projectName, out var projectSession, _projectSessions, ref error))
            {
                return new NotFoundObjectResult(error);
            }
            if (!projectSession.EditModelSystem(userSession.User, modelSystemHeader, out var session, ref error))
            {
                return new UnprocessableEntityObjectResult(error);
            }

            return new OkObjectResult(_mapper.Map<ModelSystemEditingModel>(session.ModelSystem));
        }

        /// <summary>
        /// Opens a new model system editing session.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/open-session")]
        public IActionResult OpenSession(string projectName, string modelSystemName, [FromServices] UserSession userSession)
        {
            string error = default;
            if (!Utils.XtmfUtils.GetModelSystemHeader(_xtmfRuntime, userSession, _projectSessions, projectName, modelSystemName, out var modelSystemHeader, ref error))
            {
                return new NotFoundObjectResult(error);
            }
            if (!Utils.XtmfUtils.GetProjectSession(_xtmfRuntime, userSession, projectName, out var projectSession, _projectSessions, ref error))
            {
                return new NotFoundObjectResult(error);
            }
            if (!projectSession.EditModelSystem(userSession.User, modelSystemHeader, out var session, ref error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            return new OkResult();
        }

        /// <summary>
        /// Ends a model system editing session
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/end-session")]
        public IActionResult EndSession(string projectName, string modelSystemName, [FromServices] UserSession userSession)
        {
            string error = default;
            if (!Utils.XtmfUtils.GetModelSystemHeader(_xtmfRuntime, userSession, _projectSessions, projectName, modelSystemName, out var modelSystemHeader, ref error))
            {
                return new NotFoundObjectResult(error);
            }
            if (!Utils.XtmfUtils.GetProjectSession(_xtmfRuntime, userSession, projectName, out var projectSession, _projectSessions, ref error))
            {
                return new NotFoundObjectResult(error);
            }
            if (!projectSession.EditModelSystem(userSession.User, modelSystemHeader, out var session, ref error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            //dispose of the session
            session.Dispose();
            return new OkResult();
        }

        /// <summary>
        /// Adds a new boundary to the specified model system
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/boundary")]
        public IActionResult AddBoundary(string projectName, string modelSystemName, [FromBody] Path parentBoundaryPath,
        [FromBody] BoundaryModel boundary, [FromServices] UserSession userSession)
        {
            if (!Utils.XtmfUtils.GetModelSystemSession(_xtmfRuntime, userSession, projectName,
                modelSystemName, _projectSessions, _modelSystemSessions, out var modelSystemSession))
            {
                return new NotFoundObjectResult(null);
            }
            string error = default;
            Boundary parentBoundary = ModelSystemUtils.GetModelSystemObjectByPath<Boundary>(_xtmfRuntime, modelSystemSession, parentBoundaryPath);
            if (!modelSystemSession.AddBoundary(userSession.User, parentBoundary, boundary.Name, out var newBoundary, ref error))
            {
                return new UnprocessableEntityObjectResult(error);
            }

            return new CreatedResult("AddBoundary", newBoundary);
        }

        /// <summary>
        /// Adds a model start node to the model system.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/start")]
        public IActionResult AddModelSystemStart(string projectName, string modelSystemName, Path parentBoundaryPath,
                        [FromBody] BoundaryModel boundary, [FromServices] UserSession userSession)
        {
            if (!Utils.XtmfUtils.GetModelSystemSession(_xtmfRuntime, userSession, projectName,
                modelSystemName, _projectSessions, _modelSystemSessions, out var modelSystemSession))
            {
                return new NotFoundObjectResult(null);
            }
            string error = default;
            Boundary parentBoundary = ModelSystemUtils.GetModelSystemObjectByPath<Boundary>(_xtmfRuntime, modelSystemSession, parentBoundaryPath);
            if (!modelSystemSession.AddModelSystemStart(userSession.User, parentBoundary, boundary.Name, out var start, ref error))
            {
                return new UnprocessableEntityObjectResult(error);
            }

            return new CreatedResult("AddStart", start);
        }

        /// <summary>
        /// Adds a comment block to the specified model system
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="parentBoundaryPath"></param>
        /// <param name="commentBlock"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/comment-block")]
        public IActionResult AddCommentBlock(string projectName, string modelSystemName, [FromBody] Path parentBoundaryPath, [FromBody] CommentBlockModel commentBlock,
        [FromServices] UserSession userSession)
        {
            if (!Utils.XtmfUtils.GetModelSystemSession(_xtmfRuntime, userSession, projectName,
                modelSystemName, _projectSessions, _modelSystemSessions, out var modelSystemSession))
            {
                return new NotFoundObjectResult(null);
            }
            string error = default;
            Boundary parentBoundary = ModelSystemUtils.GetModelSystemObjectByPath<Boundary>(_xtmfRuntime, modelSystemSession, parentBoundaryPath);
            if (!modelSystemSession.AddCommentBlock(userSession.User, parentBoundary,
             commentBlock.Text, new Point(commentBlock.Location.Item1, commentBlock.Location.Item2), out var commentBlockRef, ref error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            return new CreatedResult("AddCommentBlock", commentBlockRef);
        }


        /// <summary>
        /// Adds a comment block to the specified model system
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="parentBoundaryPath"></param>
        /// <param name="commentBlock"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/link")]
        public IActionResult AddLink(string projectName, string modelSystemName, string parentBoundaryPath, [FromBody] CommentBlockModel commentBlock,
        [FromServices] UserSession userSession)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="parentBoundaryPath"></param>
        /// <param name="commentBlock"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/node-generate-parameters")]
        public IActionResult AddNodeGenerateParameteres(string projectName, string modelSystemName, string parentBoundaryPath,
                                                        [FromServices] UserSession userSession)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="parentBoundaryPath"></param>
        /// <param name="commentBlock"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/node")]
        public IActionResult AddNode(string projectName, string modelSystemName, string parentBoundaryPath, [FromBody] CommentBlockModel commentBlock,
                                    [FromServices] UserSession userSession)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="parentBoundaryPath"></param>
        /// <param name="commentBlock"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("projects/{projectName}/model-systems/{modelSystemName}/comment-block")]
        public IActionResult RemoveCommentBlock(string projectName, string modelSystemName, string parentBoundaryPath, [FromBody] CommentBlockModel commentBlock,
                                                [FromServices] UserSession userSession)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes the specified boundary from the model system.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="id"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("projects/{projectName}/model-systems/{modelSystemName}/boundary/{id}")]
        public IActionResult RemoveBoundary(string projectName, string modelSystemName, string id,
                                                [FromServices] UserSession userSession)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes a model system start object from the specified model system.
        /// </summary>
        /// <param name="projectName">The name of the project the model system belongs to.</param>
        /// <param name="modelSystemName">The name of the model system.</param>
        /// <param name="id">The id or path of the start element to remove.</param>
        /// <returns>OK result if successful, otherwise other error messages depending on result.</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("projects/{projectName}/model-systems/{modelSystemName}/model-system-start/{id}")]
        public IActionResult RemoveModelSystemStart(string projectName, string modelSystemName, string id)
        {
            //TODO
            throw new System.NotImplementedException();
        }


    }
}