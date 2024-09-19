using System.Security.Claims;
using IWantApp.Domain.Infra.Data;
using IWantApp.Domain.Products;
using IWantApp.Endpoints.Products.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Endpoints.Products;

public static class ProductPost
{
    public static string Template => "/products";
    public static string[] Methods => new[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ProductRequest request, ApplicationDbContext context,
        HttpContext httpContext)
    {
        var userId = httpContext.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId);

        if (category == null)
        {
            var error = new Dictionary<string, string[]>
            {
                { "Error", ["Category not found"] }
            };
            return Results.ValidationProblem(error);
        }

        var product = new Product(
            request.Name, category, request.Description, request.HasStock, userId
        );

        if (!product.IsValid)
            return Results.ValidationProblem(product.Notifications.ConvertToProblemDetails());

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return Results.Created($"/products/{product.Id}", product.Id);
    }
}
