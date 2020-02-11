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

using System.Collections.ObjectModel;
using XTMF2.Controllers;

namespace XTMF2.Web.Server.Session
{
    /// <summary>
    ///     Holds the session state for an active user. Maintains references to projects and other
    ///     related items.
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        public UserSession(User user)
        {
            User = user;
            Projects = ProjectController.GetProjects(user);
        }

        /// <summary>
        ///     The XTMF User object associated with this session
        /// </summary>
        /// <value></value>
        public User User { get; }

        /// <summary>
        ///     Reference store for projects created / referenced during this session.
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="Project"></typeparam>
        /// <returns></returns>
        public ReadOnlyObservableCollection<Project> Projects { get; }
    }
}