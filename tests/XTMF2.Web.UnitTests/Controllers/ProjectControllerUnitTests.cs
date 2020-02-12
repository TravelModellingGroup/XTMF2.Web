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
using XTMF2.Web.Server.Profiles;
using XTMF2.Web.Server.Session;
using Xunit;

namespace XTMF2.Web.UnitTests.Controllers
{
    /// <summary>
    /// Unit tests related to the ProjectController
    /// </summary>
    public class ProjectControllerUnitTests : IDisposable
    {

        private IMapper _mapper;
        private XTMFRuntime _runtime;
        private ILogger<ProjectController> _logger;
        private ProjectController _controller;
        private UserSession _userSession;
        private ProjectSessions _projectSessions;
        private string _userName;

        public ProjectControllerUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProjectProfile>();
            });
            _userName = Guid.NewGuid().ToString();
            _mapper = config.CreateMapper();
            TestHelper.CreateTestUser(_userName);
            _runtime = TestHelper.Runtime;
            _logger = Mock.Of<ILogger<ProjectController>>();
            _userSession = new UserSession(_runtime.UserController.GetUserByName(_userName));
            _projectSessions = new ProjectSessions();
            _controller = new ProjectController(_runtime, _logger, _mapper, _projectSessions);


        }

        /// <summary>
        /// Tests if the projects list returns a valid list for the default (local) user.
        /// </summary>
        [Fact]
        public void IndexGet_ReturnsValidList()
        {
            var projects = _controller.List(_userSession);
            //assert
            Assert.IsAssignableFrom<ActionResult<IEnumerable<ProjectModel>>>(projects);
        }

        /// <summary>
        /// Tests creating a user, and that the controller returns the correct response type.
        /// </summary>
        [Fact]
        public void CreatePost_ReturnsCreatedResult_WhenProjectCreated()
        {
            var result = _controller.Create("projectName", _userSession);
            var projects = _controller.List(_userSession);
            //assert
            Assert.IsAssignableFrom<CreatedResult>(result);
            Assert.IsAssignableFrom<OkObjectResult>(projects.Result);
            Assert.Single(((List<ProjectModel>)((OkObjectResult)projects.Result).Value));

        }

        /// <summary>
        /// Test that project creation wont accept invalid name / model.
        /// </summary>
        [Fact]
        public void CreatePost_ReturnsInvalid_WhenProjectCreated()
        {
            var result = _controller.Create("", _userSession);
            var projects = _controller.List(_userSession);
            //assert
            Assert.IsAssignableFrom<UnprocessableEntityObjectResult>(result);
            Assert.IsAssignableFrom<OkObjectResult>(projects.Result);
            Assert.Empty(((List<ProjectModel>)((OkObjectResult)projects.Result).Value));

        }

        /// <summary>
        /// Tests deleting a single project
        /// </summary>
        [Fact]
        public void DeletePost_ReturnsValid_WhenProjectDeleted()
        {
            _controller.Create("projectName", _userSession);
            var result = _controller.Delete("projectName", _userSession);
            //assert
            Assert.IsAssignableFrom<OkResult>(result);
        }
        /// <summary>
        /// Tests deletion of a non-existing project
        /// </summary>
        [Fact]
        public void DeletePost_ReturnsNotFound_WhenProjectDeleted()
        {
            var result = _controller.Delete("projectName", _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Tests retrieving of a single project
        /// </summary>
        [Fact]
        public void Get_ReturnsValid_WhenProjectRetrieved()
        {
            _controller.Create("projectName", _userSession);
            var result = _controller.Get("projectName", _userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal("projectName", ((ProjectModel)((OkObjectResult)result).Value).Name);
        }

        /// <summary>
        /// Tests single get project, should return not found.
        /// </summary>
        [Fact]
        public void Get_ReturnsNotFound_WhenProjectRetrieved()
        {
            _controller.Create("projectName", _userSession);
            var result = _controller.Get("projectNameNoTvalid", _userSession);
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        /// <summary>
        /// Dispose of test data
        /// </summary>
        public void Dispose()
        {
            TestHelper.CleanUpTestContext(_runtime, _userName);
            _runtime.Shutdown();
        }
    }
}