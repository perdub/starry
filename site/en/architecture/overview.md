# Architecture Overview

**Starry** is engineered as a decentralized, masterless GitOps orchestrator designed to operate a heterogeneous fleet of virtual private servers (VPS), cloud instances, and bare-metal nodes.

---

## 1. High-Level Architecture

```text
               +--------------------------------------------------+
               |               GitOps Repository                  |
               |        (starry.yaml + ENC[...] secrets)          |
               +--------------------------------------------------+
                                        │
                       ┌────────────────┴────────────────┐
                       ▼                                 ▼
             +───────────────────+             +───────────────────+
             |   Developer CLI   |             |   CI/CD Runner    |
             |   (starry apply)  |             |  (GitHub Actions) |
             +───────────────────+             +───────────────────+
                       │                                 │
                       └────────────────┬────────────────┘
                                        │ SSH / WireGuard
                                        ▼
    ┌───────────────────────────────────┬───────────────────────────────────┐
    │  Node A (Ubuntu 24.04 / Docker)   │  Node B (AlmaLinux 9 / Podman)   │
    │                                   │                                   │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |     Starry Host Agent       |  │  |     Starry Host Agent       |  │
    │  |  (Healthcheck / Telemetry)  |  │  |  (Healthcheck / Telemetry)  |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |      nftables Firewall      |  │  |      nftables Firewall      |  │
    │  | (ports open to wg0 mesh)    |  │  | (ports open to wg0 mesh)    |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |   WireGuard Mesh (10.42.0.1)│◄─┼─►│   WireGuard Mesh (10.42.0.2)│  │
    │  +-----------------------------+  │  +-----------------------------+  │
    │  |      Docker Engine API      |  │  |    Podman Libpod API (Root) |  │
    │  |   [App: Web, Nginx, DB]     |  │  |   [App: Worker, Cache]      |  │
    │  +-----------------------------+  │  +-----------------------------+  │
    └───────────────────────────────────┴───────────────────────────────────┘
```

---

## 2. Single-Binary Hybrid Model

Starry compiles into a single, self-contained Native AOT executable under 30 MB with zero external runtime requirements.

It operates in two complementary modes:

1. **CLI Mode (`starry <command>`)**:
   - Executes on the operator's workstation or within CI/CD automation pipelines.
   - Reads `starry.yaml`, decrypting sensitive tokens using `STARRY_MASTER_KEY`.
   - Inspects nodes via SSH, computes the dependency graph (DAG), and identifies configuration drift.
   - Orchestrates parallel declarative convergence without relying on a centralized control plane.

2. **Agent Mode (`starry agent run`)**:
   - Runs on fleet nodes as a supervised `systemd` daemon.
   - Manages local container lifecycles, healthchecks, and auto-restart policies.
   - Enforces atomic `nftables` rules and monitors WireGuard mesh health.
   - Dispatches telemetry and execution events.

---

## 3. WireGuard Full-Mesh Overlay Network

All cluster nodes participate in an encrypted private subnet (`10.42.0.0/16`):

- **Automated Routing:** Nodes receive predictable internal IP addresses (e.g. `10.42.0.1`, `10.42.0.2`). Peer exchange and WireGuard configurations are generated deterministically.
- **NAT Traversal:** Nodes behind NAT or cloud firewalls stay connected via `PersistentKeepalive = 25` and dynamic endpoint resolution.
- **Kernel-Level Encryption:** Inter-node communication (backend-to-database, internal RPCs) is secured with modern Noise / ChaCha20-Poly1305 cryptography directly in the Linux kernel.

---

## 4. Multi-Engine Containers: Docker & Podman

Starry transparently reconciles differences between container runtimes through an abstract interface:

| Capability | Docker Engine | Podman (Libpod) |
|---|---|---|
| **API Socket** | `/var/run/docker.sock` | `/run/podman/podman.sock` |
| **Rootless Support** | Experimental | Native out-of-the-box |
| **Primary Distros** | Ubuntu, Debian, Alpine | RHEL, AlmaLinux, Rocky, Fedora, Arch |
| **Architecture** | Daemon-based (`dockerd`) | Daemonless (fork/exec via conmon/crun) |

### P2P Image Streaming (Image Mesh)

When deploying or migrating services without access to an external container registry, Starry streams compressed container images (`zstd`) directly between peer nodes over the WireGuard mesh, preventing registry downtime from impacting fleet operations.

---

## 5. Atomic nftables Firewall & Deadman Switch

Host perimeter defense is handled by the modern `nftables` subsystem:

1. **Port Isolation:** Internal container ports are bound exclusively to the `wg0` interface. They remain completely invisible and unreachable from the public internet.
2. **Atomic Rulesets:** Rules are assembled and loaded in a single transaction via `nft -f`.
3. **Deadman Switch Rollback:** If an updated firewall ruleset inadvertently disrupts active SSH management connectivity, an automatic deadman countdown reverts to the previous valid configuration, preventing accidental node lockouts.

---

## 6. Secrets Management & Envelope Encryption

Sensitive credentials live directly within version control:

- **AES-256-GCM Envelope Encryption:** Each value is encrypted with a unique 96-bit initialization vector (IV) and a 128-bit authentication tag to prevent tampering.
- **Inline Secret Token:**
  ```text
  ENC[AES256_GCM,data:4gY3...,iv:9kP2...,tag:z0X8...]
  ```
- **Single-Command Master Key Rotation:**
  Running `starry rotate-secrets` decrypts existing values, generates a fresh 256-bit key, re-encrypts the manifest, and updates GitHub Actions repository secrets via the LibSodium REST API.
- **Automated Key Rotation:**
  Running `starry rotate-keys` replaces Ed25519 SSH keys and WireGuard keypairs across the fleet with connectivity pre-flight verification.

---

## 7. Declarative OS Maintenance & Rolling Drain

Starry maintains both application containers and host OS packages:

- **Package Managers:** Automatic detection of `/etc/os-release` and seamless interaction with `apt-get`, `dnf`, `apk`, and `pacman`.
- **Reboot Detection:** Proactive discovery of kernel update triggers (`/var/run/reboot-required` on Debian/Ubuntu, `needs-restarting -r` on RHEL/AlmaLinux).
- **Zero-Downtime Rolling Drain:**
  1. Route traffic away from the target node via DNS or load balancer.
  2. Gracefully terminate active workloads.
  3. Execute host `reboot`.
  4. Await SSH availability and verify agent health.
  5. Restore traffic and proceed to the next node.
