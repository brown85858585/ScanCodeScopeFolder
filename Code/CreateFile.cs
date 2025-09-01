using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class CreateFile : IFileFormatter
{
    public void CreateSummary(List<FileInfoContainer> files, string rootPath)
    {
        var summaryPath = Path.Combine(rootPath, "ScanCodeScopeSummary.md");
        
        using (var writer = new StreamWriter(summaryPath, false, Encoding.UTF8))
        {
            writer.WriteLine("# Project Summary");
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            writer.WriteLine($"Total files: {files.Count}");
            writer.WriteLine();

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
}