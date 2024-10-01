using System.Security.Claims;
using IWantApp.Endpoints.Employees.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employee";
    public static string[] Methods => new[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeeRequest request, UserManager<IdentityUser> userManager,
        HttpContext httpContext)
    {
        var userId = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        var userClaims = new List<Claim>
        {
            new("EmployeeCode", request.EmployeeCode),
            new("Name", request.Name),
            new("CreatedBy", userId),
            new("CreatedOn", DateTime.Now.ToString())
        };

        var claimResult = await userManager.AddClaimsAsync(user, userClaims);

        if (!result.Succeeded || !claimResult.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        return Results.Created($"/employee/{user.Id}", user.Id);
    }
}
