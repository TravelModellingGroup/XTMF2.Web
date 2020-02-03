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
        /// Performs a login action.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(string userName)
        {
            try {
                var result = await _client.LoginAsync("local");
                await _storage.SetItemAsync("token", result);
                await _storage.SetItemAsync("userName", userName);

            }
            catch (ApiException exception) {
                _logger.LogError("Invalid login.", exception);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async void LogoutAsync()
        {
            await _client.LogoutAsync();
        }

    }
}