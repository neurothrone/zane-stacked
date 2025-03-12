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
            return;

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
        List<Skill> skills =
        [
            new() { Name = "C#", YearsOfExperience = 5, Proficiency = "Expert" },
            new() { Name = ".NET", YearsOfExperience = 3, Proficiency = "Advanced" },
            new() { Name = ".NET MAUI", YearsOfExperience = 2, Proficiency = "Advanced" },
            new() { Name = "Flutter", YearsOfExperience = 3, Proficiency = "Advanced" },
            new() { Name = "Firebase", YearsOfExperience = 3, Proficiency = "Intermediate" },
            new() { Name = "Swift", YearsOfExperience = 3, Proficiency = "Advanced" },
            new() { Name = "SwiftUI", YearsOfExperience = 3, Proficiency = "Advanced" },
            new() { Name = "Python", YearsOfExperience = 3, Proficiency = "Advanced" },
            new() { Name = "SQL", YearsOfExperience = 3, Proficiency = "Advanced" },
            new() { Name = "JavaScript", YearsOfExperience = 3, Proficiency = "Advanced" },
            new() { Name = "TypeScript", YearsOfExperience = 2, Proficiency = "Advanced" },
            new() { Name = "React", YearsOfExperience = 2, Proficiency = "Advanced" },
            new() { Name = "Docker", YearsOfExperience = 2, Proficiency = "Intermediate" },
            new() { Name = "Blazor", YearsOfExperience = 1, Proficiency = "Intermediate" },
            new() { Name = "Azure", YearsOfExperience = 1, Proficiency = "Intermediate" },
            new() { Name = "AWS", YearsOfExperience = 1, Proficiency = "Intermediate" },
            new() { Name = "WebSockets", YearsOfExperience = 1, Proficiency = "Intermediate" }
        ];

        await context.Skills.AddRangeAsync(skills);
        await context.SaveChangesAsync();

        // Create projects
        List<Project> projects =
        [
            new()
            {
                Name = "ZaneStacked",
                Description =
                    "A full-stack portfolio site built with Blazor WebAssembly and a .NET API. Deployed on Azure because even my CV needs to scale.",
                GitHubUrl = "https://github.com/neurothrone/zane-stacked",
                FeaturedImage = "zanestacked.png",
                DemoUrl = "https://neurothrone.tech",
                Skills = skills.Where(s => s.Name is "Azure" or "Blazor" or ".NET" or "C#" or "SQL").ToList()
            },
            new()
            {
                Name = "Project Excuses",
                Description =
                    "An API that generates ridiculous yet oddly convincing excuses. It even has a React frontend, because excuses deserve a UI.",
                GitHubUrl = "https://github.com/neurothrone/project-excuses",
                FeaturedImage = "excuses-api-docs.png",
                DemoUrl = "https://excuses-react.netlify.app",
                Skills = skills.Where(s => s.Name is "Azure" or "Blazor" or "C#" or ".NET" or "React").ToList()
            },
            new()
            {
                Name = "Byte Me Truck",
                Description =
                    "A food truck frontend built with React and TypeScript that consumes an API running on AWS. Digital street food, served hot.",
                GitHubUrl = "https://github.com/neurothrone/byte-me-truck",
                FeaturedImage = "byte-me-truck.png",
                DemoUrl = "https://byte-me-truck.netlify.app",
                Skills = skills.Where(s => s.Name is "React" or "TypeScript" or "AWS" or "Docker").ToList()
            },
            new()
            {
                Name = "Project Victory",
                Description =
                    "A full-stack chat app with WebSockets featuring a React frontend and an Express backend, deployed on Azure. Because real-time conversations shouldn’t feel like waiting for dial-up.",
                GitHubUrl = "https://github.com/neurothrone/project-victory-react",
                FeaturedImage = "project-victory-react.jpg",
                DemoUrl = "https://www.youtube.com/watch?v=YsOR4z0WT4A",
                Skills = skills.Where(s =>
                    s.Name is "React" or "Express" or "JavaScript" or "WebSockets" or "Docker" or "Azure").ToList()
            },
            new()
            {
                Name = "AGI Events",
                Description =
                    "An exhibitor-focused event app available on AppStore and PlayStore. Scan badges, collect leads, and pretend you're not just here for the free snacks. Built in Flutter, then ported to .NET MAUI after realizing I hadn't suffered enough the first time.",
                GitHubUrl = "https://github.com/neurothrone/agi-events",
                FeaturedImage = "agi-events.png",
                DemoUrl = "https://www.youtube.com/watch?v=i_1-V75mpbo",
                Skills = skills.Where(s => s.Name is "Flutter" or "MAUI" or "Firebase").ToList()
            },
            new()
            {
                Name = "GasMatic",
                Description =
                    "A professional tool for electricians needing precise gas volume calculations. Available on AppStore, PlayStore, and Microsoft Store—because gas math should be everywhere. Written twice—first in Flutter, then in .NET MAUI. Because rewriting software builds character.",
                GitHubUrl = "https://github.com/neurothrone/flutter-gasmatic",
                FeaturedImage = "gasmatic.png",
                Skills = skills.Where(s => s.Name is "Flutter" or "Firebase" or ".NET MAUI").ToList()
            },
            new()
            {
                Name = "Spotly",
                Description = "A location-based app for storing and sharing cool spots. Think Yelp, but for hipsters.",
                GitHubUrl = "https://github.com/gu-tig333-ht24/spotly",
                FeaturedImage = "spotly.png",
                Skills = skills.Where(s => s.Name is "Flutter" or "Firebase").ToList()
            },
            new()
            {
                Name = "Parkheim",
                Description =
                    "A two-app system built with Flutter & Firebase: one for users to find parking spots and another for admins to manage them. Because parking shouldn't be a survival game.",
                GitHubUrl = "https://github.com/neurothrone/parkheim",
                FeaturedImage = "parkheim.png",
                Skills = skills.Where(s => s.Name is "Flutter" or "Firebase").ToList()
            },
            new()
            {
                Name = "Darkly Ever After",
                Description =
                    "A Python-based interactive story generator that lets users craft darkly comedic, existential tales. Because who doesn't love a good laugh about the inevitable void?",
                GitHubUrl = "https://github.com/neurothrone/darkly-ever-after",
                FeaturedImage = "darkly-ever-after.png",
                Skills = skills.Where(s => s.Name is "Python").ToList()
            },
            new()
            {
                Name = "Media Playlist Manager",
                Description =
                    "A cross-platform app for managing and playing audio playlists, built with a 5-tier architecture. Because a good playlist should survive more than just one OS.",
                GitHubUrl = "https://github.com/neurothrone/MediaPlaylistManager",
                FeaturedImage = "media-playlist-manager.png",
                Skills = skills.Where(s => s.Name is ".NET" or ".NET MAUI" or "C#").ToList()
            },
            new()
            {
                Name = "WeStock",
                Description =
                    "A Blazor-based inventory management system, ensuring you always know when you're running low on, well... everything.",
                GitHubUrl = "https://github.com/neurothrone/WeStock",
                FeaturedImage = "we-stock.png",
                Skills = skills.Where(s => s.Name is "Blazor" or ".NET" or "SQL").ToList()
            },
            new()
            {
                Name = "FastTrack",
                Description =
                    "A fasting tracker designed for convenience, available on iOS, iPadOS, Apple Watch, and macOS. Uses gestures instead of buttons, syncs with iCloud, and won't collect your data—because your fasts should be yours, not a corporation's.",
                GitHubUrl = "https://github.com/neurothrone/fast-track",
                FeaturedImage = "fast-track.png",
                Skills = skills.Where(s => s.Name is "SwiftUI").ToList()
            },
            new()
            {
                Name = "WorkWork",
                Description =
                    "A no-nonsense task manager available on iOS, iPadOS, Apple Watch (standalone), and macOS. Customizable, intuitive, and designed to keep you on track without the usual productivity app fluff. Because staying organized shouldn't feel like a full-time job.",
                GitHubUrl = "https://github.com/neurothrone/work-work",
                FeaturedImage = "work-work.jpeg",
                Skills = skills.Where(s => s.Name is "SwiftUI").ToList()
            }
        ];

        await context.Projects.AddRangeAsync(projects);
        await context.SaveChangesAsync();
    }
}