using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace PstuEshopWebApp.Controllers.Api;

[ApiController]
[Route("api/security")]
public class SecurityApiController(IAntiforgery antiforgery) : ControllerBase
{
    [HttpGet("antiforgery")]
    public IActionResult GetAntiforgeryToken()
    {
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);

        return Ok(new
        {
            token = tokens.RequestToken
        });
    }
}
