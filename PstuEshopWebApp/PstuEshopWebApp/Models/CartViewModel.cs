namespace PstuEshopWebApp.Models;

public class CartViewModel
{
    public ICollection<CartItemViewModel> Items { get; set; } = [];

    public decimal TotalPrice => Items.Sum(i => i.Product.Price * i.Quantity);
}
