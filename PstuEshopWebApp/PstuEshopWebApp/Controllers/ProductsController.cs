using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PstuEshopWebApp.Constants;
using PstuEshopWebApp.Data;
using PstuEshopWebApp.Models;
using PstuEshopWebApp.Services;

namespace PstuEshopWebApp.Controllers;

public class ProductsController(IProductsService productsService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? category, string? sort, CancellationToken ct)
    {
        var cityCode = Request.Cookies[CityOptions.CookieName];

        var model = new CatalogViewModel
        {
            Category = category,
            Sort = !string.IsNullOrWhiteSpace(sort) && ProductSortFields.All.Contains(sort) 
                ? sort
                : ProductSortFields.NameAsc,
            Categories = await productsService.GetCategoriesAsync(ct),
            Products = await productsService.GetProductsAsync(category, sort, ct),
            SelectedCityCode = !string.IsNullOrWhiteSpace(cityCode) && CityOptions.Names.ContainsKey(cityCode)
                ? cityCode
                : null
        };

        return View(model);
    }

    // Вызывается только из catalog.js
    [HttpGet]
    public async Task<IActionResult> List(string? category, string? sort, CancellationToken ct)
    {
        var products = await productsService.GetProductsAsync(category, sort, ct);

        return PartialView("_ProductCards", products);
    }
}
