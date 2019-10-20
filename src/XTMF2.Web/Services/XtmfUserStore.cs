using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using XTMF2.Web.Data;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Services
{
    public class XtmfUserStore<TUser> : IUserStore<TUser> where TUser : XtmfUser
    {
        private XTMFRuntime _xtmfRuntime;
        private IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xtmfRuntme"></param>
        /// <param name="mapper"></param>
        public XtmfUserStore(XTMF2.XTMFRuntime xtmfRuntme, IMapper mapper)
        {
            this._xtmfRuntime = xtmfRuntme;
            this._mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the specified XTMF2 user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            this._xtmfRuntime.UserController.Delete(user.UserName);
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<XtmfUser>(_xtmfRuntime.UserController.GetUserByName(userId));
            return !(user is null) ? Task.FromResult((TUser)user) : Task.FromResult<TUser>(null);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="normalizedUserName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _xtmfRuntime.UserController.GetUserByName(normalizedUserName.ToLower());
            return !(user is null) ? Task.FromResult((TUser)new XtmfUser(user.UserName, user.Admin)) : Task.FromResult<TUser>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToLower());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {

            throw new NotSupportedException();
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}
