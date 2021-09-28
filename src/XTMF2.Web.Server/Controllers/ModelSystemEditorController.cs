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
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using XTMF2.ModelSystemConstruct;
using XTMF2.Web.Data.Interfaces.Editing;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Data.Types;
using XTMF2.Web.Server.Authorization;
using XTMF2.Web.Server.Hubs;
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
        private readonly XTMFRuntime _xtmfRuntime;
        private readonly ProjectSessions _projectSessions;
        private readonly ModelSystemSessions _modelSystemSessions;
        private readonly IMapper _mapper;
        private ILogger<ModelSystemEditorController> _logger;
        private IHubContext<ModelSystemEditingHub> _editingHub;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="logger"></param>
        /// <param name="projectSessions"></param>
        /// <param name="modelSystemSessions"></param>
        /// <param name="mapper"></param>
        /// <param name="editingHub"></param>
        public ModelSystemEditorController(XTMFRuntime runtime, ILogger<ModelSystemEditorController> logger, ProjectSessions projectSessions,
            ModelSystemSessions modelSystemSessions, IMapper mapper, IHubContext<ModelSystemEditingHub> editingHub)
        {
            _xtmfRuntime = runtime;
            _projectSessions = projectSessions;
            _modelSystemSessions = modelSystemSessions;
            _mapper = mapper;
            _logger = logger;
            _editingHub = editingHub;
        }

        /// <summary>
        /// Utility request handler
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="userSession"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool HandleRequest(string projectName, string modelSystemName, UserSession userSession, out Editing.ModelSystemSession session, out IActionResult result)
        {
            if (!Utils.XtmfUtils.GetModelSystemHeader(_xtmfRuntime, userSession, _projectSessions, projectName, modelSystemName, out var modelSystemHeader, out var error))
            {
                result = error.UnauthorizedUser ? new UnauthorizedResult() : (IActionResult)(new NotFoundObjectResult(error));
                session = null;
                return false;
            }
            if (!Utils.XtmfUtils.GetProjectSession(_xtmfRuntime, userSession, projectName, out var projectSession, _projectSessions, out error))
            {
                result = new NotFoundObjectResult(error);
                session = null;
                return false;
            }
            if (!projectSession.EditModelSystem(userSession.User, modelSystemHeader, out session, out error))
            {
                result = new UnprocessableEntityObjectResult(error);
                return false;
            }
            result = null;
            return true;
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
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            // determine if the model system edting model exists, otherwise add it
            if (!_modelSystemSessions.ModelSystemEditingModels.TryGetValue(session, out var editingModel))
            {
                editingModel = _mapper.Map<ModelSystemEditingModel>(session.ModelSystem);
                _modelSystemSessions.ModelSystemEditingModels[session] = editingModel;
            }
            return new OkObjectResult(editingModel);
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
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
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
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
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
        /// <param name="parentBoundaryPath"></param>
        /// <param name="boundary"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/boundary")]
        public IActionResult AddBoundary(string projectName, string modelSystemName, [FromQuery] Guid parentId,
        [FromBody] BoundaryModel boundary, [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            if (!tracker.TryGetModelSystemObject<Boundary>(parentId, out var parentBoundary))
            {
                return new UnprocessableEntityObjectResult("Specified parent boundary does not exist.");
            }
            if (!session.AddBoundary(userSession.User, parentBoundary, boundary.Name, out var newBoundary, out var error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            // return the added boundary from the tracker
            return new CreatedResult("AddBoundary", tracker.ModelSystemObjectReferenceMap[newBoundary]);
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
        public IActionResult AddModelSystemStart(string projectName, string modelSystemName, [FromQuery] Guid parentId,
                        [FromBody] StartModel startModel, [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            if (!tracker.TryGetModelSystemObject<Boundary>(parentId, out var parentBoundary))
            {
                return new UnprocessableEntityObjectResult("Specified parent boundary does not exist.");
            }
            if (!session.AddModelSystemStart(userSession.User, parentBoundary, startModel.Name, 
                new Rectangle(startModel.Location.X, startModel.Location.Y, startModel.Location.Width, startModel.Location.Height), 
                out var start, out var error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            return new CreatedResult("AddStart", tracker.ModelSystemObjectReferenceMap[start]);
        }

        /// <summary>
        /// Adds a comment block to the specified model system
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="parentId"></param>
        /// <param name="commentBlock"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/comment-block")]
        public IActionResult AddCommentBlock(string projectName, string modelSystemName, [FromQuery] Guid parentId, [FromBody] CommentBlockModel commentBlock,
        [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            if (!tracker.TryGetModelSystemObject<Boundary>(parentId, out var parentBoundary))
            {
                return new UnprocessableEntityObjectResult("Specified parent boundary does not exist.");
            }
            if (!session.AddCommentBlock(userSession.User, parentBoundary,
             commentBlock.Comment, new Rectangle(commentBlock.Location.X, commentBlock.Location.Y, commentBlock.Location.Width, commentBlock.Location.Height), out var commentBlockRef, out var error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            
            return new CreatedResult("AddCommentBlock", tracker.ModelSystemObjectReferenceMap[commentBlockRef]);
        }


        /// <summary>
        /// Adds a comment block to the specified model system
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="commentBlock"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/link")]
        public IActionResult AddLink(string projectName, string modelSystemName, [FromQuery] Guid originNodeId, [FromQuery] Guid destinationNodeId,
        [FromQuery] Guid originHookId,
        [FromBody] CommentBlockModel commentBlock,
        [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            Node originNode = tracker.GetModelSystemObject<Node>(originNodeId);
            Node destinationNode = tracker.GetModelSystemObject<Node>(destinationNodeId);
            NodeHook originHook = tracker.GetModelSystemObject<NodeHook>(originHookId);
            if (!session.AddLink(userSession.User, originNode, originHook, destinationNode, out var link, out var error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            return new CreatedResult(nameof(AddLink), tracker.ModelSystemObjectReferenceMap[link]);
        }

        /// <summary>
        /// Add a node with generated parameters
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="parentBoundaryId"></param>
        /// <param name="nodeModel"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/node-generate-parameters")]
        public IActionResult AddNodeGenerateParameters(string projectName, string modelSystemName, [FromQuery] Guid parentId,
                                                        [FromBody] NodeModel nodeModel, 
                                                        [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            if (!tracker.TryGetModelSystemObject<Boundary>(parentId, out var parentBoundary))
            {
                return new UnprocessableEntityObjectResult("Specified parent boundary does not exist.");
            }
            if (!session.AddNodeGenerateParameters(userSession.User, parentBoundary, nodeModel.Name, nodeModel.Type, 
                new Rectangle(nodeModel.Location.X, nodeModel.Location.Y, nodeModel.Location.Width, nodeModel.Location.Height),
                out var node, out var children, out var commandError))
            {
               return new UnprocessableEntityObjectResult(commandError);
            }
            return new CreatedResult(nameof(AddNode), tracker.ModelSystemObjectReferenceMap[node]);
        }

        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="parentId"></param>
        /// <param name="userSession"></param>
        /// <param name="nodeModel"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("projects/{projectName}/model-systems/{modelSystemName}/node")]
        public IActionResult AddNode(string projectName, string modelSystemName, [FromBody] NodeModel nodeModel, [FromQuery] Guid parentId,

                                    [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            if (!tracker.TryGetModelSystemObject<Boundary>(parentId, out var parentBoundary))
            {
                return new UnprocessableEntityObjectResult("Specified parent boundary does not exist.");
            }
            if (!session.AddNode(userSession.User, parentBoundary, nodeModel.Name, nodeModel.Type, 
                new Rectangle(nodeModel.Location.X, nodeModel.Location.Y, nodeModel.Location.Width, nodeModel.Location.Height),
                out var node, out var error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            return new CreatedResult(nameof(AddNode), node);
        }

        /// <summary>
        /// Removes the specified comment block from the parent boundary.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("projects/{projectName}/model-systems/{modelSystemName}/comment-block")]
        public IActionResult RemoveCommentBlock(string projectName, string modelSystemName, [FromQuery] Guid parentBoundaryId, [FromQuery] Guid commentBlockId,
                                                [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            Boundary parentBoundary = tracker.GetModelSystemObject<Boundary>(parentBoundaryId);
            CommentBlock commentBlock = tracker.GetModelSystemObject<CommentBlock>(commentBlockId);
            if (!session.RemoveCommentBlock(userSession.User, parentBoundary, commentBlock, out var error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            return new OkResult();
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
        public IActionResult RemoveBoundary(string projectName, string modelSystemName, [FromQuery] Guid parentBoundaryId, [FromQuery] Guid boundaryId,
                                                [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            Boundary parentBoundary = tracker.GetModelSystemObject<Boundary>(parentBoundaryId);
            Boundary boundary = tracker.GetModelSystemObject<Boundary>(boundaryId);
            if (!session.RemoveBoundary(userSession.User, parentBoundary, boundary, out var error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            return new OkResult();
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
        public IActionResult RemoveModelSystemStart(string projectName, string modelSystemName, [FromQuery] Guid startId, [FromServices] UserSession userSession)
        {
            if (!HandleRequest(projectName, modelSystemName, userSession, out var session, out var requestResult))
            {
                return requestResult;
            }
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            Start start = tracker.GetModelSystemObject<Start>(startId);
            if (!session.RemoveStart(userSession.User, start, out var error))
            {
                return new UnprocessableEntityObjectResult(error);
            }
            return new OkResult();
        }


    }
}