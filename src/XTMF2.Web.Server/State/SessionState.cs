using System.Collections.Generic;

namespace XTMF2.Web.Server.State
{

    /// <summary>
    /// Holds the session state for an active user. Maintains references to projects and other
    /// related items.
    /// </summary>
    public class SessionState
    {
        /// <summary>
        /// The XTMF User object associated with this session
        /// </summary>
        /// <value></value>
        public User User { get; private set; }

        /// <summary>
        /// Reference store for projects created / referenced during this session.
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="Project"></typeparam>
        /// <returns></returns>
        public Dictionary<string, Project> Projects { get; } = new Dictionary<string, Project>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public SessionState(User user)
        {
            this.User = user;
        }
    }
}