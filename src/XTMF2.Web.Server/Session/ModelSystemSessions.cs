using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XTMF2.Web.Server.Session
{

    /// <summary>
    /// Holds the session state for an active user. Maintains references to projects and other
    /// related items.
    /// </summary>
    public class ModelSystemSessions
    {

        /// <summary>
        /// User dictionary of active model system sessions.
        /// </summary>
        /// <returns></returns>
        public Dictionary<User, List<Editing.ModelSystemSession>> Sessions { get; } = new Dictionary<User, List<Editing.ModelSystemSession>>();
        public ModelSystemSessions()
        {

        }

    }
}