using Microsoft.AspNetCore.Mvc;
using PstuEshopWebApp.Constants;
using PstuEshopWebApp.Services;

namespace PstuEshopWebApp.Controllers.Api;

[ApiController]
[Route("api/orders")]
public class OrdersApiController(IOrdersService ordersService) : ControllerBase
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrder(CancellationToken ct)
    {
        var cityCode = Request.Cookies[CityOptions.CookieName];

        var result = await ordersService.CreateOrderAsync(cityCode, ct);

        if (result.OrderId is null)
        {
            return BadRequest(new
            {
                message = result.Error
            });
        }

        return Ok(new
        {
            orderId = result.OrderId.Value
        });
    }
}
