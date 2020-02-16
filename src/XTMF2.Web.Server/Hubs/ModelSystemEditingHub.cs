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
    [Authorize]
    public class ModelSystemEditingHub : Hub
    {
        private readonly ILogger<ModelSystemEditingHub> _logger;

        /// <summary>
        /// Maps a model system to a list of users, that are currently active in editing the model system
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<ModelSystem, HashSet<User>> _modelSystemUsers = new Dictionary<ModelSystem, HashSet<User>>();

        /// <summary>
        /// Maps a single user to a list of open model systems
        /// </summary>
        /// <typeparam name="User"></typeparam>
        /// <typeparam name="ModelSystem"></typeparam>
        /// <returns></returns>
        private readonly Dictionary<User, ModelSystem> _userModelSystem = new Dictionary<User, ModelSystem>();

        /// <summary>
        /// Maps a user to a list of associated connection ids
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<User, HashSet<string>> _userConnectionIds = new Dictionary<User, HashSet<string>>();

        private readonly Dictionary<ModelSystem, ModelSystemEditingTracker> _tracking = new Dictionary<ModelSystem, ModelSystemEditingTracker>();



        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ModelSystemEditingHub(ILogger<ModelSystemEditingHub> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sends an update to all clients connected to the model system editing hub
        /// </summary>
        /// <param name="changeName"></param>
        /// <param name="data"></param>
        public void SendModelSystemChangesAsync(string changeName, object data)
        {
            Clients.Others.SendAsync(changeName, data);
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            // find model system from request 
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation("Client disconnected");
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tracker"></param>
        public void RegisterEditingTracker(ModelSystem model, ModelSystemEditingTracker tracker)
        {
            _tracking[model] = tracker;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UnRegisterEditingTracker(ModelSystem model)
        {
            _tracking.Remove(model);
        }
    }
}