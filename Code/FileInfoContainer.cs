
using System;

public class FileInfoContainer
{
    public string FilePath { get; set; }  = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
    public long Size { get; set; }
    public string Extension { get; set; } = string.Empty;
    public int Priority { get; set; }
}