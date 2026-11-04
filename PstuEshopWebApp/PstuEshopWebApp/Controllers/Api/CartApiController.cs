using Microsoft.AspNetCore.Mvc;
using PstuEshopWebApp.Constants;
using PstuEshopWebApp.Requests;
using PstuEshopWebApp.Services;

namespace PstuEshopWebApp.Controllers.Api;

[ApiController]
[Route("api/cart")]
public class CartApiController(ICartService cartService) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
    {
        var count = await cartService.GetCountAsync(ct);

        return Ok(new { totalQuantity = count });
    }

    [HttpPost("items")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemRequest request, CancellationToken ct)
    {
        if (request.ProductId <= 0)
            return BadRequest("Invalid product ID");

        var result = await cartService.AddAsync(request.ProductId, ct);

        if (result.Status == AddCartStatus.NotFound)
            return NotFound(new { Message = result.ErrorMessage });

        if (result.Status == AddCartStatus.LimitExceeded)
            return Conflict(new { Message = result.ErrorMessage });

        return Ok(new { totalQuantity = result.Quantity });
    }

    [HttpGet]
    public async Task<IActionResult> GetCart(CancellationToken ct)
    {
        var cart = await cartService.GetCartViewModelAsync(ct);

        return Ok(cart);
    }

    [HttpPut("items/{productId:long}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(long productId, [FromBody] UpdateQuantityRequest request, CancellationToken ct)
    {
        if (productId <= 0 || request.Quantity is < 1 or > CartOptions.MaxQuantity)
            return BadRequest("Некорректное количество");

        var updated = await cartService.SetExactQuantityAsync(productId, request.Quantity, ct);

        if (!updated)
            return NotFound(new { message = "Товар отсутствует в корзине" });

        return Ok(await cartService.GetCartViewModelAsync(ct));
    }

    [HttpDelete("items/{productId:long}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveItem(long productId, CancellationToken ct)
    {
        await cartService.RemoveAsync(productId, ct);

        return Ok(await cartService.GetCartViewModelAsync(ct));
    }
}
