using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XTMF2.Web.Server.Session {

    /// <summary>
    /// Holds the session state for an active user. Maintains references to projects and other
    /// related items.
    /// </summary>
    public class ProjectSessions {

        public Dictionary<string, Editing.ProjectSession> Sessions { get; } = new Dictionary<string, Editing.ProjectSession>();
        public ProjectSessions() {

        }

    }
}