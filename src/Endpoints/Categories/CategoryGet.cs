﻿using IWantApp.Domain.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryGet
{
    public static string Template => "/categories";
    public static string[] Methods => new[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(ApplicationDbContext context)
    {
        var categories = context.Categories
            .ToList();

        var response = categories
            .Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Active = category.Active
            });

        return Results.Ok(response);
    }
}