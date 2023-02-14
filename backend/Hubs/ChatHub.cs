using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace backend.Hubs;

public class ChatHub : Hub
{
    public void Send(string user, string message, string roomName)
    {
        Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
    }
    
    public void JoinRoom(string roomName, string user)
    {
        Clients.Group(roomName).SendAsync("ReceiveMessage", user, " joined to " + roomName).ConfigureAwait(true);
        Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }
    
    public void LeaveRoom(string roomName, string user)
    {
        Clients.Group(roomName).SendAsync("ReceiveMessage", user, " left " + roomName).ConfigureAwait(true);
        Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }
}