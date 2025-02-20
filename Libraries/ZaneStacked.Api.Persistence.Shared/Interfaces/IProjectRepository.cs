using ZaneStacked.Api.Persistence.Shared.Models;

namespace ZaneStacked.Api.Persistence.Shared.Interfaces;

public interface IProjectRepository
{
    Task<Project> AddAsync(Project project, int[] skillIds);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<Project?> UpdateAsync(Project project, int[] skillIds);
    Task<bool> DeleteAsync(int id);
}