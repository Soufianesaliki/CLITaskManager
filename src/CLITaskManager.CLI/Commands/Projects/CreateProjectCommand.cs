using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.DTOs;
using CLITaskManager.Application.Exceptions;

namespace CLITaskManager.CLI.Commands.Projects;

public class CreateProjectCommand : AsyncCommand<CreateProjectSettings>
{
    private readonly ProjectService _projectService;

    public CreateProjectCommand(ProjectService projectService)
    {
        _projectService = projectService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CreateProjectSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var dto = new CreateProjectDto
            {
                Name = settings.Name,
                Description = settings.Description
            };

            var project = await _projectService.CreateProjectAsync(dto);

            AnsiConsole.MarkupLine("[green]✓[/] Project created successfully!");
            AnsiConsole.MarkupLine($"  [blue]ID:[/] {project.Id}");
            AnsiConsole.MarkupLine($"  [blue]Name:[/] {project.Name}");
            
            if (!string.IsNullOrEmpty(project.Description))
            {
                AnsiConsole.MarkupLine($"  [blue]Description:[/] {project.Description}");
            }

            return 0;
        }
        catch (ValidationException ex)
        {
            AnsiConsole.MarkupLine($"[red]Validation Error:[/] {ex.Message}");
            return 1;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            return 1;
        }
    }
}