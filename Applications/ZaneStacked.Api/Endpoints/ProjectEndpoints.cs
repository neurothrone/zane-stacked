using ZaneStacked.Api.Persistence.Shared.Interfaces;
using ZaneStacked.Api.Utils;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Api.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects")
            .WithTags("Projects");

        group.MapPost("/", async (InputProjectDto project, IProjectRepository repo) =>
        {
            var createdProject = await repo.AddAsync(project.ToModel(), project.SkillIds.ToArray());
            return Results.Created($"/api/projects/{createdProject.Id}", createdProject.ToDto());
        }).RequireAuthorization();

        group.MapGet("/", async (IProjectRepository repo) =>
        {
            var projects = await repo.GetAllAsync();
            return Results.Ok(projects.Select(p => p.ToDto()));
        });

        group.MapGet("/{id:int:min(0)}", async (int id, IProjectRepository repo) =>
        {
            var project = await repo.GetByIdAsync(id);
            return project is null ? Results.NotFound() : Results.Ok(project.ToDto());
        });


        group.MapPut("/{id:int:min(0)}", async (int id, InputProjectDto project, IProjectRepository repo) =>
        {
            var updatedProject = await repo.UpdateAsync(project.ToModel(id: id), project.SkillIds.ToArray());
            return updatedProject is not null ? Results.Ok(updatedProject.ToDto()) : Results.NotFound();
        }).RequireAuthorization();

        group.MapDelete("/{id:int:min(0)}", async (int id, IProjectRepository repo) =>
            await repo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound()
        ).RequireAuthorization();
    }
}