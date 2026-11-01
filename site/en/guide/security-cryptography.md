# Security, Cryptography & GitOps

Starry provides a zero-trust security model: cluster configuration files can be stored safely even in **public Git repositories**, because all sensitive values are authenticated and encrypted using **AES-256-GCM** with contextual **AAD (Additional Authenticated Data)** binding.

---

## 1. Contextual AAD Binding: Protection Against Cut-and-Paste Attacks

A known vulnerability with encrypted secrets stored in Git is cut-and-paste manipulation: an attacker or careless operator moving an encrypted ciphertext from one property to another (e.g. copying a root SSH key into an unprivileged environment variable).

In Starry, every value is cryptographically tied to its **canonical JSON path**:
- The secret for `nodes.bocchi.ssh_key` uses `AAD = "nodes.bocchi.ssh_key"`
- If this ciphertext is pasted into `services.api.db_password`, decryption fails immediately with `CryptographicException` because the authentication tag does not match!

---

## 2. Secret Formats in Configuration

### Open Secrets (`PLAIN[...]`)
When drafting changes, developers can write raw secrets using the `PLAIN[...]` marker:
```yaml
nodes:
  bocchi:
    host: "192.168.1.10"
    ssh_key: "PLAIN[-----BEGIN OPENSSH PRIVATE KEY-----...]"
```

Running `starry encrypt` automatically finds all plain secrets, encrypts them in place, and updates the files:
```bash
starry encrypt --key $STARRY_MASTER_KEY ./cluster
```

### Encrypted Secrets (`ENC[...]`)
The resulting encrypted secret is formatted as an authenticated envelope:
```text
ENC[AES256_GCM:v1:<base64_nonce>:<base64_cipher>:<base64_tag>]
```

---

## 3. Automated Monthly Master Key Rotation via GitHub Actions

To adhere to best security hygiene, master keys should be rotated periodically (e.g. monthly).

Starry accomplishes full fleet re-encryption with a single command:
```bash
starry encrypt global --reencrypt \
  --old-key-env STARRY_MASTER_KEY_OLD \
  --new-key-env STARRY_MASTER_KEY_NEW \
  --allow-config-edit \
  --git-commit \
  --git-push \
  --update-github-secret STARRY_MASTER_KEY
```

This command:
1. Discovers all configuration files (`.yaml`, `.json`, `.toml`).
2. Decrypts with the old key, checking AAD on every field.
3. Encrypts with the new key using a fresh cryptographic nonce per field.
4. Atomically rewrites all config files using safe temporary file renames.
5. Commits changes to Git and pushes to the remote branch.
6. Updates the `STARRY_MASTER_KEY` secret in GitHub Actions using `gh secret set`.
