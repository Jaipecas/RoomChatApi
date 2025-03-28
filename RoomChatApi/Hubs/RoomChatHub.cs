using Microsoft.AspNetCore.SignalR;

namespace RoomChatApi.Hubs
{
    public class RoomChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
