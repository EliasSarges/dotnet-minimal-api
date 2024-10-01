using IWantApp.Endpoints.Categories.DTO;

namespace IWantApp.Endpoints.Products.DTO;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    CategoryResponse Category,
    bool Active,
    bool HasStock);
