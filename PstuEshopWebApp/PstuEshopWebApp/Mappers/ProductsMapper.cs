using PstuEshopWebApp.Data;
using PstuEshopWebApp.Models;

namespace PstuEshopWebApp.Mappers;

public static class ProductsMapper
{
    public static ProductViewModel MapToViewModel(this Product product)
    {
        return new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            Description = product.Description,
            Price = product.Price
        };
    }
}
