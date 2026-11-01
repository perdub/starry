# Getting Started with Starry

Welcome to the **Starry** quickstart guide! This tutorial guides you through installing the CLI, initializing your infrastructure GitOps repository, bootstrapping server nodes, and deploying your first containerized services.

---

## 1. CLI Installation

Starry is distributed in two official formats: as a self-contained **Native AOT** executable (requiring zero runtime dependencies) and as a global **.NET Tool**.

::: code-group

```bash [Native AOT (Linux / macOS)]
# Download the latest binary for your architecture
curl -fsSL https://github.com/starry-project/starry/releases/latest/download/starry-linux-x64 -o starry

# Grant execution permissions
chmod +x starry

# Move binary to system PATH
sudo mv starry /usr/local/bin/starry

# Verify installation
starry --version
```

```bash [.NET Global Tool]
# Install via .NET SDK (.NET 10+ required)
dotnet tool install -g Starry.Cli

# Verify command availability
starry --version
```

```powershell [Windows (Native AOT)]
# Download and setup in PowerShell
Invoke-WebRequest -Uri "https://github.com/starry-project/starry/releases/latest/download/starry-win-x64.exe" -OutFile "$HOME\starry.exe"

# Add to user PATH
[Environment]::SetEnvironmentVariable("Path", $env:Path + ";$HOME", "User")

starry --version
```

:::

---

## 2. Initializing the Repository

To manage your fleet using declarative GitOps patterns, create a dedicated Git repository and initialize Starry:

```bash
mkdir my-fleet && cd my-fleet
git init
starry init
```

The `starry init` command creates the initial project layout:

```text
my-fleet/
├── starry.yaml          # Main declarative cluster manifest
├── .starry/
│   ├── keys/            # Local SSH & WireGuard keys (git-ignored)
│   └── cache/           # Local state cache
├── .gitignore           # Ignores private keys and runtime artifacts
└── README.md
```

---

## 3. Configuring Master Encryption Key

Starry employs **AES-256-GCM** authenticated envelope encryption with PBKDF2/Argon2 key derivation. Sensitive tokens, database credentials, and certificates are safely committed directly into `starry.yaml`.

### Generating a Master Key

Generate a cryptographically secure 256-bit key:

```bash
starry secret keygen
```

Export the generated base64 string to your environment:

```bash
export STARRY_MASTER_KEY="your-generated-base64-master-key"
```

::: tip CI/CD Automation
When running automated workflows in GitHub Actions or GitLab CI, set the `STARRY_MASTER_KEY` repository secret.
:::

### Encrypting Secrets

To encrypt confidential values:

```bash
starry secret encrypt "my-super-secret-password"
```

The command returns an authenticated envelope:
`ENC[AES256_GCM,data:c3VwZXJzZWNyZXQ=,iv:...,tag:...]`

Paste this value directly into the `env` or `secrets` block of your `starry.yaml`.

---

## 4. Bootstrapping Server Nodes

Prepare a clean Linux server running any supported distribution (Ubuntu, Debian, AlmaLinux, Rocky Linux, Alpine, or Arch).

Run the automated bootstrap command:

```bash
starry nodes bootstrap root@203.0.113.10 --name node-01 --engine docker
```

During the bootstrap process, Starry will:
1. Connect via SSH and inspect host capabilities.
2. Install the specified container engine (`docker` or `podman`) if missing.
3. Install and configure `wireguard-tools` and `nftables`.
4. Generate WireGuard keys and assign a mesh IP from `10.42.0.0/16`.
5. Append the validated node entry into `starry.yaml`.

---

## 5. Planning and Applying Changes

Edit your `starry.yaml` file to define desired state:

```yaml
version: "1.0"

mesh:
  cidr: "10.42.0.0/16"
  port: 51820

nodes:
  node-01:
    address: "203.0.113.10"
    mesh_ip: "10.42.0.1"
    engine: "docker"
    packages:
      - curl
      - htop

services:
  web:
    image: "nginx:alpine"
    node: "node-01"
    ports:
      - "80:80"
    env:
      API_SECRET: "ENC[AES256_GCM,data:...,iv:...,tag:...]"
```

### Dry Run (Plan)

Preview planned configuration changes without executing mutations:

```bash
starry plan
```

Starry reconciles desired state against live servers and displays an actionable diff (container creates/updates, firewall rule adjustments, package installs).

### Apply

Enforce desired state across your fleet:

```bash
starry apply
```

---

## 6. Running the Server Agent Daemon

For autonomous health checking, automatic container restarts, and real-time monitoring, Starry features a lightweight server agent.

Install the systemd service on your target server:

```bash
sudo starry agent install-service
sudo systemctl status starry-agent
```

Or run the agent in the foreground for debugging:

```bash
starry agent run --verbose
```

Congratulations! Your initial Starry cluster is up, running, and secured. Proceed to the [Architecture Overview](/en/architecture/overview) to explore the system internals.
