# Changelog

All notable changes to the `GitHubAnalizer` (ScanCodeScope) project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/).

## [1.0.0] - 2025-09-01
### Added
- **Initial release**: Basic codebase scanner functionality
- **Core features**:
  - Recursive directory scanning
  - File priority system based on extensions
  - Filtering of binary and system files (bin, obj, .git, etc.)
  - Reading and displaying contents of text files
  - Generation of a comprehensive Markdown report (`ScanCodeScopeSummary.md`)
  - Configuration via `ScanCodeScope.cfg` file
  - Support for interfaces and Dependency Injection for clean code
- **Interface**: Console menu for interactive control
- **Build**: Support for .NET 8.0 and publishing as a self-contained executable (win-x64)

# Changelog

Все заметные изменения в проекте `GitHubAnalizer` (ScanCodeScope) будут документироваться в этом файле.

Формат основан на [Keep a Changelog](https://keepachangelog.com/ru/1.0.0/), и проект придерживается [Семантического Версионирования](https://semver.org/lang/ru/).

## [1.0.0] - 2025-09-01
### Добавлено
- **Первоначальный релиз**: Базовая функциональность сканера кодовой базы.
- **Основные функции**:
  - Рекурсивное сканирование директорий.
  - Система приоритетов файлов на основе расширений.
  - Фильтрация бинарных и системных файлов (bin, obj, .git и т.д.).
  - Чтение и отображение содержимого текстовых файлов.
  - Генерация комплексного Markdown-отчета (`ScanCodeScopeSummary.md`).
  - Конфигурация через файл `ScanCodeScope.cfg`.
  - Поддержка интерфейсов и Dependency Injection для чистого кода.
- **Интерфейс**: Консольное меню для интерактивного управления.
- **Сборка**: Поддержка .NET 8.0 и публикации как самораспаковывающегося исполняемого файла (win-x64).
