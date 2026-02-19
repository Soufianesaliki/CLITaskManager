using CLITaskManager.Domain.Enums;

namespace CLITaskManager.Application.DTOs;

public class UpdateTaskDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Status? Status { get; set; }
    public Priority? Priority { get; set; }
    public DateTime? Deadline { get; set; }
}