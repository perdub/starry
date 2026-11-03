# 🌟 STARRY

<p align="center">
  <img src="site/public/logo.svg" width="128" height="128" alt="Starry Logo" />
</p>

<p align="center">
  <strong>Decentralized GitOps Orchestrator for Docker, Podman, WireGuard, nftables, and VPS Fleets</strong>
</p>

<p align="center">
  <a href="https://github.com/perdub/starry/actions/workflows/ci.yml"><img src="https://github.com/perdub/starry/actions/workflows/ci.yml/badge.svg" alt="CI Status" /></a>
  <a href="https://github.com/perdub/starry/actions/workflows/ci-full-matrix.yml"><img src="https://github.com/perdub/starry/actions/workflows/ci-full-matrix.yml/badge.svg" alt="Matrix CI" /></a>
  <a href="https://perdub.github.io/starry/"><img src="https://img.shields.io/badge/Docs-VitePress-f59e0b" alt="Documentation" /></a>
  <a href="https://www.nuget.org/packages/starrycli"><img src="https://img.shields.io/nuget/v/starrycli.svg?color=blue" alt="NuGet" /></a>
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-green" alt="License" /></a>
</p>

---

## 📖 Overview

**`starry`** is an autonomous, single-binary GitOps orchestrator designed to run across heterogeneous cloud providers and low-cost virtual private servers (including NAT-isolated nodes and servers with small 10–20 GB SSDs).

It manages the entire lifecycle of:
* **Container Runtimes**: Direct Unix Domain Socket control of **Docker Engine** and **Podman** (Rootful & unprivileged Rootless) supporting full Docker Compose specifications, automatic engine bootstrap on bare VPS, and Image Digest (`ImageId`) & `starry.spec.hash` drift reconciliation.
* **Encrypted IPv6 Mesh Networking**: P2P **WireGuard** (`starry0`) running on the ULA prefix **`fd67:b00b::/32`**, assigning each host a `/48` subnet and each container replica a dedicated `/64` subnet with zero-downtime routing.
* **Cross-Platform CLI (Windows, macOS, Linux)**: Full Windows workstation compatibility (`Win32-OpenSSH`) with automatic TCP loopback bridge (`127.0.0.1:<port>` -> `/var/run/docker.sock`), colon-safe control paths, and TOFU `known_hosts`.
* **Functional Configuration Engine (`Starry.Dsl`)**: Pure F# unified graph AST supporting whole-subtree substitution (`$`), object inheritance (`$:`), and automatic `files/<service>/*` text mounting.
* **Persistent Volumes & S3 Auto-Restore**: Multi-volume container support with a 3-step lifecycle: local preservation, automatic pull & restore from S3 on fresh nodes, and zero-disk-overhead streaming backups (`tar | zstd | S3`).
* **Host Firewalls & NAT**: Isolated table `inet starry` in **`nftables`** for atomic DNAT port forwarding, inter-container network ACL isolation, symmetric outbound SNAT, and `DualStackPortRelay` for IPv4 -> IPv6 bridging.
* **Scoped Internal DNS**: Embedded DNS server (`fd67:b00b:<host_id>::1`) with source-aware ACL isolation, round-robin replica load-balancing, and egress policy enforcement.
* **Pre-Flight Doctor & Port Randomization**: Host diagnostics (`starry nodes doctor`) verifying strict UTC skew, sysctls, tun device, and port 53, plus lockout-safe SSH port migration (`starry nodes rotate-keys --randomize-ports`).
* **Operating System Packages**: Unified abstraction across **Apt** (Debian/Ubuntu), **Dnf** (RHEL/AlmaLinux/Rocky/Fedora), **Apk** (Alpine), and **Pacman** (Arch Linux) with rolling security updates.
* **Security & Secret Management**: In-repository encrypted secrets using **AES-256-GCM** bound to canonical JSON paths with **AAD (Additional Authenticated Data)** to block cut-and-paste tampering.
* **Zero-Disk Streaming Backups**: Real-time volume backups via `tar | zstd | rclone rcat` without creating intermediate files on constrained VPS disks.
* **Disaster Recovery Bundles**: Export and re-import entire fleet configurations and states to spin up on new IP addresses within minutes.

Documentation is available online at: **[https://perdub.github.io/starry/](https://perdub.github.io/starry/)** (available in **English** and **Russian**).

---

## ⚡ Quick Start

### 1. Installation

#### Option A: .NET Global Tool
```bash
dotnet tool install -g starrycli
```

#### Option B: Standalone Native AOT Binary (7.3 MB, Zero Dependencies)
Download the pre-compiled native binary for Linux (`x64`, `arm64`) from [GitHub Releases](https://github.com/perdub/starry/releases):
```bash
curl -fsSL https://github.com/perdub/starry/releases/latest/download/starry-linux-x64 -o /usr/local/bin/starry
chmod +x /usr/local/bin/starry
```

### 2. Configuration (`starry.yaml` or Modular Directory)

Starry supports single-file configurations as well as modular multi-file directories (`YAML`, `JSON`, `TOML`):

```yaml
version: "1.0"
cluster_name: "kessoku-prod"

mesh:
  cidr: "10.99.0.0/16"
  port: 51820
  keepalive: 25

nodes:
  bocchi:
    host: "198.51.100.10"
    port: 22
    vpn_ip: "10.99.0.1"
    is_public: true
    ssh_key: "ENC[AES256_GCM:v1:...]"

services:
  web:
    image: "nginx:alpine"
    node: "bocchi"
    vip: "10.99.10.5"
    priority: 80
    ports:
      - "80:80"
      - "443:443"
    files:
      /etc/nginx/conf.d/default.conf: |
        server {
            listen 80;
            location / { return 200 "Hello from Starry!"; }
        }
```

### 3. Validate, Plan & Apply

```bash
# Validate cluster syntax, schema and secrets integrity
starry config validate ./cluster

# Build DAG migration execution plan
starry plan --config ./cluster

# Apply planned changes
starry apply --config ./cluster
```

---

## 🛠️ CLI Commands Reference

| Command | Description |
|---|---|
| `starry config validate [path]` | Validate configuration schema, references, and secret envelopes. |
| `starry config pack <dir> -o <file>` | Pack modular directory hierarchy into a single bundle file. |
| `starry config split <file> -o <dir>` | Split monolithic bundle back into modular folder structure. |
| `starry config fmt <file>` | Canonically format configuration files. |
| `starry config schema` | Print official JSON Schema for IDE autocompletion. |
| `starry encrypt [path]` | Encrypt all `PLAIN[...]` secrets in configuration into `ENC[...]`. |
| `starry encrypt global --reencrypt` | Rotate master key, re-encrypt all secrets, and commit to Git. |
| `starry nodes upgrade --rolling` | Execute zero-downtime rolling OS security updates across nodes. |
| `starry secret update <path> [val]` | Atomically encrypt and update a secret value preserving graph references. |
| `starry secret get <path>` | Decrypt and output plaintext secret value using resolved master key. |
| `starry secret list` | Inspect all encrypted and plaintext secrets with their source files. |
| `starry secret sync-github` | Push resolved master key to GitHub Actions Secrets. |
| `starry nodes doctor [--fix]` | Run host preflight checks (UTC clock skew, sysctls, tun device, port 53). |
| `starry nodes rotate-keys [--randomize-ports]` | Rotate Ed25519 SSH keys and safely migrate ports without lockout risk. |
| `starry ports list` | List all named bidirectional port routing rules. |
| `starry domains list` | List all hierarchical domain routes and SNI passthrough endpoints. |
| `starry expose <svc> <port> [flags]` | Expose container service with optional random port / IP binding. |
| `starry plan` | Generate and visualize DAG migration graph with colored icons. |
| `starry apply` | Execute planned actions across the cluster fleet. |
| `starry net interfaces [--verify]` | Inspect and verify network interfaces, IPv4/IPv6, and NAT status. |
| `starry volume open <volume>` | Open remote container volume via secure SFTP tunnel. |
| `starry bundle export -o <archive>` | Export entire fleet into a disaster recovery capsule. |
| `starry bundle import <archive>` | Restore fleet capsule with optional `--override-ip` mappings. |
| `starry adopt <user@host:port>` | Brownfield zero-downtime adoption of existing Docker/Podman servers. |
| `starry unmanaged list` | List unmanaged containers running outside Starry GitOps. |

---

## 🔒 Security Architecture: AES-256-GCM + AAD

Starry uses **Authenticated Encryption with Associated Data (AEAD)**:
* **Cipher**: AES-256-GCM with a 32-byte master key, 12-byte random nonce, and 16-byte authentication tag.
* **AAD Binding**: Every secret embeds its exact canonical JSON path (e.g. `nodes.bocchi.ssh_key`) into the authentication tag. Moving an encrypted string to another field will fail decryption with `CryptographicException`!
* **Automated Key Rotation**: Monthly automated rotation in GitHub Actions (`.github/workflows/rotate-master-key.yml`).

---

## 🤝 Contributing & Architecture

* Built on **C# (.NET 10)** targeting **Native AOT** and **NuGet Tool** packaging.
* Clean architecture with zero third-party bloated Docker SDKs: pure Unix Domain Sockets over `SocketsHttpHandler`.
* Strict static analysis enforced with `TreatWarningsAsErrors=true`, `Meziantou.Analyzer`, `Roslynator`, and `SonarAnalyzer`.

```bash
# Clone and build
git clone https://github.com/perdub/starry.git
cd starry
dotnet build Starry.slnx

# Run 57 Unit, Chaos, and E2E Tests
dotnet test Starry.slnx
```

---

## 📄 License

Licensed under the [MIT License](LICENSE).
