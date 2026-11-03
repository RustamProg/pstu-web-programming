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

        var count = await cartService.AddAsync(request.ProductId, ct);

        if (count is null)
            return NotFound();

        return Ok(new { totalQuantity = count.Value });
    }
}
