using System.Text.Json;
using TaskManagerCLI.Models;

namespace TaskManagerCLI.Services;

public class TaskManager : ITaskManager
{
    private readonly IFileService _fileService;

    public TaskManager(IFileService fileService)
    {
        _fileService = fileService;
    }

    public Task<int> AddTask(string description)
    {
        string dataFromFile;
        try
        {
            dataFromFile = _fileService.ReadFileAsync("tasks.json").Result;
        }
        catch (Exception)
        {
            dataFromFile = string.Empty;
        }

        List<AppTask> tasks;

        if (string.IsNullOrEmpty(dataFromFile))
        {
            tasks = new List<AppTask>();
        }
        else
        {
            try
            {
                tasks =
                    JsonSerializer.Deserialize<List<AppTask>>(dataFromFile) ?? new List<AppTask>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                return Task.FromResult(-1);
            }
        }

        int newId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;

        AppTask newTask = new AppTask(newId, description);
        tasks.Add(newTask);

        try
        {
            string json = JsonSerializer.Serialize(
                tasks,
                new JsonSerializerOptions { WriteIndented = true }
            );
            _fileService.WriteFileAsync("tasks.json", json).Wait();
            return Task.FromResult(newId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing file: {ex.Message}");
            return Task.FromResult(-1);
        }
    }

    public Task<List<AppTask>> ListTasks()
    {
        string dataFromFile;
        try
        {
            dataFromFile = _fileService.ReadFileAsync("tasks.json").Result;
        }
        catch (Exception)
        {
            dataFromFile = string.Empty;
        }

        List<AppTask> tasks;
        if (string.IsNullOrEmpty(dataFromFile))
        {
            tasks = new List<AppTask>();
        }
        else
        {
            try
            {
                tasks =
                    JsonSerializer.Deserialize<List<AppTask>>(dataFromFile) ?? new List<AppTask>();
            }
            catch (JsonException ex)
            {
                throw new JsonException("Error deserializing JSON: ", ex);
            }
        }

        return Task.FromResult(tasks);
    }

    public Task<bool> DeleteTask(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> MarkTaskAsDone(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> MarkTaskAsInProgress(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTask(int id, string description)
    {
        throw new NotImplementedException();
    }
}
