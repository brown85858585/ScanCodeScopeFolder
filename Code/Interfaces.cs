
using System.Collections.Generic;
using System.IO;

public interface IFileScanner { List<FileInfoContainer> ScanDirectory(string rootPath); }
public interface IFileFilterService { bool ShouldIncludeFile(string filePath);
                                    bool IsTextFile(string extension); }
public interface IFilePriorityService { int GetPriority(string extension); }
public interface IFileFormatter { void CreateSummary(List<FileInfoContainer> files, string rootPath); }
public interface IDirectoryWalker { IEnumerable<string> GetAllFiles(string rootPath); }
public interface IMenuService { void ProcessKey(char keyChar);
                            void ShowMainMenu(); }