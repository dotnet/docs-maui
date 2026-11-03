---
title: ".NET Agent Skills for .NET MAUI development"
description: "Learn how to install and use .NET Agent Skills plugins to extend AI coding assistants with specialized .NET MAUI knowledge."
ms.topic: how-to
ms.date: 03/22/2026
no-loc: [".NET MAUI"]
---

# .NET Agent Skills for .NET MAUI development

Agent skills are pre-built capabilities that extend AI coding assistants with specialized .NET knowledge. Rather than relying on general-purpose AI models alone, skills provide curated, domain-specific guidance that helps your AI assistant understand .NET MAUI project structures, platform requirements, build systems, and common troubleshooting patterns.

The [.NET Agent Skills marketplace](https://github.com/dotnet/skills) is a collection of skill plugins maintained by the .NET team and community. It provides plugins for .NET MAUI development, diagnostics, builds, testing, and more. Each plugin targets a specific area of .NET development and can be installed independently.

## Available skills for .NET MAUI developers

The following table lists the skill plugins most relevant to .NET MAUI development:

| Plugin | Description |
|--------|-------------|
| `dotnet-maui` | Environment setup, diagnostics, and troubleshooting for .NET MAUI |
| `dotnet` | Core .NET development skills |
| `dotnet-msbuild` | MSBuild and build system skills |
| `dotnet-test` | Test execution and diagnostics |
| `dotnet-nuget` | NuGet package management |
| `dotnet-ai` | AI/ML integration including LLMs, RAG, and MCP |
| `dotnet-diag` | Performance profiling, debugging, and incident analysis |
| `dotnet-upgrade` | Migration and framework upgrading |

> [!TIP]
> Start with the `dotnet-maui` and `dotnet` plugins. These two cover the most common scenarios for .NET MAUI app development. Add other plugins as your workflow requires them.

## Install a skill plugin

You can install skill plugins through several methods depending on your development environment.

### GitHub Copilot CLI

If you're using the GitHub Copilot CLI agent, add the .NET skills marketplace and then install the desired plugin:

```bash
/plugin marketplace add dotnet-dnceng
/plugin install dotnet-maui
```

> [!NOTE]
> `dotnet-dnceng` is the marketplace organization name that hosts the official .NET skill plugins. Adding it registers the source so you can install individual plugins like `dotnet-maui` by name.

To verify the plugin is installed:

```bash
/plugin list
```

### Visual Studio Code

To install skill plugins in Visual Studio Code:

1. Open the **Settings** editor (**Ctrl+,** on Windows/Linux, **Cmd+,** on macOS).
1. Search for `chat.plugins.enabled` and enable the setting.
1. Open the **Extensions** view and search for the desired skill plugin in the marketplace.
1. Select **Install** on the plugin.

> [!NOTE]
> The `chat.plugins.enabled` setting must be turned on before skill plugins can be discovered and used by Copilot Chat in Visual Studio Code.

### Repository-level configuration

You can enable skill plugins for everyone who uses Copilot in a specific repository by adding a `.github/copilot/settings.json` file:

```json
{
  "plugins": {
    "dotnet-dnceng@dotnet-maui": {
      "enabled": true
    }
  }
}
```

This configuration automatically enables the `dotnet-maui` skill plugin for any contributor using Copilot in the repository. Commit this file to your repository so all team members benefit from it.

> [!IMPORTANT]
> Repository-level configuration only *enables* a plugin — it does not install it automatically. Contributors must install the plugin in their own environment before it takes effect. Pair this configuration with onboarding documentation (such as a `README.md` or `CONTRIBUTING.md` setup guide) that lists the required plugins and installation steps. For team-wide consistency, consider using [devcontainers](https://containers.dev/) or repository setup scripts (for example, a `setup.sh` or `init.ps1`) that automate plugin installation as part of the development environment setup.

> [!TIP]
> When contributing to an open-source repository, check whether maintainers recommend specific skills — for example in an `AGENTS.md` file or contributing guide — and install them to match the expected development environment.

### Codex CLI

If you're using the Codex CLI, install a skill plugin with the `skill-installer` command:

```bash
skill-installer install dotnet-maui
```

To install multiple plugins at once:

```bash
skill-installer install dotnet-maui dotnet-test dotnet-msbuild
```

## Use the dotnet-maui skill

Once installed, the `dotnet-maui` skill plugin enhances your AI coding assistant with .NET MAUI-specific capabilities. Your AI coding assistant automatically selects relevant skills based on the context of your question. You don't need to invoke skills explicitly — the assistant matches your query to available skill capabilities and activates the appropriate plugins.

### Environment setup validation

The skill can validate your development environment, including:

- .NET SDK version and installation status
- .NET MAUI workload installation
- Android SDK and emulator configuration
- Xcode and iOS simulator setup (macOS)
- Windows App SDK configuration

Ask your AI assistant to check your environment:

```text
Check if my environment is set up correctly for .NET MAUI development.
```

### Build error diagnostics

When you encounter build errors, the skill provides targeted guidance based on common .NET MAUI build issues:

```text
I'm getting error XC0000 when building for iOS. What does this mean?
```

The skill draws from known error patterns and platform-specific build requirements to suggest fixes.

#### Example: Diagnosing an Android build failure

A common scenario is an Android build that fails without a clear error message. Ask your AI assistant:

```text
Why is my Android build failing?
```

With the `dotnet-maui` skill active, the assistant can check for missing .NET workloads, verify your Android SDK version meets the project's target, confirm the Android emulator is configured correctly, and suggest the specific `dotnet workload install` or SDK manager commands needed to fix the issue. Without the skill, the assistant would only offer generic troubleshooting advice.

### Platform-specific troubleshooting

The skill understands the differences between Android, iOS, macOS, and Windows platform targets. It can help with:

- Platform-specific API usage and conditional compilation
- Manifest and entitlement configuration
- Signing and provisioning profiles
- Deployment and debugging on physical devices

### Project template scaffolding

Use the skill to generate new project structures that follow .NET MAUI best practices:

```text
Create a new .NET MAUI app with Shell navigation and MVVM architecture.
```

### Migration assistance

The skill can help with migrating from:

- Xamarin.Forms to .NET MAUI
- Older .NET MAUI versions to newer ones
- Platform-specific legacy code to cross-platform .NET MAUI patterns

## Combine multiple skills

You can install multiple skill plugins to cover different aspects of your development workflow. The AI assistant draws from all installed skills as needed.

### Example: Full-stack .NET MAUI workflow

Install a set of complementary plugins:

```bash
/plugin install dotnet-maui
/plugin install dotnet-test
/plugin install dotnet-msbuild
/plugin install dotnet-nuget
```

With these plugins active, your AI assistant can help across a typical development cycle:

1. **Build issues** — The `dotnet-msbuild` skill helps diagnose MSBuild targets, property configurations, and build order problems.
1. **Test failures** — The `dotnet-test` skill assists with test execution, diagnosing flaky tests, and improving test coverage.
1. **Package management** — The `dotnet-nuget` skill helps resolve NuGet dependency conflicts, find appropriate packages, and manage package sources.
1. **App-specific issues** — The `dotnet-maui` skill handles platform configuration, XAML issues, and .NET MAUI-specific patterns.

> [!TIP]
> Skills don't conflict with each other. Install as many as are relevant to your project. The AI assistant selects the most appropriate skill based on the context of your question.

#### Team standardization

For team projects, consider documenting your baseline set of plugins in a `CONTRIBUTING.md` or onboarding guide. This ensures every team member has a consistent AI-assisted development experience and avoids gaps when different developers have different plugins installed.

### Example: Debugging a performance issue

Combine `dotnet-maui` with `dotnet-diag` to troubleshoot performance problems:

```text
My .NET MAUI app is slow to start on Android. Help me profile the startup
and identify the bottleneck.
```

The `dotnet-diag` skill provides profiling and tracing guidance while the `dotnet-maui` skill offers platform-specific optimization strategies.

## Create custom skills

Teams can create their own skill plugins to codify internal patterns, architectural decisions, and domain-specific knowledge. Custom skills are useful when your team has:

- Established coding conventions specific to your .NET MAUI projects
- Internal libraries or frameworks that require specialized guidance
- Domain-specific patterns that general-purpose skills don't cover

To get started with creating a custom skill, see the [contributing guide in the .NET Skills repository](https://github.com/dotnet/skills/blob/main/CONTRIBUTING.md).

A custom skill plugin typically includes:

- A skill manifest that describes the plugin's capabilities
- Prompt instructions that guide the AI assistant's behavior
- Optional tools or scripts that the AI assistant can invoke

> [!NOTE]
> Custom skills follow the same plugin format as the official .NET skills. You can distribute them through your organization's internal registries or through the public marketplace.

## How skills complement instruction files

Skills and instruction files serve different roles in your AI-assisted development workflow and work best when used together:

- **Instruction files** provide *static context* — they describe your project structure, coding conventions, architectural patterns, and platform-specific guidelines. The AI assistant reads these files to understand *how* your project is organized and *what* conventions to follow.
- **Skills** provide *dynamic capabilities* — they enable the AI assistant to perform actions like running diagnostics, analyzing build errors, scaffolding code from templates, and profiling performance. Skills give the assistant the ability to *do* things, not just know things.
- **[MauiDevFlow](dev-flow.md)** provides *runtime inspection* — it connects the AI assistant to your running app so it can query the live visual tree, capture screenshots, read DOM snapshots from Blazor WebViews, and check logs. DevFlow gives the assistant visibility into actual runtime state, which neither instruction files nor skills can provide.

For example, an instruction file might tell the AI assistant to use compiled bindings in XAML, the `dotnet-maui` skill enables the assistant to validate your development environment and diagnose why a build targeting iOS is failing, and DevFlow lets the assistant inspect your running app's visual tree to identify why a layout renders differently than expected. Together, they give the AI the context, the capabilities, and the runtime visibility to assist effectively.

## See also

- [Write a copilot-instructions.md file for .NET MAUI projects](copilot-instructions.md)
- [AI-assisted UI debugging with MauiDevFlow](dev-flow.md)
- [.NET Agent Skills marketplace (GitHub)](https://github.com/dotnet/skills)
