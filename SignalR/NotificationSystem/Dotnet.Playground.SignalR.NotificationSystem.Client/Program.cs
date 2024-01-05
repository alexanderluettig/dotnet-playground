using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5176/notifications")
    .Build();

connection.On<string, string>("ReceiveMessage", (title, message) =>
{
    Console.WriteLine($"Received message: \n\tTitle: {title}\n\tMessage: {message}");
});

await connection.StartAsync();

while (true)
{
    var message = Console.ReadLine();
}