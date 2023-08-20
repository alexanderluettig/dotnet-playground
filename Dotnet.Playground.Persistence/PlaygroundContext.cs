using Microsoft.EntityFrameworkCore;

namespace Dotnet.Playground.Persistence;
public class PlaygroundContext : DbContext
{
    public PlaygroundContext(DbContextOptions<PlaygroundContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = default!;
}
