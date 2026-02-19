using CLITaskManager.Application.DTOs;
using CLITaskManager.Application.Exceptions;
using CLITaskManager.Domain.Entities;
using CLITaskManager.Domain.Interfaces;

namespace CLITaskManager.Application.Services;

public class ProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskRepository _taskRepository;

    public ProjectService(IProjectRepository projectRepository, ITaskRepository taskRepository)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _projectRepository.GetAllAsync();
    }

    public async Task<Project> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        
        if (project == null)
        {
            throw new NotFoundException("Project", id);
        }
        
        return project;
    }

    public async Task<Project> GetProjectWithTasksAsync(int id)
    {
        var project = await _projectRepository.GetByIdWithTasksAsync(id);
        
        if (project == null)
        {
            throw new NotFoundException("Project", id);
        }
        
        return project;
    }

    public async Task<Project> CreateProjectAsync(CreateProjectDto dto)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ValidationException("Project name cannot be empty.");
        }

        if (dto.Name.Length > 100)
        {
            throw new ValidationException("Project name cannot exceed 100 characters.");
        }

        // Create entity
        var project = new Project
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim()
        };

        return await _projectRepository.AddAsync(project);
    }

    public async Task<Project> UpdateProjectAsync(UpdateProjectDto dto)
    {
        // Check if exists
        var project = await _projectRepository.GetByIdAsync(dto.Id);
        if (project == null)
        {
            throw new NotFoundException("Project", dto.Id);
        }

        // Validation
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ValidationException("Project name cannot be empty.");
        }

        if (dto.Name.Length > 100)
        {
            throw new ValidationException("Project name cannot exceed 100 characters.");
        }

        // Update properties
        project.Name = dto.Name.Trim();
        project.Description = dto.Description?.Trim();

        await _projectRepository.UpdateAsync(project);
        
        return project;
    }

    public async System.Threading.Tasks.Task DeleteProjectAsync(int id)
    {
        var project = await _projectRepository.GetByIdWithTasksAsync(id);
        
        if (project == null)
        {
            throw new NotFoundException("Project", id);
        }

        // Business rule: Can't delete project with active tasks
        var activeTasks = project.Tasks.Where(t => t.Status != Domain.Enums.Status.Done).ToList();
        if (activeTasks.Any())
        {
            throw new BusinessRuleException(
                $"Cannot delete project with {activeTasks.Count} active task(s). " +
                "Complete or delete all tasks first.");
        }

        await _projectRepository.DeleteAsync(project);
    }
}