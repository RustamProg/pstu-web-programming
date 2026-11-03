namespace PstuEshopWebApp.Data;

public class Product
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
