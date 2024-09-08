using IWantApp.Domain.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id}";
    public static string[] Methods => new[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, CategoryRequest request, ApplicationDbContext context)
    {
        var category = context.Categories
            .FirstOrDefault(category => category.Id == id);

        if (category == null) return Results.NotFound();

        if (!category.IsValid) return Results.BadRequest(category.Notifications);

        category.Name = request.Name;
        category.Active = request.Active;
        context.SaveChanges();

        return Results.Ok();
    }
}
