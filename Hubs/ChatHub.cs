using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
        => await Clients.All.SendAsync("ReceiveMessage", user, message);

    public async Task SendMessageToCaller(string user, string message)
        => await Clients.Caller.SendAsync("ReceiveMessage", user, message);

    public async Task SendMessageToGroup(string user, string message, string groupName)
        => await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);    

    public async void JoinGroup(string user, string roomName)
    {
        await Clients.Group(roomName).SendAsync("ReceiveMessage", user, "joined to " + roomName).ConfigureAwait(true);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }
    
    public async void LeaveGroup(string user, string roomName)
    {
        await Clients.Group(roomName).SendAsync("ReceiveMessage", user, " left " + roomName).ConfigureAwait(true);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }
}
