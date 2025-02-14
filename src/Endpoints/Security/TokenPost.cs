using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IWantApp.Endpoints.Security.DTO;
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
    public static async Task<IResult> Action(LoginRequest request, UserManager<IdentityUser> userManager,
        IConfiguration configuration, ILogger<TokenPost> logger, IWebHostEnvironment environment)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return Results.NotFound();

        if (!await userManager.CheckPasswordAsync(user, request.Password))
            return Results.Unauthorized();

        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:Secret"]);

        var claims = await userManager.GetClaimsAsync(user);
        var subject = new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Email, request.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        });
        subject.AddClaims(claims);

        var expires = environment.IsDevelopment() || environment.IsStaging()
            ? DateTime.UtcNow.AddYears(1)
            : DateTime.UtcNow.AddSeconds(3600);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            Expires = expires
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        logger.LogInformation("Generated JWT Token {User}", user.Id);

        return Results.Ok(new { token = tokenHandler.WriteToken(token) });
    }
}
