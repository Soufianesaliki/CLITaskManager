using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.Exceptions;

namespace CLITaskManager.CLI.Commands.Tags;

public class CreateTagCommand : AsyncCommand<CreateTagSettings>
{
    private readonly TagService _tagService;

    public CreateTagCommand(TagService tagService)
    {
        _tagService = tagService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CreateTagSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var tag = await _tagService.CreateTagAsync(settings.Name);

            AnsiConsole.MarkupLine("[green]✓[/] Tag created successfully!");
            AnsiConsole.MarkupLine($"  [blue]ID:[/] {tag.Id}");
            AnsiConsole.MarkupLine($"  [blue]Name:[/] {tag.Name}");

            return 0;
        }
        catch (ValidationException ex)
        {
            AnsiConsole.MarkupLine($"[red]Validation Error:[/] {ex.Message}");
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