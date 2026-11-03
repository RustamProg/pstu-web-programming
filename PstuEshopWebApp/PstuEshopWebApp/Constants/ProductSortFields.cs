namespace PstuEshopWebApp.Constants;

public static class ProductSortFields
{
    public const string PriceAsc = "price_asc";
    public const string PriceDesc = "price_desc";
    public const string NameAsc = "name_asc";
    public const string NameDesc = "name_desc";

    public static readonly ISet<string> All = new HashSet<string>
    {
        PriceAsc,
        PriceDesc,
        NameAsc,
        NameDesc
    };
}
