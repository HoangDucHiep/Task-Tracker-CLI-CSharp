namespace TaskTrackerCLI;

public class Task
{
    public long Id { get; set; }
    public required string description { get; set; }
    public TaskStatus status { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}

public enum TaskStatus
{
    ToDo,
    InProgress,
    Done,
}
