using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZaneStacked.Api.Endpoints;
using ZaneStacked.Api.Persistence.EFCore.Data;
using ZaneStacked.Api.Persistence.EFCore.Models;
using ZaneStacked.Api.Persistence.EFCore.Repositories;
using ZaneStacked.Api.Persistence.Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Database Configuration
builder.Services.AddDbContext<ZaneStackedDbContext>(options =>
    options.UseSqlite("Data Source=zane-stacked.db"));

// Identity Setup
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ZaneStackedDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 16)
    throw new Exception("JWT Key must be at least 16 characters long.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "zanestacked";

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidIssuer = jwtIssuer
        };
    });
builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAuthEndpoints();
app.MapSkillEndpoints();
app.MapProjectEndpoints();

app.MapGet("/", () => "Welcome to ZaneStacked API!");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Seed(services);
}

app.Run();