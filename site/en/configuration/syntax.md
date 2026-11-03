# Configuration Architecture & Syntax (Starry v3.0)

In **Starry v3.0**, the configuration engine is powered by the pure functional library **`Starry.Dsl`** (written in F#). The entire cluster infrastructure is modeled as an immutable graph with whole-subtree substitution, object inheritance via `$:`, and inline secret interpolation.

---

## 1. Unified Config Graph AST & File Layout

Every file and directory within the configuration path is mounted into a single immutable AST:
- **Structured Files (`.yaml`, `.json`, `.toml`):** Parsed and mounted to their respective tree branches (e.g. `nodes/germany.yaml` -> `root.nodes.germany`).
- **Raw Text Files (`files/*`):** Text files (such as `files/nginx.conf` or `files/init.sql`) are mounted as `StringNode` objects under their path (`root.files.["nginx.conf"]`).
- **Custom Branches:** Users can organize arbitrary user branches (`presets`, `secrets`, `vars`, `dns_providers`, `backups`, `certs`) without schema rejection.

---

## 2. Graph References (`$`) & Object Inheritance (`$:`)

### Whole-Subtree Substitution
When a property references a graph path (e.g. `env: "$presets.db_env"`), Starry replaces the reference with a deep copy of the resolved subtree:

```yaml
presets:
  db_env:
    POSTGRES_DB: "starry"
    POSTGRES_USER: "admin"
    POSTGRES_PASSWORD: "ENC[AES256_GCM:v1:...]"

services:
  database:
    image: "postgres:16"
    env: "$presets.db_env"
```

### Inheritance & Merging (`$:`)
Using the special `"$"` key, a service or node can inherit and merge properties from one or more base presets:

```yaml
presets:
  base_service:
    restart: "always"
    priority: 50
    networks: ["$networks.app_net"]

services:
  web:
    $: "$presets.base_service"
    image: "nginx:alpine"
    priority: 80 # Overrides priority from base_service
```

### String & File Interpolation
All strings and text files support `$path.to.value` and `${path.to.value}` interpolation, with automatic secret decryption during deployment.

---

## 3. IPv6 Mesh (`fd67:b00b::/32`) & Monotonic ID Ledger

* **Cluster ULA Prefix:** `fd67:b00b::/32`.
* **Host Subnet (`/48`):** Each node is assigned `fd67:b00b:<host_id>::/48`. WireGuard interface `starry0` has address `fd67:b00b:<host_id>::1/32`, and peers route `AllowedIPs = fd67:b00b:<host_id>::/48` statically.
* **Container Replica Subnet (`/64`):** Each container replica gets a dedicated `/64` subnet `fd67:b00b:<host_id>:<container_id>::/64` with a single interface `eth0` bound to `fd67:b00b:<host_id>:<container_id>::1/64`.
* **Tombstone Ledger (`.starry/id-ledger.json`):** Released host and container IDs are permanently tombstoned and never reused.

---

## 4. Named Bidirectional Routing (`ports:`)

```yaml
ports:
  shop_https_v4:
    from: "$nodes.germany.ipv4:443"
    to: "$ingress:443"
    proto: "tcp"
    bidirectional: true # Inbound DNAT + symmetric outbound SNAT!

  custom_proxy:
    from: "$nodes.finland.ipv4:443"
    to: "my-proxy:443"
```

---

## 5. Managed Ingress (`$ingress`) & L4 SNI Proxying

* `$ingress` binds internally to `fd67:b00b:<host_id>:ffff::1` and never intercepts port 443 without explicit rules in `ports:`.
* Routing uses Nginx `stream { ssl_preread on; }`:
  - `sni: "<target>"` passes TLS traffic through at Layer 4 without certificate termination.
  - `http: "<target>"` routes internally to `http {}` for SSL termination and reverse proxying.

---

## 6. Hierarchical Domains (`domains:`) & Cloudflare Origin Rules

```yaml
domains:
  rocks:
    bocchi:
      dns_provider: "$dns_providers.cf"
      cert: "$certs.bocchi_wildcard"
      node: "$nodes.germany"

      "@":
        http: "landing:80"
      shop:
        http: "shop-web:3000"
      vpn:
        sni: "xray:8443"
        proxy_protocol: true
```

When an ingress entry port is non-standard (e.g. 8443), Starry automatically programs Cloudflare Origin Rules via the Rulesets API to rewrite origin ports transparently!
