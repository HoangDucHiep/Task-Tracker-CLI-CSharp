using TaskManagerCLI.Models;
using TaskStatus = TaskManagerCLI.Models.TaskStatus;

namespace TaskManagerCLI.Services;

public interface ITaskManager
{
    Task<int> AddTask(string description);
    Task<bool> UpdateTask(int id, string description);
    Task<bool> DeleteTask(int id);
    Task<bool> MarkTaskAsDone(int id);
    Task<bool> MarkTaskAsInProgress(int id);
    Task<List<AppTask>> ListTasks(TaskStatus? filterStatus = null);
}
