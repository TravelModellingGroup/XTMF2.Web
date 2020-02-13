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
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Controllers;
using XTMF2.Web.Server.Profiles;
using XTMF2.Web.Server.Session;
using Xunit;

namespace XTMF2.Web.UnitTests.Controllers
{
    /// <summary>
    /// Unit tests related to the ModelSystemEditorController
    /// </summary>
    public class ModelSystemEditorControllerUnitTests : IDisposable
    {

        private IMapper _mapper;
        private XTMFRuntime _runtime;
        private ILogger<ModelSystemEditorController> _logger;
        private ModelSystemEditorController _controller;
        private UserSession _userSession;
        private ProjectSessions _projectSessions;
        private ModelSystemSessions _modelSystemSessions;

        private string _userName;
        private User _user;

        public ModelSystemEditorControllerUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelSystemProfile>();
            });
            _mapper = config.CreateMapper();
            _userName = Guid.NewGuid().ToString();
            _user = TestHelper.CreateTestUser(_userName);
            _runtime = TestHelper.Runtime;
            _logger = Mock.Of<ILogger<ModelSystemEditorController>>();
            _projectSessions = new ProjectSessions();
            _modelSystemSessions = new ModelSystemSessions();
            _controller = new ModelSystemEditorController(_runtime, _logger, _projectSessions,_modelSystemSessions, _mapper);
            _userSession = new UserSession(_runtime.UserController.GetUserByName(_userName));
        }

        /// <summary>
        /// Tests that 200 OK is returned for valid model system
        /// </summary>
        [Fact]
        public void GetModelSystem_Returns200Ok_WhenQueryValidModelSystem() {
            TestHelper.InitializeTestModelSystem(_user,"TestProject","TestModelSystem", out var modelSystemSession);
            var result = _controller.GetModelSystem("TestProject","TestModelSystem",_userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);

        }

        /// <summary>
        /// Tests that retrieving a model system, returns the correct model system (and constructed properly)
        /// </summary>
        [Fact]
        public void GetModelSystem_ReturnsCorrectModelSystem_WhenQueryModelsystem() {
            TestHelper.InitializeTestModelSystem(_user,"TestProject","TestModelSystem", out var modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject","TestModelSystem",_userSession);
            Assert.IsType<ModelSystemEditingModel>(result.Value);

        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            TestHelper.CleanUpTestContext(_runtime, _userName);
            _runtime.Shutdown();
        }
    }
}