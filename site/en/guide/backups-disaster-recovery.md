# Streaming Backups (rclone), Volume Explorer & Fleet Bundles

Starry is engineered specifically for low-overhead operations on constrained VPS instances (such as servers with only 10–20 GB SSDs) with instant **Disaster Recovery (DR)** capabilities.

---

## 1. Zero-Disk Streaming Backups (`rclone rcat`)

Typical backup tools generate intermediate archive files (`.tar.gz`) locally on disk before uploading to cloud storage, risking `No space left on device` failures on production databases.

Starry streams backups entirely through in-memory Unix pipes:
```bash
tar -C /var/data/postgres -cf - . | zstd -3 | rclone rcat remote:backups/pg-20261101.tar.zst
```

### Key Advantages:
- **Zero Temporary Disk Consumption**: Volumes of any scale stream directly into object storage (AWS S3, Cloudflare R2, Backblaze B2, SFTP).
- **Zstandard Real-time Compression (`zstd -3`)**: Hardware-friendly compression maximizing network transfer speeds.
- **Direct Pipe Restoration**: `rclone cat ... | zstd -d | tar -C ... -xf -`.

---

## 2. Remote Volume Explorer (`starry volume open`)

To inspect or edit configuration assets and database dumps residing inside persistent container volumes:

```bash
starry volume open bocchi /var/lib/docker/volumes/app-data/_data
```

The command establishes an authenticated SSH/SFTP tunnel and outputs an actionable SFTP mount endpoint.

---

## 3. Disaster Recovery Capsule: `starry bundle`

In catastrophic situations where a VPS provider suffers a total outage, the `bundle` capsule reconstitutes the cluster on fresh servers in minutes.

### Exporting Fleet Capsule:
```bash
starry bundle export -o my-fleet.tar.gz
```
The resulting archive packages all modular configurations, encrypted secrets, and cluster metadata.

### Restoring Across New IP Addresses:
When re-provisioning on new virtual machines with different public IPs, pass `--override-ip`:

```bash
starry bundle import my-fleet.tar.gz \
  -o ./new-cluster \
  --override-ip bocchi=198.51.100.99 \
  --override-ip nijika=198.51.100.100
```

Running `starry apply` on the imported configuration regenerates the WireGuard mesh, adapts DNS, and deploys all services immediately!
