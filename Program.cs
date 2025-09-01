using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

class Program
{    
    private static ServiceProvider? _serviceProvider;
    private static ServiceCollection _services = new ServiceCollection();

    static void Main()
    {
        DefaultMain ();
    }

    static void DefaultMain ()
    {
        MainServices();
        
        var menuService = _serviceProvider!.GetRequiredService<IMenuService>();

        menuService.ShowMainMenu();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                menuService.ProcessKey(key.KeyChar);
                menuService.ShowMainMenu();
            }
            Thread.Sleep(100);
        }
    }

    static void OneAppMain ()
    {
        try
        {
            MainServices();

            var scanner = _serviceProvider!.GetRequiredService<IFileScanner>();
            var formatter = _serviceProvider!.GetRequiredService<IFileFormatter>();
            
            string currentDirectory = Directory.GetCurrentDirectory();
            var fileData = scanner.ScanDirectory(currentDirectory);
            
            if (fileData != null && fileData.Count > 0)
            {
                formatter.CreateSummary(fileData, currentDirectory);
                Console.WriteLine($"ScanCodeScope: Created summary for {fileData.Count} files");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void MainServices()
    {
        var config = ConfigParser.LoadConfig("ScanCodeScope.cfg");
        
        _services.AddSingleton<AppConfig>(config);
        _services.AddSingleton<IFileScanner, DirectoryScanner>();
        _services.AddSingleton<IFileFilterService, FileFilterService>();
        _services.AddSingleton<IFilePriorityService, FilePriorityService>();
        _services.AddSingleton<IDirectoryWalker, DirectoryWalker>();
        _services.AddSingleton<IFileFormatter, CreateFile>();
        _services.AddSingleton<IMenuService, MenuService>();
        
        _serviceProvider = _services.BuildServiceProvider();
    }
}