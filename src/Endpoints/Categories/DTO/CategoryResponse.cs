namespace IWantApp.Endpoints.Categories.DTO;

public record CategoryResponse(
    Guid Id,
    string Name,
    bool Active
);
