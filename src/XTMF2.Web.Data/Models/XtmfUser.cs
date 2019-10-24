//    Copyright 2017-2019 University of Toronto
// 
//    This file is part of XTMF2.
// 
//    XTMF2 is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    XTMF2 is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

namespace XTMF2.Web.Data.Models
{
    /// <summary>
    ///     Facade object for XTMF2.Users.
    /// </summary>
    public class XtmfUser
    {
        /// <summary>
        ///     The backing XTMF2.User
        /// </summary>
        private readonly User _user;

        /// <summary>
        ///     Default no argument constructor.
        /// </summary>
        public XtmfUser(User user)
        {
            _user = user;
        }

        /// <summary>
        /// </summary>
        public User User => _user;

        /// <summary>
        /// </summary>
        public bool IsAdmin => _user.IsAdmin;

        /// <summary>
        /// </summary>
        public string UserName => _user.UserName;
    }
}