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

### `starry nodes doctor`

Executes preflight diagnostics across nodes:
- UTC clock skew check (skew within 30s threshold to avoid WireGuard handshake drops).
- Kernel IP forwarding sysctls (`net.ipv4.ip_forward`, `net.ipv6.conf.all.forwarding`, `proxy_ndp`).
- Availability of `/dev/net/tun` and `nftables`.
- Port 53 conflicts with `systemd-resolved`.
- SELinux mode, root disk space, and free inode reserves.

```bash
# Diagnostic check:
starry nodes doctor

# Automatically fix issues (enabling sysctls, disabling systemd-resolved on port 53):
starry nodes doctor --fix
```

### `starry nodes rotate-keys`

Proactively rotates Ed25519 SSH keys and WireGuard keypairs across all nodes with rollback protection.
With `--randomize-ports`, safely migrates SSH to a random port `10000..60000` with lockout prevention.

```bash
starry nodes rotate-keys [--randomize-ports]
```

---

## 4. Secret Operations (`starry secret`)

### `starry secret update` (alias: `set`)

Encrypts and atomically writes a secret at its canonical path, **preserving all `$` graph references** untouched:

```bash
starry secret update services.db.env.POSTGRES_PASSWORD "SuperSecretPass123"
```

### `starry secret get`

Decrypts and displays the plaintext secret value:

```bash
starry secret get services.db.env.POSTGRES_PASSWORD
```

### `starry secret list`

Lists all encrypted (`ENC[...]`) and plaintext (`PLAIN[...]`) secret keys across the repository.

```bash
starry secret list
```

### `starry secret sync-github`

Uploads the resolved master key (`.starry/master.key` or `STARRY_MASTER_KEY`) to GitHub Actions Secrets via `gh secret set`.

```bash
starry secret sync-github [--name STARRY_MASTER_KEY]
```

---

## 5. Port Routing & Service Exposure

### `starry ports list`

Lists all named bidirectional port routing rules:

```bash
starry ports list
```

### `starry domains list`

Inspects hierarchical domain routes, target endpoints, and SNI passthroughs:

```bash
starry domains list
```

### `starry expose`

Exposes a service port with optional port randomization and IPv4/IPv6 family binding:

```bash
# Expose on a random port:
starry expose shop-web 3000 --family both --port random

# Expose on a specific port:
starry expose shop-web 3000 --port 8080 --proto tcp
```

---

## 6. Contextual Help (`starry help`)

Every Starry command and subcommand provides an interactive help card with syntax, flags, and examples:

```bash
starry help secret
starry nodes --help
starry expose -h
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
