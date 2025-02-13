using Microsoft.EntityFrameworkCore;
using ZaneStacked.Api.Endpoints;
using ZaneStacked.Api.Persistence.EFCore.Data;
using ZaneStacked.Api.Persistence.EFCore.Repositories;
using ZaneStacked.Api.Persistence.Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ZaneStackedDbContext>(options =>
    options.UseSqlite("Data Source=zane-stacked.db"));

builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapSkillEndpoints();
app.MapProjectEndpoints();

app.MapGet("/", () => "Welcome to ZaneStacked API!");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Seed(services);
}

app.Run();