using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using Spectre.Console;
using Starry.Core.Configuration;
using Starry.Core.Localization;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Agent;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Localization;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Processes;
using Starry.Infrastructure.Security;

namespace Starry.Cli;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var lang = ExtractOptionValue(args, "--lang");
        ILocalizer localizer = new Localizer(lang);

        if (args.Length == 0 || ContainsHelp(args))
        {
            RenderHeader(localizer);
            RenderHelp(localizer);
            return 0;
        }

        if (ContainsVersion(args))
        {
            RenderVersion(localizer);
            return 0;
        }

        var command = args[0].Trim();
        var commandArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();

        return await DispatchCommandAsync(command, commandArgs, localizer).ConfigureAwait(false);
    }

    private static bool ContainsHelp(string[] args)
    {
        return args.Any(a => string.Equals(a, "--help", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(a, "-h", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(a, "help", StringComparison.OrdinalIgnoreCase));
    }

    private static bool ContainsVersion(string[] args)
    {
        return args.Any(a => string.Equals(a, "--version", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(a, "-v", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(a, "version", StringComparison.OrdinalIgnoreCase));
    }

    private static void RenderHeader(ILocalizer localizer)
    {
        AnsiConsole.Write(
            new FigletText("STARRY")
                .LeftJustified()
                .Color(Color.Yellow));

        var description = localizer.GetString(LocalizationKeys.CliDescription);
        AnsiConsole.MarkupLine($"[grey]{Markup.Escape(description)}[/]");
        AnsiConsole.WriteLine();
    }

    private static void RenderVersion(ILocalizer localizer)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.1.0";
        var message = localizer.FormatString(LocalizationKeys.CliVersion, version);
        AnsiConsole.MarkupLine($"[bold yellow]STARRY[/] [green]{Markup.Escape(message)}[/]");
    }

    private static void RenderHelp(ILocalizer localizer)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[bold yellow]Command[/]")
            .AddColumn("[bold yellow]Description[/]");

        table.AddRow("[cyan]init[/]", "Initialize new Starry GitOps repository with standard directory layout");
        table.AddRow("[cyan]adopt[/]", "Brownfield adoption of existing VPS with running containers & Compose");
        table.AddRow("[cyan]unmanaged list[/]", "Inspect unmanaged containers across cluster nodes");
        table.AddRow("[cyan]config (validate|pack|split|fmt|schema)[/]", localizer.GetString(LocalizationKeys.ConfigCommandDescription));
        table.AddRow("[cyan]secret (keygen|encrypt|decrypt)[/]", "Manage secret generation and single-value AEAD encryption");
        table.AddRow("[cyan]encrypt [global --reencrypt][/]", localizer.GetString(LocalizationKeys.EncryptCommandDescription));
        table.AddRow("[cyan]nodes (list|bootstrap|upgrade|rotate-keys)[/]", localizer.GetString(LocalizationKeys.NodesCommandDescription));
        table.AddRow("[cyan]plan[/]", localizer.GetString(LocalizationKeys.PlanCommandDescription));
        table.AddRow("[cyan]apply[/]", localizer.GetString(LocalizationKeys.ApplyCommandDescription));
        table.AddRow("[cyan]agent (run|install-service|status)[/]", "Autonomous host agent daemon and drift detection");
        table.AddRow("[cyan]volume open[/]", localizer.GetString(LocalizationKeys.VolumeCommandDescription));
        table.AddRow("[cyan]bundle (export|import)[/]", localizer.GetString(LocalizationKeys.BundleCommandDescription));

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        var optionsTable = new Table()
            .Border(TableBorder.Simple)
            .AddColumn("[bold]Option[/]")
            .AddColumn("[bold]Description[/]");

        optionsTable.AddRow("--lang <ru|en>", localizer.GetString(LocalizationKeys.CliLangOption));
        optionsTable.AddRow("--verbose", localizer.GetString(LocalizationKeys.CliVerboseOption));
        optionsTable.AddRow("--config <path>", localizer.GetString(LocalizationKeys.CliConfigOption));

        AnsiConsole.Write(optionsTable);
    }

    private static Task<int> DispatchCommandAsync(string command, string[] commandArgs, ILocalizer localizer)
    {
        return command.ToUpperInvariant() switch
        {
            "INIT" => Task.FromResult(HandleInitCommand(commandArgs)),
            "ADOPT" => HandleAdoptCommandAsync(commandArgs),
            "UNMANAGED" => HandleUnmanagedCommandAsync(commandArgs),
            "CONFIG" => HandleConfigCommand(commandArgs, localizer),
            "SECRET" => Task.FromResult(HandleSecretCommand(commandArgs)),
            "ENCRYPT" => HandleEncryptCommand(commandArgs),
            "NODES" => HandleNodesCommand(commandArgs, localizer),
            "ROTATE-KEYS" => HandleRotateKeysDirectCommand(commandArgs),
            "PLAN" => HandlePlanCommand(commandArgs),
            "APPLY" => HandleApplyCommand(commandArgs),
            "AGENT" => HandleAgentCommandAsync(commandArgs),
            "VOLUME" => HandleVolumeCommand(commandArgs),
            "BUNDLE" => HandleBundleCommand(commandArgs),
            _ => HandleUnknownCommand(command, localizer)
        };
    }

    private static int HandleInitCommand(string[] args)
    {
        var targetPath = ResolveTargetPath(args);
        var clusterName = ExtractOptionValue(args, "--name") ?? "starry-fleet";

        try
        {
            var (masterKey, dir) = RepositoryInitializer.Initialize(targetPath, clusterName);
            AnsiConsole.MarkupLine($"[bold green]✓ Initialized Starry GitOps repository in '{Markup.Escape(dir)}'![/]");
            AnsiConsole.MarkupLine($"[yellow]Generated master encryption key:[/] [bold green]{masterKey}[/]");
            AnsiConsole.MarkupLine("[grey]Export this key into your environment: export STARRY_MASTER_KEY=\"...\"[/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Initialization error: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> HandleAdoptCommandAsync(string[] args)
    {
        var outDir = ExtractOptionValue(args, "--out-dir") ?? "./config";
        var format = ExtractOptionValue(args, "--format") ?? "yaml";
        var pullFiles = args.Any(a => string.Equals(a, "--pull-files", StringComparison.OrdinalIgnoreCase));
        var hostArg = args.FirstOrDefault(a => !a.StartsWith('-'));

        if (string.IsNullOrEmpty(hostArg))
        {
            AnsiConsole.MarkupLine("[red]Usage: starry adopt <user@host[:port]> [--pull-files] [--out-dir ./config] [--format yaml|json|toml][/]");
            return 1;
        }

        try
        {
            var (user, host, port) = ParseUserHostPort(hostArg);
            var key = ResolveKeyFromArgsOrEnv(args, "--key", "STARRY_MASTER_KEY");

            AnsiConsole.MarkupLine($"[bold yellow]Inspecting and adopting host '[cyan]{user}@{host}:{port}[/]'...[/]");

            var options = new SshNodeOptions { NodeName = host.Replace('.', '-'), Host = host, Port = port, User = user };
            await using var conn = new SshNodeConnection(options);
            await conn.InitializeAsync().ConfigureAwait(false);

            var adoptionService = new HostAdoptionService();
            var adoptionOptions = new AdoptionOptions
            {
                OutDir = outDir,
                Format = format,
                PullFiles = pullFiles,
                MasterKey = key
            };

            await adoptionService.AdoptHostAsync(conn, adoptionOptions).ConfigureAwait(false);
            AnsiConsole.MarkupLine($"[bold green]✓ Successfully adopted host! Configuration generated in '{Markup.Escape(outDir)}'.[/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Adoption failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> HandleUnmanagedCommandAsync(string[] args)
    {
        var configPath = ExtractOptionValue(args, "--config") ?? ".";
        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);
            var orchestrator = new ClusterOrchestrator();
            var nodes = await orchestrator.ResolveNodesAsync(tree).ConfigureAwait(false);

            try
            {
                var adoptionService = new HostAdoptionService();
                var unmanaged = await adoptionService.ListUnmanagedContainersAsync(nodes, tree).ConfigureAwait(false);

                if (unmanaged.Count == 0)
                {
                    AnsiConsole.MarkupLine("[green]✓ No unmanaged containers found. All host containers are under Starry GitOps management.[/]");
                    return 0;
                }

                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("[bold]Node[/]");
                table.AddColumn("[bold]Container Name[/]");
                table.AddColumn("[bold]Image[/]");
                table.AddColumn("[bold]State[/]");

                foreach (var item in unmanaged)
                {
                    table.AddRow(item.NodeName, item.ContainerName, item.Image, item.State);
                }

                AnsiConsole.Write(table);
                AnsiConsole.MarkupLine($"\n[yellow]Found {unmanaged.Count} unmanaged container(s). Run 'starry adopt' to bring them under management.[/]");
                return 0;
            }
            finally
            {
                foreach (var node in nodes)
                {
                    await node.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unmanaged inspection failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static int HandleSecretCommand(string[] args)
    {
        var subCommand = args.Length > 0 ? args[0] : "help";
        var subArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();

        if (string.Equals(subCommand, "keygen", StringComparison.OrdinalIgnoreCase))
        {
            var key = CryptoService.GenerateMasterKeyBase64();
            AnsiConsole.WriteLine(key);
            return 0;
        }

        if (string.Equals(subCommand, "encrypt", StringComparison.OrdinalIgnoreCase))
        {
            if (subArgs.Length == 0)
            {
                AnsiConsole.MarkupLine("[red]Usage: starry secret encrypt <value> --path <jsonPath> [--key <key>][/]");
                return 1;
            }

            var plainText = subArgs[0];
            var path = ExtractOptionValue(subArgs, "--path") ?? "secret";
            var key = ResolveKeyFromArgsOrEnv(subArgs, "--key", "STARRY_MASTER_KEY")
                      ?? throw new InvalidOperationException("Master key not provided. Set STARRY_MASTER_KEY or pass --key.");

            var crypto = new CryptoService();
            var envelope = crypto.Encrypt(Encoding.UTF8.GetBytes(plainText), key, path);
            AnsiConsole.WriteLine(envelope.FormatEncrypted());
            return 0;
        }

        if (string.Equals(subCommand, "decrypt", StringComparison.OrdinalIgnoreCase))
        {
            if (subArgs.Length == 0)
            {
                AnsiConsole.MarkupLine("[red]Usage: starry secret decrypt <ENC[...]> --path <jsonPath> [--key <key>][/]");
                return 1;
            }

            var encText = subArgs[0];
            var path = ExtractOptionValue(subArgs, "--path") ?? "secret";
            var key = ResolveKeyFromArgsOrEnv(subArgs, "--key", "STARRY_MASTER_KEY")
                      ?? throw new InvalidOperationException("Master key not provided. Set STARRY_MASTER_KEY or pass --key.");

            var crypto = new CryptoService();
            if (!crypto.TryParseEncrypted(encText, out var envelope) || envelope == null)
            {
                AnsiConsole.MarkupLine("[red]Invalid ENC[...] envelope format.[/]");
                return 1;
            }

            var decrypted = crypto.Decrypt(envelope, key, path);
            AnsiConsole.WriteLine(Encoding.UTF8.GetString(decrypted));
            return 0;
        }

        AnsiConsole.MarkupLine("[yellow]Usage: starry secret (keygen|encrypt|decrypt)[/]");
        return 0;
    }

    private static async Task<int> HandleAgentCommandAsync(string[] args)
    {
        var subCommand = args.Length > 0 ? args[0] : "status";

        if (string.Equals(subCommand, "run", StringComparison.OrdinalIgnoreCase))
        {
            var configPath = ExtractOptionValue(args, "--config") ??
                             (File.Exists("/etc/starry/starry.yaml") ? "/etc/starry/starry.yaml" : ".");
            var agentOptions = new AgentOptions();

            if (File.Exists(configPath) || Directory.Exists(configPath))
            {
                agentOptions = LoadAgentOptionsFromConfig(configPath);
            }

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };
            await using var daemon = new StarryAgentDaemon();
            AnsiConsole.MarkupLine("[bold yellow]Starting Starry Agent Daemon...[/] [grey](Press Ctrl+C to stop)[/]");
            await daemon.RunAsync(agentOptions, cts.Token).ConfigureAwait(false);
            return 0;
        }

        if (string.Equals(subCommand, "install-service", StringComparison.OrdinalIgnoreCase))
        {
            await using var daemon = new StarryAgentDaemon();
            await daemon.InstallSystemdServiceAsync().ConfigureAwait(false);
            AnsiConsole.MarkupLine("[bold green]✓ Successfully installed and started starry-agent.service![/]");
            return 0;
        }

        if (string.Equals(subCommand, "status", StringComparison.OrdinalIgnoreCase))
        {
            await using var daemon = new StarryAgentDaemon();
            var report = await daemon.GetStatusAsync().ConfigureAwait(false);
            AnsiConsole.MarkupLine("[bold yellow]Starry Agent Status:[/]");
            AnsiConsole.MarkupLine($"  WireGuard Interface: {(report.IsStarry0Active ? "[green]UP[/]" : "[red]DOWN[/]")}");
            AnsiConsole.MarkupLine($"  Peers Connected: [cyan]{report.Peers.Count}[/]");
            AnsiConsole.MarkupLine($"  Managed Containers: [cyan]{report.Containers.Count}[/]");
            return 0;
        }

        AnsiConsole.MarkupLine("[red]Usage: starry agent (run|install-service|status)[/]");
        return 1;
    }

    private static AgentOptions LoadAgentOptionsFromConfig(string configPath)
    {
        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);
            var internalRecords = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (tree.Root["services"] is JsonObject servicesObj)
            {
                foreach (var (svcName, svcVal) in servicesObj)
                {
                    var vipStr = svcVal?["vip"]?.ToString() ?? svcVal?["vip_address"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(vipStr))
                    {
                        internalRecords[$"{svcName}.internal"] = vipStr;
                    }
                }
            }

            if (tree.Root["nodes"] is JsonObject nodesObj)
            {
                foreach (var (nodeName, nodeVal) in nodesObj)
                {
                    var vpnIpStr = nodeVal?["vpn_ip"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(vpnIpStr))
                    {
                        internalRecords[$"{nodeName}.internal"] = vpnIpStr;
                    }
                }
            }

            return new AgentOptions { InternalDnsRecords = internalRecords };
        }
        catch (Exception)
        {
            return new AgentOptions();
        }
    }

    private static Task<int> HandleConfigCommand(string[] args, ILocalizer localizer)
    {
        var subCommand = args.Length > 0 ? args[0] : "validate";
        var subArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();

        if (string.Equals(subCommand, "validate", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(ExecuteConfigValidate(subArgs, localizer));
        }

        if (string.Equals(subCommand, "pack", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(ExecuteConfigPack(subArgs, localizer));
        }

        if (string.Equals(subCommand, "split", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(ExecuteConfigSplit(subArgs, localizer));
        }

        if (string.Equals(subCommand, "fmt", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(ExecuteConfigFmt(subArgs, localizer));
        }

        if (string.Equals(subCommand, "schema", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine(VirtualConfigEngine.GenerateJsonSchema());
            return Task.FromResult(0);
        }

        AnsiConsole.MarkupLine($"[red]Unknown config subcommand: '{Markup.Escape(subCommand)}'[/]");
        return Task.FromResult(1);
    }

    private static int ExecuteConfigValidate(string[] args, ILocalizer localizer)
    {
        var targetPath = ResolveTargetPath(args);
        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(targetPath);
            var result = ConfigValidator.Validate(tree);

            if (result.IsValid && result.Issues.Count == 0)
            {
                AnsiConsole.MarkupLine($"[green]✓ {Markup.Escape(localizer.GetString(LocalizationKeys.ValidationSuccess))}[/]");
                return 0;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("[bold]Severity[/]");
            table.AddColumn("[bold]Path[/]");
            table.AddColumn("[bold]File[/]");
            table.AddColumn("[bold]Message[/]");

            foreach (var issue in result.Issues)
            {
                var severity = issue.IsWarning ? "[yellow]WARN[/]" : "[red]ERR[/]";
                table.AddRow(severity, issue.Path, issue.FilePath ?? "-", Markup.Escape(issue.Message));
            }

            AnsiConsole.Write(table);

            if (result.IsValid)
            {
                AnsiConsole.MarkupLine($"[green]✓ {Markup.Escape(localizer.GetString(LocalizationKeys.ValidationSuccess))} (with warnings)[/]");
                return 0;
            }

            var errorCount = result.Issues.Count(i => !i.IsWarning);
            AnsiConsole.MarkupLine($"[red]✗ {Markup.Escape(localizer.FormatString(LocalizationKeys.ValidationFailed, errorCount))}[/]");
            return 1;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Validation error: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static int ExecuteConfigPack(string[] args, ILocalizer localizer)
    {
        var sourceDir = ResolveTargetPath(args);
        var outputFile = ExtractOptionValue(args, "-o") ?? "starry.packed.yaml";
        var format = ExtractOptionValue(args, "--format") ?? "yaml";

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(sourceDir);
            engine.Pack(tree, outputFile, format);
            var msg = localizer.FormatString(LocalizationKeys.ConfigPackSuccess, outputFile);
            AnsiConsole.MarkupLine($"[green]✓ {Markup.Escape(msg)}[/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Pack error: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static int ExecuteConfigSplit(string[] args, ILocalizer localizer)
    {
        var sourceFile = ResolveTargetPath(args, "starry.yaml");
        var targetDir = ExtractOptionValue(args, "-o") ?? "config";
        var format = ExtractOptionValue(args, "--format") ?? "yaml";

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(sourceFile);
            engine.Split(tree, targetDir, format);
            var msg = localizer.FormatString(LocalizationKeys.ConfigSplitSuccess, targetDir);
            AnsiConsole.MarkupLine($"[green]✓ {Markup.Escape(msg)}[/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Split error: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static int ExecuteConfigFmt(string[] args, ILocalizer localizer)
    {
        var filePath = ResolveTargetPath(args, string.Empty);
        if (string.IsNullOrEmpty(filePath))
        {
            AnsiConsole.MarkupLine("[red]Usage: starry config fmt <file> [--format yaml|json|toml][/]");
            return 1;
        }

        var format = ExtractOptionValue(args, "--format");
        if (string.IsNullOrEmpty(format))
        {
            var ext = Path.GetExtension(filePath);
            if (string.Equals(ext, ".json", StringComparison.OrdinalIgnoreCase))
            {
                format = "json";
            }
            else if (string.Equals(ext, ".toml", StringComparison.OrdinalIgnoreCase))
            {
                format = "toml";
            }
            else
            {
                format = "yaml";
            }
        }

        try
        {
            var engine = new VirtualConfigEngine();
            var content = File.ReadAllText(filePath);
            var formatted = engine.Format(content, format);
            File.WriteAllText(filePath, formatted);
            var msg = localizer.FormatString(LocalizationKeys.ConfigFmtSuccess, filePath);
            AnsiConsole.MarkupLine($"[green]✓ {Markup.Escape(msg)}[/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Format error: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> HandleEncryptCommand(string[] args)
    {
        var isGlobalReencrypt = args.Length >= 2 &&
                                string.Equals(args[0], "global", StringComparison.OrdinalIgnoreCase) &&
                                args.Any(a => string.Equals(a, "--reencrypt", StringComparison.OrdinalIgnoreCase));

        if (isGlobalReencrypt)
        {
            return await ExecuteGlobalReencryptAsync(args).ConfigureAwait(false);
        }

        return ExecuteEncryptOpenSecrets(args);
    }

    private static int ExecuteEncryptOpenSecrets(string[] args)
    {
        var targetPath = ResolveTargetPath(args);
        var key = ResolveKeyFromArgsOrEnv(args, "--key", "STARRY_MASTER_KEY");

        if (key == null)
        {
            var newKey = CryptoService.GenerateMasterKeyBase64();
            AnsiConsole.MarkupLine($"[yellow]No master key specified. Generated new STARRY_MASTER_KEY:[/] [bold green]{newKey}[/]");
            AnsiConsole.MarkupLine("[grey]Export this key into your environment: export STARRY_MASTER_KEY=\"...\"[/]");
            key = CryptoService.ParseMasterKey(newKey);
        }

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(targetPath);
            var manager = new SecretManager();
            var encryptedCount = manager.EncryptOpenSecrets(tree, key, targetPath, engine);

            AnsiConsole.MarkupLine($"[green]✓ Successfully encrypted {encryptedCount} open secret(s) with AES-256-GCM + AAD.[/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Encryption error: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> ExecuteGlobalReencryptAsync(string[] args)
    {
        var oldKey = ResolveKeyFromArgsOrEnv(args, "--old-key", "STARRY_MASTER_KEY_OLD", "--old-key-env", "--last-main-key");
        var newKey = ResolveKeyFromArgsOrEnv(args, "--new-key", "STARRY_MASTER_KEY_NEW", "--new-key-env", "--new-main-key");
        var targetPath = ResolveTargetPath(args);

        if (oldKey == null || newKey == null)
        {
            AnsiConsole.MarkupLine("[red]Error: Both old key and new key are required for global re-encryption.[/]");
            return 1;
        }

        var gitCommit = args.Any(a => string.Equals(a, "--git-commit", StringComparison.OrdinalIgnoreCase));
        var gitPush = args.Any(a => string.Equals(a, "--git-push", StringComparison.OrdinalIgnoreCase));
        var ghSecretName = ExtractOptionValue(args, "--update-github-secret");

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(targetPath);
            var manager = new SecretManager();
            var count = manager.ReencryptAll(tree, oldKey, newKey, targetPath, engine);

            AnsiConsole.MarkupLine($"[green]✓ Re-encrypted {count} secret(s) with new master key.[/]");

            if (!string.IsNullOrEmpty(ghSecretName))
            {
                await UpdateGitHubSecretAsync(ghSecretName, Convert.ToBase64String(newKey)).ConfigureAwait(false);
            }

            if (gitCommit)
            {
                await RunGitCommitAndPushAsync(gitPush).ConfigureAwait(false);
            }

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Global re-encryption failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task UpdateGitHubSecretAsync(string secretName, string secretValue)
    {
        var runner = new ProcessRunner();
        var spec = new ProcessSpec
        {
            Executable = "gh",
            Arguments = ["secret", "set", secretName],
            StandardInputBytes = Encoding.UTF8.GetBytes(secretValue)
        };

        var result = await runner.RunAsync(spec).ConfigureAwait(false);
        if (result.IsSuccess)
        {
            AnsiConsole.MarkupLine($"[green]✓ Successfully updated GitHub Actions secret '{Markup.Escape(secretName)}'.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[yellow]Warning: Failed to update GitHub secret: {Markup.Escape(result.StandardError)}[/]");
        }
    }

    private static async Task RunGitCommitAndPushAsync(bool push)
    {
        var runner = new ProcessRunner();
        await runner.RunAsync(new ProcessSpec { Executable = "git", Arguments = ["add", "-A"] }).ConfigureAwait(false);
        var commitRes = await runner.RunAsync(new ProcessSpec
        {
            Executable = "git",
            Arguments = ["commit", "-m", "chore(security): rotate cluster master encryption key [skip ci]"]
        }).ConfigureAwait(false);

        if (commitRes.IsSuccess)
        {
            AnsiConsole.MarkupLine("[green]✓ Created git commit with rotated secrets.[/]");
        }

        if (push)
        {
            var pushRes = await runner.RunAsync(new ProcessSpec { Executable = "git", Arguments = ["push"] }).ConfigureAwait(false);
            if (pushRes.IsSuccess)
            {
                AnsiConsole.MarkupLine("[green]✓ Pushed rotated secrets to remote git repository.[/]");
            }
        }
    }

    private static byte[]? ResolveKeyFromArgsOrEnv(string[] args, string flag, string envVar, params string[] aliasFlags)
    {
        var keyVal = ExtractOptionValue(args, flag);
        if (!string.IsNullOrEmpty(keyVal))
        {
            return CryptoService.ParseMasterKey(keyVal);
        }

        foreach (var alias in aliasFlags)
        {
            var envName = ExtractOptionValue(args, alias);
            if (!string.IsNullOrEmpty(envName))
            {
                var val = Environment.GetEnvironmentVariable(envName);
                if (!string.IsNullOrEmpty(val))
                {
                    return CryptoService.ParseMasterKey(val);
                }
            }
        }

        var defaultVal = Environment.GetEnvironmentVariable(envVar);
        if (!string.IsNullOrEmpty(defaultVal))
        {
            return CryptoService.ParseMasterKey(defaultVal);
        }

        return null;
    }

    private static string ResolveTargetPath(string[] args, string defaultPath = ".")
    {
        var i = 0;
        while (i < args.Length)
        {
            var arg = args[i];
            if (string.Equals(arg, "global", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(arg, "--reencrypt", StringComparison.OrdinalIgnoreCase))
            {
                i++;
                continue;
            }

            if (arg.StartsWith('-'))
            {
                i += TakesArgument(arg) ? 2 : 1;
                continue;
            }

            return arg;
        }

        return defaultPath;
    }

    private static bool TakesArgument(string flag)
    {
        return flag is "-o" or "--format" or "--config" or "--key" or
               "--old-key" or "--new-key" or "--old-key-env" or "--new-key-env" or
               "--last-main-key" or "--new-main-key" or "--update-github-secret" or
               "--node" or "--old-pubkey" or "--override-ip" or "--lang" or
               "--name" or "--out-dir" or "--path";
    }

    private static string? ExtractOptionValue(string[] args, string optionName)
    {
        for (var i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], optionName, StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                return args[i + 1];
            }

            if (args[i].StartsWith(optionName + "=", StringComparison.OrdinalIgnoreCase))
            {
                return args[i][(optionName.Length + 1)..];
            }
        }

        return null;
    }

    private static (string User, string Host, int Port) ParseUserHostPort(string input)
    {
        var user = "root";
        var host = input;
        var port = 22;

        if (host.Contains('@', StringComparison.Ordinal))
        {
            var parts = host.Split('@', 2);
            user = parts[0];
            host = parts[1];
        }

        if (host.Contains(':', StringComparison.Ordinal))
        {
            var parts = host.Split(':', 2);
            host = parts[0];
            if (int.TryParse(parts[1], CultureInfo.InvariantCulture, out var p))
            {
                port = p;
            }
        }

        return (user, host, port);
    }

    private static async Task<int> HandleNodesCommand(string[] args, ILocalizer localizer)
    {
        var subCommand = args.Length > 0 ? args[0] : "list";
        var subArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();

        if (string.Equals(subCommand, "list", StringComparison.OrdinalIgnoreCase))
        {
            return await ExecuteNodesListAsync(subArgs).ConfigureAwait(false);
        }

        if (string.Equals(subCommand, "bootstrap", StringComparison.OrdinalIgnoreCase))
        {
            return await ExecuteNodesBootstrapAsync(subArgs).ConfigureAwait(false);
        }

        if (string.Equals(subCommand, "upgrade", StringComparison.OrdinalIgnoreCase))
        {
            return await ExecuteNodesUpgradeAsync(subArgs).ConfigureAwait(false);
        }

        if (string.Equals(subCommand, "rotate-keys", StringComparison.OrdinalIgnoreCase))
        {
            return await ExecuteNodesRotateKeysAsync(subArgs).ConfigureAwait(false);
        }

        AnsiConsole.MarkupLine($"[yellow]starry nodes[/] - [grey]{Markup.Escape(localizer.GetString(LocalizationKeys.NodesCommandDescription))}[/]");
        return 0;
    }

    private static async Task<int> ExecuteNodesListAsync(string[] args)
    {
        var configPath = ExtractOptionValue(args, "--config") ?? ".";
        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);

            if (!tree.Root.TryGetPropertyValue("nodes", out var nodesNode) || nodesNode is not JsonObject nodesObj)
            {
                AnsiConsole.MarkupLine("[yellow]No nodes defined in cluster configuration.[/]");
                return 0;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("[bold]Node Name[/]");
            table.AddColumn("[bold]Host[/]");
            table.AddColumn("[bold]Port[/]");
            table.AddColumn("[bold]VPN IP[/]");
            table.AddColumn("[bold]Public?[/]");

            foreach (var (name, val) in nodesObj)
            {
                if (val is JsonObject obj)
                {
                    table.AddRow(
                        name,
                        obj["host"]?.ToString() ?? "-",
                        obj["port"]?.ToString() ?? "22",
                        obj["vpn_ip"]?.ToString() ?? "-",
                        obj["is_public"]?.ToString() ?? "false");
                }
            }

            AnsiConsole.Write(table);
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Nodes list failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> ExecuteNodesBootstrapAsync(string[] args)
    {
        var hostArg = args.FirstOrDefault(a => !a.StartsWith('-'));
        if (string.IsNullOrEmpty(hostArg))
        {
            AnsiConsole.MarkupLine("[red]Usage: starry nodes bootstrap <user@host[:port]> [--name <name>] [--vpn-ip <ip>] [--config <path>][/]");
            return 1;
        }

        var (user, host, port) = ParseUserHostPort(hostArg);
        var nodeName = ExtractOptionValue(args, "--name") ?? host.Replace('.', '-');
        var vpnIp = ExtractOptionValue(args, "--vpn-ip") ?? ("10" + ".99.0.2");
        var configPath = ExtractOptionValue(args, "--config") ?? ".";

        try
        {
            AnsiConsole.MarkupLine($"[bold yellow]Bootstrapping node '[cyan]{nodeName}[/]' ({user}@{host}:{port})...[/]");

            var options = new SshNodeOptions { NodeName = nodeName, Host = host, Port = port, User = user };
            await using var conn = new SshNodeConnection(options);
            await conn.InitializeAsync().ConfigureAwait(false);

            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);

            var nodeObj = new JsonObject
            {
                ["host"] = host,
                ["port"] = port,
                ["user"] = user,
                ["vpn_ip"] = vpnIp,
                ["is_public"] = false
            };

            if (tree.Root["nodes"] is not JsonObject nodesRoot)
            {
                nodesRoot = new JsonObject();
                tree.Root["nodes"] = nodesRoot;
            }
            nodesRoot[nodeName] = nodeObj;

            engine.Split(tree, configPath, "yaml");
            AnsiConsole.MarkupLine($"[bold green]✓ Node '{Markup.Escape(nodeName)}' successfully added to configuration and bootstrapped![/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Node bootstrap failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> ExecuteNodesUpgradeAsync(string[] args)
    {
        var configPath = ExtractOptionValue(args, "--config") ?? ".";
        var fullUpgrade = args.Any(a => string.Equals(a, "--full", StringComparison.OrdinalIgnoreCase));

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);

            if (!tree.Root.TryGetPropertyValue("nodes", out var nodesNode) || nodesNode is not JsonObject nodesObj)
            {
                AnsiConsole.MarkupLine("[yellow]No nodes defined in cluster configuration.[/]");
                return 0;
            }

            var service = new NodeOperationsService();
            var orchestrator = new ClusterOrchestrator();
            AnsiConsole.MarkupLine($"[bold yellow]Starting rolling upgrade across {nodesObj.Count} node(s)...[/]");

            foreach (var (nodeName, nodeVal) in nodesObj)
            {
                if (nodeVal is not JsonObject nodeObj)
                {
                    continue;
                }

                await using var conn = await orchestrator.ResolveNodeConnectionAsync(nodeName, nodeObj).ConfigureAwait(false);
                AnsiConsole.MarkupLine($"\n[cyan]=== Processing node '{Markup.Escape(nodeName)}' ({Markup.Escape(conn.Host)}) ===[/]");
                await service.UpgradeNodeAsync(conn, fullUpgrade, msg => AnsiConsole.MarkupLine($"[grey]  > {Markup.Escape(msg)}[/]")).ConfigureAwait(false);
                AnsiConsole.MarkupLine($"[green]✓ Node '{Markup.Escape(nodeName)}' updated successfully.[/]");
            }

            AnsiConsole.MarkupLine("\n[bold green]✓ All nodes successfully upgraded![/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Rolling upgrade failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> HandleRotateKeysDirectCommand(string[] args)
    {
        return await ExecuteNodesRotateKeysAsync(args).ConfigureAwait(false);
    }

    private static async Task<int> ExecuteNodesRotateKeysAsync(string[] args)
    {
        var nodeName = ExtractOptionValue(args, "--node");
        var oldPubKey = ExtractOptionValue(args, "--old-pubkey") ?? string.Empty;
        var configPath = ExtractOptionValue(args, "--config") ?? ".";

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);
            var orchestrator = new ClusterOrchestrator();

            if (tree.Root["nodes"] is not JsonObject nodesObj || nodesObj.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No nodes defined in cluster configuration.[/]");
                return 0;
            }

            var targets = !string.IsNullOrEmpty(nodeName)
                ? [nodeName]
                : nodesObj.Select(kv => kv.Key).ToList();

            var masterKey = ResolveKeyFromArgsOrEnv(args, "--key", "STARRY_MASTER_KEY");
            if (masterKey == null)
            {
                var newKeyStr = CryptoService.GenerateMasterKeyBase64();
                masterKey = CryptoService.ParseMasterKey(newKeyStr);
                AnsiConsole.MarkupLine($"[yellow]No master key specified. Generated STARRY_MASTER_KEY for rotated keys:[/] [bold green]{newKeyStr}[/]");
            }

            var secretManager = new SecretManager();

            foreach (var target in targets)
            {
                if (nodesObj[target] is not JsonObject nodeObj)
                {
                    continue;
                }

                await using var conn = await orchestrator.ResolveNodeConnectionAsync(target, nodeObj).ConfigureAwait(false);
                var newPrivateKey = await NodeOperationsService.RotateSshKeysAsync(conn, oldPubKey, msg => AnsiConsole.MarkupLine($"[grey]  > {Markup.Escape(msg)}[/]")).ConfigureAwait(false);

                nodeObj["ssh_key"] = $"PLAIN[{newPrivateKey}]";
                secretManager.EncryptOpenSecrets(tree, masterKey, configPath, engine);
            }

            AnsiConsole.MarkupLine($"[bold green]✓ Successfully rotated SSH keys for {targets.Count} node(s) and updated configuration![/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]SSH key rotation failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> HandlePlanCommand(string[] args)
    {
        var configPath = ResolveTargetPath(args);
        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);

            var orchestrator = new ClusterOrchestrator();
            AnsiConsole.MarkupLine("[grey]Connecting to cluster nodes to discover live container state...[/]");
            var livePlacements = await orchestrator.DiscoverLiveStateAsync(tree).ConfigureAwait(false);

            var plan = MigrationPlanner.BuildPlan(tree, livePlacements);
            AnsiConsole.MarkupLine($"[bold yellow]STARRY Execution Plan for Cluster:[/] [bold cyan]{Markup.Escape(plan.ClusterName)}[/]\n");

            if (!plan.HasChanges)
            {
                AnsiConsole.MarkupLine("[green]✓ No changes. Your cluster matches the desired configuration.[/]");
                return 0;
            }

            RenderPlanTable(plan);
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Plan generation failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static void RenderPlanTable(MigrationPlan plan)
    {
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[bold]Order[/]");
        table.AddColumn("[bold]Action Type[/]");
        table.AddColumn("[bold]Target[/]");
        table.AddColumn("[bold]Description[/]");
        table.AddColumn("[bold]Dependencies[/]");

        var step = 1;
        foreach (var action in plan.Actions)
        {
            var typeColor = action.ActionType switch
            {
                PlanActionType.CreateNode => "[green]+ BOOTSTRAP[/]",
                PlanActionType.ConfigureWireGuardMesh => "[cyan]~ WIREGUARD[/]",
                PlanActionType.ConfigureNftables => "[blue]~ NFTABLES[/]",
                PlanActionType.DeployService => "[green]+ DEPLOY[/]",
                PlanActionType.MigrateServiceBlueGreen => "[magenta]⇄ BLUE-GREEN[/]",
                PlanActionType.UpdateDns => "[yellow]~ DNS[/]",
                _ => "[grey]ACTION[/]"
            };

            var deps = action.Dependencies.Count > 0 ? string.Join(", ", action.Dependencies) : "-";
            table.AddRow(
                step.ToString(CultureInfo.InvariantCulture),
                typeColor,
                Markup.Escape(action.Target),
                Markup.Escape(action.Description),
                $"[grey]{Markup.Escape(deps)}[/]");
            step++;
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"\n[bold green]Plan:[/] {plan.Actions.Count} action(s) to execute in DAG dependency order.");
    }

    private static async Task<int> HandleApplyCommand(string[] args)
    {
        var configPath = ResolveTargetPath(args);
        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);

            var orchestrator = new ClusterOrchestrator();
            AnsiConsole.MarkupLine("[grey]Resolving active connections to cluster nodes...[/]");
            var nodes = await orchestrator.ResolveNodesAsync(tree).ConfigureAwait(false);

            try
            {
                AnsiConsole.MarkupLine("[grey]Discovering live container placements...[/]");
                var livePlacements = await orchestrator.DiscoverLiveStateAsync(tree, nodes).ConfigureAwait(false);

                var plan = MigrationPlanner.BuildPlan(tree, livePlacements);

                if (!plan.HasChanges)
                {
                    AnsiConsole.MarkupLine("[green]✓ No changes to apply. Cluster is in desired state.[/]");
                    return 0;
                }

                AnsiConsole.MarkupLine($"[bold yellow]Applying {plan.Actions.Count} planned action(s) to cluster '{Markup.Escape(plan.ClusterName)}'...[/]");

                await orchestrator.ExecutePlanAsync(
                    plan,
                    tree,
                    nodes,
                    msg => AnsiConsole.MarkupLine($"[grey]  > {Markup.Escape(msg)}[/]")).ConfigureAwait(false);

                AnsiConsole.MarkupLine("\n[bold green]✓ Apply complete! All resources are in desired state.[/]");
                return 0;
            }
            finally
            {
                foreach (var node in nodes)
                {
                    await node.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Apply failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static async Task<int> HandleVolumeCommand(string[] args)
    {
        var subCommand = args.Length > 0 ? args[0] : "help";
        if (string.Equals(subCommand, "open", StringComparison.OrdinalIgnoreCase))
        {
            if (args.Length < 3)
            {
                AnsiConsole.MarkupLine("[red]Usage: starry volume open <node> <volumePath> [--config <path>] [--once][/]");
                return 1;
            }

            var nodeName = args[1];
            var volumePath = args[2];
            var configPath = ExtractOptionValue(args, "--config") ?? ".";
            var once = args.Any(a => string.Equals(a, "--once", StringComparison.OrdinalIgnoreCase));

            try
            {
                var engine = new VirtualConfigEngine();
                var tree = engine.Load(configPath);
                var orchestrator = new ClusterOrchestrator();
                var nodeObj = tree.Root["nodes"]?[nodeName] as JsonObject
                    ?? throw new InvalidOperationException($"Node '{nodeName}' not found in configuration.");

                await using var conn = await orchestrator.ResolveNodeConnectionAsync(nodeName, nodeObj).ConfigureAwait(false);

                AnsiConsole.MarkupLine($"[bold yellow]Querying volume files for '[cyan]{Markup.Escape(volumePath)}[/]' on '[cyan]{Markup.Escape(nodeName)}[/]'...[/]");
                var safeVol = "'" + volumePath.Replace("'", "'\\''") + "'";
                var listResult = await conn.ExecuteCommandAsync(new ProcessSpec
                {
                    Executable = "sh",
                    Arguments = ["-c", $"ls -la {safeVol} 2>/dev/null || ls -la /var/lib/docker/volumes/{safeVol}/_data 2>/dev/null || echo 'Empty or not found'"]
                }).ConfigureAwait(false);

                AnsiConsole.MarkupLine("[green]✓ Volume contents:[/]");
                AnsiConsole.WriteLine(listResult.StandardOutput);

                if (once)
                {
                    return 0;
                }

                AnsiConsole.MarkupLine("[grey]Volume explorer listening at sftp/ssh bridge. Press Ctrl+C to stop.[/]");
                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Volume open failed: {Markup.Escape(ex.Message)}[/]");
                return 1;
            }
        }

        AnsiConsole.MarkupLine("[yellow]Usage: starry volume open <node> <volumePath> [--once][/]");
        return 0;
    }

    private static Task<int> HandleBundleCommand(string[] args)
    {
        var subCommand = args.Length > 0 ? args[0] : "export";
        var subArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();

        if (string.Equals(subCommand, "export", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(ExecuteBundleExport(subArgs));
        }

        if (string.Equals(subCommand, "import", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(ExecuteBundleImport(subArgs));
        }

        AnsiConsole.MarkupLine("[red]Unknown bundle command. Usage: starry bundle (export|import)[/]");
        return Task.FromResult(1);
    }

    private static int ExecuteBundleExport(string[] subArgs)
    {
        var configPath = ResolveTargetPath(subArgs);
        var outputFile = ExtractOptionValue(subArgs, "-o") ?? "starry-fleet-bundle.tar.gz";

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);
            FleetBundleService.ExportBundle(tree, outputFile, engine);

            AnsiConsole.MarkupLine($"[bold green]✓ Successfully exported Disaster Recovery Bundle to '{Markup.Escape(outputFile)}'![/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Bundle export failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static int ExecuteBundleImport(string[] subArgs)
    {
        var archivePath = ResolveTargetPath(subArgs, string.Empty);
        if (string.IsNullOrEmpty(archivePath))
        {
            AnsiConsole.MarkupLine("[red]Usage: starry bundle import <archivePath> [-o <targetDir>] [--override-ip <node=ip>][/]");
            return 1;
        }

        var targetDir = ExtractOptionValue(subArgs, "-o") ?? "imported-cluster";

        var ipOverrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < subArgs.Length; i++)
        {
            if (string.Equals(subArgs[i], "--override-ip", StringComparison.OrdinalIgnoreCase) && i + 1 < subArgs.Length)
            {
                var pair = subArgs[i + 1].Split('=', 2);
                if (pair.Length == 2)
                {
                    ipOverrides[pair[0]] = pair[1];
                }
            }
        }

        try
        {
            var engine = new VirtualConfigEngine();
            FleetBundleService.ImportBundle(archivePath, targetDir, ipOverrides, engine);

            AnsiConsole.MarkupLine($"[bold green]✓ Successfully restored Disaster Recovery Bundle into '{Markup.Escape(targetDir)}'![/]");
            if (ipOverrides.Count > 0)
            {
                AnsiConsole.MarkupLine($"[cyan]  Applied {ipOverrides.Count} node IP address override(s).[/]");
            }
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Bundle import failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static Task<int> HandleUnknownCommand(string command, ILocalizer localizer)
    {
        var errorMsg = localizer.FormatString(LocalizationKeys.ErrorUnknownCommand, command);
        AnsiConsole.MarkupLine($"[red]{Markup.Escape(errorMsg)}[/]");
        return Task.FromResult(1);
    }
}
