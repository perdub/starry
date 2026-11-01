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
