namespace PstuEshopWebApp.Services;

public class AddCartResult
{
    public required int Quantity { get; set; }
    public required AddCartStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}

public enum AddCartStatus
{
    Success,
    NotFound,
    LimitExceeded
}
