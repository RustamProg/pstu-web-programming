using Microsoft.AspNetCore.Mvc;
using PstuEshopWebApp.Constants;
using PstuEshopWebApp.Requests;

namespace PstuEshopWebApp.Controllers.Api;

[ApiController]
[Route("api/city")]
public class CityApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var code = Request.Cookies[CityOptions.CookieName];

        return Ok(new
        {
            selectedCity = code is not null && CityOptions.Names.ContainsKey(code)
                ? code
                : null,

            cities = CityOptions.Names.Select(x => new
            {
                code = x.Key,
                name = x.Value
            })
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Select([FromBody] SelectCityRequest request)
    {
        if (!CityOptions.Names.ContainsKey(request.City))
            return BadRequest(new { message = "Неизвестный город" });

        Response.Cookies.Append(
            CityOptions.CookieName,
            request.City,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                Path = "/"
            });

        return Ok(new { selectedCity = request.City });
    }
}
