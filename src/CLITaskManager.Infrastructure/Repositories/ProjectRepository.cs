using Microsoft.EntityFrameworkCore;
using CLITaskManager.Domain.Interfaces;
using CLITaskManager.Domain.Entities;
using CLITaskManager.Infrastructure.Data;
using System;

namespace CLITaskManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .FindAsync(id);
    }
    public async Task<Project?> GetByIdWithTasksAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<Project> AddAsync(Project project)
    {
        project.CreatedAt = DateTime.UtcNow;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return project;
    }
    public async System.Threading.Tasks.Task UpdateAsync(Project project)
    {
        project.UpdatedAt = DateTime.UtcNow;

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }
    public async System.Threading.Tasks.Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Projects.AnyAsync(p => p.Id == id);
    }
}