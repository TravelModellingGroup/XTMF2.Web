using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XTMF2.Web.Server.Session {

    /// <summary>
    /// Holds the session state for an active user. Maintains references to projects and other
    /// related items.
    /// </summary>
    public class UserSession {
        /// <summary>
        /// The XTMF User object associated with this session
        /// </summary>
        /// <value></value>
        public User User { get; }

        /// <summary>
        /// Reference store for projects created / referenced during this session.
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="Project"></typeparam>
        /// <returns></returns>
        public ReadOnlyObservableCollection<Project> Projects { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public UserSession(User user) {
            this.User = user;
            this.Projects = XTMF2.Controllers.ProjectController.GetProjects(user);
        }
    }
}