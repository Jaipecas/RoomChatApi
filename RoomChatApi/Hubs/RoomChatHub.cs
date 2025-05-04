using Microsoft.AspNetCore.SignalR;
using RoomChatApi.Commands;
using RoomChatApi.Queries;

namespace RoomChatApi.Hubs
{
    public class RoomChatHub : Hub
    {

        public async Task JoinGroup(int roomId, string userId, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            await UserCommands.UpdateUserIsConnected(userId, roomId, true, Context.ConnectionId);

            await Clients.Group(roomId.ToString()).SendAsync("JoinedGroup", $"{userName} entró a la sala");
        }

        public async Task SendMessage(int roomId, string userName, string message)
        {
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", userName, message);
        }

        public async Task StartTimer(int roomId, DateTime startTime, int durationInMinutes, bool disable)
        {
            await Clients.Group(roomId.ToString()).SendAsync("TimerStarted", new
            {
                endTime = startTime.AddMinutes(durationInMinutes),
                disableChat = disable
            });
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            var (userId, userName, roomId) = await UserQueries.GetUserByConnectionId(Context.ConnectionId);

            if (userId != null)
            {
                await UserCommands.UpdateUserIsConnected(userId, (int)roomId!, false, null);

                await Clients.Group(roomId.ToString()!).SendAsync("UserDisconnected", userName);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
