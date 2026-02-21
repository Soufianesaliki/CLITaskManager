using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.Exceptions;

namespace CLITaskManager.CLI.Commands.Projects;

public class ViewProjectCommand : AsyncCommand<ViewProjectSettings>
{
    private readonly ProjectService _projectService;

    public ViewProjectCommand(ProjectService projectService)
    {
        _projectService = projectService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, ViewProjectSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectService.GetProjectWithTasksAsync(settings.Id);

            var panel = new Panel($"[bold]{project.Name}[/]")
            {
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Blue)
            };

            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine($"[blue]ID:[/] {project.Id}");
            AnsiConsole.MarkupLine($"[blue]Description:[/] {project.Description ?? "[grey]N/A[/]"}");
            AnsiConsole.MarkupLine($"[blue]Created:[/] {project.CreatedAt:yyyy-MM-dd HH:mm}");
            AnsiConsole.MarkupLine($"[blue]Updated:[/] {project.UpdatedAt:yyyy-MM-dd HH:mm}");
            AnsiConsole.WriteLine();

            if (project.Tasks.Any())
            {
                var table = new Table();
                table.Border(TableBorder.Rounded);
                table.Title = new TableTitle($"[yellow]Tasks ({project.Tasks.Count})[/]");
                table.AddColumn("[blue]ID[/]");
                table.AddColumn("[blue]Title[/]");
                table.AddColumn("[blue]Status[/]");
                table.AddColumn("[blue]Priority[/]");

                foreach (var task in project.Tasks.OrderByDescending(t => t.CreatedAt))
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
                        $"[{statusColor}]{task.Status}[/]",
                        $"[{priorityColor}]{task.Priority}[/]"
                    );
                }

                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]No tasks in this project.[/]");
            }

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