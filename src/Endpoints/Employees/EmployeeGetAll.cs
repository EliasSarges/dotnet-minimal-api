using IWantApp.Domain.Infra.Data;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Employees;

public static class EmployeeGetAll
{
    public static string Template => "/employee";
    public static string[] Methods => new[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithClaimName query)
    {
        Dictionary<string, string[]> error = new();

        if (page == null || rows == null)
        {
            error.Add("Error", ["Query params Page and Rows are required."]);
            return Results.ValidationProblem(error);
        }

        if (rows > 10)
        {
            error.Add("Error", ["Query params Rows are limited to 10 items."]);
            return Results.ValidationProblem(error);
        }

        var result = await query.Execute(page.Value, rows.Value);

        return Results.Ok(result);
    }
}
