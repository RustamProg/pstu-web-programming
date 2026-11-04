namespace PstuEshopWebApp.Services;

public class AddCartResult
{
    public required int Quantity { get; init; }
    public required AddCartStatus Status { get; init; }
    public string? ErrorMessage { get; init; }
}

public enum AddCartStatus
{
    Success,
    NotFound,
    LimitExceeded
}
