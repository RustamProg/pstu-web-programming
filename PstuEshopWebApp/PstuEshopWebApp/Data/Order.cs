namespace PstuEshopWebApp.Data;

public class Order
{
    public long Id { get; set; }

    public required string City { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public List<OrderItem> Items { get; set; } = [];
}

public class OrderItem
{
    public long Id { get; set; }

    public long OrderId { get; set; }
    public Order? Order { get; set; }

    public long ProductId { get; set; }

    public required string ProductName { get; set; }

    // Можно было бы брать из товара,
    // но цена может меняться,
    // поэтому сохраняем цену на момент покупки
    // + товар в целом может быть удален из базы
    public decimal SingleItemPrice { get; set; } 

    public int Quantity { get; set; }
}
