using Microsoft.AspNetCore.Mvc;
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
}
