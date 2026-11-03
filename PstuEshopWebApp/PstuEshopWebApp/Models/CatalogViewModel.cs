namespace PstuEshopWebApp.Models;

public class CatalogViewModel
{
    public ICollection<ProductViewModel> Products { get; set; } = [];
    public ICollection<string> Categories { get; set; } = [];
    public string? Category { get; set; }
    public string Sort { get; set; } = "name_asc";
}
