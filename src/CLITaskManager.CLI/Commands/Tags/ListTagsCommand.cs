using Spectre.Console;
using Spectre.Console.Cli;
using CLITaskManager.Application.Services;

namespace CLITaskManager.CLI.Commands.Tags;

public class ListTagsCommand : AsyncCommand<ListTagsSettings>
{
    private readonly TagService _tagService;

    public ListTagsCommand(TagService tagService)
    {
        _tagService = tagService;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, ListTagsSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            var tags = await _tagService.GetAllTagsAsync();
            var tagList = tags.ToList();

            if (!tagList.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No tags found.[/]");
                return 0;
            }

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[blue]ID[/]");
            table.AddColumn("[blue]Name[/]");
            table.AddColumn("[blue]Created[/]");

            foreach (var tag in tagList)
            {
                table.AddRow(
                    tag.Id.ToString(),
                    $"[cyan]{tag.Name}[/]",
                    tag.CreatedAt.ToString("yyyy-MM-dd")
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"\n[green]Total:[/] {tagList.Count} tag(s)");

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            return 1;
        }
    }
}