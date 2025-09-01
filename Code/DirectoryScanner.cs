using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class DirectoryScanner : IFileScanner
{
    private readonly IFileFilterService _filterService;
    private readonly IFilePriorityService _priorityService;
    private readonly IDirectoryWalker _directoryWalker;

    public DirectoryScanner(IFileFilterService filterService, 
                          IFilePriorityService priorityService,
                          IDirectoryWalker directoryWalker)
    {
        _filterService = filterService;
        _priorityService = priorityService;
        _directoryWalker = directoryWalker;
    }

    public List<FileInfoContainer> ScanDirectory(string rootPath)
    {
        var fileInfos = new List<FileInfoContainer>();
        
        try
        {
            foreach (var filePath in _directoryWalker.GetAllFiles(rootPath))
            {
                try
                {
                    if (!_filterService.ShouldIncludeFile(filePath))
                    {
                        Console.WriteLine($" -// Ignored: {filePath}");
                        continue;
                    }

                    var fileInfo = new FileInfo(filePath);
                    var fileExtension = Path.GetExtension(filePath).ToLower();

                    var fileContainer = new FileInfoContainer
                    {
                        FilePath = filePath,
                        LastModified = fileInfo.LastWriteTime,
                        Size = fileInfo.Length,
                        Extension = fileExtension,
                        Priority = _priorityService.GetPriority(fileExtension)
                    };

                    if (_filterService.IsTextFile(fileExtension))
                    {
                        fileContainer.Content = File.ReadAllText(filePath, Encoding.UTF8);
                    }
                    else
                    {
                        fileContainer.Content = $" -// [Binary file - {fileExtension}]";
                    }
                    
                    fileInfos.Add(fileContainer);
                    Console.WriteLine($" /// Processed: {filePath} (Priority: {fileContainer.Priority})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" !!! Error reading {filePath}: {ex.Message}");
                }
            }

            fileInfos = fileInfos
                .OrderBy(f => f.Priority)
                .ThenBy(f => f.Extension)
                .ThenBy(f => f.FilePath)
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($" !!! Error scanning directory: {ex.Message}");
        }
        
        return fileInfos;
    }
}