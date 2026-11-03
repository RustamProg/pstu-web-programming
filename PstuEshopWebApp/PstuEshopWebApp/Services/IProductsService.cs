using PstuEshopWebApp.Models;

namespace PstuEshopWebApp.Services;

public interface IProductsService
{
    Task<ICollection<ProductViewModel>> GetAllProductsAsync(CancellationToken ct);
    Task<ICollection<string>> GetCategoriesAsync(CancellationToken ct);
    Task<ICollection<ProductViewModel>> GetProductsAsync(string? category, string? sort, CancellationToken ct);
}