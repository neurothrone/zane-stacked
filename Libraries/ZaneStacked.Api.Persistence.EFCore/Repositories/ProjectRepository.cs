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
        return await _context.Projects.Include(p => p.Skills).ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> AddAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project?> UpdateAsync(Project project)
    {
        var existingProject = await _context.Projects.FindAsync(project.Id);
        if (existingProject is null)
            return null;

        _context.Entry(existingProject).CurrentValues.SetValues(project);
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