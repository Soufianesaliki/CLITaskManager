using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;

namespace CLITaskManager.CLI.Commands.Projects;

public class ListProjectsCommand : AsyncCommand<ListProjectsSettings>
{
    private readonly ProjectService _projectService;

    public ListProjectsCommand(ProjectService projectService)
    {
        _projectService = projectService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, ListProjectsSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var projectList = projects.ToList();

            if (!projectList.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No projects found.[/]");
                return 0;
            }

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[blue]ID[/]");
            table.AddColumn("[blue]Name[/]");
            table.AddColumn("[blue]Description[/]");
            table.AddColumn("[blue]Created[/]");

            foreach (var project in projectList)
            {
                table.AddRow(
                    project.Id.ToString(),
                    project.Name,
                    project.Description ?? "[grey]N/A[/]",
                    project.CreatedAt.ToString("yyyy-MM-dd")
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"\n[green]Total:[/] {projectList.Count} project(s)");

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            return 1;
        }
    }
}