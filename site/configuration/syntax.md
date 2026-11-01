# Синтаксис и схема конфигурации

Манифест `starry.yaml` является центральным декларативным описанием желаемого состояния вашего кластера.

---

## 1. Общая структура манифеста

```yaml
version: "1.0"

# Конфигурация оверлейной сети WireGuard
mesh:
  cidr: "10.42.0.0/16"
  port: 51820
  keepalive: 25

# Описание серверных узлов (нод)
nodes:
  app-01:
    address: "203.0.113.10"
    mesh_ip: "10.42.0.1"
    ssh:
      user: "root"
      port: 22
      key_path: "~/.ssh/id_ed25519"
    engine: "docker"
    packages:
      - curl
      - htop
      - rsync
    os_maintenance:
      auto_security_updates: true
      schedule: "@weekly"
      reboot_policy: "rolling_drain"

# Описание развертываемых сервисов
services:
  web:
    image: "nginx:alpine"
    node: "app-01"
    restart: "always"
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - "/var/data/web:/usr/share/nginx/html:ro"
    env:
      ENVIRONMENT: "production"
      DATABASE_PASSWORD: "ENC[AES256_GCM,data:dXNlcjpwYXNz...,iv:9kP2...,tag:z0X8...]"
    healthcheck:
      test: ["CMD-SHELL", "curl -f http://localhost/ || exit 1"]
      interval: 10s
      timeout: 3s
      retries: 3
```

---

## 2. Секция `mesh`

Задает параметры полно-связной оверлейной сети WireGuard.

| Поле | Тип | По умолчанию | Описание |
|---|---|---|---|
| `cidr` | string | `10.42.0.0/16` | Подсеть для внутренней адресации нод и сервисов. |
| `port` | integer | `51820` | UDP-порт WireGuard для входящих соединений. |
| `keepalive` | integer | `25` | Интервал (в секундах) отправки keepalive пакетов для NAT-траверсала. |

---

## 3. Секция `nodes`

Каждый ключ в словаре `nodes` представляет собой уникальный идентификатор хоста.

| Поле | Тип | Обязательное | Описание |
|---|---|---|---|
| `address` | string | Да | Публичный IPv4/IPv6 адрес или DNS-имя хоста. |
| `mesh_ip` | string | Да | Статический IP-адрес узла в WireGuard-сети (из диапазона `mesh.cidr`). |
| `engine` | string | Да | Контейнерный движок: `docker` или `podman`. |
| `ssh.user` | string | Нет (def: `root`) | Пользователь для SSH-подключения. |
| `ssh.port` | integer | Нет (def: `22`) | SSH-порт сервера. |
| `ssh.key_path` | string | Нет | Путь к приватному SSH-ключу. |
| `packages` | list | Нет | Список системных пакетов (`apt`, `dnf`, `apk`, `pacman`). |
| `os_maintenance` | object | Нет | Политика автоматических обновлений ОС. |

### Политика `os_maintenance`

```yaml
os_maintenance:
  auto_security_updates: true # Автоматическая установка патчей безопасности
  schedule: "@weekly"         # Расписание проверки (cron или @daily/@weekly)
  reboot_policy: "rolling_drain" # Варианты: never | rolling_drain | immediate
```

- `never`: не выполнять перезагрузку автоматически, только уведомить оператора.
- `rolling_drain`: последовательный дренаж трафика, перезагрузка и возврат в строй.
- `immediate`: немедленная перезагрузка сразу после применения обновлений ядра.

---

## 4. Секция `services`

Описывает запускаемые контейнеры.

| Поле | Тип | Описание |
|---|---|---|
| `image` | string | Образ контейнера (например, `redis:7-alpine`). |
| `node` | string | Имя ноды из секции `nodes`, на которой запускается сервис. |
| `ports` | list | Проброс портов в формате `"HOST:CONTAINER"`. Доступ строго ограничен WireGuard или обратным прокси. |
| `volumes` | list | Монтирование томов в формате `"/host/path:/container/path[:ro]"`. |
| `env` | map | Переменные окружения. Значения могут быть зашифрованы в формате `ENC[...]`. |
| `restart` | string | Политика перезапуска: `always`, `on-failure`, `unless-stopped`, `no`. |
| `healthcheck`| object | Параметры проверки работоспособности (`test`, `interval`, `timeout`, `retries`). |

---

## 5. Формат зашифрованных значений (ENC)

Чувствительные данные шифруются командой `starry secret encrypt` и сохраняются в формате:

```text
ENC[AES256_GCM,data:<base64-ciphertext>,iv:<base64-iv>,tag:<base64-tag>]
```

Starry расшифровывает такие строки исключительно в оперативной памяти непосредственно перед передачей в API контейнерного движка.
