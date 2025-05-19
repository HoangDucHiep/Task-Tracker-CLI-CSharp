namespace TaskManagerCLI.Models;

public class AppTask
{
    public int Id { get; private set; }
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public AppTask(int id, string description)
    {
        Id = id;
        Description = description;
        Status = TaskStatus.todo;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}
