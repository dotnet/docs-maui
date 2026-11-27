---
title: ".NET MAUI agent skills marketplace"
description: "Learn how to install .NET MAUI agent skills from the MAUI Labs marketplace or with the .NET MAUI CLI."
ms.date: 11/27/2026
ai-usage: ai-generated
---

# .NET MAUI agent skills marketplace

The [MAUI Labs marketplace](https://github.com/dotnet/maui-labs/tree/main/plugins) provides agent skills for .NET MAUI development. A skill contains instructions and can include supporting resources that an AI coding agent can use for a task. You can install a complete plugin through your agent client, or install individual skills in a project with the .NET MAUI CLI.

> [!IMPORTANT]
> MAUI Labs tools and skills are experimental and might change between releases.

## Choose a plugin

| Plugin | Skills |
|---|---|
| `dotnet-maui` | App development, including architecture, UI, accessibility, testing, performance, platform integration, and migration. |
| `dotnet-maui-tooling` | DevFlow onboarding, debugging, and automation, platform bindings, workload discovery, and diagnostics. |

The DevFlow skills are in `dotnet-maui-tooling`. If you previously installed `dotnet-maui` for DevFlow skills, install the tooling plugin to continue using them. The `maui devflow init` command is another way to install the DevFlow skills bundled with the CLI; it doesn't install a marketplace plugin.

For the full skill list, see the [marketplace catalog](https://github.com/dotnet/maui-labs/blob/main/plugins/README.md).

## Install a marketplace plugin

Plugin installation and updates are managed by your agent client, separately from the .NET MAUI CLI. Installing a plugin doesn't configure the DevFlow Model Context Protocol (MCP) server.

### GitHub Copilot CLI

In an interactive GitHub Copilot CLI session, add the marketplace:

```text
/plugin marketplace add dotnet/maui-labs
```

Install the app-development plugin:

```text
/plugin install dotnet-maui@dotnet-maui-labs
```

For DevFlow and other tooling skills, install the tooling plugin:

```text
/plugin install dotnet-maui-tooling@dotnet-maui-labs
```

Install one or both plugins as needed. Review the plugin before installing it, and reload the agent session if needed.

> [!NOTE]
> Claude Code can't add `dotnet/maui-labs` as a marketplace by repository name because it looks for `.claude-plugin/marketplace.json`, which the repository doesn't currently provide. To install individual skills for Claude Code, use the .NET MAUI CLI with `--env Claude` as described later in this article.

### Visual Studio Code

Enable the `chat.plugins.enabled` setting if plugin support is disabled.

1. Open the Command Palette and run **Chat: Manage Plugin Marketplaces**.
1. Select **Add Marketplace...**, and enter `dotnet/maui-labs`.
1. Open **Customize** from the Chat side panel, find `dotnet-maui` or `dotnet-maui-tooling` on the **Browse** page, and select **Install**.

For client-specific plugin management, see the [GitHub Copilot CLI](https://docs.github.com/copilot/how-tos/copilot-cli/customize-copilot/plugins-finding-installing) and [Visual Studio Code](https://docs.github.com/copilot/how-tos/copilot-in-your-ide/customize-copilot/extend-copilot-with-tools-and-context/using-plugins) documentation.

## Install individual skills with the .NET MAUI CLI

[Install the .NET MAUI CLI](cli/index.md#install-the-cli), and run the following commands from your project directory. This example lists the skills, installs `maui-performance` for Visual Studio Code, checks the installed skills, and updates that skill:

```bash
maui ai list skill --env VsCode
maui ai add skill maui-performance --env VsCode --yes
maui ai status skill --env VsCode
maui ai update skill --skill maui-performance --env VsCode --yes
```

Replace `VsCode` with `CopilotCli`, `Claude`, or `OpenCode` for your client. Within a Git repository, the CLI uses the Git root as the project boundary and honors detected nested skill directories. Outside Git, it uses the current directory. The command output shows the destination. For a typical Visual Studio Code project, the example installs the skill under `.github/skills/maui-performance/`. Start or reload your agent session if needed, and ask it to review your app's performance. Review and test any changes it proposes.

The CLI updates only existing skills it manages. It doesn't restore missing skills or adopt unmanaged skills during an update. Use `maui ai add skill` to install a missing skill. Adding a skill doesn't install a plugin or an agent, or configure MCP.

Catalog operations access GitHub. If you encounter an API rate limit, the CLI can use a GitHub token supplied in the `GITHUB_TOKEN` environment variable.

### Set up recommended assets

Alternatively, `maui ai init` can install recommended skills and a compatible agent, and register the DevFlow MCP server. Preview the changes before applying them:

```bash
maui ai init --env VsCode --dry-run
maui ai init --env VsCode --yes
```

The `--yes` option accepts the confirmation prompt but doesn't authorize overwriting conflicting content. Review the destinations in the preview: for `CopilotCli`, the MCP registration is user-wide in `~/.copilot/mcp-config.json`; the other clients use project-scoped MCP configuration. Registration doesn't start the server, approve the client connection, or verify connectivity.

To use DevFlow with a running app, see [DevFlow overview](devflow/index.md) and [MCP server for AI agents](devflow/mcp-server.md). To contribute a skill, see the [contribution guide](https://github.com/dotnet/maui-labs/blob/main/plugins/CONTRIBUTING.md).
