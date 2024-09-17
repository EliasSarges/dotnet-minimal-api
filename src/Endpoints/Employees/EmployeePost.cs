using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employee";
    public static string[] Methods => new[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(EmployeeRequest request, UserManager<IdentityUser> userManager,
        HttpContext httpContext)
    {
        var userId = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = userManager.CreateAsync(user, request.Password).Result;

        var userClaims = new List<Claim>
        {
            new("EmployeeCode", request.EmployeeCode),
            new("Name", request.Name),
            new("CreatedBy", userId),
            new("CreatedOn", DateTime.Now.ToString())
        };

        var claimResult = userManager
            .AddClaimsAsync(user, userClaims)
            .Result;

        if (!result.Succeeded || !claimResult.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        return Results.Created($"/employee/{user.Id}", user.Id);
    }
}
