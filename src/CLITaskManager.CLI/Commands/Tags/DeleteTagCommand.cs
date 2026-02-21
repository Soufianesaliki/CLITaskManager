using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;
using CLITaskManager.Application.Exceptions;

namespace CLITaskManager.CLI.Commands.Tags;

public class DeleteTagCommand : AsyncCommand<DeleteTagSettings>
{
    private readonly TagService _tagService;

    public DeleteTagCommand(TagService tagService)
    {
        _tagService = tagService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DeleteTagSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var tag = await _tagService.GetTagByIdAsync(settings.Id);

            var confirm = AnsiConsole.Confirm($"Are you sure you want to delete tag '{tag.Name}'?");
            
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[yellow]Deletion cancelled.[/]");
                return 0;
            }

            await _tagService.DeleteTagAsync(settings.Id);

            AnsiConsole.MarkupLine($"[green]✓[/] Tag '{tag.Name}' deleted successfully!");

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