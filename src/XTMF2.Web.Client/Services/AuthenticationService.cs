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

using System;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Client.Services.Api;

namespace XTMF2.Web.Client.Services
{

    public class AuthenticationService
    {
        private AuthenticationClient _client;
        private ISessionStorageService _storage;
        private ILogger<AuthenticationService> _logger;
        private AuthenticationStateProvider _authProvider;
        public event EventHandler OnAuthenticated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="storage"></param>
        /// <param name="logger"></param>
        /// <param name="authProvider"></param>
        public AuthenticationService(AuthenticationClient client, ISessionStorageService storage, ILogger<AuthenticationService> logger,
        AuthenticationStateProvider authProvider)
        {
            _client = client;
            _storage = storage;
            _logger = logger;
            _authProvider = authProvider;
        }

        /// <summary>
        /// Tests the login state of the current stored user.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestLoginAsync()
        {
            return await _client.TestLoginAsync(await _storage.GetItemAsync<string>("uerName"), await _storage.GetItemAsync<string>("token"));
        }

        /// <summary>
        /// Performs a login action. If successful, the access token is stored in browser session data.
        /// </summary>
        /// <param name="userName">The username to login with.</param>
        /// <returns>True/false depending on login result.</returns>
        public async Task<bool> LoginAsync(string userName)
        {
            string result = default;
            try
            {
                result = await _client.LoginAsync("local");
                await _storage.SetItemAsync("token", result);
                await _storage.SetItemAsync("userName", userName);

            }
            catch (ApiException exception)
            {
                _logger.LogError("Invalid login.", exception);
                return false;
            }
            _logger.LogInformation("Logged in.");
            OnAuthenticated?.Invoke(this, new EventArgs());
            return true;
        }

        /// <summary>
        /// Performs a logout action.
        /// </summary>
        /// <returns></returns>
        public async void LogoutAsync()
        {
            await _client.LogoutAsync();
        }

    }
}