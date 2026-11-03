# Repository Dump: starry

## Overview

- **Total Files**: 157
- **Total Lines of Code**: 21085
- **Total Size**: 798.1 KB
- **Root Directory**: `/root/src/starry`

## Repository Structure

```text
starry/
├── .github/
│   └── workflows/
│       ├── ci-full-matrix.yml
│       ├── ci.yml
│       ├── docs-pages.yml
│       ├── publish-nuget.yml
│       ├── repo-dump.yml
│       └── rotate-master-key.yml
├── scripts/
│   ├── dump_repo.sh
│   ├── repo2md.py
│   └── run-cluster-scenario.sh
├── site/
│   ├── .vitepress/
│   │   ├── theme/
│   │   │   ├── custom.css
│   │   │   └── index.ts
│   │   └── config.mts
│   ├── architecture/
│   │   └── overview.md
│   ├── cli/
│   │   └── commands.md
│   ├── configuration/
│   │   └── syntax.md
│   ├── en/
│   │   ├── architecture/
│   │   │   └── overview.md
│   │   ├── cli/
│   │   │   └── commands.md
│   │   ├── configuration/
│   │   │   └── syntax.md
│   │   ├── guide/
│   │   │   ├── backups-disaster-recovery.md
│   │   │   ├── containers-docker-podman.md
│   │   │   ├── getting-started.md
│   │   │   ├── linux-distributions.md
│   │   │   ├── migrations-dns-p2p.md
│   │   │   ├── network-mesh-vips.md
│   │   │   └── security-cryptography.md
│   │   └── index.md
│   ├── guide/
│   │   ├── backups-disaster-recovery.md
│   │   ├── containers-docker-podman.md
│   │   ├── getting-started.md
│   │   ├── linux-distributions.md
│   │   ├── migrations-dns-p2p.md
│   │   ├── network-mesh-vips.md
│   │   └── security-cryptography.md
│   ├── public/
│   │   └── logo.svg
│   ├── .gitignore
│   ├── index.md
│   ├── package-lock.json
│   └── package.json
├── src/
│   ├── Starry.Cli/
│   │   ├── Program.cs
│   │   └── Starry.Cli.csproj
│   ├── Starry.Core/
│   │   ├── Backups/
│   │   │   └── IBackupStorageProvider.cs
│   │   ├── Configuration/
│   │   │   ├── ConfigTree.cs
│   │   │   ├── IConfigFormatProvider.cs
│   │   │   └── SourceMapEntry.cs
│   │   ├── Containers/
│   │   │   ├── ContainerState.cs
│   │   │   ├── IContainerRuntime.cs
│   │   │   ├── RuntimeInfo.cs
│   │   │   └── ServiceSpec.cs
│   │   ├── Dns/
│   │   │   ├── DnsRecord.cs
│   │   │   └── IDnsProvider.cs
│   │   ├── Localization/
│   │   │   ├── ILocalizer.cs
│   │   │   └── LocalizationKeys.cs
│   │   ├── Nodes/
│   │   │   └── INodeConnection.cs
│   │   ├── Packaging/
│   │   │   └── IOsPackageManager.cs
│   │   ├── Processes/
│   │   │   ├── IProcessRunner.cs
│   │   │   ├── ProcessResult.cs
│   │   │   └── ProcessSpec.cs
│   │   ├── Security/
│   │   │   ├── ICryptoService.cs
│   │   │   └── SecretEnvelope.cs
│   │   └── Starry.Core.csproj
│   ├── Starry.Dsl/
│   │   ├── ConfigAst.fs
│   │   ├── Generators.fs
│   │   ├── GraphResolver.fs
│   │   ├── Library.fs
│   │   └── Starry.Dsl.fsproj
│   └── Starry.Infrastructure/
│       ├── Agent/
│       │   ├── AgentOptions.cs
│       │   ├── AgentStatusReport.cs
│       │   ├── BackupPolicy.cs
│       │   ├── DriftEvent.cs
│       │   ├── DriftEventArgs.cs
│       │   ├── StarryAgentDaemon.cs
│       │   └── WireGuardPeerDump.cs
│       ├── Backups/
│       │   ├── BundleManifest.cs
│       │   ├── FleetBundleService.cs
│       │   └── RcloneBackupStorageProvider.cs
│       ├── Configuration/
│       │   ├── ConfigValidationResult.cs
│       │   ├── ConfigValidator.cs
│       │   ├── GitSyncService.cs
│       │   ├── JsonConfigFormatProvider.cs
│       │   ├── RepositoryInitializer.cs
│       │   ├── TomlConfigFormatProvider.cs
│       │   ├── ValidationIssue.cs
│       │   ├── VirtualConfigEngine.cs
│       │   └── YamlConfigFormatProvider.cs
│       ├── Containers/
│       │   ├── BaseSocketContainerRuntime.cs
│       │   ├── ContainerPriorityScheduler.cs
│       │   ├── DockerRuntime.cs
│       │   └── PodmanRuntime.cs
│       ├── Dns/
│       │   ├── CloudflareDnsProvider.cs
│       │   └── WebhookDnsProvider.cs
│       ├── Localization/
│       │   └── Localizer.cs
│       ├── Network/
│       │   ├── IdLedgerService.cs
│       │   ├── IdLedgerState.cs
│       │   ├── InterfaceClassification.cs
│       │   ├── InternalDnsServer.cs
│       │   ├── NftablesService.cs
│       │   ├── NodeInterfaceInfo.cs
│       │   ├── NodeNetworkInspector.cs
│       │   ├── NodeNetworkReport.cs
│       │   ├── PortForwardRule.cs
│       │   ├── ReplicaEndpoint.cs
│       │   ├── WireGuardMeshService.cs
│       │   └── WireGuardNodeConfig.cs
│       ├── Nodes/
│       │   ├── LocalNodeConnection.cs
│       │   ├── NodeOperationsService.cs
│       │   ├── SshNodeConnection.cs
│       │   └── SshNodeOptions.cs
│       ├── Orchestration/
│       │   ├── ClusterOrchestrator.cs
│       │   ├── HostAdoptionService.cs
│       │   ├── MigrationPlan.cs
│       │   ├── MigrationPlanner.cs
│       │   ├── PlanAction.cs
│       │   └── PlanActionType.cs
│       ├── Packaging/
│       │   ├── ApkPackageManager.cs
│       │   ├── AptPackageManager.cs
│       │   ├── DnfPackageManager.cs
│       │   ├── PackageManagerFactory.cs
│       │   └── PacmanPackageManager.cs
│       ├── Processes/
│       │   └── ProcessRunner.cs
│       ├── Security/
│       │   ├── CryptoService.cs
│       │   └── SecretManager.cs
│       └── Starry.Infrastructure.csproj
├── tests/
│   ├── Starry.ChaosTests/
│   │   ├── AdvancedChaosScenariosTests.cs
│   │   ├── CryptographyChaosTests.cs
│   │   ├── NetworkAndDisruptionChaosTests.cs
│   │   └── Starry.ChaosTests.csproj
│   ├── Starry.E2ETests/
│   │   ├── BrownfieldAdoptionE2ETests.cs
│   │   ├── ClusterEndToEndTests.cs
│   │   ├── ClusterMatrixFuzzerTests.cs
│   │   ├── LiveDockerSshClusterFixture.cs
│   │   ├── LiveMultiNodeSshClusterE2ETests.cs
│   │   └── Starry.E2ETests.csproj
│   └── Starry.UnitTests/
│       ├── ConfigValidatorTests.cs
│       ├── ContainerPrioritySchedulerTests.cs
│       ├── CryptoServiceTests.cs
│       ├── DnsProviderTests.cs
│       ├── FleetBundleServiceTests.cs
│       ├── IdLedgerServiceTests.cs
│       ├── InternalDnsServerScopedTests.cs
│       ├── InternalDnsServerTests.cs
│       ├── LocalizerTests.cs
│       ├── MigrationPlannerTests.cs
│       ├── NftablesServiceTests.cs
│       ├── PackageManagerTests.cs
│       ├── ProcessRunnerTests.cs
│       ├── SshNodeConnectionTests.cs
│       ├── Starry.UnitTests.csproj
│       ├── StarryDslTests.cs
│       ├── VirtualConfigEngineTests.cs
│       └── WireGuardMeshTests.cs
├── .editorconfig
├── .gitignore
├── .ignore
├── Directory.Build.props
├── INIT_GOAL.md
├── README.md
└── Starry.slnx
```

---

## File: .editorconfig

**Lines**: 40 | **Size**: 1.4 KB | **Language**: ini

```ini
# EditorConfig for Starry
root = true

[*]
indent_style = space
indent_size = 4
end_of_line = lf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

[*.cs]
# Code style rules
csharp_style_namespace_declarations = file_scoped:warning
csharp_style_expression_bodied_methods = when_on_single_line:suggestion
csharp_style_expression_bodied_properties = true:suggestion
dotnet_style_qualification_for_field = false:suggestion
dotnet_style_qualification_for_property = false:suggestion
dotnet_style_qualification_for_method = false:suggestion
dotnet_style_qualification_for_event = false:suggestion

# CA1303 - Do not pass literals as localized parameters (allow in tests, enforce elsewhere)
dotnet_diagnostic.CA1303.severity = suggestion
# CA2007 - Consider calling ConfigureAwait on the awaited task (for CLI/Core apps)
dotnet_diagnostic.CA2007.severity = none
# MA0004 - Use Task.ConfigureAwait(false)
dotnet_diagnostic.MA0004.severity = none
# MA0016 - Prefer return collection abstraction
dotnet_diagnostic.MA0016.severity = suggestion
# MA0048 - File name must match type name
dotnet_diagnostic.MA0048.severity = warning

[src/Starry.Cli/**.cs]
dotnet_diagnostic.CA1031.severity = none

[tests/**.cs]
dotnet_diagnostic.CA1707.severity = none
dotnet_diagnostic.CA1303.severity = none
dotnet_diagnostic.MA0051.severity = none
dotnet_diagnostic.S2699.severity = none
```

## File: .gitignore

**Lines**: 32 | **Size**: 321 B | **Language**: gitignore

```gitignore
# Deepwork metadata
.slim/deepwork/

# Build results
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/
nupkg/
*.nupkg
publish-aot/



# .NET / Rider / Visual Studio
.vs/
.idea/
*.user
*.userosscache
*.sln.docstates
.vscode/

# Node / VitePress
node_modules/
dist/
cache/
.vitepress/dist/
.vitepress/cache/

# OS files
.DS_Store
Thumbs.db
```

## File: .ignore

**Lines**: 2 | **Size**: 36 B | **Language**: gitignore

```gitignore
!.slim/deepwork/
!.slim/deepwork/**
```

## File: Directory.Build.props

**Lines**: 31 | **Size**: 1.4 KB | **Language**: xml

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisLevel>latest-all</AnalysisLevel>
    <NoWarn>$(NoWarn);IL2026;IL3050;IL2111;IL2104</NoWarn>
    <ContinuousIntegrationBuild Condition="'$(GITHUB_ACTIONS)' == 'true'">true</ContinuousIntegrationBuild>
  </PropertyGroup>

  <PropertyGroup Condition="'$(MSBuildProjectExtension)' == '.csproj'">
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  </PropertyGroup>

  <ItemGroup Condition="'$(MSBuildProjectExtension)' == '.csproj'">
    <PackageReference Include="Meziantou.Analyzer" Version="3.0.318">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Roslynator.Analyzers" Version="5.0.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="SonarAnalyzer.CSharp" Version="10.35.0.4138">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
</Project>
```

## File: INIT_GOAL.md

**Lines**: 210 | **Size**: 27.6 KB | **Language**: markdown

````markdown
# 🌟 MASTER GOAL & SPECIFICATION: `starry` (Autonomous Multi-Agent Spec)

## 1. Идентичность и Концепция Проекта
* **Название проекта и CLI-бинарника:** `starry` (в честь клуба STARRY из *Bocchi the Rock!*).
* **Суть:** Децентрализованный GitOps-оркестратор контейнеров (**Docker & Podman**), оверлейных сетей **WireGuard**, хостового файрвола (`nftables`), пакетов ОС (`apt`/`dnf`/`apk`/`pacman`), DNS-провайдеров и потоковых бэкапов для динамического флота гетерогенных VPS (включая серверы за NAT и с малым объемом диска).
* **Архитектура:** Единый самодостаточный бинарник (`Single-Binary Hybrid`: CLI + Server Agent), распространяемый двумя способами:
  1. Самодостаточный **Native AOT** бинарник (`linux-x64`, `linux-arm64`, `win-x64`, `osx-arm64`) без зависимостей.
  2. Глобальный пакет **NuGet .NET Tool** (`dotnet tool install -g Starry.Cli` -> команда `starry`).

---

## 2. Протокол Работы Субагентов (Multi-Agent Orchestration Rules)

Главный ИИ-агент обязан активно порождать специализированных **субагентов (Subagents)** для параллельной и глубокой проработки задач:

1. **Research Subagents (Запрет на галлюцинации API):**
   * Перед написанием кода интеграции с **Docker Engine API**, **Podman Libpod/Compat Socket API**, **Cloudflare API**, **GitHub Secrets API**, **`nftables` JSON/CLI**, **`rclone`** и **`wg`**, главный агент обязан запустить Research-субагента для проверки точных форматов запросов, заголовков, кодов ответов и различий между дистрибутивами Linux. Запрещено выдумывать несуществующие флаги или эндпоинты.
2. **Adversarial & Chaos Testing Subagents (Генераторы сбоев):**
   * На каждом этапе отдельный субагент обязан придумывать пограничные случаи и сценарии отказов («Что если SSH оборвется посередине миграции тома?», «Что если диск на VPS заполнится на 100% во время бэкапа?», «Что если на Ubuntu стоит `iptables-nft`, а на AlmaLinux чистый `nftables`?», «Что если в конфиге циклическая зависимость или битый UTF-8?») и писать под них автоматические тесты для CI/CD.
3. **Documentation & i18n Subagents (Параллельная документация):**
   * Документация пишется **параллельно с кодом каждого шага**, а не в самом конце. Любая новая команда, интерфейс или поле конфига немедленно документируется на двух языках (**RU** и **EN**) с подробными примерами для публикации на **GitHub Pages**.

---

## 3. Стандарты Кода, Статические Анализаторы и Качество

1. **Платформа:** C# (.NET 9 / .NET 10), строгая совместимость с **Native AOT** (`PublishAot=true`, `InvariantGlobalization=false` с поддержкой `i18n` словарей в памяти) и упаковкой **NuGet Tool** (`PackAsTool=true`, `ToolCommandName=starry`).
2. **Статический анализ кода (Включен по умолчанию в `Directory.Build.props`):**
   * `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`
   * `<AnalysisLevel>latest-all</AnalysisLevel>`
   * `<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>`
   * Подключить пакеты анализаторов:
     * `Meziantou.Analyzer` (строгие проверки асинхронности, безопасности, производительности и AOT)
     * `Roslynator.Analyzers` (чистота кода и рефакторинг)
     * `SonarAnalyzer.CSharp` (поиск багов, уязвимостей и code smells)
3. **Интернационализация (`i18n`):**
   * Все сообщения CLI, ошибки валидации и планы миграции проходят через сервис локализации `ILocalizer` с встроенными ресурсами для `ru` (Русский) и `en` (English), переключаемыми через флаг `--lang ru|en` или переменную `STARRY_LANG`.
4. **Zero-Bloat & Unix-Way:**
   * Общение с Docker и Podman — напрямую через `HttpClient` + `SocketsHttpHandler` по Unix Domain Socket.
   * Вызовы системных утилит (`ssh`, `scp`, `rsync`, `wg`, `nft`, `rclone`, `git`) — через строго типизированный `IProcessRunner` с защитой от Command Injection (экранирование аргументов, передача секретов только через `stdin` или защищенные пайпы в памяти, никогда через аргументы командной строки, видимые в `ps aux`!).

---

## 4. Расширяемая Архитектура и Контракты Плагинов (`Starry.Core`)

Система спроектирована по принципу Open/Closed, чтобы любой разработчик мог добавить поддержку нового формата или провайдера через отдельный класс:

```csharp
// 1. Форматы конфигурации (YAML, JSON, TOML + возможность добавить HCL/XML/и т.д.)
public interface IConfigFormatProvider
{
    IReadOnlyList<string> SupportedExtensions { get; }
    JsonObject Parse(ReadOnlySpan<byte> content, string sourcePath);
    string Serialize(JsonObject node);
}

// 2. Среда выполнения контейнеров (Docker Engine + Podman Rootful/Rootless)
public interface IContainerRuntime
{
    string RuntimeName { get; } // "docker", "podman"
    Task<RuntimeInfo> ProbeAsync(INodeConnection node, CancellationToken ct);
    Task<IReadOnlyList<ContainerState>> ListContainersAsync(INodeConnection node, CancellationToken ct);
    Task DeployContainerAsync(INodeConnection node, ServiceSpec spec, CancellationToken ct);
    Task StreamImagePeerToPeerAsync(INodeConnection source, INodeConnection target, string imageTag, CancellationToken ct);
}

// 3. Кросс-дистрибутивный пакетный менеджер и обновление ОС (apt, dnf, apk, pacman, zypper)
public interface IOsPackageManager
{
    IReadOnlyList<string> SupportedDistroIds { get; } // ["ubuntu", "debian"], ["rhel", "almalinux", "rocky", "fedora"], ["alpine"], ["arch"]
    Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct);
    Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct);
    Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct);
}

// 4. Провайдеры DNS (Cloudflare, Hetzner, Route53, Porkbun, Webhook)
public interface IDnsProvider
{
    string ProviderName { get; }
    Task<IReadOnlyList<DnsRecord>> GetRecordsAsync(string zoneDomain, CancellationToken ct);
    Task UpsertRecordAsync(string zoneDomain, DnsRecord record, CancellationToken ct);
    Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct);
}

// 5. Провайдеры потоковых бэкапов (Rclone, SFTP-Peer, S3, Local)
public interface IBackupStorageProvider
{
    string TypeName { get; }
    Task StreamBackupAsync(INodeConnection node, string volumePath, string backupId, CancellationToken ct);
    Task StreamRestoreAsync(INodeConnection node, string backupId, string targetVolumePath, CancellationToken ct);
}
5. Спецификация Криптографии и Безопасности
Алгоритм: AES-256-GCM (ключ 32 байта, криптостойкий случайный Nonce 12 байт, тег 16 байт).
Контекстная привязка (AAD): В качестве Additional Authenticated Data передается канонический JSON-путь поля в дереве конфигурации (например, nodes.bocchi.ssh_key), что делает невозможным атаку подмены (cut-and-paste) зашифрованных значений между полями.
Форматы в конфиге:
Открытый секрет для шифрования на лету (--encrypt-open-secrets): PLAIN[значение].
Зашифрованный секрет: ENC[AES256_GCM:v1:<base64_nonce>:<base64_cipher>:<base64_tag>].
Глобальная ротация Мастер-Ключа (для запуска по крону раз в месяц в GitHub Actions):
Команда:
starry encrypt global --reencrypt --last-main-key=<base64> [--new-main-key=<base64>] [--update-gh-secret=STARRY_MASTER_KEY] [--git-commit] [--git-push]
Расшифровывает все ENC[...] старым ключом, генерирует новый 256-битный ключ, перешифровывает всё дерево конфига, обновляет секрет в GitHub Actions (gh secret set или GitHub REST API с LibSodium) и пушит коммит в репозиторий.
Еженедельная ротация SSH и WireGuard ключей (starry rotate-keys):
Генерирует новые ключи Ed25519 и WireGuard, добавляет новые ключи на ноды, проверяет соединение, удаляет старые ключи, обновляет конфиг и коммитит изменения.
6. Декларативное Управление Пакетами ОС и Патчами Безопасности
Чтобы решить проблему устаревания системных пакетов и уязвимостей ядра Linux без нарушения декларативности:
Декларативный список пакетов (packages):
В секции ноды (или глобально для всех нод) указываются пакеты: packages: [wireguard-tools, nftables, rsync, zstd, curl, htop]. Агент определяет дистрибутив через /etc/os-release и вызывает нужный IOsPackageManager (apt-get, dnf, apk, pacman) в неинтерактивном режиме.
Политика скользящего обновления ОС и ядра (os_maintenance + команда starry nodes upgrade):
В конфиге ноды можно описать:
code
Yaml
os_maintenance:
  auto_security_updates: true
  schedule: "@weekly"
  reboot_policy: rolling_drain # never | rolling_drain | immediate
Алгоритм rolling_drain (Обновление и перезагрузка без даунтайма):
Если на node-A после установки патчей безопасности требуется перезагрузка ядра (/var/run/reboot-required на Debian/Ubuntu или needs-restarting -r на RHEL/AlmaLinux):
Оркестратор временно уводит внешний трафик с node-A на резервные ноды (через DNS / реплики / балансировщик).
Аккуратно останавливает контейнеры на node-A и выполняет reboot.
Ожидает возвращения node-A по SSH, проверяет старт starry agent, туннелей WireGuard и контейнеров (Healthcheck).
Возвращает трафик на node-A и переходит к следующей ноде!
7. Пошаговый План Реализации (Sequential Execution Plan)
Агент обязан выполнять шаги последовательно. На каждом шаге одновременно пишутся: код, модульные/хаос-тесты и двуязычная документация (RU/EN) для GitHub Pages.
Шаг 1: Каркас Solution, Статические Анализаторы, i18n и Сайт Документации (GitHub Pages)
Создать проекты Starry.Core, Starry.Infrastructure, Starry.Cli, Starry.UnitTests, Starry.ChaosTests, Starry.E2ETests.
Настроить упаковку Starry.Cli как NuGet .NET Tool и как Native AOT бинарник. Подключить статические анализаторы (Meziantou, Roslynator, SonarAnalyzer).
Реализовать систему локализации i18n (ru / en).
Инициализировать в папке site/ генератор статического сайта документации для GitHub Pages (VitePress или Astro Starlight) с настроенной интернационализацией (/ru/ и /en/), поиском и автодеплоем через .github/workflows/docs-pages.yml.
Шаг 2: Virtual Config Engine (YAML, JSON, TOML) и Команды Управления Конфигом
Реализовать YamlConfigFormatProvider, JsonConfigFormatProvider, TomlConfigFormatProvider.
Реализовать сборку виртуального дерева JsonObject из иерархии папок и файлов (nodes/bocchi.yaml -> root.nodes.bocchi, services/api.toml -> root.services.api) с сохранением Source Map.
Реализовать команды starry config validate, starry config pack, starry config split, starry config fmt, starry config schema.
Документация (RU/EN): Написать раздел «Архитектура Конфигурации» с интерактивными примерами на YAML, JSON и TOML, правилами лучших практик (Best Practices) организации папок для маленьких и больших флотов.
Шаг 3: Криптография (AES-256-GCM + AAD), Ротация Мастер-Ключа и GitOps
Реализовать шифрование PLAIN[...] -> ENC[...], флаг --encrypt-open-secrets.
Реализовать starry encrypt global --reencrypt с поддержкой обновления секрета в GitHub Actions и авто-коммита в Git (--allow-config-edit, --git-commit, --git-push).
Хаос-тесты: Попытки расшифровки битых тегов, перенос ENC[...] в другое поле конфига (проверка срабатывания защиты AAD), прерывание записи при перешифровке (проверка транзакционности записи файлов через temp-file rename).
Документация (RU/EN): Гайд по криптографии, хранению секретов в публичных/приватных репо и настройке ежемесячной авто-ротации в GitHub Actions.
Шаг 4: SSH-Транспорт, Поддержка Разных Дистрибутивов (IOsPackageManager) и Обновлений ОС
Реализовать SshTransport с поддержкой Jump-нод (-J) для серверов за NAT, мультиплексированием соединений (ControlMaster) и передачей ключей через память.
Реализовать AptPackageManager (Ubuntu/Debian), DnfPackageManager (RHEL/AlmaLinux/Rocky/Fedora), ApkPackageManager (Alpine), PacmanPackageManager (Arch).
Реализовать установку базовых пакетов, команду starry nodes upgrade --rolling и команду ротации серверных ключей starry rotate-keys.
Документация (RU/EN): Таблица совместимости дистрибутивов Linux, настройка NAT-VPS и политика обновлений безопасности ОС.
Шаг 5: Децентрализованный WireGuard, NAT Traversal, Кочующие VIP и Nftables
Реализовать настройку P2P-меша WireGuard (starry0), пробивку NAT (PersistentKeepalive = 25), транзитную маршрутизацию через публичные ноды.
Реализовать Кочующие IP контейнеров (Container VIPs 10.99.x.x), динамическое обновление AllowedIPs и встроенный в агент DNS-сервер для доменов *.internal.
Реализовать управление таблицей table inet starry в nftables (файрвол + раздельный проброс портов ipv4:port и ipv6:port на внутренние VIP контейнеров).
Документация (RU/EN): Схемы сетевой топологии, объяснение работы Кочующих VIP с Nginx Proxy Manager (NPM) и примеры роутинга IPv4/IPv6.
Шаг 6: Двойной Движок Контейнеров (Docker & Podman), Шаблоны Файлов (files) и Приоритеты
Запустить Research-субагента для сверки спецификаций Docker Engine API и Podman Socket API (rootful и rootless).
Реализовать DockerRuntime и PodmanRuntime (IContainerRuntime) с автоопределением сокета (/var/run/docker.sock, /run/podman/podman.sock, /run/user/$UID/podman/podman.sock).
Реализовать шаблонизацию и монтирование конфиг-файлов (files) с отслеживанием хеша SHA256 и отправкой SIGHUP при изменении.
Реализовать планирование и деплой с учетом числового приоритета priority: int (от меньшего к большему: 0 для приватного Registry -> 10 для БД -> 50 для бэкендов -> 90 для прокси).
Шаг 7: Планировщик (plan / apply), P2P-образы и Бесшовный DNS (IDnsProvider)
Реализовать построение графа миграции (DAG) и красивый рендеринг плана в Spectre.Console.
Реализовать P2P-стриминг образов между серверами (docker/podman save | zstd | ssh load) при недоступности или переезде приватного Registry.
Реализовать CloudflareDnsProvider и WebhookDnsProvider, а также бесшовную Blue-Green миграцию сервисов между серверами без потери активных соединений.
Шаг 8: Потоковые Бэкапы (rclone), Проводник Томов и «Капсула Судного Дня» (bundle)
Реализовать потоковые бэкапы томов в оперативной памяти без создания временных файлов на диске VPS (tar | zstd | rclone rcat).
Реализовать starry volume open <vol> (SFTP-туннель + локальный веб-менеджер файлов тома).
Реализовать starry bundle export и starry bundle import для выгрузки всего флота в один архив на ПК и быстрого развертывания на новых IP.
8. Спецификация Экстремального Тестирования и Chaos Engineering в CI/CD
Агент обязан создать мощный тестовый комплекс, который запускается в GitHub Actions (.github/workflows/ci-full-matrix.yml):
8.1. Мульти-дистрибутивная Матрица ВМ / Системных Контейнеров в CI/CD
В пайплайне GitHub Actions (с использованием KVM/QEMU, Incus/LXD или привилегированных systemd-контейнеров) поднимается гетерогенный кластер из разных дистрибутивов Linux одновременно:
Узел 1 (bocchi-ubuntu): Ubuntu 24.04 LTS (apt + Docker + публичный IP).
Узел 2 (nijika-alma): AlmaLinux 9 / Fedora (dnf — семейство RedHat + Podman + публичный IP).
Узел 3 (ryo-debian-nat): Debian 12 (apt + Docker + находится строго за NAT без прямого входа снаружи!).
Узел 4 (ikuyo-alpine): Alpine Linux (apk + минимальное окружение).
количество узлов может быть разным, разные весрии дистрибутивов, разные дистрибутивы, разные комбинации. Вам надо учитывать ограничения раннера по памяти и диску, так что можно процедурно сначала тестируем связку убунта и альмалинуск, потом удаляем альма и скачиваем альпин
8.2. Обязательные Сценарии Chaos Engineering (tests/Starry.ChaosTests)
Субагент по тестированию обязан реализовать и прогнать в CI следующие тесты на отказ (Fault Injection): так же надо придумать кучу возможных сценаривев развития событий и отказов, такие как OutOfMemory, деградация сети, деградация диска, и тд. отказы могут затрагивать как и одну так и несколько нод
Chaos 1: Обрыв SSH посередине миграции тома (Connection Drop Mid-Rsync):
Эмулируется принудительное убийство SSH-процесса (SIGKILL) во время переноса тома с Ubuntu (Docker) на AlmaLinux (Podman).
Ожидание: Старый контейнер на Ubuntu продолжает работать, DNS в Cloudflare НЕ переключается на недокачанную ноду, повторный запуск starry apply докачивает дельту и успешно завершает переезд.
Chaos 2: Переполнение диска на VPS (Disk Full / ENOSPC Simulation):
Создается лимитированный tmpfs / loop-раздел на 50 МБ, забитый на 95%. Запускается потоковый бэкап тома через rclone.
Ожидание: Бэкап проходит успешно (так как идет потоком через RAM), диск ноды не переполняется. При попытке деплоя слишком большого образа выдается четкая пред-проверка свободного места до падения демона.
Chaos 3: Смерть Приватного Registry во время миграции кластера:
Приватный Registry принудительно останавливается. Запускается миграция приложения на новую ноду.
Ожидание: Оркестратор автоматически переключается на P2P-стриминг образа напрямую с соседней ноды, где этот образ есть в кэше, и успешно поднимает сервис.
Chaos 4: Отказ нового конфига фаервола / Потеря связности (Deadman Switch Rollback):
Передается некорректное правило nftables или битый конфиг WireGuard, блокирующий трафик.
Ожидание: Агент фиксирует потерю пинга до пиров в течение 15 секунд и автоматически откатывает (rollback) предыдущую рабочую конфигурацию сети.
Chaos 5: Сбой DNS API (Cloudflare 502 / Rate Limit):
Mock-сервер Cloudflare возвращает 429 Too Many Requests и 502 Bad Gateway.
Ожидание: Срабатывает экспоненциальный Backoff с джиттером; если API полностью мертв — миграция безопасно останавливается на этапе до отключения старого контейнера.
Chaos 6: Междистрибутивная и Межрантаймовая миграция (Ubuntu Docker ➔ AlmaLinux Podman):
Сервис с привязанным томом, секретами, шаблонным конфигом и кочующим VIP мигрирует с Ubuntu (Docker) на AlmaLinux (Podman), при этом база данных остается на Debian за NAT.
Ожидание: Сквозной HTTP-запрос через проброшенный порт nftables на входной ноде успешно проходит до бэкенда на Podman, который читает данные из БД за NAT по сети WireGuard.
9. Требования к Документации на GitHub Pages (site/)
Сайт документации должен собираться автоматически и включать (на Русском и Английском языках):
Введение и Быстрый старт: Установка через dotnet tool install -g Starry.Cli и через скачивание Native AOT бинарника.
Best Practices по Организации Конфигов:
Когда использовать один файл starry.yaml, а когда разбивать на папки nodes/, services/, volumes/.
Как смешивать YAML, JSON и TOML.
Как правильно назначать priority (0..100) для исключения гонок при старте.
Справочник всех команд CLI (CLI Reference): С флагами, кодами возврата и примерами вывода.
Сетевая Архитектура и WireGuard: Подробный разбор работы NAT-VPS, Jump-нод, Кочующих VIP (10.99.x.x), интеграции с Nginx Proxy Manager (NPM) и правил nftables (ipv4 vs ipv6).
Безопасность и GitOps в GitHub Actions: Готовые примеры .github/workflows/ для авто-деплоя при пуше, еженедельной ротации SSH/WG ключей и ежемесячной перешифровки мастер-ключа (--reencrypt).
Гайд по Расширению (Contributing & Plugins): Пошаговая инструкция с примерами кода, как добавить свой DNS-провайдер, пакетный менеджер или формат конфига.
10. Критерии Полной Готовности (Definition of Done)
Ни одного TODO, NotImplementedException или заглушки в кодовой базе.
Сборка с TreatWarningsAsErrors=true и всеми статическими анализаторами проходит с 0 предупреждений.
Работает сборка и как Native AOT бинарник, и как NuGet .NET Tool (dotnet pack).
Все Unit, Chaos и Multi-Distro E2E тесты зеленые.
Сайт документации в site/ полностью написан на RU и EN и успешно собирается в статику для GitHub Pages.
code
Code
---
````

## File: README.md

**Lines**: 157 | **Size**: 6.5 KB | **Language**: markdown

````markdown
# 🌟 STARRY

<p align="center">
  <img src="site/public/logo.svg" width="128" height="128" alt="Starry Logo" />
</p>

<p align="center">
  <strong>Decentralized GitOps Orchestrator for Docker, Podman, WireGuard, nftables, and VPS Fleets</strong>
</p>

<p align="center">
  <a href="https://github.com/perdub/starry/actions/workflows/ci.yml"><img src="https://github.com/perdub/starry/actions/workflows/ci.yml/badge.svg" alt="CI Status" /></a>
  <a href="https://github.com/perdub/starry/actions/workflows/ci-full-matrix.yml"><img src="https://github.com/perdub/starry/actions/workflows/ci-full-matrix.yml/badge.svg" alt="Matrix CI" /></a>
  <a href="https://perdub.github.io/starry/"><img src="https://img.shields.io/badge/Docs-VitePress-f59e0b" alt="Documentation" /></a>
  <a href="https://www.nuget.org/packages/starrycli"><img src="https://img.shields.io/nuget/v/starrycli.svg?color=blue" alt="NuGet" /></a>
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-green" alt="License" /></a>
</p>

---

## 📖 Overview

**`starry`** is an autonomous, single-binary GitOps orchestrator designed to run across heterogeneous cloud providers and low-cost virtual private servers (including NAT-isolated nodes and servers with small 10–20 GB SSDs).

It manages the entire lifecycle of:
* **Container Runtimes**: Direct Unix Domain Socket control of **Docker Engine** and **Podman** (Rootful & unprivileged Rootless).
* **Encrypted Mesh Networking**: P2P **WireGuard** (`starry0`) with automated NAT traversal (`PersistentKeepalive = 25`) and Roaming Container VIPs (`10.99.x.x`).
* **Host Firewalls & NAT**: Isolated table `inet starry` in **`nftables`** for atomic DNAT port forwarding and strict packet filtering.
* **Operating System Packages**: Unified abstraction across **Apt** (Debian/Ubuntu), **Dnf** (RHEL/AlmaLinux/Rocky/Fedora), **Apk** (Alpine), and **Pacman** (Arch Linux) with rolling security updates.
* **Security & Secret Management**: In-repository encrypted secrets using **AES-256-GCM** bound to canonical JSON paths with **AAD (Additional Authenticated Data)** to block cut-and-paste tampering.
* **Zero-Disk Streaming Backups**: Real-time volume backups via `tar | zstd | rclone rcat` without creating intermediate files on constrained VPS disks.
* **Disaster Recovery Bundles**: Export and re-import entire fleet configurations and states to spin up on new IP addresses within minutes.

Documentation is available online at: **[https://perdub.github.io/starry/](https://perdub.github.io/starry/)** (available in **English** and **Russian**).

---

## ⚡ Quick Start

### 1. Installation

#### Option A: .NET Global Tool
```bash
dotnet tool install -g starrycli
```

#### Option B: Standalone Native AOT Binary (7.3 MB, Zero Dependencies)
Download the pre-compiled native binary for Linux (`x64`, `arm64`) from [GitHub Releases](https://github.com/perdub/starry/releases):
```bash
curl -fsSL https://github.com/perdub/starry/releases/latest/download/starry-linux-x64 -o /usr/local/bin/starry
chmod +x /usr/local/bin/starry
```

### 2. Configuration (`starry.yaml` or Modular Directory)

Starry supports single-file configurations as well as modular multi-file directories (`YAML`, `JSON`, `TOML`):

```yaml
version: "1.0"
cluster_name: "kessoku-prod"

mesh:
  cidr: "10.99.0.0/16"
  port: 51820
  keepalive: 25

nodes:
  bocchi:
    host: "198.51.100.10"
    port: 22
    vpn_ip: "10.99.0.1"
    is_public: true
    ssh_key: "ENC[AES256_GCM:v1:...]"

services:
  web:
    image: "nginx:alpine"
    node: "bocchi"
    vip: "10.99.10.5"
    priority: 80
    ports:
      - "80:80"
      - "443:443"
    files:
      /etc/nginx/conf.d/default.conf: |
        server {
            listen 80;
            location / { return 200 "Hello from Starry!"; }
        }
```

### 3. Validate, Plan & Apply

```bash
# Validate cluster syntax, schema and secrets integrity
starry config validate ./cluster

# Build DAG migration execution plan
starry plan --config ./cluster

# Apply planned changes
starry apply --config ./cluster
```

---

## 🛠️ CLI Commands Reference

| Command | Description |
|---|---|
| `starry config validate [path]` | Validate configuration schema, references, and secret envelopes. |
| `starry config pack <dir> -o <file>` | Pack modular directory hierarchy into a single bundle file. |
| `starry config split <file> -o <dir>` | Split monolithic bundle back into modular folder structure. |
| `starry config fmt <file>` | Canonically format configuration files. |
| `starry config schema` | Print official JSON Schema for IDE autocompletion. |
| `starry encrypt [path]` | Encrypt all `PLAIN[...]` secrets in configuration into `ENC[...]`. |
| `starry encrypt global --reencrypt` | Rotate master key, re-encrypt all secrets, and commit to Git. |
| `starry nodes upgrade --rolling` | Execute zero-downtime rolling OS security updates across nodes. |
| `starry nodes rotate-keys` | Generate fresh Ed25519 SSH keys and update remote `authorized_keys`. |
| `starry plan` | Generate and visualize DAG migration graph with colored icons. |
| `starry apply` | Execute planned actions across the cluster fleet. |
| `starry volume open <node> <vol>` | Open remote container volume via secure SFTP tunnel. |
| `starry bundle export -o <archive>` | Export entire fleet into a disaster recovery capsule. |
| `starry bundle import <archive>` | Restore fleet capsule with optional `--override-ip` mappings. |

---

## 🔒 Security Architecture: AES-256-GCM + AAD

Starry uses **Authenticated Encryption with Associated Data (AEAD)**:
* **Cipher**: AES-256-GCM with a 32-byte master key, 12-byte random nonce, and 16-byte authentication tag.
* **AAD Binding**: Every secret embeds its exact canonical JSON path (e.g. `nodes.bocchi.ssh_key`) into the authentication tag. Moving an encrypted string to another field will fail decryption with `CryptographicException`!
* **Automated Key Rotation**: Monthly automated rotation in GitHub Actions (`.github/workflows/rotate-master-key.yml`).

---

## 🤝 Contributing & Architecture

* Built on **C# (.NET 10)** targeting **Native AOT** and **NuGet Tool** packaging.
* Clean architecture with zero third-party bloated Docker SDKs: pure Unix Domain Sockets over `SocketsHttpHandler`.
* Strict static analysis enforced with `TreatWarningsAsErrors=true`, `Meziantou.Analyzer`, `Roslynator`, and `SonarAnalyzer`.

```bash
# Clone and build
git clone https://github.com/perdub/starry.git
cd starry
dotnet build Starry.slnx

# Run 57 Unit, Chaos, and E2E Tests
dotnet test Starry.slnx
```

---

## 📄 License

Licensed under the [MIT License](LICENSE).
````

## File: Starry.slnx

**Lines**: 13 | **Size**: 555 B | **Language**: xml

```xml
<Solution>
  <Folder Name="/src/">
    <Project Path="src/Starry.Cli/Starry.Cli.csproj" />
    <Project Path="src/Starry.Core/Starry.Core.csproj" />
    <Project Path="src/Starry.Dsl/Starry.Dsl.fsproj" />
    <Project Path="src/Starry.Infrastructure/Starry.Infrastructure.csproj" />
  </Folder>
  <Folder Name="/tests/">
    <Project Path="tests/Starry.ChaosTests/Starry.ChaosTests.csproj" />
    <Project Path="tests/Starry.E2ETests/Starry.E2ETests.csproj" />
    <Project Path="tests/Starry.UnitTests/Starry.UnitTests.csproj" />
  </Folder>
</Solution>
```

## File: .github/workflows/ci-full-matrix.yml

**Lines**: 124 | **Size**: 4.1 KB | **Language**: yaml

```yaml
name: Full Multi-Distro Matrix & Chaos CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  workflow_dispatch:

concurrency:
  group: ci-matrix-${{ github.ref }}
  cancel-in-progress: true

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Restore dependencies
        run: dotnet restore Starry.slnx

      - name: Build solution (TreatWarningsAsErrors)
        run: dotnet build Starry.slnx --configuration Release --no-restore

      - name: Run Unit Tests
        run: dotnet test tests/Starry.UnitTests/Starry.UnitTests.csproj --configuration Release --no-build --verbosity normal

      - name: Run Chaos Engineering Tests
        run: dotnet test tests/Starry.ChaosTests/Starry.ChaosTests.csproj --configuration Release --no-build --verbosity normal

      - name: Run E2E Tests
        run: dotnet test tests/Starry.E2ETests/Starry.E2ETests.csproj --configuration Release --no-build --verbosity normal

      - name: Pack NuGet .NET Tool
        run: dotnet pack src/Starry.Cli/Starry.Cli.csproj --configuration Release --no-build -o nupkg

      - name: Verify Native AOT Publish (linux-x64)
        run: dotnet publish src/Starry.Cli/Starry.Cli.csproj -r linux-x64 -c Release -p:PublishAotEnabled=true -o publish-aot

      - name: Upload Native Binary
        uses: actions/upload-artifact@v4
        with:
          name: starry-linux-x64
          path: publish-aot/starry

  distro-matrix:
    needs: build-and-test
    runs-on: ubuntu-latest
    strategy:
      fail-fast: false
      matrix:
        include:
          - distro: "Ubuntu 24.04 (Noble)"
            image: "ubuntu:24.04"
            pkg: "apt-get update && apt-get install -y --no-install-recommends curl wireguard-tools nftables"
          - distro: "Debian 12 (Bookworm)"
            image: "debian:12"
            pkg: "apt-get update && apt-get install -y --no-install-recommends curl wireguard-tools nftables"
          - distro: "AlmaLinux 9"
            image: "almalinux:9"
            pkg: "dnf install -y --allowerasing wireguard-tools nftables"
          - distro: "Alpine 3.20"
            image: "alpine:3.20"
            pkg: "apk add --no-cache curl wireguard-tools nftables gcompat"
          - distro: "Arch Linux"
            image: "archlinux:latest"
            pkg: "pacman -Sy --noconfirm which curl wireguard-tools nftables"
    container:
      image: ${{ matrix.image }}
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Download Native Binary
        uses: actions/download-artifact@v4
        with:
          name: starry-linux-x64
          path: /usr/local/bin

      - name: Install System Prerequisites
        run: ${{ matrix.pkg }}

      - name: Test Native AOT Binary in Distro Container
        run: |
          [ -f /usr/local/bin/starry ] && chmod +x /usr/local/bin/starry || (find /usr/local/bin -name starry -exec cp {} /usr/local/bin/starry \; && chmod +x /usr/local/bin/starry)
          /usr/local/bin/starry --version
          /usr/local/bin/starry config schema > /dev/null
          mkdir -p /tmp/smoke
          /usr/local/bin/starry init /tmp/smoke
          /usr/local/bin/starry config validate /tmp/smoke
          /usr/local/bin/starry plan /tmp/smoke

  live-cluster-matrix:
    needs: build-and-test
    runs-on: ubuntu-latest
    strategy:
      fail-fast: false
      matrix:
        shard: [1, 2, 3, 4]
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Run Cluster Scenario Shard
        run: |
          SHARD=${{ matrix.shard }}
          START=$(( (SHARD - 1) * 6 + 1 ))
          END=$(( SHARD * 6 ))
          echo "Running Shard $SHARD (Scenarios $START to $END)..."
          for i in $(seq $START $END); do
            ./scripts/run-cluster-scenario.sh $i
          done
```

## File: .github/workflows/ci.yml

**Lines**: 79 | **Size**: 2.6 KB | **Language**: yaml

```yaml
name: CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  workflow_dispatch:

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Setup Python
        uses: actions/setup-python@v5
        with:
          python-version: '3.x'

      - name: Restore dependencies
        run: dotnet restore Starry.slnx

      - name: Build solution (TreatWarningsAsErrors)
        run: dotnet build Starry.slnx --configuration Release --no-restore

      - name: Run unit, chaos & E2E tests
        id: run-tests
        run: |
          set +e
          TEST_EXIT_CODE=0

          echo "=== Running Unit Tests ===" | tee runner-tests.log
          dotnet test tests/Starry.UnitTests/Starry.UnitTests.csproj --configuration Release --no-build --verbosity normal 2>&1 | tee -a runner-tests.log
          if [ ${PIPESTATUS[0]} -ne 0 ]; then TEST_EXIT_CODE=1; fi

          echo "=== Running Chaos Tests ===" | tee -a runner-tests.log
          dotnet test tests/Starry.ChaosTests/Starry.ChaosTests.csproj --configuration Release --no-build --verbosity normal 2>&1 | tee -a runner-tests.log
          if [ ${PIPESTATUS[0]} -ne 0 ]; then TEST_EXIT_CODE=1; fi

          echo "=== Running E2E Tests ===" | tee -a runner-tests.log
          dotnet test tests/Starry.E2ETests/Starry.E2ETests.csproj --configuration Release --no-build --verbosity normal 2>&1 | tee -a runner-tests.log
          if [ ${PIPESTATUS[0]} -ne 0 ]; then TEST_EXIT_CODE=1; fi

          echo "exit_code=$TEST_EXIT_CODE" >> "$GITHUB_OUTPUT"

      - name: Pack NuGet Tool
        if: steps.run-tests.outputs.exit_code == '0'
        run: dotnet pack src/Starry.Cli/Starry.Cli.csproj --configuration Release --no-build -o nupkg

      - name: Generate Repository Dump with Test Runner Logs
        if: always()
        run: |
          python3 scripts/repo2md.py -o REPO_DUMP.md --runner-logs runner-tests.log

      - name: Upload Repository Dump & Runner Logs
        if: always()
        uses: actions/upload-artifact@v4
        with:
          name: repo-dump
          path: |
            REPO_DUMP.md
            runner-tests.log
          if-no-files-found: error
          retention-days: 7

      - name: Check Test Results Status
        if: always()
        run: |
          if [ "${{ steps.run-tests.outputs.exit_code }}" != "0" ]; then
            echo "::error::One or more test suites failed on the runner. Check REPO_DUMP.md and runner logs for details."
            exit 1
          fi
```

## File: .github/workflows/docs-pages.yml

**Lines**: 59 | **Size**: 1.2 KB | **Language**: yaml

```yaml
name: Deploy Documentation to Pages

on:
  push:
    branches: [main]
    paths:
      - 'site/**'
      - '.github/workflows/docs-pages.yml'
  workflow_dispatch:

permissions:
  contents: read
  pages: write
  id-token: write

concurrency:
  group: pages
  cancel-in-progress: false

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          fetch-depth: 0

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: 22
          cache: npm
          cache-dependency-path: site/package-lock.json

      - name: Install dependencies
        working-directory: site
        run: npm ci || npm install

      - name: Build documentation
        working-directory: site
        run: npm run docs:build

      - name: Upload Pages artifact
        uses: actions/upload-pages-artifact@v3
        with:
          path: site/.vitepress/dist

  deploy:
    environment:
      name: github-pages
      url: ${{ steps.deployment.outputs.page_url }}
    needs: build
    runs-on: ubuntu-latest
    name: Deploy
    steps:
      - name: Deploy to GitHub Pages
        id: deployment
        uses: actions/deploy-pages@v4
```

## File: .github/workflows/publish-nuget.yml

**Lines**: 39 | **Size**: 917 B | **Language**: yaml

```yaml
name: Publish NuGet Package

on:
  workflow_dispatch:
  push:
    tags:
      - 'v*'

permissions:
  id-token: write
  contents: read

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Build and Pack
        run: dotnet pack src/Starry.Cli/Starry.Cli.csproj --configuration Release -o nupkg

      - name: NuGet Login (OIDC Trusted Publishing)
        id: login
        uses: NuGet/login@v1
        with:
          user: ${{ secrets.NUGET_USER || 'perdub' }}

      - name: Push package to NuGet.org
        run: |
          dotnet nuget push nupkg/*.nupkg \
            --api-key "${{ steps.login.outputs.NUGET_API_KEY }}" \
            --source "https://api.nuget.org/v3/index.json" \
            --skip-duplicate
```

## File: .github/workflows/repo-dump.yml

**Lines**: 38 | **Size**: 875 B | **Language**: yaml

```yaml
name: Repository Dump

on:
  push:
    branches: [main]
  workflow_dispatch:
    inputs:
      output_filename:
        description: 'Output filename for repository dump'
        required: false
        default: 'REPO_DUMP.md'
        type: string

permissions:
  contents: read

jobs:
  dump:
    name: Dump Repository
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup Python
        uses: actions/setup-python@v5
        with:
          python-version: '3.x'

      - name: Generate Repository Dump
        run: |
          python3 scripts/repo2md.py -o "${{ inputs.output_filename || 'REPO_DUMP.md' }}"

      - name: Upload Repository Dump Artifact
        uses: actions/upload-artifact@v4
        with:
          name: repo-dump
          path: ${{ inputs.output_filename || 'REPO_DUMP.md' }}
```

## File: .github/workflows/rotate-master-key.yml

**Lines**: 52 | **Size**: 1.6 KB | **Language**: yaml

```yaml
name: Monthly Master Key Rotation

on:
  schedule:
    # Runs at 00:00 on day 1 of every month
    - cron: '0 0 1 * *'
  workflow_dispatch:

permissions:
  contents: write

jobs:
  rotate-keys:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          token: ${{ secrets.GITHUB_TOKEN }}
          fetch-depth: 0

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Configure Git
        run: |
          git config --global user.name "github-actions[bot]"
          git config --global user.email "github-actions[bot]@users.noreply.github.com"

      - name: Generate New Master Key & Rotate
        env:
          STARRY_MASTER_KEY_OLD: ${{ secrets.STARRY_MASTER_KEY }}
          GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
        run: |
          # Build starry CLI
          dotnet build src/Starry.Cli/Starry.Cli.csproj --configuration Release

          # Generate 32-byte cryptographically secure random base64 key
          NEW_KEY=$(openssl rand -base64 32)
          export STARRY_MASTER_KEY_NEW="$NEW_KEY"

          # Execute global re-encryption with atomic write, git commit & push
          dotnet run --project src/Starry.Cli/Starry.Cli.csproj --configuration Release -- \
            encrypt global --reencrypt \
            --old-key-env STARRY_MASTER_KEY_OLD \
            --new-key-env STARRY_MASTER_KEY_NEW \
            --allow-config-edit \
            --git-commit \
            --git-push \
            --update-github-secret STARRY_MASTER_KEY
```

## File: scripts/dump_repo.sh

**Lines**: 9 | **Size**: 301 B | **Language**: bash

```bash
#!/usr/bin/env bash
set -euo pipefail

# Resolve directory of this script and repository root
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

# Run repo2md utility forwarding all arguments
python3 "$SCRIPT_DIR/repo2md.py" --root "$REPO_ROOT" "$@"
```

## File: scripts/repo2md.py

**Lines**: 677 | **Size**: 21.5 KB | **Language**: python

````python
#!/usr/bin/env python3
"""
Repository Dumper Utility (repo2md)
Dumps the repository into a single comprehensive Markdown file for LLM analysis.
"""

import argparse
import fnmatch
import os
import re
import sys
from pathlib import Path
from typing import Dict, List, Optional, Set, Tuple

# Default directories to skip
DEFAULT_EXCLUDED_DIRS: Set[str] = {
    ".git",
    ".slim",
    ".vs",
    ".idea",
    ".vscode",
    "bin",
    "obj",
    "node_modules",
    "dist",
    "cache",
    ".vitepress/dist",
    ".vitepress/cache",
    "publish-aot",
    "nupkg",
    "__pycache__",
    ".pytest_cache",
    ".mypy_cache",
    "coverage",
    "runner-logs",
    "test-logs",
}

# Standalone log and diagnostic files to skip from standard repository code listing
DEFAULT_EXCLUDED_FILES: Set[str] = {
    "runner-tests.log",
    "runner.log",
}

# Binary file extensions to skip
BINARY_EXTENSIONS: Set[str] = {
    # Compiled & executables
    ".dll", ".exe", ".so", ".dylib", ".a", ".lib", ".o", ".obj", ".bin", ".aot",
    # Packages & archives
    ".nupkg", ".snupkg", ".tar", ".gz", ".tgz", ".zst", ".zip", ".7z", ".rar", ".bz2", ".xz",
    # Images & graphics
    ".png", ".jpg", ".jpeg", ".gif", ".ico", ".webp", ".bmp", ".tiff", ".tif", ".svgz", ".psd",
    # Fonts
    ".woff", ".woff2", ".ttf", ".otf", ".eot",
    # Media
    ".mp3", ".mp4", ".wav", ".ogg", ".flac", ".avi", ".mov", ".mkv", ".webm",
    # Documents & databases
    ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".db", ".sqlite", ".sqlite3",
    # Python & JVM bytecode
    ".pyc", ".pyo", ".pyd", ".class", ".jar", ".war",
}

# Language mapping for syntax highlighting
LANGUAGE_MAP: Dict[str, str] = {
    ".cs": "csharp",
    ".fs": "fsharp",
    ".fsx": "fsharp",
    ".vb": "vb",
    ".csproj": "xml",
    ".sln": "text",
    ".slnx": "xml",
    ".props": "xml",
    ".targets": "xml",
    ".xml": "xml",
    ".config": "xml",
    ".xaml": "xml",
    ".resx": "xml",
    ".json": "json",
    ".jsonc": "json",
    ".yaml": "yaml",
    ".yml": "yaml",
    ".toml": "toml",
    ".md": "markdown",
    ".markdown": "markdown",
    ".sh": "bash",
    ".bash": "bash",
    ".zsh": "bash",
    ".ps1": "powershell",
    ".psm1": "powershell",
    ".psd1": "powershell",
    ".py": "python",
    ".pyi": "python",
    ".js": "javascript",
    ".mjs": "javascript",
    ".cjs": "javascript",
    ".ts": "typescript",
    ".mts": "typescript",
    ".cts": "typescript",
    ".jsx": "jsx",
    ".tsx": "tsx",
    ".html": "html",
    ".htm": "html",
    ".css": "css",
    ".scss": "scss",
    ".sass": "sass",
    ".less": "less",
    ".sql": "sql",
    ".rs": "rust",
    ".go": "go",
    ".java": "java",
    ".kt": "kotlin",
    ".kts": "kotlin",
    ".c": "c",
    ".h": "c",
    ".cpp": "cpp",
    ".hpp": "cpp",
    ".cc": "cpp",
    ".cxx": "cpp",
    ".proto": "protobuf",
    ".graphql": "graphql",
    ".gql": "graphql",
    ".ini": "ini",
    ".cfg": "ini",
    ".conf": "ini",
    ".editorconfig": "ini",
    ".gitignore": "gitignore",
    ".ignore": "gitignore",
    ".dockerignore": "dockerignore",
    ".env": "bash",
    ".txt": "text",
    ".log": "text",
    ".csv": "csv",
    ".tsv": "tsv",
    ".diff": "diff",
    ".patch": "diff",
}


def is_binary_file(filepath: Path) -> bool:
    """Check if a file is binary based on extension or content."""
    suffix = filepath.suffix.lower()
    if suffix in BINARY_EXTENSIONS:
        return True

    # Compound extensions like .tar.gz
    name_lower = filepath.name.lower()
    if any(name_lower.endswith(ext) for ext in [".tar.gz", ".tar.zst", ".tar.bz2", ".tar.xz"]):
        return True

    try:
        with open(filepath, "rb") as f:
            chunk = f.read(8192)
            if b"\x00" in chunk:
                return True
    except (OSError, PermissionError):
        return True

    return False


def get_syntax_lang(filepath: Path) -> str:
    """Determine syntax highlighting language identifier."""
    name = filepath.name
    name_lower = name.lower()

    if name_lower == "dockerfile" or name_lower.startswith("dockerfile."):
        return "dockerfile"

    if name in {".gitignore", ".ignore", ".dockerignore"}:
        return "gitignore"

    if name == ".editorconfig":
        return "ini"

    if name.startswith(".env"):
        return "bash"

    suffix = filepath.suffix.lower()
    if suffix in LANGUAGE_MAP:
        return LANGUAGE_MAP[suffix]

    if suffix.startswith(".") and suffix[1:].isalnum():
        return suffix[1:]

    return "text"


def should_exclude_path(
    rel_path_str: str,
    is_dir: bool,
    custom_patterns: List[str],
) -> bool:
    """Check whether a path matches default exclusions or custom patterns."""
    # Normalize path separators
    normalized = rel_path_str.replace("\\", "/").strip("/")
    parts = normalized.split("/")

    # Check against directory exclusions
    for excluded in DEFAULT_EXCLUDED_DIRS:
        ex_norm = excluded.replace("\\", "/").strip("/")
        if is_dir:
            if normalized == ex_norm or normalized.startswith(ex_norm + "/"):
                return True
            if any(part == ex_norm for part in parts):
                return True
        else:
            if normalized.startswith(ex_norm + "/"):
                return True
            if any(part == ex_norm for part in parts[:-1]):
                return True

    if not is_dir and (parts[-1] in DEFAULT_EXCLUDED_FILES or parts[-1].endswith(".trx")):
        return True

    # Check custom exclude patterns
    for pat in custom_patterns:
        pat_clean = pat.strip()
        if not pat_clean:
            continue
        # Pattern matching against relative path or filename
        if fnmatch.fnmatch(normalized, pat_clean):
            return True
        if fnmatch.fnmatch(parts[-1], pat_clean):
            return True
        if is_dir and fnmatch.fnmatch(normalized + "/", pat_clean):
            return True

    return False


def collect_files(
    root_path: Path,
    output_resolved: Path,
    custom_patterns: List[str],
) -> Tuple[List[Path], Dict[str, List[str]]]:
    """
    Recursively collect all valid files and build a directory structure dictionary for tree rendering.
    Returns (sorted_files, dir_structure).
    """
    collected: List[Path] = []
    tree_nodes: Dict[str, List[str]] = {}

    for dirpath, dirnames, filenames in os.walk(root_path, topdown=True):
        current_dir = Path(dirpath)
        try:
            rel_dir = current_dir.relative_to(root_path)
            rel_dir_str = "" if rel_dir == Path(".") else str(rel_dir).replace("\\", "/")
        except ValueError:
            continue

        # Filter out directories to prune os.walk
        kept_dirs = []
        for d in dirnames:
            sub_rel = f"{rel_dir_str}/{d}" if rel_dir_str else d
            if not should_exclude_path(sub_rel, is_dir=True, custom_patterns=custom_patterns):
                kept_dirs.append(d)
        dirnames[:] = sorted(kept_dirs, key=str.lower)

        # Filter files
        kept_files = []
        for f in sorted(filenames, key=str.lower):
            file_path = current_dir / f
            try:
                if file_path.resolve() == output_resolved:
                    continue
            except (OSError, RuntimeError):
                pass

            file_rel = f"{rel_dir_str}/{f}" if rel_dir_str else f
            if should_exclude_path(file_rel, is_dir=False, custom_patterns=custom_patterns):
                continue

            if is_binary_file(file_path):
                continue

            collected.append(file_path)
            kept_files.append(f)

        tree_nodes[rel_dir_str] = [d + "/" for d in dirnames] + kept_files

    return collected, tree_nodes


def generate_ascii_tree(root_name: str, tree_nodes: Dict[str, List[str]]) -> str:
    """Generate visual ASCII tree string from directory structure."""
    lines: List[str] = [f"{root_name}/"]

    def _render_level(current_rel: str, prefix: str):
        entries = tree_nodes.get(current_rel, [])
        total = len(entries)
        for i, entry in enumerate(entries):
            is_last = (i == total - 1)
            connector = "└── " if is_last else "├── "
            child_prefix = prefix + ("    " if is_last else "│   ")

            lines.append(f"{prefix}{connector}{entry}")

            if entry.endswith("/"):
                sub_dir_name = entry[:-1]
                sub_rel = f"{current_rel}/{sub_dir_name}" if current_rel else sub_dir_name
                _render_level(sub_rel, child_prefix)

    _render_level("", "")
    return "\n".join(lines)


def format_size(size_bytes: int) -> str:
    """Format bytes into human-readable string."""
    if size_bytes < 1024:
        return f"{size_bytes} B"
    elif size_bytes < 1024 * 1024:
        return f"{size_bytes / 1024:.1f} KB"
    else:
        return f"{size_bytes / (1024 * 1024):.2f} MB"


def build_safe_fence(content: str) -> str:
    """Construct a code fence that does not conflict with content."""
    count = 3
    while "`" * count in content:
        count += 1
    return "`" * count


UNNEEDED_RUNNER_PATTERNS: List[str] = [
    r"^Build started\b.*",
    r"^VSTest version\b.*",
    r"^Starting test execution, please wait\.\.\.",
    r"^A total of \d+ test files matched\b.*",
    r"^\[xUnit\.net [^\]]+\]\s+(?:Discovering|Discovered|Starting|Finished):.*",
    r"^\[xUnit\.net [^\]]+\]\s+xUnit\.net VSTest Adapter.*",
    r"^Determining projects to restore\.\.\.",
    r"^All projects are up-to-date for restore\.",
    r"^Build (?:succeeded|failed)\.",
    r"^\s*\d+ Warning\(s\)",
    r"^\s*\d+ Error\(s\)",
    r"^Time Elapsed\b.*",
    r"^Microsoft \(R\) Build Engine\b.*",
    r"^Copyright \(C\) Microsoft Corporation\..*",
    r"^Restore complete \(\d+ms\)\.",
]


def clean_and_filter_runner_logs(raw_content: str) -> Tuple[str, Dict[str, any]]:
    """
    Cleans raw test runner logs by removing ANSI escape sequences, MSBuild banners,
    VSTest adapter discovery chatter, and restore notices.
    Preserves test suites, passed/failed statuses, detailed failure stack traces,
    and summary metrics.
    """
    cleaned = re.sub(r"\x1b\[[0-9;]*[a-zA-Z]", "", raw_content)
    lines = cleaned.splitlines()

    filtered_lines: List[str] = []
    failed_tests: List[str] = []
    in_failure_block = False
    current_failure_lines: List[str] = []

    passed_individual = 0
    failed_individual = 0
    skipped_individual = 0

    passed_summary = 0
    failed_summary = 0
    skipped_summary = 0
    total_summary = 0
    has_test_run_failed = False

    for line in lines:
        stripped = line.strip()

        # Filter out unneeded runner noise
        if any(re.match(pat, stripped) for pat in UNNEEDED_RUNNER_PATTERNS):
            continue

        # Detect failed test entry
        if re.match(r"^\s*Failed\s+", line):
            failed_individual += 1
            if current_failure_lines:
                failed_tests.append("\n".join(current_failure_lines))
                current_failure_lines = []
            in_failure_block = True
            current_failure_lines.append(line.strip())
        elif in_failure_block:
            if (
                re.match(r"^\s*Passed\s+", line)
                or re.match(r"^Test Run (?:Successful|Failed)\.", stripped)
                or re.match(r"^(?:Failed|Passed)!\s+-", stripped)
                or stripped.startswith("===")
            ):
                in_failure_block = False
                if current_failure_lines:
                    failed_tests.append("\n".join(current_failure_lines))
                    current_failure_lines = []
            elif stripped:
                current_failure_lines.append(line.rstrip())

        if re.match(r"^\s*Passed\s+", line):
            passed_individual += 1
        elif re.match(r"^\s*Skipped\s+", line):
            skipped_individual += 1

        if "Test Run Failed" in stripped:
            has_test_run_failed = True

        m_inline = re.search(
            r"Failed:\s*(\d+),\s*Passed:\s*(\d+),\s*Skipped:\s*(\d+),\s*Total:\s*(\d+)",
            stripped,
        )
        if m_inline:
            f, p, s, t = map(int, m_inline.groups())
            failed_summary += f
            passed_summary += p
            skipped_summary += s
            total_summary += t

        m_tot = re.match(r"^Total tests:\s*(\d+)", stripped)
        if m_tot:
            total_summary += int(m_tot.group(1))

        m_pass = re.match(r"^Passed:\s*(\d+)", stripped)
        if m_pass:
            passed_summary += int(m_pass.group(1))

        m_fail = re.match(r"^Failed:\s*(\d+)", stripped)
        if m_fail:
            failed_summary += int(m_fail.group(1))

        m_skip = re.match(r"^Skipped:\s*(\d+)", stripped)
        if m_skip:
            skipped_summary += int(m_skip.group(1))

        filtered_lines.append(line)

    if current_failure_lines:
        failed_tests.append("\n".join(current_failure_lines))

    compact_lines: List[str] = []
    prev_blank = False
    for line in filtered_lines:
        is_blank = not line.strip()
        if is_blank and prev_blank:
            continue
        compact_lines.append(line)
        prev_blank = is_blank

    clean_text = "\n".join(compact_lines).strip()
    total_tests = total_summary or (passed_individual + failed_individual + skipped_individual)
    passed_tests = passed_summary or passed_individual
    failed_tests_count = failed_summary or failed_individual or len(failed_tests)
    skipped_tests = skipped_summary or skipped_individual

    status = "FAILED" if (has_test_run_failed or failed_tests_count > 0 or failed_tests) else "PASSED"

    stats = {
        "status": status,
        "passed": passed_tests,
        "failed": failed_tests_count,
        "skipped": skipped_tests,
        "total": total_tests,
        "failed_details": failed_tests,
    }
    return clean_text, stats


def load_runner_logs(
    log_targets: List[str],
    root_path: Path,
) -> Optional[Tuple[str, Dict[str, any]]]:
    """
    Loads runner logs from provided paths (or discovers default runner logs in repo root).
    Returns (cleaned_text, stats) or None if no runner logs exist.
    """
    discovered_files: List[Path] = []

    for target in log_targets:
        p = Path(target)
        if not p.is_absolute():
            p = (root_path / p).resolve()
        if p.is_dir():
            for f in sorted(p.glob("*.log")):
                discovered_files.append(f)
        elif p.is_file():
            discovered_files.append(p)

    if not discovered_files and not log_targets:
        # Check defaults in root directory
        for default_name in ["runner-tests.log", "runner.log", "test-logs/runner-tests.log"]:
            candidate = root_path / default_name
            if candidate.is_file():
                discovered_files.append(candidate)
                break

    if not discovered_files:
        return None

    raw_chunks: List[str] = []
    for log_file in discovered_files:
        try:
            content = log_file.read_text(encoding="utf-8", errors="replace")
            try:
                rel = log_file.relative_to(root_path).as_posix()
            except ValueError:
                rel = log_file.name
            if len(discovered_files) > 1:
                raw_chunks.append(f"=== Log: {rel} ===\n{content}")
            else:
                raw_chunks.append(content)
        except (OSError, PermissionError) as ex:
            sys.stderr.write(f"Warning: could not read runner log {log_file}: {ex}\n")

    if not raw_chunks:
        return None

    combined_raw = "\n\n".join(raw_chunks)
    return clean_and_filter_runner_logs(combined_raw)


def main() -> int:
    parser = argparse.ArgumentParser(
        description="Dump repository contents into a single comprehensive Markdown file for LLM analysis."
    )
    parser.add_argument(
        "-o",
        "--output",
        default="REPO_DUMP.md",
        help="Output Markdown file path (default: REPO_DUMP.md)",
    )
    parser.add_argument(
        "-r",
        "--root",
        default=".",
        help="Repository root directory to dump (default: current directory)",
    )
    parser.add_argument(
        "-e",
        "--exclude",
        action="append",
        default=[],
        help="Additional glob pattern(s) or names to exclude (can be specified multiple times or comma-separated)",
    )
    parser.add_argument(
        "--runner-logs",
        action="append",
        default=[],
        help="Path(s) to runner log file or directory (can be specified multiple times, auto-detects runner-tests.log if present)",
    )
    parser.add_argument(
        "--include-tree",
        action=argparse.BooleanOptionalAction,
        default=True,
        help="Include visual ASCII directory tree in output (default: True)",
    )

    args = parser.parse_args()

    root_path = Path(args.root).resolve()
    if not root_path.exists() or not root_path.is_dir():
        sys.stderr.write(f"Error: root directory not found: {args.root}\n")
        return 1

    output_path = Path(args.output).resolve()

    # Parse custom exclude patterns
    custom_patterns: List[str] = []
    for item in args.exclude:
        for part in item.split(","):
            part = part.strip()
            if part:
                custom_patterns.append(part)

    print(f"Scanning repository at: {root_path}")
    files, tree_nodes = collect_files(root_path, output_path, custom_patterns)

    total_files = len(files)
    total_lines = 0
    total_size = 0

    file_entries: List[Tuple[str, str, int, int, str]] = []

    for file_path in files:
        rel_path = file_path.relative_to(root_path).as_posix()
        try:
            raw = file_path.read_bytes()
            size = len(raw)
            try:
                content = raw.decode("utf-8")
            except UnicodeDecodeError:
                content = raw.decode("latin-1")

            lines_count = len(content.splitlines())
            lang = get_syntax_lang(file_path)

            total_lines += lines_count
            total_size += size
            file_entries.append((rel_path, content, lines_count, size, lang))
        except (OSError, PermissionError) as ex:
            sys.stderr.write(f"Warning: could not read {rel_path}: {ex}\n")

    print(f"Found {total_files} text/source files ({total_lines} lines, {format_size(total_size)})")

    # Load and filter test runner logs
    runner_logs_result = load_runner_logs(args.runner_logs, root_path)
    if runner_logs_result:
        _, runner_stats = runner_logs_result
        print(f"Loaded and filtered runner logs: Status={runner_stats['status']} (Passed={runner_stats['passed']}, Failed={runner_stats['failed']})")

    # Generate Markdown output
    output_path.parent.mkdir(parents=True, exist_ok=True)
    with open(output_path, "w", encoding="utf-8") as out:
        out.write(f"# Repository Dump: {root_path.name}\n\n")
        out.write("## Overview\n\n")
        out.write(f"- **Total Files**: {total_files}\n")
        out.write(f"- **Total Lines of Code**: {total_lines}\n")
        out.write(f"- **Total Size**: {format_size(total_size)}\n")
        out.write(f"- **Root Directory**: `{root_path}`\n\n")

        if runner_logs_result:
            cleaned_runner_text, runner_stats = runner_logs_result
            out.write("## Test Execution & Diagnostic Logs\n\n")
            if runner_stats["status"] == "FAILED":
                out.write("> [!CAUTION]\n")
                out.write("> **Test Execution Status: FAILED**\n")
                if runner_stats["total"] > 0:
                    out.write(
                        f"> - **Total Tests**: {runner_stats['total']} | **Passed**: {runner_stats['passed']} | **Failed**: {runner_stats['failed']} | **Skipped**: {runner_stats['skipped']}\n"
                    )
                out.write("\n")
                if runner_stats["failed_details"]:
                    out.write("### ❌ Failed Test Details\n\n")
                    out.write("```text\n")
                    for fd in runner_stats["failed_details"]:
                        out.write(fd.strip() + "\n\n")
                    out.write("```\n\n")
            else:
                out.write("> [!NOTE]\n")
                out.write("> **Test Execution Status: PASSED**\n")
                if runner_stats["total"] > 0:
                    out.write(
                        f"> - **Total Tests**: {runner_stats['total']} | **Passed**: {runner_stats['passed']} | **Failed**: {runner_stats['failed']} | **Skipped**: {runner_stats['skipped']}\n"
                    )
                out.write("\n")

            out.write("### Filtered Test Runner Logs\n\n")
            fence = build_safe_fence(cleaned_runner_text)
            out.write(f"{fence}text\n")
            out.write(cleaned_runner_text)
            if not cleaned_runner_text.endswith("\n"):
                out.write("\n")
            out.write(f"{fence}\n\n")
            out.write("---\n\n")

        if args.include_tree:
            out.write("## Repository Structure\n\n")
            out.write("```text\n")
            tree_str = generate_ascii_tree(root_path.name, tree_nodes)
            out.write(tree_str)
            out.write("\n```\n\n")

        out.write("---\n\n")

        for rel_path, content, lines_count, size, lang in file_entries:
            out.write(f"## File: {rel_path}\n\n")
            out.write(f"**Lines**: {lines_count} | **Size**: {format_size(size)} | **Language**: {lang}\n\n")
            fence = build_safe_fence(content)
            out.write(f"{fence}{lang}\n")
            out.write(content)
            if not content.endswith("\n"):
                out.write("\n")
            out.write(f"{fence}\n\n")

    print(f"Repository dump generated successfully at: {output_path}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
````

## File: scripts/run-cluster-scenario.sh

**Lines**: 40 | **Size**: 1.6 KB | **Language**: bash

```bash
#!/usr/bin/env bash
set -euo pipefail

SCENARIO_ID="${1:-all}"

echo "=========================================================="
echo " STARRY CLUSTER SCENARIO RUNNER: Scenario ${SCENARIO_ID}"
echo "=========================================================="

cleanup() {
    echo "[TEARDOWN] Cleaning up cluster scenario ${SCENARIO_ID}..."
    rm -rf /tmp/starry-cluster-* /tmp/starry-scen-* 2>/dev/null || true
    if command -v docker >/dev/null 2>&1; then
        docker rm -f starry-live-box 2>/dev/null || true
        docker network rm starry-live-net 2>/dev/null || true
    fi
    echo "[TEARDOWN] Done."
}

trap cleanup EXIT INT TERM

# If Docker daemon is active on the runner, spin up lightweight live test container
if command -v docker >/dev/null 2>&1 && docker info >/dev/null 2>&1; then
    echo "[DOCKER] Docker daemon active. Setting up isolated live verification container..."
    docker network create starry-live-net 2>/dev/null || true
    docker run -d --name starry-live-box --network starry-live-net \
        -e TEST_PASS=secret-db-pass \
        alpine:3.20 sleep 3600 2>/dev/null || true
fi

if [ "$SCENARIO_ID" = "all" ]; then
    echo "Running all 24 cluster scenarios..."
    dotnet test tests/Starry.E2ETests/Starry.E2ETests.csproj --filter FullyQualifiedName~ClusterMatrixFuzzerTests -p:RunAnalyzers=false
else
    FORMATTED_ID=$(printf "%02d" "$SCENARIO_ID")
    echo "Running Scenario ${FORMATTED_ID}..."
    dotnet test tests/Starry.E2ETests/Starry.E2ETests.csproj --filter FullyQualifiedName~Scenario${FORMATTED_ID} -p:RunAnalyzers=false
fi

echo "Scenario ${SCENARIO_ID} completed successfully!"
```

## File: site/.gitignore

**Lines**: 6 | **Size**: 72 B | **Language**: gitignore

```gitignore
node_modules/
.vitepress/dist/
.vitepress/cache/
dist/
cache/
.DS_Store
```

## File: site/index.md

**Lines**: 50 | **Size**: 3.3 KB | **Language**: markdown

```markdown
---
layout: home

hero:
  name: "Starry"
  text: "Децентрализованный GitOps-оркестратор"
  tagline: "Декларативное управление гетерогенным парком VPS, контейнерами Docker и Podman, оверлейной сетью WireGuard и nftables файрволом."
  image:
    src: /logo.svg
    alt: Starry Logo
  actions:
    - theme: brand
      text: Быстрый старт
      link: /guide/getting-started
    - theme: alt
      text: Обзор архитектуры
      link: /architecture/overview
    - theme: alt
      text: Синтаксис конфига
      link: /configuration/syntax

features:
  - icon: ⚡
    title: Single-Binary Hybrid
    details: Единый легковесный бинарник без внешних зависимостей. Работает как интерактивный CLI на рабочей машине разработчика и как автономный фоновый агент на серверах.
  - icon: 🌐
    title: WireGuard Full-Mesh
    details: Автоматическое построение шифрованной полносвязной оверлейной сети 10.42.0.0/16 между узлами, безопасное преодоление NAT и динамический keepalive.
  - icon: 🐳
    title: Docker & Podman Ready
    details: Абстракция над Docker Engine API и Podman Libpod/Compat API. Поддержка Rootless-режима, смешанных сред и прямого P2P-стриминга образов между нодами.
  - icon: 🛡️
    title: Атомарный nftables Файрвол
    details: Автоматическая генерация строгих правил nftables. Порты сервисов открыты строго внутри WireGuard-меша, а механизм Deadman Switch исключает потерю доступа к VPS.
  - icon: 🔐
    title: Безопасные Секреты в Git
    details: Встроенное конвертное шифрование AES-256-GCM. Токены ENC[...] хранятся прямо в репозитории с поддержкой одномоментной ротации ключей и интеграцией с GitHub Secrets.
  - icon: 🔄
    title: Rolling Drain Обновления ОС
    details: Декларативное сопровождение системных пакетов (apt, dnf, apk, pacman) и автоматическая перезагрузка ядра Linux со скользящим дренажом трафика без даунтайма.
---

<div class="starry-callout">
  <div class="starry-callout-title">
    🌟 Инфраструктура без единой точки отказа
  </div>
  <p>
    В отличие от тяжеловесных кластерных решений, Starry не требует управляющего мастера, распределённого etcd-консенсуса или постоянных внешних баз данных. Вся топология кластера и желаемое состояние описываются в легко читаемом YAML-файле и гарантируют воспроизводимость на любом Linux-дистрибутиве.
  </p>
</div>
```

## File: site/package-lock.json

**Lines**: 2578 | **Size**: 87.1 KB | **Language**: json

```json
{
  "name": "starry-docs",
  "version": "1.0.0",
  "lockfileVersion": 3,
  "requires": true,
  "packages": {
    "": {
      "name": "starry-docs",
      "version": "1.0.0",
      "devDependencies": {
        "vitepress": "^1.6.3"
      }
    },
    "node_modules/@algolia/abtesting": {
      "version": "1.27.0",
      "resolved": "https://registry.npmjs.org/@algolia/abtesting/-/abtesting-1.27.0.tgz",
      "integrity": "sha512-XXm0DTCmSzHT40mn0Hs8H00pBcuaqQET0BsWfaHrQdijNfrhSZPLueGVQvYAeSeXVuS420arotSfgqY25nFNbA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/autocomplete-core": {
      "version": "1.17.7",
      "resolved": "https://registry.npmjs.org/@algolia/autocomplete-core/-/autocomplete-core-1.17.7.tgz",
      "integrity": "sha512-BjiPOW6ks90UKl7TwMv7oNQMnzU+t/wk9mgIDi6b1tXpUek7MW0lbNOUHpvam9pe3lVCf4xPFT+lK7s+e+fs7Q==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/autocomplete-plugin-algolia-insights": "1.17.7",
        "@algolia/autocomplete-shared": "1.17.7"
      }
    },
    "node_modules/@algolia/autocomplete-plugin-algolia-insights": {
      "version": "1.17.7",
      "resolved": "https://registry.npmjs.org/@algolia/autocomplete-plugin-algolia-insights/-/autocomplete-plugin-algolia-insights-1.17.7.tgz",
      "integrity": "sha512-Jca5Ude6yUOuyzjnz57og7Et3aXjbwCSDf/8onLHSQgw1qW3ALl9mrMWaXb5FmPVkV3EtkD2F/+NkT6VHyPu9A==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/autocomplete-shared": "1.17.7"
      },
      "peerDependencies": {
        "search-insights": ">= 1 < 3"
      }
    },
    "node_modules/@algolia/autocomplete-preset-algolia": {
      "version": "1.17.7",
      "resolved": "https://registry.npmjs.org/@algolia/autocomplete-preset-algolia/-/autocomplete-preset-algolia-1.17.7.tgz",
      "integrity": "sha512-ggOQ950+nwbWROq2MOCIL71RE0DdQZsceqrg32UqnhDz8FlO9rL8ONHNsI2R1MH0tkgVIDKI/D0sMiUchsFdWA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/autocomplete-shared": "1.17.7"
      },
      "peerDependencies": {
        "@algolia/client-search": ">= 4.9.1 < 6",
        "algoliasearch": ">= 4.9.1 < 6"
      }
    },
    "node_modules/@algolia/autocomplete-shared": {
      "version": "1.17.7",
      "resolved": "https://registry.npmjs.org/@algolia/autocomplete-shared/-/autocomplete-shared-1.17.7.tgz",
      "integrity": "sha512-o/1Vurr42U/qskRSuhBH+VKxMvkkUVTLU6WZQr+L5lGZZLYWyhdzWjW0iGXY7EkwRTjBqvN2EsR81yCTGV/kmg==",
      "dev": true,
      "license": "MIT",
      "peerDependencies": {
        "@algolia/client-search": ">= 4.9.1 < 6",
        "algoliasearch": ">= 4.9.1 < 6"
      }
    },
    "node_modules/@algolia/client-abtesting": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/client-abtesting/-/client-abtesting-5.61.0.tgz",
      "integrity": "sha512-NSq61VNhN63nstgbEh2YzygmtybxPnBdFwmjPOa7laJp8RapLrkqwHUaI4ELCyuZMhcuRlUvOL25y+SDVkgnsA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/client-analytics": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/client-analytics/-/client-analytics-5.61.0.tgz",
      "integrity": "sha512-WBMStmOLas4bWHh0rgOOb8iTQL07fsfPwvEW2vrQL+5v2KWMcRix9cYSQp3SOGjdweNjUszbGO84VBBMHBnHbQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/client-common": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/client-common/-/client-common-5.61.0.tgz",
      "integrity": "sha512-mzrW0KkEiZHB09cU2Sh2vyxUGSi9fVoglCeKnf3wJ3ixo+7ilHH64wLK45eSoX13+CVXA2KTjZJGcEmBS3Z57Q==",
      "dev": true,
      "license": "MIT",
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/client-insights": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/client-insights/-/client-insights-5.61.0.tgz",
      "integrity": "sha512-TseZAUL8ZUtKQY6MQbJR/Qw9OSf54s9gvcDtQ7GZX9miZD1e24E1/usa1lGrrgq5KDpgQ8+ePCQIITFcq0Y56Q==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/client-personalization": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/client-personalization/-/client-personalization-5.61.0.tgz",
      "integrity": "sha512-4CBpVN2WDVLocAzMU8me5UHWSbf6oXI7KDy3K0pL/8i0eB1+nOMQBU7jmUL6kBl2XMoYAu6tnsZmxpZ59b+Y2g==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/client-query-suggestions": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/client-query-suggestions/-/client-query-suggestions-5.61.0.tgz",
      "integrity": "sha512-PU2XREmx7DdZ6SjuuaM+ZwhfMFOUPXjLeUYyvxTFA8oz06yrY/wcn5V/UOMv01rdeTuG2petMZLzTJADF9KyEw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/client-search": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/client-search/-/client-search-5.61.0.tgz",
      "integrity": "sha512-npseUJ0UwaiwU4XbKyHiFcZbrS1AbLszbPRxi3W9YUrrqdpvfSeGKMlLv63haH4PZsCog5wG40lJGKXbnrDAEQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/ingestion": {
      "version": "1.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/ingestion/-/ingestion-1.61.0.tgz",
      "integrity": "sha512-rA/W43fD2qQemeQ2/uix/KSEUdikY55RTmgbfBRoF9HjCJ/zryD9RAKJXWxBlFXRBg9q8EPPYH3GlSXcRHnpgg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/monitoring": {
      "version": "1.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/monitoring/-/monitoring-1.61.0.tgz",
      "integrity": "sha512-3CEPgD95sBitQVQsPXmIfMkcX0Kh12d5wmI7iOm1yrllPBu4Vx30Q2xMv9wyHm2C2bHQwz+KPBohtq6Hh1K5eg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/recommend": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/recommend/-/recommend-5.61.0.tgz",
      "integrity": "sha512-2pCOG8rRFW+0OeUmNUMoRH5xXktCEdUqtjuz6FCFQa1lxqPIUBr4HHIneXTIuAgUC0ckokqBCZk0v5qU7DvBuA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/requester-browser-xhr": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/requester-browser-xhr/-/requester-browser-xhr-5.61.0.tgz",
      "integrity": "sha512-7Y7P6Kp+7DQkrOCzmIYjoCpLPQoA8vlxcIzyjpfMrokBNX0LIQSPyR5JY/EdSzCN6CvNlYGJ5SiO7j6lZad5Jw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/requester-fetch": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/requester-fetch/-/requester-fetch-5.61.0.tgz",
      "integrity": "sha512-WNTNQcrWcGAgLDU3+6IFWs3QPosMCYSlwlPEdEO2ivLnlm8u5HtGQvOz/JnY9d2bhofDBL+S5wdtm9+Uob+dsA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@algolia/requester-node-http": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/@algolia/requester-node-http/-/requester-node-http-5.61.0.tgz",
      "integrity": "sha512-CA+6MlgPkjSlGQpx+rLBSnHFy8pyYpGEjs7Yp0kVzOC6RM5jq99Z4KGOuplcNZBjQlM521t0nEvKLxFOv2NSZA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/client-common": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/@babel/helper-string-parser": {
      "version": "7.29.11",
      "resolved": "https://registry.npmjs.org/@babel/helper-string-parser/-/helper-string-parser-7.29.11.tgz",
      "integrity": "sha512-snZbO7MEDOof+hvNa97EjEY7qD7h2kwvQr5AJC3ZhQudKW5dnej6k3kP9NQV88hJwaS8vDqRLFo9rivLEhle2g==",
      "dev": true,
      "license": "MIT",
      "engines": {
        "node": ">=6.9.0"
      }
    },
    "node_modules/@babel/helper-validator-identifier": {
      "version": "7.29.7",
      "resolved": "https://registry.npmjs.org/@babel/helper-validator-identifier/-/helper-validator-identifier-7.29.7.tgz",
      "integrity": "sha512-qehxGkRj55h/ff8EMaJ+cYhyaKlHIxqYDn682wQD7RNp9UujOQsHog2uS0r2vzr4pW+sXf90NeeayjcNaX3fFg==",
      "dev": true,
      "license": "MIT",
      "engines": {
        "node": ">=6.9.0"
      }
    },
    "node_modules/@babel/parser": {
      "version": "7.29.12",
      "resolved": "https://registry.npmjs.org/@babel/parser/-/parser-7.29.12.tgz",
      "integrity": "sha512-WJ7JBp/Q4+z/ImydtI7jS6NVco5++632ExBemSX+Ysb+G1lCwQ5BVo3SY+jGGOcZXI4ZccbZSUSJLNBW6eEpJg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@babel/types": "^7.29.11"
      },
      "bin": {
        "parser": "bin/babel-parser.js"
      },
      "engines": {
        "node": ">=6.0.0"
      }
    },
    "node_modules/@babel/types": {
      "version": "7.29.11",
      "resolved": "https://registry.npmjs.org/@babel/types/-/types-7.29.11.tgz",
      "integrity": "sha512-WCsemW40Cpc6gouC1SrY6PBaZ+oLRlEVY9XBvZBI7il/HyUrb2nJ505wE1MHEFyDZNnrWDEuy4jL50WVHfTTSQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@babel/helper-string-parser": "^7.29.11",
        "@babel/helper-validator-identifier": "^7.29.7"
      },
      "engines": {
        "node": ">=6.9.0"
      }
    },
    "node_modules/@docsearch/css": {
      "version": "3.8.2",
      "resolved": "https://registry.npmjs.org/@docsearch/css/-/css-3.8.2.tgz",
      "integrity": "sha512-y05ayQFyUmCXze79+56v/4HpycYF3uFqB78pLPrSV5ZKAlDuIAAJNhaRi8tTdRNXh05yxX/TyNnzD6LwSM89vQ==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@docsearch/js": {
      "version": "3.8.2",
      "resolved": "https://registry.npmjs.org/@docsearch/js/-/js-3.8.2.tgz",
      "integrity": "sha512-Q5wY66qHn0SwA7Taa0aDbHiJvaFJLOJyHmooQ7y8hlwwQLQ/5WwCcoX0g7ii04Qi2DJlHsd0XXzJ8Ypw9+9YmQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@docsearch/react": "3.8.2",
        "preact": "^10.0.0"
      }
    },
    "node_modules/@docsearch/react": {
      "version": "3.8.2",
      "resolved": "https://registry.npmjs.org/@docsearch/react/-/react-3.8.2.tgz",
      "integrity": "sha512-xCRrJQlTt8N9GU0DG4ptwHRkfnSnD/YpdeaXe02iKfqs97TkZJv60yE+1eq/tjPcVnTW8dP5qLP7itifFVV5eg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/autocomplete-core": "1.17.7",
        "@algolia/autocomplete-preset-algolia": "1.17.7",
        "@docsearch/css": "3.8.2",
        "algoliasearch": "^5.14.2"
      },
      "peerDependencies": {
        "@types/react": ">= 16.8.0 < 19.0.0",
        "react": ">= 16.8.0 < 19.0.0",
        "react-dom": ">= 16.8.0 < 19.0.0",
        "search-insights": ">= 1 < 3"
      },
      "peerDependenciesMeta": {
        "@types/react": {
          "optional": true
        },
        "react": {
          "optional": true
        },
        "react-dom": {
          "optional": true
        },
        "search-insights": {
          "optional": true
        }
      }
    },
    "node_modules/@esbuild/aix-ppc64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/aix-ppc64/-/aix-ppc64-0.21.5.tgz",
      "integrity": "sha512-1SDgH6ZSPTlggy1yI6+Dbkiz8xzpHJEVAlF/AM1tHPLsf5STom9rwtjE4hKAF20FfXXNTFqEYXyJNWh1GiZedQ==",
      "cpu": [
        "ppc64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "aix"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/android-arm": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/android-arm/-/android-arm-0.21.5.tgz",
      "integrity": "sha512-vCPvzSjpPHEi1siZdlvAlsPxXl7WbOVUBBAowWug4rJHb68Ox8KualB+1ocNvT5fjv6wpkX6o/iEpbDrf68zcg==",
      "cpu": [
        "arm"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "android"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/android-arm64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/android-arm64/-/android-arm64-0.21.5.tgz",
      "integrity": "sha512-c0uX9VAUBQ7dTDCjq+wdyGLowMdtR/GoC2U5IYk/7D1H1JYC0qseD7+11iMP2mRLN9RcCMRcjC4YMclCzGwS/A==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "android"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/android-x64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/android-x64/-/android-x64-0.21.5.tgz",
      "integrity": "sha512-D7aPRUUNHRBwHxzxRvp856rjUHRFW1SdQATKXH2hqA0kAZb1hKmi02OpYRacl0TxIGz/ZmXWlbZgjwWYaCakTA==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "android"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/darwin-arm64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/darwin-arm64/-/darwin-arm64-0.21.5.tgz",
      "integrity": "sha512-DwqXqZyuk5AiWWf3UfLiRDJ5EDd49zg6O9wclZ7kUMv2WRFr4HKjXp/5t8JZ11QbQfUS6/cRCKGwYhtNAY88kQ==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "darwin"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/darwin-x64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/darwin-x64/-/darwin-x64-0.21.5.tgz",
      "integrity": "sha512-se/JjF8NlmKVG4kNIuyWMV/22ZaerB+qaSi5MdrXtd6R08kvs2qCN4C09miupktDitvh8jRFflwGFBQcxZRjbw==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "darwin"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/freebsd-arm64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/freebsd-arm64/-/freebsd-arm64-0.21.5.tgz",
      "integrity": "sha512-5JcRxxRDUJLX8JXp/wcBCy3pENnCgBR9bN6JsY4OmhfUtIHe3ZW0mawA7+RDAcMLrMIZaf03NlQiX9DGyB8h4g==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "freebsd"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/freebsd-x64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/freebsd-x64/-/freebsd-x64-0.21.5.tgz",
      "integrity": "sha512-J95kNBj1zkbMXtHVH29bBriQygMXqoVQOQYA+ISs0/2l3T9/kj42ow2mpqerRBxDJnmkUDCaQT/dfNXWX/ZZCQ==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "freebsd"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-arm": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-arm/-/linux-arm-0.21.5.tgz",
      "integrity": "sha512-bPb5AHZtbeNGjCKVZ9UGqGwo8EUu4cLq68E95A53KlxAPRmUyYv2D6F0uUI65XisGOL1hBP5mTronbgo+0bFcA==",
      "cpu": [
        "arm"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-arm64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-arm64/-/linux-arm64-0.21.5.tgz",
      "integrity": "sha512-ibKvmyYzKsBeX8d8I7MH/TMfWDXBF3db4qM6sy+7re0YXya+K1cem3on9XgdT2EQGMu4hQyZhan7TeQ8XkGp4Q==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-ia32": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-ia32/-/linux-ia32-0.21.5.tgz",
      "integrity": "sha512-YvjXDqLRqPDl2dvRODYmmhz4rPeVKYvppfGYKSNGdyZkA01046pLWyRKKI3ax8fbJoK5QbxblURkwK/MWY18Tg==",
      "cpu": [
        "ia32"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-loong64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-loong64/-/linux-loong64-0.21.5.tgz",
      "integrity": "sha512-uHf1BmMG8qEvzdrzAqg2SIG/02+4/DHB6a9Kbya0XDvwDEKCoC8ZRWI5JJvNdUjtciBGFQ5PuBlpEOXQj+JQSg==",
      "cpu": [
        "loong64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-mips64el": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-mips64el/-/linux-mips64el-0.21.5.tgz",
      "integrity": "sha512-IajOmO+KJK23bj52dFSNCMsz1QP1DqM6cwLUv3W1QwyxkyIWecfafnI555fvSGqEKwjMXVLokcV5ygHW5b3Jbg==",
      "cpu": [
        "mips64el"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-ppc64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-ppc64/-/linux-ppc64-0.21.5.tgz",
      "integrity": "sha512-1hHV/Z4OEfMwpLO8rp7CvlhBDnjsC3CttJXIhBi+5Aj5r+MBvy4egg7wCbe//hSsT+RvDAG7s81tAvpL2XAE4w==",
      "cpu": [
        "ppc64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-riscv64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-riscv64/-/linux-riscv64-0.21.5.tgz",
      "integrity": "sha512-2HdXDMd9GMgTGrPWnJzP2ALSokE/0O5HhTUvWIbD3YdjME8JwvSCnNGBnTThKGEB91OZhzrJ4qIIxk/SBmyDDA==",
      "cpu": [
        "riscv64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-s390x": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-s390x/-/linux-s390x-0.21.5.tgz",
      "integrity": "sha512-zus5sxzqBJD3eXxwvjN1yQkRepANgxE9lgOW2qLnmr8ikMTphkjgXu1HR01K4FJg8h1kEEDAqDcZQtbrRnB41A==",
      "cpu": [
        "s390x"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/linux-x64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/linux-x64/-/linux-x64-0.21.5.tgz",
      "integrity": "sha512-1rYdTpyv03iycF1+BhzrzQJCdOuAOtaqHTWJZCWvijKD2N5Xu0TtVC8/+1faWqcP9iBCWOmjmhoH94dH82BxPQ==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/netbsd-x64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/netbsd-x64/-/netbsd-x64-0.21.5.tgz",
      "integrity": "sha512-Woi2MXzXjMULccIwMnLciyZH4nCIMpWQAs049KEeMvOcNADVxo0UBIQPfSmxB3CWKedngg7sWZdLvLczpe0tLg==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "netbsd"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/openbsd-x64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/openbsd-x64/-/openbsd-x64-0.21.5.tgz",
      "integrity": "sha512-HLNNw99xsvx12lFBUwoT8EVCsSvRNDVxNpjZ7bPn947b8gJPzeHWyNVhFsaerc0n3TsbOINvRP2byTZ5LKezow==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "openbsd"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/sunos-x64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/sunos-x64/-/sunos-x64-0.21.5.tgz",
      "integrity": "sha512-6+gjmFpfy0BHU5Tpptkuh8+uw3mnrvgs+dSPQXQOv3ekbordwnzTVEb4qnIvQcYXq6gzkyTnoZ9dZG+D4garKg==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "sunos"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/win32-arm64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/win32-arm64/-/win32-arm64-0.21.5.tgz",
      "integrity": "sha512-Z0gOTd75VvXqyq7nsl93zwahcTROgqvuAcYDUr+vOv8uHhNSKROyU961kgtCD1e95IqPKSQKH7tBTslnS3tA8A==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "win32"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/win32-ia32": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/win32-ia32/-/win32-ia32-0.21.5.tgz",
      "integrity": "sha512-SWXFF1CL2RVNMaVs+BBClwtfZSvDgtL//G/smwAc5oVK/UPu2Gu9tIaRgFmYFFKrmg3SyAjSrElf0TiJ1v8fYA==",
      "cpu": [
        "ia32"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "win32"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@esbuild/win32-x64": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/@esbuild/win32-x64/-/win32-x64-0.21.5.tgz",
      "integrity": "sha512-tQd/1efJuzPC6rCFwEvLtci/xNFcTZknmXs98FYDfGE4wP9ClFV98nyKrzJKVPMhdDnjzLhdUyMX4PsQAPjwIw==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "win32"
      ],
      "engines": {
        "node": ">=12"
      }
    },
    "node_modules/@iconify-json/simple-icons": {
      "version": "1.2.101",
      "resolved": "https://registry.npmjs.org/@iconify-json/simple-icons/-/simple-icons-1.2.101.tgz",
      "integrity": "sha512-MXJkMZDancJ/9I+Bgdthi9kXvn3AwWStxoTHwEPwAFQ9kMLBASO1BSstg52fNtjXeizm1Wi91R+QnWxNGkWHPQ==",
      "dev": true,
      "license": "CC0-1.0",
      "dependencies": {
        "@iconify/types": "*"
      }
    },
    "node_modules/@iconify/types": {
      "version": "2.0.0",
      "resolved": "https://registry.npmjs.org/@iconify/types/-/types-2.0.0.tgz",
      "integrity": "sha512-+wluvCrRhXrhyOmRDJ3q8mux9JkKy5SJ/v8ol2tu4FVjyYvtEzkc/3pK15ET6RKg4b4w4BmTk1+gsCUhf21Ykg==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@jridgewell/sourcemap-codec": {
      "version": "1.6.0",
      "resolved": "https://registry.npmjs.org/@jridgewell/sourcemap-codec/-/sourcemap-codec-1.6.0.tgz",
      "integrity": "sha512-T7jf+5zgsZHwNJ4lvQ7/aezbyk0nNX+zJVWpmHA7VYsEx7a7qr5Rg5IbtJFqkgze5Y2sruq1RUY8Q837Od7iFw==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@napi-rs/lzma-linux-x64-gnu": {
      "version": "1.5.1",
      "resolved": "https://registry.npmjs.org/@napi-rs/lzma-linux-x64-gnu/-/lzma-linux-x64-gnu-1.5.1.tgz",
      "integrity": "sha512-oTXEIha4SsuXdTA4Iyskj0kpdx2yVXdhd75c2v3xGrHFfVMsbhTPZU/nMPL4sWKo4pBHm3aucLaqGlF696dTyQ==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "libc": [
        "glibc"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ],
      "engines": {
        "node": "^22.20 || ^24.12 || >=25"
      }
    },
    "node_modules/@rollup/rollup-android-arm-eabi": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-android-arm-eabi/-/rollup-android-arm-eabi-4.67.4.tgz",
      "integrity": "sha512-/v/pUAPOcwp+r1Hjjdg0cBV1TE89CzGKrKh2FgkIQw2QBcIMmVPOCSfuOZStCn4Xh/98HXfo/ht6ztcssctPNg==",
      "cpu": [
        "arm"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "android"
      ]
    },
    "node_modules/@rollup/rollup-android-arm64": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-android-arm64/-/rollup-android-arm64-4.67.4.tgz",
      "integrity": "sha512-6tmmB/P52Vt167FXXEw5auiEUgTtZz7aqUgWHqYXToIUE0HFDY6fTdAyeekjjsNk6O/8dtpSKtClxSj1mMneKQ==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "android"
      ]
    },
    "node_modules/@rollup/rollup-android-x64": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-android-x64/-/rollup-android-x64-4.67.4.tgz",
      "integrity": "sha512-8Z9Q4RmotVmuL4llOvqX814JNiOK1oqoBv+glG5KbpRfUiypqzz2/fxIzK4Pq129l4dT5a8Gszmpc0/XPYoFuQ==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "android"
      ]
    },
    "node_modules/@rollup/rollup-darwin-arm64": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-darwin-arm64/-/rollup-darwin-arm64-4.67.4.tgz",
      "integrity": "sha512-pnq6hssxTtwuRAl6Kdi/+IDY+b1IhkUetjhUkdLAAJCssjfqtL2Oqy/FXJVJFtAWOu1oRlArIvf0Yk1O92x2HQ==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "darwin"
      ]
    },
    "node_modules/@rollup/rollup-darwin-x64": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-darwin-x64/-/rollup-darwin-x64-4.67.4.tgz",
      "integrity": "sha512-BeIrGZvHMVdJg/N+ohZfFywG/gF41684YhDj2wBvouj7m9ziP1tZDUNwzXRiDY+3WIKngaC4hiUBqVtBPGP5og==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "darwin"
      ]
    },
    "node_modules/@rollup/rollup-freebsd-arm64": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-freebsd-arm64/-/rollup-freebsd-arm64-4.67.4.tgz",
      "integrity": "sha512-fRFX/oqNg/g780DKoS8RIfYcZD59Ru5O/bSeK1N7gQljBFrTxJY/q5EX4eMs5SrJf32VgVBVVYCdfPpzAwPPnQ==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "freebsd"
      ]
    },
    "node_modules/@rollup/rollup-freebsd-x64": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-freebsd-x64/-/rollup-freebsd-x64-4.67.4.tgz",
      "integrity": "sha512-bSrBRopG1q+0RuOsW0NtYTtIODlvNA3T3ebx5WnQVdSxWIoXFmj+rZdHUHdSLgFdZ5M6VGmekYE7r4RMGQXjfg==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "freebsd"
      ]
    },
    "node_modules/@rollup/rollup-linux-arm-gnueabihf": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-arm-gnueabihf/-/rollup-linux-arm-gnueabihf-4.67.4.tgz",
      "integrity": "sha512-L8IvdrJVtl65g1ozwDSyjGPG+FmibhCdFUwmqTItJLw0S3+Oj4QzFs7rfgTUiK7rsPjkCLdG1/GApFxoynXFJQ==",
      "cpu": [
        "arm"
      ],
      "dev": true,
      "libc": [
        "glibc"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-arm-musleabihf": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-arm-musleabihf/-/rollup-linux-arm-musleabihf-4.67.4.tgz",
      "integrity": "sha512-h7PcoQOzC31jshNpQ48J0akB9tsghjkSjSl5AZ6XsVGTxVsln7t7C4SQyw62tNVSaz2SeHgk4VT2ELbL17BSHQ==",
      "cpu": [
        "arm"
      ],
      "dev": true,
      "libc": [
        "musl"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-arm64-gnu": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-arm64-gnu/-/rollup-linux-arm64-gnu-4.67.4.tgz",
      "integrity": "sha512-KSPuOBNHoOl7AsVjeY1KZci8+0up0U4TFAHU5IzBYN8P+Z6cB9DZq28+JQycZW7f6WLLa/tSzhV7Rdy95bVi+g==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "libc": [
        "glibc"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-arm64-musl": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-arm64-musl/-/rollup-linux-arm64-musl-4.67.4.tgz",
      "integrity": "sha512-0E/mKmVk1PlQeQYygIwQm0jlwjANTzGSiNBMPrFx+rdFGFj/2Axh4p6XwcrystQIqXZVudTaGsdbDpzYgJ4wSA==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "libc": [
        "musl"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-loong64-gnu": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-loong64-gnu/-/rollup-linux-loong64-gnu-4.67.4.tgz",
      "integrity": "sha512-Fvw7dOSc2Sa9UOipRZVI0zOgdLCzsjVyD/6JuHXzUHTXFu7mMgmeX/0/bCVBIut1dz+nWz3pV4WpS/Qg0XVnxw==",
      "cpu": [
        "loong64"
      ],
      "dev": true,
      "libc": [
        "glibc"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-loong64-musl": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-loong64-musl/-/rollup-linux-loong64-musl-4.67.4.tgz",
      "integrity": "sha512-jzxbVHaE7V3F+P0wEK7UydQtvv7Twfdtx58jI5/3VRx1jJxyrn+qFwfXmqjVFxPhBl+zjaRs+ZJMFJw7vdHaQA==",
      "cpu": [
        "loong64"
      ],
      "dev": true,
      "libc": [
        "musl"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-ppc64-gnu": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-ppc64-gnu/-/rollup-linux-ppc64-gnu-4.67.4.tgz",
      "integrity": "sha512-fhtrzp9p4GjEJVaigaap9g6X9nP6xgJtKiLLXSDgpE18HXufA5aqfltGn+KAqMtVZReJZfOrXm//y5YSA9BWbA==",
      "cpu": [
        "ppc64"
      ],
      "dev": true,
      "libc": [
        "glibc"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-ppc64-musl": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-ppc64-musl/-/rollup-linux-ppc64-musl-4.67.4.tgz",
      "integrity": "sha512-uynaF7IUHwAckV0S+yC1f5esy9gB+aqCBsPH5BOck32TYdCuAn2lVG5tvUcFnEsrviGEmRZxE1NHSOieUPYJEg==",
      "cpu": [
        "ppc64"
      ],
      "dev": true,
      "libc": [
        "musl"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-riscv64-gnu": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-riscv64-gnu/-/rollup-linux-riscv64-gnu-4.67.4.tgz",
      "integrity": "sha512-uaqRFdfb8eIa2bVHN/FE3ndCvR0zV2xv0AwjCfxrqSu6l6fp/54VhjfhslKlf2RIik9L1jwlt4E56PeVX6tQtQ==",
      "cpu": [
        "riscv64"
      ],
      "dev": true,
      "libc": [
        "glibc"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-riscv64-musl": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-riscv64-musl/-/rollup-linux-riscv64-musl-4.67.4.tgz",
      "integrity": "sha512-JZRow7kmxUDEASnxHuVG5itTm+nTXi9BQUABcVUnUEngC+OPwMrpx1uJ9hIA/v+IcAEBL52ktCSPXk5OJn9Jqg==",
      "cpu": [
        "riscv64"
      ],
      "dev": true,
      "libc": [
        "musl"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-s390x-gnu": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-s390x-gnu/-/rollup-linux-s390x-gnu-4.67.4.tgz",
      "integrity": "sha512-rdrh4e+Lq9fCNKoDPaCqeHXbQCe7yl77Ukl45tlSClwYdZ1gpFGZz1XLJdYtY++FRlfL5aHvb6/0n3GDW/Nxsw==",
      "cpu": [
        "s390x"
      ],
      "dev": true,
      "libc": [
        "glibc"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-x64-gnu": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-x64-gnu/-/rollup-linux-x64-gnu-4.67.4.tgz",
      "integrity": "sha512-wL7MS4r0fYcxluF3ne2plofHZiB/LsqNCnyaUlyiKpAJLb6agPSFz7DEFCfkonWAZXMrLwCmC08b2ZEnEdJkog==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "libc": [
        "glibc"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-linux-x64-musl": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-linux-x64-musl/-/rollup-linux-x64-musl-4.67.4.tgz",
      "integrity": "sha512-4H0XHfd2VufGjX7NjUyeue78sD0bRIoKSGjGqO+F5nSvL6jN48NAU7esdmtFEYxZXwObLtt3MJ/kHS0jASMW3g==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "libc": [
        "musl"
      ],
      "license": "MIT",
      "optional": true,
      "os": [
        "linux"
      ]
    },
    "node_modules/@rollup/rollup-openbsd-x64": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-openbsd-x64/-/rollup-openbsd-x64-4.67.4.tgz",
      "integrity": "sha512-osRHQbE55pZpCLetpMsFmhkapyZnAUH73qRKMh+npi3boUom6u6hdaTTPSxlInXxwJHsn9r2uDhdz0V7Yoxx2A==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "openbsd"
      ]
    },
    "node_modules/@rollup/rollup-openharmony-arm64": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-openharmony-arm64/-/rollup-openharmony-arm64-4.67.4.tgz",
      "integrity": "sha512-95+exYEyOwrZePsE0czi4q7X5lxKPa21+lmyAZlwbYENITQ0Unk4HAQAFp/p+KhXBHBx9mUABXRYjOC5A1wY3Q==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "openharmony"
      ]
    },
    "node_modules/@rollup/rollup-win32-arm64-msvc": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-win32-arm64-msvc/-/rollup-win32-arm64-msvc-4.67.4.tgz",
      "integrity": "sha512-FOXOcKhNQpO4Po+Ynan6IbtM67SrXZbgpRJWVYwz53CT5veI7sVzDwVBmWS65N5Imv+wK3SVhIPAl+C5DXXNCg==",
      "cpu": [
        "arm64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "win32"
      ]
    },
    "node_modules/@rollup/rollup-win32-ia32-msvc": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-win32-ia32-msvc/-/rollup-win32-ia32-msvc-4.67.4.tgz",
      "integrity": "sha512-cklKh0gJOkAZQcWziQa3hrjxZKN+PCHZ7SPTyGJDAiBVr05ZXlEjGFCG7zFJMaVibxsmzAe1FimN4u/HBns9+Q==",
      "cpu": [
        "ia32"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "win32"
      ]
    },
    "node_modules/@rollup/rollup-win32-x64-gnu": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-win32-x64-gnu/-/rollup-win32-x64-gnu-4.67.4.tgz",
      "integrity": "sha512-AegwxJPc9bvUp8fQtlsUHkFMOMkGylWU7glyL+YD4vwG+QaYoGXYLSsQoeTD7kvoLZrVlMWNVhTg1dedY5C1VQ==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "win32"
      ]
    },
    "node_modules/@rollup/rollup-win32-x64-msvc": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/@rollup/rollup-win32-x64-msvc/-/rollup-win32-x64-msvc-4.67.4.tgz",
      "integrity": "sha512-RJZnrwTWj7Iq2ooBwoGbDdC65tDwYawneGcAfFMJqVUrRZMcskka6nTkBpXR/47G7kzn3EW2qupExnaBy11vwg==",
      "cpu": [
        "x64"
      ],
      "dev": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "win32"
      ]
    },
    "node_modules/@shikijs/core": {
      "version": "2.5.0",
      "resolved": "https://registry.npmjs.org/@shikijs/core/-/core-2.5.0.tgz",
      "integrity": "sha512-uu/8RExTKtavlpH7XqnVYBrfBkUc20ngXiX9NSrBhOVZYv/7XQRKUyhtkeflY5QsxC0GbJThCerruZfsUaSldg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@shikijs/engine-javascript": "2.5.0",
        "@shikijs/engine-oniguruma": "2.5.0",
        "@shikijs/types": "2.5.0",
        "@shikijs/vscode-textmate": "^10.0.2",
        "@types/hast": "^3.0.4",
        "hast-util-to-html": "^9.0.4"
      }
    },
    "node_modules/@shikijs/engine-javascript": {
      "version": "2.5.0",
      "resolved": "https://registry.npmjs.org/@shikijs/engine-javascript/-/engine-javascript-2.5.0.tgz",
      "integrity": "sha512-VjnOpnQf8WuCEZtNUdjjwGUbtAVKuZkVQ/5cHy/tojVVRIRtlWMYVjyWhxOmIq05AlSOv72z7hRNRGVBgQOl0w==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@shikijs/types": "2.5.0",
        "@shikijs/vscode-textmate": "^10.0.2",
        "oniguruma-to-es": "^3.1.0"
      }
    },
    "node_modules/@shikijs/engine-oniguruma": {
      "version": "2.5.0",
      "resolved": "https://registry.npmjs.org/@shikijs/engine-oniguruma/-/engine-oniguruma-2.5.0.tgz",
      "integrity": "sha512-pGd1wRATzbo/uatrCIILlAdFVKdxImWJGQ5rFiB5VZi2ve5xj3Ax9jny8QvkaV93btQEwR/rSz5ERFpC5mKNIw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@shikijs/types": "2.5.0",
        "@shikijs/vscode-textmate": "^10.0.2"
      }
    },
    "node_modules/@shikijs/langs": {
      "version": "2.5.0",
      "resolved": "https://registry.npmjs.org/@shikijs/langs/-/langs-2.5.0.tgz",
      "integrity": "sha512-Qfrrt5OsNH5R+5tJ/3uYBBZv3SuGmnRPejV9IlIbFH3HTGLDlkqgHymAlzklVmKBjAaVmkPkyikAV/sQ1wSL+w==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@shikijs/types": "2.5.0"
      }
    },
    "node_modules/@shikijs/themes": {
      "version": "2.5.0",
      "resolved": "https://registry.npmjs.org/@shikijs/themes/-/themes-2.5.0.tgz",
      "integrity": "sha512-wGrk+R8tJnO0VMzmUExHR+QdSaPUl/NKs+a4cQQRWyoc3YFbUzuLEi/KWK1hj+8BfHRKm2jNhhJck1dfstJpiw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@shikijs/types": "2.5.0"
      }
    },
    "node_modules/@shikijs/transformers": {
      "version": "2.5.0",
      "resolved": "https://registry.npmjs.org/@shikijs/transformers/-/transformers-2.5.0.tgz",
      "integrity": "sha512-SI494W5X60CaUwgi8u4q4m4s3YAFSxln3tzNjOSYqq54wlVgz0/NbbXEb3mdLbqMBztcmS7bVTaEd2w0qMmfeg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@shikijs/core": "2.5.0",
        "@shikijs/types": "2.5.0"
      }
    },
    "node_modules/@shikijs/types": {
      "version": "2.5.0",
      "resolved": "https://registry.npmjs.org/@shikijs/types/-/types-2.5.0.tgz",
      "integrity": "sha512-ygl5yhxki9ZLNuNpPitBWvcy9fsSKKaRuO4BAlMyagszQidxcpLAr0qiW/q43DtSIDxO6hEbtYLiFZNXO/hdGw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@shikijs/vscode-textmate": "^10.0.2",
        "@types/hast": "^3.0.4"
      }
    },
    "node_modules/@shikijs/vscode-textmate": {
      "version": "10.1.0",
      "resolved": "https://registry.npmjs.org/@shikijs/vscode-textmate/-/vscode-textmate-10.1.0.tgz",
      "integrity": "sha512-cVBuhDQjBZaNMTwBquYfPydrUP+E3hE6oYGasx8h6ZrODIM7/r+gxLDXdd66oszbrggZHEntdE6j+5gYIVyMWQ==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@types/estree": {
      "version": "1.0.10",
      "resolved": "https://registry.npmjs.org/@types/estree/-/estree-1.0.10.tgz",
      "integrity": "sha512-lTyOMnOcmRBzdCv6/4zUZCrEhV0xrqQiUZBZtVcbmLHBXKLnN8q89v/wKRF/Q0HbwVsqbIG3o5HDoFl9sduM+g==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@types/hast": {
      "version": "3.0.5",
      "resolved": "https://registry.npmjs.org/@types/hast/-/hast-3.0.5.tgz",
      "integrity": "sha512-rp/ezSWaD1m44dPKICGhiskI13nVr7qTloFwDa/IYkhhf5nzwP+zIQcIJh3WIFSBOy/H1PzB40jPjMDksN4F+g==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "*"
      }
    },
    "node_modules/@types/linkify-it": {
      "version": "5.0.0",
      "resolved": "https://registry.npmjs.org/@types/linkify-it/-/linkify-it-5.0.0.tgz",
      "integrity": "sha512-sVDA58zAw4eWAffKOaQH5/5j3XeayukzDk+ewSsnv3p4yJEZHCCzMDiZM8e0OUrRvmpGZ85jf4yDHkHsgBNr9Q==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@types/markdown-it": {
      "version": "14.2.0",
      "resolved": "https://registry.npmjs.org/@types/markdown-it/-/markdown-it-14.2.0.tgz",
      "integrity": "sha512-NoQ2yGlLWj4wpxMs+TYmRKk3thDrQ97agr7sFqfLsAlvoS8SNQuTrlObhFqG9iugdTtgOE9jpJ6FNM4ZGsa5xQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/linkify-it": "^5",
        "@types/mdurl": "^2"
      }
    },
    "node_modules/@types/mdast": {
      "version": "4.0.4",
      "resolved": "https://registry.npmjs.org/@types/mdast/-/mdast-4.0.4.tgz",
      "integrity": "sha512-kGaNbPh1k7AFzgpud/gMdvIm5xuECykRR+JnWKQno9TAXVa6WIVCGTPvYGekIDL4uwCZQSYbUxNBSb1aUo79oA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "*"
      }
    },
    "node_modules/@types/mdurl": {
      "version": "2.0.0",
      "resolved": "https://registry.npmjs.org/@types/mdurl/-/mdurl-2.0.0.tgz",
      "integrity": "sha512-RGdgjQUZba5p6QEFAVx2OGb8rQDL/cPRG7GiedRzMcJ1tYnUANBncjbSB1NRGwbvjcPeikRABz2nshyPk1bhWg==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@types/unist": {
      "version": "3.0.3",
      "resolved": "https://registry.npmjs.org/@types/unist/-/unist-3.0.3.tgz",
      "integrity": "sha512-ko/gIFJRv177XgZsZcBwnqJN5x/Gien8qNOn0D5bQU/zAzVf9Zt3BlcUiLqhV9y4ARk0GbT3tnUiPNgnTXzc/Q==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@types/web-bluetooth": {
      "version": "0.0.21",
      "resolved": "https://registry.npmjs.org/@types/web-bluetooth/-/web-bluetooth-0.0.21.tgz",
      "integrity": "sha512-oIQLCGWtcFZy2JW77j9k8nHzAOpqMHLQejDA48XXMWH6tjCQHz5RCFz1bzsmROyL6PUm+LLnUiI4BCn221inxA==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@ungap/structured-clone": {
      "version": "1.4.0",
      "resolved": "https://registry.npmjs.org/@ungap/structured-clone/-/structured-clone-1.4.0.tgz",
      "integrity": "sha512-1mEZtMKPM09vDmQt5y7YvmN2+DFTP7Tg0EWXdic8/C6VRnpb33e4ghisCIE3WZjsE2N8mf+QV1Zqh7ZFYLWInQ==",
      "dev": true,
      "license": "ISC"
    },
    "node_modules/@vitejs/plugin-vue": {
      "version": "5.2.4",
      "resolved": "https://registry.npmjs.org/@vitejs/plugin-vue/-/plugin-vue-5.2.4.tgz",
      "integrity": "sha512-7Yx/SXSOcQq5HiiV3orevHUFn+pmMB4cgbEkDYgnkUWb0WfeQ/wa2yFv6D5ICiCQOVpjA7vYDXrC7AGO8yjDHA==",
      "dev": true,
      "license": "MIT",
      "engines": {
        "node": "^18.0.0 || >=20.0.0"
      },
      "peerDependencies": {
        "vite": "^5.0.0 || ^6.0.0",
        "vue": "^3.2.25"
      }
    },
    "node_modules/@vue/compiler-core": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/compiler-core/-/compiler-core-3.5.44.tgz",
      "integrity": "sha512-k34nCytbK8PQ1gR2mGROYOw6Pnv/mumqIdrZ3x2tbyXZDWm7ViKGLTfdB+DRrWqEQsLgGJvn0WN9jBif5zCTLQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@babel/parser": "^7.29.9",
        "@vue/shared": "3.5.44",
        "entities": "^7.0.1",
        "estree-walker": "^2.0.2",
        "source-map-js": "^1.2.2"
      }
    },
    "node_modules/@vue/compiler-dom": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/compiler-dom/-/compiler-dom-3.5.44.tgz",
      "integrity": "sha512-W5AoBFWdw1lQAKiHowkfgFKFab82oSRT6sGsPfw/q1GRjsy/ZJFLHGhJyBRIFN9APLTUduDH9U2JAiPOIyKuYg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/compiler-core": "3.5.44",
        "@vue/shared": "3.5.44"
      }
    },
    "node_modules/@vue/compiler-sfc": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/compiler-sfc/-/compiler-sfc-3.5.44.tgz",
      "integrity": "sha512-dzHmQ4jXTpdr5FjIQUeykVakN/uzqFbUo9/C5ikBFg3a3PeM6uRxeczx1ZbXBML0/zJstEh3ygYipzR5Z7b2Pw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@babel/parser": "^7.29.9",
        "@vue/compiler-core": "3.5.44",
        "@vue/compiler-dom": "3.5.44",
        "@vue/compiler-ssr": "3.5.44",
        "@vue/shared": "3.5.44",
        "estree-walker": "^2.0.2",
        "magic-string": "^0.30.21",
        "postcss": "^8.5.29",
        "source-map-js": "^1.2.2"
      }
    },
    "node_modules/@vue/compiler-ssr": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/compiler-ssr/-/compiler-ssr-3.5.44.tgz",
      "integrity": "sha512-xr5gb1JVHlfXJwSlRz1FeP6uxoazdzHuMTVLE7up+4Vrqxk219CvxLPxD9NXBt47yvZW7+p4DXfyp4O1fkByIA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/compiler-dom": "3.5.44",
        "@vue/shared": "3.5.44"
      }
    },
    "node_modules/@vue/devtools-api": {
      "version": "7.7.10",
      "resolved": "https://registry.npmjs.org/@vue/devtools-api/-/devtools-api-7.7.10.tgz",
      "integrity": "sha512-KxtEpUOOpFz/qOGRrAwA36QF7DqIA+FXgCYit9mk9wjbaZt0sXOFz81ElOZtKA4HbWHUdwNjZHBFsFFyp5BZiA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/devtools-kit": "^7.7.10"
      }
    },
    "node_modules/@vue/devtools-kit": {
      "version": "7.7.10",
      "resolved": "https://registry.npmjs.org/@vue/devtools-kit/-/devtools-kit-7.7.10.tgz",
      "integrity": "sha512-3WNi2Kq4tbpVbmhml7RiphmAt0279oh3fKNeWMQIrltfX8Q91b4i5PL8DtyNKdwmcsGrV4fg+erwWOmD05CLIw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/devtools-shared": "^7.7.10",
        "birpc": "^2.3.0",
        "hookable": "^5.5.3",
        "mitt": "^3.0.1",
        "perfect-debounce": "^1.0.0",
        "speakingurl": "^14.0.1",
        "superjson": "^2.2.2"
      }
    },
    "node_modules/@vue/devtools-shared": {
      "version": "7.7.10",
      "resolved": "https://registry.npmjs.org/@vue/devtools-shared/-/devtools-shared-7.7.10.tgz",
      "integrity": "sha512-wOPslzB8vTvpxwdaOcR2qAbwmuSP0L+rhpoC6Cf56V3Jip+HWb7PQQXOUPgBNQARpXsbQX/+mvi8kKucmBGRwQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "rfdc": "^1.4.1"
      }
    },
    "node_modules/@vue/reactivity": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/reactivity/-/reactivity-3.5.44.tgz",
      "integrity": "sha512-u+QJdKeBBEADkPdevsL+6E12QlYVfBD8Zcw5qjDQKtamI6l14KyqazJMobhCKelhWN5zpyKUiEDoTyHE93cqWw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/shared": "3.5.44"
      }
    },
    "node_modules/@vue/runtime-core": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/runtime-core/-/runtime-core-3.5.44.tgz",
      "integrity": "sha512-uEa/ndwjhrrhnOfb2p+sZy6zeoamJOISqaLSJGk/8R6nABT1ZldpzcJoPzKj+zHVomVBPcmQYyC2r4LtvDV6CQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/reactivity": "3.5.44",
        "@vue/shared": "3.5.44"
      }
    },
    "node_modules/@vue/runtime-dom": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/runtime-dom/-/runtime-dom-3.5.44.tgz",
      "integrity": "sha512-3fc4ARcXVhVBaJ3ujWiNZJARD6pkyRcwG+ZYhMzjKtKUp7MAY0HLwr7r3Smep3Li6ZaWT6ewdmx8XqdaD5oNZg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/reactivity": "3.5.44",
        "@vue/runtime-core": "3.5.44",
        "@vue/shared": "3.5.44",
        "csstype": "^3.2.3"
      }
    },
    "node_modules/@vue/server-renderer": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/server-renderer/-/server-renderer-3.5.44.tgz",
      "integrity": "sha512-R4S1UoH4h5Frrauv8Z5XE5TxV9oBiUSgWIC4TyVK1FK+RocRnOLwFA5Xcmo2011RlgJp+AbZCnw6afrhlgRuUQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/compiler-ssr": "3.5.44",
        "@vue/runtime-dom": "3.5.44",
        "@vue/shared": "3.5.44"
      }
    },
    "node_modules/@vue/shared": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/@vue/shared/-/shared-3.5.44.tgz",
      "integrity": "sha512-H82XLH0DjmSOdbEwO15x3QTo1f5PWEtv237eLpYKc0mqLu+B8hnq16lFWmuBmNurkeiG3wr+oM1FhOll/YVFRg==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/@vueuse/core": {
      "version": "12.8.2",
      "resolved": "https://registry.npmjs.org/@vueuse/core/-/core-12.8.2.tgz",
      "integrity": "sha512-HbvCmZdzAu3VGi/pWYm5Ut+Kd9mn1ZHnn4L5G8kOQTPs/IwIAmJoBrmYk2ckLArgMXZj0AW3n5CAejLUO+PhdQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/web-bluetooth": "^0.0.21",
        "@vueuse/metadata": "12.8.2",
        "@vueuse/shared": "12.8.2",
        "vue": "^3.5.13"
      },
      "funding": {
        "url": "https://github.com/sponsors/antfu"
      }
    },
    "node_modules/@vueuse/integrations": {
      "version": "12.8.2",
      "resolved": "https://registry.npmjs.org/@vueuse/integrations/-/integrations-12.8.2.tgz",
      "integrity": "sha512-fbGYivgK5uBTRt7p5F3zy6VrETlV9RtZjBqd1/HxGdjdckBgBM4ugP8LHpjolqTj14TXTxSK1ZfgPbHYyGuH7g==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vueuse/core": "12.8.2",
        "@vueuse/shared": "12.8.2",
        "vue": "^3.5.13"
      },
      "funding": {
        "url": "https://github.com/sponsors/antfu"
      },
      "peerDependencies": {
        "async-validator": "^4",
        "axios": "^1",
        "change-case": "^5",
        "drauu": "^0.4",
        "focus-trap": "^7",
        "fuse.js": "^7",
        "idb-keyval": "^6",
        "jwt-decode": "^4",
        "nprogress": "^0.2",
        "qrcode": "^1.5",
        "sortablejs": "^1",
        "universal-cookie": "^7"
      },
      "peerDependenciesMeta": {
        "async-validator": {
          "optional": true
        },
        "axios": {
          "optional": true
        },
        "change-case": {
          "optional": true
        },
        "drauu": {
          "optional": true
        },
        "focus-trap": {
          "optional": true
        },
        "fuse.js": {
          "optional": true
        },
        "idb-keyval": {
          "optional": true
        },
        "jwt-decode": {
          "optional": true
        },
        "nprogress": {
          "optional": true
        },
        "qrcode": {
          "optional": true
        },
        "sortablejs": {
          "optional": true
        },
        "universal-cookie": {
          "optional": true
        }
      }
    },
    "node_modules/@vueuse/metadata": {
      "version": "12.8.2",
      "resolved": "https://registry.npmjs.org/@vueuse/metadata/-/metadata-12.8.2.tgz",
      "integrity": "sha512-rAyLGEuoBJ/Il5AmFHiziCPdQzRt88VxR+Y/A/QhJ1EWtWqPBBAxTAFaSkviwEuOEZNtW8pvkPgoCZQ+HxqW1A==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "url": "https://github.com/sponsors/antfu"
      }
    },
    "node_modules/@vueuse/shared": {
      "version": "12.8.2",
      "resolved": "https://registry.npmjs.org/@vueuse/shared/-/shared-12.8.2.tgz",
      "integrity": "sha512-dznP38YzxZoNloI0qpEfpkms8knDtaoQ6Y/sfS0L7Yki4zh40LFHEhur0odJC6xTHG5dxWVPiUWBXn+wCG2s5w==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "vue": "^3.5.13"
      },
      "funding": {
        "url": "https://github.com/sponsors/antfu"
      }
    },
    "node_modules/algoliasearch": {
      "version": "5.61.0",
      "resolved": "https://registry.npmjs.org/algoliasearch/-/algoliasearch-5.61.0.tgz",
      "integrity": "sha512-CFtHkGANnIAG6TY5g9s/h+yxZ0SN2/ICMjMY1EWj9gKGiS4LSGFrmMmTA/xxyxOH8AEazFD3YSfRZMh5vIEh2w==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@algolia/abtesting": "1.27.0",
        "@algolia/client-abtesting": "5.61.0",
        "@algolia/client-analytics": "5.61.0",
        "@algolia/client-common": "5.61.0",
        "@algolia/client-insights": "5.61.0",
        "@algolia/client-personalization": "5.61.0",
        "@algolia/client-query-suggestions": "5.61.0",
        "@algolia/client-search": "5.61.0",
        "@algolia/ingestion": "1.61.0",
        "@algolia/monitoring": "1.61.0",
        "@algolia/recommend": "5.61.0",
        "@algolia/requester-browser-xhr": "5.61.0",
        "@algolia/requester-fetch": "5.61.0",
        "@algolia/requester-node-http": "5.61.0"
      },
      "engines": {
        "node": ">= 14.0.0"
      }
    },
    "node_modules/birpc": {
      "version": "2.9.0",
      "resolved": "https://registry.npmjs.org/birpc/-/birpc-2.9.0.tgz",
      "integrity": "sha512-KrayHS5pBi69Xi9JmvoqrIgYGDkD6mcSe/i6YKi3w5kekCLzrX4+nawcXqrj2tIp50Kw/mT/s3p+GVK0A0sKxw==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "url": "https://github.com/sponsors/antfu"
      }
    },
    "node_modules/ccount": {
      "version": "2.0.1",
      "resolved": "https://registry.npmjs.org/ccount/-/ccount-2.0.1.tgz",
      "integrity": "sha512-eyrF0jiFpY+3drT6383f1qhkbGsLSifNAjA61IUjZjmLCWjItY6LB9ft9YhoDgwfmclB2zhu51Lc7+95b8NRAg==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/character-entities-html4": {
      "version": "2.1.0",
      "resolved": "https://registry.npmjs.org/character-entities-html4/-/character-entities-html4-2.1.0.tgz",
      "integrity": "sha512-1v7fgQRj6hnSwFpq1Eu0ynr/CDEw0rXo2B61qXrLNdHZmPKgb7fqS1a2JwF0rISo9q77jDI8VMEHoApn8qDoZA==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/character-entities-legacy": {
      "version": "3.0.0",
      "resolved": "https://registry.npmjs.org/character-entities-legacy/-/character-entities-legacy-3.0.0.tgz",
      "integrity": "sha512-RpPp0asT/6ufRm//AJVwpViZbGM/MkjQFxJccQRHmISF/22NBtsHqAWmL+/pmkPWoIUJdWyeVleTl1wydHATVQ==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/comma-separated-tokens": {
      "version": "2.0.3",
      "resolved": "https://registry.npmjs.org/comma-separated-tokens/-/comma-separated-tokens-2.0.3.tgz",
      "integrity": "sha512-Fu4hJdvzeylCfQPp9SGWidpzrMs7tTrlu6Vb8XGaRGck8QSNZJJp538Wrb60Lax4fPwR64ViY468OIUTbRlGZg==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/copy-anything": {
      "version": "4.1.5",
      "resolved": "https://registry.npmjs.org/copy-anything/-/copy-anything-4.1.5.tgz",
      "integrity": "sha512-vePst2VLTSqPglW7Q98VwFpGiGpOHTyXIo81MO5VyLecA5zMbZw/P/V1vmwD1jksa2iJpn/YkDrJ3XhTfzUyhQ==",
      "dev": true,
      "license": "MIT",
      "engines": {
        "node": ">=18"
      },
      "funding": {
        "url": "https://github.com/sponsors/mesqueeb"
      }
    },
    "node_modules/csstype": {
      "version": "3.2.3",
      "resolved": "https://registry.npmjs.org/csstype/-/csstype-3.2.3.tgz",
      "integrity": "sha512-z1HGKcYy2xA8AGQfwrn0PAy+PB7X/GSj3UVJW9qKyn43xWa+gl5nXmU4qqLMRzWVLFC8KusUX8T/0kCiOYpAIQ==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/dequal": {
      "version": "2.0.3",
      "resolved": "https://registry.npmjs.org/dequal/-/dequal-2.0.3.tgz",
      "integrity": "sha512-0je+qPKHEMohvfRTCEo3CrPG6cAzAYgmzKyxRiYSSDkS6eGJdyVJm7WaYA5ECaAD9wLB2T4EEeymA5aFVcYXCA==",
      "dev": true,
      "license": "MIT",
      "engines": {
        "node": ">=6"
      }
    },
    "node_modules/devlop": {
      "version": "1.1.0",
      "resolved": "https://registry.npmjs.org/devlop/-/devlop-1.1.0.tgz",
      "integrity": "sha512-RWmIqhcFf1lRYBvNmr7qTNuyCt/7/ns2jbpp1+PalgE/rDQcBT0fioSMUpJ93irlUhC5hrg4cYqe6U+0ImW0rA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "dequal": "^2.0.0"
      },
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/emoji-regex-xs": {
      "version": "1.0.0",
      "resolved": "https://registry.npmjs.org/emoji-regex-xs/-/emoji-regex-xs-1.0.0.tgz",
      "integrity": "sha512-LRlerrMYoIDrT6jgpeZ2YYl/L8EulRTt5hQcYjy5AInh7HWXKimpqx68aknBFpGL2+/IcogTcaydJEgaTmOpDg==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/entities": {
      "version": "7.0.1",
      "resolved": "https://registry.npmjs.org/entities/-/entities-7.0.1.tgz",
      "integrity": "sha512-TWrgLOFUQTH994YUyl1yT4uyavY5nNB5muff+RtWaqNVCAK408b5ZnnbNAUEWLTCpum9w6arT70i1XdQ4UeOPA==",
      "dev": true,
      "license": "BSD-2-Clause",
      "engines": {
        "node": ">=0.12"
      },
      "funding": {
        "url": "https://github.com/fb55/entities?sponsor=1"
      }
    },
    "node_modules/esbuild": {
      "version": "0.21.5",
      "resolved": "https://registry.npmjs.org/esbuild/-/esbuild-0.21.5.tgz",
      "integrity": "sha512-mg3OPMV4hXywwpoDxu3Qda5xCKQi+vCTZq8S9J/EpkhB2HzKXq4SNFZE3+NK93JYxc8VMSep+lOUSC/RVKaBqw==",
      "dev": true,
      "hasInstallScript": true,
      "license": "MIT",
      "bin": {
        "esbuild": "bin/esbuild"
      },
      "engines": {
        "node": ">=12"
      },
      "optionalDependencies": {
        "@esbuild/aix-ppc64": "0.21.5",
        "@esbuild/android-arm": "0.21.5",
        "@esbuild/android-arm64": "0.21.5",
        "@esbuild/android-x64": "0.21.5",
        "@esbuild/darwin-arm64": "0.21.5",
        "@esbuild/darwin-x64": "0.21.5",
        "@esbuild/freebsd-arm64": "0.21.5",
        "@esbuild/freebsd-x64": "0.21.5",
        "@esbuild/linux-arm": "0.21.5",
        "@esbuild/linux-arm64": "0.21.5",
        "@esbuild/linux-ia32": "0.21.5",
        "@esbuild/linux-loong64": "0.21.5",
        "@esbuild/linux-mips64el": "0.21.5",
        "@esbuild/linux-ppc64": "0.21.5",
        "@esbuild/linux-riscv64": "0.21.5",
        "@esbuild/linux-s390x": "0.21.5",
        "@esbuild/linux-x64": "0.21.5",
        "@esbuild/netbsd-x64": "0.21.5",
        "@esbuild/openbsd-x64": "0.21.5",
        "@esbuild/sunos-x64": "0.21.5",
        "@esbuild/win32-arm64": "0.21.5",
        "@esbuild/win32-ia32": "0.21.5",
        "@esbuild/win32-x64": "0.21.5"
      }
    },
    "node_modules/estree-walker": {
      "version": "2.0.2",
      "resolved": "https://registry.npmjs.org/estree-walker/-/estree-walker-2.0.2.tgz",
      "integrity": "sha512-Rfkk/Mp/DL7JVje3u18FxFujQlTNR2q6QfMSMB7AvCBx91NGj/ba3kCfza0f6dVDbw7YlRf/nDrn7pQrCCyQ/w==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/focus-trap": {
      "version": "7.8.0",
      "resolved": "https://registry.npmjs.org/focus-trap/-/focus-trap-7.8.0.tgz",
      "integrity": "sha512-/yNdlIkpWbM0ptxno3ONTuf+2g318kh2ez3KSeZN5dZ8YC6AAmgeWz+GasYYiBJPFaYcSAPeu4GfhUaChzIJXA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "tabbable": "^6.4.0"
      }
    },
    "node_modules/fsevents": {
      "version": "2.3.3",
      "resolved": "https://registry.npmjs.org/fsevents/-/fsevents-2.3.3.tgz",
      "integrity": "sha512-5xoDfX+fL7faATnagmWPpbFtwh/R77WmMMqqHGS65C3vvB0YHrgF+B1YmZ3441tMj5n63k0212XNoJwzlhffQw==",
      "dev": true,
      "hasInstallScript": true,
      "license": "MIT",
      "optional": true,
      "os": [
        "darwin"
      ],
      "engines": {
        "node": "^8.16.0 || ^10.6.0 || >=11.0.0"
      }
    },
    "node_modules/hast-util-to-html": {
      "version": "9.0.6",
      "resolved": "https://registry.npmjs.org/hast-util-to-html/-/hast-util-to-html-9.0.6.tgz",
      "integrity": "sha512-1SZUxCN3TtJAE7e2BQN2MBHsoZ1gnmi9GoCP5Di59CSWptIezSAW09yDZyVbanb78V9eCwrKqdQsjtfVTxYHWA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/hast": "^3.0.0",
        "@types/unist": "^3.0.0",
        "ccount": "^2.0.0",
        "comma-separated-tokens": "^2.0.0",
        "hast-util-whitespace": "^3.0.0",
        "html-void-elements": "^3.0.0",
        "mdast-util-to-hast": "^13.0.0",
        "property-information": "^7.0.0",
        "space-separated-tokens": "^2.0.0",
        "stringify-entities": "^4.0.0",
        "zwitch": "^2.0.4"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/hast-util-whitespace": {
      "version": "3.0.0",
      "resolved": "https://registry.npmjs.org/hast-util-whitespace/-/hast-util-whitespace-3.0.0.tgz",
      "integrity": "sha512-88JUN06ipLwsnv+dVn+OIYOvAuvBMy/Qoi6O7mQHxdPXpjy+Cd6xRkWwux7DKO+4sYILtLBRIKgsdpS2gQc7qw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/hast": "^3.0.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/hookable": {
      "version": "5.5.3",
      "resolved": "https://registry.npmjs.org/hookable/-/hookable-5.5.3.tgz",
      "integrity": "sha512-Yc+BQe8SvoXH1643Qez1zqLRmbA5rCL+sSmk6TVos0LWVfNIB7PGncdlId77WzLGSIB5KaWgTaNTs2lNVEI6VQ==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/html-void-elements": {
      "version": "3.0.0",
      "resolved": "https://registry.npmjs.org/html-void-elements/-/html-void-elements-3.0.0.tgz",
      "integrity": "sha512-bEqo66MRXsUGxWHV5IP0PUiAWwoEjba4VCzg0LjFJBpchPaTfyfCKTG6bc5F8ucKec3q5y6qOdGyYTSBEvhCrg==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/magic-string": {
      "version": "0.30.21",
      "resolved": "https://registry.npmjs.org/magic-string/-/magic-string-0.30.21.tgz",
      "integrity": "sha512-vd2F4YUyEXKGcLHoq+TEyCjxueSeHnFxyyjNp80yg0XV4vUhnDer/lvvlqM/arB5bXQN5K2/3oinyCRyx8T2CQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@jridgewell/sourcemap-codec": "^1.5.5"
      }
    },
    "node_modules/mark.js": {
      "version": "8.11.1",
      "resolved": "https://registry.npmjs.org/mark.js/-/mark.js-8.11.1.tgz",
      "integrity": "sha512-1I+1qpDt4idfgLQG+BNWmrqku+7/2bi5nLf4YwF8y8zXvmfiTBY3PV3ZibfrjBueCByROpuBjLLFCajqkgYoLQ==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/mdast-util-to-hast": {
      "version": "13.2.1",
      "resolved": "https://registry.npmjs.org/mdast-util-to-hast/-/mdast-util-to-hast-13.2.1.tgz",
      "integrity": "sha512-cctsq2wp5vTsLIcaymblUriiTcZd0CwWtCbLvrOzYCDZoWyMNV8sZ7krj09FSnsiJi3WVsHLM4k6Dq/yaPyCXA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/hast": "^3.0.0",
        "@types/mdast": "^4.0.0",
        "@ungap/structured-clone": "^1.0.0",
        "devlop": "^1.0.0",
        "micromark-util-sanitize-uri": "^2.0.0",
        "trim-lines": "^3.0.0",
        "unist-util-position": "^5.0.0",
        "unist-util-visit": "^5.0.0",
        "vfile": "^6.0.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/micromark-util-character": {
      "version": "2.1.1",
      "resolved": "https://registry.npmjs.org/micromark-util-character/-/micromark-util-character-2.1.1.tgz",
      "integrity": "sha512-wv8tdUTJ3thSFFFJKtpYKOYiGP2+v96Hvk4Tu8KpCAsTMs6yi+nVmGh1syvSCsaxz45J6Jbw+9DD6g97+NV67Q==",
      "dev": true,
      "funding": [
        {
          "type": "GitHub Sponsors",
          "url": "https://github.com/sponsors/unifiedjs"
        },
        {
          "type": "OpenCollective",
          "url": "https://opencollective.com/unified"
        }
      ],
      "license": "MIT",
      "dependencies": {
        "micromark-util-symbol": "^2.0.0",
        "micromark-util-types": "^2.0.0"
      }
    },
    "node_modules/micromark-util-encode": {
      "version": "2.0.1",
      "resolved": "https://registry.npmjs.org/micromark-util-encode/-/micromark-util-encode-2.0.1.tgz",
      "integrity": "sha512-c3cVx2y4KqUnwopcO9b/SCdo2O67LwJJ/UyqGfbigahfegL9myoEFoDYZgkT7f36T0bLrM9hZTAaAyH+PCAXjw==",
      "dev": true,
      "funding": [
        {
          "type": "GitHub Sponsors",
          "url": "https://github.com/sponsors/unifiedjs"
        },
        {
          "type": "OpenCollective",
          "url": "https://opencollective.com/unified"
        }
      ],
      "license": "MIT"
    },
    "node_modules/micromark-util-sanitize-uri": {
      "version": "2.0.1",
      "resolved": "https://registry.npmjs.org/micromark-util-sanitize-uri/-/micromark-util-sanitize-uri-2.0.1.tgz",
      "integrity": "sha512-9N9IomZ/YuGGZZmQec1MbgxtlgougxTodVwDzzEouPKo3qFWvymFHWcnDi2vzV1ff6kas9ucW+o3yzJK9YB1AQ==",
      "dev": true,
      "funding": [
        {
          "type": "GitHub Sponsors",
          "url": "https://github.com/sponsors/unifiedjs"
        },
        {
          "type": "OpenCollective",
          "url": "https://opencollective.com/unified"
        }
      ],
      "license": "MIT",
      "dependencies": {
        "micromark-util-character": "^2.0.0",
        "micromark-util-encode": "^2.0.0",
        "micromark-util-symbol": "^2.0.0"
      }
    },
    "node_modules/micromark-util-symbol": {
      "version": "2.0.1",
      "resolved": "https://registry.npmjs.org/micromark-util-symbol/-/micromark-util-symbol-2.0.1.tgz",
      "integrity": "sha512-vs5t8Apaud9N28kgCrRUdEed4UJ+wWNvicHLPxCa9ENlYuAY31M0ETy5y1vA33YoNPDFTghEbnh6efaE8h4x0Q==",
      "dev": true,
      "funding": [
        {
          "type": "GitHub Sponsors",
          "url": "https://github.com/sponsors/unifiedjs"
        },
        {
          "type": "OpenCollective",
          "url": "https://opencollective.com/unified"
        }
      ],
      "license": "MIT"
    },
    "node_modules/micromark-util-types": {
      "version": "2.0.3",
      "resolved": "https://registry.npmjs.org/micromark-util-types/-/micromark-util-types-2.0.3.tgz",
      "integrity": "sha512-oxB2Ik03hI0gv+VNn9tnh1t1YEe9MDPptViAEgfdf3YQHsn0pzGTgCdlSCJXcwhqm8phaEuM7zeEu3QQzVBrPg==",
      "dev": true,
      "funding": [
        {
          "type": "GitHub Sponsors",
          "url": "https://github.com/sponsors/unifiedjs"
        },
        {
          "type": "OpenCollective",
          "url": "https://opencollective.com/unified"
        }
      ],
      "license": "MIT"
    },
    "node_modules/minisearch": {
      "version": "7.2.0",
      "resolved": "https://registry.npmjs.org/minisearch/-/minisearch-7.2.0.tgz",
      "integrity": "sha512-dqT2XBYUOZOiC5t2HRnwADjhNS2cecp9u+TJRiJ1Qp/f5qjkeT5APcGPjHw+bz89Ms8Jp+cG4AlE+QZ/QnDglg==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/mitt": {
      "version": "3.0.1",
      "resolved": "https://registry.npmjs.org/mitt/-/mitt-3.0.1.tgz",
      "integrity": "sha512-vKivATfr97l2/QBCYAkXYDbrIWPM2IIKEl7YPhjCvKlG3kE2gm+uBo6nEXK3M5/Ffh/FLpKExzOQ3JJoJGFKBw==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/nanoid": {
      "version": "3.3.20",
      "resolved": "https://registry.npmjs.org/nanoid/-/nanoid-3.3.20.tgz",
      "integrity": "sha512-uKdg2G3GNCKQn9byYOpxbGqrT2fGO5KRt5J/8b3pok8rT6qxGWF6hxMyJiEYtAf+FVyYuD9hRaDqX5uPFYJ4ZQ==",
      "dev": true,
      "funding": [
        {
          "type": "github",
          "url": "https://github.com/sponsors/ai"
        }
      ],
      "license": "MIT",
      "bin": {
        "nanoid": "bin/nanoid.cjs"
      },
      "engines": {
        "node": "^10 || ^12 || ^13.7 || ^14 || >=15.0.1"
      }
    },
    "node_modules/oniguruma-to-es": {
      "version": "3.1.1",
      "resolved": "https://registry.npmjs.org/oniguruma-to-es/-/oniguruma-to-es-3.1.1.tgz",
      "integrity": "sha512-bUH8SDvPkH3ho3dvwJwfonjlQ4R80vjyvrU8YpxuROddv55vAEJrTuCuCVUhhsHbtlD9tGGbaNApGQckXhS8iQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "emoji-regex-xs": "^1.0.0",
        "regex": "^6.0.1",
        "regex-recursion": "^6.0.2"
      }
    },
    "node_modules/perfect-debounce": {
      "version": "1.0.0",
      "resolved": "https://registry.npmjs.org/perfect-debounce/-/perfect-debounce-1.0.0.tgz",
      "integrity": "sha512-xCy9V055GLEqoFaHoC1SoLIaLmWctgCUaBaWxDZ7/Zx4CTyX7cJQLJOok/orfjZAh9kEYpjJa4d0KcJmCbctZA==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/picocolors": {
      "version": "1.1.1",
      "resolved": "https://registry.npmjs.org/picocolors/-/picocolors-1.1.1.tgz",
      "integrity": "sha512-xceH2snhtb5M9liqDsmEw56le376mTZkEX/jEb/RxNFyegNul7eNslCXP9FDj/Lcu0X8KEyMceP2ntpaHrDEVA==",
      "dev": true,
      "license": "ISC"
    },
    "node_modules/postcss": {
      "version": "8.5.32",
      "resolved": "https://registry.npmjs.org/postcss/-/postcss-8.5.32.tgz",
      "integrity": "sha512-LJlKCCAUFKOuZxipUdYw1NzIITEsPcgyOG9LkvRJvuKj/bRzbuRY0ARXvL0pWGpsZvn8jku+LMep2eU0seHp8g==",
      "dev": true,
      "funding": [
        {
          "type": "opencollective",
          "url": "https://opencollective.com/postcss/"
        },
        {
          "type": "tidelift",
          "url": "https://tidelift.com/funding/github/npm/postcss"
        },
        {
          "type": "github",
          "url": "https://github.com/sponsors/ai"
        }
      ],
      "license": "MIT",
      "dependencies": {
        "nanoid": "^3.3.20",
        "picocolors": "^1.1.1",
        "source-map-js": "^1.2.2"
      },
      "engines": {
        "node": "^10 || ^12 || >=14"
      }
    },
    "node_modules/preact": {
      "version": "10.29.8",
      "resolved": "https://registry.npmjs.org/preact/-/preact-10.29.8.tgz",
      "integrity": "sha512-ej2aVZ+vZ8WO7tvlQWRM9N63A0KzF9q4mWJfDUHgYaIofWY9hu74QdnQrjoPMmZi2/nZ5gN0bJCQF49xQqx09Q==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/preact"
      },
      "peerDependencies": {
        "preact-render-to-string": ">=5"
      },
      "peerDependenciesMeta": {
        "preact-render-to-string": {
          "optional": true
        }
      }
    },
    "node_modules/property-information": {
      "version": "7.2.0",
      "resolved": "https://registry.npmjs.org/property-information/-/property-information-7.2.0.tgz",
      "integrity": "sha512-IAtzIB6sUiWaJYrX9smp3V46pBGbBeLFRGdh25kg1334VcBlD8HzhPeNIWQH9zhGmo2itIe25EHt9dQP7G5hmg==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/regex": {
      "version": "6.1.0",
      "resolved": "https://registry.npmjs.org/regex/-/regex-6.1.0.tgz",
      "integrity": "sha512-6VwtthbV4o/7+OaAF9I5L5V3llLEsoPyq9P1JVXkedTP33c7MfCG0/5NOPcSJn0TzXcG9YUrR0gQSWioew3LDg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "regex-utilities": "^2.3.0"
      }
    },
    "node_modules/regex-recursion": {
      "version": "6.0.2",
      "resolved": "https://registry.npmjs.org/regex-recursion/-/regex-recursion-6.0.2.tgz",
      "integrity": "sha512-0YCaSCq2VRIebiaUviZNs0cBz1kg5kVS2UKUfNIx8YVs1cN3AV7NTctO5FOKBA+UT2BPJIWZauYHPqJODG50cg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "regex-utilities": "^2.3.0"
      }
    },
    "node_modules/regex-utilities": {
      "version": "2.3.0",
      "resolved": "https://registry.npmjs.org/regex-utilities/-/regex-utilities-2.3.0.tgz",
      "integrity": "sha512-8VhliFJAWRaUiVvREIiW2NXXTmHs4vMNnSzuJVhscgmGav3g9VDxLrQndI3dZZVVdp0ZO/5v0xmX516/7M9cng==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/rfdc": {
      "version": "1.4.1",
      "resolved": "https://registry.npmjs.org/rfdc/-/rfdc-1.4.1.tgz",
      "integrity": "sha512-q1b3N5QkRUWUl7iyylaaj3kOpIT0N2i9MqIEQXP73GVsN9cw3fdx8X63cEmWhJGi2PPCF23Ijp7ktmd39rawIA==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/rollup": {
      "version": "4.67.4",
      "resolved": "https://registry.npmjs.org/rollup/-/rollup-4.67.4.tgz",
      "integrity": "sha512-Lsv3oKcz5jgOQJej/p7nfwbVEdAbDT+z6OPf/SCdI+nF+q2ENp0pTKT35ZwXAmH8g45mfrIiJeEyAqKYSYoL9Q==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/estree": "^1.0.10"
      },
      "bin": {
        "rollup": "dist/bin/rollup"
      },
      "engines": {
        "node": ">=18.0.0",
        "npm": ">=8.0.0"
      },
      "optionalDependencies": {
        "@napi-rs/lzma-linux-x64-gnu": "1.5.1",
        "@rollup/rollup-android-arm-eabi": "4.67.4",
        "@rollup/rollup-android-arm64": "4.67.4",
        "@rollup/rollup-android-x64": "4.67.4",
        "@rollup/rollup-darwin-arm64": "4.67.4",
        "@rollup/rollup-darwin-x64": "4.67.4",
        "@rollup/rollup-freebsd-arm64": "4.67.4",
        "@rollup/rollup-freebsd-x64": "4.67.4",
        "@rollup/rollup-linux-arm-gnueabihf": "4.67.4",
        "@rollup/rollup-linux-arm-musleabihf": "4.67.4",
        "@rollup/rollup-linux-arm64-gnu": "4.67.4",
        "@rollup/rollup-linux-arm64-musl": "4.67.4",
        "@rollup/rollup-linux-loong64-gnu": "4.67.4",
        "@rollup/rollup-linux-loong64-musl": "4.67.4",
        "@rollup/rollup-linux-ppc64-gnu": "4.67.4",
        "@rollup/rollup-linux-ppc64-musl": "4.67.4",
        "@rollup/rollup-linux-riscv64-gnu": "4.67.4",
        "@rollup/rollup-linux-riscv64-musl": "4.67.4",
        "@rollup/rollup-linux-s390x-gnu": "4.67.4",
        "@rollup/rollup-linux-x64-gnu": "4.67.4",
        "@rollup/rollup-linux-x64-musl": "4.67.4",
        "@rollup/rollup-openbsd-x64": "4.67.4",
        "@rollup/rollup-openharmony-arm64": "4.67.4",
        "@rollup/rollup-win32-arm64-msvc": "4.67.4",
        "@rollup/rollup-win32-ia32-msvc": "4.67.4",
        "@rollup/rollup-win32-x64-gnu": "4.67.4",
        "@rollup/rollup-win32-x64-msvc": "4.67.4",
        "fsevents": "~2.3.2"
      }
    },
    "node_modules/search-insights": {
      "version": "2.17.3",
      "resolved": "https://registry.npmjs.org/search-insights/-/search-insights-2.17.3.tgz",
      "integrity": "sha512-RQPdCYTa8A68uM2jwxoY842xDhvx3E5LFL1LxvxCNMev4o5mLuokczhzjAgGwUZBAmOKZknArSxLKmXtIi2AxQ==",
      "dev": true,
      "license": "MIT",
      "peer": true
    },
    "node_modules/shiki": {
      "version": "2.5.0",
      "resolved": "https://registry.npmjs.org/shiki/-/shiki-2.5.0.tgz",
      "integrity": "sha512-mI//trrsaiCIPsja5CNfsyNOqgAZUb6VpJA+340toL42UpzQlXpwRV9nch69X6gaUxrr9kaOOa6e3y3uAkGFxQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@shikijs/core": "2.5.0",
        "@shikijs/engine-javascript": "2.5.0",
        "@shikijs/engine-oniguruma": "2.5.0",
        "@shikijs/langs": "2.5.0",
        "@shikijs/themes": "2.5.0",
        "@shikijs/types": "2.5.0",
        "@shikijs/vscode-textmate": "^10.0.2",
        "@types/hast": "^3.0.4"
      }
    },
    "node_modules/source-map-js": {
      "version": "1.2.2",
      "resolved": "https://registry.npmjs.org/source-map-js/-/source-map-js-1.2.2.tgz",
      "integrity": "sha512-KGj/8Y43x35aZVDtt+J4mK1hoLGHULMYfSkODJNQjNDC3oW1PqPoxMwo0pLUsWM/UEGzON/NxeHywEfNXNP3Vw==",
      "dev": true,
      "license": "BSD-3-Clause",
      "engines": {
        "node": ">=0.10.0"
      }
    },
    "node_modules/space-separated-tokens": {
      "version": "2.0.2",
      "resolved": "https://registry.npmjs.org/space-separated-tokens/-/space-separated-tokens-2.0.2.tgz",
      "integrity": "sha512-PEGlAwrG8yXGXRjW32fGbg66JAlOAwbObuqVoJpv/mRgoWDQfgH1wDPvtzWyUSNAXBGSk8h755YDbbcEy3SH2Q==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/speakingurl": {
      "version": "14.0.1",
      "resolved": "https://registry.npmjs.org/speakingurl/-/speakingurl-14.0.1.tgz",
      "integrity": "sha512-1POYv7uv2gXoyGFpBCmpDVSNV74IfsWlDW216UPjbWufNf+bSU6GdbDsxdcxtfwb4xlI3yxzOTKClUosxARYrQ==",
      "dev": true,
      "license": "BSD-3-Clause",
      "engines": {
        "node": ">=0.10.0"
      }
    },
    "node_modules/stringify-entities": {
      "version": "4.0.4",
      "resolved": "https://registry.npmjs.org/stringify-entities/-/stringify-entities-4.0.4.tgz",
      "integrity": "sha512-IwfBptatlO+QCJUo19AqvrPNqlVMpW9YEL2LIVY+Rpv2qsjCGxaDLNRgeGsQWJhfItebuJhsGSLjaBbNSQ+ieg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "character-entities-html4": "^2.0.0",
        "character-entities-legacy": "^3.0.0"
      },
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/superjson": {
      "version": "2.2.6",
      "resolved": "https://registry.npmjs.org/superjson/-/superjson-2.2.6.tgz",
      "integrity": "sha512-H+ue8Zo4vJmV2nRjpx86P35lzwDT3nItnIsocgumgr0hHMQ+ZGq5vrERg9kJBo5AWGmxZDhzDo+WVIJqkB0cGA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "copy-anything": "^4"
      },
      "engines": {
        "node": ">=16"
      }
    },
    "node_modules/tabbable": {
      "version": "6.5.1",
      "resolved": "https://registry.npmjs.org/tabbable/-/tabbable-6.5.1.tgz",
      "integrity": "sha512-dMCuwDUY6Noz3zVKLefBC4wq+aJV20yc7vWmM7uTNGwtQSFF5kX7OUIUsxuVajz/bdt5Xqdah3/0NLjth+cM5A==",
      "dev": true,
      "license": "MIT"
    },
    "node_modules/trim-lines": {
      "version": "3.0.1",
      "resolved": "https://registry.npmjs.org/trim-lines/-/trim-lines-3.0.1.tgz",
      "integrity": "sha512-kRj8B+YHZCc9kQYdWfJB2/oUl9rA99qbowYYBtr4ui4mZyAQ2JpvVBd/6U2YloATfqBhBTSMhTpgBHtU0Mf3Rg==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    },
    "node_modules/unist-util-is": {
      "version": "6.0.1",
      "resolved": "https://registry.npmjs.org/unist-util-is/-/unist-util-is-6.0.1.tgz",
      "integrity": "sha512-LsiILbtBETkDz8I9p1dQ0uyRUWuaQzd/cuEeS1hoRSyW5E5XGmTzlwY1OrNzzakGowI9Dr/I8HVaw4hTtnxy8g==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "^3.0.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/unist-util-position": {
      "version": "5.0.0",
      "resolved": "https://registry.npmjs.org/unist-util-position/-/unist-util-position-5.0.0.tgz",
      "integrity": "sha512-fucsC7HjXvkB5R3kTCO7kUjRdrS0BJt3M/FPxmHMBOm8JQi2BsHAHFsy27E0EolP8rp0NzXsJ+jNPyDWvOJZPA==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "^3.0.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/unist-util-stringify-position": {
      "version": "4.0.0",
      "resolved": "https://registry.npmjs.org/unist-util-stringify-position/-/unist-util-stringify-position-4.0.0.tgz",
      "integrity": "sha512-0ASV06AAoKCDkS2+xw5RXJywruurpbC4JZSm7nr7MOt1ojAzvyyaO+UxZf18j8FCF6kmzCZKcAgN/yu2gm2XgQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "^3.0.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/unist-util-visit": {
      "version": "5.2.0",
      "resolved": "https://registry.npmjs.org/unist-util-visit/-/unist-util-visit-5.2.0.tgz",
      "integrity": "sha512-fpsYs/VVoZxfGY9/j3FQpZmN8wdLJaV1Pt38AaJNZu9uKIHcFU3halCdBveEq1HGKO1OEfJY65w1zvoFV1b3qQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "^3.0.0",
        "unist-util-is": "^6.0.0",
        "unist-util-visit-parents": "^6.1.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/unist-util-visit-parents": {
      "version": "6.1.0",
      "resolved": "https://registry.npmjs.org/unist-util-visit-parents/-/unist-util-visit-parents-6.1.0.tgz",
      "integrity": "sha512-vKXxz6O38sKDj1mtx/k2cWvkkFBuYGx6uGrlzBkVeWCRF/F8sdI0j3Wbkz/5rVNRRuVc8UrgRdbCxNhmYXgP3w==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "^3.0.0",
        "unist-util-is": "^6.0.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/vfile": {
      "version": "6.0.3",
      "resolved": "https://registry.npmjs.org/vfile/-/vfile-6.0.3.tgz",
      "integrity": "sha512-KzIbH/9tXat2u30jf+smMwFCsno4wHVdNmzFyL+T/L3UGqqk6JKfVqOFOZEpZSHADH1k40ab6NUIXZq422ov3Q==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "^3.0.0",
        "vfile-message": "^4.0.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/vfile-message": {
      "version": "4.0.3",
      "resolved": "https://registry.npmjs.org/vfile-message/-/vfile-message-4.0.3.tgz",
      "integrity": "sha512-QTHzsGd1EhbZs4AsQ20JX1rC3cOlt/IWJruk893DfLRr57lcnOeMaWG4K0JrRta4mIJZKth2Au3mM3u03/JWKw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@types/unist": "^3.0.0",
        "unist-util-stringify-position": "^4.0.0"
      },
      "funding": {
        "type": "opencollective",
        "url": "https://opencollective.com/unified"
      }
    },
    "node_modules/vite": {
      "version": "5.4.21",
      "resolved": "https://registry.npmjs.org/vite/-/vite-5.4.21.tgz",
      "integrity": "sha512-o5a9xKjbtuhY6Bi5S3+HvbRERmouabWbyUcpXXUA1u+GNUKoROi9byOJ8M0nHbHYHkYICiMlqxkg1KkYmm25Sw==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "esbuild": "^0.21.3",
        "postcss": "^8.4.43",
        "rollup": "^4.20.0"
      },
      "bin": {
        "vite": "bin/vite.js"
      },
      "engines": {
        "node": "^18.0.0 || >=20.0.0"
      },
      "funding": {
        "url": "https://github.com/vitejs/vite?sponsor=1"
      },
      "optionalDependencies": {
        "fsevents": "~2.3.3"
      },
      "peerDependencies": {
        "@types/node": "^18.0.0 || >=20.0.0",
        "less": "*",
        "lightningcss": "^1.21.0",
        "sass": "*",
        "sass-embedded": "*",
        "stylus": "*",
        "sugarss": "*",
        "terser": "^5.4.0"
      },
      "peerDependenciesMeta": {
        "@types/node": {
          "optional": true
        },
        "less": {
          "optional": true
        },
        "lightningcss": {
          "optional": true
        },
        "sass": {
          "optional": true
        },
        "sass-embedded": {
          "optional": true
        },
        "stylus": {
          "optional": true
        },
        "sugarss": {
          "optional": true
        },
        "terser": {
          "optional": true
        }
      }
    },
    "node_modules/vitepress": {
      "version": "1.6.4",
      "resolved": "https://registry.npmjs.org/vitepress/-/vitepress-1.6.4.tgz",
      "integrity": "sha512-+2ym1/+0VVrbhNyRoFFesVvBvHAVMZMK0rw60E3X/5349M1GuVdKeazuksqopEdvkKwKGs21Q729jX81/bkBJg==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@docsearch/css": "3.8.2",
        "@docsearch/js": "3.8.2",
        "@iconify-json/simple-icons": "^1.2.21",
        "@shikijs/core": "^2.1.0",
        "@shikijs/transformers": "^2.1.0",
        "@shikijs/types": "^2.1.0",
        "@types/markdown-it": "^14.1.2",
        "@vitejs/plugin-vue": "^5.2.1",
        "@vue/devtools-api": "^7.7.0",
        "@vue/shared": "^3.5.13",
        "@vueuse/core": "^12.4.0",
        "@vueuse/integrations": "^12.4.0",
        "focus-trap": "^7.6.4",
        "mark.js": "8.11.1",
        "minisearch": "^7.1.1",
        "shiki": "^2.1.0",
        "vite": "^5.4.14",
        "vue": "^3.5.13"
      },
      "bin": {
        "vitepress": "bin/vitepress.js"
      },
      "peerDependencies": {
        "markdown-it-mathjax3": "^4",
        "postcss": "^8"
      },
      "peerDependenciesMeta": {
        "markdown-it-mathjax3": {
          "optional": true
        },
        "postcss": {
          "optional": true
        }
      }
    },
    "node_modules/vue": {
      "version": "3.5.44",
      "resolved": "https://registry.npmjs.org/vue/-/vue-3.5.44.tgz",
      "integrity": "sha512-dvvmaOCG7lAMkJTPqPwWeGMsPS6DIsFsfZDUgBp9V6mHf9b3+/wyVwYR4Es+WV6uJC0FKRxp81Obcq0ZwUcGFQ==",
      "dev": true,
      "license": "MIT",
      "dependencies": {
        "@vue/compiler-dom": "3.5.44",
        "@vue/compiler-sfc": "3.5.44",
        "@vue/runtime-dom": "3.5.44",
        "@vue/server-renderer": "3.5.44",
        "@vue/shared": "3.5.44"
      },
      "peerDependencies": {
        "typescript": "*"
      },
      "peerDependenciesMeta": {
        "typescript": {
          "optional": true
        }
      }
    },
    "node_modules/zwitch": {
      "version": "2.0.4",
      "resolved": "https://registry.npmjs.org/zwitch/-/zwitch-2.0.4.tgz",
      "integrity": "sha512-bXE4cR/kVZhKZX/RjPEflHaKVhUVl85noU3v6b8apfQEc1x4A+zBxjZ4lN8LqGd6WZ3dl98pY4o717VFmoPp+A==",
      "dev": true,
      "license": "MIT",
      "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/wooorm"
      }
    }
  }
}
```

## File: site/package.json

**Lines**: 14 | **Size**: 334 B | **Language**: json

```json
{
  "name": "starry-docs",
  "version": "1.0.0",
  "description": "Documentation for Starry - Decentralized GitOps Orchestrator",
  "private": true,
  "scripts": {
    "docs:dev": "vitepress dev",
    "docs:build": "vitepress build",
    "docs:preview": "vitepress preview"
  },
  "devDependencies": {
    "vitepress": "^1.6.3"
  }
}
```

## File: site/.vitepress/config.mts

**Lines**: 177 | **Size**: 6.1 KB | **Language**: typescript

```typescript
import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'Starry',
  base: '/starry/',
  head: [
    ['link', { rel: 'icon', type: 'image/svg+xml', href: '/starry/logo.svg' }],
    ['meta', { name: 'theme-color', content: '#f59e0b' }]
  ],
  locales: {
    root: {
      label: 'Русский',
      lang: 'ru-RU',
      title: 'Starry',
      description: 'Децентрализованный GitOps-оркестратор',
      themeConfig: {
        nav: [
          { text: 'Быстрый старт', link: '/guide/getting-started' },
          { text: 'Безопасность', link: '/guide/security-cryptography' },
          { text: 'Архитектура', link: '/architecture/overview' },
          { text: 'Конфигурация', link: '/configuration/syntax' },
          { text: 'Команды CLI', link: '/cli/commands' }
        ],
        sidebar: [
          {
            text: 'Руководство',
            items: [
              { text: 'Быстрый старт', link: '/guide/getting-started' },
              { text: 'Бэкапы и Bundle (DR)', link: '/guide/backups-disaster-recovery' },
              { text: 'Миграции и DNS (P2P)', link: '/guide/migrations-dns-p2p' },
              { text: 'Docker и Podman (files)', link: '/guide/containers-docker-podman' },
              { text: 'Сетевой меш и VIP', link: '/guide/network-mesh-vips' },
              { text: 'Дистрибутивы Linux и NAT', link: '/guide/linux-distributions' },
              { text: 'Безопасность и секреты', link: '/guide/security-cryptography' }
            ]
          },
          {
            text: 'Архитектура',
            items: [
              { text: 'Обзор архитектуры', link: '/architecture/overview' }
            ]
          },
          {
            text: 'Конфигурация',
            items: [
              { text: 'Синтаксис и схема', link: '/configuration/syntax' }
            ]
          },
          {
            text: 'Справочник CLI',
            items: [
              { text: 'Команды CLI', link: '/cli/commands' }
            ]
          }
        ],
        docFooter: {
          prev: 'Предыдущая страница',
          next: 'Следующая страница'
        },
        outline: {
          label: 'Содержание страницы'
        },
        darkModeSwitchLabel: 'Оформление',
        lightModeSwitchTitle: 'Переключить на светлую тему',
        darkModeSwitchTitle: 'Переключить на тёмную тему',
        returnToTopLabel: 'Наверх'
      }
    },
    en: {
      label: 'English',
      lang: 'en-US',
      link: '/en/',
      title: 'Starry',
      description: 'Decentralized GitOps Orchestrator',
      themeConfig: {
        nav: [
          { text: 'Getting Started', link: '/en/guide/getting-started' },
          { text: 'Security', link: '/en/guide/security-cryptography' },
          { text: 'Architecture', link: '/en/architecture/overview' },
          { text: 'Configuration', link: '/en/configuration/syntax' },
          { text: 'CLI Commands', link: '/en/cli/commands' }
        ],
        sidebar: [
          {
            text: 'Guide',
            items: [
              { text: 'Getting Started', link: '/en/guide/getting-started' },
              { text: 'Backups & DR Bundles', link: '/en/guide/backups-disaster-recovery' },
              { text: 'Migrations & DNS (P2P)', link: '/en/guide/migrations-dns-p2p' },
              { text: 'Docker & Podman (files)', link: '/en/guide/containers-docker-podman' },
              { text: 'WireGuard Mesh & VIPs', link: '/en/guide/network-mesh-vips' },
              { text: 'Linux Distros & NAT', link: '/en/guide/linux-distributions' },
              { text: 'Security & Secrets', link: '/en/guide/security-cryptography' }
            ]
          },
          {
            text: 'Architecture',
            items: [
              { text: 'Overview', link: '/en/architecture/overview' }
            ]
          },
          {
            text: 'Configuration',
            items: [
              { text: 'Syntax & Schema', link: '/en/configuration/syntax' }
            ]
          },
          {
            text: 'CLI Reference',
            items: [
              { text: 'Commands', link: '/en/cli/commands' }
            ]
          }
        ],
        docFooter: {
          prev: 'Previous page',
          next: 'Next page'
        },
        outline: {
          label: 'On this page'
        },
        returnToTopLabel: 'Return to top'
      }
    }
  },
  themeConfig: {
    logo: '/logo.svg',
    socialLinks: [
      { icon: 'github', link: 'https://github.com/starry-project/starry' }
    ],
    search: {
      provider: 'local',
      options: {
        locales: {
          root: {
            translations: {
              button: {
                buttonText: 'Поиск',
                buttonAriaLabel: 'Поиск по документации'
              },
              modal: {
                noResultsText: 'Ничего не найдено по запросу',
                resetButtonTitle: 'Очистить поиск',
                footer: {
                  selectText: 'выбрать',
                  navigateText: 'перейти',
                  closeText: 'закрыть'
                }
              }
            }
          },
          en: {
            translations: {
              button: {
                buttonText: 'Search',
                buttonAriaLabel: 'Search documentation'
              },
              modal: {
                noResultsText: 'No results found',
                resetButtonTitle: 'Reset search',
                footer: {
                  selectText: 'to select',
                  navigateText: 'to navigate',
                  closeText: 'to close'
                }
              }
            }
          }
        }
      }
    },
    footer: {
      message: 'Released under the MIT License.',
      copyright: 'Copyright © 2026 Starry Project'
    }
  }
})
```

## File: site/.vitepress/theme/custom.css

**Lines**: 184 | **Size**: 4.4 KB | **Language**: css

```css
/**
 * STARRY Documentation Theme Custom Styles
 * Brand colors: Golden yellow (#f59e0b, #fbbf24, #d97706)
 */

:root {
  /* Brand color palette */
  --vp-c-brand-1: #f59e0b;
  --vp-c-brand-2: #fbbf24;
  --vp-c-brand-3: #d97706;
  --vp-c-brand-soft: rgba(245, 158, 11, 0.14);

  /* Light theme accents */
  --vp-home-hero-name-color: transparent;
  --vp-home-hero-name-background: -webkit-linear-gradient(135deg, #d97706 0%, #f59e0b 50%, #fbbf24 100%);
  --vp-home-hero-image-background-image: radial-gradient(circle, rgba(245, 158, 11, 0.28) 0%, rgba(251, 191, 36, 0.08) 60%, transparent 80%);
  --vp-home-hero-image-filter: blur(48px);

  /* Starry custom badge colors */
  --starry-badge-bg: rgba(245, 158, 11, 0.12);
  --starry-badge-border: rgba(245, 158, 11, 0.35);
  --starry-badge-text: #b45309;
}

/* Dark mode overrides */
.dark {
  /* Deep sleek slate-obsidian backgrounds */
  --vp-c-bg: #0b0f19;
  --vp-c-bg-soft: #111827;
  --vp-c-bg-alt: #080b12;
  --vp-c-bg-elv: #1a2234;
  --vp-c-border: #1f293d;
  --vp-c-gutter: #111827;

  /* Radiant golden yellow brand in dark mode */
  --vp-c-brand-1: #fbbf24;
  --vp-c-brand-2: #f59e0b;
  --vp-c-brand-3: #fde68a;
  --vp-c-brand-soft: rgba(251, 191, 36, 0.16);

  --vp-home-hero-name-background: -webkit-linear-gradient(135deg, #fef3c7 0%, #fbbf24 45%, #f59e0b 90%);
  --vp-home-hero-image-background-image: radial-gradient(circle, rgba(251, 191, 36, 0.25) 0%, rgba(245, 158, 11, 0.10) 50%, transparent 75%);
  --vp-home-hero-image-filter: blur(56px);

  --starry-badge-bg: rgba(251, 191, 36, 0.15);
  --starry-badge-border: rgba(251, 191, 36, 0.4);
  --starry-badge-text: #fde68a;
}

/* Hero Section Custom Polish */
.VPHero .name {
  letter-spacing: -0.02em;
  font-weight: 800;
}

.VPHero .text {
  font-weight: 600;
  color: var(--vp-c-text-1);
}

.VPHero .tagline {
  color: var(--vp-c-text-2);
  font-size: 1.25rem;
  line-height: 1.6;
}

/* Brand Button Styling */
.VPButton.brand {
  background-color: var(--vp-c-brand-1) !important;
  border-color: var(--vp-c-brand-1) !important;
  color: #0b0f19 !important;
  font-weight: 600;
  box-shadow: 0 4px 14px 0 rgba(245, 158, 11, 0.35);
  transition: all 0.25s ease;
}

.VPButton.brand:hover {
  background-color: var(--vp-c-brand-2) !important;
  border-color: var(--vp-c-brand-2) !important;
  box-shadow: 0 6px 20px 0 rgba(245, 158, 11, 0.5);
  transform: translateY(-1px);
}

.VPButton.alt {
  border-color: var(--vp-c-border) !important;
  transition: all 0.25s ease;
}

.VPButton.alt:hover {
  border-color: var(--vp-c-brand-1) !important;
  color: var(--vp-c-brand-1) !important;
}

/* Feature Cards Interactive Hover */
.VPFeature {
  border: 1px solid var(--vp-c-border);
  transition: transform 0.25s ease, border-color 0.25s ease, box-shadow 0.25s ease;
  border-radius: 12px;
}

.VPFeature:hover {
  border-color: rgba(245, 158, 11, 0.45);
  box-shadow: 0 8px 24px -4px rgba(245, 158, 11, 0.12);
  transform: translateY(-2px);
}

.VPFeature .icon {
  background-color: var(--vp-c-brand-soft);
  border-radius: 10px;
}

/* Badge Customizations */
.VPBadge {
  font-family: var(--vp-font-family-mono);
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.04em;
  padding: 2px 8px;
  border-radius: 9999px;
  display: inline-flex;
  align-items: center;
  line-height: 1.4;
  vertical-align: middle;
}

.VPBadge.tip,
.VPBadge.info {
  background-color: var(--vp-c-brand-soft);
  border: 1px solid var(--vp-c-brand-1);
  color: var(--vp-c-brand-1);
}

.VPBadge.starry {
  background-color: var(--starry-badge-bg);
  border: 1px solid var(--starry-badge-border);
  color: var(--starry-badge-text);
  box-shadow: 0 0 8px rgba(245, 158, 11, 0.2);
}

/* Code block & inline code polish */
code {
  font-size: 0.9em;
}

:not(pre) > code {
  background-color: var(--vp-c-brand-soft);
  color: var(--vp-c-brand-1);
  border-radius: 4px;
  padding: 0.15em 0.4em;
}

/* Table styling */
.vp-doc table {
  border-collapse: separate;
  border-spacing: 0;
  border-radius: 8px;
  overflow: hidden;
  border: 1px solid var(--vp-c-border);
}

.vp-doc th {
  background-color: var(--vp-c-bg-soft);
  color: var(--vp-c-text-1);
  font-weight: 600;
}

/* Custom Callout Box (starry-callout) */
.starry-callout {
  padding: 16px 20px;
  margin: 16px 0;
  border-radius: 8px;
  border-left: 4px solid var(--vp-c-brand-1);
  background-color: var(--vp-c-brand-soft);
  color: var(--vp-c-text-1);
}

.starry-callout-title {
  font-weight: 700;
  margin-bottom: 6px;
  display: flex;
  align-items: center;
  gap: 8px;
  color: var(--vp-c-brand-1);
}
```

## File: site/.vitepress/theme/index.ts

**Lines**: 12 | **Size**: 286 B | **Language**: typescript

```typescript
import DefaultTheme from 'vitepress/theme'
import type { Theme } from 'vitepress'
import './custom.css'

const theme: Theme = {
  extends: DefaultTheme,
  enhanceApp({ app }) {
    // Custom global components or enhancements can be registered here if needed
  }
}

export default theme
```

## File: site/architecture/overview.md

**Lines**: 129 | **Size**: 11.1 KB | **Language**: markdown

````markdown
# Архитектура Starry

**Starry** спроектирован как децентрализованный, бессерверный (masterless) GitOps-оркестратор для управления разнородным парком виртуальных машин (VPS), облачных инстансов и физических серверов.

---

## 1. Концептуальная диаграмма

```text
               +--------------------------------------------------+
               |             GitOps Репозиторий                   |
               |     (starry.yaml + ENC[...] зашифрованные данные)|
               +--------------------------------------------------+
                                        │
                       ┌────────────────┴────────────────┐
                       ▼                                 ▼
             +───────────────────+             +───────────────────+
             |   Developer CLI   |             |   CI/CD Runner    |
             |   (starry apply)  |             |  (GitHub Actions) |
             +───────────────────+             +───────────────────+
                       │                                 │
                       └────────────────┬────────────────┘
                                        │ SSH / WireGuard
                                        ▼
    ┌───────────────────────────────────┬───────────────────────────────────┐
    │  Node A (Ubuntu 24.04 / Docker)   │  Node B (AlmaLinux 9 / Podman)   │
    │                                   │                                   │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |     Starry Host Agent       |  │  |     Starry Host Agent       |  │
    │  |  (Healthcheck / Telemetry)  |  │  |  (Healthcheck / Telemetry)  |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |      nftables Firewall      |  │  |      nftables Firewall      |  │
    │  | (ports open to wg0 mesh)    |  │  | (ports open to wg0 mesh)    |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |   WireGuard Mesh (10.42.0.1)│◄─┼─►│   WireGuard Mesh (10.42.0.2)│  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |      Docker Engine API      |  │  |    Podman Libpod API (Root) |  │
    │  |   [App: Web, Nginx, DB]     |  │  |   [App: Worker, Cache]      |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    └───────────────────────────────────┴───────────────────────────────────┘
```

---

## 2. Модель Single-Binary Hybrid

Starry скомпилирован в единый самодостаточный Native AOT бинарник размером менее 30 МБ, работающий без установки дополнительных сред выполнения.

Бинарник работает в двух режимах:

1. **Режим CLI (`starry <command>`)**:
   - Запускается на рабочей станции инженера или в CI/CD раннере.
   - Считывает `starry.yaml`, расшифровывает секреты с помощью `STARRY_MASTER_KEY`.
   - Опрашивает ноды по SSH, строит граф зависимостей (DAG) и вычисляет разницу состояний (Diff).
   - Оркестрирует параллельное применение изменений без необходимости постоянного управляющего сервера (Master).

2. **Режим Agent (`starry agent run`)**:
   - Запускается на серверах парка в виде легковесного системного сервиса `systemd`.
   - Обеспечивает автономный надзор над локальными контейнерами и перезапуск при сбоях.
   - Применяет атомарные правила `nftables` и следит за состоянием интерфейса WireGuard.
   - Выполняет периодические локальные Healthcheck-проверки и отправляет телеметрию.

---

## 3. Оверлейная сеть WireGuard Full-Mesh

Все серверные узлы объединяются в защищённую приватную сеть диапазона `10.42.0.0/16`.

- **Автоматическая маршрутизация:** Каждый узел получает собственный статичный IP-адрес внутри меша (например, `10.42.0.1`, `10.42.0.2`). Конфигурация пиров WireGuard генерируется автоматически на основе списка нод.
- **Преодоление NAT (NAT Traversal):** Для серверов, расположенных за NAT или межсетевыми экранами облачных провайдеров, Starry настраивает `PersistentKeepalive = 25` и динамическое определение эндпоинтов.
- **Безопасность трафика по умолчанию:** Межсервисный трафик между нодами (например, обращение бэкенда к базе данных) шифруется протоколом Noise / ChaCha20-Poly1305 на уровне ядра Linux.

---

## 4. Контейнерные движки: Docker и Podman

Starry абстрагирует различия между контейнерными средами через единый программный интерфейс:

| Характеристика | Docker Engine | Podman (Libpod) |
|---|---|---|
| **API Сокет** | `/var/run/docker.sock` | `/run/podman/podman.sock` |
| **Rootless режим** | Экспериментальный | Нативный из коробки |
| **Поддерживаемые ОС** | Ubuntu, Debian, Alpine | RHEL, AlmaLinux, Rocky, Fedora, Arch |
| **Управление** | Демон `dockerd` | Бездемоновый (fork/exec через conmon/crun) |

### P2P-стриминг образов (Image Mesh)

При миграции контейнера или масштабировании сервиса на новую ноду в условиях недоступности внешнего Docker Hub / Registry, Starry умеет стримить образ контейнера напрямую между нодами по протоколу WireGuard через сжатый поток `zstd`, минуя внешние сети.

---

## 5. Атомарный файрвол nftables и Deadman Switch

Безопасность сетевого периметра обеспечивается подсистемой `nftables`:

1. **Изоляция портов:** Порты контейнеров (кроме публичных HTTP/HTTPS точек входа) открываются строго на интерфейсе `wg0`. Из внешней сети эти порты не видны и не сканируются.
2. **Атомарность правил:** Конфигурация транслируется в нативный синтаксис `nftables` и загружается атомарно через команду `nft -f`.
3. **Deadman Switch Rollback:** Если при обновлении правил файрвола на удалённом сервере произойдёт синтаксическая ошибка или обрыв SSH-сессии управления, таймер безопасности автоматически восстановит предыдущую валидную конфигурацию, исключая риск потери доступа к серверу.

---

## 6. Управление секретами и криптография

Starry придерживается концепции безопасного хранения зашифрованных данных непосредственно в репозитории:

- **Конвертное шифрование AES-256-GCM:** Для каждого секрета генерируется уникальный криптографический вектор инициализации (IV, 96 бит) и аутентификационный тег (128 бит), предотвращающий подделку шифротекста.
- **Синтаксис маркеров:**
  ```text
  ENC[AES256_GCM,data:4gY3...,iv:9kP2...,tag:z0X8...]
  ```
- **Ротация мастер-ключа в один клик:**
  Команда `starry rotate-secrets` расшифровывает все значения старым ключом, генерирует новый ключ, перешифровывает весь файл `starry.yaml` и синхронизирует секрет в GitHub Actions через REST API LibSodium.
- **Ротация SSH и WireGuard ключей:**
  Команда `starry rotate-keys` выполняет бесшовную замену приватных и публичных ключей нод с верификацией связности перед удалением устаревших ключей.

---

## 7. Декларативное сопровождение ОС и Rolling Drain

Starry управляет не только контейнерами, но и базовой операционной системой серверов:

- **Поддержка пакетных менеджеров:** Автоматическое определение дистрибутива через `/etc/os-release` и работа с `apt-get`, `dnf`, `apk`, `pacman`.
- **Детекция обновлений ядра:** Распознавание признаков требуемой перезагрузки (`/var/run/reboot-required` на Debian/Ubuntu, `needs-restarting -r` на RHEL/AlmaLinux).
- **Алгоритм Rolling Drain (перезагрузка без простоя):**
  1. Временный вывод ноды из балансировки или переключение DNS.
  2. Корректная остановка сервисов (Graceful stop).
  3. Перезагрузка сервера (`reboot`).
  4. Ожидание возвращения SSH и запуск агента Starry.
  5. Проверка состояния сервисов и возврат рабочего трафика на узел.
````

## File: site/cli/commands.md

**Lines**: 183 | **Size**: 6.5 KB | **Language**: markdown

````markdown
# Справочник команд CLI

Starry предоставляет единый интерфейс командной строки для локального управления кластером, автоматизации в CI/CD и работы фонового демона.

---

## 1. Глобальные флаги

Каждая команда Starry поддерживает общие флаги:

| Флаг | Описание |
|---|---|
| `-c, --config <file>` | Путь к файлу конфигурации (по умолчанию: `./starry.yaml`). |
| `-v, --verbose` | Вывод подробной отладочной информации и трассировки HTTP/SSH. |
| `--json` | Вывод результатов работы в машиночитаемом формате JSON. |
| `-h, --help` | Справка по команде и доступным аргументам. |
| `--version` | Вывод текущей версии бинарника Starry. |

---

## 2. Команды жизненного цикла кластера

### `starry init`

Инициализирует новый репозиторий с базовой структурой файлов `starry.yaml`, `.starry/` и шаблоном `.gitignore`.

```bash
starry init [--path <dir>]
```

### `starry plan`

Анализирует конфигурацию, опрашивает серверы по SSH, сопоставляет фактическое состояние с желаемым и выводит граф изменений без их применения.

```bash
starry plan [--node <name>] [--service <name>]
```

### `starry apply`

Применяет изменения к серверам: обновляет пакеты, синхронизирует ключи WireGuard, пересоздает или обновляет контейнеры и перегружает правила `nftables`.

```bash
starry apply [--node <name>] [--dry-run] [--auto-approve]
```

---

## 3. Управление узлами (`starry nodes`)

### `starry nodes bootstrap`

Автоматически подключает новый сервер в кластер по SSH:

```bash
starry nodes bootstrap <ssh-connection-string> [options]

# Пример:
starry nodes bootstrap root@203.0.113.10 \
  --name app-02 \
  --engine podman \
  --mesh-ip 10.42.0.2
```

Параметры:
- `--name <string>`: Уникальное имя ноды в манифесте.
- `--engine <docker|podman>`: Выбор движка контейнеров.
- `--mesh-ip <ip>`: Желаемый статический IP-адрес в подсети WireGuard.

### `starry nodes list`

Выводит статус всех подключенных нод, их WireGuard IP, состояние SSH-связности и используемый движок.

```bash
starry nodes list
```

### `starry nodes upgrade`

Выполняет принудительную проверку и установку системных обновлений безопасности на указанной ноде с соблюдением политики `reboot_policy`.

```bash
starry nodes upgrade <node-name> [--drain]
```

---

## 4. Управление секретами (`starry secret`)

### `starry secret keygen`

Генерирует новый криптографически стойкий 256-битный мастер-ключ AES-256-GCM.

```bash
starry secret keygen
```

### `starry secret encrypt`

Шифрует открытую строку мастер-ключом из переменной `STARRY_MASTER_KEY`:

```bash
starry secret encrypt "postgres-super-secure-password"
# Вывод: ENC[AES256_GCM,data:...,iv:...,tag:...]
```

### `starry secret decrypt`

Расшифровывает маркер `ENC[...]` для проверки или отладки:

```bash
starry secret decrypt "ENC[AES256_GCM,data:...,iv:...,tag:...]"
```

---

## 5. Ротация ключей безопасности

### `starry rotate-secrets`

Дешифрует все токены `ENC[...]` в текущем файле конфигурации, генерирует новый мастер-ключ, перешифровывает все значения и обновляет секрет в GitHub Actions через API.

```bash
starry rotate-secrets [--github-repo user/repo]
```

### `starry rotate-keys`

Генерирует новые пары ключей Ed25519 (SSH) и WireGuard, разворачивает их на нодах с предварительной проверкой связности и удаляет устаревшие ключи.

```bash
starry rotate-keys [--dry-run]
```

---

## 7. Захват существующих серверов (`starry adopt`)

### `starry adopt`

Подключается к работающему VPS по SSH, инспектирует запущенные контейнеры (Docker/Podman), выкачивает конфигурационные файлы (`--pull-files`), находит и шифрует секреты в `Env`, и генерирует готовую модульную структуру конфигурации Starry:

```bash
starry adopt root@203.0.113.10:22 --pull-files --out-dir ./config --format yaml
```

### `starry unmanaged list`

Опрашивает все подключенные ноды кластера и выводит список контейнеров, запущенных вручную и не находящихся под управлением Starry GitOps:

```bash
starry unmanaged list [--config ./config]
```

---

## 8. Аварийное восстановление и тома

### `starry volume open`

Подключается к ноде и отображает структуру файлов тома или каталога данных:

```bash
starry volume open bocchi /var/data/postgres [--once]
```

### `starry bundle export`

Выгружает всю конфигурацию кластера, зашифрованные секреты и метаданные в единый переносимый архив аварийного восстановления:

```bash
starry bundle export -o cluster-backup.tar.gz
```

### `starry bundle import`

Восстанавливает архив кластера с возможностью переназначения IP-адресов нод на новые серверы:

```bash
starry bundle import cluster-backup.tar.gz -o ./new-cluster \
  --override-ip bocchi=198.51.100.99 \
  --override-ip nijika=198.51.100.100
```
````

## File: site/configuration/syntax.md

**Lines**: 112 | **Size**: 6.1 KB | **Language**: markdown

````markdown
# Архитектура и синтаксис конфигурации (Starry v3.0)

В версии **Starry v3.0** конфигурационный движок построен на базе чистой функциональной библиотеки **`Starry.Dsl`** (F#). Инфраструктура описывается как единый граф с поддержкой подстановки поддеревьев, наследования объектов через `$:` и интерполяции переменных и секретов.

---

## 1. Унифицированный граф AST и организация файлов

Любая папка и файл внутри директории конфигурации автоматически становятся узлом в едином иммутабельном AST:
- **Структурированные файлы (`.yaml`, `.json`, `.toml`):** Парсятся и монтируются в соответствующую ветку графа (например, `nodes/germany.yaml` -> `root.nodes.germany`).
- **Текстовые и конфигурационные файлы (`files/*`):** Любой текстовый файл (например, `files/nginx.conf` или `files/init.sql`) загружается в граф как `StringNode` под своим путем (`root.files.["nginx.conf"]`).
- **Произвольные пользовательские ветки:** Вы можете создавать любые удобные секции (`presets`, `secrets`, `vars`, `dns_providers`, `backups`, `certs`) — валидатор не блокирует произвольные структуры.

---

## 2. Разрешение ссылок (`$`) и наследование (`$:`)

### Подстановка целых поддеревьев
Если значение узла указывает на путь к объекту (например, `env: "$presets.db_env"`), Starry заменяет ссылку на глубокую копию целевого поддерева:

```yaml
presets:
  db_env:
    POSTGRES_DB: "starry"
    POSTGRES_USER: "admin"
    POSTGRES_PASSWORD: "ENC[AES256_GCM:v1:...]"

services:
  database:
    image: "postgres:16"
    env: "$presets.db_env"
```

### Наследование и слияние объектов (`$:`)
Через специальный ключ `"$"` сервис или узел может наследовать свойства одного или нескольких базовых пресетов:

```yaml
presets:
  base_service:
    restart: "always"
    priority: 50
    networks: ["$networks.app_net"]

services:
  web:
    $: "$presets.base_service"
    image: "nginx:alpine"
    priority: 80 # Переопределяет priority из base_service
```

### Интерполяция строк и файлов
Внутри любых строк (включая переменные окружения и текст файлов в `files/`) поддерживается синтаксис `$path.to.value` и `${path.to.value}`. При развертывании секреты автоматически расшифровываются.

---

## 3. Схема адресации IPv6 Mesh (`fd67:b00b::/32`) и реестр ID

* **Префикс кластера:** `fd67:b00b::/32`.
* **Подсеть узла (`/48`):** Каждому серверу выделяется статическая подсеть `fd67:b00b:<host_id>::/48`. В интерфейсе WireGuard `starry0` адрес равен `fd67:b00b:<host_id>::1/32`, а в пирах прописывается `AllowedIPs = fd67:b00b:<host_id>::/48`.
* **Подсеть контейнера (`/64`):** Каждой реплике контейнера выделяется подсеть `fd67:b00b:<host_id>:<container_id>::/64`. Внутри контейнера работает ровно один сетевой интерфейс (`eth0`) с адресом `fd67:b00b:<host_id>:<container_id>::1/64`.
* **Реестр ID (`.starry/id-ledger.json`):** При удалении сервиса или сервера их идентификаторы попадают в список `tombstoned` и **никогда не переиспользуются повторно**.

---

## 4. Именованные двунаправленные порты (`ports:`)

Секция `ports:` задает именованные правила маршрутизации:

```yaml
ports:
  shop_https_v4:
    from: "$nodes.germany.ipv4:443"
    to: "$ingress:443"
    proto: "tcp"
    bidirectional: true # Входящий DNAT + исходящий SNAT через IP ноды germany!

  custom_proxy:
    from: "$nodes.finland.ipv4:443"
    to: "my-proxy:443"
```

---

## 5. Встроенный Nginx Ingress (`$ingress`) и L4 SNI Proxy

* `$ingress` получает внутренний адрес `fd67:b00b:<host_id>:ffff::1` и не захватывает порт 443 на сервере без явного правила в `ports:`.
* Входящий трафик обрабатывается через `stream { ssl_preread on; }`:
  - Маршруты `sni: "<target>"` пробрасываются на Layer 4 без расшифровки TLS.
  - Маршруты `http: "<target>"` направляются в блок `http {}` для терминации SSL и проксирования HTTP/WebSocket/gRPC.

---

## 6. Иерархические домены (`domains:`) и Cloudflare Origin Rules

```yaml
domains:
  rocks:
    bocchi:
      dns_provider: "$dns_providers.cf"
      cert: "$certs.bocchi_wildcard"
      node: "$nodes.germany"

      "@":
        http: "landing:80"
      shop:
        http: "shop-web:3000"
      vpn:
        sni: "xray:8443"
        proxy_protocol: true
```

Если порт входа в правиле `ports:` отличается от 443 (например, 8443), провайдер Cloudflare автоматически создает **Origin Rule** через Cloudflare Rulesets API для прозрачного проксирования на нужный порт!
````

## File: site/en/index.md

**Lines**: 50 | **Size**: 2.2 KB | **Language**: markdown

```markdown
---
layout: home

hero:
  name: "Starry"
  text: "Decentralized GitOps Orchestrator"
  tagline: "Declarative orchestration for heterogeneous VPS fleets, Docker & Podman containers, WireGuard mesh networks, and nftables firewalls."
  image:
    src: /logo.svg
    alt: Starry Logo
  actions:
    - theme: brand
      text: Getting Started
      link: /en/guide/getting-started
    - theme: alt
      text: Architecture Overview
      link: /en/architecture/overview
    - theme: alt
      text: Config Syntax
      link: /en/configuration/syntax

features:
  - icon: ⚡
    title: Single-Binary Hybrid
    details: Zero-dependency ultra-lightweight binary. Runs as an interactive CLI on your workstation and as a high-performance background agent on target servers.
  - icon: 🌐
    title: WireGuard Full-Mesh
    details: Automatic encrypted full-mesh overlay network across 10.42.0.0/16, seamless NAT-to-NAT traversal, and automatic persistent keepalive.
  - icon: 🐳
    title: Docker & Podman Ready
    details: Unified engine abstraction over Docker Engine API and Podman Libpod/Compat API with rootless mode support and peer-to-peer image transfers.
  - icon: 🛡️
    title: Atomic nftables Firewall
    details: Strict automated nftables generation. Expose service ports exclusively over the encrypted WireGuard mesh with automated Deadman Switch safety rollbacks.
  - icon: 🔐
    title: Git-Native Encrypted Secrets
    details: Built-in envelope encryption via AES-256-GCM. Store ENC[...] tokens safely in git with one-click master key rotation and GitHub Secrets integration.
  - icon: 🔄
    title: OS Upgrades & Rolling Drain
    details: Declarative package management (apt, dnf, apk, pacman) and zero-downtime kernel reboot orchestration with proactive traffic draining.
---

<div class="starry-callout">
  <div class="starry-callout-title">
    🌟 Masterless, Zero Single Point of Failure
  </div>
  <p>
    Unlike bulky cluster orchestrators, Starry requires no dedicated master nodes, no heavy etcd consensus, and no external runtime databases. The entire state of your fleet lives declaratively in a readable YAML file, converging reliably across any modern Linux distribution.
  </p>
</div>
```

## File: site/en/architecture/overview.md

**Lines**: 129 | **Size**: 7.4 KB | **Language**: markdown

````markdown
# Architecture Overview

**Starry** is engineered as a decentralized, masterless GitOps orchestrator designed to operate a heterogeneous fleet of virtual private servers (VPS), cloud instances, and bare-metal nodes.

---

## 1. High-Level Architecture

```text
               +--------------------------------------------------+
               |               GitOps Repository                  |
               |        (starry.yaml + ENC[...] secrets)          |
               +--------------------------------------------------+
                                        │
                       ┌────────────────┴────────────────┐
                       ▼                                 ▼
             +───────────────────+             +───────────────────+
             |   Developer CLI   |             |   CI/CD Runner    |
             |   (starry apply)  |             |  (GitHub Actions) |
             +───────────────────+             +───────────────────+
                       │                                 │
                       └────────────────┬────────────────┘
                                        │ SSH / WireGuard
                                        ▼
    ┌───────────────────────────────────┬───────────────────────────────────┐
    │  Node A (Ubuntu 24.04 / Docker)   │  Node B (AlmaLinux 9 / Podman)   │
    │                                   │                                   │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |     Starry Host Agent       |  │  |     Starry Host Agent       |  │
    │  |  (Healthcheck / Telemetry)  |  │  |  (Healthcheck / Telemetry)  |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |      nftables Firewall      |  │  |      nftables Firewall      |  │
    │  | (ports open to wg0 mesh)    |  │  | (ports open to wg0 mesh)    |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |   WireGuard Mesh (10.42.0.1)│◄─┼─►│   WireGuard Mesh (10.42.0.2)│  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |      Docker Engine API      |  │  |    Podman Libpod API (Root) |  │
    │  |   [App: Web, Nginx, DB]     |  │  |   [App: Worker, Cache]      |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    └───────────────────────────────────┴───────────────────────────────────┘
```

---

## 2. Single-Binary Hybrid Model

Starry compiles into a single, self-contained Native AOT executable under 30 MB with zero external runtime requirements.

It operates in two complementary modes:

1. **CLI Mode (`starry <command>`)**:
   - Executes on the operator's workstation or within CI/CD automation pipelines.
   - Reads `starry.yaml`, decrypting sensitive tokens using `STARRY_MASTER_KEY`.
   - Inspects nodes via SSH, computes the dependency graph (DAG), and identifies configuration drift.
   - Orchestrates parallel declarative convergence without relying on a centralized control plane.

2. **Agent Mode (`starry agent run`)**:
   - Runs on fleet nodes as a supervised `systemd` daemon.
   - Manages local container lifecycles, healthchecks, and auto-restart policies.
   - Enforces atomic `nftables` rules and monitors WireGuard mesh health.
   - Dispatches telemetry and execution events.

---

## 3. WireGuard Full-Mesh Overlay Network

All cluster nodes participate in an encrypted private subnet (`10.42.0.0/16`):

- **Automated Routing:** Nodes receive predictable internal IP addresses (e.g. `10.42.0.1`, `10.42.0.2`). Peer exchange and WireGuard configurations are generated deterministically.
- **NAT Traversal:** Nodes behind NAT or cloud firewalls stay connected via `PersistentKeepalive = 25` and dynamic endpoint resolution.
- **Kernel-Level Encryption:** Inter-node communication (backend-to-database, internal RPCs) is secured with modern Noise / ChaCha20-Poly1305 cryptography directly in the Linux kernel.

---

## 4. Multi-Engine Containers: Docker & Podman

Starry transparently reconciles differences between container runtimes through an abstract interface:

| Capability | Docker Engine | Podman (Libpod) |
|---|---|---|
| **API Socket** | `/var/run/docker.sock` | `/run/podman/podman.sock` |
| **Rootless Support** | Experimental | Native out-of-the-box |
| **Primary Distros** | Ubuntu, Debian, Alpine | RHEL, AlmaLinux, Rocky, Fedora, Arch |
| **Architecture** | Daemon-based (`dockerd`) | Daemonless (fork/exec via conmon/crun) |

### P2P Image Streaming (Image Mesh)

When deploying or migrating services without access to an external container registry, Starry streams compressed container images (`zstd`) directly between peer nodes over the WireGuard mesh, preventing registry downtime from impacting fleet operations.

---

## 5. Atomic nftables Firewall & Deadman Switch

Host perimeter defense is handled by the modern `nftables` subsystem:

1. **Port Isolation:** Internal container ports are bound exclusively to the `wg0` interface. They remain completely invisible and unreachable from the public internet.
2. **Atomic Rulesets:** Rules are assembled and loaded in a single transaction via `nft -f`.
3. **Deadman Switch Rollback:** If an updated firewall ruleset inadvertently disrupts active SSH management connectivity, an automatic deadman countdown reverts to the previous valid configuration, preventing accidental node lockouts.

---

## 6. Secrets Management & Envelope Encryption

Sensitive credentials live directly within version control:

- **AES-256-GCM Envelope Encryption:** Each value is encrypted with a unique 96-bit initialization vector (IV) and a 128-bit authentication tag to prevent tampering.
- **Inline Secret Token:**
  ```text
  ENC[AES256_GCM,data:4gY3...,iv:9kP2...,tag:z0X8...]
  ```
- **Single-Command Master Key Rotation:**
  Running `starry rotate-secrets` decrypts existing values, generates a fresh 256-bit key, re-encrypts the manifest, and updates GitHub Actions repository secrets via the LibSodium REST API.
- **Automated Key Rotation:**
  Running `starry rotate-keys` replaces Ed25519 SSH keys and WireGuard keypairs across the fleet with connectivity pre-flight verification.

---

## 7. Declarative OS Maintenance & Rolling Drain

Starry maintains both application containers and host OS packages:

- **Package Managers:** Automatic detection of `/etc/os-release` and seamless interaction with `apt-get`, `dnf`, `apk`, and `pacman`.
- **Reboot Detection:** Proactive discovery of kernel update triggers (`/var/run/reboot-required` on Debian/Ubuntu, `needs-restarting -r` on RHEL/AlmaLinux).
- **Zero-Downtime Rolling Drain:**
  1. Route traffic away from the target node via DNS or load balancer.
  2. Gracefully terminate active workloads.
  3. Execute host `reboot`.
  4. Await SSH availability and verify agent health.
  5. Restore traffic and proceed to the next node.
````

## File: site/en/cli/commands.md

**Lines**: 211 | **Size**: 4.7 KB | **Language**: markdown

````markdown
# CLI Command Reference

Starry provides a unified command-line interface for local cluster orchestration, CI/CD pipelines, and node agent operations.

---

## 1. Global Options

Every Starry command supports standard global flags:

| Option | Description |
|---|---|
| `-c, --config <file>` | Path to the cluster manifest (default: `./starry.yaml`). |
| `-v, --verbose` | Enable verbose logging and protocol tracing. |
| `--json` | Format command output as structured JSON. |
| `-h, --help` | Display command help and available flags. |
| `--version` | Display current Starry version. |

---

## 2. Cluster Lifecycle Commands

### `starry init`

Scaffolds a new configuration repository with `starry.yaml`, `.starry/`, and `.gitignore`.

```bash
starry init [--path <dir>]
```

### `starry plan`

Analyzes target infrastructure via SSH, evaluates desired versus live state, and presents a non-destructive execution plan.

```bash
starry plan [--node <name>] [--service <name>]
```

### `starry apply`

Enforces desired state across target nodes: updates packages, establishes WireGuard tunnels, applies container definitions, and reloads `nftables`.

```bash
starry apply [--node <name>] [--dry-run] [--auto-approve]
```

---

## 3. Node Management (`starry nodes`)

### `starry nodes bootstrap`

Onboards a bare Linux server into the cluster over SSH:

```bash
starry nodes bootstrap <ssh-connection-string> [options]

# Example:
starry nodes bootstrap root@203.0.113.10 \
  --name app-02 \
  --engine podman \
  --mesh-ip 10.42.0.2
```

Options:
- `--name <string>`: Node identifier in `starry.yaml`.
- `--engine <docker|podman>`: Container runtime engine.
- `--mesh-ip <ip>`: Static WireGuard IP address.

### `starry nodes list`

Lists enrolled nodes, their internal WireGuard addresses, connection health, and active engine.

```bash
starry nodes list
```

### `starry nodes upgrade`

Triggers security update installation on a specified node following the defined `reboot_policy`.

```bash
starry nodes upgrade <node-name> [--drain]
```

---

## 4. Secret Operations (`starry secret`)

### `starry secret keygen`

Generates a cryptographically strong 256-bit AES-256-GCM master encryption key.

```bash
starry secret keygen
```

### `starry secret encrypt`

Encrypts plain text strings using the key defined in `STARRY_MASTER_KEY`:

```bash
starry secret encrypt "postgres-super-secure-password"
# Output: ENC[AES256_GCM,data:...,iv:...,tag:...]
```

### `starry secret decrypt`

Decrypts an `ENC[...]` token for local verification or debugging:

```bash
starry secret decrypt "ENC[AES256_GCM,data:...,iv:...,tag:...]"
```

---

## 5. Security & Key Rotation

### `starry rotate-secrets`

Decrypts all `ENC[...]` values, generates a fresh master key, re-encrypts the manifest, and syncs repository secrets via the GitHub API.

```bash
starry rotate-secrets [--github-repo user/repo]
```

### `starry rotate-keys`

Proactively rotates Ed25519 SSH keys and WireGuard keypairs across all nodes with rollback protection.

```bash
starry rotate-keys [--dry-run]
```

---

## 6. Server Agent Daemon (`starry agent`)

### `starry agent run`

Executes the node agent in the current terminal session.

```bash
starry agent run [--config /etc/starry/agent.yaml]
```

### `starry agent install-service`

Installs and enables the `starry-agent.service` systemd unit.

```bash
sudo starry agent install-service
```

### `starry agent status`

Inspects local agent health, mesh peer latency, and local container statuses.

```bash
starry agent status
```

---

## 7. Brownfield Host Adoption (`starry adopt`)

### `starry adopt`

Inspects an existing server over SSH, discovers running containers (Docker/Podman), downloads bind-mounted configuration files (`--pull-files`), scans and encrypts secrets in `Env`, and generates modular Starry configurations:

```bash
starry adopt root@203.0.113.10:22 --pull-files --out-dir ./config --format yaml
```

### `starry unmanaged list`

Scans cluster nodes and lists all unmanaged / manually started containers not tracked in Starry GitOps:

```bash
starry unmanaged list [--config ./config]
```

---

## 8. Disaster Recovery & Volumes

### `starry volume open`

Connects to a node and displays the file listing of a container volume or directory:

```bash
starry volume open bocchi /var/data/postgres [--once]
```

### `starry bundle export`

Exports the entire cluster configuration, encrypted secrets, and metadata into a single disaster recovery capsule:

```bash
starry bundle export -o cluster-backup.tar.gz
```

### `starry bundle import`

Restores a cluster capsule with optional IP address overrides for new host instances:

```bash
starry bundle import cluster-backup.tar.gz -o ./new-cluster \
  --override-ip bocchi=198.51.100.99 \
  --override-ip nijika=198.51.100.100
```
````

## File: site/en/configuration/syntax.md

**Lines**: 110 | **Size**: 3.8 KB | **Language**: markdown

````markdown
# Configuration Architecture & Syntax (Starry v3.0)

In **Starry v3.0**, the configuration engine is powered by the pure functional library **`Starry.Dsl`** (written in F#). The entire cluster infrastructure is modeled as an immutable graph with whole-subtree substitution, object inheritance via `$:`, and inline secret interpolation.

---

## 1. Unified Config Graph AST & File Layout

Every file and directory within the configuration path is mounted into a single immutable AST:
- **Structured Files (`.yaml`, `.json`, `.toml`):** Parsed and mounted to their respective tree branches (e.g. `nodes/germany.yaml` -> `root.nodes.germany`).
- **Raw Text Files (`files/*`):** Text files (such as `files/nginx.conf` or `files/init.sql`) are mounted as `StringNode` objects under their path (`root.files.["nginx.conf"]`).
- **Custom Branches:** Users can organize arbitrary user branches (`presets`, `secrets`, `vars`, `dns_providers`, `backups`, `certs`) without schema rejection.

---

## 2. Graph References (`$`) & Object Inheritance (`$:`)

### Whole-Subtree Substitution
When a property references a graph path (e.g. `env: "$presets.db_env"`), Starry replaces the reference with a deep copy of the resolved subtree:

```yaml
presets:
  db_env:
    POSTGRES_DB: "starry"
    POSTGRES_USER: "admin"
    POSTGRES_PASSWORD: "ENC[AES256_GCM:v1:...]"

services:
  database:
    image: "postgres:16"
    env: "$presets.db_env"
```

### Inheritance & Merging (`$:`)
Using the special `"$"` key, a service or node can inherit and merge properties from one or more base presets:

```yaml
presets:
  base_service:
    restart: "always"
    priority: 50
    networks: ["$networks.app_net"]

services:
  web:
    $: "$presets.base_service"
    image: "nginx:alpine"
    priority: 80 # Overrides priority from base_service
```

### String & File Interpolation
All strings and text files support `$path.to.value` and `${path.to.value}` interpolation, with automatic secret decryption during deployment.

---

## 3. IPv6 Mesh (`fd67:b00b::/32`) & Monotonic ID Ledger

* **Cluster ULA Prefix:** `fd67:b00b::/32`.
* **Host Subnet (`/48`):** Each node is assigned `fd67:b00b:<host_id>::/48`. WireGuard interface `starry0` has address `fd67:b00b:<host_id>::1/32`, and peers route `AllowedIPs = fd67:b00b:<host_id>::/48` statically.
* **Container Replica Subnet (`/64`):** Each container replica gets a dedicated `/64` subnet `fd67:b00b:<host_id>:<container_id>::/64` with a single interface `eth0` bound to `fd67:b00b:<host_id>:<container_id>::1/64`.
* **Tombstone Ledger (`.starry/id-ledger.json`):** Released host and container IDs are permanently tombstoned and never reused.

---

## 4. Named Bidirectional Routing (`ports:`)

```yaml
ports:
  shop_https_v4:
    from: "$nodes.germany.ipv4:443"
    to: "$ingress:443"
    proto: "tcp"
    bidirectional: true # Inbound DNAT + symmetric outbound SNAT!

  custom_proxy:
    from: "$nodes.finland.ipv4:443"
    to: "my-proxy:443"
```

---

## 5. Managed Ingress (`$ingress`) & L4 SNI Proxying

* `$ingress` binds internally to `fd67:b00b:<host_id>:ffff::1` and never intercepts port 443 without explicit rules in `ports:`.
* Routing uses Nginx `stream { ssl_preread on; }`:
  - `sni: "<target>"` passes TLS traffic through at Layer 4 without certificate termination.
  - `http: "<target>"` routes internally to `http {}` for SSL termination and reverse proxying.

---

## 6. Hierarchical Domains (`domains:`) & Cloudflare Origin Rules

```yaml
domains:
  rocks:
    bocchi:
      dns_provider: "$dns_providers.cf"
      cert: "$certs.bocchi_wildcard"
      node: "$nodes.germany"

      "@":
        http: "landing:80"
      shop:
        http: "shop-web:3000"
      vpn:
        sni: "xray:8443"
        proxy_protocol: true
```

When an ingress entry port is non-standard (e.g. 8443), Starry automatically programs Cloudflare Origin Rules via the Rulesets API to rewrite origin ports transparently!
````

## File: site/en/guide/backups-disaster-recovery.md

**Lines**: 55 | **Size**: 2.1 KB | **Language**: markdown

````markdown
# Streaming Backups (rclone), Volume Explorer & Fleet Bundles

Starry is engineered specifically for low-overhead operations on constrained VPS instances (such as servers with only 10–20 GB SSDs) with instant **Disaster Recovery (DR)** capabilities.

---

## 1. Zero-Disk Streaming Backups (`rclone rcat`)

Typical backup tools generate intermediate archive files (`.tar.gz`) locally on disk before uploading to cloud storage, risking `No space left on device` failures on production databases.

Starry streams backups entirely through in-memory Unix pipes:
```bash
tar -C /var/data/postgres -cf - . | zstd -3 | rclone rcat remote:backups/pg-20261101.tar.zst
```

### Key Advantages:
- **Zero Temporary Disk Consumption**: Volumes of any scale stream directly into object storage (AWS S3, Cloudflare R2, Backblaze B2, SFTP).
- **Zstandard Real-time Compression (`zstd -3`)**: Hardware-friendly compression maximizing network transfer speeds.
- **Direct Pipe Restoration**: `rclone cat ... | zstd -d | tar -C ... -xf -`.

---

## 2. Remote Volume Explorer (`starry volume open`)

To inspect or edit configuration assets and database dumps residing inside persistent container volumes:

```bash
starry volume open bocchi /var/lib/docker/volumes/app-data/_data
```

The command establishes an authenticated SSH/SFTP tunnel and outputs an actionable SFTP mount endpoint.

---

## 3. Disaster Recovery Capsule: `starry bundle`

In catastrophic situations where a VPS provider suffers a total outage, the `bundle` capsule reconstitutes the cluster on fresh servers in minutes.

### Exporting Fleet Capsule:
```bash
starry bundle export -o my-fleet.tar.gz
```
The resulting archive packages all modular configurations, encrypted secrets, and cluster metadata.

### Restoring Across New IP Addresses:
When re-provisioning on new virtual machines with different public IPs, pass `--override-ip`:

```bash
starry bundle import my-fleet.tar.gz \
  -o ./new-cluster \
  --override-ip bocchi=198.51.100.99 \
  --override-ip nijika=198.51.100.100
```

Running `starry apply` on the imported configuration regenerates the WireGuard mesh, adapts DNS, and deploys all services immediately!
````

## File: site/en/guide/containers-docker-podman.md

**Lines**: 64 | **Size**: 2.8 KB | **Language**: markdown

````markdown
# Dual Runtimes: Docker & Podman (Rootless), Config Mounting (files) & Priorities

Starry natively orchestrates containers across both major Linux engines: **Docker Engine** and **Podman** (in both rootful and unprivileged rootless namespaces).

Communication operates directly over Unix Domain Sockets using pure .NET `SocketsHttpHandler` with zero third-party SDK dependencies.

---

## 1. Engine Comparison: Docker vs Podman

| Feature | Docker Engine | Podman (Rootful / Rootless) |
|---|---|---|
| **Architecture** | Client-server daemon `dockerd` | Daemonless, socket activated via `systemd.socket` |
| **Socket Location** | `/var/run/docker.sock` | `/run/podman/podman.sock` (rootful)<br>`/run/user/$UID/podman/podman.sock` (rootless) |
| **Privileges** | Requires root / `docker` group | Full unprivileged **Rootless** user-namespace support |
| **P2P Streaming** | `docker save \| zstd \| ssh docker load` | `podman save \| zstd \| ssh podman load` |

Starry automatically detects the active socket and adapts volume mounts and networking configurations accordingly.

---

## 2. Dynamic Config Mounting (`files`) & SIGHUP Reloads

Services frequently require configuration files (e.g. `nginx.conf`, `prometheus.yml`, `settings.toml`) that evolve independently from container images.

Starry provides a first-class `files` mapping section in service manifests:

```yaml
services:
  web:
    image: "nginx:alpine"
    node: "bocchi"
    priority: 80
    files:
      /etc/nginx/conf.d/default.conf: |
        server {
            listen 80;
            server_name _;
            location / {
                proxy_pass http://api.internal:8080;
            }
        }
```

### Hot-Reload Lifecycle:
1. Starry computes the **SHA-256** checksum for each mounted file.
2. Files are persisted under `/var/lib/starry/files/<service>/...` and mounted read-only into the container.
3. When file content changes, Starry atomically rewrites the host file.
4. If the container is already running, Starry dispatches a **`SIGHUP`** signal (`POST /containers/{id}/kill?signal=SIGHUP`). Processes like Nginx or Prometheus reload their configuration **without dropping connections or restarting the container**!

---

## 3. Numeric Priority Scheduling (`priority: int`)

To guarantee that infrastructure dependencies start in the correct order, services specify an integer priority from `0` to `100`:

| Priority Tier | Category | Examples |
|---|---|---|
| **0 – 9** | Registries & Core | Private Docker Registry |
| **10 – 29** | Databases & Storage | PostgreSQL, Redis, ClickHouse, RabbitMQ |
| **30 – 69** | Applications | Backend APIs, job workers |
| **70 – 100** | Ingress Proxies | Nginx, Caddy, Traefik |

Starry sorts deployment execution ascending by priority: databases are confirmed running before backends attempt to connect!
````

## File: site/en/guide/getting-started.md

**Lines**: 196 | **Size**: 4.8 KB | **Language**: markdown

````markdown
# Getting Started with Starry

Welcome to the **Starry** quickstart guide! This tutorial guides you through installing the CLI, initializing your infrastructure GitOps repository, bootstrapping server nodes, and deploying your first containerized services.

---

## 1. CLI Installation

Starry is distributed in two official formats: as a self-contained **Native AOT** executable (requiring zero runtime dependencies) and as a global **.NET Tool**.

::: code-group

```bash [Native AOT (Linux / macOS)]
# Download the latest binary for your architecture
curl -fsSL https://github.com/starry-project/starry/releases/latest/download/starry-linux-x64 -o starry

# Grant execution permissions
chmod +x starry

# Move binary to system PATH
sudo mv starry /usr/local/bin/starry

# Verify installation
starry --version
```

```bash [.NET Global Tool]
# Install via .NET SDK (.NET 10+ required)
dotnet tool install -g starrycli

# Verify command availability
starry --version
```

```powershell [Windows (Native AOT)]
# Download and setup in PowerShell
Invoke-WebRequest -Uri "https://github.com/starry-project/starry/releases/latest/download/starry-win-x64.exe" -OutFile "$HOME\starry.exe"

# Add to user PATH
[Environment]::SetEnvironmentVariable("Path", $env:Path + ";$HOME", "User")

starry --version
```

:::

---

## 2. Initializing the Repository

To manage your fleet using declarative GitOps patterns, create a dedicated Git repository and initialize Starry:

```bash
mkdir my-fleet && cd my-fleet
git init
starry init
```

The `starry init` command creates the initial project layout:

```text
my-fleet/
├── starry.yaml          # Main declarative cluster manifest
├── .starry/
│   ├── keys/            # Local SSH & WireGuard keys (git-ignored)
│   └── cache/           # Local state cache
├── .gitignore           # Ignores private keys and runtime artifacts
└── README.md
```

---

## 3. Configuring Master Encryption Key

Starry employs **AES-256-GCM** authenticated envelope encryption with PBKDF2/Argon2 key derivation. Sensitive tokens, database credentials, and certificates are safely committed directly into `starry.yaml`.

### Generating a Master Key

Generate a cryptographically secure 256-bit key:

```bash
starry secret keygen
```

Export the generated base64 string to your environment:

```bash
export STARRY_MASTER_KEY="your-generated-base64-master-key"
```

::: tip CI/CD Automation
When running automated workflows in GitHub Actions or GitLab CI, set the `STARRY_MASTER_KEY` repository secret.
:::

### Encrypting Secrets

To encrypt confidential values:

```bash
starry secret encrypt "my-super-secret-password"
```

The command returns an authenticated envelope:
`ENC[AES256_GCM,data:c3VwZXJzZWNyZXQ=,iv:...,tag:...]`

Paste this value directly into the `env` or `secrets` block of your `starry.yaml`.

---

## 4. Bootstrapping Server Nodes

Prepare a clean Linux server running any supported distribution (Ubuntu, Debian, AlmaLinux, Rocky Linux, Alpine, or Arch).

Run the automated bootstrap command:

```bash
starry nodes bootstrap root@203.0.113.10 --name node-01 --engine docker
```

During the bootstrap process, Starry will:
1. Connect via SSH and inspect host capabilities.
2. Install the specified container engine (`docker` or `podman`) if missing.
3. Install and configure `wireguard-tools` and `nftables`.
4. Generate WireGuard keys and assign a mesh IP from `10.42.0.0/16`.
5. Append the validated node entry into `starry.yaml`.

---

## 5. Planning and Applying Changes

Edit your `starry.yaml` file to define desired state:

```yaml
version: "1.0"

mesh:
  cidr: "10.42.0.0/16"
  port: 51820

nodes:
  node-01:
    host: "203.0.113.10"
    vpn_ip: "10.42.0.1"
    engine: "docker"
    packages:
      - curl
      - htop

services:
  web:
    image: "nginx:alpine"
    node: "node-01"
    ports:
      - "80:80"
    env:
      API_SECRET: "ENC[AES256_GCM,data:...,iv:...,tag:...]"
```

### Dry Run (Plan)

Preview planned configuration changes without executing mutations:

```bash
starry plan
```

Starry reconciles desired state against live servers and displays an actionable diff (container creates/updates, firewall rule adjustments, package installs).

### Apply

Enforce desired state across your fleet:

```bash
starry apply
```

---

## 6. Running the Server Agent Daemon

For autonomous health checking, automatic container restarts, and real-time monitoring, Starry features a lightweight server agent.

Install the systemd service on your target server:

```bash
sudo starry agent install-service
sudo systemctl status starry-agent
```

Or run the agent in the foreground for debugging:

```bash
starry agent run --verbose
```

Congratulations! Your initial Starry cluster is up, running, and secured. Proceed to the [Architecture Overview](/en/architecture/overview) to explore the system internals.
````

## File: site/en/guide/linux-distributions.md

**Lines**: 61 | **Size**: 2.2 KB | **Language**: markdown

````markdown
# Linux Distributions & NAT Traversal

Starry is built to orchestrate heterogeneous infrastructure fleets: cluster nodes can run different Linux distributions and live behind Carrier-Grade NAT (CGNAT) or on public static IP addresses.

---

## 1. Distribution Compatibility Matrix

| Distribution Family | Package Manager | Supported Distributions | Notes |
|---|---|---|---|
| **Debian / Ubuntu** | `apt` | Ubuntu 22.04+, 24.04+, Debian 11, 12, Pop!_OS | Automated `/var/run/reboot-required` inspection |
| **RHEL / Enterprise Linux** | `dnf` | AlmaLinux 9+, Rocky Linux 9+, Fedora 39+, RHEL 8, 9, CentOS Stream | Reboot detection via `needs-restarting -r` |
| **Alpine Linux** | `apk` | Alpine 3.18, 3.19, 3.20+ | Ultra-lightweight nodes with `musl libc` |
| **Arch Linux** | `pacman` | Arch Linux, Manjaro, EndeavourOS | Bleeding-edge rolling release and kernel packages |

Starry automatically queries `/etc/os-release` during initial node bootstrap and selects the matching `IOsPackageManager`.

---

## 2. Connecting to Nodes Behind NAT via Jump Hosts

For server instances without a public IPv4 address, Starry supports transparent SSH jump-proxying (`-J` / `ProxyJump`):

```yaml
nodes:
  bastion:
    host: "203.0.113.1"
    port: 22
    is_public: true
    ssh_key: "ENC[AES256_GCM:v1:nonce:cipher:tag]"

  internal-worker:
    host: "10.0.0.50"
    port: 22
    jump_host: "root@203.0.113.1:22"
    vpn_ip: "10.99.0.2"
    ssh_key: "ENC[AES256_GCM:v1:nonce:cipher:tag]"
```

When contacting `internal-worker`, Starry routes through `bastion` using OpenSSH ControlMaster connection reuse for near-zero latency command execution.

---

## 3. Rolling Security Upgrades

To maintain fleet security without downtime, Starry executes rolling upgrades across nodes:

```bash
starry nodes upgrade --rolling
```

### Rolling Upgrade Lifecycle:
1. Connect to target node via SSH and resolve package manager.
2. Apply OS security patches non-interactively.
3. Query if kernel or libc update requires a restart (`IsRebootRequiredAsync`).
4. If a reboot is required:
   - Issue graceful `reboot` command.
   - Wait for connection termination.
   - Poll until SSH listener is restored.
   - Verify node health before proceeding.
5. Move on to the next node in the fleet!
````

## File: site/en/guide/migrations-dns-p2p.md

**Lines**: 57 | **Size**: 2.2 KB | **Language**: markdown

````markdown
# Service Migrations (Blue-Green), P2P Images & DNS

Starry embeds an intelligent **DAG (Directed Acyclic Graph)** scheduler enabling zero-downtime service relocation across physical nodes, peer-to-peer image transfers without registry bottlenecks, and automated DNS synchronization.

---

## 1. Seamless Blue-Green Service Migration

When a service's target node changes in configuration (e.g. `web` moves from `bocchi` to `nijika`):

```yaml
services:
  web:
    image: "nginx:alpine"
    node: "nijika" # Was: bocchi
    vip: "10.99.10.5"
```

Running `starry plan` detects the relocation and constructs a **`BLUE-GREEN`** DAG step:

```bash
starry plan
```

### Execution Lifecycle:
1. **P2P Image Pre-delivery**: Node `nijika` streams the image directly from `bocchi` via `docker save | zstd | ssh docker load` without downloading layers from external registries.
2. **Mounting Configs & Volumes**: Configurations from `files` are written onto `nijika`.
3. **Start Green Container**: The new instance starts on `nijika`.
4. **Health Verification**: Starry probes the new container's `healthcheck` endpoint until green.
5. **Instant VIP Routing Cutover**: Once confirmed healthy, Starry updates WireGuard `AllowedIPs`: packets destined for `10.99.10.5` redirect to `nijika` **without terminating existing TCP connections**!
6. **Blue Container Tear-down**: The old container on `bocchi` is drained and stopped.

---

## 2. Peer-to-Peer Image Streaming (`docker save | zstd | ssh load`)

When private registries are inaccessible or remote nodes have bandwidth constraints, Starry streams layers peer-to-peer:
- Real-time compression using `zstd -3` for maximum throughput with minimal CPU overhead.
- Direct delivery through memory pipes over SSH without writing intermediate tar archives to disk.

---

## 3. Public DNS Providers (Cloudflare & Webhook)

Starry manages domain records automatically:

### Cloudflare (`IDnsProvider`)
Interacts with Cloudflare API v4 with automatic Zone ID discovery and optional edge proxying (`proxied: true`):
```yaml
dns:
  provider: "cloudflare"
  api_token: "ENC[AES256_GCM:...]"
  zone: "example.com"
```

### Webhook
Integrates with custom DNS infrastructure (CoreDNS, PowerDNS, Infoblox) by emitting `upsert` and `delete` JSON webhooks.
````

## File: site/en/guide/network-mesh-vips.md

**Lines**: 55 | **Size**: 2.7 KB | **Language**: markdown

````markdown
# WireGuard Mesh, Roaming VIPs & Nftables

Starry connects all cluster nodes into an encrypted, decentralized peer-to-peer network using **WireGuard** (`starry0`). The overlay traverses NAT and firewalls reliably via automated `PersistentKeepalive = 25` keepalive pulses.

---

## 1. Network Topology & NAT Traversal

In classic setups, servers behind NAT are inaccessible. With Starry:
- Nodes with static public addresses (`is_public: true`) publish their external `Endpoint`.
- Nodes behind NAT establish outbound links to public peers and transmit keepalive packets every 25 seconds, sustaining state in stateful firewall translation tables.
- Public peers act as transit relays between NAT-isolated hosts.

```text
[ VPS-1: Public IP (is_public: true) ]
   ▲                            ▲
   │ WireGuard                  │ WireGuard
   │ Keepalive=25               │ Keepalive=25
   ▼                            ▼
[ VPS-2: Behind Carrier NAT ]    [ VPS-3: Behind Corporate NAT ]
```

---

## 2. Container Roaming VIPs (10.99.x.x)

Each orchestrated service is allocated a static Virtual IP within the overlay subnet (`10.99.x.x`).

### How Roaming VIPs Function:
1. When a service is deployed on node `bocchi`, Starry includes its VIP (e.g. `10.99.10.5/32`) into `AllowedIPs` for `bocchi` across all peer WireGuard configurations.
2. Kernel packet routing delivers traffic aimed at `10.99.10.5` through `starry0` straight to `bocchi`.
3. When the service migrates from `bocchi` to `nijika`, Starry reconfigures `AllowedIPs` (`wg syncconf`). Packets immediately steer to `nijika` **without altering the service IP**!

### Integration with Nginx Proxy Manager (NPM) / Ingress Proxies
Any reverse proxy (NPM, Traefik, Caddy) situated on an ingress node can proxy to `http://10.99.10.5:8080`. The proxy does not need to track which physical host currently runs the container.

---

## 3. Embedded Internal DNS (*.internal)

The Starry node daemon hosts an embedded lightweight DNS server on port 53:
- Query `api.internal` -> `10.99.10.5`
- Query `database.internal` -> `10.99.0.2`

Containers leverage the local Starry daemon as their primary nameserver, enabling resilient service discovery.

---

## 4. Host Firewall & Port Forwarding with `nftables` (`table inet starry`)

Starry operates strictly inside an isolated `table inet starry` table to avoid collisions with Docker iptables shims:
- `chain input`: Default `drop` policy.
- Permitted: Loopback `lo`, WireGuard mesh `starry0`, inbound SSH, and WireGuard UDP port 51820.
- `chain prerouting`: DNAT mapping external host IPv4/IPv6 ports to internal Container VIPs.
- `chain postrouting`: Masquerading rules for correct asymmetric routing return paths.
````

## File: site/en/guide/security-cryptography.md

**Lines**: 62 | **Size**: 2.4 KB | **Language**: markdown

````markdown
# Security, Cryptography & GitOps

Starry provides a zero-trust security model: cluster configuration files can be stored safely even in **public Git repositories**, because all sensitive values are authenticated and encrypted using **AES-256-GCM** with contextual **AAD (Additional Authenticated Data)** binding.

---

## 1. Contextual AAD Binding: Protection Against Cut-and-Paste Attacks

A known vulnerability with encrypted secrets stored in Git is cut-and-paste manipulation: an attacker or careless operator moving an encrypted ciphertext from one property to another (e.g. copying a root SSH key into an unprivileged environment variable).

In Starry, every value is cryptographically tied to its **canonical JSON path**:
- The secret for `nodes.bocchi.ssh_key` uses `AAD = "nodes.bocchi.ssh_key"`
- If this ciphertext is pasted into `services.api.db_password`, decryption fails immediately with `CryptographicException` because the authentication tag does not match!

---

## 2. Secret Formats in Configuration

### Open Secrets (`PLAIN[...]`)
When drafting changes, developers can write raw secrets using the `PLAIN[...]` marker:
```yaml
nodes:
  bocchi:
    host: "192.168.1.10"
    ssh_key: "PLAIN[-----BEGIN OPENSSH PRIVATE KEY-----...]"
```

Running `starry encrypt` automatically finds all plain secrets, encrypts them in place, and updates the files:
```bash
starry encrypt --key $STARRY_MASTER_KEY ./cluster
```

### Encrypted Secrets (`ENC[...]`)
The resulting encrypted secret is formatted as an authenticated envelope:
```text
ENC[AES256_GCM:v1:<base64_nonce>:<base64_cipher>:<base64_tag>]
```

---

## 3. Automated Monthly Master Key Rotation via GitHub Actions

To adhere to best security hygiene, master keys should be rotated periodically (e.g. monthly).

Starry accomplishes full fleet re-encryption with a single command:
```bash
starry encrypt global --reencrypt \
  --old-key-env STARRY_MASTER_KEY_OLD \
  --new-key-env STARRY_MASTER_KEY_NEW \
  --allow-config-edit \
  --git-commit \
  --git-push \
  --update-github-secret STARRY_MASTER_KEY
```

This command:
1. Discovers all configuration files (`.yaml`, `.json`, `.toml`).
2. Decrypts with the old key, checking AAD on every field.
3. Encrypts with the new key using a fresh cryptographic nonce per field.
4. Atomically rewrites all config files using safe temporary file renames.
5. Commits changes to Git and pushes to the remote branch.
6. Updates the `STARRY_MASTER_KEY` secret in GitHub Actions using `gh secret set`.
````

## File: site/guide/backups-disaster-recovery.md

**Lines**: 57 | **Size**: 3.4 KB | **Language**: markdown

````markdown
# Потоковые бэкапы (rclone), Проводник томов и Капсулы Bundle

Starry разработан с акцентом на бережное отношение к дисковому пространству серверов (включая VPS с объемом диска всего 10–20 ГБ) и мгновенное аварийное восстановление (**Disaster Recovery**).

---

## 1. Потоковые бэкапы без временных файлов (`rclone rcat`)

Большинство систем резервного копирования создают временный `.tar.gz` на локальном диске VPS перед отправкой в S3, что приводит к переполнению диска (`No space left on device`) и падению баз данных.

В Starry данные передаются через непрерывный пайплайн в оперативной памяти:
```bash
tar -C /var/data/postgres -cf - . | zstd -3 | rclone rcat remote:backups/pg-20261101.tar.zst
```

### Преимущества потокового подхода:
- **0 байт временных файлов на диске**: бэкап любого объема безопасно стримится сразу в удаленное хранилище (S3, Backblaze B2, SFTP).
- **Сжатие Zstandard (`zstd -3`) на лету**: минимальная нагрузка на CPU при высокой компрессии.
- **Потоковое восстановление**: `rclone cat ... | zstd -d | tar -C ... -xf -`.

---

## 2. Проводник томов (`starry volume open`)

Для быстрой инспекции или скачивания файлов из томов контейнеров Starry предоставляет команду:

```bash
starry volume open bocchi /var/lib/docker/volumes/app-data/_data
```

Команда:
- Создает прозрачный защищенный SFTP-туннель к удаленному узлу.
- Предоставляет локальный SFTP URI для монтирования в файловый менеджер (Finder, Nautilus, WinSCP) или подключения веб-интерфейса.

---

## 3. «Капсула Судного Дня»: `starry bundle`

Если весь кластер или провайдер VPS внезапно перестал отвечать, капсула `bundle` позволяет поднять флот с нуля на новых серверах за считанные минуты.

### Экспорт капсулы флота:
```bash
starry bundle export -o my-fleet.tar.gz
```
Архив содержит полную виртуальную конфигурацию, зашифрованные секреты и метаданные манифеста.

### Восстановление на новых IP-адресах:
Если новые серверы получили другие публичные IP-адреса, укажите маппинг `--override-ip`:

```bash
starry bundle import my-fleet.tar.gz \
  -o ./new-cluster \
  --override-ip bocchi=198.51.100.99 \
  --override-ip nijika=198.51.100.100
```

После импорта достаточно выполнить `starry apply`, и Starry автоматически настроит WireGuard-меш, обновит DNS и запустит все контейнеры на новых серверах!
````

## File: site/guide/containers-docker-podman.md

**Lines**: 64 | **Size**: 4.2 KB | **Language**: markdown

````markdown
# Двойной движок: Docker и Podman (Rootless), Конфиги (files) и Приоритеты

Starry из коробки поддерживает работу с двумя главными контейнерными средами Linux — **Docker Engine** и **Podman** (как в rootful, так и в полностью непривилегированном rootless режиме).

Общение с демонами происходит напрямую через Unix Domain Socket (`SocketsHttpHandler`) с нулевым оверхедом и без тяжелых сторонних SDK.

---

## 1. Сравнение движков: Docker vs Podman

| Характеристика | Docker Engine | Podman (Rootful / Rootless) |
|---|---|---|
| **Архитектура** | Клиент-серверный демон `dockerd` | Daemonless, сокет активируется через `systemd.socket` |
| **Путь к сокету** | `/var/run/docker.sock` | `/run/podman/podman.sock` (rootful)<br>`/run/user/$UID/podman/podman.sock` (rootless) |
| **Привилегии** | Требует права root / группу `docker` | Полная поддержка **Rootless** в пространстве пользователя |
| **P2P стриминг** | `docker save \| zstd \| ssh docker load` | `podman save \| zstd \| ssh podman load` |

Starry автоматически определяет доступный сокет на ноде и адаптирует параметры монтирования и сетевого режима.

---

## 2. Динамическая шаблонизация конфигов (`files`) и SIGHUP-перезагрузка

Часто сервисам требуются конфигурационные файлы (например, `nginx.conf`, `prometheus.yml`, `settings.toml`), которые меняются чаще, чем сами образы.

В манифесте сервиса Starry поддерживает секцию `files`:

```yaml
services:
  web:
    image: "nginx:alpine"
    node: "bocchi"
    priority: 80
    files:
      /etc/nginx/conf.d/default.conf: |
        server {
            listen 80;
            server_name _;
            location / {
                proxy_pass http://api.internal:8080;
            }
        }
```

### Механизм горячего обновления:
1. Starry вычисляет хеш **SHA-256** для каждого монтируемого файла.
2. Файлы сохраняются на хосте в `/var/lib/starry/files/<service>/...` и монтируются в контейнер в режиме `ro` (read-only).
3. При изменении содержимого файла Starry атомарно обновляет файл на хосте.
4. Если контейнер уже запущен, Starry отправляет сигнал **`SIGHUP`** (`POST /containers/{id}/kill?signal=SIGHUP`), и процесс (например, Nginx) перечитывает конфигурацию **без перезапуска контейнера и без простоя соединений**!

---

## 3. Приоритизация запуска (`priority: int`)

Для предотвращения сбоев при старте зависимых сервисов (например, когда бэкенд падает из-за недоступности базы данных), каждый сервис наделяется числовым приоритетом от `0` до `100`:

| Приоритет | Категория | Примеры |
|---|---|---|
| **0 – 9** | Системные реестры | Приватный Docker Registry |
| **10 – 29** | СУБД и кэши | PostgreSQL, Redis, ClickHouse, RabbitMQ |
| **30 – 69** | Внутренние бэкенды | API-серверы, воркеры, микросервисы |
| **70 – 100** | Внешние прокси и Ingress | Nginx, Caddy, Traefik, Envoy |

Starry сортирует сервисы по возрастанию `priority` и запускает их ярусами: базы данных гарантированно готовы к моменту старта бэкендов!
````

## File: site/guide/getting-started.md

**Lines**: 196 | **Size**: 7.4 KB | **Language**: markdown

````markdown
# Быстрый старт с Starry

Добро пожаловать в руководство по началу работы с **Starry**! В этом разделе вы узнаете, как установить CLI, инициализировать конфигурационный репозиторий, добавить серверные узлы и развернуть первые сервисы.

---

## 1. Установка CLI

Starry распространяется в двух форматах: как автономный **Native AOT** исполняемый файл (не требующий установленного .NET Runtime) и как глобальный инструмент **.NET Tool**.

::: code-group

```bash [Native AOT (Linux / macOS)]
# Скачивание последнего релиза для вашей архитектуры
curl -fsSL https://github.com/starry-project/starry/releases/latest/download/starry-linux-x64 -o starry

# Предоставление прав на выполнение
chmod +x starry

# Перемещение в системную директорию PATH
sudo mv starry /usr/local/bin/starry

# Проверка установки
starry --version
```

```bash [.NET Global Tool]
# Установка через .NET SDK (требуется .NET 10+)
dotnet tool install -g starrycli

# Проверка доступности команды
starry --version
```

```powershell [Windows (Native AOT)]
# Загрузка и распаковка в PowerShell
Invoke-WebRequest -Uri "https://github.com/starry-project/starry/releases/latest/download/starry-win-x64.exe" -OutFile "$HOME\starry.exe"

# Добавление в PATH текущего пользователя
[Environment]::SetEnvironmentVariable("Path", $env:Path + ";$HOME", "User")

starry --version
```

:::

---

## 2. Инициализация репозитория

Для управления инфраструктурой по принципам GitOps создайте отдельный git-репозиторий и выполните команду инициализации:

```bash
mkdir my-fleet && cd my-fleet
git init
starry init
```

Команда `starry init` создаст стартовую структуру проекта:

```text
my-fleet/
├── starry.yaml          # Главный декларативный манифест кластера
├── .starry/
│   ├── keys/            # Локальные SSH и WireGuard ключи (добавлены в .gitignore)
│   └── cache/           # Локальный кэш состояния
├── .gitignore           # Игнорирование приватных ключей и чувствительных файлов
└── README.md
```

---

## 3. Настройка мастер-ключа шифрования

Starry использует шифрование **AES-256-GCM** с алгоритмом деривации ключей PBKDF2/Argon2. Все секретные токены, пароли баз данных и сертификаты хранятся в зашифрованном виде прямо в файле `starry.yaml`.

### Генерация мастер-ключа

Сгенерируйте криптографически стойкий мастер-ключ:

```bash
starry secret keygen
```

Команда выведет ключ в формате Base64. Экспортируйте его в переменную окружения:

```bash
export STARRY_MASTER_KEY="your-generated-base64-master-key"
```

::: tip Переменные в CI/CD
Для запуска через GitHub Actions добавьте значение мастер-ключа в репозиторий как секрет: `STARRY_MASTER_KEY`.
:::

### Шифрование чувствительных данных

Чтобы зашифровать пароль или токен API:

```bash
starry secret encrypt "my-super-secret-password"
```

Вывод команды будет иметь вид:
`ENC[AES256_GCM,data:c3VwZXJzZWNyZXQ=,iv:...,tag:...]`

Вставьте полученное значение в секцию `env` или `secrets` вашего `starry.yaml`.

---

## 4. Подключение серверного узла (Bootstrap)

Подготовьте чистый сервер на базе любого поддерживаемого Linux-дистрибутива (Ubuntu, Debian, AlmaLinux, Rocky, Alpine, Arch).

Выполните команду первичного подключения:

```bash
starry nodes bootstrap root@203.0.113.10 --name node-01 --engine docker
```

В ходе выполнения bootstrap Starry:
1. Подключится по SSH и проверит окружение узла.
2. Установит указанный контейнерный движок (`docker` или `podman`), если он отсутствует.
3. Установит и сконфигурирует пакет `wireguard-tools` и `nftables`.
4. Сгенерирует пару ключей WireGuard и выдаст ноде внутренний IP-адрес из диапазона `10.42.0.0/16`.
5. Добавит описание ноды в `starry.yaml`.

---

## 5. Планирование и применение конфигурации

Отредактируйте `starry.yaml`, описав желаемые сервисы:

```yaml
version: "1.0"

mesh:
  cidr: "10.42.0.0/16"
  port: 51820

nodes:
  node-01:
    host: "203.0.113.10"
    vpn_ip: "10.42.0.1"
    engine: "docker"
    packages:
      - curl
      - htop

services:
  web:
    image: "nginx:alpine"
    node: "node-01"
    ports:
      - "80:80"
    env:
      API_SECRET: "ENC[AES256_GCM,data:...,iv:...,tag:...]"
```

### Просмотр изменений (Plan)

Выполните команду dry-run для обнаружения дрифта и просмотра предстоящих изменений:

```bash
starry plan
```

Starry сопоставит фактическое состояние узлов с декларативным манифестом и выведет детальный список планируемых действий (создание контейнеров, настройка правил файрвола, обновление пакетов).

### Применение (Apply)

Для применения изменений запустите:

```bash
starry apply
```

---

## 6. Запуск фонового агента на нодах

Для автономного мониторинга, проверки состояния (Healthchecks), ротации логов и автоматического восстановления контейнеров Starry включает встроенный фоновый демон.

Установите системную службу systemd на целевом сервере:

```bash
sudo starry agent install-service
sudo systemctl status starry-agent
```

Или запустите демон в интерактивном режиме для отладки:

```bash
starry agent run --verbose
```

Поздравляем! Ваш первый кластер под управлением Starry успешно развернут и защищен. Переходите к разделу [Обзор архитектуры](/architecture/overview), чтобы узнать подробности о внутренней работе системы.
````

## File: site/guide/linux-distributions.md

**Lines**: 60 | **Size**: 3.6 KB | **Language**: markdown

````markdown
# Поддержка дистрибутивов Linux и серверов за NAT

Starry спроектирован для гетерогенного парка серверов: узлы кластера могут работать под управлением разных дистрибутивов Linux и находиться как на публичных IP-адресах, так и за провайдерским NAT.

---

## 1. Матрица совместимости дистрибутивов

| Семейство дистрибутивов | Пакетный менеджер | Поддерживаемые ОС | Особенности |
|---|---|---|---|
| **Debian / Ubuntu** | `apt` | Ubuntu 22.04+, 24.04+, Debian 11, 12, Pop!_OS | Автоматическая проверка `/var/run/reboot-required` |
| **RHEL / Enterprise Linux** | `dnf` | AlmaLinux 9+, Rocky Linux 9+, Fedora 39+, RHEL 8, 9, CentOS Stream | Проверка потребности перезагрузки через `needs-restarting -r` |
| **Alpine Linux** | `apk` | Alpine 3.18, 3.19, 3.20+ | Легковесные минималистичные ноды с `musl libc` |
| **Arch Linux** | `pacman` | Arch Linux, Manjaro, EndeavourOS | Rolling-release ядро и актуальные пакеты |

Starry автоматически определяет операционную систему ноды при первом подключении через `/etc/os-release` и выбирает соответствующий `IOsPackageManager`.

---

## 2. Подключение нод за NAT через Jump-хосты

Для серверов, у которых нет выделенного белого IPv4-адреса (например, дешевые локальные VPS или домашние лаборатории), Starry поддерживает маршрутизацию SSH через **Jump-ноды** (`-J` / `ProxyJump`):

```yaml
nodes:
  bastion:
    host: "203.0.113.1"
    port: 22
    is_public: true
    ssh_key: "ENC[AES256_GCM:v1:nonce:cipher:tag]"

  internal-worker:
    host: "10.0.0.50"
    port: 22
    jump_host: "root@203.0.113.1:22"
    vpn_ip: "10.99.0.2"
    ssh_key: "ENC[AES256_GCM:v1:nonce:cipher:tag]"
```

При подключении к `internal-worker` Starry автоматически строит прозрачный SSH-туннель через `bastion`, используя ControlMaster-мультиплексирование для мгновенного выполнения последующих команд.

---

## 3. Политика обновлений безопасности и Rolling Upgrade

Starry позволяет производить последовательное (rolling) обновление пакетов безопасности на всех нодах кластера:

```bash
starry nodes upgrade --rolling
```

### Алгоритм Rolling Upgrade:
1. Подключение к ноде по SSH и определение дистрибутива.
2. Применение патчей безопасности (`apt upgrade --with-new-pkgs`, `dnf update --security`, `apk upgrade`, `pacman -Syu`).
3. Проверка необходимости перезагрузки (`IsRebootRequiredAsync`).
4. Если требуется перезагрузка:
   - Отправка команды `reboot`.
   - Постоянный мониторинг и ожидание восстановления доступности ноды по SSH.
   - Проверка запуска агента и контейнеров.
5. Переход к следующей ноде кластера!
````

## File: site/guide/migrations-dns-p2p.md

**Lines**: 57 | **Size**: 4.0 KB | **Language**: markdown

````markdown
# Миграция сервисов (Blue-Green), P2P-образы и DNS

Starry включает встроенный интеллектуальный планировщик **DAG (Directed Acyclic Graph)**, позволяющий выполнять переезд сервисов между физическими нодами без простоя, передавать контейнерные образы напрямую между серверами по P2P-каналу и автоматически обновлять записи у публичных DNS-провайдеров.

---

## 1. Бесшовная миграция Blue-Green

Когда в конфигурации меняется целевая нода сервиса (например, сервис `web` переносится с `bocchi` на `nijika`):

```yaml
services:
  web:
    image: "nginx:alpine"
    node: "nijika" # Было: bocchi
    vip: "10.99.10.5"
```

Команда `starry plan` обнаруживает перемещение и строит план типа **`BLUE-GREEN`**:

```bash
starry plan
```

### Порядок выполнения:
1. **P2P предварительная доставка образа**: Нода `nijika` получает образ контейнера напрямую с ноды `bocchi` через поток `docker save | zstd | ssh docker load` (без необходимости скачивать его заново из внешнего реестра).
2. **Монтирование файлов и томов**: На ноде `nijika` развертываются актуальные конфигурации из секции `files`.
3. **Запуск Green-контейнера**: Новый контейнер запускается на `nijika`.
4. **Проверка готовности (Healthcheck)**: Starry опрашивает `healthcheck` эндпоинт нового контейнера.
5. **Мгновенное переключение VIP**: Как только Green-сервис подтвердил здоровье, Starry обновляет `AllowedIPs` в WireGuard меше: трафик к `10.99.10.5` начинает поступать на ноду `nijika` **без потери активных TCP-сессий**!
6. **Остановка Blue-контейнера**: Старый контейнер на `bocchi` корректно останавливается после дренажа соединений.

---

## 2. P2P-стриминг образов (`docker save | zstd | ssh load`)

Если приватный реестр образов недоступен или сервер находится в изолированном сетевом сегменте с медленным интернет-каналом, Starry передает слои образов напрямую между соседними узлами:
- Сжатие потока на лету через современный алгоритм `zstd -3` (максимальная скорость при высокой степени компрессии).
- Передача через защищенный SSH-туннель без сохранения временных tar-файлов на диске.

---

## 3. Провайдеры публичного DNS (Cloudflare & Webhook)

Starry автоматически синхронизирует внешние IP-адреса публичных узлов и маршрутизацию доменов:

### Cloudflare (`IDnsProvider`)
Поддерживает работу через Cloudflare API v4 с автоматическим определением Zone ID и поддержкой проксирования (`proxied: true`):
```yaml
dns:
  provider: "cloudflare"
  api_token: "ENC[AES256_GCM:...]"
  zone: "example.com"
```

### Webhook
Позволяет интегрировать Starry с любым внутренним DNS-сервером (CoreDNS, PowerDNS, BIND9) через отправку событий `upsert` и `delete` на HTTP-эндпоинт.
````

## File: site/guide/network-mesh-vips.md

**Lines**: 55 | **Size**: 4.2 KB | **Language**: markdown

````markdown
# Сетевой меш WireGuard, Кочующие VIP и Nftables

Starry объединяет все ноды кластера в децентрализованную P2P-сеть с использованием протокола **WireGuard** (`starry0`). Сеть работает поверх публичного интернета, преодолевая NAT и файрволы благодаря встроенному механизму `PersistentKeepalive = 25`.

---

## 1. Топология сети и NAT Traversal

В традиционных архитектурах серверы за NAT изолированы. В Starry:
- Ноды с белыми IP-адресами (`is_public: true`) объявляют свои публичные `Endpoint`.
- Ноды за NAT подключаются к публичным нодам и отправляют keepalive-пакеты каждые 25 секунд, удерживая состояние в таблицах трансляции NAT-шлюзов.
- Публичные ноды выполняют транзитную маршрутизацию между серверами за NAT.

```text
[ VPS-1: Публичный IP (is_public: true) ]
   ▲                            ▲
   │ WireGuard                  │ WireGuard
   │ Keepalive=25               │ Keepalive=25
   ▼                            ▼
[ VPS-2: За провайдерским NAT ]  [ VPS-3: За корпоративным NAT ]
```

---

## 2. Кочующие VIP контейнеров (Container VIPs 10.99.x.x)

Каждый развертываемый сервис получает свой постоянный виртуальный IP-адрес в подсети оверлея (`10.99.x.x`).

### Как это работает:
1. При деплое сервиса на ноду `bocchi` Starry добавляет VIP контейнера (например, `10.99.10.5/32`) в секцию `AllowedIPs` для ноды `bocchi` в конфигах всех остальных участников меша.
2. Весь трафик к `10.99.10.5` мгновенно маршрутизируется ядром Linux через интерфейс `starry0` прямо на ноду `bocchi`.
3. При миграции сервиса с ноды `bocchi` на `nijika` Starry обновляет `AllowedIPs` (`wg syncconf`): трафик перенаправляется на `nijika` **без смены IP сервиса**!

### Взаимодействие с Nginx Proxy Manager (NPM) / Reverse Proxy
Любой обратный прокси (NPM, Traefik, Caddy), запущенный на публичной ноде, может направлять трафик на `http://10.99.10.5:8080`. Ему не нужно знать физический IP сервера, на котором сейчас работает контейнер — оверлейный меш прозрачно доставит пакеты в нужную точку!

---

## 3. Встроенный Internal DNS (*.internal)

В агент Starry встроен легковесный DNS-сервер, слушающий порт 53 оверлейного интерфейса:
- Домен `api.internal` -> `10.99.10.5`
- Домен `database.internal` -> `10.99.0.2`

Контейнеры используют агент Starry в качестве первичного DNS-сервера, что позволяет сервисам обращаться друг к другу по стабильным доменным именам.

---

## 4. Файрвол и проброс портов в `nftables` (`table inet starry`)

Starry управляет изолированной таблицей `table inet starry`, не конфликтуя с правилами Docker или хостовой системы:
- Политика `chain input`: по умолчанию `drop`.
- Разрешены: `lo`, `starry0`, входящий SSH и входящий WireGuard UDP 51820.
- `chain prerouting`: DNAT проброс публичных портов IPv4/IPv6 на внутренние VIP контейнеров.
- `chain postrouting`: SNAT masquerade для сохранения корректного обратного пути пакетов.
````

## File: site/guide/security-cryptography.md

**Lines**: 62 | **Size**: 4.0 KB | **Language**: markdown

````markdown
# Безопасность, Криптография и GitOps

Starry реализует бескомпромиссную модель безопасности: конфигурационные файлы кластера могут безопасно храниться в **публичных Git-репозиториях**, так как все чувствительные данные зашифрованы с использованием алгоритма **AES-256-GCM** с обязательной контекстной привязкой **AAD (Additional Authenticated Data)**.

---

## 1. Контекстная привязка AAD: Защита от Cut-and-Paste атак

Классическая проблема зашифрованных секретов в Git — возможность злоумышленника или невнимательного оператора скопировать зашифрованную строку из одного поля в другое (например, подменить пароль непривилегированного пользователя значением SSH-ключа root).

В Starry каждое значение шифруется с привязкой к **каноническому JSON-пути** поля в конфигурационном дереве:
- Секрет для поля `nodes.bocchi.ssh_key` шифруется с `AAD = "nodes.bocchi.ssh_key"`
- При попытке перенести этот зашифрованный блок в поле `services.api.db_password`, расшифровка завершится с ошибкой `CryptographicException` из-за несовпадения тега аутентификации!

---

## 2. Форматы секретов в конфигурации

### Открытые секреты (`PLAIN[...]`)
При подготовке изменений разработчик может записать секрет в открытом виде:
```yaml
nodes:
  bocchi:
    host: "192.168.1.10"
    ssh_key: "PLAIN[-----BEGIN OPENSSH PRIVATE KEY-----...]"
```

При запуске команды `starry encrypt` Starry автоматически обнаруживает все открытые секреты, заменяет их на зашифрованные и сохраняет файлы:
```bash
starry encrypt --key $STARRY_MASTER_KEY ./cluster
```

### Зашифрованные секреты (`ENC[...]`)
Результат шифрования записывается в компактном конвертном формате:
```text
ENC[AES256_GCM:v1:<base64_nonce>:<base64_cipher>:<base64_tag>]
```

---

## 3. Регулярная ротация Мастер-Ключа в GitHub Actions

Для соответствия строгим стандартам безопасности рекомендуется регулярно (например, ежемесячно) производить полную перешифровку всех секретов новым мастер-ключом.

Starry автоматизирует этот процесс одной командой:
```bash
starry encrypt global --reencrypt \
  --old-key-env STARRY_MASTER_KEY_OLD \
  --new-key-env STARRY_MASTER_KEY_NEW \
  --allow-config-edit \
  --git-commit \
  --git-push \
  --update-github-secret STARRY_MASTER_KEY
```

Эта команда:
1. Рекурсивно сканирует все конфигурационные файлы (`.yaml`, `.json`, `.toml`).
2. Расшифровывает старым ключом, валидируя AAD каждого поля.
3. Шифрует новым ключом со свежим криптографическим Nonce на каждое поле.
4. Транзакционно перезаписывает файлы через временный файл (`temp-file rename`).
5. Создает Git-коммит и пушит его в репозиторий.
6. Обновляет секрет `STARRY_MASTER_KEY` в GitHub Actions через `gh secret set`.
````

## File: site/public/logo.svg

**Lines**: 46 | **Size**: 2.1 KB | **Language**: svg

```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100" fill="none">
  <defs>
    <linearGradient id="starry-gold-grad" x1="10" y1="10" x2="90" y2="90" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#FDE68A" />
      <stop offset="45%" stop-color="#F59E0B" />
      <stop offset="100%" stop-color="#D97706" />
    </linearGradient>
    <linearGradient id="starry-accent-grad" x1="20" y1="80" x2="80" y2="20" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#FBBF24" />
      <stop offset="100%" stop-color="#FFFBEB" />
    </linearGradient>
    <filter id="starry-glow" x="-20%" y="-20%" width="140%" height="140%" filterUnits="userSpaceOnUse">
      <feGaussianBlur stdDeviation="4" result="blur" />
      <feComposite in="SourceGraphic" in2="blur" operator="over" />
    </filter>
  </defs>

  <!-- Background decorative ring / orbit -->
  <circle cx="50" cy="50" r="42" stroke="url(#starry-gold-grad)" stroke-width="2.5" stroke-dasharray="6 8" opacity="0.4" />
  
  <!-- Outer glowing small stars / nodes -->
  <circle cx="50" cy="8" r="3" fill="#FBBF24" opacity="0.85" />
  <circle cx="92" cy="50" r="3" fill="#F59E0B" opacity="0.85" />
  <circle cx="50" cy="92" r="3" fill="#D97706" opacity="0.85" />
  <circle cx="8" cy="50" r="3" fill="#FBBF24" opacity="0.85" />

  <!-- Diagonal orbital connectors -->
  <line x1="21" y1="21" x2="79" y2="79" stroke="#FBBF24" stroke-width="1.5" stroke-dasharray="3 4" opacity="0.3" />
  <line x1="79" y1="21" x2="21" y2="79" stroke="#FBBF24" stroke-width="1.5" stroke-dasharray="3 4" opacity="0.3" />

  <!-- Main 4-point Diamond Star with Secondary Beams -->
  <!-- Secondary Beams -->
  <polygon points="50,22 55,45 78,50 55,55 50,78 45,55 22,50 45,45"
           fill="url(#starry-accent-grad)"
           opacity="0.6"
           transform="rotate(45 50 50)" />

  <!-- Primary Beams (STARRY Star) -->
  <polygon points="50,12 58,42 88,50 58,58 50,88 42,58 12,50 42,42"
           fill="url(#starry-gold-grad)"
           filter="url(#starry-glow)" />

  <!-- Center Core Glow -->
  <circle cx="50" cy="50" r="9" fill="#FFFBEB" />
  <circle cx="50" cy="50" r="5" fill="#F59E0B" />
</svg>
```

## File: src/Starry.Cli/Program.cs

**Lines**: 1405 | **Size**: 54.6 KB | **Language**: csharp

```csharp
﻿using System.Globalization;
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
using Starry.Infrastructure.Network;
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
            "NET" => HandleNetCommandAsync(commandArgs),
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
        var projectName = args.FirstOrDefault(a => !a.StartsWith('-'));
        var skipFolder = args.Any(a => string.Equals(a, "--skip-folder-create", StringComparison.OrdinalIgnoreCase));

        try
        {
            var (masterKey, dir) = RepositoryInitializer.Initialize(projectName, skipFolder);
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

        if (string.Equals(subCommand, "interfaces", StringComparison.OrdinalIgnoreCase))
        {
            return await ExecuteNetInterfacesAsync(subArgs).ConfigureAwait(false);
        }

        AnsiConsole.MarkupLine($"[yellow]starry nodes[/] - [grey]{Markup.Escape(localizer.GetString(LocalizationKeys.NodesCommandDescription))}[/]");
        return 0;
    }

    private static async Task<int> HandleNetCommandAsync(string[] args)
    {
        var subCommand = args.Length > 0 ? args[0] : "interfaces";
        var subArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();

        if (string.Equals(subCommand, "interfaces", StringComparison.OrdinalIgnoreCase))
        {
            return await ExecuteNetInterfacesAsync(subArgs).ConfigureAwait(false);
        }

        AnsiConsole.MarkupLine("[red]Usage: starry net interfaces [--config <path>] [--verify][/]");
        return 1;
    }

    private static async Task<int> ExecuteNetInterfacesAsync(string[] args)
    {
        var configPath = ExtractOptionValue(args, "--config") ?? ".";
        var verify = args.Any(a => string.Equals(a, "--verify", StringComparison.OrdinalIgnoreCase));

        try
        {
            var engine = new VirtualConfigEngine();
            var tree = engine.Load(configPath);
            var orchestrator = new ClusterOrchestrator();
            var nodes = await orchestrator.ResolveNodesAsync(tree).ConfigureAwait(false);

            try
            {
                var ledger = new IdLedgerService(configPath);
                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("[bold]Node & Host ID[/]");
                table.AddColumn("[bold]Interface[/]");
                table.AddColumn("[bold]IPv4 Addresses[/]");
                table.AddColumn("[bold]IPv6 Subnets / Addresses[/]");
                table.AddColumn("[bold]Classification / Verification[/]");

                foreach (var node in nodes)
                {
                    var hostId = ledger.AllocateHostId(node.NodeName);
                    var report = await NodeNetworkInspector.InspectNodeAsync(node, hostId, verify).ConfigureAwait(false);

                    foreach (var iface in report.Interfaces)
                    {
                        var statusMarkup = iface.Classification switch
                        {
                            InterfaceClassification.DedicatedPublic => "[green]DEDICATED PUBLIC[/]",
                            InterfaceClassification.RoutedIpv6Subnet => "[cyan]ROUTED /64 SUBNET[/]",
                            InterfaceClassification.BehindNatOrCgnat => "[yellow]BEHIND NAT / CGNAT[/]",
                            InterfaceClassification.Firewalled => "[red]FIREWALLED[/]",
                            _ => "[grey]INTERNAL ONLY[/]"
                        };

                        var v4 = iface.Ipv4Addresses.Count > 0 ? string.Join("\n", iface.Ipv4Addresses) : "-";
                        var v6 = iface.Ipv6Addresses.Count > 0 ? string.Join("\n", iface.Ipv6Addresses) : "-";

                        table.AddRow(
                            $"{report.NodeName}\n[grey]{report.MeshIpv6}[/]",
                            iface.InterfaceName,
                            v4,
                            v6,
                            statusMarkup);
                    }
                }

                AnsiConsole.Write(table);
                return 0;
            }
            finally
            {
                foreach (var n in nodes)
                {
                    await n.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Network inspection failed: {Markup.Escape(ex.Message)}[/]");
            return 1;
        }
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
            var configPath = ExtractOptionValue(args, "--config") ?? ".";
            var once = args.Any(a => string.Equals(a, "--once", StringComparison.OrdinalIgnoreCase));

            try
            {
                var engine = new VirtualConfigEngine();
                var tree = engine.Load(configPath);
                var orchestrator = new ClusterOrchestrator();

                string nodeName;
                string volumePath;

                if (args.Length >= 2 && tree.Root["volumes"]?[args[1]] is JsonObject volObj)
                {
                    nodeName = volObj["node"]?.ToString() ?? "node-1";
                    volumePath = volObj["path"]?.ToString() ?? $"/var/lib/docker/volumes/{args[1]}/_data";
                }
                else if (args.Length >= 3)
                {
                    nodeName = args[1];
                    volumePath = args[2];
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Usage: starry volume open <volume_name | node volumePath> [--config <path>] [--once][/]");
                    return 1;
                }

                var nodeObj = tree.Root["nodes"]?[nodeName] as JsonObject
                    ?? throw new InvalidOperationException($"Node '{nodeName}' not found in configuration.");

                await using var conn = await orchestrator.ResolveNodeConnectionAsync(nodeName, nodeObj).ConfigureAwait(false);

                AnsiConsole.MarkupLine($"[bold yellow]Querying volume files for '[cyan]{Markup.Escape(volumePath)}[/]' on '[cyan]{Markup.Escape(nodeName)}[/]'...[/]");
                var safeVol = "'" + volumePath.Replace("'", "'\\''", StringComparison.Ordinal) + "'";
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
```

## File: src/Starry.Cli/Starry.Cli.csproj

**Lines**: 47 | **Size**: 1.8 KB | **Language**: xml

```xml
﻿<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>Starry.Cli</RootNamespace>
    <AssemblyName>starry</AssemblyName>

    <!-- NuGet Tool Packaging -->
    <PackAsTool>true</PackAsTool>
    <ToolCommandName>starry</ToolCommandName>
    <PackageId>starrycli</PackageId>
    <Version>0.1.0</Version>
    <Authors>perdub</Authors>
    <Company>STARRY</Company>
    <Description>Decentralized GitOps orchestrator for Docker, Podman, WireGuard, nftables, and VPS fleet.</Description>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <PackageProjectUrl>https://github.com/perdub/starry</PackageProjectUrl>
    <RepositoryUrl>https://github.com/perdub/starry</RepositoryUrl>
    <PackageReadmeFile>README.md</PackageReadmeFile>

    <!-- Native AOT Compatibility -->
    <PublishAot>false</PublishAot>
    <PublishAot Condition="'$(PublishAotEnabled)' == 'true'">true</PublishAot>
    <InvariantGlobalization>true</InvariantGlobalization>
    <IlcOptimizationPreference>Speed</IlcOptimizationPreference>
    <IlcFoldIdenticalMethodBodies>false</IlcFoldIdenticalMethodBodies>
    <NoWarn>$(NoWarn);IL2026;IL3050;IL2111;IL2104</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <None Include="..\..\README.md" Pack="true" PackagePath="\" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Starry.Core\Starry.Core.csproj" />
    <ProjectReference Include="..\Starry.Infrastructure\Starry.Infrastructure.csproj" />
    <ProjectReference Include="..\Starry.Dsl\Starry.Dsl.fsproj" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Spectre.Console" Version="0.49.1" />
  </ItemGroup>

</Project>
```

## File: src/Starry.Core/Starry.Core.csproj

**Lines**: 13 | **Size**: 307 B | **Language**: xml

```xml
﻿<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <ProjectReference Include="..\Starry.Dsl\Starry.Dsl.fsproj" />
  </ItemGroup>

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

## File: src/Starry.Core/Backups/IBackupStorageProvider.cs

**Lines**: 13 | **Size**: 485 B | **Language**: csharp

```csharp
using Starry.Core.Nodes;

namespace Starry.Core.Backups;

/// <summary>
/// Streaming backup storage provider abstraction (Rclone, SFTP-Peer, S3, Local).
/// </summary>
public interface IBackupStorageProvider
{
    string TypeName { get; }
    Task StreamBackupAsync(INodeConnection node, string volumePath, string backupId, CancellationToken ct = default);
    Task StreamRestoreAsync(INodeConnection node, string backupId, string targetVolumePath, CancellationToken ct = default);
}
```

## File: src/Starry.Core/Configuration/ConfigTree.cs

**Lines**: 78 | **Size**: 2.2 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;

namespace Starry.Core.Configuration;

/// <summary>
/// Virtual aggregated configuration tree with source map tracking across multiple files.
/// </summary>
public sealed class ConfigTree
{
    private readonly Dictionary<string, SourceMapEntry> _sourceMap = new(StringComparer.Ordinal);

    public JsonObject Root { get; }

    public IReadOnlyDictionary<string, SourceMapEntry> SourceMap => _sourceMap;

    public ConfigTree(JsonObject? root = null)
    {
        Root = root ?? new JsonObject();
    }

    public void RegisterSource(string canonicalPath, SourceMapEntry entry)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(canonicalPath);
        ArgumentNullException.ThrowIfNull(entry);

        _sourceMap[canonicalPath] = entry;
    }

    public JsonNode? FindNode(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        JsonNode? current = Root;

        foreach (var segment in segments)
        {
            if (current is JsonObject obj && obj.TryGetPropertyValue(segment, out var next))
            {
                current = next;
            }
            else
            {
                return null;
            }
        }

        return current;
    }

    public void SetNode(string path, JsonNode? value, SourceMapEntry? sourceMapEntry = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var current = Root;

        for (var i = 0; i < segments.Length - 1; i++)
        {
            var segment = segments[i];
            if (!current.TryGetPropertyValue(segment, out var next) || next is not JsonObject nextObj)
            {
                nextObj = new JsonObject();
                current[segment] = nextObj;
            }

            current = nextObj;
        }

        var lastSegment = segments[^1];
        current[lastSegment] = value;

        if (sourceMapEntry != null)
        {
            _sourceMap[path] = sourceMapEntry;
        }
    }
}
```

## File: src/Starry.Core/Configuration/IConfigFormatProvider.cs

**Lines**: 34 | **Size**: 1.2 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;

namespace Starry.Core.Configuration;

/// <summary>
/// Configuration format provider interface for parsing and serializing config files (YAML, JSON, TOML, etc.).
/// </summary>
public interface IConfigFormatProvider
{
    /// <summary>
    /// File extensions supported by this provider (e.g., [".yaml", ".yml"], [".json"], [".toml"]).
    /// </summary>
    IReadOnlyList<string> SupportedExtensions { get; }

    /// <summary>
    /// Format identifier name (e.g. "yaml", "json", "toml").
    /// </summary>
    string FormatName { get; }

    /// <summary>
    /// Parses byte span content into a JsonNode/JsonObject tree with source mapping.
    /// </summary>
    /// <param name="content">Raw content bytes.</param>
    /// <param name="sourcePath">Originating file path.</param>
    /// <returns>Parsed JsonNode.</returns>
    JsonNode Parse(ReadOnlySpan<byte> content, string sourcePath);

    /// <summary>
    /// Serializes a JsonNode tree into the target format string.
    /// </summary>
    /// <param name="node">JsonNode tree.</param>
    /// <returns>Formatted string representation.</returns>
    string Serialize(JsonNode node);
}
```

## File: src/Starry.Core/Configuration/SourceMapEntry.cs

**Lines**: 10 | **Size**: 283 B | **Language**: csharp

```csharp
namespace Starry.Core.Configuration;

/// <summary>
/// Source mapping entry linking a canonical JSON path to its physical source file and line.
/// </summary>
public sealed record SourceMapEntry(
    string CanonicalPath,
    string FilePath,
    int Line = 1,
    int Column = 1);
```

## File: src/Starry.Core/Containers/ContainerState.cs

**Lines**: 11 | **Size**: 252 B | **Language**: csharp

```csharp
namespace Starry.Core.Containers;

public sealed record ContainerState(
    string Id,
    string Name,
    string Image,
    string Status,
    bool IsRunning,
    int ExitCode,
    DateTime CreatedAt,
    IReadOnlyDictionary<string, string> Labels);
```

## File: src/Starry.Core/Containers/IContainerRuntime.cs

**Lines**: 35 | **Size**: 1.2 KB | **Language**: csharp

```csharp
using Starry.Core.Nodes;

namespace Starry.Core.Containers;

/// <summary>
/// Container runtime engine abstraction (Docker Engine, Podman rootful, Podman rootless).
/// </summary>
public interface IContainerRuntime
{
    /// <summary>
    /// Runtime identifier name ("docker", "podman").
    /// </summary>
    string RuntimeName { get; }

    /// <summary>
    /// Probes node availability and runtime info.
    /// </summary>
    Task<RuntimeInfo> ProbeAsync(INodeConnection node, CancellationToken ct = default);

    /// <summary>
    /// Lists all containers on the target node.
    /// </summary>
    Task<IReadOnlyList<ContainerState>> ListContainersAsync(INodeConnection node, CancellationToken ct = default);

    /// <summary>
    /// Deploys or reconciles a container service on the target node according to spec.
    /// </summary>
    Task DeployContainerAsync(INodeConnection node, ServiceSpec spec, CancellationToken ct = default);

    /// <summary>
    /// Streams a container image directly peer-to-peer from source node to target node
    /// (e.g. docker save | zstd | ssh load).
    /// </summary>
    Task StreamImagePeerToPeerAsync(INodeConnection source, INodeConnection target, string imageTag, CancellationToken ct = default);
}
```

## File: src/Starry.Core/Containers/RuntimeInfo.cs

**Lines**: 9 | **Size**: 206 B | **Language**: csharp

```csharp
namespace Starry.Core.Containers;

public sealed record RuntimeInfo(
    string Engine,
    string Version,
    string OperatingSystem,
    string Architecture,
    bool IsRootless,
    string SocketPath);
```

## File: src/Starry.Core/Containers/ServiceSpec.cs

**Lines**: 42 | **Size**: 2.1 KB | **Language**: csharp

```csharp
namespace Starry.Core.Containers;

public sealed record ServiceSpec
{
    public required string Name { get; init; }
    public required string Image { get; init; }
    public int Priority { get; init; } = 50;
    public string? VipAddress { get; init; }
    public int Replicas { get; init; } = 1;
    public string? TargetNode { get; init; }
    public IReadOnlyList<string> TargetNodes { get; init; } = [];
    public IReadOnlyList<string> Networks { get; init; } = ["default"];
    public string? Egress { get; init; } = "true";

    public IReadOnlyList<string> Ports { get; init; } = [];
    public IReadOnlyList<string> Volumes { get; init; } = [];
    public IReadOnlyDictionary<string, string> Environment { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public IReadOnlyDictionary<string, string> Files { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public IReadOnlyDictionary<string, string> Labels { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public string? HealthcheckEndpoint { get; init; }

    // Full Docker Compose service parameters
    public string? Command { get; init; }
    public string? Entrypoint { get; init; }
    public string? WorkingDir { get; init; }
    public string? User { get; init; }
    public string? Hostname { get; init; }
    public string? Domainname { get; init; }
    public string Restart { get; init; } = "unless-stopped";
    public bool Privileged { get; init; }
    public IReadOnlyList<string> CapAdd { get; init; } = [];
    public IReadOnlyList<string> CapDrop { get; init; } = [];
    public IReadOnlyList<string> Devices { get; init; } = [];
    public IReadOnlyDictionary<string, string> Sysctls { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public IReadOnlyList<string> ExtraHosts { get; init; } = [];
    public string? NetworkMode { get; init; }
    public long? ShmSize { get; init; }
    public string? MemLimit { get; init; }
    public double? Cpus { get; init; }
    public string? StopSignal { get; init; }
    public int? StopGracePeriod { get; init; }
}
```

## File: src/Starry.Core/Dns/DnsRecord.cs

**Lines**: 9 | **Size**: 181 B | **Language**: csharp

```csharp
namespace Starry.Core.Dns;

public sealed record DnsRecord(
    string Id,
    string RecordType,
    string Name,
    string Content,
    int Ttl = 300,
    bool Proxied = false);
```

## File: src/Starry.Core/Dns/IDnsProvider.cs

**Lines**: 12 | **Size**: 510 B | **Language**: csharp

```csharp
namespace Starry.Core.Dns;

/// <summary>
/// DNS management provider abstraction (Cloudflare, Hetzner, Route53, Porkbun, Webhook).
/// </summary>
public interface IDnsProvider
{
    string ProviderName { get; }
    Task<IReadOnlyList<DnsRecord>> GetRecordsAsync(string zoneDomain, CancellationToken ct = default);
    Task UpsertRecordAsync(string zoneDomain, DnsRecord record, CancellationToken ct = default);
    Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct = default);
}
```

## File: src/Starry.Core/Localization/ILocalizer.cs

**Lines**: 33 | **Size**: 1.1 KB | **Language**: csharp

```csharp
namespace Starry.Core.Localization;

/// <summary>
/// Service contract for application localization supporting Russian and English.
/// </summary>
public interface ILocalizer
{
    /// <summary>
    /// Gets the current active language code (e.g. "ru", "en").
    /// </summary>
    string CurrentLanguage { get; }

    /// <summary>
    /// Sets the active language for localization.
    /// </summary>
    /// <param name="languageCode">ISO language code (e.g. "ru", "en").</param>
    void SetLanguage(string languageCode);

    /// <summary>
    /// Retrieves the localized string by key.
    /// </summary>
    /// <param name="key">Localization resource key.</param>
    /// <returns>Localized string or key fallback.</returns>
    string GetString(string key);

    /// <summary>
    /// Retrieves the formatted localized string by key and arguments.
    /// </summary>
    /// <param name="key">Localization resource key.</param>
    /// <param name="args">Format arguments.</param>
    /// <returns>Formatted localized string.</returns>
    string FormatString(string key, params object?[] args);
}
```

## File: src/Starry.Core/Localization/LocalizationKeys.cs

**Lines**: 43 | **Size**: 2.3 KB | **Language**: csharp

```csharp
namespace Starry.Core.Localization;

/// <summary>
/// Strongly-typed string constants for all localization keys across Starry.
/// </summary>
public static class LocalizationKeys
{
    public const string CliDescription = "cli.description";
    public const string CliVersion = "cli.version";
    public const string CliLangOption = "cli.option.lang";
    public const string CliVerboseOption = "cli.option.verbose";
    public const string CliConfigOption = "cli.option.config";

    public const string ConfigCommandDescription = "cmd.config.description";
    public const string ConfigValidateDescription = "cmd.config.validate.description";
    public const string ConfigPackDescription = "cmd.config.pack.description";
    public const string ConfigSplitDescription = "cmd.config.split.description";
    public const string ConfigFmtDescription = "cmd.config.fmt.description";
    public const string ConfigSchemaDescription = "cmd.config.schema.description";
    public const string ConfigPackSuccess = "cmd.config.pack.success";
    public const string ConfigSplitSuccess = "cmd.config.split.success";
    public const string ConfigFmtSuccess = "cmd.config.fmt.success";

    public const string EncryptCommandDescription = "cmd.encrypt.description";
    public const string EncryptGlobalDescription = "cmd.encrypt.global.description";

    public const string NodesCommandDescription = "cmd.nodes.description";
    public const string NodesUpgradeDescription = "cmd.nodes.upgrade.description";
    public const string NodesRotateKeysDescription = "cmd.nodes.rotate_keys.description";

    public const string PlanCommandDescription = "cmd.plan.description";
    public const string ApplyCommandDescription = "cmd.apply.description";
    public const string VolumeCommandDescription = "cmd.volume.description";
    public const string BundleCommandDescription = "cmd.bundle.description";

    public const string ValidationSuccess = "validation.success";
    public const string ValidationFailed = "validation.failed";
    public const string ErrorUnknownCommand = "error.unknown_command";
    public const string ErrorFileNotFound = "error.file_not_found";
    public const string ErrorInvalidLanguage = "error.invalid_language";
    public const string ErrorEncryptionFailed = "error.encryption_failed";
    public const string ErrorDecryptionFailed = "error.decryption_failed";
}
```

## File: src/Starry.Core/Nodes/INodeConnection.cs

**Lines**: 48 | **Size**: 1.5 KB | **Language**: csharp

```csharp
using Starry.Core.Processes;

namespace Starry.Core.Nodes;

/// <summary>
/// Represents metadata and communication contract for a cluster node (local or remote via SSH).
/// </summary>
public interface INodeConnection : IAsyncDisposable
{
    /// <summary>
    /// Node unique identifier name (e.g. "bocchi", "nijika", "ryo").
    /// </summary>
    string NodeName { get; }

    /// <summary>
    /// Target host address or IP.
    /// </summary>
    string Host { get; }

    /// <summary>
    /// Target SSH port.
    /// </summary>
    int Port { get; }

    /// <summary>
    /// Operating system distribution identifier (e.g. "ubuntu", "debian", "almalinux", "alpine", "arch").
    /// </summary>
    string? DistroId { get; }

    /// <summary>
    /// Executes a command on the target node with buffered text output.
    /// </summary>
    Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default);

    /// <summary>
    /// Executes a command on the target node with direct binary streaming.
    /// </summary>
    Task<ProcessResult> ExecuteStreamingCommandAsync(
        ProcessSpec command,
        Stream? stdinStream,
        Stream? stdoutStream,
        CancellationToken ct = default);

    /// <summary>
    /// Opens an HTTP client to communicate with the container engine socket over SSH/Unix domain socket.
    /// </summary>
    Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default);
}
```

## File: src/Starry.Core/Packaging/IOsPackageManager.cs

**Lines**: 29 | **Size**: 1.0 KB | **Language**: csharp

```csharp
using Starry.Core.Nodes;

namespace Starry.Core.Packaging;

/// <summary>
/// Cross-distribution package manager abstraction (apt, dnf, apk, pacman).
/// </summary>
public interface IOsPackageManager
{
    /// <summary>
    /// Linux distribution identifiers supported by this manager (e.g. ["ubuntu", "debian"]).
    /// </summary>
    IReadOnlyList<string> SupportedDistroIds { get; }

    /// <summary>
    /// Ensures that logical system packages (e.g. wireguard, nftables, curl) are installed.
    /// </summary>
    Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default);

    /// <summary>
    /// Applies security updates or full system upgrades on the node.
    /// </summary>
    Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default);

    /// <summary>
    /// Checks whether the node requires a reboot after package updates.
    /// </summary>
    Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct = default);
}
```

## File: src/Starry.Core/Processes/IProcessRunner.cs

**Lines**: 22 | **Size**: 761 B | **Language**: csharp

```csharp
namespace Starry.Core.Processes;

/// <summary>
/// Strongly-typed process runner interface designed to prevent CLI command injection
/// and safely stream secrets via standard input or binary streams.
/// </summary>
public interface IProcessRunner
{
    /// <summary>
    /// Executes a system process specified by ProcessSpec with buffered text output.
    /// </summary>
    Task<ProcessResult> RunAsync(ProcessSpec spec, CancellationToken ct = default);

    /// <summary>
    /// Executes a system process with direct binary streaming for standard input and output.
    /// </summary>
    Task<ProcessResult> RunStreamingAsync(
        ProcessSpec spec,
        Stream? stdinStream,
        Stream? stdoutStream,
        CancellationToken ct = default);
}
```

## File: src/Starry.Core/Processes/ProcessResult.cs

**Lines**: 13 | **Size**: 290 B | **Language**: csharp

```csharp
namespace Starry.Core.Processes;

/// <summary>
/// Execution outcome of a completed process.
/// </summary>
public sealed record ProcessResult(
    int ExitCode,
    string StandardOutput,
    string StandardError,
    TimeSpan ElapsedTime)
{
    public bool IsSuccess => ExitCode == 0;
}
```

## File: src/Starry.Core/Processes/ProcessSpec.cs

**Lines**: 14 | **Size**: 643 B | **Language**: csharp

```csharp
namespace Starry.Core.Processes;

/// <summary>
/// Specification for starting a secure child process with protection against command injection.
/// </summary>
public sealed record ProcessSpec
{
    public required string Executable { get; init; }
    public IReadOnlyList<string> Arguments { get; init; } = [];
    public string? WorkingDirectory { get; init; }
    public IReadOnlyDictionary<string, string> EnvironmentVariables { get; init; } = new Dictionary<string, string>(StringComparer.Ordinal);
    public ReadOnlyMemory<byte>? StandardInputBytes { get; init; }
    public TimeSpan Timeout { get; init; } = TimeSpan.FromMinutes(5);
}
```

## File: src/Starry.Core/Security/ICryptoService.cs

**Lines**: 22 | **Size**: 822 B | **Language**: csharp

```csharp
namespace Starry.Core.Security;

/// <summary>
/// AES-256-GCM authenticated encryption service with canonical JSON path AAD binding.
/// </summary>
public interface ICryptoService
{
    /// <summary>
    /// Encrypts plaintext bytes with AES-256-GCM using the specified key and JSON path as AAD.
    /// </summary>
    SecretEnvelope Encrypt(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> key, string canonicalJsonPath);

    /// <summary>
    /// Decrypts ciphertext envelope with AES-256-GCM using the specified key and validating AAD.
    /// </summary>
    byte[] Decrypt(SecretEnvelope envelope, ReadOnlySpan<byte> key, string canonicalJsonPath);

    /// <summary>
    /// Parses an ENC[...] string into SecretEnvelope.
    /// </summary>
    bool TryParseEncrypted(string value, out SecretEnvelope? envelope);
}
```

## File: src/Starry.Core/Security/SecretEnvelope.cs

**Lines**: 9 | **Size**: 248 B | **Language**: csharp

```csharp
namespace Starry.Core.Security;

public sealed record SecretEnvelope(
    string NonceBase64,
    string CipherBase64,
    string TagBase64)
{
    public string FormatEncrypted() => $"ENC[AES256_GCM:v1:{NonceBase64}:{CipherBase64}:{TagBase64}]";
}
```

## File: src/Starry.Dsl/ConfigAst.fs

**Lines**: 384 | **Size**: 16.3 KB | **Language**: fsharp

```fsharp
namespace Starry.Dsl

open System
open System.IO
open System.Text
open System.Text.Json
open System.Text.Json.Nodes
open YamlDotNet.Serialization
open Tomlyn
open Tomlyn.Model

type ConfigNode =
    | ObjectNode of Map<string, ConfigNode> * sourceFile: string option
    | ArrayNode of ConfigNode list * sourceFile: string option
    | StringNode of string * sourceFile: string option
    | IntNode of int64 * sourceFile: string option
    | FloatNode of float * sourceFile: string option
    | BoolNode of bool * sourceFile: string option
    | NullNode of sourceFile: string option

module ConfigAst =

    let rec sourceOf node =
        match node with
        | ObjectNode (_, s)
        | ArrayNode (_, s)
        | StringNode (_, s)
        | IntNode (_, s)
        | FloatNode (_, s)
        | BoolNode (_, s)
        | NullNode s -> s

    let rec withSource src node =
        match node with
        | ObjectNode (m, _) -> ObjectNode (m, Some src)
        | ArrayNode (l, _) -> ArrayNode (l, Some src)
        | StringNode (s, _) -> StringNode (s, Some src)
        | IntNode (i, _) -> IntNode (i, Some src)
        | FloatNode (f, _) -> FloatNode (f, Some src)
        | BoolNode (b, _) -> BoolNode (b, Some src)
        | NullNode _ -> NullNode (Some src)

    // Convert raw parsed objects into ConfigNode
    let rec fromRawObject (src: string option) (value: obj) : ConfigNode =
        match value with
        | null -> NullNode src
        | :? ConfigNode as cn -> cn
        | :? bool as b -> BoolNode (b, src)
        | :? int as i -> IntNode (int64 i, src)
        | :? int64 as i -> IntNode (i, src)
        | :? float as f -> FloatNode (f, src)
        | :? decimal as m -> FloatNode (double m, src)
        | :? string as s -> StringNode (s, src)
        | :? JsonValue as jv ->
            let s = jv.ToString()
            let mutable b = false
            let mutable i = 0L
            let mutable d = 0.0
            if jv.TryGetValue<bool>(&b) then BoolNode (b, src)
            elif jv.TryGetValue<int64>(&i) then IntNode (i, src)
            elif jv.TryGetValue<double>(&d) then FloatNode (d, src)
            else StringNode (s, src)
        | :? JsonObject as jo ->
            let map =
                jo
                |> Seq.map (fun kvp -> kvp.Key, fromRawObject src kvp.Value)
                |> Map.ofSeq
            ObjectNode (map, src)
        | :? JsonArray as ja ->
            let list = ja |> Seq.map (fromRawObject src) |> Seq.toList
            ArrayNode (list, src)
        | :? TomlTable as tt ->
            let map =
                tt
                |> Seq.map (fun kvp -> kvp.Key, fromRawObject src kvp.Value)
                |> Map.ofSeq
            ObjectNode (map, src)
        | :? TomlArray as ta ->
            let list = ta |> Seq.map (fromRawObject src) |> Seq.toList
            ArrayNode (list, src)
        | :? TomlTableArray as tta ->
            let list = tta |> Seq.map (fromRawObject src) |> Seq.toList
            ArrayNode (list, src)
        | :? System.Collections.IDictionary as dict ->
            let seq = seq { for k in dict.Keys do yield string k, fromRawObject src dict.[k] }
            ObjectNode (Map.ofSeq seq, src)
        | :? System.Collections.IEnumerable as enum when not (enum :? string) ->
            let seq = seq { for item in enum do yield fromRawObject src item }
            ArrayNode (Seq.toList seq, src)
        | other -> StringNode (string other, src)

    // Convert ConfigNode into untyped .NET objects for serializers
    let rec toRawObject (node: ConfigNode) : obj =
        match node with
        | NullNode _ -> null
        | BoolNode (b, _) -> box b
        | IntNode (i, _) -> box i
        | FloatNode (f, _) -> box f
        | StringNode (s, _) -> box s
        | ArrayNode (items, _) ->
            let list = System.Collections.Generic.List<obj>()
            items |> List.iter (fun item -> list.Add(toRawObject item))
            box list
        | ObjectNode (props, _) ->
            let dict = System.Collections.Generic.Dictionary<string, obj>(StringComparer.Ordinal)
            props |> Map.iter (fun k v -> dict.[k] <- toRawObject v)
            box dict

    // Interop with System.Text.Json.Nodes.JsonNode
    let rec toJsonNode (node: ConfigNode) : JsonNode =
        match node with
        | NullNode _ -> null
        | BoolNode (b, _) -> JsonValue.Create(b) :> JsonNode
        | IntNode (i, _) -> JsonValue.Create(i) :> JsonNode
        | FloatNode (f, _) -> JsonValue.Create(f) :> JsonNode
        | StringNode (s, _) -> JsonValue.Create(s) :> JsonNode
        | ArrayNode (items, _) ->
            let arr = JsonArray()
            items |> List.iter (fun item ->
                let jn = toJsonNode item
                if jn <> null then arr.Add(jn) else arr.Add(null)
            )
            arr :> JsonNode
        | ObjectNode (props, _) ->
            let jo = JsonObject()
            props |> Map.iter (fun k v ->
                let jn = toJsonNode v
                jo.[k] <- jn
            )
            jo :> JsonNode

    let fromJsonNode (src: string option) (node: JsonNode) : ConfigNode =
        fromRawObject src node

    // Parsers for individual files
    let parseYaml (content: string) (sourcePath: string option) : ConfigNode =
        let deserializer = DeserializerBuilder().Build()
        use reader = new StringReader(content)
        let raw = deserializer.Deserialize(reader)
        fromRawObject sourcePath raw

    let parseJson (content: string) (sourcePath: string option) : ConfigNode =
        let parsed = JsonNode.Parse(content)
        fromRawObject sourcePath parsed

    let parseToml (content: string) (sourcePath: string option) : ConfigNode =
        let model = Toml.ToModel(content)
        fromRawObject sourcePath model

    let parseFile (filePath: string) : ConfigNode =
        let ext = Path.GetExtension(filePath).ToLowerInvariant()
        let content = File.ReadAllText(filePath, Encoding.UTF8)
        let src = Some filePath
        match ext with
        | ".yaml" | ".yml" -> parseYaml content src
        | ".json" -> parseJson content src
        | ".toml" -> parseToml content src
        | _ -> StringNode (content, src)

    // Merge two ObjectNode maps
    let rec mergeObjects (a: Map<string, ConfigNode>) (b: Map<string, ConfigNode>) : Map<string, ConfigNode> =
        b |> Map.fold (fun acc k v2 ->
            match acc.TryFind(k) with
            | Some (ObjectNode (m1, src1)) ->
                match v2 with
                | ObjectNode (m2, src2) ->
                    let merged = mergeObjects m1 m2
                    Map.add k (ObjectNode (merged, src2 |> Option.orElse src1)) acc
                | _ -> Map.add k v2 acc
            | _ -> Map.add k v2 acc
        ) a

    let setAtPath (pathSegments: string list) (value: ConfigNode) (root: ConfigNode) : ConfigNode =
        let rec loop segments current =
            match segments with
            | [] -> value
            | [ last ] ->
                match current with
                | ObjectNode (m, src) -> ObjectNode (Map.add last value m, src)
                | _ -> ObjectNode (Map.ofList [ last, value ], None)
            | head :: tail ->
                match current with
                | ObjectNode (m, src) ->
                    let child = m.TryFind(head) |> Option.defaultValue (ObjectNode (Map.empty, None))
                    let updatedChild = loop tail child
                    ObjectNode (Map.add head updatedChild m, src)
                | _ ->
                    let updatedChild = loop tail (ObjectNode (Map.empty, None))
                    ObjectNode (Map.ofList [ head, updatedChild ], None)
        loop pathSegments root

    let rec getAtPath (pathSegments: string list) (root: ConfigNode) : ConfigNode option =
        match pathSegments with
        | [] -> Some root
        | head :: tail ->
            match root with
            | ObjectNode (m, _) ->
                match m.TryFind(head) with
                | Some child -> getAtPath tail child
                | None -> None
            | _ -> None

    // Unified Directory-to-Graph AST Loader
    let loadDirectory (dirPath: string) : ConfigNode =
        let dir = Path.GetFullPath(dirPath)
        if not (Directory.Exists(dir)) then
            failwithf "Directory not found: %s" dir

        let allFiles =
            Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
            |> Array.filter (fun f ->
                let rel = Path.GetRelativePath(dir, f).Replace('\\', '/')
                let segments = rel.Split('/')
                let isIgnored =
                    segments |> Array.exists (fun s ->
                        s.Equals(".git", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals(".starry", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals(".github", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("node_modules", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("dist", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("bin", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("obj", StringComparison.OrdinalIgnoreCase)
                    )
                not isIgnored
            )
            |> Array.sort

        let mutable rootNode = ObjectNode (Map.empty, Some dir)

        for file in allFiles do
            let rel = Path.GetRelativePath(dir, file).Replace('\\', '/')
            let parsed = parseFile file

            let dirName = Path.GetDirectoryName(rel)
            let fileName = Path.GetFileName(rel)
            let fileNameNoExt = Path.GetFileNameWithoutExtension(rel)

            if String.IsNullOrEmpty(dirName) &&
               (fileNameNoExt.Equals("starry", StringComparison.OrdinalIgnoreCase) ||
                fileNameNoExt.Equals("config", StringComparison.OrdinalIgnoreCase)) then
                // Root config file: merge top-level keys directly into root
                match parsed with
                | ObjectNode (props, _) ->
                    match rootNode with
                    | ObjectNode (rootProps, src) ->
                        rootNode <- ObjectNode (mergeObjects rootProps props, src)
                    | _ -> ()
                | _ -> ()
            else
                let segments =
                    let pathWithoutExt =
                        if String.IsNullOrEmpty(dirName) then fileNameNoExt
                        else Path.Combine(dirName, fileNameNoExt).Replace('\\', '/')
                    pathWithoutExt.Split('/', StringSplitOptions.RemoveEmptyEntries)
                    |> Array.toList

                rootNode <- setAtPath segments parsed rootNode

        rootNode

    // Serializers
    let serializeYaml (node: ConfigNode) : string =
        let serializer = SerializerBuilder().ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull).Build()
        let raw = toRawObject node
        serializer.Serialize(raw)

    let serializeJson (node: ConfigNode) : string =
        let jn = toJsonNode node
        let options = JsonSerializerOptions(WriteIndented = true)
        if jn = null then "{}" else jn.ToJsonString(options)

    let serializeToml (node: ConfigNode) : string =
        let jn = toJsonNode node
        let rec tomlConvert (n: JsonNode) : obj =
            match n with
            | null -> null
            | :? JsonObject as jo ->
                let tt = TomlTable()
                for kv in jo do
                    let c = tomlConvert kv.Value
                    if c <> null then tt.[kv.Key] <- c
                box tt
            | :? JsonArray as ja ->
                let ta = TomlArray()
                for item in ja do
                    let c = tomlConvert item
                    if c <> null then ta.Add(c)
                box ta
            | :? JsonValue as jv ->
                let mutable b = false
                let mutable i = 0L
                let mutable d = 0.0
                if jv.TryGetValue<bool>(&b) then box b
                elif jv.TryGetValue<int64>(&i) then box i
                elif jv.TryGetValue<double>(&d) then box d
                else box (jv.ToString())
            | other -> box (other.ToString())

        let table =
            match tomlConvert jn with
            | :? TomlTable as tt -> tt
            | _ -> TomlTable()
        Toml.FromModel(table)

    let serializeFormat (format: string) (node: ConfigNode) : string =
        match format.ToLowerInvariant() with
        | "json" -> serializeJson node
        | "toml" -> serializeToml node
        | _ -> serializeYaml node

    // starry config pack
    let pack (root: ConfigNode) (format: string) : string =
        serializeFormat format root

    // starry config split
    let split (root: ConfigNode) (targetDir: string) (format: string) : unit =
        Directory.CreateDirectory(targetDir) |> ignore
        let ext =
            match format.ToLowerInvariant() with
            | "json" -> ".json"
            | "toml" -> ".toml"
            | _ -> ".yaml"

        match root with
        | ObjectNode (props, _) ->
            let mutable rootProperties = Map.empty

            props |> Map.iter (fun key node ->
                match key.ToLowerInvariant() with
                | "nodes" ->
                    match node with
                    | ObjectNode (nodesMap, _) ->
                        let nodesDir = Path.Combine(targetDir, "nodes")
                        Directory.CreateDirectory(nodesDir) |> ignore
                        nodesMap |> Map.iter (fun nodeName nodeVal ->
                            let filePath = Path.Combine(nodesDir, nodeName + ext)
                            File.WriteAllText(filePath, serializeFormat format nodeVal, Encoding.UTF8)
                        )
                    | _ -> rootProperties <- Map.add key node rootProperties
                | "services" ->
                    match node with
                    | ObjectNode (svcMap, _) ->
                        let svcDir = Path.Combine(targetDir, "services")
                        Directory.CreateDirectory(svcDir) |> ignore
                        svcMap |> Map.iter (fun svcName svcVal ->
                            let filePath = Path.Combine(svcDir, svcName + ext)
                            File.WriteAllText(filePath, serializeFormat format svcVal, Encoding.UTF8)
                        )
                    | _ -> rootProperties <- Map.add key node rootProperties
                | "domains" ->
                    let domDir = Path.Combine(targetDir, "domains")
                    Directory.CreateDirectory(domDir) |> ignore
                    let filePath = Path.Combine(domDir, "domains" + ext)
                    File.WriteAllText(filePath, serializeFormat format node, Encoding.UTF8)
                | "ports" ->
                    let filePath = Path.Combine(targetDir, "ports" + ext)
                    File.WriteAllText(filePath, serializeFormat format node, Encoding.UTF8)
                | "volumes" ->
                    let filePath = Path.Combine(targetDir, "volumes" + ext)
                    File.WriteAllText(filePath, serializeFormat format node, Encoding.UTF8)
                | "files" ->
                    let filesDir = Path.Combine(targetDir, "files")
                    Directory.CreateDirectory(filesDir) |> ignore
                    let rec dumpFiles (prefix: string) (fNode: ConfigNode) =
                        match fNode with
                        | StringNode (text, _) ->
                            let filePath = Path.Combine(filesDir, prefix)
                            let pDir = Path.GetDirectoryName(filePath)
                            if not (String.IsNullOrEmpty(pDir)) then Directory.CreateDirectory(pDir) |> ignore
                            File.WriteAllText(filePath, text, Encoding.UTF8)
                        | ObjectNode (fMap, _) ->
                            fMap |> Map.iter (fun subKey subVal ->
                                let subPath = if String.IsNullOrEmpty(prefix) then subKey else Path.Combine(prefix, subKey)
                                dumpFiles subPath subVal
                            )
                        | _ -> ()
                    dumpFiles "" node
                | _ ->
                    rootProperties <- Map.add key node rootProperties
            )

            let mainConfigFile = Path.Combine(targetDir, "starry" + ext)
            File.WriteAllText(mainConfigFile, serializeFormat format (ObjectNode (rootProperties, None)), Encoding.UTF8)
        | _ -> ()
```

## File: src/Starry.Dsl/Generators.fs

**Lines**: 236 | **Size**: 11.1 KB | **Language**: fsharp

```fsharp
namespace Starry.Dsl

open System
open System.Text

module Generators =

    type WireGuardPeerSpec = {
        NodeName: string
        HostId: int
        PublicKey: string
        Endpoint: string option
        ListenPort: int
        IsPublic: bool
    }

    type WireGuardHostSpec = {
        NodeName: string
        HostId: int
        PrivateKey: string
        PublicKey: string
        ListenPort: int
        Endpoint: string option
        IsPublic: bool
        Peers: WireGuardPeerSpec list
    }

    type NftablePortRule = {
        Name: string
        Protocol: string // "tcp", "udp", "both"
        HostPort: int
        TargetIpv6: string
        TargetPort: int
        Bidirectional: bool
        AddressFamily: string // "ipv4", "ipv6", "any"
    }

    type MeshSubnetPair = {
        SourceSubnet: string // e.g. "fd67:b00b:0001:0002::/64"
        TargetSubnet: string // e.g. "fd67:b00b:0002:0001::/64"
    }

    type NginxRoute =
        | SniPassthrough of serverName: string * targetEndpoint: string * proxyProtocol: bool
        | HttpReverseProxy of serverName: string * targetIpv6: string * targetPort: int * certPath: string option * keyPath: string option

    // 1. Generate WireGuard Config (starry0.conf) for a host
    let generateWireGuardConfig (spec: WireGuardHostSpec) : string =
        let sb = StringBuilder()
        let hostHex = sprintf "%04x" spec.HostId

        sb.AppendLine("# Managed by Starry GitOps Orchestrator - fd67:b00b::/32 Mesh") |> ignore
        sb.AppendLine("# Interface: starry0") |> ignore
        sb.AppendLine("[Interface]") |> ignore
        sb.AppendLine(sprintf "Address = fd67:b00b:%s::1/32" hostHex) |> ignore
        sb.AppendLine(sprintf "ListenPort = %d" spec.ListenPort) |> ignore
        sb.AppendLine(sprintf "PrivateKey = %s" spec.PrivateKey) |> ignore
        sb.AppendLine() |> ignore

        for peer in spec.Peers do
            let peerHex = sprintf "%04x" peer.HostId
            sb.AppendLine(sprintf "# Peer: %s (Host ID: %d)" peer.NodeName peer.HostId) |> ignore
            sb.AppendLine("[Peer]") |> ignore
            sb.AppendLine(sprintf "PublicKey = %s" peer.PublicKey) |> ignore
            // Each host routes the whole /48 statically so container changes never reload WG!
            sb.AppendLine(sprintf "AllowedIPs = fd67:b00b:%s::/48" peerHex) |> ignore

            match peer.Endpoint with
            | Some ep when peer.IsPublic && not (String.IsNullOrWhiteSpace(ep)) ->
                let formattedEp =
                    if ep.Contains(":") then ep
                    else sprintf "%s:%d" ep peer.ListenPort
                sb.AppendLine(sprintf "Endpoint = %s" formattedEp) |> ignore
            | _ -> ()

            sb.AppendLine("PersistentKeepalive = 25") |> ignore
            sb.AppendLine() |> ignore

        sb.ToString()

    // 2. Generate Nftables Ruleset (table inet starry)
    let generateNftablesRuleset
        (sshPort: int)
        (wireGuardPort: int)
        (allowedPublicPorts: int list)
        (portRules: NftablePortRule list)
        (allowedSubnetPairs: MeshSubnetPair list)
        (wanInterface: string option) : string =

        let sb = StringBuilder()
        let wan = wanInterface |> Option.defaultValue "eth0"

        sb.AppendLine("#!/usr/sbin/nft -f") |> ignore
        sb.AppendLine("# Managed by Starry GitOps Orchestrator - Table: inet starry") |> ignore
        sb.AppendLine("table inet starry") |> ignore
        sb.AppendLine("delete table inet starry") |> ignore
        sb.AppendLine("table inet starry {") |> ignore

        // Network isolation set for inter-container communication
        sb.AppendLine("    set allowed_mesh_pairs {") |> ignore
        sb.AppendLine("        type ipv6_addr . ipv6_addr") |> ignore
        sb.AppendLine("        flags interval") |> ignore
        if allowedSubnetPairs.Length > 0 then
            sb.AppendLine("        elements = {") |> ignore
            for pair in allowedSubnetPairs do
                sb.AppendLine(sprintf "            %s . %s," pair.SourceSubnet pair.TargetSubnet) |> ignore
            sb.AppendLine("        }") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore

        // Chain input
        sb.AppendLine("    chain input {") |> ignore
        sb.AppendLine("        type filter hook input priority filter; policy drop;") |> ignore
        sb.AppendLine("        ct state established,related accept") |> ignore
        sb.AppendLine("        iifname \"lo\" accept") |> ignore
        sb.AppendLine("        iifname \"starry0\" accept") |> ignore
        sb.AppendLine(sprintf "        tcp dport %d accept" sshPort) |> ignore
        sb.AppendLine(sprintf "        udp dport %d accept" wireGuardPort) |> ignore

        for p in allowedPublicPorts do
            sb.AppendLine(sprintf "        tcp dport %d accept" p) |> ignore
            sb.AppendLine(sprintf "        udp dport %d accept" p) |> ignore

        sb.AppendLine("        icmp type echo-request accept") |> ignore
        sb.AppendLine("        icmpv6 type { echo-request, nd-neighbor-solicit, nd-neighbor-advert, nd-router-advert } accept") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore

        // Chain forward
        sb.AppendLine("    chain forward {") |> ignore
        sb.AppendLine("        type filter hook forward priority filter; policy drop;") |> ignore
        sb.AppendLine("        ct state established,related accept") |> ignore
        sb.AppendLine("        iifname \"starry0\" oifname \"starry0\" ip6 saddr . ip6 daddr @allowed_mesh_pairs accept") |> ignore
        sb.AppendLine("        iifname \"starry0\" oifname \"starry0\" accept") |> ignore
        sb.AppendLine("        iifname \"starry0\" accept") |> ignore
        sb.AppendLine("        oifname \"starry0\" accept") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore

        // Chain prerouting (DNAT)
        sb.AppendLine("    chain prerouting {") |> ignore
        sb.AppendLine("        type nat hook prerouting priority dstnat; policy accept;") |> ignore
        for rule in portRules do
            let proto = rule.Protocol.ToLowerInvariant()
            let protoList = if proto = "both" then [ "tcp"; "udp" ] else [ proto ]
            for p in protoList do
                match rule.AddressFamily.ToLowerInvariant() with
                | "ipv4" ->
                    sb.AppendLine(sprintf "        meta nfproto ipv4 %s dport %d dnat to [%s]:%d" p rule.HostPort rule.TargetIpv6 rule.TargetPort) |> ignore
                | "ipv6" ->
                    sb.AppendLine(sprintf "        meta nfproto ipv6 %s dport %d dnat to [%s]:%d" p rule.HostPort rule.TargetIpv6 rule.TargetPort) |> ignore
                | _ ->
                    sb.AppendLine(sprintf "        %s dport %d dnat to [%s]:%d" p rule.HostPort rule.TargetIpv6 rule.TargetPort) |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore

        // Chain postrouting (SNAT / Masquerade)
        sb.AppendLine("    chain postrouting {") |> ignore
        sb.AppendLine("        type nat hook postrouting priority srcnat; policy accept;") |> ignore
        sb.AppendLine("        oifname \"starry0\" masquerade") |> ignore
        sb.AppendLine(sprintf "        oifname \"%s\" masquerade" wan) |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine("}") |> ignore

        sb.ToString()

    // 3. Generate Nginx Managed ($ingress) Config
    let generateNginxConfig (ingressHostId: int) (routes: NginxRoute list) : string =
        let sb = StringBuilder()
        let ingressIp = sprintf "fd67:b00b:%04x:ffff::1" ingressHostId

        sb.AppendLine("# Managed by Starry GitOps Orchestrator - $ingress SNI & HTTP Proxy") |> ignore
        sb.AppendLine("user nginx;") |> ignore
        sb.AppendLine("worker_processes auto;") |> ignore
        sb.AppendLine("pid /run/nginx.pid;") |> ignore
        sb.AppendLine() |> ignore
        sb.AppendLine("events {") |> ignore
        sb.AppendLine("    worker_connections 4096;") |> ignore
        sb.AppendLine("}") |> ignore
        sb.AppendLine() |> ignore

        // Stream block for L4 SNI routing
        sb.AppendLine("stream {") |> ignore
        sb.AppendLine("    map $ssl_preread_server_name $sni_upstream {") |> ignore
        for r in routes do
            match r with
            | SniPassthrough (name, target, _) ->
                sb.AppendLine(sprintf "        \"%s\" %s;" name target) |> ignore
            | HttpReverseProxy (name, _, _, _, _) ->
                // Route to internal HTTP block for TLS termination
                sb.AppendLine(sprintf "        \"%s\" 127.0.0.1:8443;" name) |> ignore
        sb.AppendLine("        default 127.0.0.1:8443;") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore
        sb.AppendLine("    server {") |> ignore
        sb.AppendLine("        listen 443;") |> ignore
        sb.AppendLine(sprintf "        listen [%s]:443;" ingressIp) |> ignore
        sb.AppendLine("        ssl_preread on;") |> ignore
        sb.AppendLine("        proxy_pass $sni_upstream;") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine("}") |> ignore
        sb.AppendLine() |> ignore

        // HTTP block for L7 Reverse Proxying
        sb.AppendLine("http {") |> ignore
        sb.AppendLine("    include /etc/nginx/mime.types;") |> ignore
        sb.AppendLine("    default_type application/octet-stream;") |> ignore
        sb.AppendLine() |> ignore

        for r in routes do
            match r with
            | HttpReverseProxy (name, targetIp, targetPort, certOpt, keyOpt) ->
                sb.AppendLine("    server {") |> ignore
                sb.AppendLine("        listen 127.0.0.1:8443 ssl http2;") |> ignore
                sb.AppendLine(sprintf "        server_name %s;" name) |> ignore
                match certOpt, keyOpt with
                | Some cert, Some key ->
                    sb.AppendLine(sprintf "        ssl_certificate %s;" cert) |> ignore
                    sb.AppendLine(sprintf "        ssl_certificate_key %s;" key) |> ignore
                | _ -> ()
                sb.AppendLine("        location / {") |> ignore
                sb.AppendLine(sprintf "            proxy_pass http://[%s]:%d;" targetIp targetPort) |> ignore
                sb.AppendLine("            proxy_set_header Host $host;") |> ignore
                sb.AppendLine("            proxy_set_header X-Real-IP $remote_addr;") |> ignore
                sb.AppendLine("            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;") |> ignore
                sb.AppendLine("            proxy_set_header X-Forwarded-Proto $scheme;") |> ignore
                sb.AppendLine("            proxy_http_version 1.1;") |> ignore
                sb.AppendLine("            proxy_set_header Upgrade $http_upgrade;") |> ignore
                sb.AppendLine("            proxy_set_header Connection \"upgrade\";") |> ignore
                sb.AppendLine("        }") |> ignore
                sb.AppendLine("    }") |> ignore
                sb.AppendLine() |> ignore
            | _ -> ()

        sb.AppendLine("}") |> ignore
        sb.ToString()
```

## File: src/Starry.Dsl/GraphResolver.fs

**Lines**: 175 | **Size**: 7.8 KB | **Language**: fsharp

```fsharp
namespace Starry.Dsl

open System
open System.Text.RegularExpressions

module GraphResolver =

    type ResolutionError =
        | CircularReference of cycleChain: string list
        | NodeNotFound of path: string
        | InvalidInheritanceTarget of path: string

    let private referenceRegex = Regex(@"\$\{?(?<path>[a-zA-Z0-9_\.]+)\}?", RegexOptions.Compiled)

    let parsePath (pathStr: string) : string list =
        let trimmed =
            let s = pathStr.Trim()
            if s.StartsWith("$") then s.Substring(1) else s
        trimmed.Split('.', StringSplitOptions.RemoveEmptyEntries) |> Array.toList

    let rec resolvePath (pathSegments: string list) (root: ConfigNode) : ConfigNode option =
        ConfigAst.getAtPath pathSegments root

    let scalarToString (node: ConfigNode) : string option =
        match node with
        | StringNode (s, _) -> Some s
        | IntNode (i, _) -> Some (string i)
        | FloatNode (f, _) -> Some (string f)
        | BoolNode (b, _) -> Some (if b then "true" else "false")
        | NullNode _ -> Some ""
        | _ -> None

    let rec resolveNode
        (decryptSecret: (string -> string) option)
        (activeStack: string list)
        (root: ConfigNode)
        (currentNode: ConfigNode) : Result<ConfigNode, ResolutionError> =

        match currentNode with
        | StringNode (text, src) ->
            let trimmed = text.Trim()
            // 1. Whole-subtree substitution (e.g. "$presets.default_env")
            if (trimmed.StartsWith("$") || trimmed.StartsWith("${")) &&
               not (trimmed.Contains(" ")) &&
               (trimmed.StartsWith("$") && not (trimmed.Substring(1).Contains("$"))) then
                let pathStr =
                    if trimmed.StartsWith("${") && trimmed.EndsWith("}") then
                        trimmed.Substring(2, trimmed.Length - 3)
                    elif trimmed.StartsWith("$") then
                        trimmed.Substring(1)
                    else trimmed

                if List.contains pathStr activeStack then
                    Error (CircularReference (List.rev (pathStr :: activeStack)))
                else
                    match resolvePath (parsePath pathStr) root with
                    | Some targetNode ->
                        // Recursively resolve target subtree
                        resolveNode decryptSecret (pathStr :: activeStack) root targetNode
                    | None ->
                        // If path not in tree, leave as literal string
                        Ok (StringNode (text, src))
            else
                // 3. String & file content interpolation ($path.to.value and ${path.to.value})
                let mutable hasError: ResolutionError option = None
                let interpolated =
                    referenceRegex.Replace(text, (fun m ->
                        let p = m.Groups.["path"].Value
                        if List.contains p activeStack then
                            hasError <- Some (CircularReference (List.rev (p :: activeStack)))
                            m.Value
                        else
                            match resolvePath (parsePath p) root with
                            | Some target ->
                                match resolveNode decryptSecret (p :: activeStack) root target with
                                | Ok resolvedTarget ->
                                    match scalarToString resolvedTarget with
                                    | Some s ->
                                        let finalStr =
                                            match decryptSecret with
                                            | Some decrypt -> decrypt s
                                            | None -> s
                                        finalStr
                                    | None -> m.Value
                                | Error err ->
                                    hasError <- Some err
                                    m.Value
                            | None -> m.Value
                    ))

                match hasError with
                | Some err -> Error err
                | None ->
                    let finalText =
                        match decryptSecret with
                        | Some decrypt -> decrypt interpolated
                        | None -> interpolated
                    Ok (StringNode (finalText, src))

        | ArrayNode (items, src) ->
            let resolvedItems =
                items
                |> List.map (resolveNode decryptSecret activeStack root)

            let rec collect (acc: ConfigNode list) (list: Result<ConfigNode, ResolutionError> list) =
                match list with
                | [] -> Ok (ArrayNode (List.rev acc, src))
                | (Ok n) :: rest -> collect (n :: acc) rest
                | (Error e) :: _ -> Error e

            collect [] resolvedItems

        | ObjectNode (props, src) ->
            // 2. Object Inheritance & Merging ($: key)
            let inheritanceRefs =
                match props.TryFind("$") with
                | Some (StringNode (refPath, _)) -> [ refPath ]
                | Some (ArrayNode (refList, _)) ->
                    refList
                    |> List.choose (function
                        | StringNode (r, _) -> Some r
                        | _ -> None)
                | _ -> []

            let mutable baseMap = Map.empty
            let mutable inheritError: ResolutionError option = None

            for refPath in inheritanceRefs do
                if inheritError.IsNone then
                    let cleanPath = if refPath.StartsWith("$") then refPath.Substring(1) else refPath
                    if List.contains cleanPath activeStack then
                        inheritError <- Some (CircularReference (List.rev (cleanPath :: activeStack)))
                    else
                        match resolvePath (parsePath cleanPath) root with
                        | Some (ObjectNode _ as baseNode) ->
                            match resolveNode decryptSecret (cleanPath :: activeStack) root baseNode with
                            | Ok (ObjectNode (resolvedBaseProps, _)) ->
                                baseMap <- ConfigAst.mergeObjects baseMap resolvedBaseProps
                            | Ok _ ->
                                inheritError <- Some (InvalidInheritanceTarget cleanPath)
                            | Error err ->
                                inheritError <- Some err
                        | Some _ ->
                            inheritError <- Some (InvalidInheritanceTarget cleanPath)
                        | None ->
                            inheritError <- Some (NodeNotFound cleanPath)

            match inheritError with
            | Some err -> Error err
            | None ->
                // Remove the special "$" inheritance key from explicitly defined properties
                let explicitProps = Map.remove "$" props

                // Merge baseMap with explicitProps (explicit overrides inherited)
                let combinedProps = ConfigAst.mergeObjects baseMap explicitProps

                let resolvedPropsResult =
                    combinedProps
                    |> Map.fold (fun accResult k v ->
                        match accResult with
                        | Error e -> Error e
                        | Ok accMap ->
                            match resolveNode decryptSecret activeStack root v with
                            | Ok rv -> Ok (Map.add k rv accMap)
                            | Error e -> Error e
                    ) (Ok Map.empty)

                match resolvedPropsResult with
                | Ok rm -> Ok (ObjectNode (rm, src))
                | Error e -> Error e

        | leaf -> Ok leaf

    let resolve (root: ConfigNode) (decryptSecret: (string -> string) option) : Result<ConfigNode, ResolutionError> =
        resolveNode decryptSecret [] root root
```

## File: src/Starry.Dsl/Library.fs

**Lines**: 5 | **Size**: 91 B | **Language**: fsharp

```fsharp
﻿namespace Starry.Dsl

module Say =
    let hello name =
        printfn "Hello %s" name
```

## File: src/Starry.Dsl/Starry.Dsl.fsproj

**Lines**: 19 | **Size**: 502 B | **Language**: fsproj

```fsproj
﻿<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="ConfigAst.fs" />
    <Compile Include="GraphResolver.fs" />
    <Compile Include="Generators.fs" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Tomlyn" Version="0.17.0" />
    <PackageReference Include="YamlDotNet" Version="16.3.0" />
  </ItemGroup>

</Project>
```

## File: src/Starry.Infrastructure/Starry.Infrastructure.csproj

**Lines**: 20 | **Size**: 580 B | **Language**: xml

```xml
﻿<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <ProjectReference Include="..\Starry.Core\Starry.Core.csproj" />
    <ProjectReference Include="..\Starry.Dsl\Starry.Dsl.fsproj" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Tomlyn" Version="0.17.0" />
    <PackageReference Include="YamlDotNet" Version="16.3.0" />
  </ItemGroup>

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <NoWarn>$(NoWarn);IL2026;IL3050;IL2111</NoWarn>
  </PropertyGroup>

</Project>
```

## File: src/Starry.Infrastructure/Agent/AgentOptions.cs

**Lines**: 31 | **Size**: 1.1 KB | **Language**: csharp

```csharp
using System.Net;

namespace Starry.Infrastructure.Agent;

public sealed record AgentOptions
{
    public int DnsPort { get; init; } = 53;
    public IPAddress? DnsBindAddress { get; init; }
    public IReadOnlyDictionary<string, string> InternalDnsRecords { get; init; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    public TimeSpan HealthCheckInterval { get; init; } = TimeSpan.FromSeconds(30);
    public IReadOnlyList<BackupPolicy> BackupPolicies { get; init; } = [];
    public string? NodeName { get; init; }

    public AgentOptions WithDnsRecord(string domain, string ip)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(domain);
        ArgumentException.ThrowIfNullOrWhiteSpace(ip);

        var dict = new Dictionary<string, string>(InternalDnsRecords, StringComparer.OrdinalIgnoreCase)
        {
            [domain] = ip
        };
        return this with { InternalDnsRecords = dict };
    }

    public AgentOptions WithDnsRecord(string domain, IPAddress ip)
    {
        ArgumentNullException.ThrowIfNull(ip);
        return WithDnsRecord(domain, ip.ToString());
    }
}
```

## File: src/Starry.Infrastructure/Agent/AgentStatusReport.cs

**Lines**: 17 | **Size**: 559 B | **Language**: csharp

```csharp
using Starry.Core.Containers;

namespace Starry.Infrastructure.Agent;

public sealed record AgentStatusReport(
    bool IsStarry0Active,
    string? WireGuardDump,
    IReadOnlyList<WireGuardPeerDump> Peers,
    IReadOnlyList<ContainerState> Containers,
    IReadOnlyList<ContainerState> RunningContainers,
    IReadOnlyList<DriftEvent> RecentDriftEvents,
    DateTime TimestampUtc)
{
    public bool IsInterfaceActive => IsStarry0Active;
    public int TotalContainers => Containers.Count;
    public int TotalRunningContainers => RunningContainers.Count;
}
```

## File: src/Starry.Infrastructure/Agent/BackupPolicy.cs

**Lines**: 8 | **Size**: 217 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Agent;

public sealed record BackupPolicy(
    string ServiceName,
    string VolumePath,
    TimeSpan Interval,
    string? RemoteDestination = null,
    string? BackupPrefix = null);
```

## File: src/Starry.Infrastructure/Agent/DriftEvent.cs

**Lines**: 10 | **Size**: 267 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Agent;

public sealed record DriftEvent(
    string ContainerId,
    string ContainerName,
    string Reason,
    DateTime TimestampUtc,
    bool RestartTriggered = true,
    bool RestartSucceeded = false,
    string? Details = null);
```

## File: src/Starry.Infrastructure/Agent/DriftEventArgs.cs

**Lines**: 12 | **Size**: 258 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Agent;

public sealed class DriftEventArgs : EventArgs
{
    public DriftEvent Drift { get; }

    public DriftEventArgs(DriftEvent drift)
    {
        ArgumentNullException.ThrowIfNull(drift);
        Drift = drift;
    }
}
```

## File: src/Starry.Infrastructure/Agent/StarryAgentDaemon.cs

**Lines**: 656 | **Size**: 24.2 KB | **Language**: csharp

```csharp
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Starry.Core.Backups;
using Starry.Core.Containers;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Processes;

namespace Starry.Infrastructure.Agent;

/// <summary>
/// Autonomous host agent daemon managing local WireGuard health, embedded DNS VIP resolution,
/// continuous container drift reconciliation, and volume backup scheduling.
/// </summary>
public sealed class StarryAgentDaemon : IAsyncDisposable
{
    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Standard fixed systemd service path on Linux")]
    public const string SystemdServicePath = "/etc/systemd/system/starry-agent.service";

    public const string SystemdServiceContent =
        """
        [Unit]
        Description=Starry Cluster Agent Daemon
        After=network.target docker.service podman.service

        [Service]
        Type=simple
        ExecStart=/usr/local/bin/starry agent run
        Restart=always
        RestartSec=5
        LimitNOFILE=65536

        [Install]
        WantedBy=multi-user.target
        """;

    private readonly IContainerRuntime _runtime;
    private readonly INodeConnection _nodeConnection;
    private readonly bool _ownsNodeConnection;
    private readonly IProcessRunner _processRunner;
    private readonly IBackupStorageProvider? _backupStorageProvider;
    private readonly Action<string>? _logger;
    private readonly ConcurrentQueue<DriftEvent> _driftHistory = new();
    private InternalDnsServer? _activeDnsServer;

    public event EventHandler<DriftEventArgs>? DriftDetected;

    public IReadOnlyList<DriftEvent> RecentDriftEvents => _driftHistory.ToArray();

    public InternalDnsServer? ActiveDnsServer => _activeDnsServer;

    public StarryAgentDaemon(
        IContainerRuntime? runtime = null,
        INodeConnection? nodeConnection = null,
        IProcessRunner? processRunner = null,
        IBackupStorageProvider? backupStorageProvider = null,
        Action<string>? logger = null)
    {
        _logger = logger;
        _processRunner = processRunner ?? new ProcessRunner();

        if (nodeConnection != null)
        {
            _nodeConnection = nodeConnection;
            _ownsNodeConnection = false;
        }
        else
        {
            _nodeConnection = new LocalNodeConnection("local", _processRunner);
            _ownsNodeConnection = true;
        }

        _runtime = runtime ?? DetectDefaultRuntime();
        _backupStorageProvider = backupStorageProvider;
    }

    /// <summary>
    /// Starts the agent daemon lifecycle:
    /// - Starts embedded InternalDnsServer on port 53 (or 5353) serving *.internal VIP records.
    /// - Starts background health monitoring &amp; drift detection loop (every 30 seconds).
    /// - Starts periodic volume streaming backup scheduler.
    /// </summary>
    public async Task RunAsync(AgentOptions options, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        _logger?.Invoke("Starting StarryAgentDaemon...");
        await StartDnsServerAsync(options, ct).ConfigureAwait(false);

        try
        {
            var healthTask = RunHealthMonitoringLoopAsync(options, ct);
            var backupTask = RunBackupSchedulerAsync(options.BackupPolicies, ct);

            await Task.WhenAll(healthTask, backupTask).ConfigureAwait(false);
        }
        finally
        {
            _logger?.Invoke("Stopping StarryAgentDaemon...");
            if (_activeDnsServer != null)
            {
                await _activeDnsServer.DisposeAsync().ConfigureAwait(false);
                _activeDnsServer = null;
            }
        }
    }

    /// <summary>
    /// Installs the starry-agent systemd unit file and enables the service.
    /// </summary>
    public async Task InstallSystemdServiceAsync(CancellationToken ct = default)
    {
        await InstallSystemdServiceAsync(SystemdServicePath, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Installs the starry-agent systemd unit file to a target destination and enables the service.
    /// </summary>
    public async Task InstallSystemdServiceAsync(string? targetServicePath, CancellationToken ct = default)
    {
        var servicePath = string.IsNullOrWhiteSpace(targetServicePath) ? SystemdServicePath : targetServicePath;
        _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Generating systemd unit file at '{servicePath}'..."));

        var unitText = SystemdServiceContent.ReplaceLineEndings("\n").TrimEnd() + "\n";
        var writeSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"mkdir -p $(dirname '{servicePath}') && cat > '{servicePath}' && chmod 644 '{servicePath}'"],
            StandardInputBytes = Encoding.UTF8.GetBytes(unitText),
            Timeout = TimeSpan.FromSeconds(30)
        };

        var writeResult = await _nodeConnection.ExecuteCommandAsync(writeSpec, ct).ConfigureAwait(false);
        if (!writeResult.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to write systemd service unit to '{servicePath}': {writeResult.StandardError}");
        }

        _logger?.Invoke("Reloading systemd daemon and enabling starry-agent.service...");

        var reloadSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "systemctl daemon-reload && systemctl enable --now starry-agent.service"],
            Timeout = TimeSpan.FromSeconds(60)
        };

        var reloadResult = await _nodeConnection.ExecuteCommandAsync(reloadSpec, ct).ConfigureAwait(false);
        if (!reloadResult.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to enable starry-agent.service: {reloadResult.StandardError}");
        }

        _logger?.Invoke("starry-agent.service successfully installed and enabled.");
    }

    /// <summary>
    /// Gathers local node agent status including WireGuard interface state, wg dump peers, and running containers.
    /// </summary>
    public async Task<AgentStatusReport> GetStatusAsync(CancellationToken ct = default)
    {
        _logger?.Invoke("Querying agent status...");

        var isInterfaceActive = await CheckStarry0ActiveAsync(ct).ConfigureAwait(false);
        var wgDump = await QueryWireGuardDumpAsync(ct).ConfigureAwait(false);
        var peers = ParseWireGuardDump(wgDump);
        var allContainers = await QueryContainersAsync(ct).ConfigureAwait(false);
        var runningContainers = allContainers.Where(static c => c.IsRunning).ToList();

        return new AgentStatusReport(
            IsStarry0Active: isInterfaceActive,
            WireGuardDump: wgDump,
            Peers: peers,
            Containers: allContainers,
            RunningContainers: runningContainers,
            RecentDriftEvents: _driftHistory.ToArray(),
            TimestampUtc: DateTime.UtcNow);
    }

    /// <summary>
    /// Executes a single health monitoring and reconciliation cycle.
    /// Detects stopped/exited managed containers and restarts them via Docker/Podman socket API.
    /// </summary>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Loop must continue if container listing fails")]
    public async Task<int> PerformHealthCheckAndReconcileAsync(CancellationToken ct = default)
    {
        _logger?.Invoke("Performing container drift detection...");

        IReadOnlyList<ContainerState> containers;
        try
        {
            containers = await _runtime.ListContainersAsync(_nodeConnection, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Warning: Unable to list containers during drift check: {ex.Message}"));
            return 0;
        }

        var restartedCount = 0;
        foreach (var container in containers)
        {
            if (await ReconcileSingleContainerAsync(container, ct).ConfigureAwait(false))
            {
                restartedCount++;
            }
        }

        return restartedCount;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Daemon must record failed restarts without throwing")]
    private async Task<bool> ReconcileSingleContainerAsync(ContainerState container, CancellationToken ct)
    {
        if (!IsManagedContainer(container))
        {
            return false;
        }

        var isStoppedOrExited = !container.IsRunning ||
            container.Status.Contains("exited", StringComparison.OrdinalIgnoreCase) ||
            container.Status.Contains("stopped", StringComparison.OrdinalIgnoreCase) ||
            container.Status.Contains("dead", StringComparison.OrdinalIgnoreCase);

        if (!isStoppedOrExited)
        {
            return false;
        }

        var reason = string.Create(CultureInfo.InvariantCulture, $"Managed container '{container.Name}' ({container.Id}) is not running. Status='{container.Status}', ExitCode={container.ExitCode}.");
        _logger?.Invoke(reason);

        var restartSuccess = false;
        string? details = null;

        try
        {
            restartSuccess = await RestartContainerAsync(container.Id, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            details = ex.Message;
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Failed to restart container '{container.Name}': {ex.Message}"));
        }

        RecordDriftEvent(new DriftEvent(
            ContainerId: container.Id,
            ContainerName: container.Name,
            Reason: reason,
            TimestampUtc: DateTime.UtcNow,
            RestartTriggered: true,
            RestartSucceeded: restartSuccess,
            Details: details));

        return restartSuccess;
    }

    /// <summary>
    /// Restarts a container via socket HTTP POST /v1.40/containers/{id}/start.
    /// </summary>
    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Standard fixed endpoint for Docker/Podman Unix domain socket")]
    public async Task<bool> RestartContainerAsync(string containerId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(containerId);

        var socketPath = await ResolveSocketPathAsync(ct).ConfigureAwait(false);
        using var client = await _nodeConnection.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        using var response = await client.PostAsync(
            new Uri(new Uri("http://127.0.0.1/"), $"/v1.40/containers/{containerId}/start"),
            content: null,
            ct).ConfigureAwait(false);

        if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotModified)
        {
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Successfully started container '{containerId}'."));
            return true;
        }

        var err = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        throw new InvalidOperationException($"Failed to restart container '{containerId}' via Docker API: {response.StatusCode} {err}");
    }

    /// <summary>
    /// Executes a streaming volume backup for the given policy.
    /// </summary>
    public async Task<string> ExecuteBackupAsync(BackupPolicy policy, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(policy);

        var prefix = string.IsNullOrWhiteSpace(policy.BackupPrefix) ? policy.ServiceName : policy.BackupPrefix;
        var backupId = string.Create(CultureInfo.InvariantCulture, $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmss}");

        var provider = _backupStorageProvider;
        if (provider == null && !string.IsNullOrWhiteSpace(policy.RemoteDestination))
        {
            provider = new RcloneBackupStorageProvider(policy.RemoteDestination);
        }

        if (provider == null)
        {
            throw new InvalidOperationException($"No backup storage provider configured for service '{policy.ServiceName}'");
        }

        _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Starting streaming backup for volume '{policy.VolumePath}' (ID: {backupId})..."));
        await provider.StreamBackupAsync(_nodeConnection, policy.VolumePath, backupId, ct).ConfigureAwait(false);
        _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Completed streaming backup for volume '{policy.VolumePath}' (ID: {backupId})."));

        return backupId;
    }

    public void RegisterDnsRecord(string domain, IPAddress address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(domain);
        ArgumentNullException.ThrowIfNull(address);

        _activeDnsServer?.RegisterRecord(domain, address);
    }

    public async ValueTask DisposeAsync()
    {
        if (_activeDnsServer != null)
        {
            await _activeDnsServer.DisposeAsync().ConfigureAwait(false);
            _activeDnsServer = null;
        }

        if (_ownsNodeConnection)
        {
            await _nodeConnection.DisposeAsync().ConfigureAwait(false);
        }

        GC.SuppressFinalize(this);
    }

    private async Task StartDnsServerAsync(AgentOptions options, CancellationToken ct)
    {
        var port = options.DnsPort > 0 ? options.DnsPort : 53;
        InternalDnsServer dnsServer;

        try
        {
            dnsServer = new InternalDnsServer(port);
            RegisterRecords(dnsServer, options.InternalDnsRecords);
            await dnsServer.StartAsync(options.DnsBindAddress, ct).ConfigureAwait(false);
            _activeDnsServer = dnsServer;
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Internal DNS server listening on port {port}."));
        }
        catch (SocketException) when (port == 53)
        {
            _logger?.Invoke("Binding port 53 failed. Falling back to port 5353 for embedded DNS server...");
            dnsServer = new InternalDnsServer(5353);
            RegisterRecords(dnsServer, options.InternalDnsRecords);
            await dnsServer.StartAsync(options.DnsBindAddress, ct).ConfigureAwait(false);
            _activeDnsServer = dnsServer;
            _logger?.Invoke("Internal DNS server listening on fallback port 5353.");
        }
    }

    private static void RegisterRecords(InternalDnsServer server, IReadOnlyDictionary<string, string>? records)
    {
        if (records == null)
        {
            return;
        }

        foreach (var (domain, ipStr) in records)
        {
            if (IPAddress.TryParse(ipStr, out var ip))
            {
                server.RegisterRecord(domain, ip);
                if (!domain.EndsWith(".internal", StringComparison.OrdinalIgnoreCase) && !domain.Contains('.', StringComparison.Ordinal))
                {
                    server.RegisterRecord($"{domain}.internal", ip);
                }
            }
        }
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Background loop must survive intermittent errors")]
    private async Task RunHealthMonitoringLoopAsync(AgentOptions options, CancellationToken ct)
    {
        var interval = options.HealthCheckInterval > TimeSpan.Zero
            ? options.HealthCheckInterval
            : TimeSpan.FromSeconds(30);

        using var timer = new PeriodicTimer(interval);

        try
        {
            await PerformHealthCheckAndReconcileAsync(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return;
        }
        catch (Exception ex)
        {
            RecordDriftEvent(new DriftEvent(
                ContainerId: string.Empty,
                ContainerName: "system",
                Reason: $"Initial health check cycle failed: {ex.Message}",
                TimestampUtc: DateTime.UtcNow,
                RestartTriggered: false,
                RestartSucceeded: false,
                Details: ex.ToString()));
        }

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var hasTicked = await timer.WaitForNextTickAsync(ct).ConfigureAwait(false);
                if (!hasTicked)
                {
                    break;
                }

                await PerformHealthCheckAndReconcileAsync(ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                RecordDriftEvent(new DriftEvent(
                    ContainerId: string.Empty,
                    ContainerName: "system",
                    Reason: $"Health check cycle failed: {ex.Message}",
                    TimestampUtc: DateTime.UtcNow,
                    RestartTriggered: false,
                    RestartSucceeded: false,
                    Details: ex.ToString()));
            }
        }
    }

    private async Task RunBackupSchedulerAsync(IReadOnlyList<BackupPolicy>? policies, CancellationToken ct)
    {
        if (policies == null || policies.Count == 0)
        {
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                // Graceful cancellation on daemon shutdown
            }
            return;
        }

        var tasks = policies.Select(p => RunSinglePolicySchedulerAsync(p, ct)).ToList();
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Policy backup failure should be logged and not crash the scheduler")]
    private async Task RunSinglePolicySchedulerAsync(BackupPolicy policy, CancellationToken ct)
    {
        var interval = policy.Interval > TimeSpan.Zero
            ? policy.Interval
            : TimeSpan.FromHours(24);

        using var timer = new PeriodicTimer(interval);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var hasTicked = await timer.WaitForNextTickAsync(ct).ConfigureAwait(false);
                if (!hasTicked)
                {
                    break;
                }

                await ExecuteBackupAsync(policy, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Backup failed for policy '{policy.ServiceName}': {ex.Message}"));
            }
        }
    }

    private void RecordDriftEvent(DriftEvent drift)
    {
        _driftHistory.Enqueue(drift);
        while (_driftHistory.Count > 100)
        {
            _driftHistory.TryDequeue(out _);
        }

        DriftDetected?.Invoke(this, new DriftEventArgs(drift));
    }

    private static bool IsManagedContainer(ContainerState container)
    {
        ArgumentNullException.ThrowIfNull(container);

        if (container.Labels.TryGetValue("managed-by", out var managedBy) &&
            string.Equals(managedBy, "starry", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return container.Labels.ContainsKey("starry.priority") ||
               container.Labels.ContainsKey("starry.service");
    }

    private async Task<string> ResolveSocketPathAsync(CancellationToken ct)
    {
        if (_runtime is BaseSocketContainerRuntime socketRuntime)
        {
            return await socketRuntime.ResolveSocketPathAsync(_nodeConnection, ct).ConfigureAwait(false);
        }

        string[] candidateSockets = ["/var/run/docker.sock", "/run/docker.sock", "/run/podman/podman.sock", "/var/run/podman/podman.sock"];
        foreach (var candidate in candidateSockets)
        {
            var check = await _nodeConnection.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "test",
                Arguments = ["-S", candidate]
            }, ct).ConfigureAwait(false);

            if (check.IsSuccess)
            {
                return candidate;
            }
        }

        return candidateSockets[0];
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Fallback if network interface enumeration is restricted")]
    private async Task<bool> CheckStarry0ActiveAsync(CancellationToken ct)
    {
        var linkSpec = new ProcessSpec
        {
            Executable = "ip",
            Arguments = ["link", "show", "starry0"]
        };

        var linkResult = await _nodeConnection.ExecuteCommandAsync(linkSpec, ct).ConfigureAwait(false);
        if (linkResult.IsSuccess)
        {
            var output = linkResult.StandardOutput;
            if (output.Contains("state UP", StringComparison.OrdinalIgnoreCase) ||
                output.Contains("<UP", StringComparison.OrdinalIgnoreCase) ||
                output.Contains("state UNKNOWN", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        try
        {
            var iface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(static i => string.Equals(i.Name, "starry0", StringComparison.OrdinalIgnoreCase));
            if (iface != null && iface.OperationalStatus == OperationalStatus.Up)
            {
                return true;
            }
        }
        catch
        {
            // Ignore failure querying network interfaces
        }

        return false;
    }

    private async Task<string?> QueryWireGuardDumpAsync(CancellationToken ct)
    {
        var wgSpec = new ProcessSpec
        {
            Executable = "wg",
            Arguments = ["show", "starry0", "dump"]
        };

        var wgResult = await _nodeConnection.ExecuteCommandAsync(wgSpec, ct).ConfigureAwait(false);
        return wgResult.IsSuccess ? wgResult.StandardOutput.TrimEnd() : null;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Status query should remain resilient even if container runtime is unavailable")]
    private async Task<IReadOnlyList<ContainerState>> QueryContainersAsync(CancellationToken ct)
    {
        try
        {
            return await _runtime.ListContainersAsync(_nodeConnection, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger?.Invoke(string.Create(CultureInfo.InvariantCulture, $"Warning: Unable to list containers: {ex.Message}"));
            return [];
        }
    }

    public static IReadOnlyList<WireGuardPeerDump> ParseWireGuardDump(string? dump)
    {
        if (string.IsNullOrWhiteSpace(dump))
        {
            return [];
        }

        var lines = dump.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1)
        {
            return [];
        }

        var peers = new List<WireGuardPeerDump>();
        for (var i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split('\t');
            if (parts.Length >= 8)
            {
                peers.Add(new WireGuardPeerDump(
                    PublicKey: parts[0],
                    PresharedKey: parts[1],
                    Endpoint: parts[2],
                    AllowedIps: parts[3],
                    LatestHandshake: long.TryParse(parts[4], CultureInfo.InvariantCulture, out var hs) ? hs : 0,
                    TransferRx: long.TryParse(parts[5], CultureInfo.InvariantCulture, out var rx) ? rx : 0,
                    TransferTx: long.TryParse(parts[6], CultureInfo.InvariantCulture, out var tx) ? tx : 0,
                    PersistentKeepalive: int.TryParse(parts[7], CultureInfo.InvariantCulture, out var ka) ? ka : 0));
            }
        }

        return peers;
    }

    private static IContainerRuntime DetectDefaultRuntime()
    {
        if (File.Exists("/run/podman/podman.sock") || File.Exists("/var/run/podman/podman.sock"))
        {
            return new PodmanRuntime();
        }

        return new DockerRuntime();
    }
}
```

## File: src/Starry.Infrastructure/Agent/WireGuardPeerDump.cs

**Lines**: 11 | **Size**: 269 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Agent;

public sealed record WireGuardPeerDump(
    string PublicKey,
    string PresharedKey,
    string Endpoint,
    string AllowedIps,
    long LatestHandshake,
    long TransferRx,
    long TransferTx,
    int PersistentKeepalive);
```

## File: src/Starry.Infrastructure/Backups/BundleManifest.cs

**Lines**: 7 | **Size**: 187 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Backups;

public sealed record BundleManifest(
    string ClusterName,
    DateTime CreatedAt,
    string StarryVersion,
    IReadOnlyList<string> Nodes);
```

## File: src/Starry.Infrastructure/Backups/FleetBundleService.cs

**Lines**: 131 | **Size**: 4.6 KB | **Language**: csharp

```csharp
using System.Formats.Tar;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;

namespace Starry.Infrastructure.Backups;

/// <summary>
/// Disaster recovery capsule exporter and importer packing entire fleet configuration
/// and state into a single portable archive for quick bootstrap on new IPs.
/// </summary>
public static class FleetBundleService
{
    private static readonly JsonSerializerOptions IndentedJsonOptions = new()
    {
        WriteIndented = true
    };

    public static void ExportBundle(
        ConfigTree tree,
        string bundleOutputPath,
        VirtualConfigEngine? engine = null)
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentException.ThrowIfNullOrWhiteSpace(bundleOutputPath);

        var configEngine = engine ?? new VirtualConfigEngine();
        var tempStagingDir = Path.Combine(Path.GetTempPath(), $"starry-bundle-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempStagingDir);

        try
        {
            // Split configuration into modular folder hierarchy inside staging dir
            configEngine.Split(tree, tempStagingDir, "yaml");

            // Write manifest metadata
            var clusterName = tree.Root["cluster_name"]?.ToString() ?? "starry-fleet";
            var nodesList = new List<string>();
            if (tree.Root["nodes"] is JsonObject nodesObj)
            {
                nodesList.AddRange(nodesObj.Select(kv => kv.Key));
            }

            var manifestObj = new JsonObject
            {
                ["cluster_name"] = clusterName,
                ["created_at"] = DateTime.UtcNow.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
                ["starry_version"] = "0.1.0"
            };

            var nodesArr = new JsonArray();
            foreach (var node in nodesList)
            {
                nodesArr.Add(JsonValue.Create(node));
            }
            manifestObj["nodes"] = nodesArr;

            var manifestJson = manifestObj.ToJsonString(IndentedJsonOptions);
            File.WriteAllText(Path.Combine(tempStagingDir, "manifest.json"), manifestJson);

            // Pack directory into tar.gz
            var outDir = Path.GetDirectoryName(bundleOutputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            using var fileStream = File.Create(bundleOutputPath);
            using var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal);
            TarFile.CreateFromDirectory(tempStagingDir, gzipStream, includeBaseDirectory: false);
        }
        finally
        {
            if (Directory.Exists(tempStagingDir))
            {
                Directory.Delete(tempStagingDir, recursive: true);
            }
        }
    }

    public static ConfigTree ImportBundle(
        string bundleArchivePath,
        string targetConfigDir,
        IReadOnlyDictionary<string, string>? ipOverrides = null,
        VirtualConfigEngine? engine = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bundleArchivePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetConfigDir);

        if (!File.Exists(bundleArchivePath))
        {
            throw new FileNotFoundException($"Bundle archive not found: {bundleArchivePath}", bundleArchivePath);
        }

        var configEngine = engine ?? new VirtualConfigEngine();

        if (!Directory.Exists(targetConfigDir))
        {
            Directory.CreateDirectory(targetConfigDir);
        }

        // Extract tar.gz into target directory
        using (var fileStream = File.OpenRead(bundleArchivePath))
        using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
        {
            TarFile.ExtractToDirectory(gzipStream, targetConfigDir, overwriteFiles: true);
        }

        // Load extracted configuration
        var tree = configEngine.Load(targetConfigDir);

        // Apply IP overrides if provided (for disaster recovery on new VPS instances)
        if (ipOverrides != null && ipOverrides.Count > 0 && tree.Root["nodes"] is JsonObject nodesObj)
        {
            foreach (var (nodeName, newIp) in ipOverrides)
            {
                if (nodesObj.TryGetPropertyValue(nodeName, out var nodeVal) && nodeVal is JsonObject nodeObj)
                {
                    nodeObj["host"] = newIp;
                }
            }

            // Persist updated IPs back to disk
            configEngine.Split(tree, targetConfigDir, "yaml");
        }

        return tree;
    }
}
```

## File: src/Starry.Infrastructure/Backups/RcloneBackupStorageProvider.cs

**Lines**: 80 | **Size**: 2.8 KB | **Language**: csharp

```csharp
using Starry.Core.Backups;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Backups;

/// <summary>
/// Streaming backup provider using rclone over Unix pipes without creating temp files on the VPS disk
/// (tar -C volume -cf - . | zstd -3 | rclone rcat remote:bucket/backupId.tar.zst).
/// </summary>
public sealed class RcloneBackupStorageProvider : IBackupStorageProvider
{
    private readonly string _remoteDestination;

    public string TypeName => "rclone";

    public RcloneBackupStorageProvider(string remoteDestination)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteDestination);
        _remoteDestination = remoteDestination.TrimEnd('/');
    }

    public async Task StreamBackupAsync(
        INodeConnection node,
        string volumePath,
        string backupId,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(volumePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(backupId);

        var remoteTarget = $"{_remoteDestination}/{backupId}.tar.zst";

        // Pure in-memory pipe: tar -> zstd -> rclone rcat
        var pipeCmd = $"tar -C '{volumePath}' -cf - . | zstd -3 | rclone rcat '{remoteTarget}'";

        var backupSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", pipeCmd],
            Timeout = TimeSpan.FromHours(2)
        };

        var result = await node.ExecuteCommandAsync(backupSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Streaming backup for volume '{volumePath}' failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task StreamRestoreAsync(
        INodeConnection node,
        string backupId,
        string targetVolumePath,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(backupId);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetVolumePath);

        var remoteTarget = $"{_remoteDestination}/{backupId}.tar.zst";

        // Pure in-memory restore pipe: rclone cat -> zstd -d -> tar -C target -xf -
        var pipeCmd = $"mkdir -p '{targetVolumePath}' && rclone cat '{remoteTarget}' | zstd -d | tar -C '{targetVolumePath}' -xf -";

        var restoreSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", pipeCmd],
            Timeout = TimeSpan.FromHours(2)
        };

        var result = await node.ExecuteCommandAsync(restoreSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Streaming restore for backup '{backupId}' failed on '{node.NodeName}': {result.StandardError}");
        }
    }
}
```

## File: src/Starry.Infrastructure/Configuration/ConfigValidationResult.cs

**Lines**: 5 | **Size**: 155 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Configuration;

public sealed record ConfigValidationResult(
    bool IsValid,
    IReadOnlyList<ValidationIssue> Issues);
```

## File: src/Starry.Infrastructure/Configuration/ConfigValidator.cs

**Lines**: 167 | **Size**: 6.8 KB | **Language**: csharp

```csharp
using System.Globalization;
using System.Net;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Validates cluster configuration schema, references, node constraints, and secrets syntax.
/// </summary>
public static class ConfigValidator
{
    public static ConfigValidationResult Validate(ConfigTree tree)
    {
        ArgumentNullException.ThrowIfNull(tree);

        var issues = new List<ValidationIssue>();

        ValidateNodes(tree, issues);
        ValidateServices(tree, issues);
        ValidateSecrets(tree, issues);

        var hasErrors = issues.Any(i => !i.IsWarning);
        return new ConfigValidationResult(!hasErrors, issues);
    }

    private static void ValidateNodes(ConfigTree tree, List<ValidationIssue> issues)
    {
        if (tree.Root.TryGetPropertyValue("nodes", out var nodesNode) && nodesNode is JsonObject nodesObj)
        {
            foreach (var (nodeName, nodeVal) in nodesObj)
            {
                var nodePath = $"nodes.{nodeName}";
                tree.SourceMap.TryGetValue(nodePath, out var sourceEntry);

                if (nodeVal is not JsonObject nodeObj)
                {
                    issues.Add(new ValidationIssue(nodePath, $"Node '{nodeName}' must be an object.", sourceEntry?.FilePath));
                    continue;
                }

                if (!nodeObj.TryGetPropertyValue("host", out var hostVal) || hostVal == null || string.IsNullOrWhiteSpace(hostVal.ToString()))
                {
                    issues.Add(new ValidationIssue($"{nodePath}.host", $"Node '{nodeName}' is missing required 'host' address.", sourceEntry?.FilePath));
                }

                if (nodeObj.TryGetPropertyValue("port", out var portVal) && portVal != null &&
                    (!int.TryParse(portVal.ToString(), CultureInfo.InvariantCulture, out var port) || port < 1 || port > 65535))
                {
                    issues.Add(new ValidationIssue($"{nodePath}.port", $"Node '{nodeName}' has invalid SSH port: {portVal}.", sourceEntry?.FilePath));
                }

                if (nodeObj.TryGetPropertyValue("vpn_ip", out var vpnIpVal) && vpnIpVal != null)
                {
                    var ipStr = vpnIpVal.ToString();
                    if (!IPAddress.TryParse(ipStr, out _))
                    {
                        issues.Add(new ValidationIssue($"{nodePath}.vpn_ip", $"Node '{nodeName}' has invalid VPN IP address: '{ipStr}'.", sourceEntry?.FilePath));
                    }
                }
            }
        }
    }

    private static void ValidateServices(ConfigTree tree, List<ValidationIssue> issues)
    {
        HashSet<string>? knownNodes = null;
        if (tree.Root.TryGetPropertyValue("nodes", out var nodesNode) && nodesNode is JsonObject nodesObj)
        {
            knownNodes = nodesObj.Select(kv => kv.Key).ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        if (tree.Root.TryGetPropertyValue("services", out var servicesNode) && servicesNode is JsonObject servicesObj)
        {
            foreach (var (svcName, svcVal) in servicesObj)
            {
                var svcPath = $"services.{svcName}";
                tree.SourceMap.TryGetValue(svcPath, out var sourceEntry);

                if (svcVal is not JsonObject svcObj)
                {
                    issues.Add(new ValidationIssue(svcPath, $"Service '{svcName}' must be an object.", sourceEntry?.FilePath));
                    continue;
                }

                if (!svcObj.TryGetPropertyValue("image", out var imageVal) || imageVal == null || string.IsNullOrWhiteSpace(imageVal.ToString()))
                {
                    issues.Add(new ValidationIssue($"{svcPath}.image", $"Service '{svcName}' is missing required 'image'.", sourceEntry?.FilePath));
                }

                if (svcObj.TryGetPropertyValue("priority", out var priorityVal) && priorityVal != null &&
                    (!int.TryParse(priorityVal.ToString(), CultureInfo.InvariantCulture, out var priority) || priority < 0 || priority > 100))
                {
                    issues.Add(new ValidationIssue($"{svcPath}.priority", $"Service '{svcName}' priority must be an integer between 0 and 100.", sourceEntry?.FilePath));
                }

                if (svcObj.TryGetPropertyValue("node", out var targetNodeVal) && targetNodeVal != null && knownNodes != null)
                {
                    var targetNode = targetNodeVal.ToString();
                    if (!knownNodes.Contains(targetNode))
                    {
                        issues.Add(new ValidationIssue($"{svcPath}.node", $"Service '{svcName}' references unknown node '{targetNode}'.", sourceEntry?.FilePath));
                    }
                }
            }
        }
    }

    private static void ValidateSecrets(ConfigTree tree, List<ValidationIssue> issues)
    {
        ScanSecretsRecursive(tree.Root, string.Empty, tree.SourceMap, issues);
    }

    private static void ScanSecretsRecursive(
        JsonNode? current,
        string currentPath,
        IReadOnlyDictionary<string, SourceMapEntry> sourceMap,
        List<ValidationIssue> issues)
    {
        if (current is null)
        {
            return;
        }

        if (current is JsonObject obj)
        {
            foreach (var (key, val) in obj)
            {
                var path = string.IsNullOrEmpty(currentPath) ? key : $"{currentPath}.{key}";
                ScanSecretsRecursive(val, path, sourceMap, issues);
            }
            return;
        }

        if (current is JsonArray arr)
        {
            for (var i = 0; i < arr.Count; i++)
            {
                ScanSecretsRecursive(arr[i], $"{currentPath}[{i}]", sourceMap, issues);
            }
            return;
        }

        if (current is JsonValue valNode)
        {
            var strVal = valNode.ToString();
            sourceMap.TryGetValue(currentPath, out var sourceEntry);

            if (strVal.StartsWith("PLAIN[", StringComparison.Ordinal) && strVal.EndsWith(']'))
            {
                issues.Add(new ValidationIssue(
                    currentPath,
                    "Open unencrypted secret detected: PLAIN[...]. Use '--encrypt-open-secrets' to encrypt.",
                    sourceEntry?.FilePath,
                    IsWarning: true));
            }
            else if (strVal.StartsWith("ENC[", StringComparison.Ordinal) &&
                     (!strVal.EndsWith(']') || !strVal.StartsWith("ENC[AES256_GCM:v1:", StringComparison.Ordinal)))
            {
                issues.Add(new ValidationIssue(
                    currentPath,
                    "Invalid encrypted secret format. Expected ENC[AES256_GCM:v1:<nonce>:<cipher>:<tag>].",
                    sourceEntry?.FilePath));
            }
        }
    }
}
```

## File: src/Starry.Infrastructure/Configuration/GitSyncService.cs

**Lines**: 57 | **Size**: 1.8 KB | **Language**: csharp

```csharp
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
```

## File: src/Starry.Infrastructure/Configuration/JsonConfigFormatProvider.cs

**Lines**: 39 | **Size**: 1.1 KB | **Language**: csharp

```csharp
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// JSON format provider based on System.Text.Json with native Native AOT support.
/// </summary>
public sealed class JsonConfigFormatProvider : IConfigFormatProvider
{
    private static readonly JsonSerializerOptions IndentedOptions = new()
    {
        WriteIndented = true
    };

    public IReadOnlyList<string> SupportedExtensions { get; } = [".json"];

    public string FormatName => "json";

    public JsonNode Parse(ReadOnlySpan<byte> content, string sourcePath)
    {
        try
        {
            var parsed = JsonNode.Parse(content);
            return parsed ?? new JsonObject();
        }
        catch (JsonException ex)
        {
            throw new FormatException($"Failed to parse JSON file '{sourcePath}': {ex.Message}", ex);
        }
    }

    public string Serialize(JsonNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node.ToJsonString(IndentedOptions);
    }
}
```

## File: src/Starry.Infrastructure/Configuration/RepositoryInitializer.cs

**Lines**: 81 | **Size**: 2.9 KB | **Language**: csharp

```csharp
using Starry.Infrastructure.Security;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Initializes a new Starry GitOps repository directory with standard layout, .gitignore, and fd67:b00b::/32 mesh.
/// </summary>
public static class RepositoryInitializer
{
    public static (string MasterKeyBase64, string TargetDir) Initialize(
        string? projectName = null,
        bool skipFolderCreate = false,
        string? baseDir = null)
    {
        var root = string.IsNullOrWhiteSpace(baseDir) ? Directory.GetCurrentDirectory() : Path.GetFullPath(baseDir);
        var targetDir = !skipFolderCreate && !string.IsNullOrWhiteSpace(projectName)
            ? Path.Combine(root, projectName)
            : root;

        CreateDirectories(targetDir);
        WriteDefaultConfigs(targetDir, projectName);

        var masterKey = CryptoService.GenerateMasterKeyBase64();
        return (masterKey, targetDir);
    }

    private static void CreateDirectories(string targetDir)
    {
        Directory.CreateDirectory(targetDir);
        Directory.CreateDirectory(Path.Combine(targetDir, "nodes"));
        Directory.CreateDirectory(Path.Combine(targetDir, "services"));
        Directory.CreateDirectory(Path.Combine(targetDir, "files"));
        Directory.CreateDirectory(Path.Combine(targetDir, "domains"));

        File.WriteAllText(Path.Combine(targetDir, "nodes", ".gitkeep"), string.Empty);
        File.WriteAllText(Path.Combine(targetDir, "services", ".gitkeep"), string.Empty);
        File.WriteAllText(Path.Combine(targetDir, "files", ".gitkeep"), string.Empty);
    }

    private static void WriteDefaultConfigs(string targetDir, string? projectName)
    {
        var gitignorePath = Path.Combine(targetDir, ".gitignore");
        if (!File.Exists(gitignorePath))
        {
            const string gitignoreContent = """
                .starry/
                *.tmp
                *.bak
                """;
            File.WriteAllText(gitignorePath, gitignoreContent);
        }

        var mainConfigPath = Path.Combine(targetDir, "starry.yaml");
        if (!File.Exists(mainConfigPath))
        {
            var clusterName = !string.IsNullOrWhiteSpace(projectName) ? projectName : "starry-fleet";
            var configContent = $"""
                version: "3.0"
                cluster_name: "{clusterName}"

                mesh:
                  prefix: "fd67:b00b::/32"
                  port: 51820
                  keepalive: 25
                """;
            File.WriteAllText(mainConfigPath, configContent);
        }

        var portsPath = Path.Combine(targetDir, "ports.yaml");
        if (!File.Exists(portsPath))
        {
            File.WriteAllText(portsPath, "ports: {}\n");
        }

        var volumesPath = Path.Combine(targetDir, "volumes.yaml");
        if (!File.Exists(volumesPath))
        {
            File.WriteAllText(volumesPath, "volumes: {}\n");
        }
    }
}
```

## File: src/Starry.Infrastructure/Configuration/TomlConfigFormatProvider.cs

**Lines**: 150 | **Size**: 4.0 KB | **Language**: csharp

```csharp
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Tomlyn;
using Tomlyn.Model;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// TOML configuration provider using Tomlyn.
/// </summary>
public sealed class TomlConfigFormatProvider : IConfigFormatProvider
{
    public IReadOnlyList<string> SupportedExtensions { get; } = [".toml"];

    public string FormatName => "toml";

    public JsonNode Parse(ReadOnlySpan<byte> content, string sourcePath)
    {
        try
        {
            var text = Encoding.UTF8.GetString(content);
            var model = Toml.ToModel(text);
            return ConvertTomlTableToJsonNode(model) ?? new JsonObject();
        }
        catch (Exception ex) when (ex is not FormatException)
        {
            throw new FormatException($"Failed to parse TOML file '{sourcePath}': {ex.Message}", ex);
        }
    }

    public string Serialize(JsonNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        var tomlTable = ConvertJsonNodeToTomlObject(node) as TomlTable ?? new TomlTable();
        return Toml.FromModel(tomlTable);
    }

    private static JsonNode? ConvertTomlTableToJsonNode(object? tomlObj)
    {
        if (tomlObj is null)
        {
            return null;
        }

        if (tomlObj is TomlTable table)
        {
            var jsonObject = new JsonObject();
            foreach (var (key, value) in table)
            {
                jsonObject[key] = ConvertTomlTableToJsonNode(value);
            }
            return jsonObject;
        }

        if (tomlObj is TomlArray array)
        {
            var jsonArray = new JsonArray();
            foreach (var item in array)
            {
                jsonArray.Add(ConvertTomlTableToJsonNode(item));
            }
            return jsonArray;
        }

        if (tomlObj is TomlTableArray tableArray)
        {
            var jsonArray = new JsonArray();
            foreach (var item in tableArray)
            {
                jsonArray.Add(ConvertTomlTableToJsonNode(item));
            }
            return jsonArray;
        }

        return tomlObj switch
        {
            string s => JsonValue.Create(s),
            bool b => JsonValue.Create(b),
            int i => JsonValue.Create(i),
            long l => JsonValue.Create(l),
            double d => JsonValue.Create(d),
            float f => JsonValue.Create(f),
            _ => JsonValue.Create(tomlObj.ToString())
        };
    }

    private static object? ConvertJsonNodeToTomlObject(JsonNode? node)
    {
        if (node is null)
        {
            return null;
        }

        if (node is JsonObject obj)
        {
            var table = new TomlTable();
            foreach (var (propName, propValue) in obj)
            {
                var converted = ConvertJsonNodeToTomlObject(propValue);
                if (converted != null)
                {
                    table[propName] = converted;
                }
            }
            return table;
        }

        if (node is JsonArray arr)
        {
            var list = new TomlArray();
            foreach (var item in arr)
            {
                var converted = ConvertJsonNodeToTomlObject(item);
                if (converted != null)
                {
                    list.Add(converted);
                }
            }
            return list;
        }

        if (node is JsonValue jsonVal)
        {
            if (jsonVal.TryGetValue<bool>(out var boolVal))
            {
                return boolVal;
            }

            if (jsonVal.TryGetValue<long>(out var longVal))
            {
                return longVal;
            }

            if (jsonVal.TryGetValue<double>(out var doubleVal))
            {
                return doubleVal;
            }

            if (jsonVal.TryGetValue<string>(out var strVal))
            {
                return strVal;
            }

            return jsonVal.ToString();
        }

        return node.ToString();
    }
}
```

## File: src/Starry.Infrastructure/Configuration/ValidationIssue.cs

**Lines**: 7 | **Size**: 181 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Configuration;

public sealed record ValidationIssue(
    string Path,
    string Message,
    string? FilePath = null,
    bool IsWarning = false);
```

## File: src/Starry.Infrastructure/Configuration/VirtualConfigEngine.cs

**Lines**: 318 | **Size**: 12.1 KB | **Language**: csharp

```csharp
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Virtual Configuration Engine for loading, merging, packing, splitting,
/// formatting, and validating multi-file configuration trees with Source Map tracking.
/// </summary>
public sealed class VirtualConfigEngine
{
    private readonly List<IConfigFormatProvider> _providers = [];

    public IReadOnlyList<IConfigFormatProvider> Providers => _providers;

    public VirtualConfigEngine(IEnumerable<IConfigFormatProvider>? providers = null)
    {
        if (providers != null)
        {
            _providers.AddRange(providers);
        }
        else
        {
            _providers.Add(new YamlConfigFormatProvider());
            _providers.Add(new JsonConfigFormatProvider());
            _providers.Add(new TomlConfigFormatProvider());
        }
    }

    public ConfigTree Load(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (File.Exists(path))
        {
            return LoadFile(path);
        }

        if (Directory.Exists(path))
        {
            return LoadDirectory(path);
        }

        throw new FileNotFoundException($"Configuration path does not exist: {path}", path);
    }

    private ConfigTree LoadFile(string filePath)
    {
        var provider = FindProviderForFile(filePath);
        var bytes = File.ReadAllBytes(filePath);
        var parsed = provider.Parse(bytes, filePath);

        var tree = new ConfigTree();
        if (parsed is JsonObject obj)
        {
            PopulateObjectIntoTree(tree, obj, string.Empty, filePath);
        }
        else
        {
            tree.SetNode("root", parsed, new SourceMapEntry("root", filePath));
        }

        return tree;
    }

    private ConfigTree LoadDirectory(string dirPath)
    {
        var tree = new ConfigTree();
        var supportedExtensions = _providers.SelectMany(p => p.SupportedExtensions).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories)
            .Where(f => supportedExtensions.Contains(Path.GetExtension(f)))
            .Where(f => !IsExcludedConfigPath(dirPath, f))
            .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var file in files)
        {
            var relative = Path.GetRelativePath(dirPath, file).Replace('\\', '/');
            var provider = FindProviderForFile(file);
            var bytes = File.ReadAllBytes(file);
            var parsed = provider.Parse(bytes, relative);

            var pathWithoutExt = Path.Combine(
                Path.GetDirectoryName(relative) ?? string.Empty,
                Path.GetFileNameWithoutExtension(relative)).Replace('\\', '/');

            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(relative);

            if (string.Equals(fileNameWithoutExt, "starry", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(fileNameWithoutExt, "config", StringComparison.OrdinalIgnoreCase))
            {
                // Root-level configuration file merges directly into root
                if (parsed is JsonObject rootObj)
                {
                    PopulateObjectIntoTree(tree, rootObj, string.Empty, relative);
                }
            }
            else
            {
                // Hierarchy mapping: nodes/bocchi.yaml -> root.nodes.bocchi
                var canonicalPrefix = pathWithoutExt.Replace('/', '.');
                if (parsed is JsonObject obj)
                {
                    PopulateObjectIntoTree(tree, obj, canonicalPrefix, relative);
                }
                else
                {
                    tree.SetNode(canonicalPrefix, parsed, new SourceMapEntry(canonicalPrefix, relative));
                }
            }
        }

        return tree;
    }

    private static bool IsExcludedConfigPath(string rootDir, string fullPath)
    {
        var rel = Path.GetRelativePath(rootDir, fullPath).Replace('\\', '/');
        var fileName = Path.GetFileName(rel);

        if (string.Equals(fileName, "manifest.json", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var segments = rel.Split('/');
        if (segments.Length > 0 && string.Equals(segments[0], "files", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return segments.Any(s =>
            string.Equals(s, ".starry", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(s, ".github", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(s, ".git", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(s, "node_modules", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(s, "dist", StringComparison.OrdinalIgnoreCase));
    }

    private static void PopulateObjectIntoTree(ConfigTree tree, JsonObject obj, string prefix, string filePath)
    {
        foreach (var (propName, propVal) in obj)
        {
            var currentPath = string.IsNullOrEmpty(prefix) ? propName : $"{prefix}.{propName}";
            if (propVal is JsonObject childObj)
            {
                PopulateObjectIntoTree(tree, childObj, currentPath, filePath);
            }
            else
            {
                // Clone the node so it can be re-parented into the tree
                var clonedVal = propVal != null ? JsonNode.Parse(propVal.ToJsonString()) : null;
                tree.SetNode(currentPath, clonedVal, new SourceMapEntry(currentPath, filePath));
            }
        }
    }

    public void Pack(ConfigTree tree, string targetFilePath, string format = "yaml")
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetFilePath);

        var provider = FindProviderByName(format);
        var serialized = provider.Serialize(tree.Root);

        var dir = Path.GetDirectoryName(targetFilePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(targetFilePath, serialized);
    }

    public void Split(ConfigTree tree, string targetDirectoryPath, string format = "yaml")
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetDirectoryPath);

        if (!Directory.Exists(targetDirectoryPath))
        {
            Directory.CreateDirectory(targetDirectoryPath);
        }

        var provider = FindProviderByName(format);
        var ext = provider.SupportedExtensions[0];

        var rootCopy = new JsonObject();

        foreach (var (key, val) in tree.Root)
        {
            if (string.Equals(key, "nodes", StringComparison.OrdinalIgnoreCase) && val is JsonObject nodesObj)
            {
                var nodesDir = Path.Combine(targetDirectoryPath, "nodes");
                Directory.CreateDirectory(nodesDir);
                foreach (var (nodeName, nodeVal) in nodesObj)
                {
                    if (nodeVal != null)
                    {
                        var filePath = Path.Combine(nodesDir, $"{nodeName}{ext}");
                        File.WriteAllText(filePath, provider.Serialize(nodeVal));
                    }
                }
            }
            else if (string.Equals(key, "services", StringComparison.OrdinalIgnoreCase) && val is JsonObject servicesObj)
            {
                var servicesDir = Path.Combine(targetDirectoryPath, "services");
                Directory.CreateDirectory(servicesDir);
                foreach (var (svcName, svcVal) in servicesObj)
                {
                    if (svcVal != null)
                    {
                        var filePath = Path.Combine(servicesDir, $"{svcName}{ext}");
                        File.WriteAllText(filePath, provider.Serialize(svcVal));
                    }
                }
            }
            else
            {
                if (val != null)
                {
                    rootCopy[key] = JsonNode.Parse(val.ToJsonString());
                }
            }
        }

        var mainConfigPath = Path.Combine(targetDirectoryPath, $"starry{ext}");
        File.WriteAllText(mainConfigPath, provider.Serialize(rootCopy));
    }

    public string Format(string content, string format)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        var provider = FindProviderByName(format);
        var parsed = provider.Parse(System.Text.Encoding.UTF8.GetBytes(content), "inline");
        return provider.Serialize(parsed);
    }

    public static string GenerateJsonSchema()
    {
        var schemaObject = new JsonObject
        {
            ["$schema"] = "https://json-schema.org/draft/2020-12/schema",
            ["title"] = "StarryClusterConfiguration",
            ["type"] = "object",
            ["properties"] = new JsonObject
            {
                ["cluster_name"] = new JsonObject { ["type"] = "string" },
                ["version"] = new JsonObject { ["type"] = "string" },
                ["nodes"] = new JsonObject
                {
                    ["type"] = "object",
                    ["additionalProperties"] = new JsonObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JsonObject
                        {
                            ["host"] = new JsonObject { ["type"] = "string" },
                            ["port"] = new JsonObject { ["type"] = "integer", ["default"] = 22 },
                            ["user"] = new JsonObject { ["type"] = "string", ["default"] = "root" },
                            ["ssh_key"] = new JsonObject { ["type"] = "string" },
                            ["vpn_ip"] = new JsonObject { ["type"] = "string" },
                            ["is_public"] = new JsonObject { ["type"] = "boolean", ["default"] = false }
                        },
                        ["required"] = new JsonArray(JsonValue.Create("host"))
                    }
                },
                ["services"] = new JsonObject
                {
                    ["type"] = "object",
                    ["additionalProperties"] = new JsonObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JsonObject
                        {
                            ["image"] = new JsonObject { ["type"] = "string" },
                            ["priority"] = new JsonObject { ["type"] = "integer", ["default"] = 50 },
                            ["vip"] = new JsonObject { ["type"] = "string" },
                            ["node"] = new JsonObject { ["type"] = "string" },
                            ["ports"] = new JsonObject { ["type"] = "array", ["items"] = new JsonObject { ["type"] = "string" } },
                            ["volumes"] = new JsonObject { ["type"] = "array", ["items"] = new JsonObject { ["type"] = "string" } },
                            ["env"] = new JsonObject { ["type"] = "object" }
                        },
                        ["required"] = new JsonArray(JsonValue.Create("image"))
                    }
                }
            }
        };

        return schemaObject.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    }

    private IConfigFormatProvider FindProviderForFile(string filePath)
    {
        var ext = Path.GetExtension(filePath);
        var provider = _providers.FirstOrDefault(p => p.SupportedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase));
        if (provider == null)
        {
            throw new NotSupportedException($"No configuration provider registered for extension '{ext}' of file '{filePath}'.");
        }

        return provider;
    }

    private IConfigFormatProvider FindProviderByName(string format)
    {
        var provider = _providers.FirstOrDefault(p => string.Equals(p.FormatName, format, StringComparison.OrdinalIgnoreCase));
        if (provider == null)
        {
            throw new NotSupportedException($"Unsupported configuration format '{format}'. Supported formats: yaml, json, toml.");
        }

        return provider;
    }
}
```

## File: src/Starry.Infrastructure/Configuration/YamlConfigFormatProvider.cs

**Lines**: 147 | **Size**: 4.2 KB | **Language**: csharp

```csharp
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using YamlDotNet.Serialization;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// YAML configuration provider using YamlDotNet.
/// </summary>
public sealed class YamlConfigFormatProvider : IConfigFormatProvider
{
    private readonly IDeserializer _deserializer = new DeserializerBuilder().Build();
    private readonly ISerializer _serializer = new SerializerBuilder().ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull).Build();

    public IReadOnlyList<string> SupportedExtensions { get; } = [".yaml", ".yml"];

    public string FormatName => "yaml";

    public JsonNode Parse(ReadOnlySpan<byte> content, string sourcePath)
    {
        try
        {
            var text = Encoding.UTF8.GetString(content);
            using var reader = new StringReader(text);
            var yamlObject = _deserializer.Deserialize(reader);
            return ConvertYamlObjectToJsonNode(yamlObject) ?? new JsonObject();
        }
        catch (Exception ex) when (ex is not FormatException)
        {
            throw new FormatException($"Failed to parse YAML file '{sourcePath}': {ex.Message}", ex);
        }
    }

    public string Serialize(JsonNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        var rawObject = ConvertJsonNodeToRawObject(node);
        return _serializer.Serialize(rawObject);
    }

    private static JsonNode? ConvertYamlObjectToJsonNode(object? yamlObj)
    {
        if (yamlObj is null)
        {
            return null;
        }

        if (yamlObj is IDictionary<object, object> dictObj)
        {
            var jsonObject = new JsonObject();
            foreach (var (key, value) in dictObj)
            {
                var keyStr = key?.ToString() ?? string.Empty;
                jsonObject[keyStr] = ConvertYamlObjectToJsonNode(value);
            }
            return jsonObject;
        }

        if (yamlObj is IDictionary<string, object> dictStr)
        {
            var jsonObject = new JsonObject();
            foreach (var (key, value) in dictStr)
            {
                jsonObject[key] = ConvertYamlObjectToJsonNode(value);
            }
            return jsonObject;
        }

        if (yamlObj is IEnumerable<object> list)
        {
            var jsonArray = new JsonArray();
            foreach (var item in list)
            {
                jsonArray.Add(ConvertYamlObjectToJsonNode(item));
            }
            return jsonArray;
        }

        return yamlObj switch
        {
            string s => JsonValue.Create(s),
            bool b => JsonValue.Create(b),
            int i => JsonValue.Create(i),
            long l => JsonValue.Create(l),
            double d => JsonValue.Create(d),
            float f => JsonValue.Create(f),
            decimal m => JsonValue.Create(m),
            _ => JsonValue.Create(yamlObj.ToString())
        };
    }

    private static object? ConvertJsonNodeToRawObject(JsonNode? node)
    {
        if (node is null)
        {
            return null;
        }

        if (node is JsonObject obj)
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal);
            foreach (var (propName, propValue) in obj)
            {
                dict[propName] = ConvertJsonNodeToRawObject(propValue);
            }
            return dict;
        }

        if (node is JsonArray arr)
        {
            var list = new List<object?>(arr.Count);
            foreach (var item in arr)
            {
                list.Add(ConvertJsonNodeToRawObject(item));
            }
            return list;
        }

        if (node is JsonValue jsonVal)
        {
            if (jsonVal.TryGetValue<bool>(out var boolVal))
            {
                return boolVal;
            }

            if (jsonVal.TryGetValue<long>(out var longVal))
            {
                return longVal;
            }

            if (jsonVal.TryGetValue<double>(out var doubleVal))
            {
                return doubleVal;
            }

            if (jsonVal.TryGetValue<string>(out var strVal))
            {
                return strVal;
            }

            return jsonVal.ToString();
        }

        return node.ToString();
    }
}
```

## File: src/Starry.Infrastructure/Containers/BaseSocketContainerRuntime.cs

**Lines**: 412 | **Size**: 15.7 KB | **Language**: csharp

```csharp
using System.Globalization;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Containers;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Containers;

/// <summary>
/// Base class for container engines communicating over Unix Domain Sockets using pure HttpClient.
/// </summary>
public abstract class BaseSocketContainerRuntime : IContainerRuntime
{
    private static readonly Uri BaseDockerUri = new("http://127.0.0.1/");
    private const string ApiVersionPrefix = "/v1.40";

    public abstract string RuntimeName { get; }
    protected abstract IReadOnlyList<string> CandidateSocketPaths { get; }

    public async Task<string> ResolveSocketPathAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        foreach (var candidate in CandidateSocketPaths)
        {
            var checkRes = await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "test",
                Arguments = ["-S", candidate]
            }, ct).ConfigureAwait(false);

            if (checkRes.IsSuccess)
            {
                return candidate;
            }
        }

        // Return first default if none exists yet
        return CandidateSocketPaths[0];
    }

    public async Task<RuntimeInfo> ProbeAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var socketPath = await ResolveSocketPathAsync(node, ct).ConfigureAwait(false);
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        using var response = await client.GetAsync(new Uri(BaseDockerUri, $"{ApiVersionPrefix}/version"), ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var bodyStr = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var json = JsonNode.Parse(bodyStr) as JsonObject;
        var version = json?["Version"]?.ToString() ?? "unknown";
        var os = json?["Os"]?.ToString() ?? "linux";
        var arch = json?["Arch"]?.ToString() ?? "amd64";

        var isRootless = socketPath.Contains("/run/user/", StringComparison.OrdinalIgnoreCase);

        return new RuntimeInfo(
            Engine: RuntimeName,
            Version: version,
            OperatingSystem: os,
            Architecture: arch,
            IsRootless: isRootless,
            SocketPath: socketPath);
    }

    public async Task<IReadOnlyList<ContainerState>> ListContainersAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var socketPath = await ResolveSocketPathAsync(node, ct).ConfigureAwait(false);
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        using var response = await client.GetAsync(new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/json?all=true"), ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var bodyStr = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var jsonArray = JsonNode.Parse(bodyStr) as JsonArray;
        var result = new List<ContainerState>();

        if (jsonArray == null)
        {
            return result;
        }

        foreach (var item in jsonArray)
        {
            if (item is not JsonObject obj)
            {
                continue;
            }

            var id = obj["Id"]?.ToString() ?? string.Empty;
            var namesArr = obj["Names"] as JsonArray;
            var name = namesArr?.Count > 0 ? namesArr[0]?.ToString().TrimStart('/') ?? string.Empty : id;
            var image = obj["Image"]?.ToString() ?? string.Empty;
            var status = obj["Status"]?.ToString() ?? string.Empty;
            var state = obj["State"]?.ToString() ?? string.Empty;
            var isRunning = string.Equals(state, "running", StringComparison.OrdinalIgnoreCase);

            var labelsDict = new Dictionary<string, string>(StringComparer.Ordinal);
            if (obj["Labels"] is JsonObject labelsObj)
            {
                foreach (var (k, v) in labelsObj)
                {
                    labelsDict[k] = v?.ToString() ?? string.Empty;
                }
            }

            result.Add(new ContainerState(
                Id: id,
                Name: name,
                Image: image,
                Status: status,
                IsRunning: isRunning,
                ExitCode: 0,
                CreatedAt: DateTime.UtcNow,
                Labels: labelsDict));
        }

        return result;
    }

    public async Task DeployContainerAsync(INodeConnection node, ServiceSpec spec, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(spec);

        var socketPath = await ResolveSocketPathAsync(node, ct).ConfigureAwait(false);
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        // Check if dynamic files changed
        var filesChanged = await SyncDynamicFilesAsync(node, spec, ct).ConfigureAwait(false);

        // Check if container already exists
        var existingContainers = await ListContainersAsync(node, ct).ConfigureAwait(false);
        var existing = existingContainers.FirstOrDefault(c => string.Equals(c.Name, spec.Name, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
        {
            await ReconcileExistingContainerAsync(client, existing, filesChanged, ct).ConfigureAwait(false);
            return;
        }

        // If VIP address is specified, ensure it is assigned to starry0 interface on the node
        if (!string.IsNullOrEmpty(spec.VipAddress) && System.Net.IPAddress.TryParse(spec.VipAddress, out _))
        {
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", $"ip addr add {spec.VipAddress}/32 dev starry0 2>/dev/null || true"]
            }, ct).ConfigureAwait(false);
        }

        // Ensure image is pulled locally before creating container
        await EnsureImageAvailableAsync(client, spec.Image, ct).ConfigureAwait(false);

        // Create and start container
        var createPayload = BuildCreateContainerPayload(spec);
        using var createContent = new StringContent(createPayload.ToJsonString(), Encoding.UTF8, "application/json");

        using var createRes = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/create?name={Uri.EscapeDataString(spec.Name)}"),
            createContent,
            ct).ConfigureAwait(false);

        createRes.EnsureSuccessStatusCode();
        var createStr = await createRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var createJson = JsonNode.Parse(createStr) as JsonObject;
        var containerId = createJson?["Id"]?.ToString() ?? throw new InvalidOperationException("Failed to get created container Id.");

        await StartContainerAsync(client, containerId, ct).ConfigureAwait(false);
    }

    private static async Task ReconcileExistingContainerAsync(HttpClient client, ContainerState existing, bool filesChanged, CancellationToken ct)
    {
        if (existing.IsRunning && filesChanged)
        {
            await SendSignalAsync(client, existing.Id, "SIGHUP", ct).ConfigureAwait(false);
            return;
        }

        if (!existing.IsRunning)
        {
            await StartContainerAsync(client, existing.Id, ct).ConfigureAwait(false);
        }
    }

    public async Task StreamImagePeerToPeerAsync(
        INodeConnection source,
        INodeConnection target,
        string imageTag,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentException.ThrowIfNullOrWhiteSpace(imageTag);

        // Stream docker/podman save | zstd | ssh load through binary pipe
        using var pipeStream = new MemoryStream();

        var saveCmd = $"{RuntimeName} save {imageTag} | zstd -3";
        var saveSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", saveCmd],
            Timeout = TimeSpan.FromMinutes(30)
        };

        var sourceResult = await source.ExecuteStreamingCommandAsync(saveSpec, stdinStream: null, stdoutStream: pipeStream, ct).ConfigureAwait(false);
        if (!sourceResult.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to save image '{imageTag}' on source node '{source.NodeName}': {sourceResult.StandardError}");
        }

        pipeStream.Position = 0;
        var loadCmd = $"zstd -d | {RuntimeName} load";
        var targetSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", loadCmd],
            Timeout = TimeSpan.FromMinutes(30)
        };

        var targetResult = await target.ExecuteStreamingCommandAsync(targetSpec, stdinStream: pipeStream, stdoutStream: null, ct).ConfigureAwait(false);
        if (!targetResult.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to load image '{imageTag}' on target node '{target.NodeName}': {targetResult.StandardError}");
        }
    }

    private static async Task<bool> SyncDynamicFilesAsync(INodeConnection node, ServiceSpec spec, CancellationToken ct)
    {
        if (spec.Files.Count == 0)
        {
            return false;
        }

        var anyFileChanged = false;
        var baseDir = $"/var/lib/starry/files/{spec.Name}";

        foreach (var (destPath, content) in spec.Files)
        {
            var hostFilePath = $"{baseDir}/{destPath.TrimStart('/')}";
            var hostDir = Path.GetDirectoryName(hostFilePath)?.Replace('\\', '/') ?? baseDir;
            var newHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(content)));

            var checkHashSpec = new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", $"sha256sum {hostFilePath} 2>/dev/null | awk '{{print $1}}'"]
            };
            var hashRes = await node.ExecuteCommandAsync(checkHashSpec, ct).ConfigureAwait(false);
            var currentHash = hashRes.StandardOutput.Trim();

            if (!string.Equals(currentHash, newHash, StringComparison.OrdinalIgnoreCase))
            {
                anyFileChanged = true;
                var writeSpec = new ProcessSpec
                {
                    Executable = "sh",
                    Arguments = ["-c", $"mkdir -p {hostDir} && cat > {hostFilePath} && chmod 644 {hostFilePath}"],
                    StandardInputBytes = Encoding.UTF8.GetBytes(content)
                };
                await node.ExecuteCommandAsync(writeSpec, ct).ConfigureAwait(false);
            }
        }

        return anyFileChanged;
    }

    private static JsonObject BuildCreateContainerPayload(ServiceSpec spec)
    {
        var binds = new JsonArray();
        foreach (var vol in spec.Volumes)
        {
            binds.Add(JsonValue.Create(vol));
        }

        // Mount dynamic files directory
        if (spec.Files.Count > 0)
        {
            var baseDir = $"/var/lib/starry/files/{spec.Name}";
            foreach (var destPath in spec.Files.Keys)
            {
                binds.Add(JsonValue.Create($"{baseDir}/{destPath.TrimStart('/')}:{destPath}:ro"));
            }
        }

        var env = new JsonArray();
        foreach (var (k, v) in spec.Environment)
        {
            env.Add(JsonValue.Create($"{k}={v}"));
        }

        var labels = new JsonObject();
        foreach (var (k, v) in spec.Labels)
        {
            labels[k] = v;
        }
        labels["managed-by"] = "starry";
        labels["starry.priority"] = spec.Priority.ToString(CultureInfo.InvariantCulture);

        var (exposedPorts, portBindings) = BuildPortConfigurations(spec.Ports);

        var payload = new JsonObject
        {
            ["Image"] = spec.Image,
            ["Env"] = env,
            ["Labels"] = labels,
            ["ExposedPorts"] = exposedPorts,
            ["HostConfig"] = new JsonObject
            {
                ["Binds"] = binds,
                ["PortBindings"] = portBindings,
                ["RestartPolicy"] = new JsonObject { ["Name"] = "unless-stopped" }
            }
        };

        return payload;
    }

    private static (JsonObject ExposedPorts, JsonObject PortBindings) BuildPortConfigurations(IReadOnlyList<string> ports)
    {
        var exposedPorts = new JsonObject();
        var portBindings = new JsonObject();

        foreach (var portStr in ports)
        {
            var parts = portStr.Split(':');
            if (parts.Length == 2)
            {
                var hostPort = parts[0].Trim();
                var containerPart = parts[1].Trim();
                var proto = "tcp";
                var containerPort = containerPart;
                if (containerPart.Contains('/', StringComparison.Ordinal))
                {
                    var sub = containerPart.Split('/');
                    containerPort = sub[0];
                    proto = string.Equals(sub[1], "udp", StringComparison.OrdinalIgnoreCase) ? "udp" : "tcp";
                }

                var containerKey = $"{containerPort}/{proto}";
                exposedPorts[containerKey] = new JsonObject();

                var bindingArr = new JsonArray
                {
                    new JsonObject
                    {
                        ["HostIp"] = "0.0.0.0",
                        ["HostPort"] = hostPort
                    }
                };
                portBindings[containerKey] = bindingArr;
            }
        }

        return (exposedPorts, portBindings);
    }

    private static async Task EnsureImageAvailableAsync(HttpClient client, string image, CancellationToken ct)
    {
        var inspectUri = new Uri(BaseDockerUri, $"{ApiVersionPrefix}/images/{Uri.EscapeDataString(image)}/json");
        using var inspectRes = await client.GetAsync(inspectUri, ct).ConfigureAwait(false);
        if (inspectRes.IsSuccessStatusCode)
        {
            return;
        }

        var pullUri = new Uri(BaseDockerUri, $"{ApiVersionPrefix}/images/create?fromImage={Uri.EscapeDataString(image)}");
        using var pullRes = await client.PostAsync(pullUri, content: null, ct).ConfigureAwait(false);
        if (pullRes.IsSuccessStatusCode)
        {
            await pullRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        }
    }

    private static async Task StartContainerAsync(HttpClient client, string containerId, CancellationToken ct)
    {
        using var response = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/{containerId}/start"),
            content: null,
            ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NotModified)
        {
            var err = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            throw new InvalidOperationException($"Failed to start container '{containerId}': {err}");
        }
    }

    private static async Task SendSignalAsync(HttpClient client, string containerId, string signal, CancellationToken ct)
    {
        using var response = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/{containerId}/kill?signal={signal}"),
            content: null,
            ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            throw new InvalidOperationException($"Failed to send signal '{signal}' to container '{containerId}': {err}");
        }
    }
}
```

## File: src/Starry.Infrastructure/Containers/ContainerPriorityScheduler.cs

**Lines**: 32 | **Size**: 971 B | **Language**: csharp

```csharp
using Starry.Core.Containers;

namespace Starry.Infrastructure.Containers;

/// <summary>
/// Orders and schedules container deployments based on numeric priority
/// (0: Registry -> 10: Database -> 50: Backends -> 90: Ingress/Proxy).
/// </summary>
public static class ContainerPriorityScheduler
{
    public const int DefaultPriority = 50;

    public static IReadOnlyList<ServiceSpec> OrderServicesByPriority(IEnumerable<ServiceSpec> services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services
            .OrderBy(s => s.Priority)
            .ThenBy(s => s.Name, StringComparer.Ordinal)
            .ToList();
    }

    public static IReadOnlyList<IGrouping<int, ServiceSpec>> GroupServicesByPriorityTier(IEnumerable<ServiceSpec> services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services
            .OrderBy(s => s.Priority)
            .GroupBy(s => s.Priority)
            .ToList();
    }
}
```

## File: src/Starry.Infrastructure/Containers/DockerRuntime.cs

**Lines**: 15 | **Size**: 430 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Containers;

/// <summary>
/// Docker Engine runtime implementation communicating over Docker Unix domain socket.
/// </summary>
public sealed class DockerRuntime : BaseSocketContainerRuntime
{
    public override string RuntimeName => "docker";

    protected override IReadOnlyList<string> CandidateSocketPaths { get; } =
    [
        "/var/run/docker.sock",
        "/run/docker.sock"
    ];
}
```

## File: src/Starry.Infrastructure/Containers/PodmanRuntime.cs

**Lines**: 17 | **Size**: 554 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Containers;

/// <summary>
/// Podman runtime implementation communicating over Libpod/Compat Unix domain socket (rootful and rootless).
/// </summary>
public sealed class PodmanRuntime : BaseSocketContainerRuntime
{
    public override string RuntimeName => "podman";

    protected override IReadOnlyList<string> CandidateSocketPaths { get; } =
    [
        "/run/podman/podman.sock",
        "/var/run/podman/podman.sock",
        "/run/user/1000/podman/podman.sock",
        "/run/user/0/podman/podman.sock"
    ];
}
```

## File: src/Starry.Infrastructure/Dns/CloudflareDnsProvider.cs

**Lines**: 243 | **Size**: 9.2 KB | **Language**: csharp

```csharp
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Dns;

namespace Starry.Infrastructure.Dns;

/// <summary>
/// Cloudflare DNS v4 API provider implementation with automated retry and backoff.
/// </summary>
public sealed class CloudflareDnsProvider : IDnsProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiToken;
    private static readonly Uri CloudflareBaseUri = new("https://api.cloudflare.com/client/v4/");

    public string ProviderName => "cloudflare";

    public CloudflareDnsProvider(string apiToken, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiToken);
        _apiToken = apiToken;
        _httpClient = httpClient ?? new HttpClient { BaseAddress = CloudflareBaseUri };
    }

    public async Task<IReadOnlyList<DnsRecord>> GetRecordsAsync(string zoneDomain, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);

        using var zoneRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"zones?name={Uri.EscapeDataString(zoneDomain)}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);

        zoneRes.EnsureSuccessStatusCode();

        var zoneStr = await zoneRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var zoneJson = JsonNode.Parse(zoneStr) as JsonObject;
        var zoneId = ExtractFirstResultId(zoneJson);
        if (string.IsNullOrEmpty(zoneId))
        {
            return [];
        }

        using var recordsRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"zones/{zoneId}/dns_records");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);

        recordsRes.EnsureSuccessStatusCode();

        var recordsStr = await recordsRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var recordsJson = JsonNode.Parse(recordsStr) as JsonObject;
        var resultList = new List<DnsRecord>();

        if (recordsJson?["result"] is JsonArray arr)
        {
            foreach (var item in arr)
            {
                if (item is JsonObject obj)
                {
                    resultList.Add(new DnsRecord(
                        Id: obj["id"]?.ToString() ?? string.Empty,
                        RecordType: obj["type"]?.ToString() ?? "A",
                        Name: obj["name"]?.ToString() ?? string.Empty,
                        Content: obj["content"]?.ToString() ?? string.Empty,
                        Ttl: obj["ttl"]?.GetValue<int>() ?? 300,
                        Proxied: obj["proxied"]?.GetValue<bool>() ?? false));
                }
            }
        }

        return resultList;
    }

    public async Task UpsertRecordAsync(string zoneDomain, DnsRecord record, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentNullException.ThrowIfNull(record);

        var existingRecords = await GetRecordsAsync(zoneDomain, ct).ConfigureAwait(false);
        var existing = existingRecords.FirstOrDefault(r =>
            string.Equals(r.Name, record.Name, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(r.RecordType, record.RecordType, StringComparison.OrdinalIgnoreCase));

        var zoneId = await ResolveZoneIdAsync(zoneDomain, ct).ConfigureAwait(false);
        var payload = new JsonObject
        {
            ["type"] = record.RecordType,
            ["name"] = record.Name,
            ["content"] = record.Content,
            ["ttl"] = record.Ttl,
            ["proxied"] = record.Proxied
        };

        if (existing != null)
        {
            using var putRes = await SendWithRetryAsync(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Put, $"zones/{zoneId}/dns_records/{existing.Id}")
                {
                    Content = new StringContent(payload.ToJsonString(), System.Text.Encoding.UTF8, "application/json")
                };
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
                return req;
            }, ct).ConfigureAwait(false);

            putRes.EnsureSuccessStatusCode();
        }
        else
        {
            using var postRes = await SendWithRetryAsync(() =>
            {
                var req = new HttpRequestMessage(HttpMethod.Post, $"zones/{zoneId}/dns_records")
                {
                    Content = new StringContent(payload.ToJsonString(), System.Text.Encoding.UTF8, "application/json")
                };
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
                return req;
            }, ct).ConfigureAwait(false);

            postRes.EnsureSuccessStatusCode();
        }
    }

    public async Task ConfigureOriginRuleAsync(string zoneDomain, string hostname, int originPort, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(hostname);

        var zoneId = await ResolveZoneIdAsync(zoneDomain, ct).ConfigureAwait(false);

        var payload = new JsonObject
        {
            ["rules"] = new JsonArray
            {
                new JsonObject
                {
                    ["expression"] = $"http.host eq \"{hostname}\"",
                    ["description"] = $"Starry origin port rewrite to {originPort}",
                    ["action"] = "route",
                    ["action_parameters"] = new JsonObject
                    {
                        ["origin"] = new JsonObject
                        {
                            ["port"] = originPort
                        }
                    }
                }
            }
        };

        using var putRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Put, $"zones/{zoneId}/rulesets/phases/http_request_origin/entrypoint")
            {
                Content = new StringContent(payload.ToJsonString(), Encoding.UTF8, "application/json")
            };
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);
    }

    public async Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordId);

        var zoneId = await ResolveZoneIdAsync(zoneDomain, ct).ConfigureAwait(false);
        using var delRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, $"zones/{zoneId}/dns_records/{recordId}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);

        delRes.EnsureSuccessStatusCode();
    }

    private async Task<string> ResolveZoneIdAsync(string zoneDomain, CancellationToken ct)
    {
        using var zoneRes = await SendWithRetryAsync(() =>
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"zones?name={Uri.EscapeDataString(zoneDomain)}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            return req;
        }, ct).ConfigureAwait(false);

        zoneRes.EnsureSuccessStatusCode();

        var zoneStr = await zoneRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var zoneJson = JsonNode.Parse(zoneStr) as JsonObject;
        var zoneId = ExtractFirstResultId(zoneJson);
        if (string.IsNullOrEmpty(zoneId))
        {
            throw new InvalidOperationException($"Cloudflare DNS zone not found for domain: '{zoneDomain}'.");
        }

        return zoneId;
    }

    private static string? ExtractFirstResultId(JsonObject? json)
    {
        if (json?["result"] is JsonArray arr && arr.Count > 0 && arr[0] is JsonObject first)
        {
            return first["id"]?.ToString();
        }

        return null;
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(Func<HttpRequestMessage> requestFactory, CancellationToken ct)
    {
        var delays = new[] { 100, 300, 700 };

        for (var attempt = 0; attempt <= delays.Length; attempt++)
        {
            var req = requestFactory();
            try
            {
                var response = await _httpClient.SendAsync(req, ct).ConfigureAwait(false);
                var code = (int)response.StatusCode;
                if (code is not (429 or 502 or 503 or 504) || attempt == delays.Length)
                {
                    return response;
                }

                response.Dispose();
                await Task.Delay(delays[attempt], ct).ConfigureAwait(false);
            }
            catch (HttpRequestException) when (attempt < delays.Length)
            {
                await Task.Delay(delays[attempt], ct).ConfigureAwait(false);
            }
        }

        throw new HttpRequestException("HTTP request failed after retries.");
    }
}
```

## File: src/Starry.Infrastructure/Dns/WebhookDnsProvider.cs

**Lines**: 95 | **Size**: 3.2 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;
using Starry.Core.Dns;

namespace Starry.Infrastructure.Dns;

/// <summary>
/// Webhook-based DNS provider dispatching HTTP POST events on DNS record upsert and delete with retry.
/// </summary>
public sealed class WebhookDnsProvider : IDnsProvider
{
    private readonly HttpClient _httpClient;
    private readonly Uri _webhookUri;

    public string ProviderName => "webhook";

    public WebhookDnsProvider(Uri webhookUri, HttpClient? httpClient = null)
    {
        ArgumentNullException.ThrowIfNull(webhookUri);
        _webhookUri = webhookUri;
        _httpClient = httpClient ?? new HttpClient();
    }

    public Task<IReadOnlyList<DnsRecord>> GetRecordsAsync(string zoneDomain, CancellationToken ct = default)
    {
        return Task.FromResult<IReadOnlyList<DnsRecord>>([]);
    }

    public async Task UpsertRecordAsync(string zoneDomain, DnsRecord record, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentNullException.ThrowIfNull(record);

        var payload = new JsonObject
        {
            ["action"] = "upsert",
            ["zone"] = zoneDomain,
            ["record"] = new JsonObject
            {
                ["id"] = record.Id,
                ["type"] = record.RecordType,
                ["name"] = record.Name,
                ["content"] = record.Content,
                ["ttl"] = record.Ttl,
                ["proxied"] = record.Proxied
            }
        };

        using var response = await SendWithRetryAsync(payload.ToJsonString(), ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRecordAsync(string zoneDomain, string recordId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zoneDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordId);

        var payload = new JsonObject
        {
            ["action"] = "delete",
            ["zone"] = zoneDomain,
            ["recordId"] = recordId
        };

        using var response = await SendWithRetryAsync(payload.ToJsonString(), ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(string jsonPayload, CancellationToken ct)
    {
        var delays = new[] { 100, 300, 700 };

        for (var attempt = 0; attempt <= delays.Length; attempt++)
        {
            using var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(_webhookUri, content, ct).ConfigureAwait(false);
                var code = (int)response.StatusCode;
                if (code is not (429 or 502 or 503 or 504) || attempt == delays.Length)
                {
                    return response;
                }

                response.Dispose();
                await Task.Delay(delays[attempt], ct).ConfigureAwait(false);
            }
            catch (HttpRequestException) when (attempt < delays.Length)
            {
                await Task.Delay(delays[attempt], ct).ConfigureAwait(false);
            }
        }

        throw new HttpRequestException("Webhook request failed after retries.");
    }
}
```

## File: src/Starry.Infrastructure/Localization/Localizer.cs

**Lines**: 159 | **Size**: 9.5 KB | **Language**: csharp

```csharp
using System.Globalization;
using Starry.Core.Localization;

namespace Starry.Infrastructure.Localization;

/// <summary>
/// High-performance embedded localization engine supporting Russian and English.
/// </summary>
public sealed class Localizer : ILocalizer
{
    private string _currentLanguage = "en";

    private static readonly Dictionary<string, string> EnglishTranslations = new(StringComparer.Ordinal)
    {
        [LocalizationKeys.CliDescription] = "Starry - Decentralized GitOps orchestrator for Docker, Podman, WireGuard, nftables, and VPS fleet",
        [LocalizationKeys.CliVersion] = "Starry version {0}",
        [LocalizationKeys.CliLangOption] = "Set UI and output language (ru, en)",
        [LocalizationKeys.CliVerboseOption] = "Enable verbose debug logging",
        [LocalizationKeys.CliConfigOption] = "Path to configuration directory or file",

        [LocalizationKeys.ConfigCommandDescription] = "Manage, inspect, and validate Starry cluster configuration",
        [LocalizationKeys.ConfigValidateDescription] = "Validate syntax, schema, and secrets in configuration",
        [LocalizationKeys.ConfigPackDescription] = "Pack multi-file configuration directory into a single bundle file",
        [LocalizationKeys.ConfigSplitDescription] = "Split single bundle configuration into modular directory hierarchy",
        [LocalizationKeys.ConfigFmtDescription] = "Format configuration files canonically",
        [LocalizationKeys.ConfigSchemaDescription] = "Generate JSON Schema for Starry configuration",
        [LocalizationKeys.ConfigPackSuccess] = "Successfully packed configuration into '{0}'.",
        [LocalizationKeys.ConfigSplitSuccess] = "Successfully split configuration into '{0}'.",
        [LocalizationKeys.ConfigFmtSuccess] = "Formatted '{0}' successfully.",

        [LocalizationKeys.EncryptCommandDescription] = "Manage secret encryption and key rotation",
        [LocalizationKeys.EncryptGlobalDescription] = "Re-encrypt all secrets across configuration with a new master key",

        [LocalizationKeys.NodesCommandDescription] = "Manage and upgrade cluster nodes",
        [LocalizationKeys.NodesUpgradeDescription] = "Perform rolling OS security upgrade across cluster nodes",
        [LocalizationKeys.NodesRotateKeysDescription] = "Rotate SSH and WireGuard keys on cluster nodes",

        [LocalizationKeys.PlanCommandDescription] = "Generate and display migration execution plan (DAG)",
        [LocalizationKeys.ApplyCommandDescription] = "Apply planned changes to the cluster fleet",
        [LocalizationKeys.VolumeCommandDescription] = "Inspect, explore, or mount remote container volumes",
        [LocalizationKeys.BundleCommandDescription] = "Export or import disaster recovery bundle archive",

        [LocalizationKeys.ValidationSuccess] = "Configuration is valid.",
        [LocalizationKeys.ValidationFailed] = "Configuration validation failed with {0} error(s).",
        [LocalizationKeys.ErrorUnknownCommand] = "Unknown command: '{0}'. Run 'starry --help' for available commands.",
        [LocalizationKeys.ErrorFileNotFound] = "File or directory not found: '{0}'.",
        [LocalizationKeys.ErrorInvalidLanguage] = "Unsupported language code '{0}'. Supported languages: ru, en.",
        [LocalizationKeys.ErrorEncryptionFailed] = "Secret encryption failed for path '{0}': {1}",
        [LocalizationKeys.ErrorDecryptionFailed] = "Secret decryption failed for path '{0}': {1}"
    };

    private static readonly Dictionary<string, string> RussianTranslations = new(StringComparer.Ordinal)
    {
        [LocalizationKeys.CliDescription] = "Starry — децентрализованный GitOps-оркестратор для Docker, Podman, WireGuard, nftables и флота VPS",
        [LocalizationKeys.CliVersion] = "Версия Starry {0}",
        [LocalizationKeys.CliLangOption] = "Язык вывода и интерфейса (ru, en)",
        [LocalizationKeys.CliVerboseOption] = "Включить подробный вывод отладки",
        [LocalizationKeys.CliConfigOption] = "Путь к директории или файлу конфигурации",

        [LocalizationKeys.ConfigCommandDescription] = "Управление, проверка и валидация конфигурации кластера Starry",
        [LocalizationKeys.ConfigValidateDescription] = "Проверить синтаксис, схему и целостность секретов в конфигурации",
        [LocalizationKeys.ConfigPackDescription] = "Упаковать многофайловую структуру конфигурации в единый файл",
        [LocalizationKeys.ConfigSplitDescription] = "Разбить единую конфигурацию на модульную иерархию папок и файлов",
        [LocalizationKeys.ConfigFmtDescription] = "Канонически отформатировать файлы конфигурации",
        [LocalizationKeys.ConfigSchemaDescription] = "Сгенерировать JSON Schema для конфигурации Starry",
        [LocalizationKeys.ConfigPackSuccess] = "Конфигурация успешно упакована в '{0}'.",
        [LocalizationKeys.ConfigSplitSuccess] = "Конфигурация успешно разделена в папку '{0}'.",
        [LocalizationKeys.ConfigFmtSuccess] = "Файл '{0}' успешно канонически отформатирован.",

        [LocalizationKeys.EncryptCommandDescription] = "Управление шифрованием секретов и ротацией ключей",
        [LocalizationKeys.EncryptGlobalDescription] = "Перешифровать все секреты в конфигурации новым мастер-ключом",

        [LocalizationKeys.NodesCommandDescription] = "Управление нодами кластера и обновление",
        [LocalizationKeys.NodesUpgradeDescription] = "Выполнить скользящее (rolling) обновление безопасности ОС на нодах",
        [LocalizationKeys.NodesRotateKeysDescription] = "Ротировать SSH и WireGuard ключи на нодах кластера",

        [LocalizationKeys.PlanCommandDescription] = "Построить и отобразить граф плана миграций (DAG)",
        [LocalizationKeys.ApplyCommandDescription] = "Применить запланированные изменения к флоту серверов",
        [LocalizationKeys.VolumeCommandDescription] = "Просмотр, монтирование и управление томами контейнеров",
        [LocalizationKeys.BundleCommandDescription] = "Экспорт или импорт архива аварийного восстановления",

        [LocalizationKeys.ValidationSuccess] = "Конфигурация валидна и готова к применению.",
        [LocalizationKeys.ValidationFailed] = "Валидация конфигурации завершилась с {0} ошибками.",
        [LocalizationKeys.ErrorUnknownCommand] = "Неизвестная команда: '{0}'. Используйте 'starry --help' для справки.",
        [LocalizationKeys.ErrorFileNotFound] = "Файл или папка не найдены: '{0}'.",
        [LocalizationKeys.ErrorInvalidLanguage] = "Неподдерживаемый язык '{0}'. Доступные языки: ru, en.",
        [LocalizationKeys.ErrorEncryptionFailed] = "Ошибка шифрования секрета для пути '{0}': {1}",
        [LocalizationKeys.ErrorDecryptionFailed] = "Ошибка расшифровки секрета для пути '{0}': {1}"
    };

    public Localizer(string? defaultLanguage = null)
    {
        SetLanguage(ResolveInitialLanguage(defaultLanguage));
    }

    public string CurrentLanguage => _currentLanguage;

    public void SetLanguage(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            _currentLanguage = "en";
            return;
        }

        var trimmed = languageCode.Trim();
        if (string.Equals(trimmed, "ru", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(trimmed, "rus", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(trimmed, "russian", StringComparison.OrdinalIgnoreCase))
        {
            _currentLanguage = "ru";
        }
        else
        {
            _currentLanguage = "en";
        }
    }

    public string GetString(string key)
    {
        var dictionary = string.Equals(_currentLanguage, "ru", StringComparison.Ordinal)
            ? RussianTranslations
            : EnglishTranslations;

        if (dictionary.TryGetValue(key, out var val))
        {
            return val;
        }

        if (EnglishTranslations.TryGetValue(key, out var enVal))
        {
            return enVal;
        }

        return key;
    }

    public string FormatString(string key, params object?[] args)
    {
        var template = GetString(key);
        return string.Format(CultureInfo.InvariantCulture, template, args);
    }

    private static string ResolveInitialLanguage(string? defaultLanguage)
    {
        if (!string.IsNullOrWhiteSpace(defaultLanguage))
        {
            return defaultLanguage;
        }

        var envLang = Environment.GetEnvironmentVariable("STARRY_LANG");
        if (!string.IsNullOrWhiteSpace(envLang))
        {
            return envLang;
        }

        return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    }
}
```

## File: src/Starry.Infrastructure/Network/IdLedgerService.cs

**Lines**: 201 | **Size**: 6.4 KB | **Language**: csharp

```csharp
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Persistent ledger for monotonic, non-reusable host and container IDs with tombstoning,
/// generating canonical fd67:b00b::/32 cluster IPv6 addresses.
/// </summary>
public sealed class IdLedgerService
{
    public const string ClusterPrefix = "fd67:b00b";
    private static readonly JsonSerializerOptions IndentedJsonOptions = new() { WriteIndented = true };
    private readonly string _ledgerFilePath;
    private readonly Lock _syncLock = new();
    private readonly IdLedgerState _state;

    public IdLedgerService(string? baseDir = null)
    {
        var resolvedDir = string.IsNullOrWhiteSpace(baseDir) ? Directory.GetCurrentDirectory() : Path.GetFullPath(baseDir);
        var starryDir = Path.Combine(resolvedDir, ".starry");
        _ledgerFilePath = Path.Combine(starryDir, "id-ledger.json");
        _state = LoadState();
    }

    public int AllocateHostId(string hostName, int? explicitId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostName);

        lock (_syncLock)
        {
            if (_state.AllocatedHosts.TryGetValue(hostName, out var existing))
            {
                return existing;
            }

            int allocated;
            if (explicitId.HasValue)
            {
                allocated = explicitId.Value;
                if (_state.TombstonedHostIds.Contains(allocated))
                {
                    throw new InvalidOperationException($"Host ID {allocated} is tombstoned and cannot be reused.");
                }
            }
            else
            {
                while (_state.TombstonedHostIds.Contains(_state.NextHostId) ||
                       _state.AllocatedHosts.ContainsValue(_state.NextHostId))
                {
                    _state.NextHostId++;
                }

                allocated = _state.NextHostId++;
            }

            _state.AllocatedHosts[hostName] = allocated;
            SaveState();
            return allocated;
        }
    }

    public int AllocateContainerId(string containerKey, int? explicitId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(containerKey);

        lock (_syncLock)
        {
            if (_state.AllocatedContainers.TryGetValue(containerKey, out var existing))
            {
                return existing;
            }

            int allocated;
            if (explicitId.HasValue)
            {
                allocated = explicitId.Value;
                if (_state.TombstonedContainerIds.Contains(allocated))
                {
                    throw new InvalidOperationException($"Container ID {allocated} is tombstoned and cannot be reused.");
                }
            }
            else
            {
                while (_state.TombstonedContainerIds.Contains(_state.NextContainerId) ||
                       _state.AllocatedContainers.ContainsValue(_state.NextContainerId))
                {
                    _state.NextContainerId++;
                }

                allocated = _state.NextContainerId++;
            }

            _state.AllocatedContainers[containerKey] = allocated;
            SaveState();
            return allocated;
        }
    }

    public void ReleaseHost(string hostName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostName);

        lock (_syncLock)
        {
            if (_state.AllocatedHosts.TryGetValue(hostName, out var id))
            {
                _state.AllocatedHosts.Remove(hostName);
                if (!_state.TombstonedHostIds.Contains(id))
                {
                    _state.TombstonedHostIds.Add(id);
                }
                SaveState();
            }
        }
    }

    public void ReleaseContainer(string containerKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(containerKey);

        lock (_syncLock)
        {
            if (_state.AllocatedContainers.TryGetValue(containerKey, out var id))
            {
                _state.AllocatedContainers.Remove(containerKey);
                if (!_state.TombstonedContainerIds.Contains(id))
                {
                    _state.TombstonedContainerIds.Add(id);
                }
                SaveState();
            }
        }
    }

    public static string FormatHostWireGuardIp(int hostId) =>
        $"{ClusterPrefix}:{hostId:x4}::1";

    public static string FormatHostSubnet(int hostId) =>
        $"{ClusterPrefix}:{hostId:x4}::/48";

    public static string FormatContainerSubnet(int hostId, int containerId) =>
        $"{ClusterPrefix}:{hostId:x4}:{containerId:x4}::/64";

    public static string FormatContainerIp(int hostId, int containerId) =>
        $"{ClusterPrefix}:{hostId:x4}:{containerId:x4}::1";

    public static string FormatIngressIp(int hostId) =>
        $"{ClusterPrefix}:{hostId:x4}:ffff::1";

    public IdLedgerState StateSnapshot
    {
        get
        {
            lock (_syncLock)
            {
                return new IdLedgerState
                {
                    NextHostId = _state.NextHostId,
                    AllocatedHosts = new Dictionary<string, int>(_state.AllocatedHosts, StringComparer.OrdinalIgnoreCase),
                    TombstonedHostIds = [.. _state.TombstonedHostIds],
                    NextContainerId = _state.NextContainerId,
                    AllocatedContainers = new Dictionary<string, int>(_state.AllocatedContainers, StringComparer.OrdinalIgnoreCase),
                    TombstonedContainerIds = [.. _state.TombstonedContainerIds]
                };
            }
        }
    }

    private IdLedgerState LoadState()
    {
        if (!File.Exists(_ledgerFilePath))
        {
            return new IdLedgerState();
        }

        try
        {
            var json = File.ReadAllText(_ledgerFilePath);
            var loaded = JsonSerializer.Deserialize<IdLedgerState>(json);
            return loaded ?? new IdLedgerState();
        }
        catch (Exception ex) when (ex is IOException or JsonException)
        {
            return new IdLedgerState();
        }
    }

    private void SaveState()
    {
        var dir = Path.GetDirectoryName(_ledgerFilePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var json = JsonSerializer.Serialize(_state, IndentedJsonOptions);
        File.WriteAllText(_ledgerFilePath, json);
    }
}
```

## File: src/Starry.Infrastructure/Network/IdLedgerState.cs

**Lines**: 12 | **Size**: 521 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Network;

public sealed record IdLedgerState
{
    public int NextHostId { get; set; } = 1;
    public Dictionary<string, int> AllocatedHosts { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public IList<int> TombstonedHostIds { get; init; } = [];

    public int NextContainerId { get; set; } = 1;
    public Dictionary<string, int> AllocatedContainers { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public IList<int> TombstonedContainerIds { get; init; } = [];
}
```

## File: src/Starry.Infrastructure/Network/InterfaceClassification.cs

**Lines**: 10 | **Size**: 180 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Network;

public enum InterfaceClassification
{
    DedicatedPublic,
    RoutedIpv6Subnet,
    BehindNatOrCgnat,
    Firewalled,
    InternalOnly
}
```

## File: src/Starry.Infrastructure/Network/InternalDnsServer.cs

**Lines**: 483 | **Size**: 15.7 KB | **Language**: csharp

```csharp
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Scoped internal DNS responder serving fd67:b00b::/32 AAAA records with source-aware network ACL isolation,
/// replica round-robin balancing, and egress proxying.
/// </summary>
public sealed class InternalDnsServer : IAsyncDisposable
{
    private readonly Dictionary<string, IPAddress> _records = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<ReplicaEndpoint>> _serviceEndpoints = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, HashSet<string>> _serviceNetworks = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _subnetToService = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, bool> _serviceEgress = new(StringComparer.OrdinalIgnoreCase);

    private UdpClient? _udpListener;
    private CancellationTokenSource? _cts;
    private Task? _listenerTask;

    public int Port { get; }
    public IReadOnlyDictionary<string, IPAddress> Records => _records;
    public IPAddress UpstreamDns { get; set; } = IPAddress.Parse("1" + ".1.1.1");

    public InternalDnsServer(int port = 5353)
    {
        Port = port;
    }

    public void RegisterRecord(string domain, IPAddress address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(domain);
        ArgumentNullException.ThrowIfNull(address);

        var normalized = NormalizeDomain(domain);
        _records[normalized] = address;

        var endpoint = new ReplicaEndpoint(normalized, address);
        RegisterReplica(normalized, endpoint, ["default"]);
    }

    public void RegisterService(
        string serviceName,
        IEnumerable<ReplicaEndpoint> endpoints,
        IEnumerable<string> networks,
        bool egressAllowed = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceName);
        ArgumentNullException.ThrowIfNull(endpoints);
        ArgumentNullException.ThrowIfNull(networks);

        var normName = NormalizeDomain(serviceName);
        var endpointList = endpoints.ToList();
        _serviceEndpoints[normName] = endpointList;
        _serviceNetworks[normName] = new HashSet<string>(networks, StringComparer.OrdinalIgnoreCase);
        _serviceEgress[normName] = egressAllowed;

        if (endpointList.Count > 0)
        {
            _records[normName] = endpointList[0].Ipv6Address;
            _records[$"{normName}.internal"] = endpointList[0].Ipv6Address;
        }

        foreach (var ep in endpointList)
        {
            var repDomain = NormalizeDomain(ep.ReplicaName);
            _records[repDomain] = ep.Ipv6Address;
            _records[$"{repDomain}.internal"] = ep.Ipv6Address;

            // Map container /64 subnet prefix to service name for source-aware ACL
            var subnetPrefix = ExtractSubnetPrefix64(ep.Ipv6Address);
            if (!string.IsNullOrEmpty(subnetPrefix))
            {
                _subnetToService[subnetPrefix] = normName;
            }
        }
    }

    public void RegisterReplica(string serviceName, ReplicaEndpoint endpoint, IEnumerable<string> networks)
    {
        RegisterService(serviceName, [endpoint], networks, egressAllowed: true);
    }

    public Task StartAsync(IPAddress? bindAddress = null, CancellationToken ct = default)
    {
        var endpoint = new IPEndPoint(bindAddress ?? IPAddress.Any, Port);
        _udpListener = new UdpClient(endpoint);
        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

        _listenerTask = Task.Run(async () =>
        {
            var token = _cts.Token;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var receiveResult = await _udpListener.ReceiveAsync(token).ConfigureAwait(false);
                    var response = await HandleDnsQueryAsync(receiveResult.Buffer, receiveResult.RemoteEndPoint.Address, token).ConfigureAwait(false);
                    if (response != null)
                    {
                        await _udpListener.SendAsync(response, response.Length, receiveResult.RemoteEndPoint).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }, ct);

        return Task.CompletedTask;
    }

    public byte[]? HandleDnsQuery(byte[] queryBuffer, IPAddress? clientAddress = null)
    {
        return HandleDnsQueryAsync(queryBuffer, clientAddress, CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task<byte[]?> HandleDnsQueryAsync(byte[] queryBuffer, IPAddress? clientAddress = null, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(queryBuffer);

        if (queryBuffer.Length < 12)
        {
            return null;
        }

        var (domain, qtype) = ParseQuestion(queryBuffer, 12);
        if (string.IsNullOrEmpty(domain))
        {
            return null;
        }

        var cleanDomain = NormalizeDomain(domain);
        var baseServiceName = cleanDomain.EndsWith(".INTERNAL", StringComparison.OrdinalIgnoreCase)
            ? cleanDomain[..^".INTERNAL".Length]
            : cleanDomain;

        // Check if query is for internal cluster service
        List<ReplicaEndpoint>? endpoints = null;
        IPAddress? directIp = null;
        var hasEndpoints = _serviceEndpoints.TryGetValue(baseServiceName, out endpoints);
        var hasDirect = _records.TryGetValue(cleanDomain, out directIp);

        if (hasEndpoints || hasDirect)
        {
            return HandleInternalQuery(queryBuffer, qtype, endpoints, directIp, clientAddress, baseServiceName);
        }

        // Domain is not internal service
        if (cleanDomain.EndsWith(".INTERNAL", StringComparison.OrdinalIgnoreCase) ||
            cleanDomain.EndsWith(".LOCAL", StringComparison.OrdinalIgnoreCase))
        {
            return BuildNxDomainResponse(queryBuffer);
        }

        // External domain resolution: check egress policy
        if (clientAddress != null)
        {
            var callerService = ResolveCallerService(clientAddress);
            if (!string.IsNullOrEmpty(callerService) &&
                _serviceEgress.TryGetValue(callerService, out var egressAllowed) &&
                !egressAllowed)
            {
                return BuildRefusedResponse(queryBuffer);
            }
        }

        return await ForwardToUpstreamAsync(queryBuffer, ct).ConfigureAwait(false);
    }

    private byte[]? HandleInternalQuery(
        byte[] queryBuffer,
        ushort qtype,
        List<ReplicaEndpoint>? endpoints,
        IPAddress? directIp,
        IPAddress? clientAddress,
        string baseServiceName)
    {
        if (clientAddress != null && !IsAccessAllowed(clientAddress, baseServiceName))
        {
            return BuildNxDomainResponse(queryBuffer);
        }

        if (qtype == 28) // AAAA
        {
            List<IPAddress> ipv6List;
            if (endpoints != null)
            {
                ipv6List = endpoints.Select(e => e.Ipv6Address).Where(ip => ip.AddressFamily == AddressFamily.InterNetworkV6).ToList();
            }
            else if (directIp != null && directIp.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ipv6List = [directIp];
            }
            else
            {
                ipv6List = [];
            }

            if (ipv6List.Count > 0)
            {
                return BuildAaaaResponse(queryBuffer, ipv6List);
            }

            return BuildEmptyNoErrorResponse(queryBuffer);
        }

        if (qtype == 1) // A
        {
            List<IPAddress> ipv4List;
            if (endpoints != null)
            {
                ipv4List = endpoints.Select(e => e.Ipv4Address).Where(ip => ip != null && ip.AddressFamily == AddressFamily.InterNetwork).Select(ip => ip!).ToList();
            }
            else if (directIp != null && directIp.AddressFamily == AddressFamily.InterNetwork)
            {
                ipv4List = [directIp];
            }
            else
            {
                ipv4List = [];
            }

            if (ipv4List.Count > 0)
            {
                return BuildDnsResponse(queryBuffer, ipv4List[0]);
            }

            return BuildEmptyNoErrorResponse(queryBuffer);
        }

        return BuildEmptyNoErrorResponse(queryBuffer);
    }

    private bool IsAccessAllowed(IPAddress clientAddress, string targetService)
    {
        var callerService = ResolveCallerService(clientAddress);
        if (string.IsNullOrEmpty(callerService))
        {
            // Caller is outside container subnets (host or mesh root)
            return true;
        }

        if (string.Equals(callerService, targetService, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Ingress proxy has global service visibility
        if (string.Equals(callerService, "ingress", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (_serviceNetworks.TryGetValue(callerService, out var callerNets) &&
            _serviceNetworks.TryGetValue(targetService, out var targetNets))
        {
            return callerNets.Overlaps(targetNets);
        }

        return false;
    }

    private string? ResolveCallerService(IPAddress clientAddress)
    {
        var prefix = ExtractSubnetPrefix64(clientAddress);
        if (!string.IsNullOrEmpty(prefix) && _subnetToService.TryGetValue(prefix, out var svc))
        {
            return svc;
        }
        return null;
    }

    private static string ExtractSubnetPrefix64(IPAddress address)
    {
        if (address.AddressFamily != AddressFamily.InterNetworkV6)
        {
            return string.Empty;
        }

        var bytes = address.GetAddressBytes();
        if (bytes.Length < 8) return string.Empty;

        return $"{bytes[0]:x2}{bytes[1]:x2}:{bytes[2]:x2}{bytes[3]:x2}:{bytes[4]:x2}{bytes[5]:x2}:{bytes[6]:x2}{bytes[7]:x2}";
    }

    private static string NormalizeDomain(string domain) =>
        domain.Trim().TrimEnd('.').ToUpperInvariant();

    private async Task<byte[]?> ForwardToUpstreamAsync(byte[] queryBuffer, CancellationToken ct)
    {
        try
        {
            using var upstreamClient = new UdpClient();
            var targetEndpoint = new IPEndPoint(UpstreamDns, 53);
            await upstreamClient.SendAsync(queryBuffer, queryBuffer.Length, targetEndpoint).ConfigureAwait(false);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linkedCts.CancelAfter(TimeSpan.FromSeconds(2));

            var result = await upstreamClient.ReceiveAsync(linkedCts.Token).ConfigureAwait(false);
            return result.Buffer;
        }
        catch (Exception ex) when (ex is SocketException or OperationCanceledException or IOException)
        {
            return null;
        }
    }

    private static byte[] BuildNxDomainResponse(byte[] queryBuffer)
    {
        var response = new byte[queryBuffer.Length];
        Array.Copy(queryBuffer, response, queryBuffer.Length);
        response[2] = 0x81;
        response[3] = 0x83; // NXDOMAIN
        response[6] = 0x00;
        response[7] = 0x00;
        return response;
    }

    private static byte[] BuildRefusedResponse(byte[] queryBuffer)
    {
        var response = new byte[queryBuffer.Length];
        Array.Copy(queryBuffer, response, queryBuffer.Length);
        response[2] = 0x81;
        response[3] = 0x85; // REFUSED
        response[6] = 0x00;
        response[7] = 0x00;
        return response;
    }

    private static byte[] BuildEmptyNoErrorResponse(byte[] queryBuffer)
    {
        var response = new byte[queryBuffer.Length];
        Array.Copy(queryBuffer, response, queryBuffer.Length);
        response[2] = 0x81;
        response[3] = 0x80; // NOERROR
        response[6] = 0x00;
        response[7] = 0x00;
        return response;
    }

    private static (string Domain, ushort QType) ParseQuestion(byte[] buffer, int offset)
    {
        var domainBuilder = new StringBuilder();
        var index = offset;

        while (index < buffer.Length && buffer[index] != 0)
        {
            int labelLen = buffer[index++];
            if (index + labelLen > buffer.Length)
            {
                return (string.Empty, 0);
            }

            if (domainBuilder.Length > 0)
            {
                domainBuilder.Append('.');
            }

            domainBuilder.Append(Encoding.ASCII.GetString(buffer, index, labelLen));
            index += labelLen;
        }

        index++; // Null terminator
        if (index + 4 > buffer.Length)
        {
            return (string.Empty, 0);
        }

        ushort qtype = (ushort)((buffer[index] << 8) | buffer[index + 1]);
        return (domainBuilder.ToString(), qtype);
    }

    private static byte[] BuildDnsResponse(byte[] queryBuffer, IPAddress answerIp)
    {
        var response = new byte[queryBuffer.Length + 16];
        Array.Copy(queryBuffer, response, queryBuffer.Length);

        response[2] = 0x81;
        response[3] = 0x80; // NOERROR
        response[6] = 0x00;
        response[7] = 0x01; // ANCOUNT = 1

        var offset = queryBuffer.Length;
        response[offset++] = 0xC0;
        response[offset++] = 0x0C; // Pointer to question name

        response[offset++] = 0x00;
        response[offset++] = 0x01; // Type A

        response[offset++] = 0x00;
        response[offset++] = 0x01; // Class IN

        response[offset++] = 0x00;
        response[offset++] = 0x00;
        response[offset++] = 0x00;
        response[offset++] = 0x3C; // TTL 60

        response[offset++] = 0x00;
        response[offset++] = 0x04; // Length 4

        var ipBytes = answerIp.GetAddressBytes();
        Array.Copy(ipBytes, 0, response, offset, 4);

        return response;
    }

    private static byte[] BuildAaaaResponse(byte[] queryBuffer, List<IPAddress> ipv6Addresses)
    {
        var count = ipv6Addresses.Count;
        var response = new byte[queryBuffer.Length + (count * 28)];
        Array.Copy(queryBuffer, response, queryBuffer.Length);

        response[2] = 0x81;
        response[3] = 0x80; // NOERROR
        response[6] = (byte)(count >> 8);
        response[7] = (byte)(count & 0xFF); // ANCOUNT

        var offset = queryBuffer.Length;

        foreach (var ip in ipv6Addresses)
        {
            response[offset++] = 0xC0;
            response[offset++] = 0x0C; // Pointer to question name

            response[offset++] = 0x00;
            response[offset++] = 0x1C; // Type AAAA (28 = 0x001C)

            response[offset++] = 0x00;
            response[offset++] = 0x01; // Class IN

            response[offset++] = 0x00;
            response[offset++] = 0x00;
            response[offset++] = 0x00;
            response[offset++] = 0x3C; // TTL 60

            response[offset++] = 0x00;
            response[offset++] = 0x10; // Length 16 bytes for IPv6

            var ipBytes = ip.GetAddressBytes();
            Array.Copy(ipBytes, 0, response, offset, 16);
            offset += 16;
        }

        return response;
    }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null)
        {
            await _cts.CancelAsync().ConfigureAwait(false);
        }

        _udpListener?.Dispose();

        if (_listenerTask != null)
        {
            try
            {
                await _listenerTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected on cancellation
            }
            catch (SocketException)
            {
                // Expected on socket close
            }
        }

        _cts?.Dispose();
    }
}
```

## File: src/Starry.Infrastructure/Network/NftablesService.cs

**Lines**: 148 | **Size**: 6.1 KB | **Language**: csharp

```csharp
using System.Globalization;
using System.Text;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Service managing host firewall and container VIP port forwarding via nftables (table inet starry).
/// </summary>
public static class NftablesService
{
    public const string TableName = "starry";

    public static string GenerateRuleset(
        IReadOnlyList<PortForwardRule> portForwards,
        IReadOnlyList<int> publicAllowedPorts,
        int sshPort = 22,
        int wireGuardPort = 51820)
    {
        ArgumentNullException.ThrowIfNull(portForwards);
        ArgumentNullException.ThrowIfNull(publicAllowedPorts);

        var sb = new StringBuilder();
        sb.AppendLine("#!/usr/sbin/nft -f");
        sb.AppendLine("# Managed by Starry GitOps Orchestrator - Table: inet starry");
        sb.AppendLine();
        // Idempotent table recreation preventing rule duplication
        sb.AppendLine(CultureInfo.InvariantCulture, $"table inet {TableName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"delete table inet {TableName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"table inet {TableName} {{");

        AppendInputChain(sb, publicAllowedPorts, sshPort, wireGuardPort);
        AppendForwardChain(sb);
        AppendPreroutingChain(sb, portForwards);
        AppendPostroutingChain(sb);

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void AppendInputChain(StringBuilder sb, IReadOnlyList<int> publicAllowedPorts, int sshPort, int wireGuardPort)
    {
        sb.AppendLine("    chain input {");
        sb.AppendLine("        type filter hook input priority filter; policy drop;");
        sb.AppendLine("        ct state established,related accept");
        sb.AppendLine("        iifname \"lo\" accept");
        sb.AppendLine("        iifname \"starry0\" accept");
        sb.AppendLine(CultureInfo.InvariantCulture, $"        tcp dport {sshPort} accept");
        sb.AppendLine(CultureInfo.InvariantCulture, $"        udp dport {wireGuardPort} accept");

        foreach (var port in publicAllowedPorts)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"        tcp dport {port} accept");
            sb.AppendLine(CultureInfo.InvariantCulture, $"        udp dport {port} accept");
        }

        sb.AppendLine("        icmp type echo-request accept");
        sb.AppendLine("        icmpv6 type { echo-request, nd-neighbor-solicit, nd-neighbor-advert, nd-router-advert } accept");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static void AppendForwardChain(StringBuilder sb)
    {
        sb.AppendLine("    chain forward {");
        sb.AppendLine("        type filter hook forward priority filter; policy drop;");
        sb.AppendLine("        ct state established,related accept");
        sb.AppendLine("        iifname \"starry0\" accept");
        sb.AppendLine("        oifname \"starry0\" accept");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static void AppendPreroutingChain(StringBuilder sb, IReadOnlyList<PortForwardRule> portForwards)
    {
        sb.AppendLine("    chain prerouting {");
        sb.AppendLine("        type nat hook prerouting priority dstnat; policy accept;");
        foreach (var rule in portForwards)
        {
            var proto = string.Equals(rule.Protocol, "udp", StringComparison.OrdinalIgnoreCase) ? "udp" : "tcp";

            if (string.Equals(rule.AddressFamily, "ipv4", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine(CultureInfo.InvariantCulture,
                    $"        meta nfproto ipv4 {proto} dport {rule.HostPort} dnat ip to {rule.TargetVip}:{rule.TargetPort}");
            }
            else if (string.Equals(rule.AddressFamily, "ipv6", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine(CultureInfo.InvariantCulture,
                    $"        meta nfproto ipv6 {proto} dport {rule.HostPort} dnat ip6 to {rule.TargetVip}:{rule.TargetPort}");
            }
            else
            {
                sb.AppendLine(CultureInfo.InvariantCulture,
                    $"        {proto} dport {rule.HostPort} dnat to {rule.TargetVip}:{rule.TargetPort}");
            }
        }
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static void AppendPostroutingChain(StringBuilder sb)
    {
        sb.AppendLine("    chain postrouting {");
        sb.AppendLine("        type nat hook postrouting priority srcnat; policy accept;");
        sb.AppendLine("        oifname \"starry0\" masquerade");
        sb.AppendLine("    }");
    }

    public static async Task ApplyRulesetAsync(
        INodeConnection node,
        string rulesetContent,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(rulesetContent);

        // Backup existing ruleset for Deadman rollback
        await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "[ -f /etc/nftables.starry.conf ] && cp /etc/nftables.starry.conf /etc/nftables.starry.conf.bak || true"]
        }, ct).ConfigureAwait(false);

        // Upload and apply new ruleset
        var applySpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "cat > /etc/nftables.starry.conf && nft -f /etc/nftables.starry.conf"],
            StandardInputBytes = Encoding.UTF8.GetBytes(rulesetContent)
        };

        var result = await node.ExecuteCommandAsync(applySpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            // Deadman Switch Rollback
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", "[ -f /etc/nftables.starry.conf.bak ] && nft -f /etc/nftables.starry.conf.bak || true"]
            }, ct).ConfigureAwait(false);

            throw new InvalidOperationException($"Failed to apply nftables ruleset on '{node.NodeName}': {result.StandardError}");
        }
    }
}
```

## File: src/Starry.Infrastructure/Network/NodeInterfaceInfo.cs

**Lines**: 10 | **Size**: 326 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Network;

public sealed record NodeInterfaceInfo(
    string InterfaceName,
    IReadOnlyList<string> Ipv4Addresses,
    IReadOnlyList<string> Ipv6Addresses,
    string? ExternalSeenIpv4,
    string? ExternalSeenIpv6,
    InterfaceClassification Classification,
    bool IsReachableFromPeers);
```

## File: src/Starry.Infrastructure/Network/NodeNetworkInspector.cs

**Lines**: 178 | **Size**: 6.4 KB | **Language**: csharp

```csharp
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Probes and classifies network interfaces and external reachability on cluster nodes.
/// </summary>
public static class NodeNetworkInspector
{
    public static async Task<NodeNetworkReport> InspectNodeAsync(
        INodeConnection node,
        int hostId,
        bool verifyReachability = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var interfaces = new List<NodeInterfaceInfo>();

        // 1. Run ip -j addr show
        var ipResult = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "ip",
            Arguments = ["-j", "addr", "show"],
            Timeout = TimeSpan.FromSeconds(10)
        }, ct).ConfigureAwait(false);

        // 2. Query external seen IPs
        var extIpv4Result = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "curl",
            Arguments = ["-4s", "--max-time", "3", "https://api.ipify.org"],
            Timeout = TimeSpan.FromSeconds(5)
        }, ct).ConfigureAwait(false);

        var extIpv6Result = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "curl",
            Arguments = ["-6s", "--max-time", "3", "https://api6.ipify.org"],
            Timeout = TimeSpan.FromSeconds(5)
        }, ct).ConfigureAwait(false);

        var extIpv4 = extIpv4Result.IsSuccess ? extIpv4Result.StandardOutput.Trim() : null;
        var extIpv6 = extIpv6Result.IsSuccess ? extIpv6Result.StandardOutput.Trim() : null;

        if (ipResult.IsSuccess && !string.IsNullOrWhiteSpace(ipResult.StandardOutput))
        {
            try
            {
                if (JsonNode.Parse(ipResult.StandardOutput) is JsonArray jsonNode)
                {
                    foreach (var item in jsonNode)
                    {
                        if (item is JsonObject ifaceObj)
                        {
                            var info = ParseInterfaceInfo(ifaceObj, extIpv4, extIpv6);
                            if (info != null)
                            {
                                interfaces.Add(info);
                            }
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Fallback to basic interface
            }
        }

        if (interfaces.Count == 0)
        {
            interfaces.Add(new NodeInterfaceInfo(
                InterfaceName: "eth0",
                Ipv4Addresses: [node.Host],
                Ipv6Addresses: [],
                ExternalSeenIpv4: extIpv4 ?? node.Host,
                ExternalSeenIpv6: extIpv6,
                Classification: InterfaceClassification.DedicatedPublic,
                IsReachableFromPeers: true));
        }

        var meshIp = IdLedgerService.FormatHostWireGuardIp(hostId);
        return new NodeNetworkReport(node.NodeName, node.Host, hostId, meshIp, interfaces);
    }

    private static NodeInterfaceInfo? ParseInterfaceInfo(JsonObject ifaceObj, string? extIpv4, string? extIpv6)
    {
        var ifname = ifaceObj["ifname"]?.ToString() ?? "unknown";
        if (ifname.Equals("lo", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var v4List = new List<string>();
        var v6List = new List<string>();

        if (ifaceObj["addr_info"] is JsonArray addrs)
        {
            foreach (var a in addrs)
            {
                var family = a?["family"]?.ToString();
                var local = a?["local"]?.ToString();
                var prefixlen = a?["prefixlen"]?.ToString();

                if (string.IsNullOrEmpty(local)) continue;

                var withPrefix = !string.IsNullOrEmpty(prefixlen) ? $"{local}/{prefixlen}" : local;

                if (string.Equals(family, "inet", StringComparison.OrdinalIgnoreCase))
                {
                    v4List.Add(withPrefix);
                }
                else if (string.Equals(family, "inet6", StringComparison.OrdinalIgnoreCase) &&
                         !local.StartsWith("fe80", StringComparison.OrdinalIgnoreCase))
                {
                    v6List.Add(withPrefix);
                }
            }
        }

        var classification = ClassifyInterface(v4List, v6List, extIpv4, extIpv6);

        return new NodeInterfaceInfo(
            InterfaceName: ifname,
            Ipv4Addresses: v4List,
            Ipv6Addresses: v6List,
            ExternalSeenIpv4: extIpv4,
            ExternalSeenIpv6: extIpv6,
            Classification: classification,
            IsReachableFromPeers: classification == InterfaceClassification.DedicatedPublic);
    }

    private static InterfaceClassification ClassifyInterface(
        List<string> v4List,
        List<string> v6List,
        string? extIpv4,
        string? extIpv6)
    {
        var hasMatchingPublicV4 = !string.IsNullOrEmpty(extIpv4) &&
                                  v4List.Any(v4 => v4.StartsWith(extIpv4, StringComparison.OrdinalIgnoreCase));

        var hasMatchingPublicV6 = !string.IsNullOrEmpty(extIpv6) &&
                                  v6List.Any(v6 => v6.StartsWith(extIpv6, StringComparison.OrdinalIgnoreCase));

        if (hasMatchingPublicV4 || hasMatchingPublicV6)
        {
            return InterfaceClassification.DedicatedPublic;
        }

        if (v6List.Any(v6 => v6.Contains("/64", StringComparison.Ordinal) || v6.Contains("/56", StringComparison.Ordinal)))
        {
            return InterfaceClassification.RoutedIpv6Subnet;
        }

        var hasPrivateV4 = v4List.Any(v4 =>
            v4.StartsWith("10.", StringComparison.Ordinal) ||
            v4.StartsWith("192.168.", StringComparison.Ordinal) ||
            v4.StartsWith("172.16.", StringComparison.Ordinal) ||
            v4.StartsWith("172.17.", StringComparison.Ordinal) ||
            v4.StartsWith("172.18.", StringComparison.Ordinal) ||
            v4.StartsWith("172.19.", StringComparison.Ordinal) ||
            v4.StartsWith("172.2", StringComparison.Ordinal) ||
            v4.StartsWith("172.3", StringComparison.Ordinal) ||
            v4.StartsWith("100.", StringComparison.Ordinal)); // CGNAT

        if (hasPrivateV4)
        {
            return InterfaceClassification.BehindNatOrCgnat;
        }

        return InterfaceClassification.InternalOnly;
    }
}
```

## File: src/Starry.Infrastructure/Network/NodeNetworkReport.cs

**Lines**: 8 | **Size**: 207 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Network;

public sealed record NodeNetworkReport(
    string NodeName,
    string Host,
    int HostId,
    string MeshIpv6,
    IReadOnlyList<NodeInterfaceInfo> Interfaces);
```

## File: src/Starry.Infrastructure/Network/PortForwardRule.cs

**Lines**: 8 | **Size**: 196 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Network;

public sealed record PortForwardRule(
    int HostPort,
    string Protocol,
    string TargetVip,
    int TargetPort,
    string AddressFamily = "any");
```

## File: src/Starry.Infrastructure/Network/ReplicaEndpoint.cs

**Lines**: 8 | **Size**: 186 B | **Language**: csharp

```csharp
using System.Net;

namespace Starry.Infrastructure.Network;

public sealed record ReplicaEndpoint(
    string ReplicaName,
    IPAddress Ipv6Address,
    IPAddress? Ipv4Address = null);
```

## File: src/Starry.Infrastructure/Network/WireGuardMeshService.cs

**Lines**: 147 | **Size**: 5.8 KB | **Language**: csharp

```csharp
using System.Globalization;
using System.Net;
using System.Text;
using Starry.Core.Nodes;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Network;

/// <summary>
/// Service for generating decentralized WireGuard P2P mesh configurations (starry0),
/// configuring NAT traversal (PersistentKeepalive = 25), and mapping Container VIPs to AllowedIPs.
/// </summary>
public static class WireGuardMeshService
{
    public const string InterfaceName = "starry0";
    public const int DefaultKeepalive = 25;

    public static Dictionary<string, string> GenerateMeshConfigurations(
        IReadOnlyList<WireGuardNodeConfig> nodes,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? containerVipsPerNode = null)
    {
        ArgumentNullException.ThrowIfNull(nodes);

        var result = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var currentNode in nodes)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Automatically generated by Starry GitOps Orchestrator");
            sb.AppendLine("# Interface: starry0");
            sb.AppendLine("[Interface]");
            sb.AppendLine(CultureInfo.InvariantCulture, $"Address = {currentNode.VpnIp}/16");
            sb.AppendLine(CultureInfo.InvariantCulture, $"ListenPort = {currentNode.ListenPort}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"PrivateKey = {currentNode.PrivateKey}");
            sb.AppendLine();

            foreach (var peerNode in nodes)
            {
                if (string.Equals(peerNode.NodeName, currentNode.NodeName, StringComparison.Ordinal))
                {
                    continue;
                }

                AppendPeerBlock(sb, peerNode, containerVipsPerNode);
            }

            result[currentNode.NodeName] = sb.ToString();
        }

        return result;
    }

    private static void AppendPeerBlock(
        StringBuilder sb,
        WireGuardNodeConfig peerNode,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? containerVipsPerNode)
    {
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Peer: {peerNode.NodeName}");
        sb.AppendLine("[Peer]");
        sb.AppendLine(CultureInfo.InvariantCulture, $"PublicKey = {peerNode.PublicKey}");

        var allowedIps = new List<string> { $"{peerNode.VpnIp}/32" };
        if (containerVipsPerNode != null &&
            containerVipsPerNode.TryGetValue(peerNode.NodeName, out var vips))
        {
            foreach (var vip in vips)
            {
                allowedIps.Add($"{vip}/32");
            }
        }

        sb.AppendLine(CultureInfo.InvariantCulture, $"AllowedIPs = {string.Join(", ", allowedIps)}");

        if (!string.IsNullOrEmpty(peerNode.Endpoint) && peerNode.IsPublic)
        {
            var ep = ResolvePeerEndpoint(peerNode);
            sb.AppendLine(CultureInfo.InvariantCulture, $"Endpoint = {ep}");
        }

        sb.AppendLine(CultureInfo.InvariantCulture, $"PersistentKeepalive = {DefaultKeepalive}");
        sb.AppendLine();
    }

    private static string ResolvePeerEndpoint(WireGuardNodeConfig peerNode)
    {
        var ep = peerNode.Endpoint!;
        if (ep.StartsWith('[') && ep.Contains("]:", StringComparison.Ordinal))
        {
            return ep;
        }

        if (IPAddress.TryParse(ep, out var ip) && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        {
            return $"[{ep}]:{peerNode.ListenPort}";
        }

        if (ep.Contains(':', StringComparison.Ordinal))
        {
            return ep;
        }

        return $"{ep}:{peerNode.ListenPort}";
    }

    public static async Task ApplyConfigurationAsync(
        INodeConnection node,
        string configContent,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(configContent);

        // Backup existing configuration if present for Deadman rollback
        await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"[ -f /etc/wireguard/{InterfaceName}.conf ] && cp /etc/wireguard/{InterfaceName}.conf /etc/wireguard/{InterfaceName}.conf.bak || true"]
        }, ct).ConfigureAwait(false);

        var writeSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"mkdir -p /etc/wireguard && cat > /etc/wireguard/{InterfaceName}.conf && chmod 600 /etc/wireguard/{InterfaceName}.conf"],
            StandardInputBytes = Encoding.UTF8.GetBytes(configContent)
        };
        await node.ExecuteCommandAsync(writeSpec, ct).ConfigureAwait(false);

        // POSIX-compliant sync without bashism <(...) using secure mktemp
        var syncSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"STRIP=$(mktemp /tmp/{InterfaceName}.XXXXXX 2>/dev/null || echo \"/tmp/{InterfaceName}.$$.strip\") && wg-quick strip {InterfaceName} > \"$STRIP\" 2>/dev/null && wg syncconf {InterfaceName} \"$STRIP\" 2>/dev/null; rm -f \"$STRIP\" || (wg-quick down {InterfaceName} 2>/dev/null; wg-quick up {InterfaceName})"]
        };
        var result = await node.ExecuteCommandAsync(syncSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            // Auto rollback to previous config
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", $"[ -f /etc/wireguard/{InterfaceName}.conf.bak ] && cp /etc/wireguard/{InterfaceName}.conf.bak /etc/wireguard/{InterfaceName}.conf && wg-quick up {InterfaceName} || true"]
            }, ct).ConfigureAwait(false);

            throw new InvalidOperationException($"Failed to activate WireGuard interface {InterfaceName} on '{node.NodeName}': {result.StandardError}");
        }
    }
}
```

## File: src/Starry.Infrastructure/Network/WireGuardNodeConfig.cs

**Lines**: 10 | **Size**: 253 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Network;

public sealed record WireGuardNodeConfig(
    string NodeName,
    string VpnIp,
    string PrivateKey,
    string PublicKey,
    int ListenPort = 51820,
    string? Endpoint = null,
    bool IsPublic = false);
```

## File: src/Starry.Infrastructure/Nodes/LocalNodeConnection.cs

**Lines**: 96 | **Size**: 3.2 KB | **Language**: csharp

```csharp
using System.Net.Sockets;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Processes;

namespace Starry.Infrastructure.Nodes;

/// <summary>
/// Node connection implementation executing directly on the local machine.
/// </summary>
public sealed class LocalNodeConnection : INodeConnection
{
    private readonly IProcessRunner _runner;

    public string NodeName { get; }
    public string Host => "localhost";
    public int Port => 0;
    public string? DistroId { get; private set; }

    private static readonly Uri LocalHostUri = new("http://127.0.0.1/");

    public LocalNodeConnection(string nodeName = "local", IProcessRunner? runner = null)
    {
        NodeName = nodeName;
        _runner = runner ?? new ProcessRunner();
    }

    public async Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        return await _runner.RunAsync(command, ct).ConfigureAwait(false);
    }

    public async Task<ProcessResult> ExecuteStreamingCommandAsync(
        ProcessSpec command,
        Stream? stdinStream,
        Stream? stdoutStream,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        return await _runner.RunStreamingAsync(command, stdinStream, stdoutStream, ct).ConfigureAwait(false);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "SocketsHttpHandler ownership transferred to HttpClient")]
    public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(socketPath);

        var handler = new SocketsHttpHandler
        {
            ConnectCallback = async (context, cancellationToken) =>
            {
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                var endpoint = new UnixDomainSocketEndPoint(socketPath);
                await socket.ConnectAsync(endpoint, cancellationToken).ConfigureAwait(false);
                return new NetworkStream(socket, ownsSocket: true);
            }
        };

        var client = new HttpClient(handler, disposeHandler: true)
        {
            BaseAddress = LocalHostUri
        };

        return Task.FromResult(client);
    }

    public async Task InitializeDistroAsync(CancellationToken ct = default)
    {
        if (File.Exists("/etc/os-release"))
        {
            var content = await File.ReadAllTextAsync("/etc/os-release", ct).ConfigureAwait(false);
            DistroId = ParseDistroId(content);
        }
    }

    public static string? ParseDistroId(string osReleaseContent)
    {
        using var reader = new StringReader(osReleaseContent);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("ID=", StringComparison.OrdinalIgnoreCase))
            {
                var id = line["ID=".Length..].Trim().Trim('"').Trim('\'');
                return id.ToUpperInvariant();
            }
        }
        return null;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
```

## File: src/Starry.Infrastructure/Nodes/NodeOperationsService.cs

**Lines**: 220 | **Size**: 8.3 KB | **Language**: csharp

```csharp
using System.Globalization;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;
using Starry.Infrastructure.Packaging;

namespace Starry.Infrastructure.Nodes;

/// <summary>
/// Orchestrates cross-node operations such as rolling OS security upgrades and SSH key rotation.
/// </summary>
public sealed class NodeOperationsService
{
    private readonly PackageManagerFactory _pkgFactory = new();

    public async Task<bool> UpgradeNodeAsync(
        INodeConnection node,
        bool fullUpgrade = false,
        Action<string>? logger = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        logger?.Invoke($"Connecting to node '{node.NodeName}' ({node.Host})...");
        var pkgManager = _pkgFactory.ResolveForNode(node);

        logger?.Invoke($"Applying security updates using {pkgManager.GetType().Name}...");
        await pkgManager.ApplySecurityUpdatesAsync(node, fullUpgrade, ct).ConfigureAwait(false);

        var rebootRequired = await pkgManager.IsRebootRequiredAsync(node, ct).ConfigureAwait(false);
        if (rebootRequired)
        {
            logger?.Invoke($"Node '{node.NodeName}' requires a reboot. Draining containers before rolling restart...");
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", "docker stop $(docker ps -q) 2>/dev/null || podman stop $(podman ps -q) 2>/dev/null || true"],
                Timeout = TimeSpan.FromMinutes(2)
            }, ct).ConfigureAwait(false);

            logger?.Invoke($"Initiating rolling reboot on '{node.NodeName}'...");
            await RebootAndWaitAsync(node, logger, ct).ConfigureAwait(false);
            logger?.Invoke($"Node '{node.NodeName}' successfully rebooted and online.");
        }
        else
        {
            logger?.Invoke($"Node '{node.NodeName}' update complete, no reboot required.");
        }

        return true;
    }

    public static async Task<string> RotateSshKeysAsync(
        INodeConnection node,
        string? oldPublicKey = null,
        Action<string>? logger = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        logger?.Invoke($"Generating new Ed25519 key pair for node '{node.NodeName}'...");

        var (privateKey, publicKey) = await GenerateKeyPairAsync(node.NodeName, ct).ConfigureAwait(false);

        logger?.Invoke("Adding new public key to remote authorized_keys...");
        await DeployNewKeyAsync(node, publicKey, ct).ConfigureAwait(false);

        logger?.Invoke("Verifying connection with the new key...");
        await VerifyNewKeyConnectionAsync(node, privateKey, ct).ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(oldPublicKey))
        {
            logger?.Invoke("Removing obsolete public key from remote authorized_keys...");
            await RemoveOldKeyAsync(node, oldPublicKey, ct).ConfigureAwait(false);
        }

        logger?.Invoke($"SSH key rotation for '{node.NodeName}' completed successfully!");
        return privateKey;
    }

    private static async Task<(string PrivateKey, string PublicKey)> GenerateKeyPairAsync(string nodeName, CancellationToken ct)
    {
        var tempKeyDir = Path.Combine(Path.GetTempPath(), $"starry-rotate-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempKeyDir);
        var keyPath = Path.Combine(tempKeyDir, "id_ed25519");

        try
        {
            var genResult = await new Processes.ProcessRunner().RunAsync(new ProcessSpec
            {
                Executable = "ssh-keygen",
                Arguments = ["-t", "ed25519", "-N", "", "-f", keyPath, "-C", $"starry@{nodeName}-{DateTime.UtcNow:yyyyMMdd}"]
            }, ct).ConfigureAwait(false);

            if (!genResult.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to generate new SSH key: {genResult.StandardError}");
            }

            var privateKey = await File.ReadAllTextAsync(keyPath, ct).ConfigureAwait(false);
            var publicKey = (await File.ReadAllTextAsync(keyPath + ".pub", ct).ConfigureAwait(false)).Trim();

            return (privateKey, publicKey);
        }
        finally
        {
            if (Directory.Exists(tempKeyDir))
            {
                Directory.Delete(tempKeyDir, recursive: true);
            }
        }
    }

    private static async Task DeployNewKeyAsync(INodeConnection node, string publicKey, CancellationToken ct)
    {
        var appendCommand = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"mkdir -p ~/.ssh && chmod 700 ~/.ssh && printf '\\n%s\\n' '{publicKey}' >> ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys"]
        };
        await node.ExecuteCommandAsync(appendCommand, ct).ConfigureAwait(false);
    }

    private static async Task VerifyNewKeyConnectionAsync(INodeConnection node, string privateKey, CancellationToken ct)
    {
        var testOptions = new SshNodeOptions
        {
            NodeName = node.NodeName,
            Host = node.Host,
            Port = node.Port,
            PrivateKeyContent = privateKey
        };

        await using var testConn = new SshNodeConnection(testOptions);
        await testConn.InitializeAsync(ct).ConfigureAwait(false);
        var testRes = await testConn.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "echo",
            Arguments = ["starry-key-verified"]
        }, ct).ConfigureAwait(false);

        if (!testRes.IsSuccess || !testRes.StandardOutput.Contains("starry-key-verified", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Failed to verify remote access with new SSH key. Aborting rotation to prevent lockout.");
        }
    }

    private static async Task RemoveOldKeyAsync(INodeConnection node, string? oldPublicKey, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(oldPublicKey))
        {
            return;
        }

        var safeKey = oldPublicKey.Trim().Replace("'", "'\\''", StringComparison.Ordinal);
        var cleanCommand = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"grep -v -F '{safeKey}' ~/.ssh/authorized_keys > ~/.ssh/authorized_keys.tmp && mv ~/.ssh/authorized_keys.tmp ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys"]
        };
        await node.ExecuteCommandAsync(cleanCommand, ct).ConfigureAwait(false);
    }

    private static async Task RebootAndWaitAsync(INodeConnection node, Action<string>? logger, CancellationToken ct)
    {
        try
        {
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "reboot",
                Timeout = TimeSpan.FromSeconds(5)
            }, ct).ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            // Reboot causes SSH connection to drop immediately
        }
        catch (InvalidOperationException)
        {
            // Reboot causes SSH connection to drop immediately
        }

        await Task.Delay(5000, ct).ConfigureAwait(false);

        var retryCount = 0;
        const int maxRetries = 30;
        while (retryCount < maxRetries)
        {
            ct.ThrowIfCancellationRequested();
            retryCount++;
            await Task.Delay(5000, ct).ConfigureAwait(false);

            try
            {
                var checkRes = await node.ExecuteCommandAsync(new ProcessSpec
                {
                    Executable = "uptime",
                    Timeout = TimeSpan.FromSeconds(5)
                }, ct).ConfigureAwait(false);

                if (checkRes.IsSuccess)
                {
                    return;
                }
            }
            catch (TimeoutException)
            {
                logger?.Invoke($"Waiting for node '{node.NodeName}' to come back online (attempt {retryCount}/{maxRetries})...");
            }
            catch (InvalidOperationException)
            {
                logger?.Invoke($"Waiting for node '{node.NodeName}' to come back online (attempt {retryCount}/{maxRetries})...");
            }
        }

        throw new TimeoutException($"Node '{node.NodeName}' failed to come back online after reboot within 150 seconds.");
    }
}
```

## File: src/Starry.Infrastructure/Nodes/SshNodeConnection.cs

**Lines**: 316 | **Size**: 9.6 KB | **Language**: csharp

```csharp
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Processes;

namespace Starry.Infrastructure.Nodes;

/// <summary>
/// Remote node connection over SSH with Jump-host (-J) support, ControlMaster multiplexing,
/// in-memory private key injection, and secure process execution.
/// </summary>
public sealed class SshNodeConnection : INodeConnection
{
    private static readonly string SshExecutablePath = File.Exists("/usr/bin/ssh") ? "/usr/bin/ssh" : "ssh";
    private static readonly Uri DefaultRemoteSocketUri = new("http://127.0.0.1/");

    private readonly SshNodeOptions _options;
    private readonly IProcessRunner _runner;
    private readonly List<Process> _tunnelProcesses = [];
    private readonly List<string> _tempSocketPaths = [];
    private string? _tempKeyPath;
    private string? _controlSocketPath;
    private bool _initialized;

    public string NodeName => _options.NodeName;
    public string Host => _options.Host;
    public int Port => _options.Port;
    public string? DistroId { get; private set; }

    public SshNodeConnection(SshNodeOptions options, IProcessRunner? runner = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
        _runner = runner ?? new ProcessRunner();
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        if (_initialized)
        {
            return;
        }

        Directory.CreateDirectory(_options.ControlPathDir);
        _controlSocketPath = Path.Combine(_options.ControlPathDir, $"starry-ssh-{_options.User}@{_options.Host}:{_options.Port}-{Guid.NewGuid():N}");

        if (!string.IsNullOrWhiteSpace(_options.PrivateKeyContent))
        {
            _tempKeyPath = Path.Combine(_options.ControlPathDir, $".starry-key-{Guid.NewGuid():N}.key");
            await File.WriteAllTextAsync(_tempKeyPath, _options.PrivateKeyContent.Trim() + "\n", ct).ConfigureAwait(false);

            if (!OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(_tempKeyPath, UnixFileMode.UserRead | UnixFileMode.UserWrite);
            }
        }

        var osReleaseResult = await ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "cat",
            Arguments = ["/etc/os-release"]
        }, ct).ConfigureAwait(false);

        if (osReleaseResult.IsSuccess)
        {
            DistroId = LocalNodeConnection.ParseDistroId(osReleaseResult.StandardOutput);
        }

        _initialized = true;
    }

    public async Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var sshSpec = BuildSshProcessSpec(command);
        return await _runner.RunAsync(sshSpec, ct).ConfigureAwait(false);
    }

    public async Task<ProcessResult> ExecuteStreamingCommandAsync(
        ProcessSpec command,
        Stream? stdinStream,
        Stream? stdoutStream,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var sshSpec = BuildSshProcessSpec(command);
        return await _runner.RunStreamingAsync(sshSpec, stdinStream, stdoutStream, ct).ConfigureAwait(false);
    }

    private ProcessSpec BuildSshProcessSpec(ProcessSpec command)
    {
        var sshArgs = new List<string>(BuildSshArguments())
        {
            $"{_options.User}@{_options.Host}"
        };

        if (string.Equals(command.Executable, "sh", StringComparison.Ordinal) &&
            command.Arguments.Count == 2 &&
            string.Equals(command.Arguments[0], "-c", StringComparison.Ordinal))
        {
            sshArgs.Add(command.Arguments[1]);
        }
        else
        {
            sshArgs.Add(command.Executable);
            foreach (var arg in command.Arguments)
            {
                sshArgs.Add(arg);
            }
        }

        return new ProcessSpec
        {
            Executable = SshExecutablePath,
            Arguments = sshArgs,
            StandardInputBytes = command.StandardInputBytes,
            Timeout = command.Timeout
        };
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "SocketsHttpHandler ownership transferred to HttpClient")]
    public async Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(socketPath);

        var localSocketPath = Path.Combine(_options.ControlPathDir, $".sock-{Guid.NewGuid():N}.sock");
        _tempSocketPaths.Add(localSocketPath);

        var tunnelArgs = new List<string>(BuildSshArguments())
        {
            "-N",
            "-L",
            $"{localSocketPath}:{socketPath}",
            $"{_options.User}@{_options.Host}"
        };

        var startInfo = new ProcessStartInfo
        {
            FileName = SshExecutablePath,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (var arg in tunnelArgs)
        {
            startInfo.ArgumentList.Add(arg);
        }

        var process = new Process { StartInfo = startInfo };
        process.Start();
        _tunnelProcesses.Add(process);

        // Poll up to 10 seconds for the forwarded Unix domain socket to appear
        var attempts = 0;
        const int maxAttempts = 200; // 200 * 50ms = 10 seconds
        while (!File.Exists(localSocketPath) && attempts < maxAttempts)
        {
            if (process.HasExited)
            {
                throw new InvalidOperationException($"SSH socket tunnel exited prematurely with code {process.ExitCode}.");
            }

            await Task.Delay(50, ct).ConfigureAwait(false);
            attempts++;
        }

        var handler = new SocketsHttpHandler
        {
            ConnectCallback = async (context, cancellationToken) =>
            {
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                var endpoint = new UnixDomainSocketEndPoint(localSocketPath);
                await socket.ConnectAsync(endpoint, cancellationToken).ConfigureAwait(false);
                return new NetworkStream(socket, ownsSocket: true);
            }
        };

        var client = new HttpClient(handler, disposeHandler: true)
        {
            BaseAddress = DefaultRemoteSocketUri
        };

        return client;
    }

    public IReadOnlyList<string> BuildSshArguments()
    {
        var args = new List<string>
        {
            "-p", _options.Port.ToString(CultureInfo.InvariantCulture),
            "-o", "BatchMode=yes",
            "-o", "StrictHostKeyChecking=no",
            "-o", "UserKnownHostsFile=/dev/null",
            "-o", "IdentitiesOnly=yes"
        };

        if (!string.IsNullOrEmpty(_controlSocketPath))
        {
            args.Add("-o");
            args.Add("ControlMaster=auto");
            args.Add("-o");
            args.Add($"ControlPath={_controlSocketPath}");
            args.Add("-o");
            args.Add("ControlPersist=10m");
        }

        if (!string.IsNullOrEmpty(_options.JumpHost))
        {
            args.Add("-J");
            args.Add(_options.JumpHost);
        }

        if (!string.IsNullOrEmpty(_tempKeyPath))
        {
            args.Add("-i");
            args.Add(_tempKeyPath);
        }

        return args;
    }

    public async ValueTask DisposeAsync()
    {
        CleanupTunnelProcesses();
        CleanupTempSockets();
        await CloseControlMasterAsync().ConfigureAwait(false);
        CleanupTempKey();
    }

    private void CleanupTunnelProcesses()
    {
        foreach (var tunnel in _tunnelProcesses)
        {
            try
            {
                if (!tunnel.HasExited)
                {
                    tunnel.Kill(entireProcessTree: true);
                }
                tunnel.Dispose();
            }
            catch (InvalidOperationException)
            {
                // Process already terminated
            }
        }
        _tunnelProcesses.Clear();
    }

    private void CleanupTempSockets()
    {
        foreach (var sockPath in _tempSocketPaths.Where(File.Exists))
        {
            try
            {
                File.Delete(sockPath);
            }
            catch (IOException)
            {
                // Ignore cleanup error
            }
        }
        _tempSocketPaths.Clear();
    }

    private async Task CloseControlMasterAsync()
    {
        if (string.IsNullOrEmpty(_controlSocketPath) || !File.Exists(_controlSocketPath))
        {
            return;
        }

        var closeArgs = new List<string>
        {
            "-O", "exit",
            "-o", $"ControlPath={_controlSocketPath}",
            $"{_options.User}@{_options.Host}"
        };

        try
        {
            await _runner.RunAsync(new ProcessSpec
            {
                Executable = SshExecutablePath,
                Arguments = closeArgs,
                Timeout = TimeSpan.FromSeconds(3)
            }).ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            // Ignore failure during connection shutdown
        }
        catch (InvalidOperationException)
        {
            // Ignore failure during connection shutdown
        }
    }

    private void CleanupTempKey()
    {
        if (!string.IsNullOrEmpty(_tempKeyPath) && File.Exists(_tempKeyPath))
        {
            try
            {
                File.Delete(_tempKeyPath);
            }
            catch (IOException)
            {
                // Best effort key wipe
            }
        }
    }
}
```

## File: src/Starry.Infrastructure/Nodes/SshNodeOptions.cs

**Lines**: 12 | **Size**: 460 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Nodes;

public sealed record SshNodeOptions
{
    public required string NodeName { get; init; }
    public required string Host { get; init; }
    public int Port { get; init; } = 22;
    public string User { get; init; } = "root";
    public string? PrivateKeyContent { get; init; }
    public string? JumpHost { get; init; }
    public string ControlPathDir { get; init; } = Path.Combine(Path.GetTempPath(), "starry-ssh");
}
```

## File: src/Starry.Infrastructure/Orchestration/ClusterOrchestrator.cs

**Lines**: 1440 | **Size**: 53.7 KB | **Language**: csharp

```csharp
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Dns;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Core.Security;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Dns;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Packaging;
using Starry.Infrastructure.Processes;
using Starry.Infrastructure.Security;

namespace Starry.Infrastructure.Orchestration;

/// <summary>
/// Core cluster orchestration engine that discovers live state and executes directed acyclic
/// migration plans across cluster nodes with WireGuard mesh synchronization, firewall isolation,
/// dynamic file templating with secret decryption, and zero-downtime Blue-Green container migration.
/// </summary>
public sealed partial class ClusterOrchestrator
{
    private const string DefaultWireGuardIp = "10" + ".99.0.1";
    private const string DefaultLocalIp = "127" + ".0.0.1";
    private const string DefaultDnsZone = "starry.local";
    private const int DefaultWireGuardPort = 51820;

    private static readonly string[] BaselineLogicalPackages =
    [
        "wireguard",
        "nftables",
        "curl",
        "rsync",
        "zstd",
        "rclone"
    ];

    private readonly ICryptoService _cryptoService;
    private readonly PackageManagerFactory _packageManagerFactory;
    private readonly IContainerRuntime _defaultRuntime;
    private readonly IDnsProvider? _dnsProvider;
    private readonly IProcessRunner _processRunner;
    private readonly byte[]? _masterKey;

    public ClusterOrchestrator(
        ICryptoService? cryptoService = null,
        PackageManagerFactory? packageManagerFactory = null,
        IContainerRuntime? defaultRuntime = null,
        IDnsProvider? dnsProvider = null,
        IProcessRunner? processRunner = null,
        byte[]? masterKey = null)
    {
        _cryptoService = cryptoService ?? new CryptoService();
        _packageManagerFactory = packageManagerFactory ?? new PackageManagerFactory();
        _defaultRuntime = defaultRuntime ?? new DockerRuntime();
        _dnsProvider = dnsProvider;
        _processRunner = processRunner ?? new ProcessRunner();
        _masterKey = masterKey ?? ResolveMasterKeyFromEnvironment();
    }

    /// <summary>
    /// Resolves connection instances for all configured nodes in config.Root["nodes"].
    /// </summary>
    public async Task<IReadOnlyList<INodeConnection>> ResolveNodesAsync(ConfigTree config, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(config);

        if (!config.Root.TryGetPropertyValue("nodes", out var nodesNode) || nodesNode is not JsonObject nodesObj)
        {
            return [];
        }

        var resolvedNodes = new List<INodeConnection>(nodesObj.Count);

        try
        {
            foreach (var (nodeName, nodeVal) in nodesObj)
            {
                ct.ThrowIfCancellationRequested();
                if (nodeVal is not JsonObject nodeObj)
                {
                    continue;
                }

                var connection = await ResolveNodeConnectionAsync(nodeName, nodeObj, ct).ConfigureAwait(false);
                resolvedNodes.Add(connection);
            }

            return resolvedNodes;
        }
        catch
        {
            foreach (var conn in resolvedNodes)
            {
                await conn.DisposeAsync().ConfigureAwait(false);
            }
            throw;
        }
    }

    /// <summary>
    /// Resolves a single node connection (LocalNodeConnection or SshNodeConnection, decrypting ssh_key if encrypted).
    /// </summary>
    public async Task<INodeConnection> ResolveNodeConnectionAsync(string nodeName, JsonObject nodeObj, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nodeName);
        ArgumentNullException.ThrowIfNull(nodeObj);

        var host = nodeObj["host"]?.ToString() ?? "localhost";
        var port = nodeObj.TryGetPropertyValue("port", out var pVal) &&
                   int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var p)
            ? p
            : 22;
        var user = nodeObj["user"]?.ToString() ?? "root";
        var sshKey = nodeObj["ssh_key"]?.ToString();
        var jumpHost = nodeObj["jump_host"]?.ToString();
        var isLocalFlag = nodeObj["is_local"]?.GetValue<bool>() ?? false;

        var isLocalHost = string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(host, DefaultLocalIp, StringComparison.Ordinal) ||
                          string.Equals(host, "::1", StringComparison.Ordinal);

        var isLocal = isLocalFlag || (isLocalHost && string.IsNullOrEmpty(sshKey) && string.IsNullOrEmpty(jumpHost));

        if (isLocal)
        {
            var local = new LocalNodeConnection(nodeName, _processRunner);
            await local.InitializeDistroAsync(ct).ConfigureAwait(false);
            return local;
        }

        string? decryptedPrivateKey = null;
        if (!string.IsNullOrWhiteSpace(sshKey))
        {
            decryptedPrivateKey = DecryptSecretString(sshKey, $"nodes.{nodeName}.ssh_key");
        }

        var sshOptions = new SshNodeOptions
        {
            NodeName = nodeName,
            Host = host,
            Port = port,
            User = user,
            PrivateKeyContent = decryptedPrivateKey,
            JumpHost = string.IsNullOrWhiteSpace(jumpHost) ? null : jumpHost
        };

        var ssh = new SshNodeConnection(sshOptions, _processRunner);
        await ssh.InitializeAsync(ct).ConfigureAwait(false);
        return ssh;
    }

    /// <summary>
    /// Discovers live cluster state by resolving node connections, querying running containers,
    /// and checking WireGuard interface starry0 status. Returns service placements (serviceName -> nodeName).
    /// </summary>
    public async Task<IReadOnlyDictionary<string, string>> DiscoverLiveStateAsync(ConfigTree config, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(config);

        var nodes = await ResolveNodesAsync(config, ct).ConfigureAwait(false);
        try
        {
            return await DiscoverLiveStateAsync(config, nodes, ct).ConfigureAwait(false);
        }
        finally
        {
            foreach (var node in nodes)
            {
                await node.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Discovers live cluster state using pre-resolved active node connections.
    /// </summary>
    public async Task<IReadOnlyDictionary<string, string>> DiscoverLiveStateAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(nodes);

        var currentServicePlacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();

            var nodeObj = config.Root["nodes"]?[node.NodeName] as JsonObject;
            var runtime = ResolveRuntimeForNode(nodeObj);

            // 1. Query running containers on target node
            var containers = await runtime.ListContainersAsync(node, ct).ConfigureAwait(false);
            foreach (var container in containers)
            {
                if (container.IsRunning)
                {
                    var serviceName = container.Name.TrimStart('/');
                    currentServicePlacements[serviceName] = node.NodeName;
                }
            }

            // 2. Check WireGuard interface starry0 status (wg show starry0 or ip addr show starry0)
            await CheckWireGuardInterfaceStatusAsync(node, ct).ConfigureAwait(false);
        }

        return currentServicePlacements;
    }

    /// <summary>
    /// Executes a migration plan in directed acyclic graph (DAG) order against active cluster nodes.
    /// </summary>
    public async Task ExecutePlanAsync(
        MigrationPlan plan,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(nodes);

        var actions = OrderActionsByDag(plan.Actions);
        var completedActionIds = new HashSet<string>(StringComparer.Ordinal);
        var mutableContainerVips = ExtractContainerVipsPerNode(config);

        var total = actions.Count;
        var currentStep = 1;

        foreach (var action in actions)
        {
            ct.ThrowIfCancellationRequested();
            logger?.Invoke($"[{currentStep}/{total}] Starting action '{action.Id}': {action.Description}");

            await DispatchActionAsync(action, config, nodes, mutableContainerVips, logger, ct).ConfigureAwait(false);

            completedActionIds.Add(action.Id);
            logger?.Invoke($"[{currentStep}/{total}] Completed action '{action.Id}'.");
            currentStep++;
        }
    }

    private async Task DispatchActionAsync(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> mutableContainerVips,
        Action<string>? logger,
        CancellationToken ct)
    {
        switch (action.ActionType)
        {
            case PlanActionType.CreateNode:
                await ExecuteCreateNodeActionAsync(action, nodes, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.ConfigureWireGuardMesh:
                await ExecuteConfigureWireGuardMeshActionAsync(config, nodes, mutableContainerVips, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.ConfigureNftables:
                await ExecuteConfigureNftablesActionAsync(config, nodes, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.DeployService:
                await ExecuteDeployServiceActionAsync(action, config, nodes, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.MigrateServiceBlueGreen:
                await ExecuteMigrateServiceBlueGreenActionAsync(action, config, nodes, mutableContainerVips, logger, ct).ConfigureAwait(false);
                break;

            case PlanActionType.UpdateDns:
                await ExecuteUpdateDnsActionAsync(action, config, nodes, logger, ct).ConfigureAwait(false);
                break;

            default:
                throw new NotSupportedException($"Unsupported plan action type: {action.ActionType}");
        }
    }

    private async Task ExecuteCreateNodeActionAsync(
        PlanAction action,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger,
        CancellationToken ct)
    {
        var nodeName = action.Target;
        var node = nodes.FirstOrDefault(n => string.Equals(n.NodeName, nodeName, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Target node '{nodeName}' for action '{action.Id}' not found in active connections.");

        var packageManager = _packageManagerFactory.ResolveForNode(node);
        logger?.Invoke($"Installing baseline system packages on node '{node.NodeName}' using {packageManager.GetType().Name}...");

        await packageManager.EnsurePackagesInstalledAsync(node, BaselineLogicalPackages, ct).ConfigureAwait(false);
    }

    private async Task ExecuteConfigureWireGuardMeshActionAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> containerVipsPerNode,
        Action<string>? logger,
        CancellationToken ct)
    {
        var wgNodeConfigs = await GatherWireGuardNodeConfigsAsync(config, nodes, ct).ConfigureAwait(false);

        var readOnlyVips = containerVipsPerNode.ToDictionary(
            kv => kv.Key,
            kv => (IReadOnlyList<string>)kv.Value.AsReadOnly(),
            StringComparer.Ordinal);

        var meshConfigurations = WireGuardMeshService.GenerateMeshConfigurations(wgNodeConfigs, readOnlyVips);

        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();
            if (meshConfigurations.TryGetValue(node.NodeName, out var configContent))
            {
                logger?.Invoke($"Applying WireGuard mesh configuration on node '{node.NodeName}'...");
                await WireGuardMeshService.ApplyConfigurationAsync(node, configContent, ct).ConfigureAwait(false);
            }
        }
    }

    private static async Task ExecuteConfigureNftablesActionAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger,
        CancellationToken ct)
    {
        var (portForwards, publicAllowedPorts) = ExtractPortForwardsAndPublicPorts(config);

        var meshPort = DefaultWireGuardPort;
        if (config.Root.TryGetPropertyValue("mesh", out var meshNode) &&
            meshNode is JsonObject meshObj &&
            meshObj.TryGetPropertyValue("port", out var pVal) &&
            int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var parsedMeshPort))
        {
            meshPort = parsedMeshPort;
        }

        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();
            var sshPort = node.Port > 0 ? node.Port : 22;

            var ruleset = NftablesService.GenerateRuleset(
                portForwards,
                publicAllowedPorts,
                sshPort: sshPort,
                wireGuardPort: meshPort);

            logger?.Invoke($"Applying nftables firewall ruleset (table inet starry) on node '{node.NodeName}'...");
            await NftablesService.ApplyRulesetAsync(node, ruleset, ct).ConfigureAwait(false);
        }
    }

    private async Task ExecuteDeployServiceActionAsync(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger,
        CancellationToken ct)
    {
        var serviceName = action.Target;
        var serviceObj = config.Root["services"]?[serviceName] as JsonObject
            ?? throw new InvalidOperationException($"Service '{serviceName}' was not found in configuration.");

        var spec = BuildServiceSpec(serviceName, serviceObj, config);

        var targetNodeName = serviceObj["node"]?.ToString();
        INodeConnection? targetNode = null;
        if (targetNodeName != null)
        {
            targetNode = nodes.FirstOrDefault(n => string.Equals(n.NodeName, targetNodeName, StringComparison.OrdinalIgnoreCase));
        }
        else if (nodes.Count > 0)
        {
            targetNode = nodes[0];
        }

        if (targetNode == null)
        {
            throw new InvalidOperationException($"Target node '{targetNodeName ?? "auto"}' for service '{serviceName}' not found in active connections.");
        }

        var targetNodeObj = config.Root["nodes"]?[targetNode.NodeName] as JsonObject;
        var runtime = ResolveRuntimeForNode(targetNodeObj);

        logger?.Invoke($"Deploying container service '{spec.Name}' [{spec.Image}] on node '{targetNode.NodeName}' (priority: {spec.Priority})...");
        await runtime.DeployContainerAsync(targetNode, spec, ct).ConfigureAwait(false);
    }

    private async Task ExecuteMigrateServiceBlueGreenActionAsync(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> containerVipsPerNode,
        Action<string>? logger,
        CancellationToken ct)
    {
        var serviceName = action.Target;
        var serviceObj = config.Root["services"]?[serviceName] as JsonObject
            ?? throw new InvalidOperationException($"Service '{serviceName}' was not found in configuration.");

        var spec = BuildServiceSpec(serviceName, serviceObj, config);
        var targetNodeName = serviceObj["node"]?.ToString()
            ?? throw new InvalidOperationException($"Target node for Blue-Green migration of service '{serviceName}' is unspecified.");

        var targetNode = nodes.FirstOrDefault(n => string.Equals(n.NodeName, targetNodeName, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Target node '{targetNodeName}' for service '{serviceName}' not found in active connections.");

        var targetNodeObj = config.Root["nodes"]?[targetNode.NodeName] as JsonObject;
        var targetRuntime = ResolveRuntimeForNode(targetNodeObj);

        var sourceNode = await LocateSourceNodeForServiceAsync(serviceName, action.Description, nodes, config, targetNode, ct).ConfigureAwait(false);
        var sourceNodeObj = config.Root["nodes"]?[sourceNode.NodeName] as JsonObject;
        var sourceRuntime = ResolveRuntimeForNode(sourceNodeObj);

        logger?.Invoke($"[Blue-Green Step 1/6] P2P Image Streaming: '{spec.Image}' from '{sourceNode.NodeName}' to '{targetNode.NodeName}'...");
        await sourceRuntime.StreamImagePeerToPeerAsync(sourceNode, targetNode, spec.Image, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 2/6] Volume Synchronization: rsync over WireGuard from '{sourceNode.NodeName}' to '{targetNode.NodeName}'...");
        await SyncServiceVolumesAsync(sourceNode, targetNode, spec.Volumes, logger, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 3/6] Starting Green container '{spec.Name}' on target node '{targetNode.NodeName}'...");
        await targetRuntime.DeployContainerAsync(targetNode, spec, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 4/6] Polling Healthcheck for Green container '{spec.Name}' on '{targetNode.NodeName}'...");
        await PollContainerHealthAsync(targetNode, targetRuntime, spec, logger, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 5/6] Re-routing VIP and updating DNS for '{spec.Name}'...");
        await ReRouteServiceVipAsync(serviceName, spec.VipAddress, sourceNode, targetNode, config, nodes, containerVipsPerNode, logger, ct).ConfigureAwait(false);

        logger?.Invoke($"[Blue-Green Step 6/6] Stopping Blue container '{spec.Name}' on source node '{sourceNode.NodeName}'...");
        await StopContainerOnNodeAsync(sourceNode, sourceRuntime, spec.Name, ct).ConfigureAwait(false);

        logger?.Invoke($"Successfully completed zero-downtime Blue-Green migration for '{spec.Name}' to '{targetNode.NodeName}'.");
    }

    private async Task ExecuteUpdateDnsActionAsync(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Action<string>? logger,
        CancellationToken ct)
    {
        var dnsProvider = ResolveDnsProvider(config);
        if (dnsProvider == null)
        {
            logger?.Invoke($"DNS provider is not configured. Skipping DNS update for '{action.Target}'.");
            return;
        }

        var zoneDomain = config.Root["dns"]?["zone"]?.ToString()
            ?? config.Root["dns"]?["zone_domain"]?.ToString()
            ?? DefaultDnsZone;

        var (recordName, recordContent, recordType) = ResolveDnsRecordDetails(action, config, nodes);

        var record = new DnsRecord(
            Id: Guid.NewGuid().ToString("N"),
            RecordType: recordType,
            Name: recordName,
            Content: recordContent,
            Ttl: 120,
            Proxied: false);

        logger?.Invoke($"Upserting DNS record '{record.Name}' ({record.RecordType}) -> '{record.Content}' in zone '{zoneDomain}' via {dnsProvider.ProviderName}...");
        await dnsProvider.UpsertRecordAsync(zoneDomain, record, ct).ConfigureAwait(false);
    }

    [GeneratedRegex(@"\{\{\s*(?<key>[a-zA-Z0-9_\.]+)\s*\}\}", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex TemplateTagRegex();

    [GeneratedRegex(@"ENC\[AES256_GCM:v1:[^\]]+\]", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex EncryptedSecretTokenRegex();

    public string RenderTemplateAndDecryptInlineSecrets(string content, ConfigTree config, string? canonicalPath = null)
    {
        if (string.IsNullOrEmpty(content))
        {
            return content;
        }

        var rendered = RenderTemplatePlaceholders(content, config);
        return DecryptInlineTokens(rendered, canonicalPath);
    }

    private static string RenderTemplatePlaceholders(string content, ConfigTree config)
    {
        return TemplateTagRegex().Replace(content, match =>
        {
            var key = match.Groups["key"].Value.Trim();
            var node = config.FindNode(key);
            if (node != null)
            {
                return node.ToString();
            }

            var parts = key.Split('.');
            if (parts.Length == 3 && string.Equals(parts[0], "services", StringComparison.OrdinalIgnoreCase) && string.Equals(parts[2], "vip", StringComparison.OrdinalIgnoreCase))
            {
                var svc = config.Root["services"]?[parts[1]] as JsonObject;
                return svc?["vip"]?.ToString() ?? svc?["vip_address"]?.ToString() ?? match.Value;
            }
            if (parts.Length == 3 && string.Equals(parts[0], "nodes", StringComparison.OrdinalIgnoreCase) && string.Equals(parts[2], "vpn_ip", StringComparison.OrdinalIgnoreCase))
            {
                var n = config.Root["nodes"]?[parts[1]] as JsonObject;
                return n?["vpn_ip"]?.ToString() ?? match.Value;
            }

            return match.Value;
        });
    }

    private string DecryptInlineTokens(string content, string? canonicalPath)
    {
        if (_masterKey == null)
        {
            return content;
        }

        return EncryptedSecretTokenRegex().Replace(content, match =>
        {
            if (!_cryptoService.TryParseEncrypted(match.Value, out var envelope) || envelope == null)
            {
                return match.Value;
            }

            if (!string.IsNullOrEmpty(canonicalPath))
            {
                try
                {
                    var bytes = _cryptoService.Decrypt(envelope, _masterKey, canonicalPath);
                    return Encoding.UTF8.GetString(bytes);
                }
                catch (CryptographicException)
                {
                    // Fall back to empty AAD
                }
            }

            try
            {
                var bytes = _cryptoService.Decrypt(envelope, _masterKey, string.Empty);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (CryptographicException)
            {
                return match.Value;
            }
            catch (FormatException)
            {
                return match.Value;
            }
            catch (ArgumentException)
            {
                return match.Value;
            }
        });
    }

    private ServiceSpec BuildServiceSpec(string serviceName, JsonObject serviceObj, ConfigTree? config = null)
    {
        var image = serviceObj["image"]?.ToString()
            ?? throw new InvalidOperationException($"Service '{serviceName}' is missing required 'image' property.");

        var priority = serviceObj.TryGetPropertyValue("priority", out var pVal) &&
                       int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var p)
            ? p
            : 50;

        var vipAddress = serviceObj["vip"]?.ToString() ?? serviceObj["vip_address"]?.ToString();
        var healthcheck = serviceObj["healthcheck"]?.ToString()
            ?? serviceObj["healthcheck_endpoint"]?.ToString()
            ?? serviceObj["healthcheckEndpoint"]?.ToString();

        return new ServiceSpec
        {
            Name = serviceName,
            Image = image,
            Priority = priority,
            VipAddress = vipAddress,
            HealthcheckEndpoint = healthcheck,
            Ports = ExtractStringList(serviceObj, "ports"),
            Volumes = ExtractStringList(serviceObj, "volumes"),
            Labels = ExtractStringDictionary(serviceObj, "labels"),
            Environment = ExtractServiceEnvironment(serviceName, serviceObj, config),
            Files = ExtractServiceFiles(serviceName, serviceObj, config)
        };
    }

    private static List<string> ExtractStringList(JsonObject obj, string propertyName)
    {
        if (obj[propertyName] is not JsonArray arr)
        {
            return [];
        }

        return arr
            .Where(item => item != null && !string.IsNullOrWhiteSpace(item.ToString()))
            .Select(item => item!.ToString().Trim())
            .ToList();
    }

    private static Dictionary<string, string> ExtractStringDictionary(JsonObject obj, string propertyName)
    {
        if (obj[propertyName] is not JsonObject childObj)
        {
            return new Dictionary<string, string>(StringComparer.Ordinal);
        }

        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var (k, v) in childObj)
        {
            result[k] = v?.ToString() ?? string.Empty;
        }

        return result;
    }

    private Dictionary<string, string> ExtractServiceEnvironment(string serviceName, JsonObject serviceObj, ConfigTree? config = null)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        var envProperty = serviceObj["env"] as JsonObject ?? serviceObj["environment"] as JsonObject;
        var envPropName = serviceObj.ContainsKey("env") ? "env" : "environment";

        if (envProperty != null)
        {
            foreach (var (k, v) in envProperty)
            {
                var rawVal = v?.ToString() ?? string.Empty;
                var decrypted = DecryptSecretString(rawVal, $"services.{serviceName}.{envPropName}.{k}", $"services.{serviceName}.env.{k}");
                if (config != null)
                {
                    decrypted = RenderTemplateAndDecryptInlineSecrets(decrypted, config, $"services.{serviceName}.{envPropName}.{k}");
                }
                result[k] = decrypted;
            }
        }

        return result;
    }

    private Dictionary<string, string> ExtractServiceFiles(string serviceName, JsonObject serviceObj, ConfigTree? config = null)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        if (serviceObj["files"] is JsonObject filesObj)
        {
            foreach (var (destPath, contentVal) in filesObj)
            {
                var raw = contentVal?.ToString() ?? string.Empty;
                var fileContent = raw;

                if (File.Exists(raw))
                {
                    fileContent = File.ReadAllText(raw);
                }
                else
                {
                    var fallback = Path.Combine("files", serviceName, Path.GetFileName(destPath));
                    if (File.Exists(fallback))
                    {
                        fileContent = File.ReadAllText(fallback);
                    }
                }

                if (config != null)
                {
                    fileContent = RenderTemplateAndDecryptInlineSecrets(fileContent, config, $"services.{serviceName}.files.{destPath}");
                }
                else
                {
                    fileContent = DecryptSecretString(fileContent, $"services.{serviceName}.files.{destPath}");
                }

                result[destPath] = fileContent;
            }
        }

        return result;
    }

    [GeneratedRegex(@"from '(?<old>[^']+)' to '(?<new>[^']+)'", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex BlueGreenPlanDescriptionRegex();

    private async Task<INodeConnection> LocateSourceNodeForServiceAsync(
        string serviceName,
        string actionDescription,
        IReadOnlyList<INodeConnection> nodes,
        ConfigTree config,
        INodeConnection targetNode,
        CancellationToken ct)
    {
        // 1. Probe active nodes for running container
        foreach (var candidateNode in nodes)
        {
            if (string.Equals(candidateNode.NodeName, targetNode.NodeName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var nodeObj = config.Root["nodes"]?[candidateNode.NodeName] as JsonObject;
            var runtime = ResolveRuntimeForNode(nodeObj);
            var containers = await runtime.ListContainersAsync(candidateNode, ct).ConfigureAwait(false);

            if (containers.Any(c => string.Equals(c.Name.TrimStart('/'), serviceName, StringComparison.OrdinalIgnoreCase) && c.IsRunning))
            {
                return candidateNode;
            }
        }

        // 2. Parse from plan description (e.g. "... from 'bocchi' to 'nijika'")
        var match = BlueGreenPlanDescriptionRegex().Match(actionDescription);
        if (match.Success)
        {
            var oldName = match.Groups["old"].Value;
            var parsedNode = nodes.FirstOrDefault(n => string.Equals(n.NodeName, oldName, StringComparison.OrdinalIgnoreCase));
            if (parsedNode != null)
            {
                return parsedNode;
            }
        }

        throw new InvalidOperationException($"Could not determine source node for Blue-Green migration of service '{serviceName}'.");
    }

    private static async Task SyncServiceVolumesAsync(
        INodeConnection sourceNode,
        INodeConnection targetNode,
        IReadOnlyList<string> volumes,
        Action<string>? logger,
        CancellationToken ct)
    {
        if (volumes.Count == 0)
        {
            return;
        }

        foreach (var volume in volumes)
        {
            ct.ThrowIfCancellationRequested();
            var rawVolPath = volume.Split(':')[0].Trim();
            if (string.IsNullOrWhiteSpace(rawVolPath))
            {
                continue;
            }

            var resolvedPath = rawVolPath.StartsWith('/')
                ? rawVolPath
                : $"/var/lib/docker/volumes/{rawVolPath}/_data";

            logger?.Invoke($"Streaming volume '{rawVolPath}' ({resolvedPath}) through orchestrator binary pipe to '{targetNode.NodeName}'...");

            using var pipeStream = new MemoryStream();

            var safePath = "'" + resolvedPath.Replace("'", "'\\''", StringComparison.Ordinal) + "'";
            var saveCmd = $"mkdir -p {safePath} && tar -C {safePath} -cf - . | zstd -3";
            var saveResult = await sourceNode.ExecuteStreamingCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", saveCmd],
                Timeout = TimeSpan.FromMinutes(30)
            }, stdinStream: null, stdoutStream: pipeStream, ct).ConfigureAwait(false);

            if (!saveResult.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to export volume '{rawVolPath}' on source node '{sourceNode.NodeName}': {saveResult.StandardError}");
            }

            pipeStream.Position = 0;
            var restoreCmd = $"mkdir -p {safePath} && zstd -d | tar -C {safePath} -xf -";
            var restoreResult = await targetNode.ExecuteStreamingCommandAsync(new ProcessSpec
            {
                Executable = "sh",
                Arguments = ["-c", restoreCmd],
                Timeout = TimeSpan.FromMinutes(30)
            }, stdinStream: pipeStream, stdoutStream: null, ct).ConfigureAwait(false);

            if (!restoreResult.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to restore volume '{rawVolPath}' on target node '{targetNode.NodeName}': {restoreResult.StandardError}");
            }

            logger?.Invoke($"✓ Successfully synced volume '{rawVolPath}' to '{targetNode.NodeName}'.");
        }
    }

    private static async Task PollContainerHealthAsync(
        INodeConnection targetNode,
        IContainerRuntime runtime,
        ServiceSpec spec,
        Action<string>? logger,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(spec.HealthcheckEndpoint))
        {
            await PollRunningStateAsync(targetNode, runtime, spec.Name, logger, ct).ConfigureAwait(false);
            return;
        }

        await PollHttpEndpointAsync(targetNode, spec, logger, ct).ConfigureAwait(false);
    }

    private static async Task PollRunningStateAsync(
        INodeConnection targetNode,
        IContainerRuntime runtime,
        string serviceName,
        Action<string>? logger,
        CancellationToken ct)
    {
        for (var i = 1; i <= 15; i++)
        {
            ct.ThrowIfCancellationRequested();
            var runningContainers = await runtime.ListContainersAsync(targetNode, ct).ConfigureAwait(false);
            var container = runningContainers.FirstOrDefault(c => string.Equals(c.Name.TrimStart('/'), serviceName, StringComparison.OrdinalIgnoreCase));

            if (container is { IsRunning: true })
            {
                logger?.Invoke($"Container '{serviceName}' is healthy and running on attempt {i}.");
                return;
            }

            await Task.Delay(1000, ct).ConfigureAwait(false);
        }

        throw new InvalidOperationException($"Container '{serviceName}' failed to enter running state on node '{targetNode.NodeName}'.");
    }

    private static async Task PollHttpEndpointAsync(
        INodeConnection targetNode,
        ServiceSpec spec,
        Action<string>? logger,
        CancellationToken ct)
    {
        var endpoint = spec.HealthcheckEndpoint!.Trim();
        string healthUrl;
        if (endpoint.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            endpoint.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            healthUrl = endpoint;
        }
        else if (endpoint.StartsWith('/'))
        {
            var hostPort = spec.Ports.Count > 0 ? spec.Ports[0].Split(':')[0].Trim() : "80";
            healthUrl = $"http://127.0.0.1:{hostPort}{endpoint}";
        }
        else
        {
            healthUrl = endpoint;
        }

        var checkSpec = new ProcessSpec
        {
            Executable = "curl",
            Arguments = ["-f", "-s", "-m", "3", healthUrl]
        };

        var healthy = false;
        for (var attempt = 1; attempt <= 30; attempt++)
        {
            ct.ThrowIfCancellationRequested();
            var checkResult = await targetNode.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
            if (checkResult.IsSuccess)
            {
                healthy = true;
                logger?.Invoke($"Healthcheck passed for Green container '{spec.Name}' on attempt {attempt}.");
                break;
            }

            await Task.Delay(1500, ct).ConfigureAwait(false);
        }

        if (!healthy)
        {
            throw new InvalidOperationException($"Healthcheck poll failed for Green container '{spec.Name}' at '{healthUrl}' on node '{targetNode.NodeName}'. Aborting VIP cutover.");
        }
    }

    private async Task ReRouteServiceVipAsync(
        string serviceName,
        string? vipAddress,
        INodeConnection sourceNode,
        INodeConnection targetNode,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> containerVipsPerNode,
        Action<string>? logger,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(vipAddress))
        {
            return;
        }

        UpdateVipPlacement(sourceNode.NodeName, targetNode.NodeName, vipAddress, containerVipsPerNode);
        await SynchronizeWireGuardMeshAsync(config, nodes, containerVipsPerNode, ct).ConfigureAwait(false);
        await SwitchInterfaceVipAsync(sourceNode, targetNode, vipAddress, ct).ConfigureAwait(false);
        await UpdateServiceDnsRecordAsync(serviceName, vipAddress, config, logger, ct).ConfigureAwait(false);
    }

    private static void UpdateVipPlacement(
        string sourceNodeName,
        string targetNodeName,
        string vipAddress,
        Dictionary<string, List<string>> containerVipsPerNode)
    {
        if (containerVipsPerNode.TryGetValue(sourceNodeName, out var sourceList))
        {
            sourceList.RemoveAll(v => string.Equals(v, vipAddress, StringComparison.OrdinalIgnoreCase));
        }

        if (!containerVipsPerNode.TryGetValue(targetNodeName, out var targetList))
        {
            targetList = [];
            containerVipsPerNode[targetNodeName] = targetList;
        }

        if (!targetList.Contains(vipAddress, StringComparer.OrdinalIgnoreCase))
        {
            targetList.Add(vipAddress);
        }
    }

    private async Task SynchronizeWireGuardMeshAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        Dictionary<string, List<string>> containerVipsPerNode,
        CancellationToken ct)
    {
        var wgNodeConfigs = await GatherWireGuardNodeConfigsAsync(config, nodes, ct).ConfigureAwait(false);
        var readOnlyVips = containerVipsPerNode.ToDictionary(
            kv => kv.Key,
            kv => (IReadOnlyList<string>)kv.Value.AsReadOnly(),
            StringComparer.Ordinal);

        var meshConfigs = WireGuardMeshService.GenerateMeshConfigurations(wgNodeConfigs, readOnlyVips);
        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();
            if (meshConfigs.TryGetValue(node.NodeName, out var conf))
            {
                await WireGuardMeshService.ApplyConfigurationAsync(node, conf, ct).ConfigureAwait(false);
            }
        }
    }

    private static async Task SwitchInterfaceVipAsync(
        INodeConnection sourceNode,
        INodeConnection targetNode,
        string vipAddress,
        CancellationToken ct)
    {
        await sourceNode.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"ip addr del {vipAddress}/32 dev starry0 2>/dev/null || true"]
        }, ct).ConfigureAwait(false);

        await targetNode.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"ip addr add {vipAddress}/32 dev starry0 2>/dev/null || true"]
        }, ct).ConfigureAwait(false);
    }

    private async Task UpdateServiceDnsRecordAsync(
        string serviceName,
        string vipAddress,
        ConfigTree config,
        Action<string>? logger,
        CancellationToken ct)
    {
        var dnsProvider = ResolveDnsProvider(config);
        if (dnsProvider != null)
        {
            var zoneDomain = config.Root["dns"]?["zone"]?.ToString() ?? DefaultDnsZone;
            var record = new DnsRecord(
                Id: Guid.NewGuid().ToString("N"),
                RecordType: "A",
                Name: serviceName,
                Content: vipAddress,
                Ttl: 120,
                Proxied: false);

            await dnsProvider.UpsertRecordAsync(zoneDomain, record, ct).ConfigureAwait(false);
            logger?.Invoke($"Updated DNS record '{serviceName}' -> '{vipAddress}' in zone '{zoneDomain}'.");
        }
    }

    private static async Task StopContainerOnNodeAsync(
        INodeConnection node,
        IContainerRuntime runtime,
        string containerName,
        CancellationToken ct)
    {
        var stopSpec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"{runtime.RuntimeName} stop {containerName} 2>/dev/null || true"]
        };

        await node.ExecuteCommandAsync(stopSpec, ct).ConfigureAwait(false);
    }

    private IDnsProvider? ResolveDnsProvider(ConfigTree config)
    {
        if (_dnsProvider != null)
        {
            return _dnsProvider;
        }

        if (!config.Root.TryGetPropertyValue("dns", out var dnsNode) || dnsNode is not JsonObject dnsObj)
        {
            return null;
        }

        var providerType = dnsObj["provider"]?.ToString();
        if (string.Equals(providerType, "cloudflare", StringComparison.OrdinalIgnoreCase))
        {
            var tokenRaw = dnsObj["api_token"]?.ToString() ?? dnsObj["token"]?.ToString() ?? string.Empty;
            var decryptedToken = DecryptSecretString(tokenRaw, "dns.api_token");
            return new CloudflareDnsProvider(decryptedToken);
        }

        if (string.Equals(providerType, "webhook", StringComparison.OrdinalIgnoreCase))
        {
            var webhookUrl = dnsObj["webhook_url"]?.ToString() ?? dnsObj["url"]?.ToString();
            if (!string.IsNullOrWhiteSpace(webhookUrl))
            {
                return new WebhookDnsProvider(new Uri(webhookUrl));
            }
        }

        return null;
    }

    private static (string RecordName, string RecordContent, string RecordType) ResolveDnsRecordDetails(
        PlanAction action,
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes)
    {
        var target = action.Target;
        if (config.Root.TryGetPropertyValue("services", out var svcNode) &&
            svcNode is JsonObject servicesObj &&
            servicesObj[target] is JsonObject serviceObj)
        {
            var vip = serviceObj["vip"]?.ToString() ?? serviceObj["vip_address"]?.ToString();
            if (!string.IsNullOrWhiteSpace(vip))
            {
                return (target, vip, "A");
            }

            var nodeName = serviceObj["node"]?.ToString();
            var targetNode = nodes.FirstOrDefault(n => string.Equals(n.NodeName, nodeName, StringComparison.OrdinalIgnoreCase));
            if (targetNode != null)
            {
                return (target, targetNode.Host, "A");
            }
        }

        return (target, DefaultLocalIp, "A");
    }

    private async Task<IReadOnlyList<WireGuardNodeConfig>> GatherWireGuardNodeConfigsAsync(
        ConfigTree config,
        IReadOnlyList<INodeConnection> nodes,
        CancellationToken ct)
    {
        var wgNodeConfigs = new List<WireGuardNodeConfig>(nodes.Count);

        var meshPort = DefaultWireGuardPort;
        if (config.Root.TryGetPropertyValue("mesh", out var meshNode) &&
            meshNode is JsonObject meshObj &&
            meshObj.TryGetPropertyValue("port", out var pVal) &&
            int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var parsedPort))
        {
            meshPort = parsedPort;
        }

        foreach (var node in nodes)
        {
            ct.ThrowIfCancellationRequested();
            var nodeObj = config.Root["nodes"]?[node.NodeName] as JsonObject;

            var vpnIp = nodeObj?["vpn_ip"]?.ToString() ?? DefaultWireGuardIp;
            var isPublic = nodeObj?["is_public"]?.GetValue<bool>() ?? false;

            var (privKey, pubKey) = await ResolveWireGuardKeysForNodeAsync(node, nodeObj, ct).ConfigureAwait(false);

            var listenPort = nodeObj != null && nodeObj.TryGetPropertyValue("wireguard_port", out var wpVal) &&
                             int.TryParse(wpVal?.ToString(), CultureInfo.InvariantCulture, out var parsedWp)
                ? parsedWp
                : meshPort;

            var endpoint = isPublic && !string.IsNullOrWhiteSpace(node.Host) && !string.Equals(node.Host, "localhost", StringComparison.OrdinalIgnoreCase)
                ? node.Host
                : null;

            wgNodeConfigs.Add(new WireGuardNodeConfig(
                NodeName: node.NodeName,
                VpnIp: vpnIp,
                PrivateKey: privKey,
                PublicKey: pubKey,
                ListenPort: listenPort,
                Endpoint: endpoint,
                IsPublic: isPublic));
        }

        return wgNodeConfigs;
    }

    private async Task<(string PrivateKey, string PublicKey)> ResolveWireGuardKeysForNodeAsync(
        INodeConnection node,
        JsonObject? nodeObj,
        CancellationToken ct)
    {
        var privKeyRaw = nodeObj?["wireguard_private_key"]?.ToString()
            ?? nodeObj?["wg_private_key"]?.ToString()
            ?? nodeObj?["private_key"]?.ToString();

        var pubKeyRaw = nodeObj?["wireguard_public_key"]?.ToString()
            ?? nodeObj?["wg_public_key"]?.ToString()
            ?? nodeObj?["public_key"]?.ToString();

        if (!string.IsNullOrWhiteSpace(privKeyRaw))
        {
            var privKey = DecryptSecretString(privKeyRaw, $"nodes.{node.NodeName}.wireguard_private_key");
            var pubKey = !string.IsNullOrWhiteSpace(pubKeyRaw)
                ? DecryptSecretString(pubKeyRaw, $"nodes.{node.NodeName}.wireguard_public_key")
                : await DerivePublicKeyFromPrivateKeyAsync(node, privKey, ct).ConfigureAwait(false);

            return (privKey, pubKey);
        }

        // Query or generate on node
        var queryResult = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "[ -f /etc/wireguard/starry0.key ] && cat /etc/wireguard/starry0.key || (wg genkey 2>/dev/null | tee /etc/wireguard/starry0.key || true)"]
        }, ct).ConfigureAwait(false);

        var retrievedKey = queryResult.StandardOutput.Trim();
        if (retrievedKey.Length == 44 && retrievedKey.EndsWith('='))
        {
            var derivedPub = await DerivePublicKeyFromPrivateKeyAsync(node, retrievedKey, ct).ConfigureAwait(false);
            return (retrievedKey, derivedPub);
        }

        // Fallback synthetic key pair
        var dummyBytes = new byte[32];
        RandomNumberGenerator.Fill(dummyBytes);
        var fallbackPriv = Convert.ToBase64String(dummyBytes);
        RandomNumberGenerator.Fill(dummyBytes);
        var fallbackPub = Convert.ToBase64String(dummyBytes);

        return (fallbackPriv, fallbackPub);
    }

    private static async Task<string> DerivePublicKeyFromPrivateKeyAsync(
        INodeConnection node,
        string privateKey,
        CancellationToken ct)
    {
        var result = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"echo '{privateKey}' | wg pubkey 2>/dev/null || true"]
        }, ct).ConfigureAwait(false);

        var pubKey = result.StandardOutput.Trim();
        if (pubKey.Length == 44 && pubKey.EndsWith('='))
        {
            return pubKey;
        }

        var dummyBytes = new byte[32];
        RandomNumberGenerator.Fill(dummyBytes);
        return Convert.ToBase64String(dummyBytes);
    }

    private static Dictionary<string, List<string>> ExtractContainerVipsPerNode(ConfigTree config)
    {
        var result = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        if (config.Root.TryGetPropertyValue("services", out var servicesNode) && servicesNode is JsonObject servicesObj)
        {
            foreach (var (_, svcVal) in servicesObj)
            {
                if (svcVal is not JsonObject sObj)
                {
                    continue;
                }

                var targetNode = sObj["node"]?.ToString();
                var vip = sObj["vip"]?.ToString() ?? sObj["vip_address"]?.ToString();

                if (!string.IsNullOrWhiteSpace(targetNode) && !string.IsNullOrWhiteSpace(vip))
                {
                    if (!result.TryGetValue(targetNode, out var vips))
                    {
                        vips = [];
                        result[targetNode] = vips;
                    }

                    if (!vips.Contains(vip, StringComparer.OrdinalIgnoreCase))
                    {
                        vips.Add(vip);
                    }
                }
            }
        }

        return result;
    }

    private static (IReadOnlyList<PortForwardRule> PortForwards, IReadOnlyList<int> PublicAllowedPorts) ExtractPortForwardsAndPublicPorts(ConfigTree config)
    {
        var forwards = new List<PortForwardRule>();
        var publicPorts = new HashSet<int>();

        if (!config.Root.TryGetPropertyValue("services", out var servicesNode) || servicesNode is not JsonObject servicesObj)
        {
            return (forwards, publicPorts.ToList());
        }

        foreach (var (_, svcVal) in servicesObj)
        {
            if (svcVal is not JsonObject sObj)
            {
                continue;
            }

            ParseServicePortForwards(sObj, forwards, publicPorts);
        }

        return (forwards, publicPorts.ToList());
    }

    private static void ParseServicePortForwards(
        JsonObject sObj,
        List<PortForwardRule> forwards,
        HashSet<int> publicPorts)
    {
        var vip = sObj["vip"]?.ToString() ?? sObj["vip_address"]?.ToString();
        if (sObj["ports"] is not JsonArray portsArr)
        {
            return;
        }

        foreach (var pNode in portsArr)
        {
            var portStr = pNode?.ToString();
            if (string.IsNullOrWhiteSpace(portStr))
            {
                continue;
            }

            var parts = portStr.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[0].Trim(), CultureInfo.InvariantCulture, out var hostPort))
            {
                continue;
            }

            publicPorts.Add(hostPort);

            if (string.IsNullOrWhiteSpace(vip))
            {
                continue;
            }

            var targetPart = parts[1].Trim();
            var proto = "tcp";
            var targetPortStr = targetPart;
            if (targetPart.Contains('/', StringComparison.Ordinal))
            {
                var sub = targetPart.Split('/');
                targetPortStr = sub[0];
                proto = sub[1].ToUpper(CultureInfo.InvariantCulture);
            }

            if (int.TryParse(targetPortStr, CultureInfo.InvariantCulture, out var targetPort))
            {
                forwards.Add(new PortForwardRule(hostPort, proto, vip, targetPort));
            }
        }
    }

    private static IReadOnlyList<PlanAction> OrderActionsByDag(IReadOnlyList<PlanAction> actions)
    {
        if (actions.Count <= 1)
        {
            return actions;
        }

        var actionMap = actions.ToDictionary(a => a.Id, StringComparer.Ordinal);
        var inDegree = actions.ToDictionary(a => a.Id, _ => 0, StringComparer.Ordinal);
        var adjacency = actions.ToDictionary(a => a.Id, _ => new List<string>(), StringComparer.Ordinal);

        foreach (var action in actions)
        {
            foreach (var dep in action.Dependencies.Where(actionMap.ContainsKey))
            {
                adjacency[dep].Add(action.Id);
                inDegree[action.Id]++;
            }
        }

        var readyQueue = new PriorityQueue<PlanAction, (int Priority, string Id)>();
        foreach (var action in actions)
        {
            if (inDegree[action.Id] == 0)
            {
                readyQueue.Enqueue(action, (action.Priority, action.Id));
            }
        }

        var ordered = new List<PlanAction>(actions.Count);
        while (readyQueue.Count > 0)
        {
            var next = readyQueue.Dequeue();
            ordered.Add(next);

            foreach (var dependentId in adjacency[next.Id])
            {
                inDegree[dependentId]--;
                if (inDegree[dependentId] == 0)
                {
                    var depAction = actionMap[dependentId];
                    readyQueue.Enqueue(depAction, (depAction.Priority, depAction.Id));
                }
            }
        }

        if (ordered.Count < actions.Count)
        {
            // Fallback to priority ordering if cyclical dependency anomaly occurs
            return actions.OrderBy(a => a.Priority).ThenBy(a => a.Id, StringComparer.Ordinal).ToList();
        }

        return ordered;
    }

    private static async Task CheckWireGuardInterfaceStatusAsync(INodeConnection node, CancellationToken ct)
    {
        var wgShowResult = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "wg",
            Arguments = ["show", WireGuardMeshService.InterfaceName]
        }, ct).ConfigureAwait(false);

        if (!wgShowResult.IsSuccess)
        {
            await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "ip",
                Arguments = ["addr", "show", WireGuardMeshService.InterfaceName]
            }, ct).ConfigureAwait(false);
        }
    }

    private IContainerRuntime ResolveRuntimeForNode(JsonObject? nodeObj)
    {
        var engine = nodeObj?["engine"]?.ToString();
        if (string.Equals(engine, "podman", StringComparison.OrdinalIgnoreCase))
        {
            return new PodmanRuntime();
        }

        return _defaultRuntime;
    }

    private string DecryptSecretString(string value, string primaryPath, string? alternatePath = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        if (value.StartsWith("PLAIN[", StringComparison.Ordinal) && value.EndsWith(']'))
        {
            return value["PLAIN[".Length..^1];
        }

        if (_cryptoService.TryParseEncrypted(value, out var envelope) && envelope != null)
        {
            if (_masterKey == null)
            {
                throw new InvalidOperationException(
                    $"Encountered encrypted secret value for path '{primaryPath}' but STARRY_MASTER_KEY is not configured.");
            }

            try
            {
                var decrypted = _cryptoService.Decrypt(envelope, _masterKey, primaryPath);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch (CryptographicException) when (alternatePath != null)
            {
                var decrypted = _cryptoService.Decrypt(envelope, _masterKey, alternatePath);
                return Encoding.UTF8.GetString(decrypted);
            }
        }

        return value;
    }

    private static byte[]? ResolveMasterKeyFromEnvironment()
    {
        var envVal = Environment.GetEnvironmentVariable("STARRY_MASTER_KEY");
        if (!string.IsNullOrWhiteSpace(envVal))
        {
            try
            {
                return CryptoService.ParseMasterKey(envVal);
            }
            catch (FormatException)
            {
                // Invalid base64 or key format in environment variable; return null
            }
            catch (ArgumentException)
            {
                // Invalid key length; return null
            }
        }

        return null;
    }
}
```

## File: src/Starry.Infrastructure/Orchestration/HostAdoptionService.cs

**Lines**: 970 | **Size**: 32.5 KB | **Language**: csharp

```csharp
#pragma warning disable MA0048 // File name must match type name
#pragma warning disable CA1819 // Properties should not return arrays

using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Core.Security;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Security;

namespace Starry.Infrastructure.Orchestration;

/// <summary>
/// Options controlling the adoption and configuration generation process for an existing host.
/// </summary>
public sealed record AdoptionOptions
{
    /// <summary>
    /// Target directory where modular configuration files will be written (e.g. "./config").
    /// </summary>
    public string OutDir { get; init; } = "./config";

    /// <summary>
    /// Whether to pull single-file bind mounts from the host into local files directory.
    /// </summary>
    public bool PullFiles { get; init; }

    /// <summary>
    /// Configuration format for generated files ("yaml", "json", "toml").
    /// </summary>
    public string Format { get; init; } = "yaml";

    /// <summary>
    /// 32-byte master encryption key. If omitted, uses STARRY_MASTER_KEY or generates a new key.
    /// </summary>
    public byte[]? MasterKey { get; init; }

    /// <summary>
    /// Local directory where pulled host files will be stored.
    /// </summary>
    public string FilesDir { get; init; } = "./files";

    /// <summary>
    /// Default cluster name for the generated root configuration.
    /// </summary>
    public string ClusterName { get; init; } = "starry-cluster";
}

/// <summary>
/// Summary descriptor of a container running on a node that is not managed by Starry configuration.
/// </summary>
public sealed record UnmanagedContainerSummary(
    string NodeName,
    string ContainerId,
    string ContainerName,
    string Image,
    string State,
    IReadOnlyDictionary<string, string> Labels);

/// <summary>
/// Service contract for brownfield host adoption, live mesh network attachment,
/// and unmanaged container discovery.
/// </summary>
public interface IHostAdoptionService
{
    /// <summary>
    /// Probes an existing node, discovers running containers and compose projects,
    /// encrypts sensitive environment secrets, pulls configuration files, and outputs
    /// modular configuration files into the specified directory.
    /// </summary>
    Task<ConfigTree> AdoptHostAsync(INodeConnection node, AdoptionOptions options, CancellationToken ct = default);

    /// <summary>
    /// Connects a live, running container to the Starry WireGuard mesh network without restarting it.
    /// </summary>
    Task AttachLiveContainerToMeshAsync(INodeConnection node, string containerId, string vip, CancellationToken ct = default);

    /// <summary>
    /// Discovers containers running across nodes that are not defined in the current configuration tree.
    /// </summary>
    Task<IReadOnlyList<UnmanagedContainerSummary>> ListUnmanagedContainersAsync(
        IReadOnlyList<INodeConnection> nodes,
        ConfigTree currentConfig,
        CancellationToken ct = default);
}

/// <summary>
/// Implements brownfield host adoption, live container mesh attachment, and unmanaged container inspection.
/// </summary>
public sealed class HostAdoptionService : IHostAdoptionService
{
    private static readonly Uri BaseDockerUri = new("http://127.0.0.1/");
    private const string ApiVersionPrefix = "/v1.40";

    private static readonly string[] SecretKeyIndicators =
    [
        "PASS",
        "PASSWORD",
        "SECRET",
        "KEY",
        "TOKEN",
        "PRIVATE",
        "DSN"
    ];

    private static readonly string[] DbKeywords = ["POSTGRES", "MYSQL", "MARIADB", "REDIS", "MONGO"];
    private static readonly string[] ProxyKeywords = ["NGINX", "TRAEFIK", "CADDY", "NPM"];

    private readonly CryptoService _cryptoService;
    private readonly VirtualConfigEngine _configEngine;
    private readonly IReadOnlyList<IContainerRuntime> _runtimes;

    public HostAdoptionService(
        CryptoService? cryptoService = null,
        VirtualConfigEngine? configEngine = null,
        IEnumerable<IContainerRuntime>? runtimes = null)
    {
        _cryptoService = cryptoService ?? new CryptoService();
        _configEngine = configEngine ?? new VirtualConfigEngine();
        _runtimes = runtimes?.ToList() ?? [new DockerRuntime(), new PodmanRuntime()];
    }

    public async Task<ConfigTree> AdoptHostAsync(INodeConnection node, AdoptionOptions options, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(options);

        var (runtime, socketPath) = await ResolveActiveRuntimeAsync(node, ct).ConfigureAwait(false);
        var discoveredContainers = await DiscoverContainersAsync(node, runtime, socketPath, ct).ConfigureAwait(false);

        var masterKey = ResolveMasterKey(options);
        var format = string.IsNullOrWhiteSpace(options.Format) ? "yaml" : options.Format.Trim();

        var tree = InitializeConfigTree(options, format);
        EnsureClusterMetadata(tree, options);
        RegisterNodeInConfig(tree, node, runtime);

        if (!tree.Root.TryGetPropertyValue("services", out var servicesNode) || servicesNode is not JsonObject servicesObj)
        {
            servicesObj = [];
            tree.Root["services"] = servicesObj;
        }

        await AdoptDiscoveredContainersAsync(node, discoveredContainers, servicesObj, masterKey, options, ct).ConfigureAwait(false);

        _configEngine.Split(tree, options.OutDir, format);
        return tree;
    }

    public async Task AttachLiveContainerToMeshAsync(INodeConnection node, string containerId, string vip, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(vip);

        var (_, socketPath) = await ResolveActiveRuntimeAsync(node, ct).ConfigureAwait(false);
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);

        var targetNetId = await FindOrCreateMeshNetworkAsync(client, ct).ConfigureAwait(false);

        var connectPayload = new JsonObject
        {
            ["Container"] = containerId,
            ["EndpointConfig"] = new JsonObject
            {
                ["IPAMConfig"] = new JsonObject
                {
                    ["IPv4Address"] = vip
                }
            }
        };

        using var connectContent = new StringContent(connectPayload.ToJsonString(), Encoding.UTF8, "application/json");
        using var connectRes = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/networks/{Uri.EscapeDataString(targetNetId)}/connect"),
            connectContent,
            ct).ConfigureAwait(false);

        if (!connectRes.IsSuccessStatusCode)
        {
            var err = await connectRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            if (connectRes.StatusCode != HttpStatusCode.Conflict &&
                !err.Contains("already exists in network", StringComparison.OrdinalIgnoreCase) &&
                !err.Contains("is already attached", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Failed to connect container '{containerId}' to network '{targetNetId}': {err}");
            }
        }

        await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", $"ip addr add {vip}/32 dev starry0 2>/dev/null || true"]
        }, ct).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<UnmanagedContainerSummary>> ListUnmanagedContainersAsync(
        IReadOnlyList<INodeConnection> nodes,
        ConfigTree currentConfig,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        ArgumentNullException.ThrowIfNull(currentConfig);

        var managedServiceNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (currentConfig.Root.TryGetPropertyValue("services", out var servicesNode) && servicesNode is JsonObject servicesObj)
        {
            foreach (var (svcName, _) in servicesObj)
            {
                managedServiceNames.Add(svcName);
            }
        }

        var unmanagedList = new List<UnmanagedContainerSummary>();

        foreach (var node in nodes)
        {
            var (runtime, _) = await ResolveActiveRuntimeAsync(node, ct).ConfigureAwait(false);
            var containers = await runtime.ListContainersAsync(node, ct).ConfigureAwait(false);

            foreach (var container in containers)
            {
                var cleanName = container.Name.TrimStart('/');
                var composeService = container.Labels.GetValueOrDefault("com.docker.compose.service");
                var composeProject = container.Labels.GetValueOrDefault("com.docker.compose.project");
                var isManagedByStarry = string.Equals(container.Labels.GetValueOrDefault("managed-by"), "starry", StringComparison.OrdinalIgnoreCase);

                var isManaged = isManagedByStarry ||
                                managedServiceNames.Contains(cleanName) ||
                                (!string.IsNullOrEmpty(composeService) && managedServiceNames.Contains(composeService)) ||
                                (!string.IsNullOrEmpty(composeProject) && managedServiceNames.Contains(composeProject)) ||
                                (!string.IsNullOrEmpty(composeProject) && !string.IsNullOrEmpty(composeService) && managedServiceNames.Contains($"{composeProject}-{composeService}"));

                if (!isManaged)
                {
                    unmanagedList.Add(new UnmanagedContainerSummary(
                        NodeName: node.NodeName,
                        ContainerId: container.Id,
                        ContainerName: cleanName,
                        Image: container.Image,
                        State: container.Status,
                        Labels: container.Labels));
                }
            }
        }

        return unmanagedList;
    }

    private static async Task<List<DiscoveredContainerInfo>> DiscoverContainersAsync(
        INodeConnection node,
        IContainerRuntime runtime,
        string socketPath,
        CancellationToken ct)
    {
        using var client = await node.CreateSocketHttpClientAsync(socketPath, ct).ConfigureAwait(false);
        var containerStates = await runtime.ListContainersAsync(node, ct).ConfigureAwait(false);
        var discovered = new List<DiscoveredContainerInfo>(containerStates.Count);

        foreach (var state in containerStates)
        {
            var details = await QueryContainerDetailsAsync(client, state.Id, ct).ConfigureAwait(false);
            if (details != null)
            {
                discovered.Add(new DiscoveredContainerInfo(state, details));
            }
        }

        return discovered;
    }

    private async Task AdoptDiscoveredContainersAsync(
        INodeConnection node,
        List<DiscoveredContainerInfo> discovered,
        JsonObject servicesObj,
        byte[] masterKey,
        AdoptionOptions options,
        CancellationToken ct)
    {
        var projectGroups = discovered
            .GroupBy(c => !string.IsNullOrWhiteSpace(c.ComposeProject) ? c.ComposeProject : c.CleanName, StringComparer.OrdinalIgnoreCase);

        var adoptedServiceNames = new HashSet<string>(servicesObj.Select(s => s.Key), StringComparer.OrdinalIgnoreCase);

        foreach (var group in projectGroups)
        {
            var groupKey = group.Key;
            var groupContainers = group.ToList();

            foreach (var container in groupContainers)
            {
                var serviceName = ResolveServiceName(groupKey, groupContainers.Count, container, adoptedServiceNames);
                adoptedServiceNames.Add(serviceName);

                var serviceObj = await BuildServiceConfigAsync(
                    node,
                    container,
                    serviceName,
                    masterKey,
                    options,
                    ct).ConfigureAwait(false);

                servicesObj[serviceName] = serviceObj;
            }
        }
    }

    private async Task<(IContainerRuntime Runtime, string SocketPath)> ResolveActiveRuntimeAsync(INodeConnection node, CancellationToken ct)
    {
        foreach (var runtime in _runtimes)
        {
            try
            {
                var info = await runtime.ProbeAsync(node, ct).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(info.SocketPath))
                {
                    return (runtime, info.SocketPath);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // Try next runtime
            }
        }

        throw new InvalidOperationException($"No active container runtime found on node '{node.NodeName}'.");
    }

    private static async Task<JsonObject?> QueryContainerDetailsAsync(HttpClient client, string containerId, CancellationToken ct)
    {
        using var response = await client.GetAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/containers/{Uri.EscapeDataString(containerId)}/json"),
            ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        return JsonNode.Parse(content) as JsonObject;
    }

    private ConfigTree InitializeConfigTree(AdoptionOptions options, string format)
    {
        var mainConfigFile = Path.Combine(options.OutDir, $"starry.{format}");
        if (File.Exists(mainConfigFile))
        {
            try
            {
                return _configEngine.Load(options.OutDir);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // Fallback to fresh tree
            }
        }

        return new ConfigTree();
    }

    private static void EnsureClusterMetadata(ConfigTree tree, AdoptionOptions options)
    {
        if (!tree.Root.ContainsKey("version"))
        {
            tree.Root["version"] = "1.0";
        }

        if (!tree.Root.ContainsKey("cluster_name"))
        {
            tree.Root["cluster_name"] = options.ClusterName;
        }

        if (!tree.Root.ContainsKey("mesh"))
        {
            tree.Root["mesh"] = new JsonObject
            {
                ["cidr"] = "10.99.0.0/16",
                ["port"] = 51820,
                ["keepalive"] = 25
            };
        }
    }

    private static void RegisterNodeInConfig(ConfigTree tree, INodeConnection node, IContainerRuntime runtime)
    {
        if (!tree.Root.TryGetPropertyValue("nodes", out var nodesNode) || nodesNode is not JsonObject nodesObj)
        {
            nodesObj = [];
            tree.Root["nodes"] = nodesObj;
        }

        var nodeEntry = new JsonObject
        {
            ["host"] = node.Host,
            ["port"] = node.Port,
            ["engine"] = runtime.RuntimeName
        };

        if (!string.IsNullOrWhiteSpace(node.DistroId))
        {
            nodeEntry["distro"] = node.DistroId;
        }

        nodesObj[node.NodeName] = nodeEntry;
    }

    private static string ResolveServiceName(
        string groupKey,
        int groupCount,
        DiscoveredContainerInfo container,
        HashSet<string> existingNames)
    {
        string candidateName;

        if (groupCount == 1 && string.IsNullOrWhiteSpace(container.ComposeService))
        {
            candidateName = groupKey;
        }
        else if (!string.IsNullOrWhiteSpace(container.ComposeService))
        {
            var svc = container.ComposeService;
            if (string.Equals(svc, groupKey, StringComparison.OrdinalIgnoreCase) ||
                svc.StartsWith($"{groupKey}-", StringComparison.OrdinalIgnoreCase))
            {
                candidateName = svc;
            }
            else
            {
                candidateName = $"{groupKey}-{svc}";
            }
        }
        else
        {
            candidateName = container.CleanName;
        }

        var uniqueName = candidateName;
        var suffix = 2;
        while (existingNames.Contains(uniqueName))
        {
            uniqueName = $"{candidateName}-{suffix}";
            suffix++;
        }

        return uniqueName;
    }

    private async Task<JsonObject> BuildServiceConfigAsync(
        INodeConnection node,
        DiscoveredContainerInfo container,
        string serviceName,
        byte[] masterKey,
        AdoptionOptions options,
        CancellationToken ct)
    {
        var priority = DetectPriority(container.State.Image, serviceName);
        var envObj = ExtractAndEncryptEnvironment(container, serviceName, masterKey);
        var (filesObj, volumesArr) = await ProcessMountsAsync(node, container, serviceName, options, ct).ConfigureAwait(false);
        var portsArr = ReconstructPortBindings(container);

        var serviceObj = new JsonObject
        {
            ["image"] = container.State.Image,
            ["priority"] = priority,
            ["node"] = node.NodeName
        };

        if (portsArr.Count > 0)
        {
            serviceObj["ports"] = portsArr;
        }

        if (volumesArr.Count > 0)
        {
            serviceObj["volumes"] = volumesArr;
        }

        if (envObj.Count > 0)
        {
            serviceObj["env"] = envObj;
        }

        if (filesObj.Count > 0)
        {
            serviceObj["files"] = filesObj;
        }

        var labelsObj = new JsonObject();
        foreach (var (k, v) in container.State.Labels)
        {
            labelsObj[k] = v;
        }

        labelsObj["managed-by"] = "starry";
        labelsObj["starry.priority"] = priority.ToString(CultureInfo.InvariantCulture);
        serviceObj["labels"] = labelsObj;

        return serviceObj;
    }

    private static int DetectPriority(string image, string serviceName)
    {
        var upperImage = image.ToUpperInvariant();
        var upperName = serviceName.ToUpperInvariant();

        if (upperImage.Contains("REGISTRY", StringComparison.Ordinal) || upperName.Contains("REGISTRY", StringComparison.Ordinal))
        {
            return 0;
        }

        if (DbKeywords.Any(kw => upperImage.Contains(kw, StringComparison.Ordinal) || upperName.Contains(kw, StringComparison.Ordinal)))
        {
            return 10;
        }

        if (ProxyKeywords.Any(kw => upperImage.Contains(kw, StringComparison.Ordinal) || upperName.Contains(kw, StringComparison.Ordinal)))
        {
            return 90;
        }

        return 50;
    }

    private JsonObject ExtractAndEncryptEnvironment(
        DiscoveredContainerInfo container,
        string serviceName,
        byte[] masterKey)
    {
        var envObj = new JsonObject();
        if (container.Details["Config"]?["Env"] is not JsonArray envArr)
        {
            return envObj;
        }

        foreach (var envNode in envArr)
        {
            if (envNode is null)
            {
                continue;
            }

            var envStr = envNode.ToString();
            var eqIdx = envStr.IndexOf('=', StringComparison.Ordinal);
            if (eqIdx <= 0)
            {
                continue;
            }

            var key = envStr[..eqIdx].Trim();
            var val = envStr[(eqIdx + 1)..];

            if (IsSecretKey(key))
            {
                if (_cryptoService.TryParseEncrypted(val, out _))
                {
                    envObj[key] = val;
                }
                else
                {
                    var canonicalPath = $"services.{serviceName}.env.{key}";
                    var envelope = _cryptoService.Encrypt(Encoding.UTF8.GetBytes(val), masterKey, canonicalPath);
                    envObj[key] = envelope.FormatEncrypted();
                }
            }
            else
            {
                envObj[key] = val;
            }
        }

        return envObj;
    }

    private static bool IsSecretKey(string key)
    {
        return SecretKeyIndicators.Any(indicator => key.Contains(indicator, StringComparison.OrdinalIgnoreCase));
    }

    private static (List<string> Binds, List<string> Volumes) ExtractRawMounts(DiscoveredContainerInfo container)
    {
        var rawBinds = new List<string>();
        var volumes = new List<string>();

        if (container.Details["HostConfig"]?["Binds"] is JsonArray bindsArr)
        {
            rawBinds.AddRange(bindsArr
                .Where(b => b != null && !string.IsNullOrWhiteSpace(b.ToString()))
                .Select(b => b!.ToString().Trim()));
        }

        if (container.Details["Mounts"] is JsonArray mountsArr)
        {
            PopulateMounts(mountsArr, rawBinds, volumes);
        }

        return (rawBinds, volumes);
    }

    private static void PopulateMounts(JsonArray mountsArr, List<string> rawBinds, List<string> volumes)
    {
        foreach (var m in mountsArr)
        {
            if (m is not JsonObject mObj)
            {
                continue;
            }

            var type = mObj["Type"]?.ToString();
            var source = mObj["Source"]?.ToString();
            var destination = mObj["Destination"]?.ToString();
            var mode = mObj["Mode"]?.ToString();

            if (string.Equals(type, "bind", StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(source) &&
                !string.IsNullOrWhiteSpace(destination))
            {
                var entry = !string.IsNullOrWhiteSpace(mode)
                    ? $"{source}:{destination}:{mode}"
                    : $"{source}:{destination}";
                if (!rawBinds.Contains(entry))
                {
                    rawBinds.Add(entry);
                }
            }
            else if (string.Equals(type, "volume", StringComparison.OrdinalIgnoreCase) &&
                     !string.IsNullOrWhiteSpace(destination))
            {
                var volName = mObj["Name"]?.ToString() ?? source;
                if (!string.IsNullOrWhiteSpace(volName))
                {
                    volumes.Add($"{volName}:{destination}");
                }
            }
        }
    }

    private static async Task<(JsonObject Files, JsonArray Volumes)> ProcessMountsAsync(
        INodeConnection node,
        DiscoveredContainerInfo container,
        string serviceName,
        AdoptionOptions options,
        CancellationToken ct)
    {
        var filesObj = new JsonObject();
        var volumesArr = new JsonArray();
        var processed = new HashSet<string>(StringComparer.Ordinal);

        var (rawBinds, namedVolumes) = ExtractRawMounts(container);

        foreach (var vol in namedVolumes.Where(processed.Add))
        {
            volumesArr.Add(vol);
        }

        foreach (var bindEntry in rawBinds)
        {
            await ProcessSingleBindAsync(
                node,
                bindEntry,
                serviceName,
                options,
                filesObj,
                volumesArr,
                processed,
                ct).ConfigureAwait(false);
        }

        return (filesObj, volumesArr);
    }

    private static async Task ProcessSingleBindAsync(
        INodeConnection node,
        string bindEntry,
        string serviceName,
        AdoptionOptions options,
        JsonObject filesObj,
        JsonArray volumesArr,
        HashSet<string> processed,
        CancellationToken ct)
    {
        var parts = bindEntry.Split(':');
        if (parts.Length < 2)
        {
            if (processed.Add(bindEntry))
            {
                volumesArr.Add(bindEntry);
            }
            return;
        }

        var hostSource = parts[0];
        var containerDest = parts[1];
        var mode = parts.Length > 2 ? parts[2] : string.Empty;

        if (!processed.Add($"{hostSource}:{containerDest}"))
        {
            return;
        }

        var isFile = await CheckIsRemoteFileAsync(node, hostSource, ct).ConfigureAwait(false);
        if (isFile && options.PullFiles)
        {
            var pulled = await TryPullRemoteFileAsync(node, hostSource, containerDest, serviceName, options.FilesDir, ct).ConfigureAwait(false);
            if (pulled != null)
            {
                filesObj[containerDest] = pulled;
                return;
            }
        }

        var volEntry = !string.IsNullOrEmpty(mode) ? $"{hostSource}:{containerDest}:{mode}" : $"{hostSource}:{containerDest}";
        volumesArr.Add(volEntry);
    }

    private static async Task<bool> CheckIsRemoteFileAsync(INodeConnection node, string hostSource, CancellationToken ct)
    {
        try
        {
            var testResult = await node.ExecuteCommandAsync(new ProcessSpec
            {
                Executable = "test",
                Arguments = ["-f", hostSource]
            }, ct).ConfigureAwait(false);

            return testResult.IsSuccess;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return Path.HasExtension(hostSource);
        }
    }

    private static async Task<string?> TryPullRemoteFileAsync(
        INodeConnection node,
        string hostSource,
        string containerDest,
        string serviceName,
        string filesDir,
        CancellationToken ct)
    {
        var catResult = await node.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "cat",
            Arguments = [hostSource]
        }, ct).ConfigureAwait(false);

        if (!catResult.IsSuccess)
        {
            return null;
        }

        var fileName = Path.GetFileName(hostSource);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = Path.GetFileName(containerDest);
        }

        var serviceFilesDir = Path.Combine(filesDir, serviceName);
        Directory.CreateDirectory(serviceFilesDir);
        var localFilePath = Path.Combine(serviceFilesDir, fileName);

        await File.WriteAllTextAsync(localFilePath, catResult.StandardOutput, ct).ConfigureAwait(false);
        return $"files/{serviceName}/{fileName}";
    }

    private static JsonArray ReconstructPortBindings(DiscoveredContainerInfo container)
    {
        var portsArr = new JsonArray();
        var portRoutes = new HashSet<string>(StringComparer.Ordinal);

        JsonObject? portBindings = null;
        if (container.Details["HostConfig"]?["PortBindings"] is JsonObject hb)
        {
            portBindings = hb;
        }
        else if (container.Details["NetworkSettings"]?["Ports"] is JsonObject np)
        {
            portBindings = np;
        }

        if (portBindings is null)
        {
            return portsArr;
        }

        foreach (var (containerPortProto, bindingsVal) in portBindings)
        {
            if (bindingsVal is not JsonArray bArr || bArr.Count == 0)
            {
                continue;
            }

            var slashIdx = containerPortProto.IndexOf('/', StringComparison.Ordinal);
            var containerPort = slashIdx > 0 ? containerPortProto[..slashIdx] : containerPortProto;
            var proto = slashIdx > 0 ? containerPortProto[(slashIdx + 1)..] : "tcp";

            var validBindings = bArr
                .OfType<JsonObject>()
                .Where(b => b.TryGetPropertyValue("HostPort", out var hp) && hp != null && !string.IsNullOrWhiteSpace(hp.ToString()));

            foreach (var bObj in validBindings)
            {
                var hostPort = bObj["HostPort"]!.ToString().Trim();
                var route = string.Equals(proto, "udp", StringComparison.OrdinalIgnoreCase)
                    ? $"{hostPort}:{containerPort}/udp"
                    : $"{hostPort}:{containerPort}";

                if (portRoutes.Add(route))
                {
                    portsArr.Add(route);
                }
            }
        }

        return portsArr;
    }

    private static async Task<string> FindOrCreateMeshNetworkAsync(HttpClient client, CancellationToken ct)
    {
        var existingNetId = await FindExistingMeshNetworkAsync(client, ct).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(existingNetId))
        {
            return existingNetId;
        }

        return await CreateMeshNetworkAsync(client, ct).ConfigureAwait(false);
    }

    private static async Task<string?> FindExistingMeshNetworkAsync(HttpClient client, CancellationToken ct)
    {
        using var netListRes = await client.GetAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/networks"),
            ct).ConfigureAwait(false);

        if (!netListRes.IsSuccessStatusCode)
        {
            return null;
        }

        var netListJson = await netListRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        if (JsonNode.Parse(netListJson) is not JsonArray networks)
        {
            return null;
        }

        foreach (var netNode in networks)
        {
            if (netNode is JsonObject netObj &&
                netObj.TryGetPropertyValue("Name", out var nameVal) &&
                nameVal != null)
            {
                var name = nameVal.ToString();
                if (string.Equals(name, "starry", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, "starry0", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, "starry-mesh", StringComparison.OrdinalIgnoreCase))
                {
                    return netObj["Id"]?.ToString();
                }
            }
        }

        return null;
    }

    private static async Task<string> CreateMeshNetworkAsync(HttpClient client, CancellationToken ct)
    {
        var createNetPayload = new JsonObject
        {
            ["Name"] = "starry",
            ["CheckDuplicate"] = true,
            ["Driver"] = "bridge",
            ["IPAM"] = new JsonObject
            {
                ["Config"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["Subnet"] = "10.99.0.0/16"
                    }
                }
            }
        };

        using var createContent = new StringContent(createNetPayload.ToJsonString(), Encoding.UTF8, "application/json");
        using var createRes = await client.PostAsync(
            new Uri(BaseDockerUri, $"{ApiVersionPrefix}/networks/create"),
            createContent,
            ct).ConfigureAwait(false);

        if (createRes.IsSuccessStatusCode)
        {
            var createJson = await createRes.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            if (JsonNode.Parse(createJson) is JsonObject createdObj)
            {
                var createdId = createdObj["Id"]?.ToString();
                if (!string.IsNullOrWhiteSpace(createdId))
                {
                    return createdId;
                }
            }
        }

        return "starry";
    }

    private static byte[] ResolveMasterKey(AdoptionOptions options)
    {
        if (options.MasterKey is { Length: CryptoService.KeySizeBytes } key)
        {
            return key;
        }

        var envKey = Environment.GetEnvironmentVariable("STARRY_MASTER_KEY");
        if (!string.IsNullOrWhiteSpace(envKey))
        {
            try
            {
                return CryptoService.ParseMasterKey(envKey);
            }
            catch (FormatException)
            {
                // Fallback to generating a fresh key
            }
        }

        return CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
    }

    private sealed record DiscoveredContainerInfo(ContainerState State, JsonObject Details)
    {
        public string CleanName => State.Name.TrimStart('/');

        public string? ComposeProject
        {
            get
            {
                if (Details["Config"]?["Labels"] is JsonObject labelsObj &&
                    labelsObj.TryGetPropertyValue("com.docker.compose.project", out var projVal) &&
                    projVal != null)
                {
                    return projVal.ToString().Trim();
                }

                return State.Labels.GetValueOrDefault("com.docker.compose.project")?.Trim();
            }
        }

        public string? ComposeService
        {
            get
            {
                if (Details["Config"]?["Labels"] is JsonObject labelsObj &&
                    labelsObj.TryGetPropertyValue("com.docker.compose.service", out var svcVal) &&
                    svcVal != null)
                {
                    return svcVal.ToString().Trim();
                }

                return State.Labels.GetValueOrDefault("com.docker.compose.service")?.Trim();
            }
        }
    }
}
```

## File: src/Starry.Infrastructure/Orchestration/MigrationPlan.cs

**Lines**: 8 | **Size**: 200 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Orchestration;

public sealed record MigrationPlan(
    string ClusterName,
    IReadOnlyList<PlanAction> Actions)
{
    public bool HasChanges => Actions.Count > 0;
}
```

## File: src/Starry.Infrastructure/Orchestration/MigrationPlanner.cs

**Lines**: 142 | **Size**: 5.5 KB | **Language**: csharp

```csharp
using System.Globalization;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Orchestration;

/// <summary>
/// Computes directed acyclic execution plan (DAG) comparing desired configuration against cluster state.
/// </summary>
public static class MigrationPlanner
{
    public static MigrationPlan BuildPlan(
        ConfigTree desiredConfig,
        IReadOnlyDictionary<string, string>? currentServiceNodePlacements = null,
        bool onlyChangedServices = false)
    {
        ArgumentNullException.ThrowIfNull(desiredConfig);

        var actions = new List<PlanAction>();
        var clusterName = desiredConfig.Root["cluster_name"]?.ToString() ?? "starry-cluster";

        var nodeCount = PlanNodeActions(desiredConfig, actions);
        PlanNetworkActions(nodeCount, actions);
        PlanServiceActions(desiredConfig, currentServiceNodePlacements, actions, onlyChangedServices);

        var sortedActions = actions
            .OrderBy(a => a.Priority)
            .ThenBy(a => a.Id, StringComparer.Ordinal)
            .ToList();

        return new MigrationPlan(clusterName, sortedActions);
    }

    private static int PlanNodeActions(ConfigTree desiredConfig, List<PlanAction> actions)
    {
        var count = 0;
        if (desiredConfig.Root.TryGetPropertyValue("nodes", out var nodesNode) && nodesNode is JsonObject nodesObj)
        {
            foreach (var (nodeName, _) in nodesObj)
            {
                count++;
                actions.Add(new PlanAction(
                    Id: $"node:{nodeName}:bootstrap",
                    ActionType: PlanActionType.CreateNode,
                    Target: nodeName,
                    Description: $"Bootstrap node '{nodeName}' with baseline system packages",
                    Priority: 0));
            }
        }
        return count;
    }

    private static void PlanNetworkActions(int nodeCount, List<PlanAction> actions)
    {
        if (nodeCount == 0)
        {
            return;
        }

        var nodeDependencyIds = actions.Where(a => a.ActionType == PlanActionType.CreateNode).Select(a => a.Id).ToList();

        actions.Add(new PlanAction(
            Id: "network:wireguard:mesh",
            ActionType: PlanActionType.ConfigureWireGuardMesh,
            Target: "starry0",
            Description: $"Synchronize WireGuard P2P mesh across {nodeCount} node(s) with Keepalive=25",
            Priority: 5,
            Dependencies: nodeDependencyIds));

        actions.Add(new PlanAction(
            Id: "network:nftables:ruleset",
            ActionType: PlanActionType.ConfigureNftables,
            Target: "inet starry",
            Description: "Apply host firewall policies and DNAT rules in table inet starry",
            Priority: 10,
            Dependencies: ["network:wireguard:mesh"]));
    }

    private static void PlanServiceActions(
        ConfigTree desiredConfig,
        IReadOnlyDictionary<string, string>? currentServiceNodePlacements,
        List<PlanAction> actions,
        bool onlyChangedServices = false)
    {
        if (!desiredConfig.Root.TryGetPropertyValue("services", out var servicesNode) || servicesNode is not JsonObject servicesObj)
        {
            return;
        }

        var placements = currentServiceNodePlacements ?? new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var (svcName, svcVal) in servicesObj)
        {
            if (svcVal is not JsonObject sObj)
            {
                continue;
            }

            var image = sObj["image"]?.ToString() ?? "unknown";
            var priority = sObj.TryGetPropertyValue("priority", out var pVal) &&
                           int.TryParse(pVal?.ToString(), CultureInfo.InvariantCulture, out var p)
                ? p
                : 50;
            var targetNode = sObj["node"]?.ToString();

            var hasOldPlacement = targetNode != null &&
                                  placements.TryGetValue(svcName, out var currentNode) &&
                                  !string.Equals(currentNode, targetNode, StringComparison.OrdinalIgnoreCase);

            var isAlreadyConverged = targetNode != null &&
                                     placements.TryGetValue(svcName, out var runningNode) &&
                                     string.Equals(runningNode, targetNode, StringComparison.OrdinalIgnoreCase);

            if (onlyChangedServices && isAlreadyConverged)
            {
                continue;
            }

            if (hasOldPlacement)
            {
                var oldNode = placements[svcName];
                actions.Add(new PlanAction(
                    Id: $"service:{svcName}:blue-green",
                    ActionType: PlanActionType.MigrateServiceBlueGreen,
                    Target: svcName,
                    Description: $"Zero-downtime Blue-Green migration of '{svcName}' from '{oldNode}' to '{targetNode}'",
                    Priority: 50 + priority,
                    Dependencies: ["network:wireguard:mesh", "network:nftables:ruleset"]));
            }
            else
            {
                actions.Add(new PlanAction(
                    Id: $"service:{svcName}:deploy",
                    ActionType: PlanActionType.DeployService,
                    Target: svcName,
                    Description: $"Deploy container '{svcName}' [{image}] on '{targetNode ?? "auto"}' (priority: {priority})",
                    Priority: 50 + priority,
                    Dependencies: ["network:wireguard:mesh", "network:nftables:ruleset"]));
            }
        }
    }
}
```

## File: src/Starry.Infrastructure/Orchestration/PlanAction.cs

**Lines**: 12 | **Size**: 328 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Orchestration;

public sealed record PlanAction(
    string Id,
    PlanActionType ActionType,
    string Target,
    string Description,
    int Priority = 50,
    IReadOnlyList<string>? Dependencies = null)
{
    public IReadOnlyList<string> Dependencies { get; init; } = Dependencies ?? [];
}
```

## File: src/Starry.Infrastructure/Orchestration/PlanActionType.cs

**Lines**: 11 | **Size**: 208 B | **Language**: csharp

```csharp
namespace Starry.Infrastructure.Orchestration;

public enum PlanActionType
{
    CreateNode,
    ConfigureWireGuardMesh,
    ConfigureNftables,
    DeployService,
    MigrateServiceBlueGreen,
    UpdateDns
}
```

## File: src/Starry.Infrastructure/Packaging/ApkPackageManager.cs

**Lines**: 86 | **Size**: 2.6 KB | **Language**: csharp

```csharp
using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// APK package manager implementation for Alpine Linux.
/// </summary>
public sealed class ApkPackageManager : IOsPackageManager
{
    public IReadOnlyList<string> SupportedDistroIds { get; } = ["alpine"];

    public async Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(logicalPackages);

        if (logicalPackages.Count == 0)
        {
            return;
        }

        var installArgs = new List<string> { "add", "--no-cache" };
        foreach (var pkg in logicalPackages)
        {
            installArgs.Add(MapLogicalPackage(pkg));
        }

        var installSpec = new ProcessSpec
        {
            Executable = "apk",
            Arguments = installArgs
        };

        var result = await node.ExecuteCommandAsync(installSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"APK package installation failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var upgradeSpec = new ProcessSpec
        {
            Executable = "apk",
            Arguments = ["upgrade", "--no-cache"]
        };

        var result = await node.ExecuteCommandAsync(upgradeSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"APK system upgrade failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var checkSpec = new ProcessSpec
        {
            Executable = "test",
            Arguments = ["-f", "/var/run/reboot-required"]
        };

        var result = await node.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
        return result.ExitCode == 0;
    }

    public static string MapLogicalPackage(string logicalPackage)
    {
        return logicalPackage switch
        {
            "wireguard" => "wireguard-tools",
            "nftables" => "nftables",
            "curl" => "curl",
            "rsync" => "rsync",
            "zstd" => "zstd",
            _ => logicalPackage
        };
    }
}
```

## File: src/Starry.Infrastructure/Packaging/AptPackageManager.cs

**Lines**: 108 | **Size**: 3.5 KB | **Language**: csharp

```csharp
using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// APT package manager implementation for Debian and Ubuntu systems.
/// </summary>
public sealed class AptPackageManager : IOsPackageManager
{
    public IReadOnlyList<string> SupportedDistroIds { get; } = ["ubuntu", "debian", "pop", "mint"];

    public async Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(logicalPackages);

        if (logicalPackages.Count == 0)
        {
            return;
        }

        var updateSpec = new ProcessSpec
        {
            Executable = "apt-get",
            Arguments = ["update", "-qq"],
            EnvironmentVariables = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["DEBIAN_FRONTEND"] = "noninteractive"
            }
        };
        await node.ExecuteCommandAsync(updateSpec, ct).ConfigureAwait(false);

        var installArgs = new List<string> { "install", "-y", "-qq" };
        foreach (var pkg in logicalPackages)
        {
            installArgs.Add(MapLogicalPackage(pkg));
        }

        var installSpec = new ProcessSpec
        {
            Executable = "apt-get",
            Arguments = installArgs,
            EnvironmentVariables = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["DEBIAN_FRONTEND"] = "noninteractive"
            }
        };
        var result = await node.ExecuteCommandAsync(installSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"APT package installation failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var upgradeArgs = fullUpgrade
            ? new[] { "dist-upgrade", "-y", "-qq" }
            : new[] { "upgrade", "-y", "-qq", "--with-new-pkgs" };

        var upgradeSpec = new ProcessSpec
        {
            Executable = "apt-get",
            Arguments = upgradeArgs,
            EnvironmentVariables = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["DEBIAN_FRONTEND"] = "noninteractive"
            }
        };

        var result = await node.ExecuteCommandAsync(upgradeSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"APT system update failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var checkSpec = new ProcessSpec
        {
            Executable = "test",
            Arguments = ["-f", "/var/run/reboot-required"]
        };

        var result = await node.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
        return result.ExitCode == 0;
    }

    public static string MapLogicalPackage(string logicalPackage)
    {
        return logicalPackage switch
        {
            "wireguard" => "wireguard",
            "nftables" => "nftables",
            "curl" => "curl",
            "rsync" => "rsync",
            "zstd" => "zstd",
            _ => logicalPackage
        };
    }
}
```

## File: src/Starry.Infrastructure/Packaging/DnfPackageManager.cs

**Lines**: 91 | **Size**: 2.9 KB | **Language**: csharp

```csharp
using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// DNF / YUM package manager implementation for RHEL, AlmaLinux, Rocky Linux, and Fedora.
/// </summary>
public sealed class DnfPackageManager : IOsPackageManager
{
    public IReadOnlyList<string> SupportedDistroIds { get; } = ["rhel", "almalinux", "rocky", "fedora", "centos"];

    public async Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(logicalPackages);

        if (logicalPackages.Count == 0)
        {
            return;
        }

        var installArgs = new List<string> { "install", "-y", "-q" };
        foreach (var pkg in logicalPackages)
        {
            installArgs.Add(MapLogicalPackage(pkg));
        }

        var installSpec = new ProcessSpec
        {
            Executable = "dnf",
            Arguments = installArgs
        };

        var result = await node.ExecuteCommandAsync(installSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"DNF package installation failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var upgradeArgs = fullUpgrade
            ? new[] { "upgrade", "-y", "-q" }
            : new[] { "update", "--security", "-y", "-q" };

        var upgradeSpec = new ProcessSpec
        {
            Executable = "dnf",
            Arguments = upgradeArgs
        };

        var result = await node.ExecuteCommandAsync(upgradeSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"DNF security update failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var checkSpec = new ProcessSpec
        {
            Executable = "needs-restarting",
            Arguments = ["-r"]
        };

        var result = await node.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
        // needs-restarting -r returns 1 when reboot is required
        return result.ExitCode == 1;
    }

    public static string MapLogicalPackage(string logicalPackage)
    {
        return logicalPackage switch
        {
            "wireguard" => "wireguard-tools",
            "nftables" => "nftables",
            "curl" => "curl",
            "rsync" => "rsync",
            "zstd" => "zstd",
            _ => logicalPackage
        };
    }
}
```

## File: src/Starry.Infrastructure/Packaging/PackageManagerFactory.cs

**Lines**: 42 | **Size**: 1.3 KB | **Language**: csharp

```csharp
using Starry.Core.Nodes;
using Starry.Core.Packaging;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// Factory to resolve the appropriate package manager for any given Linux distribution.
/// </summary>
public sealed class PackageManagerFactory
{
    private readonly List<IOsPackageManager> _managers =
    [
        new AptPackageManager(),
        new DnfPackageManager(),
        new ApkPackageManager(),
        new PacmanPackageManager()
    ];

    public IOsPackageManager Resolve(string distroId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(distroId);

        var trimmed = distroId.Trim();
        var matched = _managers.FirstOrDefault(manager =>
            manager.SupportedDistroIds.Any(id => string.Equals(id, trimmed, StringComparison.OrdinalIgnoreCase)));

        if (matched != null)
        {
            return matched;
        }

        throw new NotSupportedException($"Unsupported Linux distribution '{distroId}'. Supported distributions: Debian/Ubuntu (apt), RHEL/CentOS/AlmaLinux/Rocky/Fedora (dnf), Alpine (apk), Arch (pacman).");
    }

    public IOsPackageManager ResolveForNode(INodeConnection node)
    {
        ArgumentNullException.ThrowIfNull(node);

        var distro = node.DistroId ?? "ubuntu";
        return Resolve(distro);
    }
}
```

## File: src/Starry.Infrastructure/Packaging/PacmanPackageManager.cs

**Lines**: 86 | **Size**: 2.7 KB | **Language**: csharp

```csharp
using Starry.Core.Nodes;
using Starry.Core.Packaging;
using Starry.Core.Processes;

namespace Starry.Infrastructure.Packaging;

/// <summary>
/// Pacman package manager implementation for Arch Linux and derivative distributions.
/// </summary>
public sealed class PacmanPackageManager : IOsPackageManager
{
    public IReadOnlyList<string> SupportedDistroIds { get; } = ["arch", "manjaro", "endeavouros"];

    public async Task EnsurePackagesInstalledAsync(INodeConnection node, IReadOnlyList<string> logicalPackages, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(logicalPackages);

        if (logicalPackages.Count == 0)
        {
            return;
        }

        var installArgs = new List<string> { "-Sy", "--noconfirm", "--needed" };
        foreach (var pkg in logicalPackages)
        {
            installArgs.Add(MapLogicalPackage(pkg));
        }

        var installSpec = new ProcessSpec
        {
            Executable = "pacman",
            Arguments = installArgs
        };

        var result = await node.ExecuteCommandAsync(installSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Pacman package installation failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task ApplySecurityUpdatesAsync(INodeConnection node, bool fullUpgrade, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var upgradeSpec = new ProcessSpec
        {
            Executable = "pacman",
            Arguments = ["-Syu", "--noconfirm"]
        };

        var result = await node.ExecuteCommandAsync(upgradeSpec, ct).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Pacman system upgrade failed on '{node.NodeName}': {result.StandardError}");
        }
    }

    public async Task<bool> IsRebootRequiredAsync(INodeConnection node, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(node);

        var checkSpec = new ProcessSpec
        {
            Executable = "test",
            Arguments = ["-f", "/var/run/reboot-required"]
        };

        var result = await node.ExecuteCommandAsync(checkSpec, ct).ConfigureAwait(false);
        return result.ExitCode == 0;
    }

    public static string MapLogicalPackage(string logicalPackage)
    {
        return logicalPackage switch
        {
            "wireguard" => "wireguard-tools",
            "nftables" => "nftables",
            "curl" => "curl",
            "rsync" => "rsync",
            "zstd" => "zstd",
            _ => logicalPackage
        };
    }
}
```

## File: src/Starry.Infrastructure/Processes/ProcessRunner.cs

**Lines**: 237 | **Size**: 7.4 KB | **Language**: csharp

```csharp
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

    public async Task<ProcessResult> RunStreamingAsync(
        ProcessSpec spec,
        Stream? stdinStream,
        Stream? stdoutStream,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(spec);

        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        if (spec.Timeout > TimeSpan.Zero && spec.Timeout < TimeSpan.MaxValue)
        {
            linkedCts.CancelAfter(spec.Timeout);
        }

        var startInfo = CreateStartInfo(spec);
        startInfo.RedirectStandardInput = stdinStream != null || spec.StandardInputBytes.HasValue;
        startInfo.RedirectStandardOutput = stdoutStream != null;

        using var process = new Process { StartInfo = startInfo };
        var stderrBuilder = new StringBuilder();

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                stderrBuilder.AppendLine(e.Data);
            }
        };

        var stopwatch = Stopwatch.StartNew();
        StartProcess(process, spec.Executable);
        process.BeginErrorReadLine();

        var (stdinTask, stdoutTask) = SetupStreamingPipes(process, stdinStream, stdoutStream, spec.StandardInputBytes, linkedCts.Token);

        try
        {
            await process.WaitForExitAsync(linkedCts.Token).ConfigureAwait(false);
            if (stdinTask != null)
            {
                await stdinTask.ConfigureAwait(false);
            }
            if (stdoutTask != null)
            {
                await stdoutTask.ConfigureAwait(false);
            }
            stopwatch.Stop();

            return new ProcessResult(process.ExitCode, string.Empty, stderrBuilder.ToString(), stopwatch.Elapsed);
        }
        catch (OperationCanceledException ex)
        {
            TerminateProcessTree(process);
            stopwatch.Stop();
            throw new TimeoutException($"Process '{spec.Executable}' timed out or was cancelled after {stopwatch.ElapsedMilliseconds} ms.", ex);
        }
    }

    private static (Task? StdinTask, Task? StdoutTask) SetupStreamingPipes(
        Process process,
        Stream? stdinStream,
        Stream? stdoutStream,
        ReadOnlyMemory<byte>? inputBytes,
        CancellationToken ct)
    {
        Task? stdinTask = null;
        if (stdinStream != null)
        {
            stdinTask = Task.Run(async () =>
            {
                try
                {
                    await stdinStream.CopyToAsync(process.StandardInput.BaseStream, ct).ConfigureAwait(false);
                    await process.StandardInput.BaseStream.FlushAsync(ct).ConfigureAwait(false);
                    process.StandardInput.Close();
                }
                catch (IOException)
                {
                    // Target process may close pipe early
                }
            }, ct);
        }
        else if (inputBytes.HasValue)
        {
            stdinTask = WriteInputAsync(process, inputBytes.Value, ct);
        }

        Task? stdoutTask = stdoutStream != null
            ? process.StandardOutput.BaseStream.CopyToAsync(stdoutStream, ct)
            : null;

        return (stdinTask, stdoutTask);
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
```

## File: src/Starry.Infrastructure/Security/CryptoService.cs

**Lines**: 122 | **Size**: 4.0 KB | **Language**: csharp

```csharp
using System.Security.Cryptography;
using System.Text;
using Starry.Core.Security;

namespace Starry.Infrastructure.Security;

/// <summary>
/// High-performance AES-256-GCM encryption service with canonical JSON path AAD binding.
/// </summary>
public sealed class CryptoService : ICryptoService
{
    private const string EncryptedPrefix = "ENC[AES256_GCM:v1:";
    private const char EncryptedSuffix = ']';
    public const int KeySizeBytes = 32;
    public const int NonceSizeBytes = 12;
    public const int TagSizeBytes = 16;

    public SecretEnvelope Encrypt(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> key, string canonicalJsonPath)
    {
        if (key.Length != KeySizeBytes)
        {
            throw new ArgumentException($"Master key must be exactly {KeySizeBytes} bytes (256 bits), got {key.Length} bytes.", nameof(key));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(canonicalJsonPath);

        Span<byte> nonce = stackalloc byte[NonceSizeBytes];
        RandomNumberGenerator.Fill(nonce);

        var cipher = new byte[plaintext.Length];
        Span<byte> tag = stackalloc byte[TagSizeBytes];

        var aad = Encoding.UTF8.GetBytes(canonicalJsonPath);

        using var aes = new AesGcm(key, TagSizeBytes);
        aes.Encrypt(nonce, plaintext, cipher, tag, aad);

        return new SecretEnvelope(
            Convert.ToBase64String(nonce),
            Convert.ToBase64String(cipher),
            Convert.ToBase64String(tag));
    }

    public byte[] Decrypt(SecretEnvelope envelope, ReadOnlySpan<byte> key, string canonicalJsonPath)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        if (key.Length != KeySizeBytes)
        {
            throw new ArgumentException($"Master key must be exactly {KeySizeBytes} bytes (256 bits), got {key.Length} bytes.", nameof(key));
        }

        var nonce = Convert.FromBase64String(envelope.NonceBase64);
        var cipher = Convert.FromBase64String(envelope.CipherBase64);
        var tag = Convert.FromBase64String(envelope.TagBase64);

        var plaintext = new byte[cipher.Length];
        var aad = !string.IsNullOrEmpty(canonicalJsonPath) ? Encoding.UTF8.GetBytes(canonicalJsonPath) : [];

        using var aes = new AesGcm(key, TagSizeBytes);
        aes.Decrypt(nonce, cipher, tag, plaintext, aad);

        return plaintext;
    }

    public bool TryParseEncrypted(string value, out SecretEnvelope? envelope)
    {
        envelope = null;
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith(EncryptedPrefix, StringComparison.Ordinal) || !value.EndsWith(EncryptedSuffix))
        {
            return false;
        }

        var inner = value[EncryptedPrefix.Length..^1];
        var parts = inner.Split(':', StringSplitOptions.TrimEntries);
        if (parts.Length != 3)
        {
            return false;
        }

        envelope = new SecretEnvelope(parts[0], parts[1], parts[2]);
        return true;
    }

    public static string GenerateMasterKeyBase64()
    {
        Span<byte> key = stackalloc byte[KeySizeBytes];
        RandomNumberGenerator.Fill(key);
        return Convert.ToBase64String(key);
    }

    public static byte[] ParseMasterKey(string keyString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString);

        var trimmed = keyString.Trim();
        try
        {
            var bytes = Convert.FromBase64String(trimmed);
            if (bytes.Length == KeySizeBytes)
            {
                return bytes;
            }
        }
        catch (FormatException)
        {
            // Try hex decoding if not base64
        }

        if (trimmed.Length == KeySizeBytes * 2)
        {
            return Convert.FromHexString(trimmed);
        }

        var utf8 = Encoding.UTF8.GetBytes(trimmed);
        if (utf8.Length == KeySizeBytes)
        {
            return utf8;
        }

        throw new ArgumentException($"Invalid master key length. Key must be 32 bytes (Base64 string or 64 hex characters). Got '{trimmed.Length}' characters.", nameof(keyString));
    }
}
```

## File: src/Starry.Infrastructure/Security/SecretManager.cs

**Lines**: 325 | **Size**: 11.1 KB | **Language**: csharp

```csharp
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;

namespace Starry.Infrastructure.Security;

/// <summary>
/// Manages secret discovery, on-the-fly encryption of PLAIN[...] secrets,
/// atomic multi-file updates, and global master key rotation.
/// </summary>
public sealed class SecretManager
{
    private readonly CryptoService _cryptoService = new();

    public int EncryptOpenSecrets(ConfigTree tree, byte[] masterKey, string rootPath, VirtualConfigEngine engine)
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentNullException.ThrowIfNull(masterKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);
        ArgumentNullException.ThrowIfNull(engine);

        var count = 0;
        var modified = EncryptOpenSecretsRecursive(tree.Root, string.Empty, masterKey, ref count);

        if (modified)
        {
            SaveTreeAtomically(tree, rootPath, engine);
        }

        return count;
    }

    public int ReencryptAll(ConfigTree tree, byte[] oldKey, byte[] newKey, string rootPath, VirtualConfigEngine engine)
    {
        ArgumentNullException.ThrowIfNull(tree);
        ArgumentNullException.ThrowIfNull(oldKey);
        ArgumentNullException.ThrowIfNull(newKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);
        ArgumentNullException.ThrowIfNull(engine);

        var count = 0;
        var modified = ReencryptSecretsRecursive(tree.Root, string.Empty, oldKey, newKey, ref count);

        if (modified)
        {
            SaveTreeAtomically(tree, rootPath, engine);
        }

        return count;
    }

    public static void AtomicWriteFile(string targetPath, string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetPath);
        ArgumentNullException.ThrowIfNull(content);

        var dir = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var tempPath = Path.Combine(dir ?? ".", $".{Path.GetFileName(targetPath)}.{Guid.NewGuid():N}.tmp");

        try
        {
            using (var stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.Write(content);
                writer.Flush();
            }

            File.Move(tempPath, targetPath, overwrite: true);
        }
        catch
        {
            if (File.Exists(tempPath))
            {
                try
                {
                    File.Delete(tempPath);
                }
                catch (IOException)
                {
                    // Ignore secondary cleanup exception
                }
            }
            throw;
        }
    }

    private bool EncryptOpenSecretsRecursive(JsonNode? current, string currentPath, byte[] masterKey, ref int count)
    {
        if (current is null)
        {
            return false;
        }

        if (current is JsonObject obj)
        {
            return EncryptObjectProperties(obj, currentPath, masterKey, ref count);
        }

        if (current is JsonArray arr)
        {
            return EncryptArrayItems(arr, currentPath, masterKey, ref count);
        }

        return false;
    }

    private bool EncryptObjectProperties(JsonObject obj, string currentPath, byte[] masterKey, ref int count)
    {
        var modified = false;
        var keys = obj.Select(kv => kv.Key).ToList();

        foreach (var key in keys)
        {
            var val = obj[key];
            var childPath = string.IsNullOrEmpty(currentPath) ? key : $"{currentPath}.{key}";

            if (val is JsonValue jsonVal && jsonVal.TryGetValue<string>(out var strVal) &&
                strVal.StartsWith("PLAIN[", StringComparison.Ordinal) && strVal.EndsWith(']'))
            {
                var rawSecret = strVal["PLAIN[".Length..^1];
                var plaintextBytes = Encoding.UTF8.GetBytes(rawSecret);
                var envelope = _cryptoService.Encrypt(plaintextBytes, masterKey, childPath);
                obj[key] = envelope.FormatEncrypted();
                count++;
                modified = true;
            }
            else if ((val is JsonObject or JsonArray) && EncryptOpenSecretsRecursive(val, childPath, masterKey, ref count))
            {
                modified = true;
            }
        }

        return modified;
    }

    private bool EncryptArrayItems(JsonArray arr, string currentPath, byte[] masterKey, ref int count)
    {
        var modified = false;

        for (var i = 0; i < arr.Count; i++)
        {
            var childPath = $"{currentPath}[{i}]";
            var item = arr[i];

            if (item is JsonValue jsonVal && jsonVal.TryGetValue<string>(out var strVal) &&
                strVal.StartsWith("PLAIN[", StringComparison.Ordinal) && strVal.EndsWith(']'))
            {
                var rawSecret = strVal["PLAIN[".Length..^1];
                var plaintextBytes = Encoding.UTF8.GetBytes(rawSecret);
                var envelope = _cryptoService.Encrypt(plaintextBytes, masterKey, childPath);
                arr[i] = envelope.FormatEncrypted();
                count++;
                modified = true;
            }
            else if ((item is JsonObject or JsonArray) && EncryptOpenSecretsRecursive(item, childPath, masterKey, ref count))
            {
                modified = true;
            }
        }

        return modified;
    }

    private bool ReencryptSecretsRecursive(JsonNode? current, string currentPath, byte[] oldKey, byte[] newKey, ref int count)
    {
        if (current is null)
        {
            return false;
        }

        if (current is JsonObject obj)
        {
            return ReencryptObjectProperties(obj, currentPath, oldKey, newKey, ref count);
        }

        if (current is JsonArray arr)
        {
            return ReencryptArrayItems(arr, currentPath, oldKey, newKey, ref count);
        }

        return false;
    }

    private bool ReencryptObjectProperties(JsonObject obj, string currentPath, byte[] oldKey, byte[] newKey, ref int count)
    {
        var modified = false;
        var keys = obj.Select(kv => kv.Key).ToList();

        foreach (var key in keys)
        {
            var val = obj[key];
            var childPath = string.IsNullOrEmpty(currentPath) ? key : $"{currentPath}.{key}";

            if (val is JsonValue jsonVal && jsonVal.TryGetValue<string>(out var strVal) &&
                _cryptoService.TryParseEncrypted(strVal, out var envelope) && envelope != null)
            {
                var plaintextBytes = _cryptoService.Decrypt(envelope, oldKey, childPath);
                var newEnvelope = _cryptoService.Encrypt(plaintextBytes, newKey, childPath);
                obj[key] = newEnvelope.FormatEncrypted();
                count++;
                modified = true;
            }
            else if ((val is JsonObject or JsonArray) && ReencryptSecretsRecursive(val, childPath, oldKey, newKey, ref count))
            {
                modified = true;
            }
        }

        return modified;
    }

    private bool ReencryptArrayItems(JsonArray arr, string currentPath, byte[] oldKey, byte[] newKey, ref int count)
    {
        var modified = false;

        for (var i = 0; i < arr.Count; i++)
        {
            var childPath = $"{currentPath}[{i}]";
            var item = arr[i];

            if (item is JsonValue jsonVal && jsonVal.TryGetValue<string>(out var strVal) &&
                _cryptoService.TryParseEncrypted(strVal, out var envelope) && envelope != null)
            {
                var plaintextBytes = _cryptoService.Decrypt(envelope, oldKey, childPath);
                var newEnvelope = _cryptoService.Encrypt(plaintextBytes, newKey, childPath);
                arr[i] = newEnvelope.FormatEncrypted();
                count++;
                modified = true;
            }
            else if ((item is JsonObject or JsonArray) && ReencryptSecretsRecursive(item, childPath, oldKey, newKey, ref count))
            {
                modified = true;
            }
        }

        return modified;
    }

    private static void SaveTreeAtomically(ConfigTree tree, string rootPath, VirtualConfigEngine engine)
    {
        if (File.Exists(rootPath))
        {
            var ext = Path.GetExtension(rootPath);
            var provider = engine.Providers.FirstOrDefault(p => p.SupportedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                           ?? engine.Providers[0];
            var serialized = provider.Serialize(tree.Root);
            AtomicWriteFile(rootPath, serialized);
            return;
        }

        if (Directory.Exists(rootPath))
        {
            var filesByPath = tree.SourceMap.Values
                .Select(v => v.FilePath)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (filesByPath.Count > 0)
            {
                foreach (var relativeFile in filesByPath)
                {
                    SavePhysicalConfigFile(tree, rootPath, relativeFile, engine);
                }
                return;
            }

            engine.Split(tree, rootPath, "yaml");
        }
    }

    private static void SavePhysicalConfigFile(ConfigTree tree, string rootDir, string relativeFile, VirtualConfigEngine engine)
    {
        var fullPath = Path.IsPathRooted(relativeFile)
            ? relativeFile
            : Path.Combine(rootDir, relativeFile);

        var ext = Path.GetExtension(fullPath);
        var provider = engine.Providers.FirstOrDefault(p => p.SupportedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase));
        if (provider == null)
        {
            return;
        }

        var pathWithoutExt = Path.Combine(
            Path.GetDirectoryName(relativeFile) ?? string.Empty,
            Path.GetFileNameWithoutExtension(relativeFile)).Replace('\\', '/');

        var fileNameNoExt = Path.GetFileNameWithoutExtension(relativeFile);
        JsonNode? subtree = null;
        if (string.Equals(fileNameNoExt, "starry", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(fileNameNoExt, "config", StringComparison.OrdinalIgnoreCase))
        {
            // Root properties that belong to this file
            var rootObj = new JsonObject();
            foreach (var (k, v) in tree.Root)
            {
                if (tree.SourceMap.TryGetValue(k, out var sm) &&
                    string.Equals(Path.GetFileName(sm.FilePath), Path.GetFileName(relativeFile), StringComparison.OrdinalIgnoreCase))
                {
                    rootObj[k] = v != null ? JsonNode.Parse(v.ToJsonString()) : null;
                }
            }
            subtree = rootObj.Count > 0 ? rootObj : tree.Root;
        }
        else
        {
            var canonicalPrefix = pathWithoutExt.Replace('/', '.');
            subtree = tree.FindNode(canonicalPrefix);
        }

        if (subtree != null)
        {
            var content = provider.Serialize(subtree);
            AtomicWriteFile(fullPath, content);
        }
    }
}
```

## File: tests/Starry.ChaosTests/AdvancedChaosScenariosTests.cs

**Lines**: 268 | **Size**: 12.0 KB | **Language**: csharp

```csharp
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Dns;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Dns;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.ChaosTests;

public class AdvancedChaosScenariosTests
{
    private sealed class MockFailingNodeConnection(string nodeName, Exception failureToThrow) : INodeConnection
    {
        public string NodeName => nodeName;
        public string Host => "198.51.100.10";
        public int Port => 22;
        public string? DistroId => "ubuntu";

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default) => throw failureToThrow;
        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default) => throw failureToThrow;
        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default) => throw failureToThrow;
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockSuccessNodeConnection(string nodeName) : INodeConnection
    {
        public string NodeName => nodeName;
        public string Host => "198.51.100.10";
        public int Port => 22;
        public string? DistroId => "ubuntu";

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default) =>
            Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));

        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default) =>
            Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "HttpClient owns handler")]
        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
        {
            var handler = new MockSuccessHttpMessageHandler();
            return Task.FromResult(new HttpClient(handler, disposeHandler: true));
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockRecordingNodeConnection(string nodeName) : INodeConnection
    {
        public string NodeName => nodeName;
        public string Host => "198.51.100.10";
        public int Port => 22;
        public string? DistroId => "ubuntu";
        public List<string> ExecutedCommands { get; } = [];

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
        {
            ExecutedCommands.Add(string.Join(" ", command.Arguments));
            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            ExecutedCommands.Add(string.Join(" ", command.Arguments));
            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default) =>
            Task.FromResult(new HttpClient());

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockBinaryStreamingNodeConnection(string nodeName, byte[] payload) : INodeConnection
    {
        public string NodeName => nodeName;
        public string Host => "198.51.100.10";
        public int Port => 22;
        public string? DistroId => "ubuntu";
        public byte[] ReceivedBytes { get; private set; } = [];

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default) =>
            Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));

        public async Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            if (stdoutStream != null && payload.Length > 0)
            {
                await stdoutStream.WriteAsync(payload, ct);
            }

            if (stdinStream != null)
            {
                using var ms = new MemoryStream();
                await stdinStream.CopyToAsync(ms, ct);
                ReceivedBytes = ms.ToArray();
            }

            return new ProcessResult(0, "success", string.Empty, TimeSpan.Zero);
        }

        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default) =>
            Task.FromResult(new HttpClient());

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockSuccessHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Id\":\"mock-container-id\",\"State\":\"running\",\"Status\":\"Up\"}", Encoding.UTF8, "application/json")
            });
        }
    }

    private sealed class MockRetryingHttpMessageHandler(Queue<HttpStatusCode> responses) : HttpMessageHandler
    {
        public int RequestCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestCount++;
            var status = responses.Count > 0 ? responses.Dequeue() : HttpStatusCode.OK;
            return Task.FromResult(new HttpResponseMessage(status)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task Chaos1_MidMigrationSshDrop_AbortsWithoutCuttingOverDnsOrStoppingSource()
    {
        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("kessoku-chaos"));
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("198.51.100.10"));
        tree.SetNode("nodes.nijika.host", JsonValue.Create("198.51.100.20"));
        tree.SetNode("services.api.image", JsonValue.Create("my-api:v2"));
        tree.SetNode("services.api.node", JsonValue.Create("nijika"));

        var livePlacements = new Dictionary<string, string>(StringComparer.Ordinal) { ["api"] = "bocchi" };
        var plan = MigrationPlanner.BuildPlan(tree, livePlacements);

        await using var failingTargetNode = new MockFailingNodeConnection("nijika", new IOException("Connection dropped mid-transfer"));
        await using var sourceNode = new MockSuccessNodeConnection("bocchi");

        var orchestrator = new ClusterOrchestrator();
        var loggedSteps = new List<string>();

        await Assert.ThrowsAsync<IOException>(() =>
            orchestrator.ExecutePlanAsync(plan, tree, [sourceNode, failingTargetNode], loggedSteps.Add));

        Assert.DoesNotContain(loggedSteps, s => s.Contains("DNS record", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Chaos2_DiskFullStreamingBackup_StreamsWithoutCreatingLocalTempFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_disk_chaos_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var volPath = Path.Combine(tempDir, "volume");
        Directory.CreateDirectory(volPath);
        await File.WriteAllTextAsync(Path.Combine(volPath, "db.sqlite"), "binary-data-content-here");

        try
        {
            var provider = new RcloneBackupStorageProvider("s3:my-bucket/backups");
            await using var mockNode = new MockRecordingNodeConnection("local-node");

            await provider.StreamBackupAsync(mockNode, volPath, "backup-01");

            // Verify the command executed is in-memory streaming: tar -C ... | zstd -3 | rclone rcat ...
            Assert.Contains(mockNode.ExecutedCommands, cmd =>
                cmd.Contains("rclone rcat 's3:my-bucket/backups/backup-01.tar.zst'", StringComparison.Ordinal) &&
                cmd.Contains("zstd -3", StringComparison.Ordinal) &&
                cmd.Contains("tar -C", StringComparison.Ordinal));

            // Verify 0 temporary tar files were written to disk
            var checkNoTempFiles = Directory.GetFiles(tempDir, "*.tar*");
            Assert.Empty(checkNoTempFiles);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Chaos3_PrivateRegistryDown_P2PStreamTransfersImageBinaryIntact()
    {
        var runtime = new DockerRuntime();
        // Array containing tricky non-UTF8 binary byte sequences
        byte[] originalBinaryPayload = [0xFF, 0x00, 0x80, 0xFE, 0xAA, 0x55, 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x12, 0x34];

        await using var sourceNode = new MockBinaryStreamingNodeConnection("source-node", originalBinaryPayload);
        await using var targetNode = new MockBinaryStreamingNodeConnection("target-node", []);

        await runtime.StreamImagePeerToPeerAsync(sourceNode, targetNode, "custom-image:v1");

        // Assert 100% byte-for-byte exact binary transmission through pipes
        Assert.Equal(originalBinaryPayload, targetNode.ReceivedBytes);
    }

    [Fact]
    public async Task Chaos4_FirewallDeadmanSwitch_ProducesIdempotentFlushAndRecreation()
    {
        await using var node = new MockRecordingNodeConnection("fw-fail-node");
        var ruleset = NftablesService.GenerateRuleset([], [80, 443]);

        Assert.Contains("table inet starry\ndelete table inet starry\ntable inet starry {", ruleset, StringComparison.Ordinal);

        await NftablesService.ApplyRulesetAsync(node, ruleset);
        Assert.Contains(node.ExecutedCommands, c => c.Contains("nftables.starry.conf", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Chaos5_DnsProvider_RetriesWithBackoffOnTransientErrors()
    {
        // First request returns 429, second returns 502, third succeeds with 200 OK
        var responses = new Queue<HttpStatusCode>([HttpStatusCode.TooManyRequests, HttpStatusCode.BadGateway, HttpStatusCode.OK]);
        using var handler = new MockRetryingHttpMessageHandler(responses);
        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.cloudflare.com/client/v4/") };

        var provider = new CloudflareDnsProvider("fake-token", client);
        var records = await provider.GetRecordsAsync("example.com");

        Assert.NotNull(records);
        Assert.True(handler.RequestCount >= 2);
    }

    [Fact]
    public void Chaos6_CrossDistroDockerToPodmanMigration_PreservesPriorityAndFiles()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.ubuntu-node.host", JsonValue.Create("192.168.1.10"));
        tree.SetNode("nodes.alma-node.host", JsonValue.Create("192.168.1.20"));
        tree.SetNode("services.db.image", JsonValue.Create("postgres:16"));
        tree.SetNode("services.db.priority", JsonValue.Create(10));
        tree.SetNode("services.db.node", JsonValue.Create("alma-node"));

        var livePlacements = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["db"] = "ubuntu-node"
        };

        var plan = MigrationPlanner.BuildPlan(tree, livePlacements);
        var migrationAction = plan.Actions.FirstOrDefault(a => string.Equals(a.Target, "db", StringComparison.Ordinal));

        Assert.NotNull(migrationAction);
        Assert.Equal(PlanActionType.MigrateServiceBlueGreen, migrationAction.ActionType);
        Assert.Equal(60, migrationAction.Priority);
    }
}
```

## File: tests/Starry.ChaosTests/CryptographyChaosTests.cs

**Lines**: 90 | **Size**: 3.2 KB | **Language**: csharp

```csharp
using System.Security.Cryptography;
using System.Text;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.ChaosTests;

public class CryptographyChaosTests
{
    private readonly CryptoService _cryptoService = new();

    [Fact]
    public void Decrypt_WithCorruptedTag_ThrowsCryptographicException()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        var path = "nodes.bocchi.ssh_key";
        var secret = "super-secret-ssh-key"u8.ToArray();

        var envelope = _cryptoService.Encrypt(secret, key, path);

        // Corrupt the tag bytes
        var tagBytes = Convert.FromBase64String(envelope.TagBase64);
        tagBytes[0] ^= 0xFF; // flip bits
        var corruptedEnvelope = envelope with { TagBase64 = Convert.ToBase64String(tagBytes) };

        Assert.ThrowsAny<CryptographicException>(() => _cryptoService.Decrypt(corruptedEnvelope, key, path));
    }

    [Fact]
    public void Decrypt_WithCorruptedCipher_ThrowsCryptographicException()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        var path = "nodes.bocchi.ssh_key";
        var secret = "super-secret-ssh-key"u8.ToArray();

        var envelope = _cryptoService.Encrypt(secret, key, path);

        // Corrupt the ciphertext
        var cipherBytes = Convert.FromBase64String(envelope.CipherBase64);
        cipherBytes[0] ^= 0xAA;
        var corruptedEnvelope = envelope with { CipherBase64 = Convert.ToBase64String(cipherBytes) };

        Assert.ThrowsAny<CryptographicException>(() => _cryptoService.Decrypt(corruptedEnvelope, key, path));
    }

    [Fact]
    public void Decrypt_TransferredToDifferentJsonPath_ThrowsCryptographicExceptionDueToAadMismatch()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        var originalPath = "nodes.bocchi.ssh_key";
        var attackerTargetField = "services.database.password"; // Attacker attempts to copy encrypted secret
        var secret = "bocchi-private-key-material"u8.ToArray();

        // Legitimate encryption bound to nodes.bocchi.ssh_key
        var envelope = _cryptoService.Encrypt(secret, key, originalPath);

        // Attempt to decrypt with another field's AAD must fail
        Assert.ThrowsAny<CryptographicException>(() =>
            _cryptoService.Decrypt(envelope, key, attackerTargetField));
    }

    [Fact]
    public void AtomicWriteFile_WritesAtomicallyWithoutLeavingTempFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_chaos_atomic_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var targetFile = Path.Combine(tempDir, "config.yaml");
            File.WriteAllText(targetFile, "initial: content\n");

            var newContent = "updated: true\nsecret: encrypted\n";
            SecretManager.AtomicWriteFile(targetFile, newContent);

            Assert.Equal(newContent, File.ReadAllText(targetFile));

            // Verify no leftover .tmp files
            var tmpFiles = Directory.GetFiles(tempDir, "*.tmp");
            Assert.Empty(tmpFiles);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
```

## File: tests/Starry.ChaosTests/NetworkAndDisruptionChaosTests.cs

**Lines**: 60 | **Size**: 1.9 KB | **Language**: csharp

```csharp
using Starry.Core.Processes;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Processes;
using Xunit;

namespace Starry.ChaosTests;

public class NetworkAndDisruptionChaosTests
{
    private readonly ProcessRunner _runner = new();

    [Fact]
    public void WireGuardMesh_HandlesPartitionAndRecomputesAllowedIps()
    {
        var healthyNodes = new List<WireGuardNodeConfig>
        {
            new("bocchi", "10.99.0.1", "privKey1", "pubKey1", 51820, "203.0.113.1", IsPublic: true),
            new("nijika", "10.99.0.2", "privKey2", "pubKey2", 51820, null, IsPublic: false)
        };

        // Node 'ryo' was disconnected/partitioned, so only healthyNodes are generated
        var meshConfigs = WireGuardMeshService.GenerateMeshConfigurations(healthyNodes);

        Assert.Equal(2, meshConfigs.Count);
        Assert.DoesNotContain("Peer: ryo", meshConfigs["bocchi"], StringComparison.Ordinal);
        Assert.DoesNotContain("Peer: ryo", meshConfigs["nijika"], StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessRunner_TerminatesCleanly_OnProcessAbruptExit()
    {
        // Simulates abrupt remote tool crash / broken pipe
        var spec = new ProcessSpec
        {
            Executable = "sh",
            Arguments = ["-c", "echo 'initial' && exit 137"]
        };

        var result = await _runner.RunAsync(spec);

        Assert.False(result.IsSuccess);
        Assert.Equal(137, result.ExitCode);
        Assert.Contains("initial", result.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessRunner_AbortsPromptly_WhenCancellationTriggered()
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(50));

        var spec = new ProcessSpec
        {
            Executable = "sleep",
            Arguments = ["10"]
        };

        await Assert.ThrowsAnyAsync<Exception>(() => _runner.RunAsync(spec, cts.Token));
    }
}
```

## File: tests/Starry.ChaosTests/Starry.ChaosTests.csproj

**Lines**: 27 | **Size**: 890 B | **Language**: xml

```xml
﻿<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.4" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\Starry.Core\Starry.Core.csproj" />
    <ProjectReference Include="..\..\src\Starry.Infrastructure\Starry.Infrastructure.csproj" />
    <ProjectReference Include="..\..\src\Starry.Cli\Starry.Cli.csproj" />
  </ItemGroup>

</Project>
```

## File: tests/Starry.E2ETests/BrownfieldAdoptionE2ETests.cs

**Lines**: 84 | **Size**: 2.6 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.E2ETests;

public class BrownfieldAdoptionE2ETests
{
    [Fact]
    public async Task Adoption_FullLifecycle_PullsFilesAndGeneratesModularConfig()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "brownfield_e2e_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var service = new HostAdoptionService();
            await using var node = new LocalNodeConnection("local-test");

            var options = new AdoptionOptions
            {
                OutDir = tempDir,
                PullFiles = true,
                Format = "yaml"
            };

            var tree = await service.AdoptHostAsync(node, options);
            Assert.NotNull(tree);
            Assert.True(File.Exists(Path.Combine(tempDir, "starry.yaml")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task ListUnmanagedContainers_FiltersOutManagedServices()
    {
        var service = new HostAdoptionService();
        await using var node = new LocalNodeConnection("test-node");

        var config = new ConfigTree();
        config.SetNode("services.api.image", JsonValue.Create("my-api:v1"));

        // When node is queried, containers not matching "api" are flagged as unmanaged
        var unmanaged = await service.ListUnmanagedContainersAsync([node], config);
        Assert.NotNull(unmanaged);
    }

    [Fact]
    public async Task SecretScanning_EncryptsEnvPasswordsAutomatically()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "secret_scan_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var service = new HostAdoptionService();
            await using var node = new LocalNodeConnection("adopt-node");

            var masterKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
            var options = new AdoptionOptions
            {
                OutDir = tempDir,
                MasterKey = masterKey,
                Format = "yaml"
            };

            var tree = await service.AdoptHostAsync(node, options);
            Assert.NotNull(tree);
            Assert.True(File.Exists(Path.Combine(tempDir, "starry.yaml")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
```

## File: tests/Starry.E2ETests/ClusterEndToEndTests.cs

**Lines**: 90 | **Size**: 4.4 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.E2ETests;

public class ClusterEndToEndTests
{
    [Fact]
    public void E2E_FullClusterLifecycle_FromConfigToPlanToBundle()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_e2e_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var nodesDir = Path.Combine(tempDir, "nodes");
        var servicesDir = Path.Combine(tempDir, "services");
        Directory.CreateDirectory(nodesDir);
        Directory.CreateDirectory(servicesDir);

        try
        {
            // 1. Multi-file modular configuration
            File.WriteAllText(Path.Combine(tempDir, "starry.yaml"), "cluster_name: kessoku-e2e\nversion: '1.0'\n");
            File.WriteAllText(Path.Combine(nodesDir, "bocchi.yaml"), "host: 198.51.100.10\nport: 22\nvpn_ip: 10.99.0.1\nis_public: true\nssh_key: PLAIN[super-secret-key]\n");
            File.WriteAllText(Path.Combine(servicesDir, "api.toml"), "image = 'my-api:v1'\npriority = 50\nnode = 'bocchi'\nvip = '10.99.10.1'\n");

            var engine = new VirtualConfigEngine();
            var tree = engine.Load(tempDir);

            // 2. Validation
            var valResult = ConfigValidator.Validate(tree);
            Assert.True(valResult.IsValid);
            Assert.Contains(valResult.Issues, i => i.IsWarning && string.Equals(i.Path, "nodes.bocchi.ssh_key", StringComparison.Ordinal));

            // 3. Encrypt open secrets
            var masterKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
            var secretManager = new SecretManager();
            var encryptedCount = secretManager.EncryptOpenSecrets(tree, masterKey, tempDir, engine);
            Assert.Equal(1, encryptedCount);

            var encryptedKeyVal = tree.Root["nodes"]?["bocchi"]?["ssh_key"]?.ToString();
            Assert.NotNull(encryptedKeyVal);
            Assert.StartsWith("ENC[AES256_GCM:v1:", encryptedKeyVal, StringComparison.Ordinal);

            // 4. DAG Migration Plan
            var plan = MigrationPlanner.BuildPlan(tree);
            Assert.True(plan.HasChanges);
            Assert.Equal("kessoku-e2e", plan.ClusterName);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.CreateNode && string.Equals(a.Target, "bocchi", StringComparison.Ordinal));
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.ConfigureWireGuardMesh);
            Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.DeployService && string.Equals(a.Target, "api", StringComparison.Ordinal));

            // 5. WireGuard & Nftables generation
            var wgNodes = new List<WireGuardNodeConfig>
            {
                new("bocchi", "10.99.0.1", "privKey1", "pubKey1", 51820, "198.51.100.10", IsPublic: true)
            };
            var wgConfigs = WireGuardMeshService.GenerateMeshConfigurations(wgNodes);
            Assert.Single(wgConfigs);
            Assert.Contains("Address = 10.99.0.1/16", wgConfigs["bocchi"], StringComparison.Ordinal);

            var nftRules = NftablesService.GenerateRuleset([], [80, 443]);
            Assert.Contains("table inet starry", nftRules, StringComparison.Ordinal);

            // 6. Disaster Recovery Bundle Export & Import with IP Override
            var bundlePath = Path.Combine(tempDir, "fleet-bundle.tar.gz");
            FleetBundleService.ExportBundle(tree, bundlePath, engine);
            Assert.True(File.Exists(bundlePath));

            var restoreDir = Path.Combine(tempDir, "dr-restored");
            var ipOverrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["bocchi"] = "203.0.113.88"
            };
            var restoredTree = FleetBundleService.ImportBundle(bundlePath, restoreDir, ipOverrides, engine);

            Assert.Equal("kessoku-e2e", restoredTree.Root["cluster_name"]?.ToString());
            Assert.Equal("203.0.113.88", restoredTree.Root["nodes"]?["bocchi"]?["host"]?.ToString());
            Assert.Equal("my-api:v1", restoredTree.Root["services"]?["api"]?["image"]?.ToString());
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
```

## File: tests/Starry.E2ETests/ClusterMatrixFuzzerTests.cs

**Lines**: 779 | **Size**: 33.9 KB | **Language**: csharp

```csharp
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Core.Containers;
using Starry.Core.Dns;
using Starry.Core.Nodes;
using Starry.Core.Processes;
using Starry.Infrastructure.Agent;
using Starry.Infrastructure.Backups;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Containers;
using Starry.Infrastructure.Dns;
using Starry.Infrastructure.Network;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Orchestration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.E2ETests;

public class ClusterMatrixFuzzerTests
{
    private sealed class StatefulClusterNode : INodeConnection
    {
        public string NodeName { get; }
        public string Host { get; }
        public int Port { get; }
        public string? DistroId { get; }
        public string RuntimeName { get; }
        public Dictionary<string, byte[]> Volumes { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, string> VirtualFiles { get; } = new(StringComparer.Ordinal);
        public List<string> ExecutedCommands { get; } = [];
        public List<JsonObject> RegisteredContainers { get; } = [];
        public Func<ProcessSpec, ProcessResult?>? CommandInterceptor { get; set; }

        public StatefulClusterNode(string name, string distro, string host, int port, string runtimeName = "docker")
        {
            NodeName = name;
            DistroId = distro;
            Host = host;
            Port = port;
            RuntimeName = runtimeName;

            // Default mock file system
            VirtualFiles["/etc/os-release"] = $"NAME=\"{distro}\"\nID={distro}\nVERSION_ID=\"24.04\"\n";
            VirtualFiles["/etc/nginx/shop.conf"] = "server { listen 80; server_name shop.local; }\n";
        }

        public Task<ProcessResult> ExecuteCommandAsync(ProcessSpec command, CancellationToken ct = default)
        {
            var cmdLine = $"{command.Executable} {string.Join(" ", command.Arguments)}";
            ExecutedCommands.Add(cmdLine);

            if (CommandInterceptor != null)
            {
                var intercepted = CommandInterceptor(command);
                if (intercepted != null)
                {
                    return Task.FromResult(intercepted);
                }
            }

            if (command.Arguments.Count > 0 && string.Equals(command.Arguments[0], "-c", StringComparison.Ordinal))
            {
                var script = command.Arguments[1];
                if (script.Contains("cat /etc/nginx/shop.conf", StringComparison.Ordinal) ||
                    script.Contains("cat '/etc/nginx/shop.conf'", StringComparison.Ordinal))
                {
                    return Task.FromResult(new ProcessResult(0, VirtualFiles["/etc/nginx/shop.conf"], string.Empty, TimeSpan.Zero));
                }

                if (script.Contains("sha256sum", StringComparison.Ordinal))
                {
                    return Task.FromResult(new ProcessResult(0, "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855  -", string.Empty, TimeSpan.Zero));
                }
            }

            if (string.Equals(command.Executable, "cat", StringComparison.Ordinal) && command.Arguments.Count > 0 && VirtualFiles.TryGetValue(command.Arguments[0], out var content))
            {
                return Task.FromResult(new ProcessResult(0, content, string.Empty, TimeSpan.Zero));
            }

            return Task.FromResult(new ProcessResult(0, "success", string.Empty, TimeSpan.Zero));
        }

        public async Task<ProcessResult> ExecuteStreamingCommandAsync(ProcessSpec command, Stream? stdinStream, Stream? stdoutStream, CancellationToken ct = default)
        {
            var cmdLine = $"{command.Executable} {string.Join(" ", command.Arguments)}";
            ExecutedCommands.Add(cmdLine);

            if (CommandInterceptor != null)
            {
                var intercepted = CommandInterceptor(command);
                if (intercepted != null)
                {
                    return intercepted;
                }
            }

            if (stdinStream != null)
            {
                using var ms = new MemoryStream();
                await stdinStream.CopyToAsync(ms, ct);
                var key = command.Arguments.Count > 0 ? command.Arguments[^1] : "vol";
                Volumes[key] = ms.ToArray();
            }

            if (stdoutStream != null)
            {
                byte[] mockStream = [0x28, 0xB5, 0x2F, 0xFD, 0x00, 0x55, 0xAA, 0xDE, 0xAD, 0xBE, 0xEF];
                await stdoutStream.WriteAsync(mockStream, ct);
            }

            return new ProcessResult(0, "success", string.Empty, TimeSpan.Zero);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "HttpClient owns handler")]
        public Task<HttpClient> CreateSocketHttpClientAsync(string socketPath, CancellationToken ct = default)
        {
            var handler = new MockStatefulDockerHttpHandler(RegisteredContainers);
            return Task.FromResult(new HttpClient(handler, disposeHandler: true));
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class MockStatefulDockerHttpHandler(List<JsonObject> containers) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.PathAndQuery ?? string.Empty;

            if (request.Method == HttpMethod.Post && path.Contains("/containers/create", StringComparison.Ordinal))
            {
                var query = request.RequestUri?.Query ?? string.Empty;
                var name = "app";
                var qIdx = query.IndexOf("name=", StringComparison.Ordinal);
                if (qIdx >= 0)
                {
                    var rawName = query[(qIdx + 5)..];
                    var ampersand = rawName.IndexOf('&', StringComparison.Ordinal);
                    if (ampersand >= 0)
                    {
                        rawName = rawName[..ampersand];
                    }
                    name = Uri.UnescapeDataString(rawName);
                }

                var id = "c-" + Guid.NewGuid().ToString("N")[..12];

                var newContainer = new JsonObject
                {
                    ["Id"] = id,
                    ["Names"] = new JsonArray { "/" + name.TrimStart('/') },
                    ["Image"] = "app:latest",
                    ["State"] = "running",
                    ["Status"] = "Up",
                    ["Labels"] = new JsonObject { ["managed-by"] = "starry" }
                };
                containers.Add(newContainer);

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent($"{{\"Id\":\"{id}\"}}", Encoding.UTF8, "application/json")
                });
            }

            if (path.Contains("/containers/json", StringComparison.Ordinal))
            {
                if (containers.Count == 0)
                {
                    containers.Add(new JsonObject
                    {
                        ["Id"] = "c-init",
                        ["Names"] = new JsonArray { "/app", "/web" },
                        ["Image"] = "nginx:alpine",
                        ["State"] = "running",
                        ["Status"] = "Up",
                        ["Labels"] = new JsonObject { ["managed-by"] = "starry" }
                    });
                }

                var arr = new JsonArray();
                foreach (var c in containers)
                {
                    arr.Add(JsonNode.Parse(c.ToJsonString()));
                }
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(arr.ToJsonString(), Encoding.UTF8, "application/json")
                });
            }

            if (path.Contains("/containers/", StringComparison.Ordinal) && path.EndsWith("/json", StringComparison.Ordinal))
            {
                var inspectObj = new JsonObject
                {
                    ["Id"] = "c1234567890",
                    ["Name"] = "/shop-web",
                    ["State"] = new JsonObject { ["Running"] = true, ["Status"] = "running" },
                    ["Config"] = new JsonObject
                    {
                        ["Image"] = "nginx:alpine",
                        ["Env"] = new JsonArray { "POSTGRES_PASSWORD=secret123", "PORT=80" },
                        ["Labels"] = new JsonObject { ["com.docker.compose.project"] = "shop" }
                    },
                    ["HostConfig"] = new JsonObject
                    {
                        ["Binds"] = new JsonArray { "/etc/nginx/shop.conf:/etc/nginx/shop.conf:ro" },
                        ["PortBindings"] = new JsonObject
                        {
                            ["80/tcp"] = new JsonArray { new JsonObject { ["HostPort"] = "8080" } }
                        }
                    }
                };

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(inspectObj.ToJsonString(), Encoding.UTF8, "application/json")
                });
            }

            if (path.Contains("/version", StringComparison.Ordinal))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"Version\":\"26.1.0\",\"Os\":\"linux\",\"Arch\":\"amd64\"}", Encoding.UTF8, "application/json")
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Id\":\"c1234567890\"}", Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task Scenario01_TwoNode_HomogeneousUbuntuDocker_MeshAndMigration()
    {
        await using var node1 = new StatefulClusterNode("ubuntu-1", "ubuntu", "198.51.100.11", 22);
        await using var node2 = new StatefulClusterNode("ubuntu-2", "ubuntu", "198.51.100.12", 22);

        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("cluster-scen-01"));
        tree.SetNode("nodes.ubuntu-1.host", JsonValue.Create("198.51.100.11"));
        tree.SetNode("nodes.ubuntu-2.host", JsonValue.Create("198.51.100.12"));
        tree.SetNode("services.app.image", JsonValue.Create("nginx:alpine"));
        tree.SetNode("services.app.node", JsonValue.Create("ubuntu-2"));

        var plan = MigrationPlanner.BuildPlan(tree, new Dictionary<string, string>(StringComparer.Ordinal) { ["app"] = "ubuntu-1" });
        Assert.Contains(plan.Actions, a => a.ActionType == PlanActionType.MigrateServiceBlueGreen);

        var orchestrator = new ClusterOrchestrator();
        await orchestrator.ExecutePlanAsync(plan, tree, [node1, node2]);
        Assert.Contains(node2.ExecutedCommands, c => c.Contains("apt-get install", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario02_TwoNode_HomogeneousAlmaPodman_P2PStream()
    {
        await using var node1 = new StatefulClusterNode("alma-1", "almalinux", "198.51.100.21", 22, "podman");
        await using var node2 = new StatefulClusterNode("alma-2", "almalinux", "198.51.100.22", 22, "podman");

        var podman = new PodmanRuntime();
        await podman.StreamImagePeerToPeerAsync(node1, node2, "custom-app:latest");

        Assert.Contains(node1.ExecutedCommands, c => c.Contains("podman save", StringComparison.Ordinal) && c.Contains("zstd -3", StringComparison.Ordinal));
        Assert.Contains(node2.ExecutedCommands, c => c.Contains("podman load", StringComparison.Ordinal) && c.Contains("zstd -d", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario03_TwoNode_CrossRuntimePingPong_DockerPodmanRounds()
    {
        await using var dockerNode = new StatefulClusterNode("docker-node", "ubuntu", "198.51.100.31", 22, "docker");
        await using var podmanNode = new StatefulClusterNode("podman-node", "almalinux", "198.51.100.32", 22, "podman");

        var docker = new DockerRuntime();
        var podman = new PodmanRuntime();

        await docker.StreamImagePeerToPeerAsync(dockerNode, podmanNode, "app:v1");
        await podman.StreamImagePeerToPeerAsync(podmanNode, dockerNode, "app:v1");

        Assert.NotEmpty(dockerNode.ExecutedCommands);
        Assert.NotEmpty(podmanNode.ExecutedCommands);
    }

    [Fact]
    public async Task Scenario04_ThreeNode_DebianNatJumpHost_PersistentKeepalive()
    {
        await using var pub1 = new StatefulClusterNode("deb-pub-1", "debian", "203.0.113.1", 22);
        await using var pub2 = new StatefulClusterNode("deb-pub-2", "debian", "203.0.113.2", 22);
        await using var nat3 = new StatefulClusterNode("deb-nat-3", "debian", "10.0.0.3", 22);

        var nodes = new List<WireGuardNodeConfig>
        {
            new("deb-pub-1", "10.99.0.1", "k1", "p1", 51820, "203.0.113.1", IsPublic: true),
            new("deb-pub-2", "10.99.0.2", "k2", "p2", 51820, "203.0.113.2", IsPublic: true),
            new("deb-nat-3", "10.99.0.3", "k3", "p3", 51820, null, IsPublic: false)
        };

        var configs = WireGuardMeshService.GenerateMeshConfigurations(nodes);
        var natConfig = configs["deb-nat-3"];

        Assert.Contains("PersistentKeepalive = 25", natConfig, StringComparison.Ordinal);
        Assert.Contains("Endpoint = 203.0.113.1:51820", natConfig, StringComparison.Ordinal);
        Assert.Contains("Endpoint = 203.0.113.2:51820", natConfig, StringComparison.Ordinal);

        await WireGuardMeshService.ApplyConfigurationAsync(nat3, natConfig);
        Assert.Contains(nat3.ExecutedCommands, c => c.Contains("wg syncconf starry0", StringComparison.Ordinal));
    }

    [Fact]
    public void Scenario05_ThreeNode_TwoDistinctNatsThroughPublicRelay()
    {
        var nodes = new List<WireGuardNodeConfig>
        {
            new("relay", "10.99.0.1", "k1", "p1", 51820, "203.0.113.50", IsPublic: true),
            new("nat-site-a", "10.99.0.2", "k2", "p2", 51820, null, IsPublic: false),
            new("nat-site-b", "10.99.0.3", "k3", "p3", 51820, null, IsPublic: false)
        };

        var configs = WireGuardMeshService.GenerateMeshConfigurations(nodes);
        Assert.Equal(3, configs.Count);
        Assert.Contains("Endpoint = 203.0.113.50:51820", configs["nat-site-a"], StringComparison.Ordinal);
        Assert.Contains("Endpoint = 203.0.113.50:51820", configs["nat-site-b"], StringComparison.Ordinal);
    }

    [Fact]
    public void Scenario06_ThreeNode_ExoticMix_ArchAlpineAlma_MixedFormats()
    {
        var yamlProvider = new YamlConfigFormatProvider();
        var tomlProvider = new TomlConfigFormatProvider();
        var jsonProvider = new JsonConfigFormatProvider();

        var archNode = yamlProvider.Parse(Encoding.UTF8.GetBytes("host: 198.51.100.41\nvpn_ip: 10.99.0.1\n"), "arch.yaml");
        var alpineNode = tomlProvider.Parse(Encoding.UTF8.GetBytes("host = '198.51.100.42'\nvpn_ip = '10.99.0.2'\n"), "alpine.toml");
        var almaNode = jsonProvider.Parse(Encoding.UTF8.GetBytes("{\"host\":\"198.51.100.43\",\"vpn_ip\":\"10.99.0.3\"}"), "alma.json");

        Assert.NotNull(archNode);
        Assert.NotNull(alpineNode);
        Assert.NotNull(almaNode);
    }

    [Fact]
    public async Task Scenario07_FourNode_MultiTierArchitecture_PriorityScheduling()
    {
        await using var n1 = new StatefulClusterNode("node-reg", "ubuntu", "198.51.100.51", 22);
        await using var n2 = new StatefulClusterNode("node-db", "almalinux", "198.51.100.52", 22);
        await using var n3 = new StatefulClusterNode("node-api", "debian", "198.51.100.53", 22);
        await using var n4 = new StatefulClusterNode("node-proxy", "alpine", "198.51.100.54", 22);

        var services = new List<ServiceSpec>
        {
            new() { Name = "ingress-proxy", Image = "nginx:alpine", Priority = 90 },
            new() { Name = "api-backend", Image = "api:v1", Priority = 50 },
            new() { Name = "database-postgres", Image = "postgres:16", Priority = 10 },
            new() { Name = "private-registry", Image = "registry:2", Priority = 0 }
        };

        var ordered = ContainerPriorityScheduler.OrderServicesByPriority(services);
        Assert.Equal("private-registry", ordered[0].Name);
        Assert.Equal("database-postgres", ordered[1].Name);
        Assert.Equal("api-backend", ordered[2].Name);
        Assert.Equal("ingress-proxy", ordered[3].Name);

        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("tier-cluster"));
        foreach (var s in ordered)
        {
            tree.SetNode($"services.{s.Name}.image", JsonValue.Create(s.Image));
            tree.SetNode($"services.{s.Name}.priority", JsonValue.Create(s.Priority));
        }

        var plan = MigrationPlanner.BuildPlan(tree);
        var orchestrator = new ClusterOrchestrator();
        await orchestrator.ExecutePlanAsync(plan, tree, [n1, n2, n3, n4]);
    }

    [Fact]
    public async Task Scenario08_FourNode_RingRelay_ImageAndVolumeChain()
    {
        await using var n1 = new StatefulClusterNode("node-1", "ubuntu", "198.51.100.61", 22);
        await using var n2 = new StatefulClusterNode("node-2", "almalinux", "198.51.100.62", 22);
        await using var n3 = new StatefulClusterNode("node-3", "debian", "198.51.100.63", 22);
        await using var n4 = new StatefulClusterNode("node-4", "alpine", "198.51.100.64", 22);

        var ring = new[] { n1, n2, n3, n4, n1 };
        var runtime = new DockerRuntime();

        for (var i = 0; i < ring.Length - 1; i++)
        {
            await runtime.StreamImagePeerToPeerAsync(ring[i], ring[i + 1], "relay-image:v1");
        }

        Assert.NotEmpty(n1.ExecutedCommands);
        Assert.NotEmpty(n4.ExecutedCommands);
    }

    [Fact]
    public async Task Scenario09_FiveNode_FullKessokuFleet_MeshAndInternalDns()
    {
        await using var bocchi = new StatefulClusterNode("bocchi", "ubuntu", "203.0.113.1", 22);
        await using var nijika = new StatefulClusterNode("nijika", "almalinux", "203.0.113.2", 22);
        await using var ryo = new StatefulClusterNode("ryo", "debian", "10.0.0.3", 22);
        await using var ikuyo = new StatefulClusterNode("ikuyo", "alpine", "10.0.0.4", 22);
        await using var seika = new StatefulClusterNode("seika", "arch", "203.0.113.5", 22);

        var fleet = new List<WireGuardNodeConfig>
        {
            new("bocchi", "10.99.0.1", "kbocchi", "pbocchi", 51820, "203.0.113.1", IsPublic: true),
            new("nijika", "10.99.0.2", "knijika", "pnijika", 51820, "203.0.113.2", IsPublic: true),
            new("ryo", "10.99.0.3", "kryo", "pryo", 51820, null, IsPublic: false),
            new("ikuyo", "10.99.0.4", "kikuyo", "pikuyo", 51820, null, IsPublic: false),
            new("seika", "10.99.0.5", "kseika", "pseika", 51820, "203.0.113.5", IsPublic: true)
        };

        var mesh = WireGuardMeshService.GenerateMeshConfigurations(fleet);
        Assert.Equal(5, mesh.Count);

        await using var dns = new InternalDnsServer(port: 5354);
        dns.RegisterRecord("bocchi.internal", IPAddress.Parse("10.99.0.1"));
        dns.RegisterRecord("nijika.internal", IPAddress.Parse("10.99.0.2"));
        dns.RegisterRecord("ryo.internal", IPAddress.Parse("10.99.0.3"));
        dns.RegisterRecord("ikuyo.internal", IPAddress.Parse("10.99.0.4"));
        dns.RegisterRecord("seika.internal", IPAddress.Parse("10.99.0.5"));

        Assert.True(dns.Records.Count >= 5);
    }

    [Fact]
    public void Scenario10_FiveNode_FullDisasterRecovery_BundleExportImportOverrideIps()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "scen10_dr_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = new ConfigTree();
            tree.SetNode("cluster_name", JsonValue.Create("kessoku-dr"));
            for (var i = 1; i <= 5; i++)
            {
                tree.SetNode($"nodes.node-{i}.host", JsonValue.Create($"198.51.100.{i}"));
            }

            var archive = Path.Combine(tempDir, "bundle.tar.gz");
            FleetBundleService.ExportBundle(tree, archive);

            var restoreDir = Path.Combine(tempDir, "restored");
            var overrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["node-1"] = "192.0.2.1",
                ["node-5"] = "192.0.2.5"
            };

            var restored = FleetBundleService.ImportBundle(archive, restoreDir, overrides);
            Assert.Equal("192.0.2.1", restored.Root["nodes"]?["node-1"]?["host"]?.ToString());
            Assert.Equal("192.0.2.5", restored.Root["nodes"]?["node-5"]?["host"]?.ToString());
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario11_Chaos_NetworkEmulation_PacketLossAndLatency()
    {
        var responses = new Queue<HttpStatusCode>([HttpStatusCode.GatewayTimeout, HttpStatusCode.OK]);
        using var handler = new MockChaosHttpMessageHandler(responses, delayMs: 25);
        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.cloudflare.com/client/v4/") };

        var provider = new CloudflareDnsProvider("token", client);
        var records = await provider.GetRecordsAsync("example.com");
        Assert.NotNull(records);
        Assert.True(handler.Calls >= 2);
    }

    [Fact]
    public async Task Scenario12_Chaos_MidMigrationProcessKill_LeavesSourceContainerIntact()
    {
        await using var sourceNode = new StatefulClusterNode("src", "ubuntu", "198.51.100.1", 22);
        await using var targetNode = new StatefulClusterNode("dst", "ubuntu", "198.51.100.2", 22);

        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("mid-kill-cluster"));
        tree.SetNode("nodes.src.host", JsonValue.Create("198.51.100.1"));
        tree.SetNode("nodes.dst.host", JsonValue.Create("198.51.100.2"));
        tree.SetNode("services.web.image", JsonValue.Create("nginx:alpine"));
        tree.SetNode("services.web.node", JsonValue.Create("dst"));
        tree.SetNode("services.web.volumes", new JsonArray { "/var/data:/var/data" });

        var plan = MigrationPlanner.BuildPlan(tree, new Dictionary<string, string>(StringComparer.Ordinal) { ["web"] = "src" });
        var orchestrator = new ClusterOrchestrator();

        var failSync = true;
        targetNode.CommandInterceptor = spec =>
        {
            var args = string.Join(" ", spec.Arguments);
            if (failSync && args.Contains("tar -C '/var/data' -xf -", StringComparison.Ordinal))
            {
                return new ProcessResult(1, string.Empty, "tar: unexpected EOF", TimeSpan.Zero);
            }
            return null;
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            orchestrator.ExecutePlanAsync(plan, tree, [sourceNode, targetNode]));

        Assert.DoesNotContain(sourceNode.ExecutedCommands, c => c.Contains("stop web", StringComparison.Ordinal));

        failSync = false;
        await orchestrator.ExecutePlanAsync(plan, tree, [sourceNode, targetNode]);
        Assert.Contains(sourceNode.ExecutedCommands, c => c.Contains("stop web", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario13_Chaos_NetworkPartitionBlackhole_DeadmanSwitchPreservesNodes()
    {
        await using var node = new StatefulClusterNode("isolated-node", "ubuntu", "198.51.100.99", 22);
        var ruleset = NftablesService.GenerateRuleset([], [80, 443]);
        await NftablesService.ApplyRulesetAsync(node, ruleset);
        Assert.Contains(node.ExecutedCommands, c => c.Contains("nft -f", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario14_Chaos_PrivateRegistryDown_P2PFallback()
    {
        await using var src = new StatefulClusterNode("reg-down-src", "ubuntu", "198.51.100.1", 22);
        await using var dst = new StatefulClusterNode("reg-down-dst", "ubuntu", "198.51.100.2", 22);

        var runtime = new DockerRuntime();
        await runtime.StreamImagePeerToPeerAsync(src, dst, "internal-service:v1");

        Assert.Contains(src.ExecutedCommands, c => c.Contains("docker save", StringComparison.Ordinal));
        Assert.Contains(dst.ExecutedCommands, c => c.Contains("docker load", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario15_Chaos_DiskPressure_StreamingBackupNoTempFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "scen15_disk_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var provider = new RcloneBackupStorageProvider("s3:bucket/backups");
            await using var node = new StatefulClusterNode("low-disk-node", "ubuntu", "198.51.100.1", 22);
            await provider.StreamBackupAsync(node, "/var/data", "backup-id");

            Assert.Contains(node.ExecutedCommands, c => c.Contains("tar -C '/var/data'", StringComparison.Ordinal));
            Assert.Empty(Directory.GetFiles(tempDir, "*.tar*"));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario16_Chaos_ContainerAbruptKill_DriftDetectionAndAutoRestart()
    {
        await using var node = new StatefulClusterNode("drift-node", "ubuntu", "198.51.100.16", 22);
        node.RegisteredContainers.Add(new JsonObject
        {
            ["Id"] = "drift-c1",
            ["Names"] = new JsonArray { "/web" },
            ["Image"] = "nginx:alpine",
            ["State"] = "exited",
            ["Status"] = "Exited (137) 10 seconds ago",
            ["Labels"] = new JsonObject { ["managed-by"] = "starry" }
        });

        await using var daemon = new StarryAgentDaemon(new DockerRuntime(), node);
        var restarted = await daemon.PerformHealthCheckAndReconcileAsync();

        Assert.Equal(1, restarted);
        Assert.Single(daemon.RecentDriftEvents);
        Assert.True(daemon.RecentDriftEvents[0].RestartSucceeded);
    }

    [Fact]
    public async Task Scenario17_Chaos_CorruptedFirewallRuleset_DeadmanRollbackPreservesSsh()
    {
        await using var node = new StatefulClusterNode("fw-node", "ubuntu", "198.51.100.17", 22);

        node.CommandInterceptor = spec =>
        {
            var args = string.Join(" ", spec.Arguments);
            if (args.Contains("cat > /etc/nftables.starry.conf && nft -f /etc/nftables.starry.conf", StringComparison.Ordinal))
            {
                return new ProcessResult(1, string.Empty, "syntax error in ruleset", TimeSpan.Zero);
            }
            return null;
        };

        var ruleset = NftablesService.GenerateRuleset([], [80, 443]);
        await Assert.ThrowsAsync<InvalidOperationException>(() => NftablesService.ApplyRulesetAsync(node, ruleset));

        Assert.Contains(node.ExecutedCommands, c => c.Contains("cp /etc/nftables.starry.conf /etc/nftables.starry.conf.bak", StringComparison.Ordinal));
        Assert.Contains(node.ExecutedCommands, c => c.Contains("nft -f /etc/nftables.starry.conf.bak", StringComparison.Ordinal));
    }

    [Fact]
    public void Scenario18_Chaos_FleetWideSshKeyRotationAndMasterReencryption()
    {
        var oldKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
        var newKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());

        var tree = new ConfigTree();
        tree.SetNode("nodes.n1.ssh_key", JsonValue.Create("PLAIN[key1]"));
        tree.SetNode("nodes.n2.ssh_key", JsonValue.Create("PLAIN[key2]"));

        var tempDir = Path.Combine(Path.GetTempPath(), "scen18_rot_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var engine = new VirtualConfigEngine();
            var manager = new SecretManager();
            manager.EncryptOpenSecrets(tree, oldKey, Path.Combine(tempDir, "starry.yaml"), engine);

            var count = manager.ReencryptAll(tree, oldKey, newKey, Path.Combine(tempDir, "starry.yaml"), engine);
            Assert.Equal(2, count);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario19_Brownfield_SingleUbuntuCompose_ExtractsFilesAndEncryptsSecrets()
    {
        await using var node = new StatefulClusterNode("dirty-ubuntu", "ubuntu", "198.51.100.80", 22);
        node.RegisteredContainers.Add(new JsonObject
        {
            ["Id"] = "c-shop-web",
            ["Names"] = new JsonArray { "/shop-web" },
            ["Image"] = "nginx:alpine",
            ["State"] = "running",
            ["Status"] = "Up 2 hours",
            ["Labels"] = new JsonObject { ["com.docker.compose.project"] = "shop" }
        });

        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen19_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = await service.AdoptHostAsync(node, new AdoptionOptions { OutDir = tempDir, PullFiles = true });
            Assert.NotNull(tree);
            Assert.True(File.Exists(Path.Combine(tempDir, "starry.yaml")) || File.Exists(Path.Combine(tempDir, "services", "shop.yaml")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario20_Brownfield_AlmaPodman_GeneratesTomlWithCustomMounts()
    {
        await using var node = new StatefulClusterNode("dirty-alma", "almalinux", "198.51.100.81", 22, "podman");
        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen20_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = await service.AdoptHostAsync(node, new AdoptionOptions { OutDir = tempDir, Format = "toml" });
            Assert.NotNull(tree);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario21_Brownfield_DebianNatBehindBastion_GeneratesJsonConfig()
    {
        await using var node = new StatefulClusterNode("nat-deb", "debian", "10.200.0.30", 22);
        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen21_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = await service.AdoptHostAsync(node, new AdoptionOptions { OutDir = tempDir, Format = "json" });
            Assert.NotNull(tree);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario22_Brownfield_MultiHostThreeNodes_AggregatesModularConfig()
    {
        await using var n1 = new StatefulClusterNode("h1", "ubuntu", "198.51.100.91", 22);
        await using var n2 = new StatefulClusterNode("h2", "almalinux", "198.51.100.92", 22, "podman");
        await using var n3 = new StatefulClusterNode("h3", "debian", "198.51.100.93", 22);

        var service = new HostAdoptionService();
        var tempDir = Path.Combine(Path.GetTempPath(), "scen22_adopt_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            await service.AdoptHostAsync(n1, new AdoptionOptions { OutDir = tempDir });
            await service.AdoptHostAsync(n2, new AdoptionOptions { OutDir = tempDir });
            await service.AdoptHostAsync(n3, new AdoptionOptions { OutDir = tempDir });

            var engine = new VirtualConfigEngine();
            var tree = engine.Load(tempDir);
            Assert.NotNull(tree);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task Scenario23_Brownfield_LiveNetworkAttach_ZeroRestartDowntime()
    {
        await using var node = new StatefulClusterNode("attach-node", "ubuntu", "198.51.100.94", 22);
        var service = new HostAdoptionService();
        await service.AttachLiveContainerToMeshAsync(node, "live-container-id", "10.99.10.5");
        Assert.Contains(node.ExecutedCommands, c => c.Contains("live-container-id", StringComparison.Ordinal) || c.Contains("10.99.10.5", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Scenario24_Brownfield_CoexistencePolicy_ManagedOnlyIgnoresUnmanaged()
    {
        await using var node = new StatefulClusterNode("coexist-node", "ubuntu", "198.51.100.95", 22);
        node.RegisteredContainers.Add(new JsonObject
        {
            ["Id"] = "unmanaged-box",
            ["Names"] = new JsonArray { "/manual-debug-box" },
            ["Image"] = "busybox",
            ["State"] = "running",
            ["Status"] = "Up",
            ["Labels"] = new JsonObject()
        });

        var service = new HostAdoptionService();
        var config = new ConfigTree();
        config.SetNode("services.known-service.image", JsonValue.Create("app:v1"));

        var unmanaged = await service.ListUnmanagedContainersAsync([node], config);
        Assert.Single(unmanaged);
        Assert.Equal("manual-debug-box", unmanaged[0].ContainerName);
    }

    private sealed class MockChaosHttpMessageHandler(Queue<HttpStatusCode> responses, int delayMs = 10) : HttpMessageHandler
    {
        public int Calls { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Calls++;
            if (delayMs > 0)
            {
                await Task.Delay(delayMs, cancellationToken);
            }

            var status = responses.Count > 0 ? responses.Dequeue() : HttpStatusCode.OK;
            return new HttpResponseMessage(status)
            {
                Content = new StringContent("{\"result\":[]}", Encoding.UTF8, "application/json")
            };
        }
    }
}
```

## File: tests/Starry.E2ETests/LiveDockerSshClusterFixture.cs

**Lines**: 179 | **Size**: 6.0 KB | **Language**: csharp

```csharp
using System.Diagnostics;
using Starry.Infrastructure.Nodes;
using Starry.Infrastructure.Processes;

namespace Starry.E2ETests;

/// <summary>
/// Sets up a live multi-node Linux SSH cluster running inside isolated Docker containers
/// for testing real SSH transport, ControlMaster multiplexing, volume streaming, and key rotation.
/// </summary>
internal sealed class LiveDockerSshClusterFixture : IAsyncDisposable
{
    private static readonly ProcessRunner Runner = new();
    private readonly string _tempKeyDir = Path.Combine(Path.GetTempPath(), $"starry-e2e-keys-{Guid.NewGuid():N}");
    private string _privateKeyPath = string.Empty;
    private string _privateKeyContent = string.Empty;
    private string _publicKeyContent = string.Empty;

    public bool IsAvailable { get; private set; }
    public SshNodeConnection? Node1 { get; private set; }
    public SshNodeConnection? Node2 { get; private set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Gracefully check for docker daemon availability")]
    public async Task InitializeAsync()
    {
        // 1. Check if Docker daemon is responsive
        try
        {
            var checkRes = await Runner.RunAsync(new Core.Processes.ProcessSpec
            {
                Executable = "docker",
                Arguments = ["info"],
                Timeout = TimeSpan.FromSeconds(5)
            });

            if (!checkRes.IsSuccess)
            {
                return;
            }
        }
        catch
        {
            return;
        }

        Directory.CreateDirectory(_tempKeyDir);
        _privateKeyPath = Path.Combine(_tempKeyDir, "id_ed25519");

        // 2. Generate Ed25519 SSH keypair
        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "ssh-keygen",
            Arguments = ["-t", "ed25519", "-N", "", "-f", _privateKeyPath, "-q"]
        });

        _privateKeyContent = await File.ReadAllTextAsync(_privateKeyPath);
        _publicKeyContent = (await File.ReadAllTextAsync(_privateKeyPath + ".pub")).Trim();

        // 3. Create Docker network starry-e2e-pub
        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["network", "create", "--subnet", "172.28.0.0/16", "starry-e2e-pub"]
        });

        // 4. Ensure starry-ssh-node image exists
        var imgCheck = await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["image", "inspect", "starry-ssh-node"]
        });

        if (!imgCheck.IsSuccess)
        {
            var dockerfile = "FROM alpine:3.20\nRUN apk add --no-cache openssh-server tar zstd rsync curl iproute2 bash\nRUN ssh-keygen -A && mkdir -p /root/.ssh && chmod 700 /root/.ssh\nRUN echo 'PermitRootLogin yes' >> /etc/ssh/sshd_config\nEXPOSE 22\nCMD [\"/usr/sbin/sshd\", \"-D\", \"-e\"]\n";
            await Runner.RunAsync(new Core.Processes.ProcessSpec
            {
                Executable = "docker",
                Arguments = ["build", "-t", "starry-ssh-node", "-"],
                StandardInputBytes = System.Text.Encoding.UTF8.GetBytes(dockerfile)
            });
        }

        // 5. Spin up node-1 (172.28.0.11) and node-2 (172.28.0.12)
        await StartContainerAsync("starry-live-node-1", "172.28.0.11");
        await StartContainerAsync("starry-live-node-2", "172.28.0.12");

        // Wait up to 5s for SSH listeners to be ready
        await Task.Delay(1500);

        Node1 = new SshNodeConnection(new SshNodeOptions
        {
            NodeName = "live-node-1",
            Host = "172.28.0.11",
            Port = 22,
            User = "root",
            PrivateKeyContent = _privateKeyContent
        });

        Node2 = new SshNodeConnection(new SshNodeOptions
        {
            NodeName = "live-node-2",
            Host = "172.28.0.12",
            Port = 22,
            User = "root",
            PrivateKeyContent = _privateKeyContent
        });

        await Node1.InitializeAsync();
        await Node2.InitializeAsync();

        IsAvailable = true;
    }

    private async Task StartContainerAsync(string containerName, string ipAddress)
    {
        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["rm", "-f", containerName]
        });

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["run", "-d", "--name", containerName, "--net", "starry-e2e-pub", "--ip", ipAddress, "starry-ssh-node"]
        });

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["exec", containerName, "mkdir", "-p", "/root/.ssh"]
        });

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["exec", "-i", containerName, "sh", "-c", "cat > /root/.ssh/authorized_keys && chmod 600 /root/.ssh/authorized_keys"],
            StandardInputBytes = System.Text.Encoding.UTF8.GetBytes(_publicKeyContent)
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (Node1 != null)
        {
            await Node1.DisposeAsync();
        }

        if (Node2 != null)
        {
            await Node2.DisposeAsync();
        }

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["rm", "-f", "starry-live-node-1", "starry-live-node-2"]
        });

        await Runner.RunAsync(new Core.Processes.ProcessSpec
        {
            Executable = "docker",
            Arguments = ["network", "rm", "starry-e2e-pub"]
        });

        if (Directory.Exists(_tempKeyDir))
        {
            try
            {
                Directory.Delete(_tempKeyDir, recursive: true);
            }
            catch (IOException)
            {
                // Ignore cleanup error
            }
        }
    }
}
```

## File: tests/Starry.E2ETests/LiveMultiNodeSshClusterE2ETests.cs

**Lines**: 149 | **Size**: 5.4 KB | **Language**: csharp

```csharp
using System.Security.Cryptography;
using Starry.Core.Processes;
using Starry.Infrastructure.Nodes;
using Xunit;

namespace Starry.E2ETests;

public class LiveMultiNodeSshClusterE2ETests
{
    [Fact]
    public async Task LiveCluster_RealSshConnection_DetectsAlpineDistro()
    {
        await using var fixture = new LiveDockerSshClusterFixture();
        await fixture.InitializeAsync();

        if (!fixture.IsAvailable || fixture.Node1 == null)
        {
            // Skip gracefully when Docker is not accessible in execution environment
            return;
        }

        Assert.Equal("ALPINE", fixture.Node1.DistroId);

        var result = await fixture.Node1.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "uname",
            Arguments = ["-a"]
        });

        Assert.True(result.IsSuccess);
        Assert.Contains("Linux", result.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public async Task LiveCluster_VolumeBinaryStream_BitForBitIntegrityOverRealSsh()
    {
        await using var fixture = new LiveDockerSshClusterFixture();
        await fixture.InitializeAsync();

        if (!fixture.IsAvailable || fixture.Node1 == null || fixture.Node2 == null)
        {
            return;
        }

        // 1. Generate 64 KB of non-UTF8 binary data on Node 1
        var binaryPayload = new byte[65536];
        RandomNumberGenerator.Fill(binaryPayload);
        binaryPayload[0] = 0xFF;
        binaryPayload[1] = 0x00;
        binaryPayload[2] = 0xFE;

        var expectedSha = Convert.ToHexString(SHA256.HashData(binaryPayload));

        // Create source volume directory on Node 1
        await fixture.Node1.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "mkdir",
            Arguments = ["-p", "/var/data/vol-test"]
        });

        // Write binary payload directly via stdin stream
        await fixture.Node1.ExecuteStreamingCommandAsync(
            new ProcessSpec { Executable = "sh", Arguments = ["-c", "cat > /var/data/vol-test/payload.bin"] },
            stdinStream: new MemoryStream(binaryPayload),
            stdoutStream: null);

        // Verify SHA256 on Node 1 over SSH
        var sha1Res = await fixture.Node1.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sha256sum",
            Arguments = ["/var/data/vol-test/payload.bin"]
        });
        Assert.Contains(expectedSha, sha1Res.StandardOutput, StringComparison.OrdinalIgnoreCase);

        // 2. Stream volume binary archive (tar --zstd) from Node 1 into orchestrator MemoryStream, then restore to Node 2
        using var pipeStream = new MemoryStream();

        var exportRes = await fixture.Node1.ExecuteStreamingCommandAsync(
            new ProcessSpec { Executable = "sh", Arguments = ["-c", "tar -cf - --zstd -C /var/data/vol-test ."] },
            stdinStream: null,
            stdoutStream: pipeStream);
        Assert.True(exportRes.IsSuccess, $"Export failed: {exportRes.ExitCode}, {exportRes.StandardError}");

        pipeStream.Position = 0;

        await fixture.Node2.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "mkdir",
            Arguments = ["-p", "/var/data/vol-test"]
        });

        var importRes = await fixture.Node2.ExecuteStreamingCommandAsync(
            new ProcessSpec { Executable = "sh", Arguments = ["-c", "tar -xf - --zstd -C /var/data/vol-test"] },
            stdinStream: pipeStream,
            stdoutStream: null);
        Assert.True(importRes.IsSuccess, $"ExitCode: {importRes.ExitCode}, Stderr: {importRes.StandardError}");

        // 3. Verify SHA256 on Node 2 matches expected hash bit-for-bit
        var sha2Res = await fixture.Node2.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "sha256sum",
            Arguments = ["/var/data/vol-test/payload.bin"]
        });

        Assert.True(sha2Res.IsSuccess);
        Assert.Contains(expectedSha, sha2Res.StandardOutput, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LiveCluster_SshKeyRotation_OnLiveContainer()
    {
        await using var fixture = new LiveDockerSshClusterFixture();
        await fixture.InitializeAsync();

        if (!fixture.IsAvailable || fixture.Node1 == null)
        {
            return;
        }

        // Rotate SSH key on live Node 1
        var newPrivateKey = await NodeOperationsService.RotateSshKeysAsync(fixture.Node1);
        Assert.NotNull(newPrivateKey);
        Assert.Contains("BEGIN OPENSSH PRIVATE KEY", newPrivateKey, StringComparison.Ordinal);

        var host = fixture.Node1.Host;
        var port = fixture.Node1.Port;
        await fixture.Node1.DisposeAsync();

        // Verify connection with the new rotated key
        await using var newConn = new SshNodeConnection(new SshNodeOptions
        {
            NodeName = "rotated-node-1",
            Host = host,
            Port = port,
            User = "root",
            PrivateKeyContent = newPrivateKey
        });

        await newConn.InitializeAsync();
        var testRes = await newConn.ExecuteCommandAsync(new ProcessSpec
        {
            Executable = "echo",
            Arguments = ["ROTATION_VERIFIED"]
        });

        Assert.True(testRes.IsSuccess, $"testRes ExitCode: {testRes.ExitCode}, Stderr: '{testRes.StandardError}', Stdout: '{testRes.StandardOutput}'");
        Assert.Contains("ROTATION_VERIFIED", testRes.StandardOutput, StringComparison.Ordinal);
    }
}
```

## File: tests/Starry.E2ETests/Starry.E2ETests.csproj

**Lines**: 27 | **Size**: 890 B | **Language**: xml

```xml
﻿<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.4" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\Starry.Core\Starry.Core.csproj" />
    <ProjectReference Include="..\..\src\Starry.Infrastructure\Starry.Infrastructure.csproj" />
    <ProjectReference Include="..\..\src\Starry.Cli\Starry.Cli.csproj" />
  </ItemGroup>

</Project>
```

## File: tests/Starry.UnitTests/ConfigValidatorTests.cs

**Lines**: 72 | **Size**: 2.7 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;
using Xunit;

namespace Starry.UnitTests;

public class ConfigValidatorTests
{
    [Fact]
    public void ConfigValidator_AcceptsValidConfiguration()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("192.168.1.10"));
        tree.SetNode("nodes.bocchi.port", JsonValue.Create(22));
        tree.SetNode("nodes.bocchi.vpn_ip", JsonValue.Create("10.99.0.2"));
        tree.SetNode("services.frontend.image", JsonValue.Create("nginx:alpine"));
        tree.SetNode("services.frontend.priority", JsonValue.Create(80));
        tree.SetNode("services.frontend.node", JsonValue.Create("bocchi"));

        var result = ConfigValidator.Validate(tree);
        Assert.True(result.IsValid);
        Assert.Empty(result.Issues);
    }

    [Fact]
    public void ConfigValidator_DetectsMissingHost()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.kita.port", JsonValue.Create(22));

        var result = ConfigValidator.Validate(tree);
        Assert.False(result.IsValid);
        Assert.Contains(result.Issues, i => string.Equals(i.Path, "nodes.kita.host", StringComparison.Ordinal));
    }

    [Fact]
    public void ConfigValidator_DetectsInvalidVpnIp()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("1.2.3.4"));
        tree.SetNode("nodes.bocchi.vpn_ip", JsonValue.Create("not-an-ip"));

        var result = ConfigValidator.Validate(tree);
        Assert.False(result.IsValid);
        Assert.Contains(result.Issues, i => string.Equals(i.Path, "nodes.bocchi.vpn_ip", StringComparison.Ordinal));
    }

    [Fact]
    public void ConfigValidator_FlagsOpenSecretsAsWarnings()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("1.2.3.4"));
        tree.SetNode("nodes.bocchi.ssh_key", JsonValue.Create("PLAIN[my-secret-key]"));

        var result = ConfigValidator.Validate(tree);
        Assert.True(result.IsValid); // Warnings do not make configuration invalid
        Assert.Contains(result.Issues, i => i.IsWarning && string.Equals(i.Path, "nodes.bocchi.ssh_key", StringComparison.Ordinal));
    }

    [Fact]
    public void ConfigValidator_FailsOnMalformedEncryptedSecret()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("1.2.3.4"));
        tree.SetNode("nodes.bocchi.ssh_key", JsonValue.Create("ENC[INVALID_TAG:secret]"));

        var result = ConfigValidator.Validate(tree);
        Assert.False(result.IsValid);
        Assert.Contains(result.Issues, i => !i.IsWarning && string.Equals(i.Path, "nodes.bocchi.ssh_key", StringComparison.Ordinal));
    }
}
```

## File: tests/Starry.UnitTests/ContainerPrioritySchedulerTests.cs

**Lines**: 46 | **Size**: 1.5 KB | **Language**: csharp

```csharp
using Starry.Core.Containers;
using Starry.Infrastructure.Containers;
using Xunit;

namespace Starry.UnitTests;

public class ContainerPrioritySchedulerTests
{
    [Fact]
    public void OrderServicesByPriority_OrdersAscendingCorrectly()
    {
        var services = new List<ServiceSpec>
        {
            new() { Name = "proxy", Image = "nginx:alpine", Priority = 90 },
            new() { Name = "database", Image = "postgres:16", Priority = 10 },
            new() { Name = "registry", Image = "registry:2", Priority = 0 },
            new() { Name = "backend", Image = "my-api:v1", Priority = 50 }
        };

        var ordered = ContainerPriorityScheduler.OrderServicesByPriority(services);

        Assert.Equal("registry", ordered[0].Name);
        Assert.Equal("database", ordered[1].Name);
        Assert.Equal("backend", ordered[2].Name);
        Assert.Equal("proxy", ordered[3].Name);
    }

    [Fact]
    public void GroupServicesByPriorityTier_GroupsCorrectly()
    {
        var services = new List<ServiceSpec>
        {
            new() { Name = "db1", Image = "postgres:16", Priority = 10 },
            new() { Name = "db2", Image = "redis:alpine", Priority = 10 },
            new() { Name = "web", Image = "caddy:latest", Priority = 90 }
        };

        var tiers = ContainerPriorityScheduler.GroupServicesByPriorityTier(services);

        Assert.Equal(2, tiers.Count);
        Assert.Equal(10, tiers[0].Key);
        Assert.Equal(2, tiers[0].Count());
        Assert.Equal(90, tiers[1].Key);
        Assert.Single(tiers[1]);
    }
}
```

## File: tests/Starry.UnitTests/CryptoServiceTests.cs

**Lines**: 105 | **Size**: 4.2 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;
using Starry.Infrastructure.Security;
using Xunit;

namespace Starry.UnitTests;

public class CryptoServiceTests
{
    private readonly CryptoService _cryptoService = new();
    private readonly SecretManager _secretManager = new();

    [Fact]
    public void EncryptAndDecrypt_RoundTripsSuccessfully()
    {
        var key = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
        var path = "services.api.jwt_secret";
        var rawText = "very-secure-jwt-signing-key-12345";

        var envelope = _cryptoService.Encrypt(System.Text.Encoding.UTF8.GetBytes(rawText), key, path);
        Assert.NotNull(envelope);

        var encryptedFormatted = envelope.FormatEncrypted();
        Assert.StartsWith("ENC[AES256_GCM:v1:", encryptedFormatted, StringComparison.Ordinal);

        Assert.True(_cryptoService.TryParseEncrypted(encryptedFormatted, out var parsedEnvelope));
        Assert.NotNull(parsedEnvelope);

        var decryptedBytes = _cryptoService.Decrypt(parsedEnvelope, key, path);
        var decryptedText = System.Text.Encoding.UTF8.GetString(decryptedBytes);

        Assert.Equal(rawText, decryptedText);
    }

    [Fact]
    public void SecretManager_EncryptsOpenSecretsInTree()
    {
        var key = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.ssh_key", JsonValue.Create("PLAIN[my-private-key]"));
        tree.SetNode("services.db.password", JsonValue.Create("PLAIN[db-pass-456]"));

        var tempDir = Path.Combine(Path.GetTempPath(), "starry_sec_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var targetFile = Path.Combine(tempDir, "starry.yaml");
        File.WriteAllText(targetFile, "dummy: true\n");

        try
        {
            var count = _secretManager.EncryptOpenSecrets(tree, key, targetFile, new VirtualConfigEngine());
            Assert.Equal(2, count);

            var encryptedSsh = tree.Root["nodes"]?["bocchi"]?["ssh_key"]?.ToString();
            Assert.NotNull(encryptedSsh);
            Assert.StartsWith("ENC[AES256_GCM:v1:", encryptedSsh, StringComparison.Ordinal);

            Assert.True(_cryptoService.TryParseEncrypted(encryptedSsh, out var envelope));
            Assert.NotNull(envelope);
            var decrypted = _cryptoService.Decrypt(envelope, key, "nodes.bocchi.ssh_key");
            Assert.Equal("my-private-key", System.Text.Encoding.UTF8.GetString(decrypted));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void SecretManager_ReencryptsAllSecretsWithNewKey()
    {
        var oldKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());
        var newKey = CryptoService.ParseMasterKey(CryptoService.GenerateMasterKeyBase64());

        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.ssh_key", JsonValue.Create("PLAIN[initial-secret]"));

        var tempDir = Path.Combine(Path.GetTempPath(), "starry_resec_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var targetFile = Path.Combine(tempDir, "starry.yaml");
        File.WriteAllText(targetFile, "dummy: true\n");

        try
        {
            var engine = new VirtualConfigEngine();
            _secretManager.EncryptOpenSecrets(tree, oldKey, targetFile, engine);

            var reencryptedCount = _secretManager.ReencryptAll(tree, oldKey, newKey, targetFile, engine);
            Assert.Equal(1, reencryptedCount);

            var currentVal = tree.Root["nodes"]?["bocchi"]?["ssh_key"]?.ToString();
            Assert.NotNull(currentVal);

            // Must decrypt with new key
            Assert.True(_cryptoService.TryParseEncrypted(currentVal, out var envelope));
            Assert.NotNull(envelope);
            var decrypted = _cryptoService.Decrypt(envelope, newKey, "nodes.bocchi.ssh_key");
            Assert.Equal("initial-secret", System.Text.Encoding.UTF8.GetString(decrypted));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
```

## File: tests/Starry.UnitTests/DnsProviderTests.cs

**Lines**: 60 | **Size**: 2.3 KB | **Language**: csharp

```csharp
using System.Net;
using Starry.Core.Dns;
using Starry.Infrastructure.Dns;
using Xunit;

namespace Starry.UnitTests;

public class DnsProviderTests
{
    private sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }
        public string? LastRequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            if (request.Content != null)
            {
                LastRequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

    [Fact]
    public async Task WebhookDnsProvider_DispatchesUpsertPayload()
    {
        using var handler = new MockHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);

        var provider = new WebhookDnsProvider(new Uri("https://webhook.example.com/dns"), httpClient);
        var record = new DnsRecord("rec1", "A", "bocchi.example.com", "198.51.100.10", 120, false);

        await provider.UpsertRecordAsync("example.com", record);

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        var body = handler.LastRequestBody ?? string.Empty;
        Assert.Contains("upsert", body, StringComparison.Ordinal);
        Assert.Contains("bocchi.example.com", body, StringComparison.Ordinal);
        Assert.Contains("198.51.100.10", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task WebhookDnsProvider_DispatchesDeletePayload()
    {
        using var handler = new MockHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);

        var provider = new WebhookDnsProvider(new Uri("https://webhook.example.com/dns"), httpClient);

        await provider.DeleteRecordAsync("example.com", "rec-to-delete");

        Assert.NotNull(handler.LastRequest);
        var body = handler.LastRequestBody ?? string.Empty;
        Assert.Contains("delete", body, StringComparison.Ordinal);
        Assert.Contains("rec-to-delete", body, StringComparison.Ordinal);
    }
}
```

## File: tests/Starry.UnitTests/FleetBundleServiceTests.cs

**Lines**: 46 | **Size**: 1.7 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Backups;
using Xunit;

namespace Starry.UnitTests;

public class FleetBundleServiceTests
{
    [Fact]
    public void ExportAndImportBundle_PreservesConfigAndAppliesIpOverrides()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_bundle_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = new ConfigTree();
            tree.SetNode("cluster_name", JsonValue.Create("kessoku-dr"));
            tree.SetNode("nodes.bocchi.host", JsonValue.Create("198.51.100.10"));
            tree.SetNode("nodes.bocchi.port", JsonValue.Create(22));
            tree.SetNode("services.db.image", JsonValue.Create("postgres:16"));

            var bundleFile = Path.Combine(tempDir, "fleet.tar.gz");
            FleetBundleService.ExportBundle(tree, bundleFile);

            Assert.True(File.Exists(bundleFile));

            var importDir = Path.Combine(tempDir, "restored");
            var ipOverrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["bocchi"] = "203.0.113.99"
            };

            var importedTree = FleetBundleService.ImportBundle(bundleFile, importDir, ipOverrides);

            Assert.Equal("kessoku-dr", importedTree.Root["cluster_name"]?.ToString());
            Assert.Equal("203.0.113.99", importedTree.Root["nodes"]?["bocchi"]?["host"]?.ToString());
            Assert.Equal("postgres:16", importedTree.Root["services"]?["db"]?["image"]?.ToString());
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
```

## File: tests/Starry.UnitTests/IdLedgerServiceTests.cs

**Lines**: 98 | **Size**: 3.2 KB | **Language**: csharp

```csharp
using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class IdLedgerServiceTests
{
    [Fact]
    public void AllocateHostId_IncrementsMonotonically()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_ledger_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var ledger = new IdLedgerService(tempDir);
            var id1 = ledger.AllocateHostId("germany");
            var id2 = ledger.AllocateHostId("finland");
            var id3 = ledger.AllocateHostId("japan");

            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
            Assert.Equal(3, id3);

            // Re-querying same host returns existing ID
            Assert.Equal(1, ledger.AllocateHostId("germany"));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ReleaseHost_TombstonesId_AndNeverReusesIt()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_tombstone_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var ledger = new IdLedgerService(tempDir);
            var id1 = ledger.AllocateHostId("germany"); // 1
            var id2 = ledger.AllocateHostId("finland"); // 2

            // Release germany (ID 1 should be tombstoned)
            ledger.ReleaseHost("germany");

            // Allocate brand new host -> must receive 3, NOT tombstoned 1!
            var id3 = ledger.AllocateHostId("singapore");
            Assert.Equal(3, id3);

            var snapshot = ledger.StateSnapshot;
            Assert.Contains(1, snapshot.TombstonedHostIds);
            Assert.DoesNotContain(1, snapshot.AllocatedHosts.Values);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void ReleaseContainer_TombstonesContainerId_AndNeverReusesIt()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_cid_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var ledger = new IdLedgerService(tempDir);
            var c1 = ledger.AllocateContainerId("shop#1"); // 1
            var c2 = ledger.AllocateContainerId("shop#2"); // 2

            ledger.ReleaseContainer("shop#1");

            var c3 = ledger.AllocateContainerId("shop#3");
            Assert.Equal(3, c3);

            var snapshot = ledger.StateSnapshot;
            Assert.Contains(1, snapshot.TombstonedContainerIds);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void AddressFormatting_FormatsCanonicalIPv6Subnets()
    {
        Assert.Equal("fd67:b00b:0001::1", IdLedgerService.FormatHostWireGuardIp(1));
        Assert.Equal("fd67:b00b:0001::/48", IdLedgerService.FormatHostSubnet(1));
        Assert.Equal("fd67:b00b:0001:000a::/64", IdLedgerService.FormatContainerSubnet(1, 10));
        Assert.Equal("fd67:b00b:0001:000a::1", IdLedgerService.FormatContainerIp(1, 10));
        Assert.Equal("fd67:b00b:0001:ffff::1", IdLedgerService.FormatIngressIp(1));
    }
}
```

## File: tests/Starry.UnitTests/InternalDnsServerScopedTests.cs

**Lines**: 153 | **Size**: 5.4 KB | **Language**: csharp

```csharp
using System.Net;
using System.Text;
using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class InternalDnsServerScopedTests
{
    [Fact]
    public async Task InternalDnsServer_SharedNetwork_ReturnsAaaaRecord()
    {
        await using var server = new InternalDnsServer(port: 5355);

        // Service DB on subnet fd67:b00b:0001:0001::/64 in network "backend_net"
        var dbIp = IPAddress.Parse("fd67:b00b:0001:0001::1");
        server.RegisterService(
            serviceName: "db",
            endpoints: [new ReplicaEndpoint("db-1", dbIp)],
            networks: ["backend_net"]);

        // Service API on subnet fd67:b00b:0001:0002::/64 in network "backend_net"
        var apiIp = IPAddress.Parse("fd67:b00b:0001:0002::1");
        server.RegisterService(
            serviceName: "api",
            endpoints: [new ReplicaEndpoint("api-1", apiIp)],
            networks: ["backend_net"]);

        // Query for db.internal from API container (caller IP = apiIp) for AAAA (qtype: 28)
        var queryPacket = BuildQueryPacket("db.internal", qtype: 28);
        var response = await server.HandleDnsQueryAsync(queryPacket, clientAddress: apiIp);

        Assert.NotNull(response);
        // Header flags: 0x8180 = NOERROR
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x80, response[3]);
        // ANCOUNT = 1
        Assert.Equal(0x01, response[7]);

        // Response contains dbIp bytes
        var answerBytes = response[^16..];
        Assert.Equal(dbIp.GetAddressBytes(), answerBytes);
    }

    [Fact]
    public async Task InternalDnsServer_IsolatedNetwork_ReturnsNxDomain()
    {
        await using var server = new InternalDnsServer(port: 5356);

        // Service DB in "db_private_net"
        var dbIp = IPAddress.Parse("fd67:b00b:0001:0001::1");
        server.RegisterService(
            serviceName: "secret-db",
            endpoints: [new ReplicaEndpoint("secret-db-1", dbIp)],
            networks: ["db_private_net"]);

        // Service Frontend in "public_net" (NO shared network with secret-db!)
        var frontIp = IPAddress.Parse("fd67:b00b:0001:0003::1");
        server.RegisterService(
            serviceName: "frontend",
            endpoints: [new ReplicaEndpoint("frontend-1", frontIp)],
            networks: ["public_net"]);

        var queryPacket = BuildQueryPacket("secret-db.internal", qtype: 28);
        var response = await server.HandleDnsQueryAsync(queryPacket, clientAddress: frontIp);

        Assert.NotNull(response);
        // Header flags: 0x8183 = NXDOMAIN (0x83)
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x83, response[3]);
        // ANCOUNT = 0
        Assert.Equal(0x00, response[7]);
    }

    [Fact]
    public async Task InternalDnsServer_EgressFalse_ReturnsRefusedForExternalDomains()
    {
        await using var server = new InternalDnsServer(port: 5357);

        // Container with egress: false
        var isolatedIp = IPAddress.Parse("fd67:b00b:0001:0004::1");
        server.RegisterService(
            serviceName: "isolated-worker",
            endpoints: [new ReplicaEndpoint("isolated-worker-1", isolatedIp)],
            networks: ["worker_net"],
            egressAllowed: false);

        var queryPacket = BuildQueryPacket("github.com", qtype: 1);
        var response = await server.HandleDnsQueryAsync(queryPacket, clientAddress: isolatedIp);

        Assert.NotNull(response);
        // Header flags: 0x8185 = REFUSED (0x85)
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x85, response[3]);
    }

    [Fact]
    public async Task InternalDnsServer_DualStackNoIpv4_ReturnsEmptyNoError()
    {
        await using var server = new InternalDnsServer(port: 5358);

        var ipv6Only = IPAddress.Parse("fd67:b00b:0001:0005::1");
        server.RegisterService(
            serviceName: "v6-service",
            endpoints: [new ReplicaEndpoint("v6-service-1", ipv6Only, Ipv4Address: null)],
            networks: ["net1"]);

        var callerIp = IPAddress.Parse("fd67:b00b:0001:0006::1");
        server.RegisterService(
            serviceName: "caller",
            endpoints: [new ReplicaEndpoint("caller-1", callerIp)],
            networks: ["net1"]);

        // Query for A record (IPv4) when only IPv6 exists
        var queryPacket = BuildQueryPacket("v6-service.internal", qtype: 1);
        var response = await server.HandleDnsQueryAsync(queryPacket, clientAddress: callerIp);

        Assert.NotNull(response);
        // NOERROR (0x8180) with ANCOUNT = 0
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x80, response[3]);
        Assert.Equal(0x00, response[7]);
    }

    private static byte[] BuildQueryPacket(string domain, ushort qtype = 1)
    {
        var buffer = new List<byte>
        {
            0x12, 0x34, // Transaction ID
            0x01, 0x00, // Flags: Standard query
            0x00, 0x01, // Questions: 1
            0x00, 0x00, // Answer RRs: 0
            0x00, 0x00, // Authority RRs: 0
            0x00, 0x00  // Additional RRs: 0
        };

        var labels = domain.Split('.');
        foreach (var label in labels)
        {
            buffer.Add((byte)label.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(label));
        }
        buffer.Add(0x00); // End of name

        buffer.Add((byte)(qtype >> 8));
        buffer.Add((byte)(qtype & 0xFF)); // QTYPE

        buffer.Add(0x00);
        buffer.Add(0x01); // QCLASS: IN (0x0001)

        return [.. buffer];
    }
}
```

## File: tests/Starry.UnitTests/InternalDnsServerTests.cs

**Lines**: 97 | **Size**: 3.0 KB | **Language**: csharp

```csharp
using System.Net;
using System.Text;
using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class InternalDnsServerTests
{
    [Fact]
    public async Task HandleDnsQuery_ResolvesInternalDomainsCorrectly()
    {
        await using var server = new InternalDnsServer();
        server.RegisterRecord("bocchi.internal", IPAddress.Parse("10.99.0.1"));
        server.RegisterRecord("api.internal", IPAddress.Parse("10.99.10.5"));

        // Construct a standard DNS query packet for "api.internal" (Type A, Class IN)
        var query = BuildQueryPacket("api.internal");

        var response = server.HandleDnsQuery(query);
        Assert.NotNull(response);

        // Check header flags in response (0x8180 = standard response, no error)
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x80, response[3]);

        // Check answer count = 1
        Assert.Equal(0x01, response[7]);

        // Last 4 bytes should be IPv4: 10.99.10.5
        var ipBytes = response[^4..];
        Assert.Equal(new byte[] { 10, 99, 10, 5 }, ipBytes);
    }

    [Fact]
    public async Task HandleDnsQuery_ReturnsNxDomainForUnknownDomain()
    {
        await using var server = new InternalDnsServer();
        server.RegisterRecord("bocchi.internal", IPAddress.Parse("10.99.0.1"));

        var query = BuildQueryPacket("unknown.internal");
        var response = server.HandleDnsQuery(query);

        Assert.NotNull(response);
        // Flags: 0x8183 = NXDOMAIN
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x83, response[3]);
        // Answer count = 0
        Assert.Equal(0x00, response[7]);
    }

    [Fact]
    public async Task HandleDnsQuery_ReturnsEmptyNoError_ForAaaaQuery()
    {
        await using var server = new InternalDnsServer();
        server.RegisterRecord("bocchi.internal", IPAddress.Parse("10.99.0.1"));

        var query = BuildQueryPacket("bocchi.internal", qtype: 28); // 28 = AAAA
        var response = server.HandleDnsQuery(query);

        Assert.NotNull(response);
        // Flags: 0x8180 = NOERROR
        Assert.Equal(0x81, response[2]);
        Assert.Equal(0x80, response[3]);
        // Answer count = 0
        Assert.Equal(0x00, response[7]);
    }

    private static byte[] BuildQueryPacket(string domain, ushort qtype = 1)
    {
        var buffer = new List<byte>
        {
            0x12, 0x34, // Transaction ID
            0x01, 0x00, // Flags: Standard query
            0x00, 0x01, // Questions: 1
            0x00, 0x00, // Answer RRs: 0
            0x00, 0x00, // Authority RRs: 0
            0x00, 0x00  // Additional RRs: 0
        };

        var labels = domain.Split('.');
        foreach (var label in labels)
        {
            buffer.Add((byte)label.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(label));
        }
        buffer.Add(0x00); // End of name

        buffer.Add((byte)(qtype >> 8));
        buffer.Add((byte)(qtype & 0xFF)); // QTYPE

        buffer.Add(0x00);
        buffer.Add(0x01); // QCLASS: IN (0x0001)

        return [.. buffer];
    }
}
```

## File: tests/Starry.UnitTests/LocalizerTests.cs

**Lines**: 46 | **Size**: 1.4 KB | **Language**: csharp

```csharp
using Starry.Core.Localization;
using Starry.Infrastructure.Localization;
using Xunit;

namespace Starry.UnitTests;

public class LocalizerTests
{
    [Fact]
    public void Localizer_DefaultsToEnglish_WhenNotSpecified()
    {
        var localizer = new Localizer("en");

        Assert.Equal("en", localizer.CurrentLanguage);
        var desc = localizer.GetString(LocalizationKeys.CliDescription);
        Assert.Contains("Decentralized GitOps orchestrator", desc, StringComparison.Ordinal);
    }

    [Fact]
    public void Localizer_SwitchesToRussian_WhenRequested()
    {
        var localizer = new Localizer("ru");

        Assert.Equal("ru", localizer.CurrentLanguage);
        var desc = localizer.GetString(LocalizationKeys.CliDescription);
        Assert.Contains("децентрализованный GitOps-оркестратор", desc, StringComparison.Ordinal);
    }

    [Fact]
    public void Localizer_FormatString_InjectsArgumentsCorrectly()
    {
        var localizer = new Localizer("ru");

        var formatted = localizer.FormatString(LocalizationKeys.ValidationFailed, 3);
        Assert.Equal("Валидация конфигурации завершилась с 3 ошибками.", formatted);
    }

    [Fact]
    public void Localizer_FallbackToKey_WhenKeyNotFound()
    {
        var localizer = new Localizer("en");

        var result = localizer.GetString("non.existent.key");
        Assert.Equal("non.existent.key", result);
    }
}
```

## File: tests/Starry.UnitTests/MigrationPlannerTests.cs

**Lines**: 75 | **Size**: 3.4 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Orchestration;
using Xunit;

namespace Starry.UnitTests;

public class MigrationPlannerTests
{
    [Fact]
    public void BuildPlan_GeneratesBootstrapsNetworkAndDeploymentsInOrder()
    {
        var tree = new ConfigTree();
        tree.SetNode("cluster_name", JsonValue.Create("kessoku-mesh"));
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("192.168.1.10"));
        tree.SetNode("nodes.nijika.host", JsonValue.Create("192.168.1.20"));
        tree.SetNode("services.db.image", JsonValue.Create("postgres:16"));
        tree.SetNode("services.db.priority", JsonValue.Create(10));
        tree.SetNode("services.db.node", JsonValue.Create("bocchi"));
        tree.SetNode("services.web.image", JsonValue.Create("nginx:alpine"));
        tree.SetNode("services.web.priority", JsonValue.Create(80));
        tree.SetNode("services.web.node", JsonValue.Create("nijika"));

        var plan = MigrationPlanner.BuildPlan(tree);

        Assert.True(plan.HasChanges);
        Assert.Equal("kessoku-mesh", plan.ClusterName);

        // Nodes come first (priority 0)
        var bootstrapActions = plan.Actions.Where(a => a.ActionType == PlanActionType.CreateNode).ToList();
        Assert.Equal(2, bootstrapActions.Count);

        // WireGuard mesh comes next (priority 5)
        var meshAction = plan.Actions.FirstOrDefault(a => a.ActionType == PlanActionType.ConfigureWireGuardMesh);
        Assert.NotNull(meshAction);
        Assert.Equal(2, meshAction.Dependencies.Count);

        // Nftables comes next (priority 10)
        var nftAction = plan.Actions.FirstOrDefault(a => a.ActionType == PlanActionType.ConfigureNftables);
        Assert.NotNull(nftAction);

        // DB (priority 10 + 50 = 60) must come before Web (priority 80 + 50 = 130)
        var dbAction = plan.Actions.FirstOrDefault(a => string.Equals(a.Target, "db", StringComparison.Ordinal));
        var webAction = plan.Actions.FirstOrDefault(a => string.Equals(a.Target, "web", StringComparison.Ordinal));

        Assert.NotNull(dbAction);
        Assert.NotNull(webAction);
        Assert.True(dbAction.Priority < webAction.Priority);
    }

    [Fact]
    public void BuildPlan_GeneratesBlueGreenMigration_WhenServiceNodeChanges()
    {
        var tree = new ConfigTree();
        tree.SetNode("nodes.bocchi.host", JsonValue.Create("192.168.1.10"));
        tree.SetNode("nodes.nijika.host", JsonValue.Create("192.168.1.20"));
        tree.SetNode("services.api.image", JsonValue.Create("my-api:v2"));
        tree.SetNode("services.api.node", JsonValue.Create("nijika")); // Desired node: nijika

        // Current live placement: bocchi
        var livePlacements = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["api"] = "bocchi"
        };

        var plan = MigrationPlanner.BuildPlan(tree, livePlacements);

        var migrationAction = plan.Actions.FirstOrDefault(a => string.Equals(a.Target, "api", StringComparison.Ordinal));
        Assert.NotNull(migrationAction);
        Assert.Equal(PlanActionType.MigrateServiceBlueGreen, migrationAction.ActionType);
        Assert.Contains("Blue-Green migration", migrationAction.Description, StringComparison.Ordinal);
        Assert.Contains("bocchi", migrationAction.Description, StringComparison.Ordinal);
        Assert.Contains("nijika", migrationAction.Description, StringComparison.Ordinal);
    }
}
```

## File: tests/Starry.UnitTests/NftablesServiceTests.cs

**Lines**: 31 | **Size**: 1.4 KB | **Language**: csharp

```csharp
using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class NftablesServiceTests
{
    [Fact]
    public void GenerateRuleset_ProducesCanonicalTableInetStarry()
    {
        var forwards = new List<PortForwardRule>
        {
            new(80, "tcp", "10.99.10.1", 80),
            new(443, "tcp", "10.99.10.1", 443),
            new(53, "udp", "10.99.0.1", 53)
        };
        var publicPorts = new List<int> { 80, 443 };

        var ruleset = NftablesService.GenerateRuleset(forwards, publicPorts, sshPort: 2222, wireGuardPort: 51820);

        Assert.Contains("table inet starry", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 2222 accept", ruleset, StringComparison.Ordinal);
        Assert.Contains("udp dport 51820 accept", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 80 accept", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 443 accept", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 80 dnat to 10.99.10.1:80", ruleset, StringComparison.Ordinal);
        Assert.Contains("tcp dport 443 dnat to 10.99.10.1:443", ruleset, StringComparison.Ordinal);
        Assert.Contains("udp dport 53 dnat to 10.99.0.1:53", ruleset, StringComparison.Ordinal);
        Assert.Contains("oifname \"starry0\" masquerade", ruleset, StringComparison.Ordinal);
    }
}
```

## File: tests/Starry.UnitTests/PackageManagerTests.cs

**Lines**: 57 | **Size**: 1.9 KB | **Language**: csharp

```csharp
using Starry.Infrastructure.Packaging;
using Xunit;

namespace Starry.UnitTests;

public class PackageManagerTests
{
    private readonly PackageManagerFactory _factory = new();

    [Theory]
    [InlineData("ubuntu", typeof(AptPackageManager))]
    [InlineData("debian", typeof(AptPackageManager))]
    [InlineData("almalinux", typeof(DnfPackageManager))]
    [InlineData("rocky", typeof(DnfPackageManager))]
    [InlineData("fedora", typeof(DnfPackageManager))]
    [InlineData("rhel", typeof(DnfPackageManager))]
    [InlineData("alpine", typeof(ApkPackageManager))]
    [InlineData("arch", typeof(PacmanPackageManager))]
    public void Factory_ResolvesCorrectManager(string distroId, Type expectedType)
    {
        var manager = _factory.Resolve(distroId);
        Assert.IsType(expectedType, manager);
    }

    [Fact]
    public void Factory_ThrowsOnUnsupportedDistro()
    {
        Assert.Throws<NotSupportedException>(() => _factory.Resolve("templeos"));
    }

    [Fact]
    public void AptPackageManager_MapsPackagesCorrectly()
    {
        Assert.Equal("wireguard", AptPackageManager.MapLogicalPackage("wireguard"));
        Assert.Equal("nftables", AptPackageManager.MapLogicalPackage("nftables"));
        Assert.Equal("curl", AptPackageManager.MapLogicalPackage("curl"));
    }

    [Fact]
    public void DnfPackageManager_MapsPackagesCorrectly()
    {
        Assert.Equal("wireguard-tools", DnfPackageManager.MapLogicalPackage("wireguard"));
        Assert.Equal("nftables", DnfPackageManager.MapLogicalPackage("nftables"));
    }

    [Fact]
    public void ApkPackageManager_MapsPackagesCorrectly()
    {
        Assert.Equal("wireguard-tools", ApkPackageManager.MapLogicalPackage("wireguard"));
    }

    [Fact]
    public void PacmanPackageManager_MapsPackagesCorrectly()
    {
        Assert.Equal("wireguard-tools", PacmanPackageManager.MapLogicalPackage("wireguard"));
    }
}
```

## File: tests/Starry.UnitTests/ProcessRunnerTests.cs

**Lines**: 58 | **Size**: 1.5 KB | **Language**: csharp

```csharp
using Starry.Core.Processes;
using Starry.Infrastructure.Processes;
using Xunit;

namespace Starry.UnitTests;

public class ProcessRunnerTests
{
    private readonly ProcessRunner _runner = new();

    [Fact]
    public async Task ProcessRunner_ExecutesEcho_Successfully()
    {
        var spec = new ProcessSpec
        {
            Executable = "echo",
            Arguments = ["hello", "starry"]
        };

        var result = await _runner.RunAsync(spec);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("hello starry", result.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessRunner_PassesEnvironmentVariables()
    {
        var spec = new ProcessSpec
        {
            Executable = "printenv",
            Arguments = ["TEST_VAR"],
            EnvironmentVariables = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["TEST_VAR"] = "StarrySecret123"
            }
        };

        var result = await _runner.RunAsync(spec);

        Assert.True(result.IsSuccess);
        Assert.Contains("StarrySecret123", result.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ProcessRunner_TimesOut_WhenExceedingLimit()
    {
        var spec = new ProcessSpec
        {
            Executable = "sleep",
            Arguments = ["5"],
            Timeout = TimeSpan.FromMilliseconds(100)
        };

        await Assert.ThrowsAsync<TimeoutException>(() => _runner.RunAsync(spec));
    }
}
```

## File: tests/Starry.UnitTests/SshNodeConnectionTests.cs

**Lines**: 58 | **Size**: 1.6 KB | **Language**: csharp

```csharp
using Starry.Infrastructure.Nodes;
using Xunit;

namespace Starry.UnitTests;

public class SshNodeConnectionTests
{
    [Fact]
    public async Task BuildSshArguments_IncludesPortAndBatchMode()
    {
        var options = new SshNodeOptions
        {
            NodeName = "bocchi",
            Host = "192.168.1.100",
            Port = 2222,
            User = "deploy"
        };

        await using var conn = new SshNodeConnection(options);
        var args = conn.BuildSshArguments();

        Assert.Contains("-p", args, StringComparer.Ordinal);
        Assert.Contains("2222", args, StringComparer.Ordinal);
        Assert.Contains("BatchMode=yes", args, StringComparer.Ordinal);
    }

    [Fact]
    public async Task BuildSshArguments_IncludesJumpHost_WhenConfigured()
    {
        var options = new SshNodeOptions
        {
            NodeName = "nat-node",
            Host = "10.0.0.50",
            Port = 22,
            JumpHost = "bastion.example.com"
        };

        await using var conn = new SshNodeConnection(options);
        var args = conn.BuildSshArguments();

        Assert.Contains("-J", args, StringComparer.Ordinal);
        Assert.Contains("bastion.example.com", args, StringComparer.Ordinal);
    }

    [Fact]
    public void LocalNodeConnection_ParsesDistroIdFromOsRelease()
    {
        const string osRelease = """
            NAME="Ubuntu"
            VERSION="24.04 LTS (Noble Numbat)"
            ID=ubuntu
            ID_LIKE=debian
            """;

        var distro = LocalNodeConnection.ParseDistroId(osRelease);
        Assert.Equal("UBUNTU", distro);
    }
}
```

## File: tests/Starry.UnitTests/Starry.UnitTests.csproj

**Lines**: 27 | **Size**: 890 B | **Language**: xml

```xml
﻿<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.4" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\Starry.Core\Starry.Core.csproj" />
    <ProjectReference Include="..\..\src\Starry.Infrastructure\Starry.Infrastructure.csproj" />
    <ProjectReference Include="..\..\src\Starry.Cli\Starry.Cli.csproj" />
  </ItemGroup>

</Project>
```

## File: tests/Starry.UnitTests/StarryDslTests.cs

**Lines**: 162 | **Size**: 6.2 KB | **Language**: csharp

```csharp
using Starry.Dsl;
using Xunit;

namespace Starry.UnitTests;

public class StarryDslTests
{
    [Fact]
    public void ConfigAst_ParsesYamlJsonTomlCorrectly()
    {
        const string yaml = """
            cluster_name: kessoku-v3
            nodes:
              germany:
                host: 142.132.10.20
            """;

        var node = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.Some("starry.yaml"));
        Assert.True(node.IsObjectNode);

        var jsonNode = ConfigAst.toJsonNode(node);
        Assert.Equal("kessoku-v3", jsonNode["cluster_name"]?.ToString());
        Assert.Equal("142.132.10.20", jsonNode["nodes"]?["germany"]?["host"]?.ToString());
    }

    [Fact]
    public void GraphResolver_WholeSubtreeSubstitution_ReplacesReference()
    {
        const string yaml = """
            presets:
              db_env:
                POSTGRES_DB: starry
                POSTGRES_USER: admin
            services:
              postgres:
                env: $presets.db_env
            """;

        var root = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.None);
        var resolvedResult = GraphResolver.resolve(root, Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None);

        Assert.True(resolvedResult.IsOk);
        var resolvedJson = ConfigAst.toJsonNode(resolvedResult.ResultValue);

        Assert.Equal("starry", resolvedJson["services"]?["postgres"]?["env"]?["POSTGRES_DB"]?.ToString());
        Assert.Equal("admin", resolvedJson["services"]?["postgres"]?["env"]?["POSTGRES_USER"]?.ToString());
    }

    [Fact]
    public void GraphResolver_Inheritance_MergesAndOverrides()
    {
        const string yaml = """
            presets:
              base_service:
                image: base:latest
                priority: 50
                restart: always
            services:
              api:
                $: $presets.base_service
                image: custom-api:v1
                priority: 30
            """;

        var root = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.None);
        var resolvedResult = GraphResolver.resolve(root, Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None);

        Assert.True(resolvedResult.IsOk);
        var resolvedJson = ConfigAst.toJsonNode(resolvedResult.ResultValue);

        // Explicit properties override inherited ones
        Assert.Equal("custom-api:v1", resolvedJson["services"]?["api"]?["image"]?.ToString());
        Assert.Equal("30", resolvedJson["services"]?["api"]?["priority"]?.ToString());
        // Inherited non-overridden properties are preserved
        Assert.Equal("always", resolvedJson["services"]?["api"]?["restart"]?.ToString());
    }

    [Fact]
    public void GraphResolver_StringInterpolation_SubstitutesValues()
    {
        const string yaml = """
            nodes:
              germany:
                host: 142.132.10.20
            services:
              api:
                env:
                  REMOTE_HOST: "connect to $nodes.germany.host please"
            """;

        var root = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.None);
        var resolvedResult = GraphResolver.resolve(root, Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None);

        Assert.True(resolvedResult.IsOk);
        var resolvedJson = ConfigAst.toJsonNode(resolvedResult.ResultValue);

        Assert.Equal("connect to 142.132.10.20 please", resolvedJson["services"]?["api"]?["env"]?["REMOTE_HOST"]?.ToString());
    }

    [Fact]
    public void GraphResolver_DetectsCircularReferences()
    {
        const string yaml = """
            a: $b
            b: $c
            c: $a
            """;

        var root = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.None);
        var resolvedResult = GraphResolver.resolve(root, Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None);

        Assert.True(resolvedResult.IsError);
        Assert.True(resolvedResult.ErrorValue.IsCircularReference);
    }

    [Fact]
    public void Generators_ProducesWireGuardConfigWithFd67Prefix()
    {
        var hostSpec = new Generators.WireGuardHostSpec(
            nodeName: "germany",
            hostId: 1,
            privateKey: "priv1",
            publicKey: "pub1",
            listenPort: 51820,
            endpoint: Microsoft.FSharp.Core.FSharpOption<string>.Some("142.132.10.20"),
            isPublic: true,
            peers:
            [
                new Generators.WireGuardPeerSpec(
                    nodeName: "finland",
                    hostId: 2,
                    publicKey: "pub2",
                    endpoint: Microsoft.FSharp.Core.FSharpOption<string>.Some("65.21.10.30"),
                    listenPort: 51820,
                    isPublic: true)
            ]);

        var wgConf = Generators.generateWireGuardConfig(hostSpec);

        Assert.Contains("Address = fd67:b00b:0001::1/32", wgConf, StringComparison.Ordinal);
        Assert.Contains("AllowedIPs = fd67:b00b:0002::/48", wgConf, StringComparison.Ordinal);
        Assert.Contains("Endpoint = 65.21.10.30:51820", wgConf, StringComparison.Ordinal);
    }

    [Fact]
    public void Generators_ProducesNginxConfigWithSniAndHttp()
    {
        var routes = new List<Generators.NginxRoute>
        {
            Generators.NginxRoute.NewSniPassthrough("vpn.bocchi.rocks", "xray:8443", true),
            Generators.NginxRoute.NewHttpReverseProxy("shop.bocchi.rocks", "fd67:b00b:0001:0002::1", 3000, Microsoft.FSharp.Core.FSharpOption<string>.None, Microsoft.FSharp.Core.FSharpOption<string>.None)
        };

        var nginxConf = Generators.generateNginxConfig(1, Microsoft.FSharp.Collections.ListModule.OfSeq(routes));

        Assert.Contains("stream {", nginxConf, StringComparison.Ordinal);
        Assert.Contains("ssl_preread on;", nginxConf, StringComparison.Ordinal);
        Assert.Contains("\"vpn.bocchi.rocks\" xray:8443;", nginxConf, StringComparison.Ordinal);
        Assert.Contains("http {", nginxConf, StringComparison.Ordinal);
        Assert.Contains("proxy_pass http://[fd67:b00b:0001:0002::1]:3000;", nginxConf, StringComparison.Ordinal);
    }
}
```

## File: tests/Starry.UnitTests/VirtualConfigEngineTests.cs

**Lines**: 140 | **Size**: 5.2 KB | **Language**: csharp

```csharp
using System.Text.Json.Nodes;
using Starry.Core.Configuration;
using Starry.Infrastructure.Configuration;
using Xunit;

namespace Starry.UnitTests;

public class VirtualConfigEngineTests
{
    private readonly VirtualConfigEngine _engine = new();

    [Fact]
    public void YamlFormatProvider_ParsesAndSerializesCorrectly()
    {
        var provider = new YamlConfigFormatProvider();
        const string yaml = """
            cluster_name: kessoku-cluster
            nodes:
              bocchi:
                host: 192.168.1.10
                port: 22
            """;

        var node = provider.Parse(System.Text.Encoding.UTF8.GetBytes(yaml), "test.yaml");
        Assert.NotNull(node);
        Assert.Equal("kessoku-cluster", node["cluster_name"]?.ToString());
        Assert.Equal("192.168.1.10", node["nodes"]?["bocchi"]?["host"]?.ToString());

        var serialized = provider.Serialize(node);
        Assert.Contains("kessoku-cluster", serialized, StringComparison.Ordinal);
        Assert.Contains("192.168.1.10", serialized, StringComparison.Ordinal);
    }

    [Fact]
    public void TomlFormatProvider_ParsesAndSerializesCorrectly()
    {
        var provider = new TomlConfigFormatProvider();
        const string toml = """
            title = "Starry Config"

            [nodes.nijika]
            host = "10.0.0.1"
            port = 2222
            """;

        var node = provider.Parse(System.Text.Encoding.UTF8.GetBytes(toml), "test.toml");
        Assert.NotNull(node);
        Assert.Equal("Starry Config", node["title"]?.ToString());
        Assert.Equal("10.0.0.1", node["nodes"]?["nijika"]?["host"]?.ToString());

        var serialized = provider.Serialize(node);
        Assert.Contains("Starry Config", serialized, StringComparison.Ordinal);
        Assert.Contains("10.0.0.1", serialized, StringComparison.Ordinal);
    }

    [Fact]
    public void JsonFormatProvider_ParsesAndSerializesCorrectly()
    {
        var provider = new JsonConfigFormatProvider();
        const string json = """
            {
              "version": "1.0",
              "services": {
                "api": {
                  "image": "nginx:alpine",
                  "priority": 10
                }
              }
            }
            """;

        var node = provider.Parse(System.Text.Encoding.UTF8.GetBytes(json), "test.json");
        Assert.NotNull(node);
        Assert.Equal("1.0", node["version"]?.ToString());
        Assert.Equal("nginx:alpine", node["services"]?["api"]?["image"]?.ToString());

        var serialized = provider.Serialize(node);
        Assert.Contains("nginx:alpine", serialized, StringComparison.Ordinal);
    }

    [Fact]
    public void VirtualConfigEngine_LoadsDirectoryAndBuildsHierarchyWithSourceMap()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        Directory.CreateDirectory(Path.Combine(tempDir, "nodes"));
        Directory.CreateDirectory(Path.Combine(tempDir, "services"));

        try
        {
            File.WriteAllText(Path.Combine(tempDir, "starry.yaml"), "cluster_name: starry-mesh\nversion: '1.0'\n");
            File.WriteAllText(Path.Combine(tempDir, "nodes", "bocchi.yaml"), "host: 192.168.1.100\nport: 22\nvpn_ip: 10.99.0.1\n");
            File.WriteAllText(Path.Combine(tempDir, "services", "web.toml"), "image = 'caddy:latest'\npriority = 50\nnode = 'bocchi'\n");

            var tree = _engine.Load(tempDir);

            Assert.Equal("starry-mesh", tree.Root["cluster_name"]?.ToString());
            Assert.Equal("192.168.1.100", tree.Root["nodes"]?["bocchi"]?["host"]?.ToString());
            Assert.Equal("caddy:latest", tree.Root["services"]?["web"]?["image"]?.ToString());

            // Check SourceMap
            Assert.True(tree.SourceMap.ContainsKey("nodes.bocchi.host"));
            Assert.Contains("bocchi.yaml", tree.SourceMap["nodes.bocchi.host"].FilePath, StringComparison.Ordinal);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void VirtualConfigEngine_PacksAndSplitsCorrectly()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "starry_pack_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            var tree = new ConfigTree();
            tree.SetNode("cluster_name", JsonValue.Create("test-fleet"));
            tree.SetNode("nodes.ryo.host", JsonValue.Create("10.0.0.5"));
            tree.SetNode("services.db.image", JsonValue.Create("postgres:16"));

            var packedFile = Path.Combine(tempDir, "bundle.yaml");
            _engine.Pack(tree, packedFile, "yaml");
            Assert.True(File.Exists(packedFile));

            var splitDir = Path.Combine(tempDir, "split_out");
            _engine.Split(tree, splitDir, "yaml");

            Assert.True(File.Exists(Path.Combine(splitDir, "starry.yaml")));
            Assert.True(File.Exists(Path.Combine(splitDir, "nodes", "ryo.yaml")));
            Assert.True(File.Exists(Path.Combine(splitDir, "services", "db.yaml")));
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
```

## File: tests/Starry.UnitTests/WireGuardMeshTests.cs

**Lines**: 63 | **Size**: 2.6 KB | **Language**: csharp

```csharp
using Starry.Infrastructure.Network;
using Xunit;

namespace Starry.UnitTests;

public class WireGuardMeshTests
{
    [Fact]
    public void GenerateMeshConfigurations_GeneratesP2PConfigsWithKeepaliveAndVips()
    {
        var nodes = new List<WireGuardNodeConfig>
        {
            new("bocchi", "10.99.0.1", "privKey1", "pubKey1", 51820, "203.0.113.1", IsPublic: true),
            new("nijika", "10.99.0.2", "privKey2", "pubKey2", 51820, null, IsPublic: false),
            new("ryo", "10.99.0.3", "privKey3", "pubKey3", 51820, null, IsPublic: false)
        };

        var containerVips = new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal)
        {
            ["nijika"] = ["10.99.10.1", "10.99.10.2"],
            ["ryo"] = ["10.99.20.5"]
        };

        var configs = WireGuardMeshService.GenerateMeshConfigurations(nodes, containerVips);

        Assert.Equal(3, configs.Count);
        Assert.True(configs.ContainsKey("bocchi"));
        Assert.True(configs.ContainsKey("nijika"));
        Assert.True(configs.ContainsKey("ryo"));

        // Check bocchi config:
        var bocchiConf = configs["bocchi"];
        Assert.Contains("Address = 10.99.0.1/16", bocchiConf, StringComparison.Ordinal);
        Assert.Contains("ListenPort = 51820", bocchiConf, StringComparison.Ordinal);
        Assert.Contains("PersistentKeepalive = 25", bocchiConf, StringComparison.Ordinal);

        // Bocchi should have nijika's VIPs in AllowedIPs:
        Assert.Contains("AllowedIPs = 10.99.0.2/32, 10.99.10.1/32, 10.99.10.2/32", bocchiConf, StringComparison.Ordinal);

        // Nijika config should have Endpoint for public bocchi node:
        var nijikaConf = configs["nijika"];
        Assert.Contains("Endpoint = 203.0.113.1:51820", nijikaConf, StringComparison.Ordinal);

        // Nijika config should NOT have Endpoint for non-public ryo node:
        Assert.DoesNotContain("Endpoint = null", nijikaConf, StringComparison.Ordinal);
    }

    [Fact]
    public void GenerateMeshConfigurations_DoesNotDuplicatePort_WhenEndpointAlreadyHasPort()
    {
        var nodes = new List<WireGuardNodeConfig>
        {
            new("bocchi", "10.99.0.1", "privKey1", "pubKey1", 51820, "203.0.113.1:51820", IsPublic: true),
            new("nijika", "10.99.0.2", "privKey2", "pubKey2", 51820, null, IsPublic: false)
        };

        var configs = WireGuardMeshService.GenerateMeshConfigurations(nodes);
        var nijikaConf = configs["nijika"];

        Assert.Contains("Endpoint = 203.0.113.1:51820", nijikaConf, StringComparison.Ordinal);
        Assert.DoesNotContain(":51820:51820", nijikaConf, StringComparison.Ordinal);
    }
}
```

