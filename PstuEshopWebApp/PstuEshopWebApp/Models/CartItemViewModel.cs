namespace PstuEshopWebApp.Models;

public class CartItemViewModel
{
    public required ProductViewModel Product { get; set; }
    public required int Quantity { get; set; }
}
