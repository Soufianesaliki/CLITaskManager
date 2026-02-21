using System.ComponentModel;
using Spectre.Console.Cli;
using CLITaskManager.Domain.Enums;

namespace CLITaskManager.CLI.Commands.Tasks;

public class ListTasksSettings : CommandSettings
{
    [CommandOption("-p|--project <PROJECT_ID>")]
    [Description("Filter by project ID")]
    public int? ProjectId { get; set; }

    [CommandOption("-s|--status <STATUS>")]
    [Description("Filter by status (Todo, InProgress, Done)")]
    public Status? Status { get; set; }

    [CommandOption("--priority <PRIORITY>")]
    [Description("Filter by priority (Low, Medium, High)")]
    public Priority? Priority { get; set; }

    [CommandOption("--overdue")]
    [Description("Show only overdue tasks")]
    public bool Overdue { get; set; }

    [CommandOption("--due-this-week")]
    [Description("Show tasks due this week")]
    public bool DueThisWeek { get; set; }
}

public class CreateTaskSettings : CommandSettings
{
    [CommandArgument(0, "<title>")]
    [Description("The title of the task")]
    public string Title { get; set; } = string.Empty;

    [CommandOption("-p|--project <PROJECT_ID>")]
    [Description("Project ID (required)")]
    public int ProjectId { get; set; }

    [CommandOption("-d|--description <DESCRIPTION>")]
    [Description("Task description")]
    public string? Description { get; set; }

    [CommandOption("--priority <PRIORITY>")]
    [Description("Priority: Low, Medium, High (default: Medium)")]
    [DefaultValue(Priority.Medium)]
    public Priority Priority { get; set; }

    [CommandOption("--deadline <DATE>")]
    [Description("Deadline (format: yyyy-MM-dd)")]
    public DateTime? Deadline { get; set; }
}

public class UpdateTaskSettings : CommandSettings
{
    [CommandArgument(0, "<id>")]
    [Description("Task ID to update")]
    public int Id { get; set; }

    [CommandOption("-t|--title <TITLE>")]
    [Description("New title")]
    public string? Title { get; set; }

    [CommandOption("-d|--description <DESCRIPTION>")]
    [Description("New description")]
    public string? Description { get; set; }

    [CommandOption("-s|--status <STATUS>")]
    [Description("New status (Todo, InProgress, Done)")]
    public Status? Status { get; set; }

    [CommandOption("--priority <PRIORITY>")]
    [Description("New priority (Low, Medium, High)")]
    public Priority? Priority { get; set; }

    [CommandOption("--deadline <DATE>")]
    [Description("New deadline (format: yyyy-MM-dd)")]
    public DateTime? Deadline { get; set; }
}

public class DeleteTaskSettings : CommandSettings
{
    [CommandArgument(0, "<id>")]
    [Description("Task ID to delete")]
    public int Id { get; set; }
}

public class ViewTaskSettings : CommandSettings
{
    [CommandArgument(0, "<id>")]
    [Description("Task ID to view")]
    public int Id { get; set; }
}

public class CompleteTaskSettings : CommandSettings
{
    [CommandArgument(0, "<id>")]
    [Description("Task ID to mark as complete")]
    public int Id { get; set; }
}