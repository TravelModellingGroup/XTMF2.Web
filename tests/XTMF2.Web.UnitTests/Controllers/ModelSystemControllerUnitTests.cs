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

namespace XTMF2.Web.UnitTests.Controllers {
    public class ModelSystemControllerUnitTests {

        private IMapper _mapper;
        private XTMFRuntime _runtime;
        private ILogger<ModelSystemController> _logger;
        private ModelSystemController _controller;
        private UserSession _userSession;

        public ModelSystemControllerUnitTests() {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ProjectProfile>();
            });
            _mapper = config.CreateMapper();
            _runtime = TestHelper.CreateTestContext();
            _logger = Mock.Of<ILogger<ModelSystemController>>();
            _controller = new ModelSystemController(_runtime, _logger, _mapper);
            _userSession = new UserSession(_runtime.UserController.GetUserByName("TempUser"));
        }

    }
}