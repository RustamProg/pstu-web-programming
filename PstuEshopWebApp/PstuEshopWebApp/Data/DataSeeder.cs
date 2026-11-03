using Microsoft.EntityFrameworkCore;

namespace PstuEshopWebApp.Data;

public static class DataSeeder
{
    public static void Seed(DbContext context)
    {
        var products = context.Set<Product>();

        if (products.Any())
            return;

        products.AddRange(
            new Product
            {
                Name = "Logitech G102",
                Category = "Мыши",
                Description = "Бюджетная игровая мышь с высокой точностью и эргономичным дизайном.",
                Price = 1999.00M
            },
            new Product
            {
                Name = "Razer DeathAdder",
                Category = "Мыши",
                Description = "Среднебюджетная игровая мышь с высокой точностью и эргономичным дизайном.",
                Price = 3499.00M
            },
            new Product
            {
                Name = "Logitech MX Master 3S",
                Category = "Мыши",
                Description = "Премиальная мышь с высокой точностью и эргономичным дизайном.",
                Price = 9999.00M
            },
            new Product
            {
                Name = "Keychron K2",
                Category = "Клавиатуры",
                Description = "Дорогая механическая клавиатура с RGB подсветкой.",
                Price = 8999.00M
            },
            new Product
            {
                Name = "Logitech K380",
                Category = "Клавиатуры",
                Description = "Бюджетная беспроводная клавиатура с подсветкой.",
                Price = 3999.00M
            },
            new Product
            {
                Name = "Redragon Kumara",
                Category = "Клавиатуры",
                Description = "Среднебюджетная механическая клавиатура без подсветки.",
                Price = 4499.00M
            },
            new Product
            {
                Name = "Samsung Odyssey G5",
                Category = "Мониторы",
                Description = "Премиальный монитор 4К разрешения, изогнутый.",
                Price = 29999.00M
            },
            new Product
            {
                Name = "LG UltraGear 27",
                Category = "Мониторы",
                Description = "Среднебюджетный монитор с диагональю 27 дюймов.",
                Price = 24999.00M
            },
            new Product
            {
                Name = "AOC 24G2",
                Category = "Мониторы",
                Description = "Бюджетный монитор с IPS матрицей.",
                Price = 15999.00M
            }
        );

        context.SaveChanges();
    }
}
