using System.Text.Json;
using TaskManagerCLI.Models;
using TaskManagerCLI.Utils;

namespace TaskManagerCLI.Services;

public class TaskManager : ITaskManager
{
    private readonly IFileService _fileService;

    public TaskManager(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<int> AddTask(string description)
    {
        var tasks = await LoadTasksAsync();

        int newId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;

        AppTask newTask = new AppTask(newId, description);
        tasks.Add(newTask);

        await SaveTasksAsync(tasks);
        return newId;
    }

    public async Task<List<AppTask>> ListTasks(Models.TaskStatus? filterStatus = null)
    {
        var tasks = await LoadTasksAsync();

        if (filterStatus.HasValue)
        {
            tasks = tasks.Where(t => t.Status == filterStatus.Value).ToList();
        }

        return tasks;
    }

    public async Task<bool> DeleteTask(int id)
    {
        var tasks = await LoadTasksAsync();

        AppTask? taskToDelete = tasks.FirstOrDefault(t => t.Id == id);

        if (taskToDelete == null)
        {
            Console.WriteLine($"Task with ID {id} not found.");
            return false;
        }

        tasks.Remove(taskToDelete);
        await SaveTasksAsync(tasks);
        return true;
    }

    public async Task<bool> UpdateTask(int id, string description)
    {
        var tasks = await LoadTasksAsync();

        AppTask? taskToUpdate = tasks.FirstOrDefault(t => t.Id == id);
        if (taskToUpdate == null)
        {
            Console.WriteLine($"Task with ID {id} not found.");
            return false;
        }

        taskToUpdate.Description = description;
        taskToUpdate.UpdatedAt = DateTime.Now;

        await SaveTasksAsync(tasks);
        return true;
    }

    private async Task<List<AppTask>> LoadTasksAsync()
    {
        try
        {
            string dataFromFile = await _fileService.ReadFileAsync(Util.FILE_PATH);
            if (string.IsNullOrEmpty(dataFromFile))
            {
                return new List<AppTask>();
            }

            return JsonSerializer.Deserialize<List<AppTask>>(dataFromFile) ?? new List<AppTask>();
        }
        catch (FileNotFoundException)
        {
            return new List<AppTask>();
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Error deserializing JSON: {ex.Message}");
        }
        catch (IOException ex)
        {
            throw new IOException($"Error reading file: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error: {ex.Message}");
        }
    }

    private async Task SaveTasksAsync(List<AppTask> tasks)
    {
        try
        {
            string json = JsonSerializer.Serialize(
                tasks,
                new JsonSerializerOptions { WriteIndented = true }
            );
            await _fileService.WriteFileAsync(Util.FILE_PATH, json);
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Error serializing JSON: {ex.Message}");
        }
        catch (IOException ex)
        {
            throw new IOException($"Error writing file: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error: {ex.Message}");
        }
    }

    public Task<bool> UpdateTask(int id, Models.TaskStatus status)
    {
        var tasks = LoadTasksAsync().Result;

        AppTask? taskToUpdate = tasks.FirstOrDefault(t => t.Id == id);
        if (taskToUpdate == null)
        {
            Console.WriteLine($"Task with ID {id} not found.");
            return Task.FromResult(false);
        }

        taskToUpdate.Status = status;
        taskToUpdate.UpdatedAt = DateTime.Now;

        SaveTasksAsync(tasks).Wait();
        return Task.FromResult(true);
    }
}
