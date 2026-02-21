using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.Exceptions;
using CLITaskManager.Domain.Enums;

namespace CLITaskManager.CLI.Commands.Tasks;

public class CompleteTaskCommand : AsyncCommand<CompleteTaskSettings>
{
    private readonly TaskService _taskService;

    public CompleteTaskCommand(TaskService taskService)
    {
        _taskService = taskService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CompleteTaskSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            await _taskService.UpdateTaskStatusAsync(settings.Id, Domain.Enums.Status.Done);

            AnsiConsole.MarkupLine("[green]✓[/] Task marked as complete!");

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