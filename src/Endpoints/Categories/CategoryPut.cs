using System.Security.Claims;
using IWantApp.Domain.Infra.Data;
using IWantApp.Endpoints.Categories.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id, CategoryRequest request, ApplicationDbContext context,
        HttpContext httpContext)
    {
        var userId = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Results.Unauthorized();

        var category = context.Categories
            .FirstOrDefault(category => category.Id == id);

        if (category == null) return Results.NotFound();

        category.EditInfo(request.Name, category.Active, userId, DateTime.Now);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
