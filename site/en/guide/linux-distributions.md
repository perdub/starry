# Linux Distributions & NAT Traversal

Starry is built to orchestrate heterogeneous infrastructure fleets: cluster nodes can run different Linux distributions and live behind Carrier-Grade NAT (CGNAT) or on public static IP addresses.

---

## 1. Distribution Compatibility Matrix

| Distribution Family | Package Manager | Supported Distributions | Notes |
|---|---|---|---|
| **Debian / Ubuntu** | `apt` | Ubuntu 22.04+, 24.04+, Debian 11, 12, Pop!_OS | Automated `/var/run/reboot-required` inspection |
| **RHEL / Enterprise Linux** | `dnf` | AlmaLinux 9+, Rocky Linux 9+, Fedora 39+, RHEL 8, 9, CentOS Stream | Reboot detection via `needs-restarting -r` |
| **Alpine Linux** | `apk` | Alpine 3.18, 3.19, 3.20+ | Ultra-lightweight nodes with `musl libc` |
| **Arch Linux** | `pacman` | Arch Linux, Manjaro, EndeavourOS | Bleeding-edge rolling release and kernel packages |

Starry automatically queries `/etc/os-release` during initial node bootstrap and selects the matching `IOsPackageManager`.

---

## 2. Connecting to Nodes Behind NAT via Jump Hosts

For server instances without a public IPv4 address, Starry supports transparent SSH jump-proxying (`-J` / `ProxyJump`):

```yaml
nodes:
  bastion:
    host: "203.0.113.1"
    port: 22
    is_public: true
    ssh_key: "ENC[AES256_GCM:v1:nonce:cipher:tag]"

  internal-worker:
    host: "10.0.0.50"
    port: 22
    jump_host: "root@203.0.113.1:22"
    vpn_ip: "10.99.0.2"
    ssh_key: "ENC[AES256_GCM:v1:nonce:cipher:tag]"
```

When contacting `internal-worker`, Starry routes through `bastion` using OpenSSH ControlMaster connection reuse for near-zero latency command execution.

---

## 3. Rolling Security Upgrades

To maintain fleet security without downtime, Starry executes rolling upgrades across nodes:

```bash
starry nodes upgrade --rolling
```

### Rolling Upgrade Lifecycle:
1. Connect to target node via SSH and resolve package manager.
2. Apply OS security patches non-interactively.
3. Query if kernel or libc update requires a restart (`IsRebootRequiredAsync`).
4. If a reboot is required:
   - Issue graceful `reboot` command.
   - Wait for connection termination.
   - Poll until SSH listener is restored.
   - Verify node health before proceeding.
5. Move on to the next node in the fleet!
