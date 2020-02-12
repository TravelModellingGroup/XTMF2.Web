using XTMF2.Controllers;
using XTMF2.Editing;
using XTMF2.UnitTests;
using XTMF2.UnitTests.Modules;

namespace XTMF2.Web.UnitTests
{
    static class TestHelper
    {

        public static XTMFRuntime Runtime;
        public static UserController UserController;
        public static ProjectController ProjectController;

        static TestHelper()
        {
            Runtime = XTMFRuntime.CreateRuntime();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static User CreateTestUser(string userName)
        {
            string error = default;
            UserController = Runtime.UserController;
            ProjectController = Runtime.ProjectController;
            UserController.CreateNew(userName, true, out var user, ref error);
            return user;
        }

        /// <summary>
        ///     Cleanup the test context
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="userName"></param>
        public static void CleanUpTestContext(XTMFRuntime runtime, string userName)
        {
            runtime.UserController.Delete(userName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="modelSystemSession"></param>
        public static void InitializeTestModelSystem(User user, string projectName, string modelSystemName, 
            out ModelSystemSession modelSystemSession)
        {
            string error = default;
            ProjectController.CreateNewProject(user, projectName, out var projectSession, ref error);
            projectSession.CreateNewModelSystem(user, modelSystemName, out var modelSystem, ref error);
            projectSession.EditModelSystem(user, modelSystem, out  modelSystemSession, ref error);

            modelSystemSession.AddModelSystemStart(user, modelSystemSession.ModelSystem.GlobalBoundary, "TestStart", out var start, ref error);
            modelSystemSession.AddNode(user, modelSystemSession.ModelSystem.GlobalBoundary, "TestNode1", typeof(SimpleTestModule), out var node, ref error);
            modelSystemSession.AddLink(user,start,start.Hooks[0], node, out var link, ref error);

            modelSystemSession.AddBoundary(user,modelSystemSession.ModelSystem.GlobalBoundary, "TestBoundary1",out var boundary, ref error);

        }
    }
}
