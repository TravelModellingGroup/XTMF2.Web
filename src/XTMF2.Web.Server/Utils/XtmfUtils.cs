using XTMF2.Editing;

namespace XTMF2.Web.Server.Utils
{

    public class XtmfUtils
    {

        /// <summary>
        /// Utility method to simplify/compact retrieval of a project session.
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="user"></param>
        /// <param name="projectName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool GetProjectSession(XTMFRuntime runtime, User user, string projectName, out ProjectSession projectSession, ref string error)
        {
            if (!runtime.ProjectController.GetProject(user.UserName, projectName, out var project, ref error)) {
                projectSession = null;
                return false;
            }
            if (!runtime.ProjectController.GetProjectSession(user, project, out projectSession, ref error)) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Utility for compacting call access to retrieving a model system header
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="user"></param>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="modelSystem"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool GetModelSystemHeader(XTMFRuntime runtime, User user, string projectName, string modelSystemName, out ModelSystemHeader modelSystem, ref string error)
        {
            if (!GetProjectSession(runtime, user, projectName, out var projectSession, ref error)) {
                modelSystem = null;
                return false;
            }
            if (!projectSession.GetModelSystemHeader(user, modelSystemName, out modelSystem, ref error)) {
                return false;
            }
            return true;
        }
    }
}