using PstuEshopWebApp.Models;

namespace PstuEshopWebApp.Services;

public interface IProductsService
{
    Task<ICollection<ProductViewModel>> GetAllProductsAsync(CancellationToken ct = default);
    Task<ICollection<string>> GetCategoriesAsync(CancellationToken ct = default);
    Task<ICollection<ProductViewModel>> GetProductsAsync(string? category, string? sort, CancellationToken ct = default);
}