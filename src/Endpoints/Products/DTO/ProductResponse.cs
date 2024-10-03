namespace IWantApp.Endpoints.Products.DTO;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    string CategoryName,
    decimal Price,
    bool Active,
    bool HasStock);
