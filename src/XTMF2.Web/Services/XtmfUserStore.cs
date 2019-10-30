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

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace XTMF2.Web.Services
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class XtmfUserStore<TUser> : IUserStore<User>
    {
        private readonly IMapper _mapper;
        private readonly XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// </summary>
        /// <param name="xtmfRuntime"></param>
        /// <param name="mapper"></param>
        public XtmfUserStore(XTMFRuntime xtmfRuntime, IMapper mapper)
        {
            _xtmfRuntime = xtmfRuntime;
            _mapper = mapper;
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            _xtmfRuntime.UserController.Delete(user);
            return Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<User> IUserStore<User>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _xtmfRuntime.UserController.GetUserByName(userId);
            return !(user is null) ? Task.FromResult(user) : Task.FromResult<User>(null);
        }

        /// <summary>
        /// </summary>
        /// <param name="normalizedUserName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<User> IUserStore<User>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _xtmfRuntime.UserController.GetUserByName(normalizedUserName);
            return !(user is null) ? Task.FromResult(user) : Task.FromResult<User>(null);
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToLower());
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}