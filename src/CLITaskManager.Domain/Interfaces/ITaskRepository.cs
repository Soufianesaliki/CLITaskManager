using CLITaskManager.Domain.Entities;
using CLITaskManager.Domain.Enums;

namespace CLITaskManager.Domain.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<Domain.Entities.Task>> GetAllAsync();
    Task<Domain.Entities.Task?> GetByIdAsync(int id);
    Task<IEnumerable<Domain.Entities.Task>> GetByProjectIdAsync(int projectId);
    Task<IEnumerable<Domain.Entities.Task>> GetByStatusAsync(Status status);
    Task<IEnumerable<Domain.Entities.Task>> GetByPriorityAsync(Priority priority);
    Task<IEnumerable<Domain.Entities.Task>> GetOverdueTasksAsync();
    Task<IEnumerable<Domain.Entities.Task>> GetTasksDueThisWeekAsync();
    Task<Domain.Entities.Task> AddAsync(Domain.Entities.Task task);
    System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task task);
    System.Threading.Tasks.Task DeleteAsync(Domain.Entities.Task task);
    Task<bool> ExistsAsync(int id);
}