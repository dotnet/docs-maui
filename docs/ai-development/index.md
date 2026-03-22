---
title: "AI-assisted development"
description: "Learn how to configure your .NET MAUI projects so AI coding assistants like GitHub Copilot and Claude Code can help you build cross-platform apps more effectively."
ms.topic: concept
ms.date: 03/22/2026
no-loc: [".NET MAUI"]
---

# AI-assisted development for .NET MAUI

AI coding assistants are tools that integrate directly into your development environment — GitHub Copilot in Visual Studio and Visual Studio Code, Claude Code in the terminal, and similar tools — to suggest code, answer questions, and automate repetitive tasks using large language models. They can understand natural-language prompts alongside your code context to generate, refactor, and debug code in real time.

AI coding assistants such as GitHub Copilot and Claude Code can dramatically accelerate .NET MAUI development. From generating XAML layouts and platform-specific code to diagnosing build errors and suggesting API usage, these tools reduce the time you spend on repetitive tasks and help you explore unfamiliar platform APIs faster.

However, .NET MAUI's cross-platform nature introduces complexity that general-purpose AI coding assistants don't always handle well. A single .NET MAUI project can target up to six platforms (Android, iOS, Mac Catalyst, macOS, Windows, and Tizen), each with its own APIs, lifecycle, permissions model, and build tooling. Platform-specific code patterns (such as conditional compilation and platform-specific files), multi-targeted MSBuild configurations, and the interplay between XAML and C# all require context that an AI coding assistant won't have by default.

This section covers how to configure your repository and development workflow so that AI coding assistants understand your .NET MAUI project structure, follow your team's conventions, and produce code that builds and runs correctly across platforms.

## What's available

The following tools and techniques help AI coding assistants work effectively with .NET MAUI projects:

### Repository instructions

A [repository instructions file](copilot-instructions.md) (`.github/copilot-instructions.md`) teaches AI coding assistants about your project's structure, build commands, target frameworks, and coding conventions. This is the single most impactful step you can take to improve AI-generated code quality.

### Custom instruction files

[Custom instruction files](custom-instructions.md) provide task-specific guidance that AI coding assistants can apply in the right context. You can create separate instruction files for iOS-specific patterns, XAML best practices, testing strategies, accessibility guidelines, and more.

### Agent skills

[Agent skills](skills.md) are pre-built capabilities from the skills marketplace that give AI coding assistants specialized knowledge about .NET MAUI. The `dotnet-maui` skill, for example, provides MAUI-specific diagnostics, API guidance, and awareness of common pitfalls.

### MauiDevFlow

[MauiDevFlow](devflow.md) is an experimental tool that connects AI coding assistants to your running app's visual tree. It enables AI-assisted UI debugging by letting the assistant inspect the live element hierarchy, identify layout issues, and suggest fixes based on actual runtime state.

### Best practices

[Best practices for AI-assisted development](best-practices.md) covers project structure patterns, code organization, and documentation habits that help AI coding assistants understand your codebase. Small changes — like consistent file naming and clear XML documentation comments — can significantly improve the quality of AI-generated code.

## Quick-start checklist

Follow these steps to make your .NET MAUI repository AI-ready:

1. **Add repository instructions.** Create a `.github/copilot-instructions.md` file with your project overview, build commands (`dotnet build`, `dotnet run`), and target frameworks.
2. **Document platform-specific conventions.** List your file naming patterns for platform code (`.ios.cs`, `.android.cs`, `.windows.cs`) and any conditional compilation symbols you use.
3. **Install the `dotnet-maui` agent skill.** This gives AI coding assistants MAUI-specific diagnostics and API knowledge out of the box.
4. **Consider adding MauiDevFlow (experimental).** If you frequently debug layout or visual tree issues, this tool lets AI coding assistants inspect your running UI.
5. **Document your testing and CI setup.** Include instructions for running unit tests, UI tests, and any platform-specific test configurations so AI coding assistants can help you write and debug tests.
6. **Read existing project conventions.** When contributing to an existing repo, first read its `.github/copilot-instructions.md` and any `AGENTS.md` file to understand the project's conventions.

> [!TIP]
> Start with step 1. A well-written repository instructions file alone can eliminate most incorrect suggestions from AI coding assistants, such as using deprecated APIs or targeting the wrong platform.

## See also

- [Repository instructions for .NET MAUI](copilot-instructions.md)
- [Custom instruction files](custom-instructions.md)
- [Agent skills for .NET MAUI](skills.md)
- [MauiDevFlow](devflow.md)
- [Best practices for AI-assisted development](best-practices.md)
- [Microsoft.Extensions.AI overview](/dotnet/ai/ai-extensions)
- [On-device AI capabilities](../ai/index.md)
