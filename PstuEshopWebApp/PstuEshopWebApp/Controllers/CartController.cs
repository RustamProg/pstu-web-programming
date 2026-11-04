using Microsoft.AspNetCore.Mvc;
using PstuEshopWebApp.Services;

namespace PstuEshopWebApp.Controllers;

public class CartController(ICartService cartService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var model = await cartService.GetCartViewModelAsync(ct);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity([FromForm] long productId, [FromForm] int quantity, CancellationToken ct)
    {
        if (productId <= 0 || quantity is < 1 or > 99)
            return BadRequest("Некорректное количество");

        var updated = await cartService.SetExactQuantityAsync(productId, quantity, ct);

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove([FromForm] long productId, CancellationToken ct)
    {
        if (productId <= 0)
            return BadRequest();

        await cartService.RemoveAsync(productId, ct);

        return RedirectToAction(nameof(Index));
    }
}
