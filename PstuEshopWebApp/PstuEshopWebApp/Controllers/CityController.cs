using Microsoft.AspNetCore.Mvc;
using PstuEshopWebApp.Constants;

namespace PstuEshopWebApp.Controllers;

public class CityController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Select(string city, string? returnUrl)
    {
        if (!CityOptions.Names.ContainsKey(city))
            return BadRequest("Неизвестный город");

        Response.Cookies.Append(
            CityOptions.CookieName,
            city,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                Path = "/"
            });

        if (Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction("Index", "Products");
    }
}
