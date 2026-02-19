using Microsoft.EntityFrameworkCore;
using CLITaskManager.Domain.Interfaces;
using CLITaskManager.Infrastructure.Data;
using CLITaskManager.Domain.Enums;

namespace CLITaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetAllAsync()
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Task?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async System.Threading.Tasks.Task<IEnumerable<Domain.Entities.Task>> GetByProjectIdAsync(int projectId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.ProjectId == projectId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetByStatusAsync(Status status)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetByPriorityAsync(Priority priority)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.Priority == priority)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetOverdueTasksAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.Deadline.HasValue && t.Deadline.Value < now && t.Status != Status.Done)
            .OrderBy(t => t.Deadline)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksDueThisWeekAsync()
    {
        var now = DateTime.UtcNow;
        var endOfWeek = now.AddDays(7);
        
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.Deadline.HasValue 
                     && t.Deadline.Value >= now 
                     && t.Deadline.Value <= endOfWeek 
                     && t.Status != Status.Done)
            .OrderBy(t => t.Deadline)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Task> AddAsync(Domain.Entities.Task task)
    {
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;
        
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        
        return task;
    }

    public async System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task task)
    {
        task.UpdatedAt = DateTime.UtcNow;
        
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task DeleteAsync(Domain.Entities.Task task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Tasks.AnyAsync(t => t.Id == id);
    }
}