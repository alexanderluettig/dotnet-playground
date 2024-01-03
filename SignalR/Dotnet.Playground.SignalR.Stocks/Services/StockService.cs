
using Stocks.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Stocks.Services;

public class StockService : BackgroundService
{
    private readonly IHubContext<StockHub> _hubContext;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));
    private readonly Random _random = new();
    private double _currentStockPrice = 100;

    public StockService(IHubContext<StockHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            var change = _random.NextDouble() * _random.Next(-10, 10);
            _currentStockPrice = Math.Max(_currentStockPrice + change, 0);
            await _hubContext.Clients.All.SendAsync("UpdateStockPrice", _currentStockPrice, stoppingToken);
        }
    }
}
