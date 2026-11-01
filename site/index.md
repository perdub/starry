---
layout: home

hero:
  name: "Starry"
  text: "Децентрализованный GitOps-оркестратор"
  tagline: "Декларативное управление гетерогенным парком VPS, контейнерами Docker и Podman, оверлейной сетью WireGuard и nftables файрволом."
  image:
    src: /logo.svg
    alt: Starry Logo
  actions:
    - theme: brand
      text: Быстрый старт
      link: /guide/getting-started
    - theme: alt
      text: Обзор архитектуры
      link: /architecture/overview
    - theme: alt
      text: Синтаксис конфига
      link: /configuration/syntax

features:
  - icon: ⚡
    title: Single-Binary Hybrid
    details: Единый легковесный бинарник без внешних зависимостей. Работает как интерактивный CLI на рабочей машине разработчика и как автономный фоновый агент на серверах.
  - icon: 🌐
    title: WireGuard Full-Mesh
    details: Автоматическое построение шифрованной полносвязной оверлейной сети 10.42.0.0/16 между узлами, безопасное преодоление NAT и динамический keepalive.
  - icon: 🐳
    title: Docker & Podman Ready
    details: Абстракция над Docker Engine API и Podman Libpod/Compat API. Поддержка Rootless-режима, смешанных сред и прямого P2P-стриминга образов между нодами.
  - icon: 🛡️
    title: Атомарный nftables Файрвол
    details: Автоматическая генерация строгих правил nftables. Порты сервисов открыты строго внутри WireGuard-меша, а механизм Deadman Switch исключает потерю доступа к VPS.
  - icon: 🔐
    title: Безопасные Секреты в Git
    details: Встроенное конвертное шифрование AES-256-GCM. Токены ENC[...] хранятся прямо в репозитории с поддержкой одномоментной ротации ключей и интеграцией с GitHub Secrets.
  - icon: 🔄
    title: Rolling Drain Обновления ОС
    details: Декларативное сопровождение системных пакетов (apt, dnf, apk, pacman) и автоматическая перезагрузка ядра Linux со скользящим дренажом трафика без даунтайма.
---

<div class="starry-callout">
  <div class="starry-callout-title">
    🌟 Инфраструктура без единой точки отказа
  </div>
  <p>
    В отличие от тяжеловесных кластерных решений, Starry не требует управляющего мастера, распределённого etcd-консенсуса или постоянных внешних баз данных. Вся топология кластера и желаемое состояние описываются в легко читаемом YAML-файле и гарантируют воспроизводимость на любом Linux-дистрибутиве.
  </p>
</div>
