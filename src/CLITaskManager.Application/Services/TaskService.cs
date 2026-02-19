using CLITaskManager.Application.DTOs;
using CLITaskManager.Application.Exceptions;
using CLITaskManager.Domain.Enums;
using CLITaskManager.Domain.Interfaces;

namespace CLITaskManager.Application.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<Domain.Entities.Task> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
        {
            throw new NotFoundException("Task", id);
        }
        
        return task;
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByProjectIdAsync(int projectId)
    {
        // Verify project exists
        if (!await _projectRepository.ExistsAsync(projectId))
        {
            throw new NotFoundException("Project", projectId);
        }

        return await _taskRepository.GetByProjectIdAsync(projectId);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByStatusAsync(Status status)
    {
        return await _taskRepository.GetByStatusAsync(status);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByPriorityAsync(Priority priority)
    {
        return await _taskRepository.GetByPriorityAsync(priority);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetOverdueTasksAsync()
    {
        return await _taskRepository.GetOverdueTasksAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksDueThisWeekAsync()
    {
        return await _taskRepository.GetTasksDueThisWeekAsync();
    }

    public async Task<Domain.Entities.Task> CreateTaskAsync(CreateTaskDto dto)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ValidationException("Task title cannot be empty.");
        }

        if (dto.Title.Length > 200)
        {
            throw new ValidationException("Task title cannot exceed 200 characters.");
        }

        // Verify project exists
        if (!await _projectRepository.ExistsAsync(dto.ProjectId))
        {
            throw new NotFoundException("Project", dto.ProjectId);
        }

        // Create entity
        var task = new Domain.Entities.Task
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim(),
            Priority = dto.Priority,
            Deadline = dto.Deadline,
            ProjectId = dto.ProjectId,
            Status = Status.Todo  // New tasks always start as Todo
        };

        return await _taskRepository.AddAsync(task);
    }

    public async Task<Domain.Entities.Task> UpdateTaskAsync(UpdateTaskDto dto)
    {
        // Get existing task
        var task = await _taskRepository.GetByIdAsync(dto.Id);
        if (task == null)
        {
            throw new NotFoundException("Task", dto.Id);
        }

        // Update only provided fields
        if (dto.Title != null)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ValidationException("Task title cannot be empty.");
            }

            if (dto.Title.Length > 200)
            {
                throw new ValidationException("Task title cannot exceed 200 characters.");
            }

            task.Title = dto.Title.Trim();
        }

        if (dto.Description != null)
        {
            task.Description = dto.Description.Trim();
        }

        if (dto.Status.HasValue)
        {
            task.Status = dto.Status.Value;
        }

        if (dto.Priority.HasValue)
        {
            task.Priority = dto.Priority.Value;
        }

        if (dto.Deadline != null)
        {
            task.Deadline = dto.Deadline;
        }

        await _taskRepository.UpdateAsync(task);
        
        return task;
    }

    public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
        {
            throw new NotFoundException("Task", id);
        }

        await _taskRepository.DeleteAsync(task);
    }

    public async System.Threading.Tasks.Task UpdateTaskStatusAsync(int id, Status status)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
        {
            throw new NotFoundException("Task", id);
        }

        task.Status = status;
        await _taskRepository.UpdateAsync(task);
    }
}