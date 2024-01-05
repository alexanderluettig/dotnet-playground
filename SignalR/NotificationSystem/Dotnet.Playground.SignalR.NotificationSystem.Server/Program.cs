using Dotnet.Playground.SignalR.NotificationSystem.Server;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();

app.MapPost("/notification", (IHubContext<NotificationHub> hub, Notification notification) =>
{
    hub.Clients.All.SendAsync("ReceiveMessage", notification.Title, notification.Message);
    return Results.Ok();
});

app.MapHub<NotificationHub>("/notifications");

app.Run();

record Notification(string Title, string Message);
