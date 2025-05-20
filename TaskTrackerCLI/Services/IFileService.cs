namespace TaskManagerCLI.Services;

public interface IFileService
{
    Task<string> ReadFileAsync(string filePath);
    Task WriteFileAsync(string filePath, string content);
    Task<bool> FileExistsAsync(string filePath);
}
