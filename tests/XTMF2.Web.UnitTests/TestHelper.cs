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

using XTMF2.Controllers;
using XTMF2.Editing;
using XTMF2.UnitTests.Modules;

namespace XTMF2.Web.UnitTests
{
    internal static class TestHelper
    {
        public static XTMFRuntime Runtime;
        public static UserController UserController;
        public static ProjectController ProjectController;

        /// <summary>
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
            projectSession.EditModelSystem(user, modelSystem, out modelSystemSession, ref error);

            modelSystemSession.AddModelSystemStart(user, modelSystemSession.ModelSystem.GlobalBoundary, "TestStart",
                out var start, ref error);
            modelSystemSession.AddNode(user, modelSystemSession.ModelSystem.GlobalBoundary, "TestNode1",
                typeof(SimpleTestModule), out var node, ref error);
            modelSystemSession.AddLink(user, start, start.Hooks[0], node, out var link, ref error);
            modelSystemSession.AddBoundary(user, modelSystemSession.ModelSystem.GlobalBoundary, "TestBoundary1",
                out var boundary, ref error);
        }

        static TestHelper()
        {
            Runtime = XTMFRuntime.CreateRuntime();
        }
    }
}