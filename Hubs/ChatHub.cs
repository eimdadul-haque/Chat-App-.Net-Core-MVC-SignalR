using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat_App_.Net_Core_MVC_SignalR.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessageToAll(string message, string name)
        {
            await Clients.All.SendAsync("msg", message, name);
        }

        public async Task SendMessageToOneUser(string message, string name, string id)
        {
            await Clients.Client(id).SendAsync("msg_one_user", message, name, Context.ConnectionId);
        }

        public async Task BackToSender(string message, string name)
        {
            await Clients.Caller.SendAsync("msg_back_to_user", message, name);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", Context.ConnectionId, Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("DisConnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
