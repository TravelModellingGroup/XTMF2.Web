

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;


namespace XTMF2.Web.Client.Contexts
{

    /// <summary>
    /// Session context. Tracks user session and context, and helps notify
    /// server on client disconnect.
    /// </summary>
    public partial class SessionContext : ComponentBase
    {

        private HubConnection _hubConnection;


        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("on init");
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/project-session-context"))
            .Build();
            await _hubConnection.StartAsync();

        }
    }
}