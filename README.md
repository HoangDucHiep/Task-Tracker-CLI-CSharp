
# Task Tracker

---

> This is my work for [Task Tracker](https://roadmap.sh/projects/task-tracker) project from roadmap.sh with C#.

## How to run

Clone the repository and run the following command:

```powershell
git clone https://github.com/HoangDucHiep/Task-Tracker-CLI-CSharp.git
cd .\TaskTrackerCLI\
```

### Usage:

```c#
# Get list of all commands
dotnet run help

# Adding a new task
dotnet run add "Buy groceries"
# Output: Task added successfully (ID: 1)

# Updating and deleting tasks
dotnet run update 1 "Buy groceries and cook dinner"
dotnet run delete 1

# Marking a task as in progress or done
dotnet run mark-in-progress 1
dotnet run mark-done 1

# Listing all tasks
dotnet run list

# Listing tasks by status
dotnet run list done
dotnet run list todo
dotnet run list in_progress
```

> ***Thank guys** *
