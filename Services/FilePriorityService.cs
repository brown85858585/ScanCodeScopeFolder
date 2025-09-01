using System.Collections.Generic;

public class FilePriorityService : IFilePriorityService
{
    private readonly Dictionary<string, int> _filePriority;
    private readonly int _defaultPriority;

    public FilePriorityService(AppConfig config)
    {
        _filePriority = config.FilePriorities;
        _defaultPriority = config.DefaultPriority;
    }

    public int GetPriority(string extension) => 
        _filePriority.TryGetValue(extension, out int priority) ? priority : _defaultPriority;
}