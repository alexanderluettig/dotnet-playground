using Bogus;
using Dotnet.Playground.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PlaygroundContext>(options => options.UseInMemoryDatabase("items"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movie API",
        Description = "Search for the best Movies",
        Version = "v1"
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
});


app.MapPost("/products", async (PlaygroundContext context) =>
{
    var productFaker = new Faker<Product>()
        .RuleFor(m => m.Title, f => f.Commerce.ProductName())
        .RuleFor(m => m.Company, f => f.Company.CompanyName())
        .RuleFor(m => m.ReleaseDate, f => f.Date.Past());

    context.Products.AddRange(productFaker.Generate(10));
    await context.SaveChangesAsync();
});


app.MapGet("/products", async (PlaygroundContext context) =>
{
    return await context.Products.ToListAsync();
});

app.Run();
