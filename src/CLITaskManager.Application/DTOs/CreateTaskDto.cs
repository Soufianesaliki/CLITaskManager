using CLITaskManager.Domain.Enums;

namespace CLITaskManager.Application.DTOs;

public class CreateTaskDots
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? Deadline { get; set; }
    public int ProjectId { get; set; }
}