using IWantApp.Domain.Infra.Data;
using IWantApp.Domain.Products;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest request, ApplicationDbContext context)
    {
        var category = new Category(request.Name, "admin", "admin")
        {
            CreatedOn = DateTime.Now,
            EditedOn = DateTime.Now
        };

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
