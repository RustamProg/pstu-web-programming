namespace PstuEshopWebApp.Services;

public interface ICartService
{
    Task<int?> AddAsync(int productId, CancellationToken ct = default);
    Task<int> GetCountAsync(CancellationToken ct = default);
}