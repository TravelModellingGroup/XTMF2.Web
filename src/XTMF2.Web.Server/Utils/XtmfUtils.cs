using System.Linq;
using XTMF2.Editing;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Utils {

    public class XtmfUtils {

        /// <summary>
        /// Retrieves an active reference to a project session or creates a new one if one does not exist.
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="userSession"></param>
        /// <param name="projectName"></param>
        /// <param name="projectSession"></param>
        /// <param name="projectSessions"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool GetProjectSession(XTMFRuntime runtime, UserSession userSession, string projectName,
            out ProjectSession projectSession, ProjectSessions projectSessions,
            ref string error) {
            var project = GetProject(projectName, userSession);
            if (!runtime.ProjectController.GetProjectSession(userSession.User, project, out projectSession, ref error)) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Utility for compacting call access to retrieving a model system header.
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="user"></param>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="modelSystem"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool GetModelSystemHeader(XTMFRuntime runtime, UserSession userSession, ProjectSessions projectSessions,
            string projectName, string modelSystemName, out ModelSystemHeader modelSystem, ref string error) {
            if (!GetProjectSession(runtime, userSession, projectName, out var projectSession, projectSessions, ref error)) {
                modelSystem = null;
                return false;
            }
            if (!projectSession.GetModelSystemHeader(userSession.User, modelSystemName, out modelSystem, ref error)) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieves a reference to the project stored in the user session.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static Project GetProject(string projectName, UserSession session) {
            var project = session.Projects.FirstOrDefault(p => p.Name == projectName);
            return project;
        }
    }
}