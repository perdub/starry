using System.Diagnostics;
using System.Text;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Processes;

/// <summary>
/// Secure, strongly-typed system process runner preventing command injection
/// and streaming input safely without shell execution.
/// </summary>
public sealed class ProcessRunner : IProcessRunner
{
    public async Task<ProcessResult> RunAsync(ProcessSpec spec, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(spec);

        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        if (spec.Timeout > TimeSpan.Zero && spec.Timeout < TimeSpan.MaxValue)
        {
            linkedCts.CancelAfter(spec.Timeout);
        }

        var startInfo = CreateStartInfo(spec);
        using var process = new Process { StartInfo = startInfo };
        var stdoutBuilder = new StringBuilder();
        var stderrBuilder = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                stdoutBuilder.AppendLine(e.Data);
            }
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                stderrBuilder.AppendLine(e.Data);
            }
        };

        var stopwatch = Stopwatch.StartNew();
        StartProcess(process, spec.Executable);

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (spec.StandardInputBytes.HasValue)
        {
            await WriteInputAsync(process, spec.StandardInputBytes.Value, linkedCts.Token).ConfigureAwait(false);
        }

        try
        {
            await process.WaitForExitAsync(linkedCts.Token).ConfigureAwait(false);
            stopwatch.Stop();

            return new ProcessResult(process.ExitCode, stdoutBuilder.ToString(), stderrBuilder.ToString(), stopwatch.Elapsed);
        }
        catch (OperationCanceledException ex)
        {
            TerminateProcessTree(process);
            stopwatch.Stop();
            throw new TimeoutException($"Process '{spec.Executable}' timed out or was cancelled after {stopwatch.ElapsedMilliseconds} ms.", ex);
        }
    }

    private static async Task WriteInputAsync(Process process, ReadOnlyMemory<byte> inputBytes, CancellationToken ct)
    {
        try
        {
            await process.StandardInput.BaseStream.WriteAsync(inputBytes, ct).ConfigureAwait(false);
            await process.StandardInput.BaseStream.FlushAsync(ct).ConfigureAwait(false);
            process.StandardInput.Close();
        }
        catch (IOException)
        {
            // Process may terminate early before reading all standard input
        }
    }

    private static ProcessStartInfo CreateStartInfo(ProcessSpec spec)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = spec.Executable,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = spec.StandardInputBytes.HasValue,
            CreateNoWindow = true
        };

        if (!string.IsNullOrEmpty(spec.WorkingDirectory))
        {
            startInfo.WorkingDirectory = spec.WorkingDirectory;
        }

        foreach (var arg in spec.Arguments)
        {
            startInfo.ArgumentList.Add(arg);
        }

        foreach (var (key, value) in spec.EnvironmentVariables)
        {
            startInfo.EnvironmentVariables[key] = value;
        }

        return startInfo;
    }

    private static void StartProcess(Process process, string executable)
    {
        try
        {
            if (!process.Start())
            {
                throw new InvalidOperationException($"Failed to start process: {executable}");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error launching process '{executable}': {ex.Message}", ex);
        }
    }

    private static void TerminateProcessTree(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch (InvalidOperationException)
        {
            // Process already terminated
        }
    }
}
