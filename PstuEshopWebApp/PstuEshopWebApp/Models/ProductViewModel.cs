namespace PstuEshopWebApp.Models;

public class ProductViewModel
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
