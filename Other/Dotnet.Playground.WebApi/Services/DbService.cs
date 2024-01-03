using Dotnet.Playground.Persistence;
using Bogus;

class DbService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Faker<Product> _faker;

    public DbService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _faker = new Faker<Product>()
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Company, f => f.Company.CompanyName())
            .RuleFor(p => p.ReleaseDate, f => f.Date.Past());
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PlaygroundContext>();
        context.Products.AddRange(_faker.Generate(100));
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}