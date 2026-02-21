using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.Exceptions;

namespace CLITaskManager.CLI.Commands.Projects;

public class DeleteProjectCommand : AsyncCommand<DeleteProjectSettings>
{
    private readonly ProjectService _projectService;

    public DeleteProjectCommand(ProjectService projectService)
    {
        _projectService = projectService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DeleteProjectSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            // Get project first to show what we're deleting
            var project = await _projectService.GetProjectByIdAsync(settings.Id);

            // Confirm deletion
            var confirm = AnsiConsole.Confirm($"Are you sure you want to delete project '{project.Name}'?");
            
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[yellow]Deletion cancelled.[/]");
                return 0;
            }

            await _projectService.DeleteProjectAsync(settings.Id);

            AnsiConsole.MarkupLine($"[green]✓[/] Project '{project.Name}' deleted successfully!");

            return 0;
        }
        catch (NotFoundException ex)
        {
            AnsiConsole.MarkupLine($"[red]Not Found:[/] {ex.Message}");
            return 1;
        }
        catch (BusinessRuleException ex)
        {
            AnsiConsole.MarkupLine($"[red]Business Rule Violation:[/] {ex.Message}");
            return 1;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            return 1;
        }
    }
}