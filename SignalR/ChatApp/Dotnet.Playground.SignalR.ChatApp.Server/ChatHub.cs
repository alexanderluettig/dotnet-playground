﻿using Microsoft.AspNetCore.SignalR;

namespace Dotnet.Playground.SignalR.ChatApp.Server;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}