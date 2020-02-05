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
            _runtime = TestHelper.CreateTestContext(_userName);
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