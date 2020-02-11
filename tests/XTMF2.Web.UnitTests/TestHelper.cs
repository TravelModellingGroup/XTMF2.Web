namespace XTMF2.Web.UnitTests
{
    public class TestHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static XTMFRuntime CreateTestContext(string userName)
        {
            var runtime = XTMFRuntime.CreateRuntime();
            var userController = runtime.UserController;
            var projectController = runtime.ProjectController;
            string error = null;
            // clear out the user if possible
            // userController.Delete(userName);
            userController.CreateNew(userName, true, out var user, ref error);
            return runtime;
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
    }
}