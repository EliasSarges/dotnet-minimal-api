using System.Security.Claims;
using IWantApp.Domain.Infra.Data;
using IWantApp.Domain.Products;
using IWantApp.Endpoints.Categories.DTO;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(CategoryRequest request, ApplicationDbContext context,
        HttpContext httpContext)
    {
        var userId = httpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        var category = new Category(request.Name, userId);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
