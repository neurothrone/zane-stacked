using ZaneStacked.Api.Persistence.EFCore.Data;
using ZaneStacked.Api.Persistence.Shared.Interfaces;
using ZaneStacked.Api.Persistence.Shared.Models;

namespace ZaneStacked.Api.Persistence.EFCore.Repositories;

using Microsoft.EntityFrameworkCore;

public class SkillRepository : ISkillRepository
{
    private readonly ZaneStackedDbContext _context;

    public SkillRepository(ZaneStackedDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Skill>> GetAllAsync()
    {
        return await _context.Skills.Include(s => s.Projects).ToListAsync();
    }

    public async Task<Skill?> GetByIdAsync(int id)
    {
        return await _context.Skills.Include(s => s.Projects).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Skill> AddAsync(Skill skill)
    {
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();
        return skill;
    }

    public async Task<Skill?> UpdateAsync(Skill skill)
    {
        var existingSkill = await _context.Skills.FindAsync(skill.Id);
        if (existingSkill is null) 
            return null;

        _context.Entry(existingSkill).CurrentValues.SetValues(skill);
        await _context.SaveChangesAsync();
        return existingSkill;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill is null) 
            return false;

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();
        return true;
    }
}