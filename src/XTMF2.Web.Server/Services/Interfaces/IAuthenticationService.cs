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

using System.Threading.Tasks;

namespace XTMF2.Web.Server.Services.Interfaces
{
    /// <summary>
    ///     Authentication service for clients. Associates a session with a backed XTMF2 user account.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> SignIn(string userName, string password = null);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task SignOut();
    }
}