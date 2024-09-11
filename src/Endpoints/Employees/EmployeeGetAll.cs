using IWantApp.Domain.Infra.Data;

namespace IWantApp.Endpoints.Employees;

public static class EmployeeGetAll
{
    public static string Template => "/employee";
    public static string[] Methods => new[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    private static IResult Action(int page, int rows, ApplicationDbContext context)
    {
        var users = context.Users
            .Skip((page - 1) * rows)
            .Take(rows)
            .ToList();

        var claims = context.UserClaims
            .Where(claim => claim.ClaimType == "Name")
            .ToList();

        var result = users.Select(user =>
        {
            var userName = claims
                .FirstOrDefault(c => c.UserId == user.Id)
                ?.ClaimValue ?? string.Empty;

            return new EmployeeResponse(user.Email, userName);
        });

        return Results.Ok(result);
    }
}
