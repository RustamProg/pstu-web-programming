using PstuEshopWebApp.Models;

namespace PstuEshopWebApp.Services;

public interface ICartService
{
    Task<int> GetCountAsync(CancellationToken ct = default);
    Task<AddCartResult> AddAsync(int productId, CancellationToken ct = default);
    Task<CartViewModel> GetCartViewModelAsync(CancellationToken ct = default);
    Task<bool> SetExactQuantityAsync(long productId, int quantity, CancellationToken ct = default);
    Task RemoveAsync(long productId, CancellationToken ct = default);
}