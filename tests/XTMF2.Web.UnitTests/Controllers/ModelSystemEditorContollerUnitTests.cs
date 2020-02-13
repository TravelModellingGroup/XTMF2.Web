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
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using XTMF2.UnitTests.Modules;
using XTMF2.Web.Data.Converters;
using XTMF2.Web.Data.Interfaces.Editing;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Controllers;
using XTMF2.Web.Server.Profiles;
using XTMF2.Web.Server.Session;
using Xunit;
using Xunit.Abstractions;

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
        private readonly ITestOutputHelper output;
        private string _userName;
        private User _user;

        public ModelSystemEditorControllerUnitTests(ITestOutputHelper output)
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
            _controller = new ModelSystemEditorController(_runtime, _logger, _projectSessions, _modelSystemSessions, _mapper);
            _userSession = new UserSession(_runtime.UserController.GetUserByName(_userName));
            this.output = output;
        }

        /// <summary>
        /// Tests that 200 OK is returned for valid model system
        /// </summary>
        [Fact]
        public void GetModelSystem_Returns200Ok_WhenQueryValidModelSystem()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            var result = _controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);

        }

        /// <summary>
        /// Tests that retrieving a model system, returns the correct model system (and constructed properly)
        /// </summary>
        [Fact]
        public void GetModelSystem_ReturnsCorrectModelSystem_WhenQueryModelsystem()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            var result = (OkObjectResult)_controller.GetModelSystem("TestProject", "TestModelSystem", _userSession);
            Assert.IsType<ModelSystemEditingModel>(result.Value);
            var modelSystem = (ModelSystemEditingModel)result.Value;

            // assert 
            Assert.Single(modelSystem.GlobalBoundary.Starts);
            Assert.Equal("TestStart", modelSystem.GlobalBoundary.Starts[0].Name);
            Assert.Collection<INode>(modelSystem.GlobalBoundary.Modules,
                item =>
                {
                    // Assert.Equal(typeof(SimpleTestModule),item.Type);
                    Assert.Equal("TestNode1", item.Name);
                });

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = true,
                MaxDepth = 64
            };
            options.Converters.Add(new TypeConverter());
            output.WriteLine(System.Text.Json.JsonSerializer.Serialize<ModelSystemEditingModel>((ModelSystemEditingModel)modelSystem, options
            ));
            Assert.NotNull(modelSystem.GlobalBoundary.Modules[0].Id);
        }

        /// <summary>
        /// Tests that a 404 is returned for non existing model system
        /// </summary>
        [Fact]
        public void GetModelSystem_Returns404_WhenQueryNonExistingModelsystem()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            var result = _controller.GetModelSystem("TestProject", "TestModelSystemFake", _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
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