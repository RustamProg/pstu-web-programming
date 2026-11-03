namespace PstuEshopWebApp.Constants;

public static class CityOptions
{
    public const string CookieName = "selected_city";

    public static readonly IReadOnlyDictionary<string, string> Names = new Dictionary<string, string>
    {
        ["msk"] = "Москва",
        ["spb"] = "Санкт-Петербург",
        ["prm"] = "Пермь"
    };
}
