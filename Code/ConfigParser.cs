using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ConfigParser
{
    public static AppConfig LoadConfig(string filePath = "config.txt")
    {
        var config = new AppConfig();
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Config file '{filePath}' not found. Using default configuration.");
            return config;
        }

        try
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                ParseLine(line, config);
            }
            
            Console.WriteLine($"Configuration loaded successfully from '{filePath}'");
            return config;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading config file: {ex.Message}");
            Console.WriteLine("Using default configuration.");
            return config;
        }
    }

    private static void ParseLine(string line, AppConfig config)
    {
        // Пропускаем пустые строки и комментарии
        line = line.Trim();
        if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
            return;

        // Игнорируем строки без разделителя
        var separatorIndex = line.IndexOf('=');
        if (separatorIndex <= 0)
            return;

        var key = line.Substring(0, separatorIndex).Trim();
        var value = line.Substring(separatorIndex + 1).Trim();

        ProcessConfigValue(key, value, config);
    }

    private static void ProcessConfigValue(string key, string value, AppConfig config)
    {
        try
        {
            switch (key.ToLower())
            {
                case "filepriority":
                    ParseFilePriority(value, config.FilePriorities);
                    break;
                    
                case "ignorepattern":
                    config.IgnoredPatterns.Add(value);
                    break;
                    
                case "ignorefile":
                    config.IgnoredFiles.Add(value);
                    break;
                    
                case "textextension":
                    config.TextFileExtensions.Add(value);
                    break;
                    
                case "defaultpriority":
                    if (int.TryParse(value, out int defaultPriority))
                    {
                        // Сохраним для использования в сервисе
                        config.DefaultPriority = defaultPriority;
                    }
                    break;
                    
                default:
                    Console.WriteLine($"Unknown config key: {key}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing config key '{key}': {ex.Message}");
        }
    }

    private static void ParseFilePriority(string value, Dictionary<string, int> priorities)
    {
        // Формат: .cs=20 или cs=20
        var parts = value.Split('=');
        if (parts.Length == 2)
        {
            var extension = parts[0].Trim();
            if (!extension.StartsWith("."))
                extension = "." + extension;
                
            if (int.TryParse(parts[1].Trim(), out int priority))
            {
                priorities[extension] = priority;
            }
        }
    }
}