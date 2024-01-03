using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5146/chathub")
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user}: {message}");
});

await connection.StartAsync();

Console.WriteLine("Please enter your name:");
var name = Console.ReadLine();
Console.WriteLine("Please enter the room you want to join:");
var room = Console.ReadLine();
await connection.SendAsync("Connect", name, room);

while (true)
{
    var message = Console.ReadLine();

    if (string.IsNullOrEmpty(message))
    {
        break;
    }

    await connection.SendAsync("SendMessage", name, message);
}