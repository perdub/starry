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

### `starry nodes doctor`

Выполняет комплексную проверку серверов перед развертыванием кластера:
- Синхронизация времени по UTC (рассинхрон не более 30 сек во избежание сбоев рукопожатий WireGuard).
- Флаги маршрутизации ядра (`net.ipv4.ip_forward`, `net.ipv6.conf.all.forwarding`, `proxy_ndp`).
- Доступность интерфейса `/dev/net/tun` и файрвола `nftables`.
- Конфликты за порт 53 с `systemd-resolved`.
- Состояние SELinux, доступный объем диска и запас инодов.

```bash
# Диагностика:
starry nodes doctor

# Автоматическое исправление найденных конфликтов (включение sysctl, отключение systemd-resolved):
starry nodes doctor --fix
```

### `starry nodes rotate-keys`

Генерирует новые пары ключей Ed25519 (SSH) и WireGuard, разворачивает их на нодах с предварительной проверкой связности и удаляет устаревшие ключи.
С флагом `--randomize-ports` производит безопасную миграцию SSH-порта на случайный диапазон `10000..60000` без риска потери доступа (lockout prevention).

```bash
starry nodes rotate-keys [--randomize-ports]
```

---

## 4. Управление секретами (`starry secret`)

### `starry secret update` (алиас: `set`)

Шифрует и атомарно записывает секрет по его каноническому пути, **сохраняя неизменными все `$`-ссылки** в конфигурации:

```bash
starry secret update services.db.env.POSTGRES_PASSWORD "SuperSecretPass123"
```

### `starry secret get`

Расшифровывает и выводит открытое значение секрета:

```bash
starry secret get services.db.env.POSTGRES_PASSWORD
```

### `starry secret list`

Выводит таблицу всех зашифрованных (`ENC[...]`) и открытых (`PLAIN[...]`) секретов в репозитории с указанием исходных файлов.

```bash
starry secret list
```

### `starry secret sync-github`

Передает локально разрешенный мастер-ключ (`.starry/master.key` или `STARRY_MASTER_KEY`) в секреты GitHub Actions через утилиту `gh`.

```bash
starry secret sync-github [--name STARRY_MASTER_KEY]
```

---

## 5. Маршрутизация портов и публикация сервисов

### `starry ports list`

Выводит список всех именованных правил двунаправленной маршрутизации портов:

```bash
starry ports list
```

### `starry domains list`

Выводит иерархическое дерево маршрутов доменов, точек входа и Layer 4 SNI-прокси:

```bash
starry domains list
```

### `starry expose`

Публикует порт сервиса во внешний мир с опциональной рандомизацией порта и привязкой IPv4/IPv6:

```bash
# Опубликовать веб-сервис на случайном порту:
starry expose shop-web 3000 --family both --port random

# Опубликовать на конкретном порту:
starry expose shop-web 3000 --port 8080 --proto tcp
```

---

## 6. Контекстная справка (`starry help`)

Для любой команды и подкоманды Starry доступна интерактивная карточка помощи с синтаксисом, таблицей флагов и примерами:

```bash
starry help secret
starry nodes --help
starry expose -h
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
