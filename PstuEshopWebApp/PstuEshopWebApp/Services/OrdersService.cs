using Microsoft.EntityFrameworkCore;
using PstuEshopWebApp.Constants;
using PstuEshopWebApp.Data;
using PstuEshopWebApp.Sessions;

namespace PstuEshopWebApp.Services;

public class OrdersService(PstuEshopDbContext dbContext, ICartStore cartStore) : IOrdersService
{
    public async Task<CreateOrderResult> CreateOrderAsync(string? cityCode, CancellationToken ct = default)
    {
        if (cityCode is null || !CityOptions.Names.TryGetValue(cityCode, out var city))
            return new CreateOrderResult
            {
                Error = "Сначала выберите город в каталоге"
            };

        var cart = await cartStore.GetAsync(ct);

        if (cart.Count == 0)
            return new CreateOrderResult
            {
                Error = "Корзина пуста"
            };

        if (cart.Any(x => x.ProductId <= 0 || x.Quantity is < 1 or > CartOptions.MaxQuantity))
            return new CreateOrderResult
            {
                Error = "Некорректные данные корзины"
            };

        var productIds = cart
            .Select(x => x.ProductId)
            .Distinct()
            .ToArray();

        if (productIds.Length != cart.Count)
            return new CreateOrderResult
            {
                Error = "В корзине есть повторяющиеся позиции"
            };

        var products = await dbContext.Products
            .AsNoTracking()
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, ct);

        if (products.Count != productIds.Length)
            return new CreateOrderResult
            {
                Error = "Некоторые товары больше недоступны"
            };

        var order = new Order
        {
            City = city,
            CreatedAtUtc = DateTime.UtcNow,
            Items = cart.Select(item =>
            {
                var product = products[item.ProductId];

                return new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    SingleItemPrice = product.Price,
                    Quantity = item.Quantity
                };
            }).ToList()
        };

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(ct);

        await cartStore.SaveAsync([], ct);

        return new CreateOrderResult
        {
            OrderId = order.Id
        };
    }
}
