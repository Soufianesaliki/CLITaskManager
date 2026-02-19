namespace CLITaskManager.Application.DTOs;

public class UpdateProjectDots
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}