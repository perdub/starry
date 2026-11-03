using Starry.Core.Processes;
using Starry.Infrastructure.Processes;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Service managing automated Git repository upstream synchronization (commit, pull --rebase, push).
/// </summary>
public sealed class GitSyncService
{
    private readonly IProcessRunner _runner;

    public GitSyncService(IProcessRunner? runner = null)
    {
        _runner = runner ?? new ProcessRunner();
    }

    public async Task<bool> SyncUpstreamAsync(string workingDir, string commitMessage = "chore(starry): auto-sync cluster configuration", CancellationToken ct = default)
    {
        try
        {
            await _runner.RunAsync(new ProcessSpec
            {
                Executable = "git",
                Arguments = ["add", "-A"],
                WorkingDirectory = workingDir
            }, ct).ConfigureAwait(false);

            await _runner.RunAsync(new ProcessSpec
            {
                Executable = "git",
                Arguments = ["commit", "-m", commitMessage],
                WorkingDirectory = workingDir
            }, ct).ConfigureAwait(false);

            var pullResult = await _runner.RunAsync(new ProcessSpec
            {
                Executable = "git",
                Arguments = ["pull", "--rebase", "--autostash"],
                WorkingDirectory = workingDir
            }, ct).ConfigureAwait(false);

            var pushResult = await _runner.RunAsync(new ProcessSpec
            {
                Executable = "git",
                Arguments = ["push"],
                WorkingDirectory = workingDir
            }, ct).ConfigureAwait(false);

            return pullResult.IsSuccess && pushResult.IsSuccess;
        }
        catch (Exception ex) when (ex is InvalidOperationException or TimeoutException or IOException)
        {
            return false;
        }
    }
}
