using ZaneStacked.Api.DTOs;
using ZaneStacked.Api.Persistence.Shared.Interfaces;
using ZaneStacked.Api.Utils;

namespace ZaneStacked.Api.Endpoints;

public static class SkillEndpoints
{
    public static void MapSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/skills");

        group.MapPost("/", async (InputSkillDto skill, ISkillRepository repo) =>
        {
            var createdSkill = await repo.AddAsync(skill.ToModel());
            return Results.Created($"/api/skills/{createdSkill.Id}", createdSkill.ToDto());
        });

        group.MapGet("/", async (ISkillRepository repo) =>
        {
            var skills = await repo.GetAllAsync();
            return Results.Ok(skills.Select(s => s.ToDto()));
        });

        group.MapGet("/{id:int:min(0)}", async (int id, ISkillRepository repo) =>
        {
            var skill = await repo.GetByIdAsync(id);
            return skill == null ? Results.NotFound() : Results.Ok(skill.ToDto());
        });


        group.MapPut("/{id:int:min(0)}", async (int id, InputSkillDto skill, ISkillRepository repo) =>
        {
            var updatedSkill = await repo.UpdateAsync(skill.ToModel(id: id));
            return updatedSkill != null ? Results.Ok(updatedSkill) : Results.NotFound();
        });

        group.MapDelete("/{id:int:min(0)}", async (int id, ISkillRepository repo) =>
            await repo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound());
    }
}