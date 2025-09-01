using System.Collections.Generic;

public class AppConfig02
{
    public Dictionary<string, int> FilePriorities { get; set; } = new()
    {
        { ".cs", 20 }, { ".csproj", 20 }, { ".sln", 20 }, { ".json", 20 },
        { ".config", 21 }, { ".xml", 21 }, { ".md", 21 }, { ".txt", 21 },
        { ".yml", 22 }, { ".yaml", 22 }, { ".gitignore", 22 }
    };

    public HashSet<string> IgnoredPatterns { get; set; } = new()
    {
        ".dll", ".pdb", ".exe", ".cache", ".suo", ".user",
        ".vs", ".bin", ".obj", ".nupkg", ".snupkg"
    };

    public HashSet<string> TextFileExtensions { get; set; } = new()
    {
        ".cs", ".csproj", ".sln", ".json", ".config", ".xml",
        ".md", ".txt", ".yml", ".yaml", ".gitignore", ".editorconfig"
    };

    public HashSet<string> IgnoredFiles { get; set; } = new()
    {
        "Summary.md", "ScanCodeScopeSummary.md", ".dll", ".exe", ".pdb"
    };

    public int DefaultPriority { get; set; } = 50;
}