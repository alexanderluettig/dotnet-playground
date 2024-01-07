using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Dotnet.Playground.SignalR.Survey.Database;

public class SurveyContext(DbContextOptions<SurveyContext> options) : DbContext(options)
{
    public DbSet<SurveyData> Surveys { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SurveyData>()
            .Property(s => s.Answers)
            .HasConversion(
                a => JsonConvert.SerializeObject(a),
                a => JsonConvert.DeserializeObject<Dictionary<string, int>>(a) ?? new Dictionary<string, int>());

        var valueComparer = new ValueComparer<Dictionary<string, int>>(
            (d1, d2) => d1!.SequenceEqual(d2!),
            d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            d => d.ToDictionary(kv => kv.Key, kv => kv.Value));

        modelBuilder.Entity<SurveyData>()
            .Property(s => s.Answers)
            .Metadata
            .SetValueComparer(valueComparer);
    }
}
