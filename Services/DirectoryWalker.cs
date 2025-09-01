using System.Collections.Generic;
using System.IO;

public class DirectoryWalker : IDirectoryWalker
{
    public IEnumerable<string> GetAllFiles(string rootPath)
    {
        var queue = new Queue<string>();
        queue.Enqueue(rootPath);

        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();

            foreach (var file in Directory.GetFiles(currentPath))
            {
                yield return file;
            }

            foreach (var directory in Directory.GetDirectories(currentPath))
            {
                queue.Enqueue(directory);
            }
        }
    }
}