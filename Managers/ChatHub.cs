using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace backend.Managers;

public class ChatHub : Hub
{
    private readonly DatabaseContext _context;

    public ChatHub(DatabaseContext context)
    {
        _context = context;
    }

    public async Task SendMessageToGroup(ChatEvent chat)
    {
        IAuthContainerModel model = JWTService.GetJWTContainerModel(chat.Username, "user");
        IAuthService authService = new JWTService(model.SecretKey);

        if (!authService.IsTokenValid(chat.JWT)) return;

        await Clients.OthersInGroup(chat.Roomname).SendAsync("ReceiveMessage", chat.Username, chat.Content).ConfigureAwait(true);

        // Add new message to database
        /* Message dbmessage = new Message(); */
        /* this._context.Messages.Add(dbmessage); */
        /* this._context.SaveChanges(); */
    }

    public async Task JoinGroup(ChatEvent chat)
    {
        IAuthContainerModel model = JWTService.GetJWTContainerModel(chat.Username, "user");
        IAuthService authService = new JWTService(model.SecretKey);

        await Groups.AddToGroupAsync(Context.ConnectionId, chat.Roomname);

        if (!authService.IsTokenValid(chat.JWT)) return;

        await Clients.OthersInGroup(chat.Roomname).SendAsync("ReceiveMessage", chat.Username, $"joined to {chat.Roomname}").ConfigureAwait(true);
    }

    public async Task LeaveGroup(ChatEvent chat)
    {
        IAuthContainerModel model = JWTService.GetJWTContainerModel(chat.Username, "user");
        IAuthService authService = new JWTService(model.SecretKey);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.Roomname);

        if (!authService.IsTokenValid(chat.JWT)) return;

        await Clients.OthersInGroup(chat.Roomname).SendAsync("ReceiveMessage", chat.Username, $"left {chat.Roomname}").ConfigureAwait(true);
    }
}
