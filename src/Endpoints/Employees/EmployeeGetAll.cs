using Dapper;
using Microsoft.Data.SqlClient;

namespace IWantApp.Endpoints.Employees;

public static class EmployeeGetAll
{
    public static string Template => "/employee";
    public static string[] Methods => new[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    private static IResult Action(int? page, int? rows, IConfiguration configuration)
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

        var db = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);

        var query = @"select U.Email,
                     UC.ClaimValue as Name
            from AspNetUsers U
                     inner join AspNetUserClaims UC on UC.UserId = U.Id
            where UC.ClaimType = 'Name'
            order by 1
            offset (@page - 1) * @rows rows fetch next @rows rows only";

        var employees = db.Query<EmployeeResponse>(query, new { page, rows });

        return Results.Ok(employees);
    }
}
