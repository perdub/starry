import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'Starry',
  head: [
    ['link', { rel: 'icon', type: 'image/svg+xml', href: '/logo.svg' }],
    ['meta', { name: 'theme-color', content: '#f59e0b' }]
  ],
  locales: {
    root: {
      label: 'Русский',
      lang: 'ru-RU',
      title: 'Starry',
      description: 'Децентрализованный GitOps-оркестратор',
      themeConfig: {
        nav: [
          { text: 'Быстрый старт', link: '/guide/getting-started' },
          { text: 'Безопасность', link: '/guide/security-cryptography' },
          { text: 'Архитектура', link: '/architecture/overview' },
          { text: 'Конфигурация', link: '/configuration/syntax' },
          { text: 'Команды CLI', link: '/cli/commands' }
        ],
        sidebar: [
          {
            text: 'Руководство',
            items: [
              { text: 'Быстрый старт', link: '/guide/getting-started' },
              { text: 'Бэкапы и Bundle (DR)', link: '/guide/backups-disaster-recovery' },
              { text: 'Миграции и DNS (P2P)', link: '/guide/migrations-dns-p2p' },
              { text: 'Docker и Podman (files)', link: '/guide/containers-docker-podman' },
              { text: 'Сетевой меш и VIP', link: '/guide/network-mesh-vips' },
              { text: 'Дистрибутивы Linux и NAT', link: '/guide/linux-distributions' },
              { text: 'Безопасность и секреты', link: '/guide/security-cryptography' }
            ]
          },
          {
            text: 'Архитектура',
            items: [
              { text: 'Обзор архитектуры', link: '/architecture/overview' }
            ]
          },
          {
            text: 'Конфигурация',
            items: [
              { text: 'Синтаксис и схема', link: '/configuration/syntax' }
            ]
          },
          {
            text: 'Справочник CLI',
            items: [
              { text: 'Команды CLI', link: '/cli/commands' }
            ]
          }
        ],
        docFooter: {
          prev: 'Предыдущая страница',
          next: 'Следующая страница'
        },
        outline: {
          label: 'Содержание страницы'
        },
        darkModeSwitchLabel: 'Оформление',
        lightModeSwitchTitle: 'Переключить на светлую тему',
        darkModeSwitchTitle: 'Переключить на тёмную тему',
        returnToTopLabel: 'Наверх'
      }
    },
    en: {
      label: 'English',
      lang: 'en-US',
      link: '/en/',
      title: 'Starry',
      description: 'Decentralized GitOps Orchestrator',
      themeConfig: {
        nav: [
          { text: 'Getting Started', link: '/en/guide/getting-started' },
          { text: 'Security', link: '/en/guide/security-cryptography' },
          { text: 'Architecture', link: '/en/architecture/overview' },
          { text: 'Configuration', link: '/en/configuration/syntax' },
          { text: 'CLI Commands', link: '/en/cli/commands' }
        ],
        sidebar: [
          {
            text: 'Guide',
            items: [
              { text: 'Getting Started', link: '/en/guide/getting-started' },
              { text: 'Backups & DR Bundles', link: '/en/guide/backups-disaster-recovery' },
              { text: 'Migrations & DNS (P2P)', link: '/en/guide/migrations-dns-p2p' },
              { text: 'Docker & Podman (files)', link: '/en/guide/containers-docker-podman' },
              { text: 'WireGuard Mesh & VIPs', link: '/en/guide/network-mesh-vips' },
              { text: 'Linux Distros & NAT', link: '/en/guide/linux-distributions' },
              { text: 'Security & Secrets', link: '/en/guide/security-cryptography' }
            ]
          },
          {
            text: 'Architecture',
            items: [
              { text: 'Overview', link: '/en/architecture/overview' }
            ]
          },
          {
            text: 'Configuration',
            items: [
              { text: 'Syntax & Schema', link: '/en/configuration/syntax' }
            ]
          },
          {
            text: 'CLI Reference',
            items: [
              { text: 'Commands', link: '/en/cli/commands' }
            ]
          }
        ],
        docFooter: {
          prev: 'Previous page',
          next: 'Next page'
        },
        outline: {
          label: 'On this page'
        },
        returnToTopLabel: 'Return to top'
      }
    }
  },
  themeConfig: {
    logo: '/logo.svg',
    socialLinks: [
      { icon: 'github', link: 'https://github.com/starry-project/starry' }
    ],
    search: {
      provider: 'local',
      options: {
        locales: {
          root: {
            translations: {
              button: {
                buttonText: 'Поиск',
                buttonAriaLabel: 'Поиск по документации'
              },
              modal: {
                noResultsText: 'Ничего не найдено по запросу',
                resetButtonTitle: 'Очистить поиск',
                footer: {
                  selectText: 'выбрать',
                  navigateText: 'перейти',
                  closeText: 'закрыть'
                }
              }
            }
          },
          en: {
            translations: {
              button: {
                buttonText: 'Search',
                buttonAriaLabel: 'Search documentation'
              },
              modal: {
                noResultsText: 'No results found',
                resetButtonTitle: 'Reset search',
                footer: {
                  selectText: 'to select',
                  navigateText: 'to navigate',
                  closeText: 'to close'
                }
              }
            }
          }
        }
      }
    },
    footer: {
      message: 'Released under the MIT License.',
      copyright: 'Copyright © 2026 Starry Project'
    }
  }
})
