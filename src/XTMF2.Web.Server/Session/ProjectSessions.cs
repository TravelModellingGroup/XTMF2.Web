using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XTMF2.Web.Server.Session
{

    /// <summary>
    /// Holds the session state for an active user. Maintains references to projects and other
    /// related items.
    /// </summary>
    public class ProjectSessions
    {

        /// <summary>
        /// Dictionary access of Sessions open for a specific user.
        /// </summary>
        /// <typeparam name="User"></typeparam>
        /// <typeparam name="Editing.ProjectSession"></typeparam>
        /// <returns></returns>
        public Dictionary<User, List<Editing.ProjectSession>> Sessions { get; } = new Dictionary<User, List<Editing.ProjectSession>>();
        public ProjectSessions()
        {

        }

    }
}