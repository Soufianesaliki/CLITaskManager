using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.DTOs;
using CLITaskManager.Application.Exceptions;

namespace CLITaskManager.CLI.Commands.Tasks;

public class UpdateTaskCommand : AsyncCommand<UpdateTaskSettings>
{
    private readonly TaskService _taskService;

    public UpdateTaskCommand(TaskService taskService)
    {
        _taskService = taskService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, UpdateTaskSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var dto = new UpdateTaskDto
            {
                Id = settings.Id,
                Title = settings.Title,
                Description = settings.Description,
                Status = settings.Status,
                Priority = settings.Priority,
                Deadline = settings.Deadline
            };

            var task = await _taskService.UpdateTaskAsync(dto);

            AnsiConsole.MarkupLine("[green]✓[/] Task updated successfully!");
            AnsiConsole.MarkupLine($"  [blue]ID:[/] {task.Id}");
            AnsiConsole.MarkupLine($"  [blue]Title:[/] {task.Title}");
            AnsiConsole.MarkupLine($"  [blue]Status:[/] {task.Status}");
            AnsiConsole.MarkupLine($"  [blue]Priority:[/] {task.Priority}");

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