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

using System.Collections.Generic;
using XTMF2.Editing;

namespace XTMF2.Web.Server.Session
{
    /// <summary>
    ///     Holds the session state for an active user. Maintains references to projects and other
    ///     related items.
    /// </summary>
    public class ProjectSessions
    {
        /// <summary>
        ///     Dictionary access of Sessions open for a specific user.
        /// </summary>
        /// <typeparam name="User"></typeparam>
        /// <typeparam name="Editing.ProjectSession"></typeparam>
        /// <returns></returns>
        public Dictionary<User, List<ProjectSession>> Sessions { get; } = new Dictionary<User, List<ProjectSession>>();

        /// <summary>
        ///     Clears all project sessions for the associated user
        /// </summary>
        /// <param name="user"></param>
        public void ClearSessionsForUser(User user)
        {
            if (Sessions.ContainsKey(user))
            {
                //dispose each session
                foreach (var session in Sessions[user]) session.Dispose();
                Sessions[user].Clear();
            }
        }

        /// <summary>
        ///     Adds / tracks a session for the associated user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="session"></param>
        public void TrackSessionForUser(User user, ProjectSession session)
        {
            if (!Sessions.ContainsKey(user)) Sessions[user] = new List<ProjectSession>();
            Sessions[user].Add(session);
        }
    }
}