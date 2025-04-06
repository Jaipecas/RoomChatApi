using Microsoft.AspNetCore.SignalR;

namespace RoomChatApi.Hubs
{
    public class RoomChatHub : Hub
    {
        public async Task JoinGroup(int roomId, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", $"{userName} entró a la sala");
        }

        public async Task LeaveGroup(int roomId, string userName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", $"{userName} salió de la sala");
        }
        public async Task SendMessage(int roomId, string userName, string message)
        {
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", userName, message);
        }
    }
}
