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
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using XTMF2.UnitTests.Modules;
using XTMF2.Web.Data.Converters;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Controllers;
using XTMF2.Web.Server.Hubs;
using XTMF2.Web.Server.Mapping.Profiles;
using XTMF2.Web.Server.Session;
using Xunit;
using Xunit.Abstractions;

namespace XTMF2.Web.UnitTests.Controllers
{
    /// <summary>
    ///     Unit tests related to the ModelSystemEditorController
    /// </summary>
    public sealed class ModelSystemEditorControllerUnitTests : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly XTMFRuntime _runtime;
        private readonly ILogger<ModelSystemEditorController> _logger;
        private readonly ModelSystemEditorController _controller;
        private readonly UserSession _userSession;
        private readonly ProjectSessions _projectSessions;
        private readonly ModelSystemSessions _modelSystemSessions;
        private readonly ITestOutputHelper output;
        private readonly string _userName;
        private readonly User _user;

        /// <summary>
        /// </summary>
        /// <param name="output"></param>
        public ModelSystemEditorControllerUnitTests(ITestOutputHelper output)
        {
            var config = new MapperConfiguration(cfg =>
             {
                 cfg.AddProfile<ModelSystemProfile>();
                 cfg.AddProfile<ProjectProfile>();
             });
            _mapper = config.CreateMapper();
            _userName = Guid.NewGuid().ToString();
            _user = TestHelper.CreateTestUser(_userName);
            _runtime = TestHelper.Runtime;
            _logger = Mock.Of<ILogger<ModelSystemEditorController>>();
            _projectSessions = new ProjectSessions();
            _modelSystemSessions = new ModelSystemSessions(_mapper);
            _controller =
                new ModelSystemEditorController(_runtime, _logger, _projectSessions, _modelSystemSessions, _mapper,
                    Mock.Of<IHubContext<ModelSystemEditingHub>>());
            _userSession = new UserSession(_runtime.UserController.GetUserByName(_userName));
            this.output = output;
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            TestHelper.CleanUpTestContext(_runtime, _userName);
            _runtime.Shutdown();
        }

        /// <summary>
        ///     Tests that 200 OK is returned for valid model system
        /// </summary>
        [Fact]
        public void GetModelSystem_Returns200Ok_WhenQueryValidModelSystem()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            var result = _controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        /// <summary>
        ///     Tests that a 404 is returned for non existing model system
        /// </summary>
        [Fact]
        public void GetModelSystem_Returns404_WhenQueryNonExistingModelSystem()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            var result = _controller.GetModelSystem("TestProject", "TestModelSystemFake", _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        /// <summary>
        ///     Tests that retrieving a model system, returns the correct model system (and constructed properly)
        /// </summary>
        [Fact]
        public void GetModelSystem_ReturnsCorrectModelSystem_WhenQueryModelSystem()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            Assert.IsType<ModelSystemEditingModel>(result.Value);
            var modelSystem = (ModelSystemEditingModel)result.Value;

            // assert 
            Assert.Single(modelSystem.GlobalBoundary.Starts);
            Assert.Equal("TestStart", modelSystem.GlobalBoundary.Starts[0].Name);
            Assert.Collection(modelSystem.GlobalBoundary.Modules,
                item => { Assert.Equal("TestNode1", item.Name); });
            Assert.NotNull(modelSystem.GlobalBoundary.Modules[0].Type);
        }

        /// <summary>
        /// Tests that adding a boundary adds a boundary to the model system
        /// </summary>
        [Fact]
        public void AddBoundary_ReturnsCreatedResult_AndAddsBoundary()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            _runtime.ProjectController.GetProject(_user.UserName, "TestProject", out var project, out var error);
            _modelSystemSessions.TrackSessionForUser(_user, project, modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            var editingModel = (ModelSystemEditingModel)result.Value;
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(modelSystemSession);
            var globalBoundary = editingModel.GlobalBoundary;
            var addBoundaryResult = _controller.AddBoundary("TestProject", "TestModelSystem", globalBoundary.Id, new BoundaryModel()
            {
                Name = "TestBoundaryUnit",
                Location = new Data.Types.Rectangle()
                {
                    Height = 100,
                    Width = 100,
                    X = 50,
                    Y = 50
                }
            }, _userSession);
            Assert.IsAssignableFrom<CreatedResult>(addBoundaryResult);
            Assert.IsAssignableFrom<BoundaryModel>(((CreatedResult)addBoundaryResult).Value);
            //get reference to boundary model
            var returnedModel = (BoundaryModel)(((CreatedResult)addBoundaryResult).Value);
            Assert.True(tracker.ModelSystemEditingObjectReferenceMap.ContainsKey(returnedModel.Id));
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(tracker
                .ModelSystemEditingObjectReferenceMap[returnedModel.Id].ObjectReference));

        }

        /// <summary>
        /// Tests that adding a boundary to an invalid parent returns unprocessable entity
        /// </summary>
        [Fact]
        public void AddBoundary_ReturnsInvalid_FromInvalidParent()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            _runtime.ProjectController.GetProject(_user.UserName, "TestProject", out var project, out var error);
            _modelSystemSessions.TrackSessionForUser(_user, project, modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            var editingModel = (ModelSystemEditingModel)result.Value;
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(modelSystemSession);
            var globalBoundary = editingModel.GlobalBoundary;
            var addBoundaryResult = _controller.AddBoundary("TestProject", "TestModelSystem", Guid.Empty, new BoundaryModel()
            {
                Name = "TestBoundaryUnit",
                Location = new Data.Types.Rectangle()
                {
                    Height = 100,
                    Width = 100,
                    X = 50,
                    Y = 50
                }
            }, _userSession);
            Assert.IsAssignableFrom<UnprocessableEntityObjectResult>(addBoundaryResult);

        }

        /// <summary>
        /// Tests that adding a model system start returns correctly
        /// </summary>
        [Fact]
        public void AddStart_ReturnsCreatedResult_AndAddsBoundary()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            _runtime.ProjectController.GetProject(_user.UserName, "TestProject", out var project, out var error);
            _modelSystemSessions.TrackSessionForUser(_user, project, modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            var editingModel = (ModelSystemEditingModel)result.Value;
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(modelSystemSession);
            var globalBoundary = editingModel.GlobalBoundary;
            var addStartResult = _controller.AddModelSystemStart("TestProject", "TestModelSystem", globalBoundary.Id, new StartModel()
            {
                Name = "TestBoundaryUnit",
                Location = new Data.Types.Rectangle()
                {
                    Height = 100,
                    Width = 100,
                    X = 50,
                    Y = 50
                }
            }, _userSession);
            Assert.IsAssignableFrom<CreatedResult>(addStartResult);
            Assert.IsAssignableFrom<StartModel>(((CreatedResult)addStartResult).Value);
            //get reference to boundary model
            var returnedModel = (StartModel)(((CreatedResult)addStartResult).Value);
            Assert.True(tracker.ModelSystemEditingObjectReferenceMap.ContainsKey(returnedModel.Id));
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(tracker
                .ModelSystemEditingObjectReferenceMap[returnedModel.Id].ObjectReference));

        }

        /// <summary>
        /// Tests that adding a model system start returns correctly
        /// </summary>
        [Fact]
        public void AddCommentBlock_ReturnsCreatedResult_AndAddsBoundary()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            _runtime.ProjectController.GetProject(_user.UserName, "TestProject", out var project, out var error);
            _modelSystemSessions.TrackSessionForUser(_user, project, modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            var editingModel = (ModelSystemEditingModel)result.Value;
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(modelSystemSession);
            var globalBoundary = editingModel.GlobalBoundary;
            var addCommentBlockResult = _controller.AddCommentBlock("TestProject", "TestModelSystem", globalBoundary.Id, new CommentBlockModel()
            {
                Comment = "TestBoundaryUnit",
                Location = new Data.Types.Rectangle()
                {
                    Height = 100,
                    Width = 100,
                    X = 50,
                    Y = 50
                }
            }, _userSession);
            Assert.IsAssignableFrom<CreatedResult>(addCommentBlockResult);
            Assert.IsAssignableFrom<CommentBlockModel>(((CreatedResult)addCommentBlockResult).Value);
            //get reference to boundary model
            var returnedModel = (CommentBlockModel)(((CreatedResult)addCommentBlockResult).Value);
            Assert.True(tracker.ModelSystemEditingObjectReferenceMap.ContainsKey(returnedModel.Id));
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(tracker
                .ModelSystemEditingObjectReferenceMap[returnedModel.Id].ObjectReference));

        }

        /// <summary>
        /// Tests that adding a start to an invalid parent returns unprocessable entity
        /// </summary>
        [Fact]
        public void AddStart_ReturnsInvalid_FromInvalidParent()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            _runtime.ProjectController.GetProject(_user.UserName, "TestProject", out var project, out var error);
            _modelSystemSessions.TrackSessionForUser(_user, project, modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            var editingModel = (ModelSystemEditingModel)result.Value;
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(modelSystemSession);
            var globalBoundary = editingModel.GlobalBoundary;
            var addStartResult = _controller.AddModelSystemStart("TestProject", "TestModelSystem", Guid.Empty, new StartModel()
            {
                Name = "TestStartUnit",
                Location = new Data.Types.Rectangle()
                {
                    Height = 100,
                    Width = 100,
                    X = 50,
                    Y = 50
                }
            }, _userSession);
            Assert.IsAssignableFrom<UnprocessableEntityObjectResult>(addStartResult);
        }

        /// <summary>
        /// Tests that adding a start to an invalid parent returns unprocessable entity
        /// </summary>
        [Fact]
        public void AddCommentBlock_ReturnsInvalid_FromInvalidParent()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            _runtime.ProjectController.GetProject(_user.UserName, "TestProject", out var project, out var error);
            _modelSystemSessions.TrackSessionForUser(_user, project, modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            var editingModel = (ModelSystemEditingModel)result.Value;
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(modelSystemSession);
            var globalBoundary = editingModel.GlobalBoundary;
            var addCommentBlockResult = _controller.AddCommentBlock("TestProject", "TestModelSystem", Guid.Empty, new CommentBlockModel()
            {
                Comment = "TestStartUnit",
                Location = new Data.Types.Rectangle()
                {
                    Height = 100,
                    Width = 100,
                    X = 50,
                    Y = 50
                }
            }, _userSession);
            Assert.IsAssignableFrom<UnprocessableEntityObjectResult>(addCommentBlockResult);
        }

         /// <summary>
        /// Tests that adding a model system start returns correctly
        /// </summary>
        [Fact]
        public void AddNodeGenerateParameters_ReturnsCreatedResult_AndAddsNode()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            _runtime.ProjectController.GetProject(_user.UserName, "TestProject", out var project, out var error);
            _modelSystemSessions.TrackSessionForUser(_user, project, modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            var editingModel = (ModelSystemEditingModel)result.Value;
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(modelSystemSession);
            var globalBoundary = editingModel.GlobalBoundary;
            
            var addNodeResult = _controller.AddNodeGenerateParameters("TestProject", "TestModelSystem", globalBoundary.Id, new NodeModel()
            {
                Name = "TestNodeModule",
                Type = typeof(SimpleTestModule),
                Location = new Data.Types.Rectangle()
                {
                    Height = 100,
                    Width = 100,
                    X = 50,
                    Y = 50
                },

            }, _userSession);
            Assert.IsAssignableFrom<CreatedResult>(addNodeResult);
            Assert.IsAssignableFrom<NodeModel>(((CreatedResult)addNodeResult).Value);
            //get reference to boundary model
            var returnedModel = (NodeModel)(((CreatedResult)addNodeResult).Value);
            Assert.True(tracker.ModelSystemEditingObjectReferenceMap.ContainsKey(returnedModel.Id));
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(tracker
                .ModelSystemEditingObjectReferenceMap[returnedModel.Id].ObjectReference));
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(((NodeModel)tracker
                .ModelSystemEditingObjectReferenceMap[returnedModel.Id]).ContainedWithin.ObjectReference));
            Assert.True(tracker.ModelSystemEditingObjectReferenceMap.ContainsKey(((NodeModel)tracker
                .ModelSystemEditingObjectReferenceMap[returnedModel.Id]).ContainedWithin.Id));

        }
    }
}
