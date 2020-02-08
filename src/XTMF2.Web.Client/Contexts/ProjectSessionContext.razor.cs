

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace XTMF2.Web.Client.Contexts
{

    public partial class ProjectSessionContext : ComponentBase
    {

        private HubConnection _hubConnection;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

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