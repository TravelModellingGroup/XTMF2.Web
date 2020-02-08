using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BlazorSignalRApp.Server.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectSessionContextHub : Hub
    {
        private ILogger<ProjectSessionContextHub> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ProjectSessionContextHub(ILogger<ProjectSessionContextHub> logger)
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

            _logger.LogInformation("Client connected");
            await base.OnConnectedAsync();

        }
    }
}