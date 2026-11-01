# Service Migrations (Blue-Green), P2P Images & DNS

Starry embeds an intelligent **DAG (Directed Acyclic Graph)** scheduler enabling zero-downtime service relocation across physical nodes, peer-to-peer image transfers without registry bottlenecks, and automated DNS synchronization.

---

## 1. Seamless Blue-Green Service Migration

When a service's target node changes in configuration (e.g. `web` moves from `bocchi` to `nijika`):

```yaml
services:
  web:
    image: "nginx:alpine"
    node: "nijika" # Was: bocchi
    vip: "10.99.10.5"
```

Running `starry plan` detects the relocation and constructs a **`BLUE-GREEN`** DAG step:

```bash
starry plan
```

### Execution Lifecycle:
1. **P2P Image Pre-delivery**: Node `nijika` streams the image directly from `bocchi` via `docker save | zstd | ssh docker load` without downloading layers from external registries.
2. **Mounting Configs & Volumes**: Configurations from `files` are written onto `nijika`.
3. **Start Green Container**: The new instance starts on `nijika`.
4. **Health Verification**: Starry probes the new container's `healthcheck` endpoint until green.
5. **Instant VIP Routing Cutover**: Once confirmed healthy, Starry updates WireGuard `AllowedIPs`: packets destined for `10.99.10.5` redirect to `nijika` **without terminating existing TCP connections**!
6. **Blue Container Tear-down**: The old container on `bocchi` is drained and stopped.

---

## 2. Peer-to-Peer Image Streaming (`docker save | zstd | ssh load`)

When private registries are inaccessible or remote nodes have bandwidth constraints, Starry streams layers peer-to-peer:
- Real-time compression using `zstd -3` for maximum throughput with minimal CPU overhead.
- Direct delivery through memory pipes over SSH without writing intermediate tar archives to disk.

---

## 3. Public DNS Providers (Cloudflare & Webhook)

Starry manages domain records automatically:

### Cloudflare (`IDnsProvider`)
Interacts with Cloudflare API v4 with automatic Zone ID discovery and optional edge proxying (`proxied: true`):
```yaml
dns:
  provider: "cloudflare"
  api_token: "ENC[AES256_GCM:...]"
  zone: "example.com"
```

### Webhook
Integrates with custom DNS infrastructure (CoreDNS, PowerDNS, Infoblox) by emitting `upsert` and `delete` JSON webhooks.
