using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.DTOs;
using CLITaskManager.Application.Exceptions;

namespace CLITaskManager.CLI.Commands.Tasks;

public class CreateTaskCommand : AsyncCommand<CreateTaskSettings>
{
    private readonly TaskService _taskService;

    public CreateTaskCommand(TaskService taskService)
    {
        _taskService = taskService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CreateTaskSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var dto = new CreateTaskDto
            {
                Title = settings.Title,
                Description = settings.Description,
                Priority = settings.Priority,
                Deadline = settings.Deadline,
                ProjectId = settings.ProjectId
            };

            var task = await _taskService.CreateTaskAsync(dto);

            AnsiConsole.MarkupLine("[green]✓[/] Task created successfully!");
            AnsiConsole.MarkupLine($"  [blue]ID:[/] {task.Id}");
            AnsiConsole.MarkupLine($"  [blue]Title:[/] {task.Title}");
            AnsiConsole.MarkupLine($"  [blue]Status:[/] {task.Status}");
            AnsiConsole.MarkupLine($"  [blue]Priority:[/] {task.Priority}");
            
            if (task.Deadline.HasValue)
            {
                AnsiConsole.MarkupLine($"  [blue]Deadline:[/] {task.Deadline:yyyy-MM-dd}");
            }

            return 0;
        }
        catch (ValidationException ex)
        {
            AnsiConsole.MarkupLine($"[red]Validation Error:[/] {ex.Message}");
            return 1;
        }
        catch (NotFoundException ex)
        {
            AnsiConsole.MarkupLine($"[red]Not Found:[/] {ex.Message}");
            return 1;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            return 1;
        }
    }
}