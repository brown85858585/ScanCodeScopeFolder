using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FileFilterService : IFileFilterService
{
    private readonly HashSet<string> _ignoredFiles;
    private readonly HashSet<string> _ignoredExtensions;
    private readonly HashSet<string> _ignoredPatterns;
    private readonly HashSet<string> _textExtensions;
    private readonly HashSet<string> _ignoredDirectories;

    public FileFilterService(AppConfig config)
    {
        _ignoredFiles = config.IgnoredFiles;
        _ignoredExtensions = new HashSet<string>(config.IgnoredFiles.Where(f => f.StartsWith(".")));
        _ignoredPatterns = config.IgnoredPatterns;
        _textExtensions = config.TextFileExtensions;
        _ignoredDirectories = config.IgnoredDirectories;
    }

    public bool ShouldIncludeFile(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var fileExtension = Path.GetExtension(filePath).ToLower();

        // Проверяем игнорируемые файлы и расширения
        if (_ignoredFiles.Contains(fileName) || _ignoredExtensions.Contains(fileExtension))
            return false;

        // Проверяем игнорируемые паттерны в имени файла
        if (_ignoredPatterns.Any(pattern => 
            fileName.Contains(pattern, System.StringComparison.OrdinalIgnoreCase)))
            return false;

        // Проверяем, находится ли файл в игнорируемой директории
        if (IsInIgnoredDirectory(filePath))
            return false;

        return true;
    }

    private bool IsInIgnoredDirectory(string filePath)
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        if (string.IsNullOrEmpty(directoryPath))
            return false;

        // Разбиваем путь на части и проверяем каждую директорию
        var directories = directoryPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        
        foreach (var directory in directories)
        {
            if (_ignoredDirectories.Contains(directory, System.StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            
            // Дополнительная проверка для паттернов, которые могут быть частью имени директории
            if (_ignoredPatterns.Any(pattern => 
                directory.Contains(pattern, System.StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsTextFile(string extension) => _textExtensions.Contains(extension);
}