using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGet
{
    public static string Template => "employee/{id:guid}";
    public static string[] Methods => new[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, UserManager<IdentityUser> userManager)
    {
        var employee = userManager.Users
            .FirstOrDefault(user => user.Id == id.ToString());

        if (employee == null)
            return Results.NotFound();

        return Results.Ok(employee);
    }
}
