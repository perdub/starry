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

## 6. Демон агента (`starry agent`)

### `starry agent run`

Запуск агента узла в текущем терминале.

```bash
starry agent run [--config /etc/starry/agent.yaml]
```

### `starry agent install-service`

Создает и регистрирует unit-файл `systemd` (`starry-agent.service`) в операционной системе.

```bash
sudo starry agent install-service
```

### `starry agent status`

Проверяет состояние локального агента, соединение с соседями по WireGuard и статус локальных контейнеров.

```bash
starry agent status
```
