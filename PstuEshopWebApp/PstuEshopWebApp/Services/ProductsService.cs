using Microsoft.EntityFrameworkCore;
using PstuEshopWebApp.Constants;
using PstuEshopWebApp.Data;
using PstuEshopWebApp.Mappers;
using PstuEshopWebApp.Models;

namespace PstuEshopWebApp.Services;

public class ProductsService(PstuEshopDbContext dbContext) : IProductsService
{
    public async Task<ICollection<ProductViewModel>> GetProductsAsync(string? category, string? sort, CancellationToken ct)
    {
        var query = dbContext.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category == category);

        query = sort switch // Топорно, но работает
        {
            ProductSortFields.PriceAsc => query
                .OrderBy(p => p.Price)
                .ThenBy(p => p.Id),

            ProductSortFields.PriceDesc => query
                .OrderByDescending(p => p.Price)
                .ThenBy(p => p.Id),

            ProductSortFields.NameAsc => query
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id),

            ProductSortFields.NameDesc => query
                .OrderByDescending(p => p.Name)
                .ThenBy(p => p.Id),

            _ => query
        };

        return await query
            .Select(p => p.MapToViewModel())
            .ToListAsync(ct);
    }

    public async Task<ICollection<string>> GetCategoriesAsync(CancellationToken ct)
    {
        return await dbContext.Products
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(ct);
    }

    public async Task<ICollection<ProductViewModel>> GetAllProductsAsync(CancellationToken ct)
    {
        return await dbContext.Products
            .Select(p => p.MapToViewModel())
            .ToListAsync(ct);
    }
}
