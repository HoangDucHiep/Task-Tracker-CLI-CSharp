namespace TaskManagerCLI.Services;

public class FileService : IFileService
{
    public Task<bool> FileExistsAsync(string filePath)
    {
        return Task.FromResult(File.Exists(filePath));
    }

    public Task<string> ReadFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file '{filePath}' does not exist.");
        }

        try
        {
            return Task.FromResult(File.ReadAllText(filePath));
        }
        catch (Exception ex)
        {
            throw new IOException($"Error reading the file '{filePath}': {ex.Message}", ex);
        }
    }

    public Task WriteFileAsync(string filePath, string content)
    {
        try
        {
            File.WriteAllText(filePath, content);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new IOException($"Error writing to the file '{filePath}': {ex.Message}", ex);
        }
    }
}
