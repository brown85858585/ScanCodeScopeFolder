public class AppConfig
{
    public Dictionary<string, int> FilePriorities { get; set; } = new()
    {
        // Высокий приоритет: Код и ключевые ассеты
        { ".cs", 10 },
        { ".asmdef", 11 },
        { ".shader", 12 },
        { ".shadergraph", 12 },
        { ".cginc", 12 },
        { ".hlsl", 12 },
        
        // Средний приоритет: Структура проекта и конфиги
        { ".csproj", 20 },
        { ".sln", 20 },
        { ".json", 20 },
        { ".xml", 20 },
        { ".config", 21 },
        
        // Важные Unity-ассеты (текстовые YAML-форматы)
        { ".unity", 25 },   // Сцены
        { ".prefab", 25 },  // Префабы
        { ".asset", 26 },   // Файлы ассетов (скриптаблобжекты)
        { ".controller", 26 }, // Animator Controller
        { ".anim", 26 },    // Animation Clip
        { ".mat", 27 },     // Materials (тоже YAML)
        { ".physicsmaterial2d", 27 },
        { ".physicsmaterial", 27 },
        
        // Мета-файлы (очень важны для понимания структуры)
        { ".meta", 30 },
        
        // Низкий приоритет: Документация
        { ".md", 21 },
        { ".txt", 21 },
        { ".yml", 22 },
        { ".yaml", 22 },
        { ".gitignore", 22 }
    };

    public HashSet<string> IgnoredDirectories { get; set; } = new()
    {
        // Стандартные игнорируемые папки
        "bin", "obj", ".git", ".vs", ".vscode",
        "Library", "Temp", "Build", "Builds", "WebGLBuild",
        "Logs", "Backups", "Packages", "UserSettings",
        
        // Unity специфичные
        "Assets/AssetStoreTools",
        "ProjectSettings/AssetGraphSettings"
    };

    public HashSet<string> IgnoredPatterns { get; set; } = new()
    {        
        // Бинарные файлы
        ".dll", ".pdb", ".exe", ".cache", ".suo", ".user",
        ".bin", ".obj", ".nupkg", ".snupkg", ".jar", ".a",
        
        // Скомпилированные/кешированные ассеты Unity
        "*.cubemap", "*.rendertexture", "*.lighting", "*.guiskin",
        "*.flare", "*.fontsettings", "*.terrainlayer", "*.tmp"
    };

    public HashSet<string> TextFileExtensions { get; set; } = new()
    {
        // Все текстовые форматы, включая Unity-специфичные
        ".cs", ".csproj", ".sln", ".json", ".config", ".xml",
        ".md", ".txt", ".yml", ".yaml", ".gitignore", ".editorconfig",
        // Unity YAML-ассеты
        ".unity", ".prefab", ".asset", ".controller", ".anim", ".mat",
        ".physicsmaterial", ".physicsmaterial2d",
        // Шейдеры и код GPU
        ".shader", ".shadergraph", ".cginc", ".hlsl", ".compute",
        // Мета-файлы
        ".meta",
        // Файлы определения сборок
        ".asmdef", ".asmref"
    };

    public HashSet<string> IgnoredFiles { get; set; } = new()
    {
        // Игнорируем собственные отчеты и генерируемые файлы
        "ScanCodeScopeSummary.md",
        
        // Системные файлы Unity
        "ProjectSettings.asset",
        "UnityLockFile",
        "CREDITS.html",
        "LICENSE.html",
        "COPYING.html",
        
        // Файлы IDE
        ".DS_Store",
        "Thumbs.db"
    };

    public int DefaultPriority { get; set; } = 50;
}