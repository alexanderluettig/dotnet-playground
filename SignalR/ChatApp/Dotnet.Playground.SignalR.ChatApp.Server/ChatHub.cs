using Microsoft.AspNetCore.SignalR;

namespace Dotnet.Playground.SignalR.ChatApp.Server;

public class ChatHub : Hub
{
    private static readonly Dictionary<string, User> _users = [];

    public async Task SendMessage(string user, string message)
    {
        var currentRoom = _users[Context.ConnectionId].Room;
        await Clients.Group(currentRoom).SendAsync("ReceiveMessage", user, message);
    }

    public async Task Connect(string username, string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        _users.Add(Context.ConnectionId, new User(username, room));
        await Clients.Group(room).SendAsync("ReceiveMessage", username, "Connected");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var (username, room) = _users[Context.ConnectionId];
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        _users.Remove(Context.ConnectionId);
        await Clients.Group(room).SendAsync("ReceiveMessage", username, "Disconnected");

        await base.OnDisconnectedAsync(exception);
    }

    private record User(string Username, string Room);
}
