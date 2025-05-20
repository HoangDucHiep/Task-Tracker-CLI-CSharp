using Microsoft.Extensions.DependencyInjection;
using TaskManagerCLI.Models;
using TaskManagerCLI.Services;
using TaskStatus = TaskManagerCLI.Models.TaskStatus;

var serviceProvider = new ServiceCollection()
    .AddSingleton<ITaskManager, TaskManager>()
    .AddSingleton<IFileService, FileService>()
    .BuildServiceProvider();

var taskManager = serviceProvider.GetService<ITaskManager>();
if (taskManager == null)
{
    Console.WriteLine("Failed to create TaskManager instance.");
    return;
}

if (args.Length == 0)
{
    Console.WriteLine("Please provide a command.");
    return;
}

string command = args[0].ToLower();

switch (command)
{
    case "add":
        if (args.Length < 2)
        {
            Console.WriteLine("Please provide a task description.");
            return;
        }

        string description = args[1];
        int taskId = await taskManager.AddTask(description);
        Console.WriteLine($"Task added with ID: {taskId}");
        break;
    case "list":
        List<AppTask> tasks;
        if (args.Length == 1)
        {
            tasks = await taskManager.ListTasks();
        }
        else if (
            args.Length == 2
            && Enum.TryParse<TaskManagerCLI.Models.TaskStatus>(
                args[1],
                true,
                out TaskManagerCLI.Models.TaskStatus status
            )
        )
        {
            tasks = await taskManager.ListTasks(status);
        }
        else
        {
            Console.WriteLine("Invalid command. Use 'list' or 'list <status>'.");
            return;
        }

        PrintTasks(tasks);
        break;
    case "delete":
        if (args.Length < 2 || !int.TryParse(args[1], out int id))
        {
            Console.WriteLine("Please provide a valid task ID to delete.");
            return;
        }

        bool deleted = await taskManager.DeleteTask(id);
        Console.WriteLine(deleted ? "Task deleted." : "Task not found.");
        break;
    case "update":
        if (args.Length < 3 || !int.TryParse(args[1], out int updateId))
        {
            Console.WriteLine("Please provide a valid task ID and new description.");
            return;
        }

        string newDescription = args[2];
        bool updated = await taskManager.UpdateTask(updateId, newDescription);
        Console.WriteLine(updated ? "Task updated." : "Task not found.");
        break;
    case "mark-in-progress":
        if (args.Length < 2 || !int.TryParse(args[1], out int inProgressId))
        {
            Console.WriteLine("Please provide a valid task ID to mark as in progress.");
            return;
        }

        bool markedInProgress = await taskManager.UpdateTask(inProgressId, TaskStatus.in_progress);
        Console.WriteLine(markedInProgress ? "Task marked as in progress." : "Task not found.");
        break;
    case "mark-done":
        if (args.Length < 2 || !int.TryParse(args[1], out int doneId))
        {
            Console.WriteLine("Please provide a valid task ID to mark as done.");
            return;
        }

        bool markedDone = await taskManager.UpdateTask(doneId, TaskStatus.done);
        Console.WriteLine(markedDone ? "Task marked as done." : "Task not found.");
        break;
}

static void PrintTasks(List<AppTask> tasks)
{
    if (tasks.Count == 0)
    {
        Console.WriteLine("No tasks found.");
        return;
    }
    Console.WriteLine("Tasks:");
    Console.WriteLine("====================================");
    Console.WriteLine();
    foreach (var task in tasks)
    {
        ConsoleColor color = task.Status switch
        {
            TaskStatus.todo => ConsoleColor.Yellow,
            TaskStatus.in_progress => ConsoleColor.Cyan,
            TaskStatus.done => ConsoleColor.Green,
            _ => ConsoleColor.White,
        };

        Console.WriteLine($"ID: {task.Id}");
        Console.WriteLine($"Description: {task.Description}");
        Console.Write($"Status: ");
        Console.ForegroundColor = color;
        Console.WriteLine(task.Status);
        Console.ResetColor();
        Console.WriteLine($"Created At: {task.CreatedAt}");
        Console.WriteLine($"Updated At: {task.UpdatedAt}");
        Console.WriteLine("------------------------------------");
        Console.WriteLine();
    }
}
