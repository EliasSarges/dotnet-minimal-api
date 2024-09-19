namespace IWantApp.Endpoints.Categories.DTO;

public record CategoryRequest(
    string Name,
    bool Active = true
);
