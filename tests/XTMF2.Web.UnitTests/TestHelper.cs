using System;
using System.Collections.Generic;
using System.Text;

namespace XTMF2.Web.UnitTests
{
    public class TestHelper
    {
        public static XTMFRuntime CreateTestContext()
        {
            var runtime = XTMFRuntime.CreateRuntime();
            var userController = runtime.UserController;
            var projectController = runtime.ProjectController;
            string error = null;
            string userName = "TempUser";
            // clear out the user if possible
            userController.Delete(userName);
            userController.CreateNew(userName, true, out var user, ref error);
            return runtime;
        }
    }
}
