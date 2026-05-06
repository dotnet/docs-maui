---
description: |
  Picks up documentation issues created by the maui-labs pr-docs-check workflow.
  When an issue with '[maui-labs docs]' in the title is opened, this workflow reads
  the issue body (which contains the source PR details and suggested changes),
  writes the documentation updates, and creates a draft PR.

on:
  issues:
    types: [opened, labeled]

if: >-
  startsWith(github.event.issue.title, '[maui-labs docs]')
  && github.repository_owner == 'dotnet'

permissions:
  contents: read
  issues: read
  pull-requests: read

network:
  allowed:
    - defaults
    - github

tools:
  github:
    toolsets: [repos, issues, pull_requests]

safe-outputs:
  create-pull-request:
    title-prefix: "[maui-labs docs] "
    labels: [docs-from-code]
    draft: true
    fallback-as-issue: false
  add-comment:
    hide-older-comments: true
    discussions: false

timeout-minutes: 20
---

# Docs from Code — Create Draft PR

Read a `[maui-labs docs]` issue and create a draft PR with the documentation changes.

## Context

- **Issue Number**: `${{ github.event.issue.number }}`
- **Issue Title**: `${{ github.event.issue.title }}`
- **Repository**: `dotnet/docs-maui`

## Step 1: Read the Issue

Read the full issue body for issue #`${{ github.event.issue.number }}`. The issue
was created by an automated workflow on `dotnet/maui-labs` and contains:

- **Source PR** — link to the maui-labs PR that triggered this
- **Summary of Changes** — what user-facing changes were made
- **Documentation Pages Affected** — which files need updating
- **Suggested Changes** — specific text, code blocks, and sections to add or modify

Extract all of this information. If the issue body is empty or doesn't contain
a source PR link, comment on the issue explaining it's missing required information
and **stop**.

## Step 2: Browse Existing Documentation

Browse the documentation in this repository to understand the current state.
The developer-tools documentation lives under `docs/developer-tools/`:

**CLI documentation** (`docs/developer-tools/cli/`):
- `index.md` — .NET MAUI CLI overview
- `environment-diagnostics.md` — Environment diagnostics with `maui doctor`
- `android-management.md` — Android SDK and emulator management
- `device-management.md` — Device management

**DevFlow documentation** (`docs/developer-tools/devflow/`):
- `index.md` — DevFlow overview
- `visual-tree-screenshots.md` — Visual tree inspection and screenshots
- `element-interaction.md` — Element interaction and automation
- `blazor-cdp.md` — Blazor WebView debugging with CDP
- `mcp-server.md` — MCP server for AI agents
- `network-profiling.md` — Network monitoring and profiling
- `broker.md` — DevFlow broker architecture
- `setup-windows.md` — DevFlow Windows setup
- `setup-android.md` — DevFlow Android setup
- `setup-apple.md` — DevFlow Apple platforms setup

Also check:
- `docs/developer-tools/index.md` — Landing page
- `docs/TOC.yml` — Table of contents

Read the specific pages mentioned in the issue's "Documentation Pages Affected"
section to understand what's already documented.

## Step 3: Write Documentation Changes

Based on the issue's "Suggested Changes" section, make the actual file changes:

- **For updates to existing pages**: Edit the relevant `.md` files in place
- **For new pages**: Create new `.md` files in the appropriate directory

Follow these MS Learn documentation conventions:
- **Frontmatter**: Every page needs `title`, `description`, and `ms.date` (format: `MM/DD/YYYY`)
- **Headings**: Use `#` for the page title (must match frontmatter `title`), `##` for sections
- **Code blocks**: Use triple backticks with language identifier (e.g., `csharp`, `bash`, `xml`)
- **Notes/warnings**: Use `> [!NOTE]`, `> [!IMPORTANT]`, `> [!WARNING]`
- **Cross-references**: Use relative paths for internal links (e.g., `[DevFlow overview](../devflow/index.md)`)
- **TOC.yml**: If adding new pages, add an entry under the appropriate section with `name` and `href`
- **ms.date**: Set to today's date on any page you modify

## Step 4: Create Draft PR

Create a draft pull request with:

**Branch name**: Use `docs-from-code/<issue-number>` (e.g., `docs-from-code/3302`)

**Title**: A clear, concise title describing the documentation work
(the `[maui-labs docs]` prefix will be added automatically)

**Description** that includes:
- `Closes #<issue-number>` to auto-close the issue when merged
- A link to the source maui-labs PR (from the issue body)
- A summary of what documentation was added or changed
- A list of files modified or created

## Step 5: Comment on the Issue

Comment on the issue with:
- A message that a draft PR has been created
- A link to the draft PR
- A note that it needs human review before merging
