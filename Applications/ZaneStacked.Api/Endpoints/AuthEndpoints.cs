using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZaneStacked.Api.DTOs.Auth;
using ZaneStacked.Api.Persistence.EFCore.Models;

namespace ZaneStacked.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        group.MapPost("/register",
            async ([FromBody] AuthRegisterRequest model, UserManager<AppUser> userManager) =>
            {
                var user = new AppUser { UserName = model.Username, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                return result.Succeeded ? Results.Ok("User registered") : Results.BadRequest(result.Errors);
            });

        group.MapPost("/login",
            async ([FromBody] AuthLoginRequest model, UserManager<AppUser> userManager, IConfiguration config) =>
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                    return Results.Unauthorized();

                var token = GenerateJwtToken(user, config);
                return Results.Ok(new { token });
            });
    }

    private static string GenerateJwtToken(AppUser user, IConfiguration config)
    {
        var jwtKey = config["Jwt:Key"] ?? "supersecretkey";
        var jwtIssuer = config["Jwt:Issuer"] ?? "zanestacked";

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(3);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}