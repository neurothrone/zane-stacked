using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZaneStacked.Api.Endpoints;
using ZaneStacked.Api.Persistence.EFCore.Data;
using ZaneStacked.Api.Persistence.EFCore.Models;
using ZaneStacked.Api.Persistence.EFCore.Repositories;
using ZaneStacked.Api.Persistence.Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

// Source: https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAssemblyStandaloneWithIdentity

// Establish cookie authentication
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

// Configure authorization
builder.Services.AddAuthorizationBuilder();

// Database Configuration
// builder.Services.AddDbContext<ZaneStackedDbContext>(options =>
//     options.UseSqlite("Data Source=zane-stacked.db"));

// builder.Services.AddDbContext<ZaneStackedDbContext>(
//     options =>
//     {
//         options.UseInMemoryDatabase("AppDb");
//         //For debugging only: options.EnableDetailedErrors(true);
//         //For debugging only: options.EnableSensitiveDataLogging(true);
//     });

builder.Services.AddDbContext<ZaneStackedDbContext>(options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("AzureConnection") ?? throw new InvalidOperationException(
                "Connection string 'AzureConnection' not found.")
        );

#if DEBUG
        options.EnableSensitiveDataLogging();
#endif
    }
);

builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ZaneStackedDbContext>()
    .AddApiEndpoints();

builder.Services.AddCors(
    options => options.AddPolicy(
        "AllowWasmOrigin",
        policy => policy
            .WithOrigins(
                builder.Configuration["ApiUrl"] ?? throw new Exception("ApiUrl must be set in appsettings.json"),
                builder.Configuration["ClientUrl"] ?? throw new Exception("ClientUrl must be set in appsettings.json")
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddHttpClient(
    "ExcusesApi",
    opt => opt.BaseAddress = new Uri(builder.Configuration["ExcusesApiUrl"] ??
                                     throw new Exception("ExcusesApiUrl must be set in appsettings.json"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Apply migrations
    // await app.ApplyMigrations();

    // Seed the database
    await using var scope = app.Services.CreateAsyncScope();
    await DbInitializer.InitializeAsync(scope.ServiceProvider);

    // app.UseSwagger();
    // app.UseSwaggerUI();
}

// NOTE: For testing purposes. Remove in production.
app.UseSwagger();
app.UseSwaggerUI();

// Create routes for the identity endpoints
app.MapIdentityApi<AppUser>();

// Activate the CORS policy
app.UseCors("AllowWasmOrigin");

// Enable authentication and authorization after CORS Middleware
// processing (UseCors) in case the Authorization Middleware tries
// to initiate a challenge before the CORS Middleware has a chance
// to set the appropriate headers.
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapGet("/", () => "Welcome to ZaneStacked API!");
app.MapAuthEndpoints();
app.MapSkillEndpoints();
app.MapProjectEndpoints();
app.MapExcuseEndpoints();

app.Run();