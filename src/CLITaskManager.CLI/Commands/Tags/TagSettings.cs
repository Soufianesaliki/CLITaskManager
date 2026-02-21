using System.ComponentModel;
using Spectre.Console.Cli;

namespace CLITaskManager.CLI.Commands.Tags;

public class ListTagsSettings : CommandSettings
{
}

public class CreateTagSettings : CommandSettings
{
    [CommandArgument(0, "<name>")]
    [Description("The name of the tag")]
    public string Name { get; set; } = string.Empty;
}

public class DeleteTagSettings : CommandSettings
{
    [CommandArgument(0, "<id>")]
    [Description("The ID of the tag to delete")]
    public int Id { get; set; }
}