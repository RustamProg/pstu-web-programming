using Microsoft.AspNetCore.Mvc;
using PstuEshopWebApp.Constants;
using PstuEshopWebApp.Services;

namespace PstuEshopWebApp.Controllers;

public class OrdersController(IOrdersService ordersService) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrder(CancellationToken ct)
    {
        var cityCode = Request.Cookies[CityOptions.CookieName];

        var result = await ordersService.CreateOrderAsync(cityCode, ct);

        if (result.OrderId is null)
        {
            TempData["CartError"] = result.Error;
        }
        else
        {
            TempData["OrderSuccess"] = $"Заказ {result.OrderId} успешно оформлен!";
        }

        return RedirectToAction("Index", "Cart");
    }
}
