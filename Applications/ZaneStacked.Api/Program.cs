using Microsoft.EntityFrameworkCore;
using ZaneStacked.Api.Persistence.EFCore.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ZaneStackedDbContext>(options =>
    options.UseSqlite("Data Source=zane-stacked.db"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
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

app.MapGet("/", () => "Welcome to ZaneStacked API!");

app.Run();

