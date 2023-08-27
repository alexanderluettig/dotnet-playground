using Bogus;
using Dotnet.Playground.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Playground.WebApi.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly PlaygroundContext _context;
    private readonly Faker<Product> _faker;

    public ProductController(PlaygroundContext context)
    {
        _context = context;
        _faker = new Faker<Product>()
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Company, f => f.Company.CompanyName())
            .RuleFor(p => p.ReleaseDate, f => f.Date.Past());
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovies()
    {
        var movies = _faker.Generate(10);
        await _context.Products.AddRangeAsync(movies);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    public IEnumerable<Product> GetMovies()
    {
        var movies = _context.Products.ToList();
        return movies;
    }
}
