using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CLITaskManager.Infrastructure.Data;
using CLITaskManager.Domain.Interfaces;
using CLITaskManager.Infrastructure.Repositories;
using CLITaskManager.Application.Services;
using Spectre.Console;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .Build();

// Setup Dependency Injection
var services = new ServiceCollection();

// Add Logging
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddConfiguration(configuration.GetSection("Logging"));
});

// Add DbContext
var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register Repositories
services.AddScoped<IProjectRepository, ProjectRepository>();
services.AddScoped<ITaskRepository, TaskRepository>();
services.AddScoped<ITagRepository, TagRepository>();

// Register Services
services.AddScoped<ProjectRepository>();
services.AddScoped<TaskRepository>();
services.AddScoped<TagRepository>();

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Welcome message
AnsiConsole.Write(
    new FigletText("CLI Task Manager")
        .Centered()
        .Color(Color.Blue));

AnsiConsole.MarkupLine("[grey]Version 1.0.0[/]");
AnsiConsole.WriteLine();

try
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var canConnect = await dbContext.Database.CanConnectAsync();

    if (canConnect)
    {
        AnsiConsole.MarkupLine("[green]✓[/] Connected to database successfully!");
    }
    else
    {
        AnsiConsole.MarkupLine("[red]✗[/] Failed to connect to database.");
    }
}
catch (Exception ex)
{
    AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
}
