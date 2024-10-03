using IWantApp.Domain.Infra.Data;
using IWantApp.Endpoints.Products.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Endpoints.Products;

public class ProductGetShowcase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new[] { HttpMethods.Get };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action(ApplicationDbContext dbContext, int page = 1, int rows = 10, string orderBy = "name")
    {
        if (rows > 10)
            return Results.Problem(title: "Row count must be max 10", statusCode: 400);

        var query = dbContext.Products.AsNoTracking().Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        switch (orderBy)
        {
            case "name":
                query.OrderBy(p => p.Name);
                break;

            case "price":
                query.OrderBy(p => p.Name);
                break;

            default:
                return Results.Problem(title: "Order only by price or name", statusCode: 400);
        }

        var queryFilter = query.Skip((page - 1) * rows)
            .Take(rows);

        var products = queryFilter.ToList();

        var productsResponse = products
            .Select(p => new ProductResponse(p.Id,
                p.Name,
                p.Description,
                p.Category.Name,
                p.Price,
                p.Active,
                p.HasStock));

        return Results.Ok(productsResponse);
    }
}
