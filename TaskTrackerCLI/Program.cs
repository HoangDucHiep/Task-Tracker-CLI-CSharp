using Microsoft.Extensions.DependencyInjection;
using TaskManagerCLI.Models;
using TaskManagerCLI.Services;

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

/* taskManager.AddTask("This is the Seconds task").Wait();
Console.WriteLine("Task added successfully."); */


var tasks = await taskManager.ListTasks();

if (tasks != null && tasks.Count > 0)
{
    Console.WriteLine("Tasks:");
    foreach (var task in tasks)
    {
        Console.WriteLine($"- [{task.Status}] {task.Description} (ID: {task.Id})");
    }
}
else
{
    Console.WriteLine("No tasks found.");
}



/* bool result = await taskManager.UpdateTask(2, "This is the second task");
if (result)
{
    Console.WriteLine("Task updated successfully.");
}
else
{
    Console.WriteLine("Failed to update task.");
} */