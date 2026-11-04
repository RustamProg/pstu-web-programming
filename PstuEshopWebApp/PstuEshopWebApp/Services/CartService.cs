using Microsoft.EntityFrameworkCore;
using PstuEshopWebApp.Constants;
using PstuEshopWebApp.Data;
using PstuEshopWebApp.Mappers;
using PstuEshopWebApp.Models;
using PstuEshopWebApp.Sessions;

namespace PstuEshopWebApp.Services;

public class CartService(ICartStore cartStore, PstuEshopDbContext dbContext) : ICartService
{
    public async Task<int> GetCountAsync(CancellationToken ct = default)
    {
        var cart = await cartStore.GetAsync(ct);

        return cart.Sum(x => x.Quantity);
    }

    public async Task<AddCartResult> AddAsync(int productId, CancellationToken ct = default)
    {
        var exists = await dbContext.Products
            .AnyAsync(p => p.Id == productId, ct);

        if (!exists)
            return new AddCartResult
            {
                Quantity = 0,
                Status = AddCartStatus.NotFound,
                ErrorMessage = "Товар не найден"
            };

        var cart = await cartStore.GetAsync(ct);

        var index = cart.FindIndex(
            x => x.ProductId == productId);

        if (index >= 0)
        {
            var item = cart[index];

            if (item.Quantity < 1 || item.Quantity >= CartOptions.MaxQuantity)
            {
                return new AddCartResult
                {
                    Quantity = item.Quantity,
                    Status = AddCartStatus.LimitExceeded,
                    ErrorMessage = $"Нельзя добавить больше {CartOptions.MaxQuantity} единиц товара"
                };
            }

            cart[index] = new CartItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity + 1 // Вообще в реальном проекте это будет очень необезопасно
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

        return new AddCartResult
        {
            Quantity = cart.Sum(x => x.Quantity),
            Status = AddCartStatus.Success
        };
    }

    public async Task<CartViewModel> GetCartViewModelAsync(CancellationToken ct = default)
    {
        var cart = await cartStore.GetAsync(ct);

        var productIds = cart
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var products = await dbContext.Products
            .AsNoTracking()
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(ct);

        var items = cart
            .Join(products,
                c => c.ProductId,
                p => p.Id,
                (c, p) => new CartItemViewModel
                {
                    Product = p.MapToViewModel(),
                    Quantity = c.Quantity
                })
            .ToList();

        return new CartViewModel
        {
            Items = items
        };
    }

    public async Task<bool> SetExactQuantityAsync(long productId, int quantity, CancellationToken ct = default)
    {
        if (quantity is < 1 or > CartOptions.MaxQuantity)
            return false;

        var cart = await cartStore.GetAsync(ct);

        var index = cart.FindIndex(
            x => x.ProductId == productId);

        if (index < 0)
            return false;

        cart[index] = new CartItem
        {
            ProductId = productId,
            Quantity = quantity
        };

        await cartStore.SaveAsync(cart, ct);

        return true;
    }

    public async Task RemoveAsync(long productId, CancellationToken ct = default)
    {
        var cart = await cartStore.GetAsync(ct);

        cart.RemoveAll(x => x.ProductId == productId);

        await cartStore.SaveAsync(cart, ct);
    }
}
