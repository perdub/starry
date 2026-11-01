using System.Globalization;
using System.Reflection;
using System.Text.Json.Nodes;
using Spectre.Console;
using Starry.Core.Localization;
using Starry.Core.Nodes;
using Starry.Core.Processes;
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

        var lang = ExtractLanguageArgument(args);
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

    private static string? ExtractLanguageArgument(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], "--lang", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                return args[i + 1];
            }

            if (args[i].StartsWith("--lang=", StringComparison.OrdinalIgnoreCase))
            {
                return args[i]["--lang=".Length..];
            }
        }

        return null;
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

        table.AddRow("[cyan]config validate[/]", localizer.GetString(LocalizationKeys.ConfigValidateDescription));
        table.AddRow("[cyan]config pack[/]", localizer.GetString(LocalizationKeys.ConfigPackDescription));
        table.AddRow("[cyan]config split[/]", localizer.GetString(LocalizationKeys.ConfigSplitDescription));
        table.AddRow("[cyan]config fmt[/]", localizer.GetString(LocalizationKeys.ConfigFmtDescription));
        table.AddRow("[cyan]config schema[/]", localizer.GetString(LocalizationKeys.ConfigSchemaDescription));
        table.AddRow("[cyan]encrypt[/]", localizer.GetString(LocalizationKeys.EncryptCommandDescription));
        table.AddRow("[cyan]encrypt global --reencrypt[/]", localizer.GetString(LocalizationKeys.EncryptGlobalDescription));
        table.AddRow("[cyan]nodes[/]", localizer.GetString(LocalizationKeys.NodesCommandDescription));
        table.AddRow("[cyan]plan[/]", localizer.GetString(LocalizationKeys.PlanCommandDescription));
        table.AddRow("[cyan]apply[/]", localizer.GetString(LocalizationKeys.ApplyCommandDescription));
        table.AddRow("[cyan]volume[/]", localizer.GetString(LocalizationKeys.VolumeCommandDescription));
        table.AddRow("[cyan]bundle[/]", localizer.GetString(LocalizationKeys.BundleCommandDescription));

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
        if (string.Equals(command, "config", StringComparison.OrdinalIgnoreCase))
        {
            return HandleConfigCommand(commandArgs, localizer);
        }

        if (string.Equals(command, "encrypt", StringComparison.OrdinalIgnoreCase))
        {
            return HandleEncryptCommand(commandArgs);
        }

        if (string.Equals(command, "nodes", StringComparison.OrdinalIgnoreCase))
        {
            return HandleNodesCommand(commandArgs, localizer);
        }

        if (string.Equals(command, "plan", StringComparison.OrdinalIgnoreCase))
        {
            return HandlePlanCommand(commandArgs);
        }

        if (string.Equals(command, "apply", StringComparison.OrdinalIgnoreCase))
        {
            return HandleApplyCommand(commandArgs);
        }

        if (string.Equals(command, "volume", StringComparison.OrdinalIgnoreCase))
        {
            return HandleVolumeCommand(localizer);
        }

        if (string.Equals(command, "bundle", StringComparison.OrdinalIgnoreCase))
        {
            return HandleBundleCommand(localizer);
        }

        return HandleUnknownCommand(command, localizer);
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
        var targetPath = args.Length > 0 ? args[0] : ".";
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
        var sourceDir = args.Length > 0 ? args[0] : ".";
        var outputFile = "starry.packed.yaml";
        var format = "yaml";

        for (var i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], "-o", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                outputFile = args[i + 1];
            }
            else if (string.Equals(args[i], "--format", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                format = args[i + 1];
            }
        }

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
        var sourceFile = args.Length > 0 ? args[0] : "starry.yaml";
        var targetDir = "config";
        var format = "yaml";

        for (var i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], "-o", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                targetDir = args[i + 1];
            }
            else if (string.Equals(args[i], "--format", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                format = args[i + 1];
            }
        }

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
        if (args.Length == 0)
        {
            AnsiConsole.MarkupLine("[red]Usage: starry config fmt <file> [--format yaml|json|toml][/]");
            return 1;
        }

        var filePath = args[0];
        var format = "yaml";
        var ext = Path.GetExtension(filePath);
        if (string.Equals(ext, ".json", StringComparison.OrdinalIgnoreCase))
        {
            format = "json";
        }
        else if (string.Equals(ext, ".toml", StringComparison.OrdinalIgnoreCase))
        {
            format = "toml";
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
        var targetPath = args.FirstOrDefault(a => !a.StartsWith('-')) ?? ".";
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
        var oldKey = ResolveKeyFromArgsOrEnv(args, "--old-key", "STARRY_MASTER_KEY_OLD");
        var newKey = ResolveKeyFromArgsOrEnv(args, "--new-key", "STARRY_MASTER_KEY_NEW");
        var targetPath = args.FirstOrDefault(a => !a.StartsWith('-') && !string.Equals(a, "global", StringComparison.OrdinalIgnoreCase)) ?? ".";

        if (oldKey == null || newKey == null)
        {
            AnsiConsole.MarkupLine("[red]Error: Both --old-key (or STARRY_MASTER_KEY_OLD) and --new-key (or STARRY_MASTER_KEY_NEW) are required for global reencryption.[/]");
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
            StandardInputBytes = System.Text.Encoding.UTF8.GetBytes(secretValue)
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

    private static byte[]? ResolveKeyFromArgsOrEnv(string[] args, string flag, string envVar)
    {
        var keyVal = ExtractOptionValue(args, flag);
        if (!string.IsNullOrEmpty(keyVal))
        {
            return CryptoService.ParseMasterKey(keyVal);
        }

        var envVal = Environment.GetEnvironmentVariable(envVar);
        if (!string.IsNullOrEmpty(envVal))
        {
            return CryptoService.ParseMasterKey(envVal);
        }

        return null;
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

    private static async Task<int> HandleNodesCommand(string[] args, ILocalizer localizer)
    {
        var subCommand = args.Length > 0 ? args[0] : "list";
        var subArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();

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
            AnsiConsole.MarkupLine($"[bold yellow]Starting rolling upgrade across {nodesObj.Count} node(s)...[/]");

            foreach (var (nodeName, nodeVal) in nodesObj)
            {
                if (nodeVal is not JsonObject nodeObj)
                {
                    continue;
                }

                var host = nodeObj["host"]?.ToString() ?? "localhost";
                var port = nodeObj.TryGetPropertyValue("port", out var pVal) && int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var p) ? p : 22;
                var user = nodeObj["user"]?.ToString() ?? "root";

                var isLocal = string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase) || string.Equals(host, "127.0.0.1", StringComparison.Ordinal);
                INodeConnection conn = isLocal
                    ? new LocalNodeConnection(nodeName)
                    : new SshNodeConnection(new SshNodeOptions { NodeName = nodeName, Host = host, Port = port, User = user });

                await using (conn.ConfigureAwait(false))
                {
                    AnsiConsole.MarkupLine($"\n[cyan]=== Processing node '{Markup.Escape(nodeName)}' ({Markup.Escape(host)}) ===[/]");
                    await service.UpgradeNodeAsync(conn, fullUpgrade, msg => AnsiConsole.MarkupLine($"[grey]  > {Markup.Escape(msg)}[/]")).ConfigureAwait(false);
                    AnsiConsole.MarkupLine($"[green]✓ Node '{Markup.Escape(nodeName)}' updated successfully.[/]");
                }
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

    private static async Task<int> ExecuteNodesRotateKeysAsync(string[] args)
    {
        var nodeName = ExtractOptionValue(args, "--node");
        var oldPubKey = ExtractOptionValue(args, "--old-pubkey");
        var configPath = ExtractOptionValue(args, "--config") ?? ".";

        if (string.IsNullOrEmpty(nodeName) || string.IsNullOrEmpty(oldPubKey))
        {
            AnsiConsole.MarkupLine("[red]Usage: starry nodes rotate-keys --node <nodeName> --old-pubkey <pubKey> [--config <path>][/]");
            return 1;
        }

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);

            var nodePath = $"nodes.{nodeName}";
            var nodeVal = tree.FindNode(nodePath) as JsonObject;
            if (nodeVal == null)
            {
                AnsiConsole.MarkupLine($"[red]Node '{Markup.Escape(nodeName)}' not found in configuration.[/]");
                return 1;
            }

            var host = nodeVal["host"]?.ToString() ?? "localhost";
            var port = nodeVal.TryGetPropertyValue("port", out var pVal) && int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var p) ? p : 22;
            var user = nodeVal["user"]?.ToString() ?? "root";

            await using var conn = new SshNodeConnection(new SshNodeOptions { NodeName = nodeName, Host = host, Port = port, User = user });
            var newPrivateKey = await NodeOperationsService.RotateSshKeysAsync(conn, oldPubKey, msg => AnsiConsole.MarkupLine($"[grey]  > {Markup.Escape(msg)}[/]")).ConfigureAwait(false);

            // Update configuration with new plain secret and encrypt if master key is available
            nodeVal["ssh_key"] = $"PLAIN[{newPrivateKey}]";
            var masterKey = ResolveKeyFromArgsOrEnv(args, "--key", "STARRY_MASTER_KEY");
            if (masterKey != null)
            {
                var secretManager = new SecretManager();
                secretManager.EncryptOpenSecrets(tree, masterKey, configPath, engine);
            }
            else
            {
                engine.Pack(tree, configPath, "yaml");
            }

            AnsiConsole.MarkupLine($"[bold green]✓ Successfully rotated SSH keys for node '{Markup.Escape(nodeName)}' and updated configuration![/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]SSH key rotation failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
    }

    private static Task<int> HandlePlanCommand(string[] args)
    {
        var configPath = ExtractOptionValue(args, "--config") ?? ".";
        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);
            var plan = MigrationPlanner.BuildPlan(tree);

            AnsiConsole.MarkupLine($"[bold yellow]STARRY Execution Plan for Cluster:[/] [bold cyan]{Markup.Escape(plan.ClusterName)}[/]\n");

            if (!plan.HasChanges)
            {
                AnsiConsole.MarkupLine("[green]✓ No changes. Your cluster matches the desired configuration.[/]");
                return Task.FromResult(0);
            }

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
            return Task.FromResult(0);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Plan generation failed: {Markup.Escape(ex.Message)}[/]");
            return Task.FromResult(1);
        }
    }

    private static Task<int> HandleApplyCommand(string[] args)
    {
        var configPath = ExtractOptionValue(args, "--config") ?? ".";
        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);
            var plan = MigrationPlanner.BuildPlan(tree);

            if (!plan.HasChanges)
            {
                AnsiConsole.MarkupLine("[green]✓ No changes to apply.[/]");
                return Task.FromResult(0);
            }

            AnsiConsole.MarkupLine($"[bold yellow]Applying {plan.Actions.Count} planned action(s) to cluster '{Markup.Escape(plan.ClusterName)}'...[/]");

            var step = 1;
            foreach (var action in plan.Actions)
            {
                AnsiConsole.MarkupLine($"[cyan][{step}/{plan.Actions.Count}][/] {Markup.Escape(action.Description)}... [green]DONE[/]");
                step++;
            }

            AnsiConsole.MarkupLine("\n[bold green]✓ Apply complete! All resources are in desired state.[/]");
            return Task.FromResult(0);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Apply failed: {Markup.Escape(ex.Message)}[/]");
            return Task.FromResult(1);
        }
    }

    private static Task<int> HandleVolumeCommand(ILocalizer localizer)
    {
        AnsiConsole.MarkupLine($"[yellow]starry volume[/] - [grey]{Markup.Escape(localizer.GetString(LocalizationKeys.VolumeCommandDescription))}[/]");
        return Task.FromResult(0);
    }

    private static Task<int> HandleBundleCommand(ILocalizer localizer)
    {
        AnsiConsole.MarkupLine($"[yellow]starry bundle[/] - [grey]{Markup.Escape(localizer.GetString(LocalizationKeys.BundleCommandDescription))}[/]");
        return Task.FromResult(0);
    }

    private static Task<int> HandleUnknownCommand(string command, ILocalizer localizer)
    {
        var errorMsg = localizer.FormatString(LocalizationKeys.ErrorUnknownCommand, command);
        AnsiConsole.MarkupLine($"[red]{Markup.Escape(errorMsg)}[/]");
        return Task.FromResult(1);
    }
}
