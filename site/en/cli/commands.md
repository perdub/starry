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

### `starry nodes upgrade`

Triggers security update installation on a specified node following the defined `reboot_policy`.

```bash
starry nodes upgrade <node-name> [--drain]
```

---

## 4. Secret Operations (`starry secret`)

### `starry secret keygen`

Generates a cryptographically strong 256-bit AES-256-GCM master encryption key.

```bash
starry secret keygen
```

### `starry secret encrypt`

Encrypts plain text strings using the key defined in `STARRY_MASTER_KEY`:

```bash
starry secret encrypt "postgres-super-secure-password"
# Output: ENC[AES256_GCM,data:...,iv:...,tag:...]
```

### `starry secret decrypt`

Decrypts an `ENC[...]` token for local verification or debugging:

```bash
starry secret decrypt "ENC[AES256_GCM,data:...,iv:...,tag:...]"
```

---

## 5. Security & Key Rotation

### `starry rotate-secrets`

Decrypts all `ENC[...]` values, generates a fresh master key, re-encrypts the manifest, and syncs repository secrets via the GitHub API.

```bash
starry rotate-secrets [--github-repo user/repo]
```

### `starry rotate-keys`

Proactively rotates Ed25519 SSH keys and WireGuard keypairs across all nodes with rollback protection.

```bash
starry rotate-keys [--dry-run]
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
