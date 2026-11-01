# Configuration Architecture & Syntax

Starry leverages an advanced **Virtual Config Engine**: you can define your infrastructure as a single monolith file (`starry.yaml`) or break it down into modular directories and files across multiple formats (**YAML**, **JSON**, **TOML**). Starry seamlessly merges them into a virtual configuration tree while preserving exact source file coordinates (**Source Map**).

---

## 1. Modular Directory Organization (Best Practices)

### For Small Fleets (Single File)
For 1–3 servers, keeping everything in a single root file is simplest:

```text
my-fleet/
├── starry.yaml
└── .gitignore
```

### For Medium & Large Fleets (Modular Hierarchy)
As your fleet grows with multiple nodes and services, a modular layout is recommended:

```text
cluster/
├── starry.yaml               # Cluster global settings & mesh network
├── nodes/
│   ├── bocchi.yaml           # Node bocchi -> root.nodes.bocchi
│   ├── nijika.json           # Node nijika -> root.nodes.nijika
│   └── ryo.toml              # Node ryo -> root.nodes.ryo
└── services/
    ├── api.yaml              # Service api -> root.services.api
    ├── database.toml         # Service database -> root.services.database
    └── frontend.json         # Service frontend -> root.services.frontend
```

Starry automatically maps relative file paths to canonical JSON paths:
- `nodes/bocchi.yaml` mounts to `root.nodes.bocchi`
- `services/api.toml` mounts to `root.services.api`
- Files named `starry.yaml` or `config.yaml` at root merge directly into `root`

Every leaf property retains its exact file, line, and column reference in the **Source Map**, making validation and schema errors precise!

---

## 2. Multi-Format Support: YAML, JSON & TOML

You can freely mix formats within a single project! Below is the same configuration snippet across all three supported formats:

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

## 3. Cluster Schema & Spec

The cluster manifest defines cluster nodes (`nodes`), overlay mesh network (`mesh`), container services (`services`), and backup policies:

```yaml
version: "1.0"
cluster_name: "kessoku-prod"

# WireGuard Mesh network settings
mesh:
  cidr: "10.99.0.0/16"
  port: 51820
  keepalive: 25

# Cluster nodes
nodes:
  bocchi:
    host: "198.51.100.10"
    port: 22
    vpn_ip: "10.99.0.1"
    is_public: true
    ssh_key: "ENC[AES256_GCM:v1:nonce:cipher:tag]"

# Container services
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

## 4. Configuration Management Commands (CLI)

Starry provides a suite of CLI tools for inspecting and transforming configs:

### Validation (`starry config validate`)
Verifies schema constraints, required fields, network addresses, and encrypted secret syntax:
```bash
starry config validate ./cluster
```

### Packing (`starry config pack`)
Packs a modular directory hierarchy into a single bundle file:
```bash
starry config pack ./cluster -o starry.bundle.yaml --format yaml
```

### Splitting (`starry config split`)
Splits a monolithic bundle back into standard modular folders (`nodes/`, `services/`):
```bash
starry config split starry.bundle.yaml -o ./modular-config --format yaml
```

### Canonical Formatting (`starry config fmt`)
Formats config files cleanly according to canonical formatting rules:
```bash
starry config fmt cluster/nodes/bocchi.yaml
```

### JSON Schema Generation (`starry config schema`)
Generates the official JSON Schema for IDE validation and autocompletion:
```bash
starry config schema > starry.schema.json
```
