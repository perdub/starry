# Configuration Syntax & Schema

The `starry.yaml` manifest serves as the single source of truth describing your fleet's desired state.

---

## 1. Manifest Layout

```yaml
version: "1.0"

# WireGuard overlay network settings
mesh:
  cidr: "10.42.0.0/16"
  port: 51820
  keepalive: 25

# Cluster host definitions
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

# Containerized workloads
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

## 2. `mesh` Block

Defines parameters for the encrypted WireGuard full-mesh overlay.

| Key | Type | Default | Description |
|---|---|---|---|
| `cidr` | string | `10.42.0.0/16` | Subnet allocated for internal node and service communication. |
| `port` | integer | `51820` | WireGuard UDP listen port on external interfaces. |
| `keepalive` | integer | `25` | Interval in seconds for NAT traversal keepalive heartbeats. |

---

## 3. `nodes` Block

Each key maps to a unique server identifier.

| Key | Type | Required | Description |
|---|---|---|---|
| `address` | string | Yes | Public IPv4/IPv6 address or fully-qualified domain name. |
| `mesh_ip` | string | Yes | Static IP address within `mesh.cidr`. |
| `engine` | string | Yes | Container engine: `docker` or `podman`. |
| `ssh.user` | string | No (def: `root`) | Remote SSH user. |
| `ssh.port` | integer | No (def: `22`) | SSH port. |
| `ssh.key_path` | string | No | Path to Ed25519/RSA private key. |
| `packages` | list | No | Host OS packages (`apt`, `dnf`, `apk`, `pacman`). |
| `os_maintenance` | object | No | Automated security patching and reboot policy. |

### `os_maintenance` Policy

```yaml
os_maintenance:
  auto_security_updates: true # Apply upstream security patches automatically
  schedule: "@weekly"         # Evaluation schedule (cron or @daily/@weekly)
  reboot_policy: "rolling_drain" # Options: never | rolling_drain | immediate
```

- `never`: Install updates but skip automated reboots.
- `rolling_drain`: Graceful traffic rerouting, drain, host reboot, health verification, and traffic restore.
- `immediate`: Reboot the host immediately after kernel package installation.

---

## 4. `services` Block

Defines application containers.

| Key | Type | Description |
|---|---|---|
| `image` | string | Container image tag (e.g. `redis:7-alpine`). |
| `node` | string | Target node key from the `nodes` block. |
| `ports` | list | Port publishing `"HOST:CONTAINER"`. Isolated to WireGuard or reverse proxy. |
| `volumes` | list | Volume bindings `"/host/path:/container/path[:ro]"`. |
| `env` | map | Key-value environment variables. Supports `ENC[...]` secrets. |
| `restart` | string | Restart policy: `always`, `on-failure`, `unless-stopped`, `no`. |
| `healthcheck` | object | Healthcheck definition (`test`, `interval`, `timeout`, `retries`). |

---

## 5. Encrypted Secret Tokens (ENC)

Confidential secrets encrypted with `starry secret encrypt` follow this layout:

```text
ENC[AES256_GCM,data:<base64-ciphertext>,iv:<base64-iv>,tag:<base64-tag>]
```

Starry decrypts values strictly in-memory immediately prior to dispatching container manifests to Docker/Podman APIs.
