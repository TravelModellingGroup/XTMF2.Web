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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BlazorSignalRApp.Server.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class SessionContextHub : Hub
    {
        private ILogger<SessionContextHub> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public SessionContextHub(ILogger<SessionContextHub> logger)
        {
            _logger = logger;
        }

        public async Task SendMessage(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// Called when a client disconnects or timeout is reached.
        /// This should be used to free sessions opened by the user.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation("Client disconnected");
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Client connected callback
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {

           var user = Context.User;
            _logger.LogInformation("Client connected");
            await base.OnConnectedAsync();

        }
    }
}