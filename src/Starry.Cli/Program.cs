using System.Reflection;
using Spectre.Console;
using Starry.Core.Localization;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Localization;

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
            return HandleEncryptCommand(localizer);
        }

        if (string.Equals(command, "nodes", StringComparison.OrdinalIgnoreCase))
        {
            return HandleNodesCommand(localizer);
        }

        if (string.Equals(command, "plan", StringComparison.OrdinalIgnoreCase))
        {
            return HandlePlanCommand(localizer);
        }

        if (string.Equals(command, "apply", StringComparison.OrdinalIgnoreCase))
        {
            return HandleApplyCommand(localizer);
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

    private static Task<int> HandleEncryptCommand(ILocalizer localizer)
    {
        AnsiConsole.MarkupLine($"[yellow]starry encrypt[/] - [grey]{Markup.Escape(localizer.GetString(LocalizationKeys.EncryptCommandDescription))}[/]");
        return Task.FromResult(0);
    }

    private static Task<int> HandleNodesCommand(ILocalizer localizer)
    {
        AnsiConsole.MarkupLine($"[yellow]starry nodes[/] - [grey]{Markup.Escape(localizer.GetString(LocalizationKeys.NodesCommandDescription))}[/]");
        return Task.FromResult(0);
    }

    private static Task<int> HandlePlanCommand(ILocalizer localizer)
    {
        AnsiConsole.MarkupLine($"[yellow]starry plan[/] - [grey]{Markup.Escape(localizer.GetString(LocalizationKeys.PlanCommandDescription))}[/]");
        return Task.FromResult(0);
    }

    private static Task<int> HandleApplyCommand(ILocalizer localizer)
    {
        AnsiConsole.MarkupLine($"[yellow]starry apply[/] - [grey]{Markup.Escape(localizer.GetString(LocalizationKeys.ApplyCommandDescription))}[/]");
        return Task.FromResult(0);
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
