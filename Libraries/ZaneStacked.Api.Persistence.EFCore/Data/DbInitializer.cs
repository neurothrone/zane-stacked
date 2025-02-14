using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZaneStacked.Api.Persistence.EFCore.Models;
using ZaneStacked.Api.Persistence.Shared.Models;

namespace ZaneStacked.Api.Persistence.EFCore.Data;

public static class DbInitializer
{
    private class SeedUser : AppUser
    {
        public string[]? RoleList { get; set; }
    }

    private static readonly IEnumerable<SeedUser> SeedUsers =
    [
        new()
        {
            Email = "zane@example.com",
            NormalizedEmail = "ZANE@EXAMPLE.COM",
            NormalizedUserName = "ZANE@EXAMPLE.COM",
            RoleList = ["Admin", "Manager"],
            UserName = "zane@example.com"
        },
        new()
        {
            Email = "guest@example.com",
            NormalizedEmail = "GUEST@EXAMPLE.COM",
            NormalizedUserName = "GUEST@EXAMPLE.COM",
            RoleList = ["User"],
            UserName = "guest@example.com"
        },
    ];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await SeedUsersAsync(serviceProvider);
        await SeedData(serviceProvider);
    }

    private static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        await using var context = new ZaneStackedDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ZaneStackedDbContext>>());

        if (context.Users.Any())
        {
            return;
        }

        var userStore = new UserStore<AppUser>(context);
        var password = new PasswordHasher<AppUser>();

        using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = ["Admin", "Manager", "User"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        foreach (var user in SeedUsers)
        {
            var hashed = password.HashPassword(user, "Password1!");
            user.PasswordHash = hashed;
            await userStore.CreateAsync(user);

            if (user.Email is not null)
            {
                var appUser = await userManager.FindByEmailAsync(user.Email);

                if (appUser is not null && user.RoleList is not null)
                {
                    await userManager.AddToRolesAsync(appUser, user.RoleList);
                }
            }
        }

        await context.SaveChangesAsync();
    }


    private static async Task SeedData(IServiceProvider serviceProvider)
    {
        await using var context = new ZaneStackedDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ZaneStackedDbContext>>());

        // Prevent duplicate seeding
        if (context.Projects.Any() || context.Skills.Any())
            return;

        // Create skills
        var skills = new List<Skill>
        {
            new() { Name = "C#", YearsOfExperience = 3, Proficiency = "Expert" },
            new() { Name = "ASP.NET Core", YearsOfExperience = 2, Proficiency = "Advanced" },
            new() { Name = "Blazor", YearsOfExperience = 1, Proficiency = "Intermediate" },
            new() { Name = "JavaScript", YearsOfExperience = 3, Proficiency = "Expert" },
            new() { Name = "React", YearsOfExperience = 2, Proficiency = "Advanced" },
            new() { Name = "SQL", YearsOfExperience = 3, Proficiency = "Advanced" },
            new() { Name = "Docker", YearsOfExperience = 1, Proficiency = "Intermediate" },
            new() { Name = "AWS", YearsOfExperience = 1, Proficiency = "Intermediate" }
        };

        await context.Skills.AddRangeAsync(skills);
        await context.SaveChangesAsync();

        // Create projects
        var projects = new List<Project>
        {
            new()
            {
                Name = "Project Excuses",
                Description = "An API that generates absurd, yet plausible excuses for various situations.",
                GitHubUrl = "https://github.com/Zane/ProjectExcuses",
                DemoUrl = "https://projectexcuses.com/demo",
                FeaturedImage = "/images/excuses.jpg",
                Skills = skills.Where(s => s.Name == "C#" || s.Name == "ASP.NET Core").ToList()
            },
            new()
            {
                Name = "Spotly",
                Description = "A location-based app for storing and sharing cool spots. Built with Flutter.",
                GitHubUrl = "https://github.com/Zane/Spotly",
                DemoUrl = "https://spotly.com/demo",
                FeaturedImage = "/images/spotly.jpg",
                Skills = skills.Where(s => s.Name == "JavaScript" || s.Name == "SQL").ToList()
            },
            new()
            {
                Name = "Deployable Me",
                Description =
                    "A self-hosted resume that updates dynamically with my latest projects. It's like LinkedIn, but actually good.",
                GitHubUrl = "https://github.com/Zane/DeployableMe",
                FeaturedImage = "/images/deployable.jpg",
                Skills = skills.Where(s => s.Name == "Blazor" || s.Name == "ASP.NET Core").ToList()
            },
            new()
            {
                Name = "404 Talent Not Found",
                Description =
                    "A website that automatically generates rejection emails so you can practice handling disappointment. Fun!",
                GitHubUrl = "https://github.com/Zane/404TalentNotFound",
                FeaturedImage = "/images/404.jpg",
                Skills = skills.Where(s => s.Name == "JavaScript" || s.Name == "React").ToList()
            },
            new()
            {
                Name = "AI-Powered Standup Comedy",
                Description = "A bot that generates dark humor based on audience input. No filter, just regret.",
                GitHubUrl = "https://github.com/Zane/AiComedyBot",
                DemoUrl = "https://aicomedybot.com",
                FeaturedImage = "/images/comedy.jpg",
                Skills = skills.Where(s => s.Name == "Python" || s.Name == "SQL").ToList()
            },
            new()
            {
                Name = "Doom on a Smart Fridge",
                Description = "Because why wouldn't you want to play Doom on a fridge? Runs in a Docker container.",
                GitHubUrl = "https://github.com/Zane/DoomFridge",
                FeaturedImage = "/images/fridge.jpg",
                Skills = skills.Where(s => s.Name == "C#" || s.Name == "Docker").ToList()
            }
        };

        await context.Projects.AddRangeAsync(projects);
        await context.SaveChangesAsync();
    }
}