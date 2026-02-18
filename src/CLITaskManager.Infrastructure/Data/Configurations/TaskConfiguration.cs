using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CLITaskManager.Domain.Enums;

namespace CLITaskManager.Infrastructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<CLITaskManager.Domain.Entities.Task>
{
    public void Configure(EntityTypeBuilder<CLITaskManager.Domain.Entities.Task> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(t => t.Description)
            .HasMaxLength(1000);
            
        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(t => t.Deadline);
            
        builder.Property(t => t.CreatedAt)
            .IsRequired();
            
        builder.Property(t => t.UpdatedAt)
            .IsRequired();
        
        builder.HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);
    }
}