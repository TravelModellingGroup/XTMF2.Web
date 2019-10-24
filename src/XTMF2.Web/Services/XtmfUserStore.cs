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
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Services
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class XtmfUserStore<TUser> : IUserStore<TUser> where TUser : XtmfUser
    {
        private readonly IMapper _mapper;
        private readonly XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// </summary>
        /// <param name="xtmfRuntme"></param>
        /// <param name="mapper"></param>
        public XtmfUserStore(XTMFRuntime xtmfRuntme, IMapper mapper)
        {
            _xtmfRuntime = xtmfRuntme;
            _mapper = mapper;
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

        /// <summary>
        ///     Deletes the specified XTMF2 user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            _xtmfRuntime.UserController.Delete(user.User.UserName);
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<XtmfUser>(_xtmfRuntime.UserController.GetUserByName(userId));
            return !(user is null) ? Task.FromResult((TUser) user) : Task.FromResult<TUser>(null);
        }

        /// <summary>
        /// </summary>
        /// <param name="normalizedUserName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _xtmfRuntime.UserController.GetUserByName(normalizedUserName.ToLower());
            return !(user is null) ? Task.FromResult((TUser) new XtmfUser(user)) : Task.FromResult<TUser>(null);
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.User.UserName.ToLower());
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.User.UserName);
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.User.UserName);
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}