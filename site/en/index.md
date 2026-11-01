---
layout: home

hero:
  name: "Starry"
  text: "Decentralized GitOps Orchestrator"
  tagline: "Declarative orchestration for heterogeneous VPS fleets, Docker & Podman containers, WireGuard mesh networks, and nftables firewalls."
  image:
    src: /logo.svg
    alt: Starry Logo
  actions:
    - theme: brand
      text: Getting Started
      link: /en/guide/getting-started
    - theme: alt
      text: Architecture Overview
      link: /en/architecture/overview
    - theme: alt
      text: Config Syntax
      link: /en/configuration/syntax

features:
  - icon: ⚡
    title: Single-Binary Hybrid
    details: Zero-dependency ultra-lightweight binary. Runs as an interactive CLI on your workstation and as a high-performance background agent on target servers.
  - icon: 🌐
    title: WireGuard Full-Mesh
    details: Automatic encrypted full-mesh overlay network across 10.42.0.0/16, seamless NAT-to-NAT traversal, and automatic persistent keepalive.
  - icon: 🐳
    title: Docker & Podman Ready
    details: Unified engine abstraction over Docker Engine API and Podman Libpod/Compat API with rootless mode support and peer-to-peer image transfers.
  - icon: 🛡️
    title: Atomic nftables Firewall
    details: Strict automated nftables generation. Expose service ports exclusively over the encrypted WireGuard mesh with automated Deadman Switch safety rollbacks.
  - icon: 🔐
    title: Git-Native Encrypted Secrets
    details: Built-in envelope encryption via AES-256-GCM. Store ENC[...] tokens safely in git with one-click master key rotation and GitHub Secrets integration.
  - icon: 🔄
    title: OS Upgrades & Rolling Drain
    details: Declarative package management (apt, dnf, apk, pacman) and zero-downtime kernel reboot orchestration with proactive traffic draining.
---

<div class="starry-callout">
  <div class="starry-callout-title">
    🌟 Masterless, Zero Single Point of Failure
  </div>
  <p>
    Unlike bulky cluster orchestrators, Starry requires no dedicated master nodes, no heavy etcd consensus, and no external runtime databases. The entire state of your fleet lives declaratively in a readable YAML file, converging reliably across any modern Linux distribution.
  </p>
</div>
