using Microsoft.AspNetCore.Mvc;
using PstuEshopWebApp.Services;

namespace PstuEshopWebApp.Controllers.Api;

[ApiController]
[Route("api/products")]
public class ProductsApiController(IProductsService productsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] string? sort, CancellationToken ct)
    {
        var products = await productsService.GetProductsAsync(category, sort, ct);

        return Ok(products);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
    {
        var categories = await productsService.GetCategoriesAsync(ct);

        return Ok(categories);
    }

}
