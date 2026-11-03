using System.Text.Json;

namespace PstuEshopWebApp.Sessions;

public class SessionCartStore(IHttpContextAccessor contextAccessor) : ICartStore
{
    private const string CartKey = "shopping-cart";

    private ISession Session =>
        contextAccessor.HttpContext?.Session
        ?? throw new InvalidOperationException(
            "Сессия недоступна");

    public async Task<List<CartItem>> GetAsync(CancellationToken ct = default)
    {
        var session = Session;

        await session.LoadAsync(ct);

        var json = session.GetString(CartKey);

        return string.IsNullOrEmpty(json)
            ? []
            : JsonSerializer.Deserialize<List<CartItem>>(json) ?? [];
    }

    public async Task SaveAsync(ICollection<CartItem> items, CancellationToken ct = default)
    {
        var session = Session;

        await session.LoadAsync(ct);

        session.SetString(
            CartKey,
            JsonSerializer.Serialize(items));
    }
}
