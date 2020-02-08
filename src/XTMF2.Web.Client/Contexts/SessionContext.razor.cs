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
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using XTMF2.Web.Client.Services;

namespace XTMF2.Web.Client.Contexts
{

    /// <summary>
    /// Session context. Tracks user session and context, and helps notify
    /// server on client disconnect.
    /// </summary>
    public partial class SessionContext : ComponentBase
    {

        private HubConnection _hubConnection;

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public ISessionStorageService StorageService { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override void OnInitialized()
        {
            AuthenticationService.OnAuthenticated += OnAuthenticated;

        }

        private async void OnAuthenticated(object sender, EventArgs eventArgs)
        {
            _hubConnection = new HubConnectionBuilder()
                        .WithUrl(NavigationManager.ToAbsoluteUri("/project-session-context"), options =>
                        {
                            options.AccessTokenProvider = () => StorageService.GetItemAsync<string>("token");
                        })
                        .Build();
            await _hubConnection.StartAsync();
        }

    }
}