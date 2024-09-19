namespace IWantApp.Endpoints.Products.DTO;

public record ProductRequest(string Name, string Description, bool HasStock, Guid CategoryId);
