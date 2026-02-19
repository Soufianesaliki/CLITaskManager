using CLITaskManager.Domain.Entities;


namespace CLITaskManager.Application.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<Project?> GetByIdWithTasksAsync(int id);
    Task<Project> AddAsync(Project project);
    System.Threading.Tasks.Task UpdateAsync(Project project);
    System.Threading.Tasks.Task DeleteAsync(Project project);
    Task<bool> ExistsAsync(int id);
}