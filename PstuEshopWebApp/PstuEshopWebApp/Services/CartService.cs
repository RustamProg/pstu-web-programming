using Microsoft.EntityFrameworkCore;
using PstuEshopWebApp.Data;
using PstuEshopWebApp.Sessions;

namespace PstuEshopWebApp.Services;

public class CartService(ICartStore cartStore, PstuEshopDbContext dbContext) : ICartService
{
    public async Task<int> GetCountAsync(CancellationToken ct = default)
    {
        var cart = await cartStore.GetAsync(ct);

        return cart.Sum(x => x.Quantity);
    }

    public async Task<int?> AddAsync(int productId, CancellationToken ct = default)
    {
        var exists = await dbContext.Products
            .AnyAsync(p => p.Id == productId, ct);

        if (!exists)
            return null;

        var cart = await cartStore.GetAsync(ct);

        var index = cart.FindIndex(
            x => x.ProductId == productId);

        if (index >= 0)
        {
            cart[index] = new CartItem
            {
                ProductId = productId,
                Quantity = cart[index].Quantity + 1
            };
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = productId,
                Quantity = 1
            });
        }

        await cartStore.SaveAsync(cart, ct);

        return cart.Sum(x => x.Quantity);
    }
}
