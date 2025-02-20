using ZaneStacked.Api.Persistence.EFCore.Data;
using ZaneStacked.Api.Persistence.Shared.Interfaces;
using ZaneStacked.Api.Persistence.Shared.Models;

namespace ZaneStacked.Api.Persistence.EFCore.Repositories;

using Microsoft.EntityFrameworkCore;

public class ProjectRepository : IProjectRepository
{
    private readonly ZaneStackedDbContext _context;

    public ProjectRepository(ZaneStackedDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.Skills)
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Skills)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> AddAsync(Project project, int[] skillIds)
    {
        var skills = await _context.Skills
            .Where(s => skillIds.Contains(s.Id))
            .ToListAsync();

        project.Skills = skills;
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return project;
    }
    
    public async Task<Project?> UpdateAsync(Project project, int[] skillIds)
    {
        var existingProject = await _context.Projects
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == project.Id);

        if (existingProject is null)
            return null;

        // Update scalar properties
        _context.Entry(existingProject).CurrentValues.SetValues(project);

        // Convert Skill IDs from param into skill entities
        var existingSkillIds = existingProject.Skills.Select(s => s.Id).ToHashSet();
        var newSkillIds = skillIds.ToHashSet();

        // Remove skills that are no longer associated
        existingProject.Skills.RemoveAll(s => !newSkillIds.Contains(s.Id));

        // Add new skills that are not already present
        var skillsToAdd = await _context.Skills
            .Where(s => newSkillIds.Contains(s.Id) && !existingSkillIds.Contains(s.Id))
            .ToListAsync();

        existingProject.Skills.AddRange(skillsToAdd);
        await _context.SaveChangesAsync();
        
        return existingProject;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
            return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        
        return true;
    }
}