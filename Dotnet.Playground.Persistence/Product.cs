public class Product
{
    public required int Id { get; set; }
    public required string Title { get; set; } = default!;
    public required string Company { get; set; } = default!;
    public required DateTime ReleaseDate { get; set; }
}