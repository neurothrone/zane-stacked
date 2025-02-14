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

        group.MapGet("/state",
            (HttpContext httpContext) => httpContext.User.Identity is { IsAuthenticated: true }
                ? Results.Ok(new { Username = httpContext.User.Identity.Name })
                : Results.Ok());

        // provide an endpoint to clear the cookie for logout
        //
        // For more information on the logout endpoint and antiforgery, see:
        // https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity#antiforgery-support
        group.MapPost("/logout", async (SignInManager<AppUser> signInManager, [FromBody] object empty) =>
        {
            if (empty is not null)
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            }

            return Results.Unauthorized();
        }).RequireAuthorization();

        // provide an endpoint for user roles
        group.MapGet("/roles", (ClaimsPrincipal user) =>
        {
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)user.Identity;
                var roles = identity.FindAll(identity.RoleClaimType)
                    .Select(c =>
                        new
                        {
                            c.Issuer,
                            c.OriginalIssuer,
                            c.Type,
                            c.Value,
                            c.ValueType
                        });

                return TypedResults.Json(roles);
            }

            return Results.Unauthorized();
        }).RequireAuthorization();

        group.MapPost("/login-jwt", async (
            [FromBody] AuthLoginDto model,
            UserManager<AppUser> userManager,
            IConfiguration config) =>
        {
            var user = await userManager.FindByEmailAsync(model.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                return Results.Unauthorized();

            var token = GenerateJwtToken(user, config);
            return Results.Ok(new { Token = token });
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