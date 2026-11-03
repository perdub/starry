using System.Globalization;
using Spectre.Console;
using Starry.Core.Localization;

namespace Starry.Cli;

/// <summary>
/// Renders rich contextual CLI help cards (syntax, flags table, examples)
/// for any starry command and subcommand.
/// </summary>
internal static class CliHelpRenderer
{
    public static void RenderCommandHelp(string[] args, ILocalizer localizer)
    {
        ArgumentNullException.ThrowIfNull(args);
        ArgumentNullException.ThrowIfNull(localizer);

        var cleaned = args.Where(a => !string.Equals(a, "--help", StringComparison.OrdinalIgnoreCase) &&
                                      !string.Equals(a, "-h", StringComparison.OrdinalIgnoreCase) &&
                                      !string.Equals(a, "help", StringComparison.OrdinalIgnoreCase)).ToArray();

        var cmd = cleaned.Length > 0 ? cleaned[0].ToLowerInvariant() : string.Empty;

        switch (cmd)
        {
            case "secret":
                RenderSecretHelp();
                break;
            case "nodes":
                RenderNodesHelp();
                break;
            case "config":
                RenderConfigHelp();
                break;
            case "ports":
            case "domains":
            case "expose":
                RenderNetworkRoutingHelp();
                break;
            case "init":
                RenderInitHelp();
                break;
            case "adopt":
                RenderAdoptHelp();
                break;
            case "plan":
            case "apply":
                RenderPlanApplyHelp(cmd);
                break;
            default:
                RenderGenericHelp(cmd);
                break;
        }
    }

    private static void RenderSecretHelp()
    {
        AnsiConsole.MarkupLine("[bold yellow]STARRY SECRET MANAGEMENT[/]");
        AnsiConsole.MarkupLine("[grey]Unified AES-256-GCM + AAD secret handling and GitHub sync.[/]\n");

        var table = new Table().Border(TableBorder.Rounded).AddColumn("Subcommand").AddColumn("Usage").AddColumn("Description");
        table.AddRow("update / set", "starry secret update <path> [val] [--config <dir>]", "Encrypt and atomically store secret (preserves graph references)");
        table.AddRow("get", "starry secret get <canonical.path> [--config <dir>]", "Decrypt and display plaintext secret value");
        table.AddRow("list", "starry secret list [--config <dir>]", "List all encrypted and plaintext secret keys in configuration");
        table.AddRow("sync-github", "starry secret sync-github [--name <name>]", "Push resolved master key to GitHub Actions Secrets");
        table.AddRow("keygen", "starry secret keygen", "Generate a fresh 32-byte Base64 master key");

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold]Examples:[/]");
        AnsiConsole.MarkupLine("  [cyan]starry secret update services.db.env.POSTGRES_PASSWORD mypassword[/]");
        AnsiConsole.MarkupLine("  [cyan]starry secret get services.db.env.POSTGRES_PASSWORD[/]");
        AnsiConsole.MarkupLine("  [cyan]starry secret list[/]");
    }

    private static void RenderNodesHelp()
    {
        AnsiConsole.MarkupLine("[bold yellow]STARRY NODES MANAGEMENT[/]");
        AnsiConsole.MarkupLine("[grey]Manage VPS fleet nodes, bootstrapping, rolling upgrades, and preflight checks.[/]\n");

        var table = new Table().Border(TableBorder.Rounded).AddColumn("Subcommand").AddColumn("Usage").AddColumn("Description");
        table.AddRow("list", "starry nodes list [--config <dir>]", "List all configured cluster nodes");
        table.AddRow("bootstrap", "starry nodes bootstrap <user@host:port> [--name <n>]", "Bootstrap packages, container engine, and WireGuard on a node");
        table.AddRow("upgrade", "starry nodes upgrade [--rolling] [--full]", "Perform zero-downtime rolling OS security updates");
        table.AddRow("rotate-keys", "starry nodes rotate-keys [--randomize-ports]", "Rotate Ed25519 SSH keys and safely migrate ports");
        table.AddRow("doctor", "starry nodes doctor [node] [--fix]", "Run preflight health check (UTC clock skew, sysctls, tun, port 53)");

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold]Examples:[/]");
        AnsiConsole.MarkupLine("  [cyan]starry nodes doctor --fix[/]");
        AnsiConsole.MarkupLine("  [cyan]starry nodes rotate-keys --randomize-ports[/]");
    }

    private static void RenderConfigHelp()
    {
        AnsiConsole.MarkupLine("[bold yellow]STARRY CONFIGURATION TOOLBOX[/]");
        AnsiConsole.MarkupLine("[grey]Validate, pack, split, and format virtual multi-file configuration trees.[/]\n");

        var table = new Table().Border(TableBorder.Rounded).AddColumn("Subcommand").AddColumn("Usage").AddColumn("Description");
        table.AddRow("validate", "starry config validate [dir]", "Validate schema, graph references, and secret envelopes");
        table.AddRow("pack", "starry config pack <dir> -o <file>", "Pack directory tree into a single standalone YAML/JSON file");
        table.AddRow("split", "starry config split <file> -o <dir>", "Split monolithic config into modular directory hierarchy");
        table.AddRow("fmt", "starry config fmt <file>", "Format configuration file according to standard canonical style");
        table.AddRow("schema", "starry config schema", "Print JSON Schema (draft 2020-12) for Starry configuration");

        AnsiConsole.Write(table);
    }

    private static void RenderNetworkRoutingHelp()
    {
        AnsiConsole.MarkupLine("[bold yellow]STARRY NETWORK & PORT ROUTING[/]");
        AnsiConsole.MarkupLine("[grey]Inspect and expose services across the IPv6 mesh and public gateways.[/]\n");

        var table = new Table().Border(TableBorder.Rounded).AddColumn("Command").AddColumn("Usage").AddColumn("Description");
        table.AddRow("ports list", "starry ports list [--config <dir>]", "List all named bidirectional port routing rules");
        table.AddRow("domains list", "starry domains list [--config <dir>]", "List all hierarchical domain routes and SNI passthroughs");
        table.AddRow("expose", "starry expose <service> <port> [flags]", "Expose container service with optional random port / IP");

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold]Flags for 'starry expose':[/]");
        AnsiConsole.MarkupLine("  [green]--family <ipv4|ipv6|both>[/]  Address family to bind (default: both)");
        AnsiConsole.MarkupLine("  [green]--port <random|number>[/]     Specific or random host port");
        AnsiConsole.MarkupLine("  [green]--proto <tcp|udp>[/]          Transport protocol (default: tcp)");
    }

    private static void RenderInitHelp()
    {
        AnsiConsole.MarkupLine("[bold yellow]STARRY INIT[/]");
        AnsiConsole.MarkupLine("[grey]Initialize a new Starry GitOps repository with standard directory layout.[/]\n");
        AnsiConsole.MarkupLine("[bold]Usage:[/] [cyan]starry init [project-name] [--skip-folder-create][/]\n");
        AnsiConsole.MarkupLine("[bold]Outputs:[/] Creates nodes/, services/, files/, domains/, starry.yaml, and .starry/master.key.");
    }

    private static void RenderAdoptHelp()
    {
        AnsiConsole.MarkupLine("[bold yellow]STARRY ADOPT[/]");
        AnsiConsole.MarkupLine("[grey]Brownfield zero-downtime adoption of existing Docker/Podman servers.[/]\n");
        AnsiConsole.MarkupLine("[bold]Usage:[/] [cyan]starry adopt <user@host[:port]> [--pull-files] [--out-dir ./config] [--format yaml][/]");
    }

    private static void RenderPlanApplyHelp(string cmd)
    {
        AnsiConsole.MarkupLine(CultureInfo.InvariantCulture, $"[bold yellow]STARRY {cmd.ToUpperInvariant()}[/]");
        AnsiConsole.MarkupLine(CultureInfo.InvariantCulture, $"[bold]Usage:[/] [cyan]starry {cmd} [--config <dir>] [--only-changed][/]");
    }

    private static void RenderGenericHelp(string cmd)
    {
        AnsiConsole.MarkupLine($"[bold yellow]STARRY[/] - Command '[cyan]{Markup.Escape(cmd)}[/]'");
        AnsiConsole.MarkupLine("Run [cyan]starry --help[/] to view the main command overview.");
    }
}
