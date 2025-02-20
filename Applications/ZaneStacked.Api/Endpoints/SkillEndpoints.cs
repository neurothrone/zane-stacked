using ZaneStacked.Api.Persistence.Shared.Interfaces;
using ZaneStacked.Api.Utils;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Api.Endpoints;

public static class SkillEndpoints
{
    public static void MapSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/skills")
            .WithTags("Skills");

        group.MapPost("/", async (InputSkillDto skill, ISkillRepository repo) =>
        {
            var createdSkill = await repo.AddAsync(skill.ToModel());
            return Results.Created($"/api/skills/{createdSkill.Id}", createdSkill.ToDto());
        }).RequireAuthorization();

        group.MapGet("/", async (ISkillRepository repo) =>
        {
            var skills = await repo.GetAllAsync();
            return Results.Ok(skills.Select(s => s.ToDto()));
        });

        group.MapGet("/{id:int:min(0)}", async (int id, ISkillRepository repo) =>
        {
            var skill = await repo.GetByIdAsync(id);
            return skill is null ? Results.NotFound() : Results.Ok(skill.ToDto());
        });

        group.MapPut("/{id:int:min(0)}", async (int id, InputSkillDto skill, ISkillRepository repo) =>
        {
            var updatedSkill = await repo.UpdateAsync(skill.ToModel(id: id));
            return updatedSkill is not null ? Results.Ok(updatedSkill.ToDto()) : Results.NotFound();
        }).RequireAuthorization();

        group.MapDelete("/{id:int:min(0)}", async (int id, ISkillRepository repo) =>
            await repo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound()
        ).RequireAuthorization();
    }
}