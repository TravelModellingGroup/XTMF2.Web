
using Xunit;
using XTMF2.Web.Server.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using XTMF2.Web.Server.Profiles;
using System.Collections.Generic;
using XTMF2.Web.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace XTMF2.Web.UnitTests.Controllers
{
    public class ProjectControllerUnitTests
    {

        /// <summary>
        /// Tests if the projects list returns a valid list for the default (local) user.
        /// </summary>
        [Fact]
        public void IndexGet_ReturnsValidList()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProjectProfile>();
            });
            var mapper = config.CreateMapper();
            var runtime = TestHelper.CreateTestContext();
            var logger = Mock.Of<ILogger<ProjectController>>();
            var controller = new ProjectController(runtime, logger, mapper);
            var user = runtime.UserController.GetUserByName("TempUser");
            var projects = controller.List(user);

            //assert
            Assert.IsAssignableFrom<ActionResult<IEnumerable<ProjectModel>>>(projects);
        }

        /// <summary>
        /// Tests creating a user, and that the controller returns the correct response type.
        /// </summary>
        [Fact]
        public void CreatePost_ReturnsValid_WhenProjectCreated()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProjectProfile>();
            });
            var mapper = config.CreateMapper();
            var runtime = TestHelper.CreateTestContext();
            var logger = Mock.Of<ILogger<ProjectController>>();
            var controller = new ProjectController(runtime, logger, mapper);
            var user = runtime.UserController.GetUserByName("TempUser");
            var result = controller.Create("projectName",user);
            var projects = controller.List(user);

            //assert
            Assert.IsAssignableFrom<CreatedResult>(result);
            Assert.IsAssignableFrom<OkObjectResult>(projects.Result);
            Assert.Single(((List<ProjectModel>)((OkObjectResult)projects.Result).Value));

        }

    }
}