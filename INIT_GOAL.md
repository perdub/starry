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