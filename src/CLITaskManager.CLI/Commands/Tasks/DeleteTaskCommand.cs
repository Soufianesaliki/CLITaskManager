using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.Exceptions;

namespace CLITaskManager.CLI.Commands.Tasks;

public class DeleteTaskCommand : AsyncCommand<DeleteTaskSettings>
{
    private readonly TaskService _taskService;

    public DeleteTaskCommand(TaskService taskService)
    {
        _taskService = taskService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DeleteTaskSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(settings.Id);

            var confirm = AnsiConsole.Confirm($"Are you sure you want to delete task '{task.Title}'?");
            
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[yellow]Deletion cancelled.[/]");
                return 0;
            }

            await _taskService.DeleteTaskAsync(settings.Id);

            AnsiConsole.MarkupLine($"[green]✓[/] Task '{task.Title}' deleted successfully!");

            return 0;
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