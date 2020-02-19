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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Hubs
{
    /// <summary>
    ///     Tracks user connects and disconnects via a SignalR hub.
    /// </summary>
    [Authorize]
    public class SessionContextHub : Hub
    {
        private readonly ILogger<SessionContextHub> _logger;
        private readonly ModelSystemSessions _modelSystemSessions;
        private readonly ProjectSessions _projectSessions;
        public readonly Dictionary<User, int> UserSessionCounts = new Dictionary<User, int>();

        /// <summary>
        ///     Called when a client disconnects or timeout is reached.
        ///     This should be used to free sessions opened by the user.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation("Client disconnected");
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        ///     Tracks a user connected
        /// </summary>
        /// <param name="userSession"></param>
        public void TrackUserConnected(UserSession userSession)
        {
            if (!UserSessionCounts.ContainsKey(userSession.User))
            {
                UserSessionCounts[userSession.User] = 0;
            }

            UserSessionCounts[userSession.User]++;
        }

        /// <summary>
        ///     Tracks a disconnected user.
        /// </summary>
        /// <param name="userSession"></param>
        public void TrackUserDisconnected(UserSession userSession)
        {
            UserSessionCounts[userSession.User]--;
            if (UserSessionCounts[userSession.User] == 0)
            {
                // no session counts left, clear project sessions
                _projectSessions.ClearSessionsForUser(userSession.User);
                _modelSystemSessions.ClearSessionsForUser(userSession.User);
            }
        }

        /// <summary>
        ///     Client connected callback
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            _logger.LogInformation("Client connected");
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="projectSessions"></param>
        /// <param name="modelSystemSessions"></param>
        public SessionContextHub(ILogger<SessionContextHub> logger, ProjectSessions projectSessions,
            ModelSystemSessions modelSystemSessions)
        {
            _logger = logger;
            _projectSessions = projectSessions;
            _modelSystemSessions = modelSystemSessions;
        }
    }
}