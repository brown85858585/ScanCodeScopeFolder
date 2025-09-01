using System;
using System.Collections.Generic;

public class MenuService : IMenuService
{
    private readonly IFileScanner _scanner;
    private readonly IFileFormatter _formatter;
    private readonly AppConfig _config;
    private readonly IGitService _gitService;
    private readonly char _scanKey = '1';
    private readonly char _gitScanKey = '2'; 
    private readonly char _configKey = '3';
    private readonly char _exitKey = '5';

    public MenuService(IFileScanner scanner, IFileFormatter formatter, AppConfig config, IGitService gitService)
    {
        _scanner = scanner;
        _formatter = formatter;
        _config = config;
        _gitService = gitService;
    }

    public void ProcessKey(char keyChar)
    {
        switch (keyChar)
        {
            case var k when k == _scanKey:
                ScanDirectory();
                break;
                
            case var k when k == _gitScanKey:
                ScanGitRepository();
                break;
                
            case var k when k == _configKey:
                ShowConfiguration();
                break;
                
            case var k when k == _exitKey:
                ExitProgram();
                break;
                
            default:
                ShowUnknownKeyMessage(keyChar);
                break;
        }
    }

    private async void ScanGitRepository()
    {
        Console.WriteLine("\n +++ Scan Git Repository");
        Console.Write("Enter repository URL: ");
        var repositoryUrl = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(repositoryUrl))
        {
            Console.WriteLine("Repository URL cannot be empty!");
            return;
        }

        string? tempDir = null;
        try
        {
            var progress = new Progress<string>(message => 
            {
                Console.WriteLine($"   {DateTime.Now:HH:mm:ss}: {message}");
            });

            tempDir = await _gitService.CloneRepositoryAsync(repositoryUrl, progress);
            
            var projectName = ExtractProjectName(repositoryUrl);
            
            Console.WriteLine($"\n +++ Scanning cloned repository: {projectName}");
            
            var fileData = _scanner.ScanDirectory(tempDir);
            
            Console.WriteLine($"\n +++ Scan finished. Scanned files: {fileData?.Count ?? 0}");
            
            if (fileData != null && fileData.Count > 0)
            {
                CreateProjectSummary(fileData, tempDir, projectName);
                Console.WriteLine($"===> {projectName}_Summary.md created successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scanning Git repository: {ex.Message}");
        }
        finally
        {
            if (tempDir != null)
            {
                _gitService.Cleanup(tempDir);
            }
        }
    }

    private string ExtractProjectName(string repositoryUrl)
    {
        try
        {
            // Извлекаем имя проекта из URL
            var uri = new Uri(repositoryUrl);
            var segments = uri.Segments;
            var lastSegment = segments[^1].TrimEnd('/');
            
            // Убираем .git если есть
            if (lastSegment.EndsWith(".git"))
            {
                lastSegment = lastSegment[..^4];
            }
            
            return lastSegment;
        }
        catch
        {
            return "GitProject";
        }
    }

    private void CreateProjectSummary(List<FileInfoContainer> files, string rootPath, string projectName)
    {
        var summaryPath = Path.Combine(Directory.GetCurrentDirectory(), $"{projectName}_Summary.md");
        
        using (var writer = new StreamWriter(summaryPath, false, System.Text.Encoding.UTF8))
        {
            writer.WriteLine($"# {projectName} - Project Summary");
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            writer.WriteLine($"Repository scanned remotely");
            writer.WriteLine($"Total files: {files.Count}");
            writer.WriteLine();

            // Остальная логика создания отчета такая же как в CreateFile
            writer.WriteLine("## File List (Sorted by Priority)");
            writer.WriteLine();

            var groupedFiles = files.GroupBy(f => f.Priority)
                                   .OrderBy(g => g.Key);

            foreach (var group in groupedFiles)
            {
                writer.WriteLine($"### Priority {group.Key} Files");
                foreach (var file in group)
                {
                    var relativePath = Path.GetRelativePath(rootPath, file.FilePath);
                    writer.WriteLine($"- **{relativePath}** ({file.Size} bytes, {file.LastModified:yyyy-MM-dd})");
                }
                writer.WriteLine();
            }

            writer.WriteLine("## File Contents");
            writer.WriteLine();

            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(rootPath, file.FilePath);
                
                writer.WriteLine($"### {relativePath}");
                writer.WriteLine($"```{file.Extension.TrimStart('.')}");
                writer.WriteLine(file.Content);
                writer.WriteLine("```");
                writer.WriteLine();
            }
        }
    }

    private void ScanDirectory()
    {
        Console.WriteLine("\n +++ Start Scan current Directory...");
        
        string currentDirectory = Directory.GetCurrentDirectory();
        var fileData = _scanner.ScanDirectory(currentDirectory);
        
        Console.WriteLine($"\n +++ Scan finished. Scanned files: {fileData?.Count ?? 0}");
        
        if (fileData != null && fileData.Count > 0)
        {
            _formatter.CreateSummary(fileData, currentDirectory);
            Console.WriteLine("===> Summary.md created successfully!");
        }
    }

    private void ShowConfiguration()
    {
        Console.WriteLine("\n=== Current Configuration ===");
        Console.WriteLine("File Priorities:");
        foreach (var kvp in _config.FilePriorities.OrderBy(x => x.Value))
        {
            Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
        }
        
        Console.WriteLine($"\nDefault Priority: {_config.DefaultPriority}");
        
        Console.WriteLine("\nIgnored Patterns: " + string.Join(", ", _config.IgnoredPatterns));
        Console.WriteLine("Ignored Files: " + string.Join(", ", _config.IgnoredFiles));
        Console.WriteLine("Text Extensions: " + string.Join(", ", _config.TextFileExtensions));
        Console.WriteLine("================================\n");
    }

    private void ExitProgram()
    {
        BeepMenuCall(500, 300, "Goodbye!");
        Environment.Exit(0);
    }

    private void ShowUnknownKeyMessage(char keyChar)
    {
        BeepMenuCall(500, 300,
            $" !!! You pushed: {keyChar} key.",
            "Available options:",
            $"{_scanKey} - Scan directory and create summary",
            $"{_gitScanKey} - Scan Git repository",
            $"{_configKey} - Show current configuration", 
            $"{_exitKey} - Exit program"
        );
    }

    public void ShowMainMenu()
    {
        BeepMenuCall(500, 300,
            "=== GitHubAnalizer ===",
            "Available options:",
            $"{_scanKey} - Scan directory and create summary",
            $"{_gitScanKey} - Scan Git repository",
            $"{_configKey} - Show current configuration", 
            $"{_exitKey} - Exit program",
            "======================"
        );
    }

    private static void BeepMenuCall(int frequency, int duration, params string[] messages)
    {
        if (OperatingSystem.IsWindows())
        {
            Console.Beep(frequency, duration);
        }
        
        foreach (var message in messages)
        {
            Console.WriteLine(message);
        }
    }
}