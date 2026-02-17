# CLITaskManager

A professional task and project management CLI tool built with C# and .NET 8, designed to help developers manage projects and tasks directly from the terminal.

---

## 🛠️ Technologies
- **.NET 8** - Application framework
- **C#** - Programming language
- **PostgreSQL** - Database
- **Entity Framework Core** - ORM (Object Relational Mapper)
- **xUnit** - Unit testing framework

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles, organized into 4 layers:
```
CLITaskManager.CLI            → Presentation Layer (user interaction)
CLITaskManager.Application    → Business Logic Layer (rules & validation)
CLITaskManager.Infrastructure → Data Access Layer (database operations)
CLITaskManager.Domain         → Core Entities (shared foundation)
```

**Dependency Rule:** Outer layers depend on inner layers. Inner layers are unaware of outer layers.

---

## 📋 Features

### Project Management
- Create, list, view, and delete projects

### Task Management
- Create tasks with title, optional description, priority, optional deadline
- Assign tasks to projects
- Update task status (Todo → In Progress → Done)
- Update priority (Low / Medium / High)
- Delete tasks

### Tags
- Create and assign tags to tasks
- Organize and filter tasks by tag

### Filtering & Reporting
- Filter tasks by status, priority, or tag
- View overdue tasks
- View tasks due this week

### Data Management
- Export tasks to JSON
- Import tasks from JSON

---

## 🗄️ Data Model

### Entities & Relationships
```
Project (1) ──────────< Task (Many)
Tag (Many) >────────── TaskTag ──────────< Task (Many)
```

### Project
| Field       | Type     | Required |
|-------------|----------|----------|
| Id          | int      | ✅       |
| Name        | string   | ✅       |
| Description | string   | ❌       |
| CreatedAt   | datetime | ✅       |
| UpdatedAt   | datetime | ✅       |

### Task
| Field       | Type     | Required |
|-------------|----------|----------|
| Id          | int      | ✅       |
| Title       | string   | ✅       |
| Description | string   | ❌       |
| Status      | enum     | ✅       |
| Priority    | enum     | ✅       |
| Deadline    | datetime | ❌       |
| ProjectId   | int (FK) | ✅       |
| CreatedAt   | datetime | ✅       |
| UpdatedAt   | datetime | ✅       |

### Tag
| Field | Type   | Required |
|-------|--------|----------|
| Id    | int    | ✅       |
| Name  | string | ✅       |

### Enums
- **Status:** Todo, InProgress, Done
- **Priority:** Low, Medium, High

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL

### Installation
```bash
# Clone the repository
git clone https://github.com/Soufianesaliki/CLITaskManager.git

# Navigate to project
cd CLITaskManager

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run
```

---

## 🧪 Testing
```bash
dotnet test
```

---

## 📁 Project Structure
```
CLITaskManager/
├── src/
│   ├── CLITaskManager.Domain/
│   ├── CLITaskManager.Application/
│   ├── CLITaskManager.Infrastructure/
│   └── CLITaskManager.CLI/
└── tests/
    └── CLITaskManager.Tests/
```

---

## 👤 Author
[Your Name]
- GitHub: [@Soufianesaliki](https://github.com/Soufianesaliki)
- LinkedIn: [Soufiane Saliki](https://linkedin.com/in/soufianesaliki)

---

## 📌 Status
🚧 Under Active Development