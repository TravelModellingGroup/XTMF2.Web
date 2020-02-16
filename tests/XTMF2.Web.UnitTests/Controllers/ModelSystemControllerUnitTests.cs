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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Server.Controllers;
using XTMF2.Web.Server.Mapping.Profiles;
using XTMF2.Web.Server.Session;
using Xunit;

namespace XTMF2.Web.UnitTests.Controllers
{
    /// <summary>
    ///     Unit tests related to the ModelSystemController
    /// </summary>
    public class ModelSystemControllerUnitTests : IDisposable
    {
        private readonly XTMFRuntime _runtime;
        private readonly ModelSystemController _controller;
        private readonly UserSession _userSession;
        private readonly ProjectSessions _projectSessions;
        private readonly ProjectController _projectController;
        private readonly string _userName;

        public ModelSystemControllerUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelSystemProfile>();
                cfg.AddProfile<ProjectProfile>();
            });
            _userName = Guid.NewGuid().ToString();
            var mapper = config.CreateMapper();
            TestHelper.CreateTestUser(_userName);
            _runtime = TestHelper.Runtime;
            _projectSessions = new ProjectSessions();
            _controller = new ModelSystemController(_runtime, Mock.Of<ILogger<ModelSystemController>>(), mapper, _projectSessions);
            _userSession = new UserSession(_runtime.UserController.GetUserByName(_userName));
            _projectController = new ProjectController(_runtime, Mock.Of<ILogger<ProjectController>>(), mapper,
                _projectSessions);
        }

        public void Dispose()
        {
            if (_projectSessions.Sessions.ContainsKey(_userSession.User))
            {
                _projectSessions.Sessions[_userSession.User].ForEach(i => { i.Dispose(); });
            }

            TestHelper.CleanUpTestContext(_runtime, _userName);
            _runtime.Shutdown();
        }

        /// <summary>
        ///     Tests creation of a model system and returns the appropriate value
        /// </summary>
        [Fact]
        public void CreatePost_ReturnsCreatedResult_WhenModelSystemCreated()
        {
            _projectController.Create("projectName", _userSession);
            var result = _controller.Create("projectName", new ModelSystemModel
            {
                Description = "Description",
                Name = "NameTest"
            }, _userSession);
            var queryResult = _controller.Get("projectName", "NameTest", _userSession);
            Assert.IsAssignableFrom<CreatedResult>(result);
            Assert.Equal("NameTest", ((ModelSystemModel)((OkObjectResult)queryResult).Value).Name);
        }

        /// <summary>
        ///     Tests that a model system cannot be created with an invalid project.
        /// </summary>
        [Fact]
        public void CreatePost_ReturnsNotFound_WhenModelSystemCreatedWithInvalidProject()
        {
            _projectController.Create("projectName", _userSession);
            var result = _controller.Create("projectNameInvalid", new ModelSystemModel
            {
                Description = "Description",
                Name = "Name"
            }, _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        /// <summary>
        ///     Tests for not found result when deleting a model system that does not exist.
        /// </summary>
        [Fact]
        public void ModelSystemDelete_ReturnsNotFoundObjectResult_WhenInvalidModelSystemDeleted()
        {
            _projectController.Create("projectName2", _userSession);
            _controller.Create("projectName2", new ModelSystemModel
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            var result = _controller.Delete("projectName2", "MSNameWrong", _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        /// <summary>
        ///     Tests that an appropriate accessed model system is deleted.
        /// </summary>
        [Fact]
        public void ModelSystemDelete_ReturnsOkResult_WhenValidModelSystemDeleted()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            var result = _controller.Delete("projectName", "MSName", _userSession);

            // end project sessions 
            Assert.IsAssignableFrom<OkResult>(result);
        }

        /// <summary>
        ///     Tests that multiple calls to get model system keeps a single reference to project session
        /// </summary>
        [Fact]
        public void ModelSystemGet_MaintainsCorrectProjectSessionReference()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel
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
        ///     Tests that retrieval of a valid model system returns the correct model system.
        /// </summary>
        [Fact]
        public void ModelSystemGet_ReturnsNotFound_WhenInvalidRetrieved()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            var result = _controller.Get("projectName", "MSNameWrong", _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        /// <summary>
        ///     Tests that retrieval of a valid model system returns the correct model system.
        /// </summary>
        [Fact]
        public void ModelSystemGet_ReturnsOkObject_WhenValidRetrieved()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            var result = _controller.Get("projectName", "MSName", _userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        /// <summary>
        ///     Tests that retrieval of a valid model system returns the correct model system.
        /// </summary>
        [Fact]
        public void ModelSystemList_ReturnsOkObject_WhenValidRetrieved()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            _controller.Create("projectName", new ModelSystemModel
            {
                Description = "Description",
                Name = "MSName2"
            }, _userSession);
            var result = _controller.List("projectName", _userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<List<ModelSystemModel>>(((OkObjectResult)result).Value);
            Assert.Collection((IEnumerable<ModelSystemModel>)((OkObjectResult)result).Value, item =>
            {
                item.Name = "MSName";
            }, item =>
            {
                item.Name = "MSNam2";
            });
        }

                /// <summary>
        ///     Tests that retrieval of a valid model system returns the correct model system.
        /// </summary>
        [Fact]
        public void ModelSystemList_ReturnsOkObjectEmptyList_WhenNoModelSystems()
        {
            _projectController.Create("projectName", _userSession);
            _controller.Create("projectName", new ModelSystemModel
            {
                Description = "Description",
                Name = "MSName"
            }, _userSession);
            _controller.Create("projectName", new ModelSystemModel
            {
                Description = "Description",
                Name = "MSName2"
            }, _userSession);
            var result = _controller.List("projectName", _userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<List<ModelSystemModel>>(((OkObjectResult)result).Value);
            Assert.Empty((IEnumerable<ModelSystemModel>)((OkObjectResult)result).Value);
        }
    }
}