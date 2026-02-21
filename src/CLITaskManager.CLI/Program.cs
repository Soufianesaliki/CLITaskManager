using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Spectre.Console.Cli;
using CLITaskManager.Infrastructure.Data;
using CLITaskManager.Domain.Interfaces;
using CLITaskManager.Infrastructure.Repositories;
using CLITaskManager.Application.Services;
using CLITaskManager.CLI.Commands.Projects;
using CLITaskManager.CLI;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .Build();

// Setup Dependency Injection
var services = new ServiceCollection();

// Add logging
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddConfiguration(configuration.GetSection("Logging"));
});

// Add DbContext
var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register Repositories
services.AddScoped<IProjectRepository, ProjectRepository>();
services.AddScoped<ITaskRepository, TaskRepository>();
services.AddScoped<ITagRepository, TagRepository>();

// Register Services
services.AddScoped<ProjectService>();
services.AddScoped<TaskService>();
services.AddScoped<TagService>();

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Create type registrar for Spectre.Console.Cli
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("task-manager");
    
    // Project commands
    config.AddBranch("project", project =>
    {
        project.SetDescription("Manage projects");
        
        project.AddCommand<ListProjectsCommand>("list")
            .WithDescription("List all projects");
            
        project.AddCommand<CreateProjectCommand>("create")
            .WithDescription("Create a new project")
            .WithExample(new[] { "project", "create", "Website Redesign" })
            .WithExample(new[] { "project", "create", "Mobile App", "--description", "iOS and Android app" });
            
        project.AddCommand<ViewProjectCommand>("view")
            .WithDescription("View project details")
            .WithExample(new[] { "project", "view", "1" });
            
        project.AddCommand<DeleteProjectCommand>("delete")
            .WithDescription("Delete a project")
            .WithExample(new[] { "project", "delete", "1" });
    });
});

return await app.RunAsync(args);