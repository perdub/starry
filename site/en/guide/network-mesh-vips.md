# WireGuard Mesh, Roaming VIPs & Nftables

Starry connects all cluster nodes into an encrypted, decentralized peer-to-peer network using **WireGuard** (`starry0`). The overlay traverses NAT and firewalls reliably via automated `PersistentKeepalive = 25` keepalive pulses.

---

## 1. Network Topology & NAT Traversal

In classic setups, servers behind NAT are inaccessible. With Starry:
- Nodes with static public addresses (`is_public: true`) publish their external `Endpoint`.
- Nodes behind NAT establish outbound links to public peers and transmit keepalive packets every 25 seconds, sustaining state in stateful firewall translation tables.
- Public peers act as transit relays between NAT-isolated hosts.

```text
[ VPS-1: Public IP (is_public: true) ]
   ▲                            ▲
   │ WireGuard                  │ WireGuard
   │ Keepalive=25               │ Keepalive=25
   ▼                            ▼
[ VPS-2: Behind Carrier NAT ]    [ VPS-3: Behind Corporate NAT ]
```

---

## 2. Container Roaming VIPs (10.99.x.x)

Each orchestrated service is allocated a static Virtual IP within the overlay subnet (`10.99.x.x`).

### How Roaming VIPs Function:
1. When a service is deployed on node `bocchi`, Starry includes its VIP (e.g. `10.99.10.5/32`) into `AllowedIPs` for `bocchi` across all peer WireGuard configurations.
2. Kernel packet routing delivers traffic aimed at `10.99.10.5` through `starry0` straight to `bocchi`.
3. When the service migrates from `bocchi` to `nijika`, Starry reconfigures `AllowedIPs` (`wg syncconf`). Packets immediately steer to `nijika` **without altering the service IP**!

### Integration with Nginx Proxy Manager (NPM) / Ingress Proxies
Any reverse proxy (NPM, Traefik, Caddy) situated on an ingress node can proxy to `http://10.99.10.5:8080`. The proxy does not need to track which physical host currently runs the container.

---

## 3. Embedded Internal DNS (*.internal)

The Starry node daemon hosts an embedded lightweight DNS server on port 53:
- Query `api.internal` -> `10.99.10.5`
- Query `database.internal` -> `10.99.0.2`

Containers leverage the local Starry daemon as their primary nameserver, enabling resilient service discovery.

---

## 4. Host Firewall & Port Forwarding with `nftables` (`table inet starry`)

Starry operates strictly inside an isolated `table inet starry` table to avoid collisions with Docker iptables shims:
- `chain input`: Default `drop` policy.
- Permitted: Loopback `lo`, WireGuard mesh `starry0`, inbound SSH, and WireGuard UDP port 51820.
- `chain prerouting`: DNAT mapping external host IPv4/IPv6 ports to internal Container VIPs.
- `chain postrouting`: Masquerading rules for correct asymmetric routing return paths.
