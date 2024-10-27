using Microsoft.AspNetCore.SignalR;
using Test.Application.Abstraction;
using Test.WebAPI.Hubs;

namespace Test.WebAPI.Notifiers
{
    public class HubNotifier : IHubNotifier
    {
        private readonly IHubContext<SocketHub> _hubContext;

        public HubNotifier(IHubContext<SocketHub> hubContext) { _hubContext = hubContext; }

        public async Task NotifyUser(string message, string connectionId) =>  await _hubContext.Clients.Client(connectionId).SendAsync("UpdateUserInfo", message);
        
    }
   
}
