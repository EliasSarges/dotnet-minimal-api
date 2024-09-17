using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IWantApp.Endpoints.Security;

public class TokenPost
{
    public static string Template => "/token";
    public static string[] Methods => [HttpMethod.Post.ToString()];
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action(LoginRequest request, UserManager<IdentityUser> userManager,
        IConfiguration configuration)
    {
        var user = userManager.FindByEmailAsync(request.Email).Result;

        if (user == null)
            return Results.NotFound();

        if (!userManager.CheckPasswordAsync(user, request.Password).Result)
            return Results.Unauthorized();

        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:Secret"]);

        var claims = userManager.GetClaimsAsync(user).Result;
        var subject = new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Email, request.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        });
        subject.AddClaims(claims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddSeconds(60)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Results.Ok(new { token = tokenHandler.WriteToken(token) });
    }
}
