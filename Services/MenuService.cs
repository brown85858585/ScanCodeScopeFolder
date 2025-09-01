using System;
using System.Collections.Generic;

public class MenuService : IMenuService
{
    private readonly IFileScanner _scanner;
    private readonly IFileFormatter _formatter;
    private readonly AppConfig _config;
    private readonly char _scanKey = '1';
    private readonly char _configKey = '3';
    private readonly char _exitKey = '5';

    public MenuService(IFileScanner scanner, IFileFormatter formatter, AppConfig config)
    {
        _scanner = scanner;
        _formatter = formatter;
        _config = config;
    }

    public void ProcessKey(char keyChar)
    {
        switch (keyChar)
        {
            case var k when k == _scanKey:
                ScanDirectory();
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