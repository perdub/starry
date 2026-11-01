# Архитектура и синтаксис конфигурации

Starry использует передовую концепцию **Virtual Config Engine**: вы можете описывать инфраструктуру как единым файлом (`starry.yaml`), так и разбивать её на модульные папки и файлы любого удобного формата (**YAML**, **JSON**, **TOML**). Starry автоматически объединяет их в единое виртуальное дерево с полным сохранением карты источников (**Source Map**).

---

## 1. Модульная организация файлов (Best Practices)

### Для небольших кластеров (Single File)
Если у вас 1–3 сервера, удобно хранить всю конфигурацию в одном корневом файле:

```text
my-fleet/
├── starry.yaml
└── .gitignore
```

### Для средних и крупных флотов (Modular Hierarchy)
При росте количества серверов и сервисов рекомендуется модульная структура:

```text
cluster/
├── starry.yaml               # Общие настройки кластера и сети mesh
├── nodes/
│   ├── bocchi.yaml           # Нода bocchi -> root.nodes.bocchi
│   ├── nijika.json           # Нода nijika -> root.nodes.nijika
│   └── ryo.toml              # Нода ryo -> root.nodes.ryo
└── services/
    ├── api.yaml              # Сервис api -> root.services.api
    ├── database.toml         # Сервис database -> root.services.database
    └── frontend.json         # Сервис frontend -> root.services.frontend
```

Starry маппит относительные пути файлов в канонические JSON-пути виртуального дерева:
- `nodes/bocchi.yaml` автоматически монтируется в `root.nodes.bocchi`
- `services/api.toml` автоматически монтируется в `root.services.api`
- Файлы `starry.yaml` или `config.yaml` в корне объединяются с корнем дерева `root`

Каждое терминальное значение сохраняет точную ссылку на файл, строку и колонку в **Source Map**, благодаря чему ошибки валидации указывают на конкретные исходные файлы!

---

## 2. Поддержка форматов: YAML, JSON и TOML

Вы можете свободно смешивать форматы в одном проекте! Ниже приведен один и тот же фрагмент конфигурации в трёх поддерживаемых форматах:

::: code-group

```yaml [YAML (nodes/bocchi.yaml)]
host: "198.51.100.10"
port: 22
vpn_ip: "10.99.0.1"
engine: "docker"
packages:
  - wireguard
  - nftables
  - curl
```

```toml [TOML (nodes/bocchi.toml)]
host = "198.51.100.10"
port = 22
vpn_ip = "10.99.0.1"
engine = "docker"
packages = ["wireguard", "nftables", "curl"]
```

```json [JSON (nodes/bocchi.json)]
{
  "host": "198.51.100.10",
  "port": 22,
  "vpn_ip": "10.99.0.1",
  "engine": "docker",
  "packages": [
    "wireguard",
    "nftables",
    "curl"
  ]
}
```

:::

---

## 3. Схема и структура кластера

Манифест кластера описывает узлы (`nodes`), оверлейную сеть (`mesh`), развертываемые сервисы (`services`) и политики резервного копирования:

```yaml
version: "1.0"
cluster_name: "kessoku-prod"

# Параметры WireGuard Mesh оверлея
mesh:
  cidr: "10.99.0.0/16"
  port: 51820
  keepalive: 25

# Серверные узлы кластера
nodes:
  bocchi:
    host: "198.51.100.10"
    port: 22
    vpn_ip: "10.99.0.1"
    is_public: true
    ssh_key: "ENC[AES256_GCM:v1:nonce:cipher:tag]"

# Контейнерные сервисы
services:
  web:
    image: "nginx:alpine"
    node: "bocchi"
    priority: 80
    ports:
      - "80:80"
      - "443:443"
    env:
      DB_PASSWORD: "ENC[AES256_GCM:v1:nonce:cipher:tag]"
```

---

## 4. Команды управления конфигурацией (CLI)

Starry предоставляет богатый набор встроенных утилит для инспекции и трансформации конфигов:

### Валидация (`starry config validate`)
Проверяет структуру, существование обязательных полей, корректность сетевых адресов и синтаксис секретов:
```bash
starry config validate ./cluster
```

### Упаковка в единый файл (`starry config pack`)
Объединяет модульную директорию со всеми подпапками в один канонический файл:
```bash
starry config pack ./cluster -o starry.bundle.yaml --format yaml
```

### Разделение монолита на модули (`starry config split`)
Автоматически разбивает монолитный `starry.yaml` на стандартную иерархию папок (`nodes/`, `services/`):
```bash
starry config split starry.bundle.yaml -o ./modular-config --format yaml
```

### Каноническое форматирование (`starry config fmt`)
Форматирует конфигурационный файл в соответствии со стандартом:
```bash
starry config fmt cluster/nodes/bocchi.yaml
```

### Генерация JSON Schema (`starry config schema`)
Генерирует официальную JSON Schema для валидации и автодополнения в VS Code, JetBrains Rider и Neovim:
```bash
starry config schema > starry.schema.json
```
