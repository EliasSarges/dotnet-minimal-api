namespace IWantApp.Endpoints.Products.DTO;

public record ProductRequest(string Name, string Description, decimal price, bool HasStock, Guid CategoryId);
