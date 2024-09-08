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
        var category = new Category
        {
            Name = request.Name,
            CreatedBy = "admin",
            CreatedOn = DateTime.Now,
            EditedBy = "admin",
            EditedOn = DateTime.Now
        };

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
