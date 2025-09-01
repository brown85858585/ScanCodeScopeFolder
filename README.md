# ScanCodeScope / GitHub Analyzer 🤖📁

**A powerful tool to prepare your codebase for analysis by AI models (ChatGPT, Claude, Gemini, etc.)**

[![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE.md)

## 🎯 Why is this needed?

Modern AI models (LLMs) have enormous potential for code analysis, refactoring, vulnerability detection, and documentation generation. However, they have context processing limits.

**This program solves a key problem:** it automatically analyzes your project, filters the most important content, and creates a structured summary that you can feed to AI for effective contextual analysis.

### Advantages over simple copying:
- **✅ Prioritization**: Important files (.cs, .csproj) are shown first
- **✅ Filtering**: Binary files, builds, and caches are ignored (.dll, .exe, bin/, obj/)
- **✅ Context**: The AI sees the project structure, not scattered files
- **✅ Token economy**: Only relevant code enters the context, not all the junk
- **✅ Convenience**: One `ScanCodeScopeSummary.md` file instead of dozens of copied files

## 🚀 Quick Start

1.  **Download and run** the executable from the latest release
2.  **Place it in your project root** (next to `.sln` or `.csproj`)
3.  **Run the program**:
    ```bash
    # Simply run the executable
    ScanCodeScope.exe
    ```
4.  **Done!** The `ScanCodeScopeSummary.md` file will appear in the project root
5.  **Copy its contents** and paste into your AI prompt

**Example prompt for ChatGPT/Claude:**
> "Please analyze the codebase presented below. This is the scanning result of the ScanCodeScope project. Describe the overall architecture, find potential issues, suggest improvements, and check for compliance with modern C# and .NET practices."
> *[Insert ScanCodeScopeSummary.md content]*

## ✨ Key Features

-   **Smart scanning**: Recursive folder traversal ignoring `bin`, `obj`, `.git`, `.vs` and binary files
-   **Priority system**: Files sorted by importance (source code > configs > documentation)
-   **Content extraction**: Automatic reading and insertion of only text files (.cs, .md, .json, .txt, etc.)
-   **Comprehensive report**: Generates a single Markdown file with table of contents, metadata, and all file contents
-   **Flexible configuration**: Custom priorities and filtering rules via `ScanCodeScope.cfg` file

## 📁 What does the result look like?

The program creates a detailed `.md` file that includes:
-   Header and generation date
-   **List of all files**, sorted by priority, with sizes and dates
-   **Contents of each file**, formatted in code blocks with syntax highlighting

**Result:** You get a perfectly prepared "snapshot" of your project for sending to AI assistants.

## ⚙️ Configuration (optional)

For fine-tuning, create a `ScanCodeScope.cfg` file in the project root:

```ini
# File priorities (lower number = higher priority in report)
filepriority=.cs=10
filepriority=.csproj=15
filepriority=.json=20
filepriority=.config=21
filepriority=.md=21

# File patterns to completely ignore
ignorepattern=.dll
ignorepattern=.exe
ignorepattern=.pdb
ignorepattern=.suo
ignorepattern=.user

# Which extensions to consider as text (their content will be shown)
textextension=.cs
textextension=.md
textextension=.json
textextension=.txt

# Default priority for all other files
defaultpriority=50


## 🏗️ For Developers

The project is written in C# (.NET 8.0) using modern practices:
- **Clean Architecture**: Interfaces and Dependency Injection
- **SOLID principles**: Each service has single responsibility
- **Easy extensibility**: Easy to add new filters, formatters, or sources

**Build from source:**
git clone https://github.com/your-username/ScanCodeScope.git
cd ScanCodeScope
dotnet build
dotnet publish -c Release -r win-x64 --self-contained true
dotnet publish -c Release -r linux-x64 --self-contained true
dotnet publish -c Release -r osx-x64 --self-contained true

## 📄 License

This project is distributed under the MIT license. For details, see the LICENSE.md file.

Make your AI communications about code maximally efficient! 🚀