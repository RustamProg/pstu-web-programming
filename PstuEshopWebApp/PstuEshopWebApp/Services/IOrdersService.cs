namespace PstuEshopWebApp.Services;

public interface IOrdersService
{
    Task<CreateOrderResult> CreateOrderAsync(string? cityCode, CancellationToken ct = default);
}