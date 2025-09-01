using System;
using System.IO;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

public class GitService : IGitService
{
    public async Task<string> CloneRepositoryAsync(string repositoryUrl, IProgress<string> progress)
    {
        return await Task.Run(() =>
        {
            try
            {
                var tempDir = Path.Combine(Path.GetTempPath(), "ScanCodeScope", Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempDir);

                progress?.Report($"Cloning repository: {repositoryUrl}");
                progress?.Report($"To directory: {tempDir}");

                var cloneOptions = new CloneOptions
                {
                    Checkout = true,
                    OnProgress = (serverProgressOutput) =>
                    {
                        progress?.Report($"Git: {serverProgressOutput}");
                        return true;
                    },
                    OnTransferProgress = (transferProgress) =>
                    {
                        if (transferProgress.TotalObjects > 0)
                        {
                            var percentage = (transferProgress.ReceivedObjects * 100) / transferProgress.TotalObjects;
                            progress?.Report($"Downloading: {percentage}% ({transferProgress.ReceivedObjects}/{transferProgress.TotalObjects} objects)");
                        }
                        return true;
                    },
                    OnCheckoutProgress = (path, completedSteps, totalSteps) =>
                    {
                        if (totalSteps > 0)
                        {
                            var percentage = (completedSteps * 100) / totalSteps;
                            progress?.Report($"Checking out: {percentage}% ({completedSteps}/{totalSteps} files)");
                        }
                    }
                };

                Repository.Clone(repositoryUrl, tempDir, cloneOptions);

                progress?.Report("Repository cloned successfully!");
                return tempDir;
            }
            catch (Exception ex)
            {
                progress?.Report($"Error cloning repository: {ex.Message}");
                throw;
            }
        });
    }

    public void Cleanup(string tempPath)
    {
        try
        {
            if (Directory.Exists(tempPath))
            {
                RemoveReadOnlyAttributes(tempPath);
                
                Directory.Delete(tempPath, true);
                Console.WriteLine($"Cleaned up temporary directory: {tempPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cleaning up: {ex.Message}");
        }
    }

    private void RemoveReadOnlyAttributes(string directoryPath)
    {
        foreach (var file in Directory.GetFiles(directoryPath))
        {
            var attributes = File.GetAttributes(file);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(file, attributes & ~FileAttributes.ReadOnly);
            }
        }

        foreach (var dir in Directory.GetDirectories(directoryPath))
        {
            RemoveReadOnlyAttributes(dir);
        }
    }
}