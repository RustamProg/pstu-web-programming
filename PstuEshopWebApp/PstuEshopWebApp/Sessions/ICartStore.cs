namespace PstuEshopWebApp.Sessions;

public interface ICartStore
{
    Task<List<CartItem>> GetAsync(CancellationToken ct = default);
    Task SaveAsync(ICollection<CartItem> items, CancellationToken ct = default);
}