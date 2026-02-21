using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;

namespace CLITaskManager.CLI.Commands.Tasks;

public class ListTasksCommand : AsyncCommand<ListTasksSettings>
{
    private readonly TaskService _taskService;

    public ListTasksCommand(TaskService taskService)
    {
        _taskService = taskService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, ListTasksSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Entities.Task> tasks;

            // Apply filters based on settings
            if (settings.Overdue)
            {
                tasks = await _taskService.GetOverdueTasksAsync();
            }
            else if (settings.DueThisWeek)
            {
                tasks = await _taskService.GetTasksDueThisWeekAsync();
            }
            else if (settings.ProjectId.HasValue)
            {
                tasks = await _taskService.GetTasksByProjectIdAsync(settings.ProjectId.Value);
            }
            else if (settings.Status.HasValue)
            {
                tasks = await _taskService.GetTasksByStatusAsync(settings.Status.Value);
            }
            else if (settings.Priority.HasValue)
            {
                tasks = await _taskService.GetTasksByPriorityAsync(settings.Priority.Value);
            }
            else
            {
                tasks = await _taskService.GetAllTasksAsync();
            }

            var taskList = tasks.ToList();

            if (!taskList.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No tasks found.[/]");
                return 0;
            }

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[blue]ID[/]");
            table.AddColumn("[blue]Title[/]");
            table.AddColumn("[blue]Project[/]");
            table.AddColumn("[blue]Status[/]");
            table.AddColumn("[blue]Priority[/]");
            table.AddColumn("[blue]Deadline[/]");

            foreach (var task in taskList)
            {
                var statusColor = task.Status switch
                {
                    Domain.Enums.Status.Todo => "yellow",
                    Domain.Enums.Status.InProgress => "blue",
                    Domain.Enums.Status.Done => "green",
                    _ => "white"
                };

                var priorityColor = task.Priority switch
                {
                    Domain.Enums.Priority.High => "red",
                    Domain.Enums.Priority.Medium => "yellow",
                    Domain.Enums.Priority.Low => "grey",
                    _ => "white"
                };

                table.AddRow(
                    task.Id.ToString(),
                    task.Title,
                    task.Project?.Name ?? "[grey]N/A[/]",
                    $"[{statusColor}]{task.Status}[/]",
                    $"[{priorityColor}]{task.Priority}[/]",
                    task.Deadline?.ToString("yyyy-MM-dd") ?? "[grey]N/A[/]"
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"\n[green]Total:[/] {taskList.Count} task(s)");

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            return 1;
        }
    }
}