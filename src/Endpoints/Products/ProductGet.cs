using IWantApp.Domain.Infra.Data;
using IWantApp.Endpoints.Products.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Endpoints.Products;

public class ProductGet
{
    public static string Template => "/products/{id:guid}";
    public static string[] Methods => new[] { HttpMethods.Get };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var product = await context.Products
            .Include(product => product.Category)
            .FirstOrDefaultAsync(product => product.Id == id);

        if (product == null)
            return Results.NotFound();

        var category = product.Category;

        var productResponse = new ProductResponse(product.Id,
            product.Name,
            product.Description,
            category.Name,
            product.Price,
            product.Active,
            product.HasStock);

        return Results.Ok(productResponse);
    }
}
