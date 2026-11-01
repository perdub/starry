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
dotnet tool install -g Starry.Cli

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
    address: "203.0.113.10"
    mesh_ip: "10.42.0.1"
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
