using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Server.Controllers;
using XTMF2.Web.Server.Profiles;
using XTMF2.Web.Server.Session;
using Xunit;

namespace XTMF2.Web.UnitTests.Controllers
{
    /// <summary>
    /// Unit tests related to the ModelSystemController
    /// </summary>
    [Collection("Sequential")]
    public class ModelSystemControllerUnitTests : IDisposable
    {

        private IMapper _mapper;
        private XTMFRuntime _runtime;
        private ILogger<ModelSystemController> _logger;
        private ModelSystemController _controller;
        private UserSession _userSession;
        private ProjectSessions _projectSessions;
        private ProjectController _projectController;
        private string _userName;

        public ModelSystemControllerUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelSystemProfile>();
                cfg.AddProfile<ProjectProfile>();
            });
            _userName = Guid.NewGuid().ToString();
            _mapper = config.CreateMapper();
            _runtime = TestHelper.CreateTestContext(_userName);
            _logger = Mock.Of<ILogger<ModelSystemController>>();
            _projectSessions = new ProjectSessions();
            _controller = new ModelSystemController(_runtime, _logger, _mapper, _projectSessions);
            _userSession = new UserSession(_runtime.UserController.GetUserByName(_userName));
            _projectController = new ProjectController(_runtime, Mock.Of<ILogger<ProjectController>>(), _mapper, _projectSessions);
        }

        /// <summary>
        /// Tests creation of a model system and returns the appropriate value
        /// </summary>
        [Fact]
        public void CreatePost_ReturnsCreatedResult_WhenModelSystemCreated()
        {
            _projectController.Create("projectName", _userSession);
            var result = _controller.Create("projectName", new ModelSystemModel()
            {
                Description = "Description",
                Name = "NameTest"
            }, _userSession);
            var queryResult = _controller.Get("projectName", "NameTest", _userSession);
            Assert.IsAssignableFrom<CreatedResult>(result);
            Assert.Equal("NameTest", (((ModelSystemModel)((OkObjectResult)queryResult).Value).Name));

        }

        /// <summary>
        /// Tests that a model system cannot be created with an invalid project.
        /// </summary>
        [Fact]
        public void CreatePost_ReturnsNotFound_WhenModelSystemCreatedWithInvalidProject()
        {
            _projectController.Create("projectName", _userSession);
            var result = _controller.Create("projectNameInvalid", new ModelSystemModel()
            {
                Description = "Description",
                Name = "Name"
            }, _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Tests that retrieval of a valid model system returns the correct model system.
        /// </summary>
        [Fact]
        public void ModelSystemGet_ReturnsOkObject_WhenValidRetrieved()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel()
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            var result = _controller.Get("projectName", "MSName", _userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }


        /// <summary>
        /// Tests that retrieval of a valid model system returns the correct model system.
        /// </summary>
        [Fact]
        public void ModelSystemGet_ReturnsNotFound_WhenInvalidRetrieved()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel()
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            var result = _controller.Get("projectName", "MSNameWrong", _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Tests that multiple calls to get model system keeps a single reference to project session
        /// </summary>
        [Fact]
        public void ModelSystemGet_MaintainsCorrectProjectSessionReference()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel()
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            _controller.Get("projectName", "MSName", _userSession);
            _controller.Get("projectName", "MSName", _userSession);
            _controller.Get("projectName", "MSName", _userSession);
            Assert.Single(_projectSessions.Sessions[_userSession.User]);
            _projectSessions.Sessions[_userSession.User][0].Dispose();
        }

        /// <summary>
        /// Tests that an appropriate accessed model system is deleted.
        /// </summary>
        [Fact]
        public void ModelSystemDelete_ReturnsOkResult_WhenValidModelSystemDeleted()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel()
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            var result = _controller.Delete("projectName", "MSName", _userSession);

            // end project sessions 
            Assert.IsAssignableFrom<OkResult>(result);
        }

        /// <summary>
        /// Tests for not found result when deleting a model system that does not exist.
        /// </summary>
        [Fact]
        public void ModelSystemDelete_ReturnsNotFoundObjectResult_WhenInvalidModelSystemDeleted()
        {
            _projectController.Create("projectName2", _userSession);
            _controller.Create("projectName2", new ModelSystemModel()
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            var result = _controller.Delete("projectName2", "MSNameWrong", _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);

        }

        public void Dispose()
        {
            if (_projectSessions.Sessions.ContainsKey(_userSession.User))
            {
                _projectSessions.Sessions[_userSession.User].ForEach((i) =>
                            {
                                i.Dispose();
                            });
            }

            TestHelper.CleanUpTestContext(_runtime, _userName);
            _runtime.Shutdown();
        }
    }
}