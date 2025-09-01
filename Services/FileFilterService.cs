using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FileFilterService : IFileFilterService
{
    private readonly HashSet<string> _ignoredFiles;
    private readonly HashSet<string> _ignoredExtensions;
    private readonly HashSet<string> _ignoredPatterns;
    private readonly HashSet<string> _textExtensions;

    public FileFilterService(AppConfig config)
    {
        _ignoredFiles = config.IgnoredFiles;
        _ignoredExtensions = new HashSet<string>(config.IgnoredFiles.Where(f => f.StartsWith(".")));
        _ignoredPatterns = config.IgnoredPatterns;
        _textExtensions = config.TextFileExtensions;
    }

    public bool ShouldIncludeFile(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var fileExtension = Path.GetExtension(filePath).ToLower();

        if (_ignoredFiles.Contains(fileName) || _ignoredExtensions.Contains(fileExtension))
            return false;

        if (_ignoredPatterns.Any(pattern => 
            fileName.Contains(pattern, System.StringComparison.OrdinalIgnoreCase)))
            return false;

        var directoryName = Path.GetFileName(Path.GetDirectoryName(filePath));
        if (directoryName == "bin" || directoryName == "obj" || directoryName == ".git")
            return false;

        return true;
    }

    public bool IsTextFile(string extension) => _textExtensions.Contains(extension);
}