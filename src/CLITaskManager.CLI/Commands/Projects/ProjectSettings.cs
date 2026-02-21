using System.ComponentModel;
using Spectre.Console.Cli;

namespace CLITaskManager.CLI.Commands.Projects;

public class ListProjectsSettings : CommandSettings
{
}

public class CreateProjectSettings : CommandSettings
{
    [CommandArgument(0, "<name>")]
    [Description("The name of the project")]
    public string Name { get; set; } = string.Empty;

    [CommandOption("-d|--description")]
    [Description("Optional project description")]
    public string? Description { get; set; }
}

public class DeleteProjectSettings : CommandSettings
{
    [CommandArgument(0, "<id>")]
    [Description("The ID of the project to delete")]
    public int Id { get; set; }
}

public class ViewProjectSettings : CommandSettings
{
    [CommandArgument(0, "<id>")]
    [Description("The ID of the project to view")]
    public int Id { get; set; }
}