using CLITaskManager.Domain.Enums;
namespace CLITaskManager.Domain.Entities;

public class Task : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    Status Status { get; set; } = Status.Todo;
    Priority Priority { get; set; } = Priority.Medium;
    public DateTime? Deadline { get; set; }

    // Foreign key
    public int ProjectId { get; set; }

    // Navigation properties
    public Project Project { get; set; } = null!;
    public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();

}