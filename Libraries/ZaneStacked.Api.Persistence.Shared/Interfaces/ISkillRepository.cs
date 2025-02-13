using ZaneStacked.Api.Persistence.Shared.Models;

namespace ZaneStacked.Api.Persistence.Shared.Interfaces;

public interface ISkillRepository
{
    Task<Skill> AddAsync(Skill skill);
    Task<IEnumerable<Skill>> GetAllAsync();
    Task<Skill?> GetByIdAsync(int id);
    Task<Skill?> UpdateAsync(Skill skill);
    Task<bool> DeleteAsync(int id);
}