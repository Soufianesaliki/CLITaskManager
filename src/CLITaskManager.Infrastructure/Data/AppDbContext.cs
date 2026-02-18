using Microsoft.EntityFrameworkCore;

namespace CLITaskManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<CLITaskManager.Domain.Entities.Project> Projects => Set<CLITaskManager.Domain.Entities.Project>();
    public DbSet<CLITaskManager.Domain.Entities.Task> Tasks => Set<CLITaskManager.Domain.Entities.Task>();
    public DbSet<CLITaskManager.Domain.Entities.Tag> Tags => Set<CLITaskManager.Domain.Entities.Tag>();
    public DbSet<CLITaskManager.Domain.Entities.TaskTag> TaskTags => Set<CLITaskManager.Domain.Entities.TaskTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}