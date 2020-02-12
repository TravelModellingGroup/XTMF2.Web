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
    /// Unit tests related to the ModelSystemEditorController
    /// </summary>
    public class ModelSystemEditorControllerUnitTests : IDisposable
    {

        private IMapper _mapper;
        private XTMFRuntime _runtime;
        private ILogger<ModelSystemController> _logger;
        private ModelSystemController _controller;
        private UserSession _userSession;
        private ProjectSessions _projectSessions;

        private string _userName;

        public ModelSystemEditorControllerUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelSystemProfile>();
            });
            _mapper = config.CreateMapper();
            _userName = Guid.NewGuid().ToString();
            TestHelper.CreateTestUser(_userName);
            _runtime = TestHelper.Runtime;
            _logger = Mock.Of<ILogger<ModelSystemController>>();
            _projectSessions = new ProjectSessions();
            _controller = new ModelSystemController(_runtime, _logger, _mapper, _projectSessions);
            _userSession = new UserSession(_runtime.UserController.GetUserByName(_userName));
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