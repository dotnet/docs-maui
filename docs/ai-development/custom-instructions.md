---
title: Custom instruction files and AGENTS.md
description: Learn how to use task-specific instruction files and AGENTS.md to give AI assistants focused guidance for .NET MAUI development tasks.
ms.topic: how-to
ms.date: 03/22/2026
no-loc: [".NET MAUI"]
---

# Custom instruction files and AGENTS.md

While `copilot-instructions.md` provides project-wide context, custom instruction files let you provide focused guidance for specific tasks or file types. These files live in `.github/instructions/` and are loaded automatically based on file glob patterns. When you open or edit a file in VS Code or work with files through GitHub Copilot CLI, any instruction file whose `applyTo` glob matches is automatically included in the AI context—no manual configuration required. For project-wide setup, see [Write a copilot-instructions.md file for .NET MAUI projects](copilot-instructions.md).

This article covers how to create task-specific instruction files and how to use `AGENTS.md` for repository-level agent policies.

## Custom instruction files

Custom instruction files use the naming convention `.github/instructions/<name>.instructions.md`. Each file contains YAML front matter with an `applyTo` field that specifies a glob pattern. When you work on files matching that pattern, the AI assistant automatically includes the corresponding instructions in its context.

```text
.github/
  instructions/
    safe-area-ios.instructions.md
    uitests.instructions.md
    xaml.instructions.md
    android-handlers.instructions.md
```

### File format

Every custom instruction file starts with YAML front matter containing the `applyTo` glob pattern, followed by Markdown content:

```markdown
---
applyTo: "**/*.xaml"
---

Your instructions here in Markdown format.
```

The `applyTo` field accepts comma-separated glob patterns to match multiple file types or directories:

```markdown
---
applyTo: "**/*.xaml,**/*.xaml.cs,**/Resources/Styles/**"
---
```

> [!TIP]
> Keep each instruction file focused on a single concern. Multiple small files are easier to maintain than one large file covering everything.

## .NET MAUI instruction file examples

The following examples show instruction files for common .NET MAUI development scenarios. Use them as starting points and adapt them to your project's conventions.

> [!TIP]
> If you're contributing to a project, skim the `.github/instructions/` folder for any `*.instructions.md` files whose `applyTo` patterns match the files you're editing. These files describe the conventions the AI assistant follows and can help you align your changes with the project's expectations.

### iOS and Mac Catalyst safe area handling

Create `.github/instructions/safe-area-ios.instructions.md` to provide guidance when working on iOS or Mac Catalyst platform code:

```markdown
---
applyTo: "**/Platforms/iOS/**,**/Platforms/MacCatalyst/**,**/*.ios.cs,**/*.maccatalyst.cs"
---

# iOS and Mac Catalyst platform guidance

## Safe area insets
- Always account for safe area insets when positioning UI elements.
  Use `UIApplication.SharedApplication.ConnectedScenes` to get the
  active window scene and read `SafeAreaInsets` from the root view.
- Never hardcode status bar or home indicator heights. Device
  geometry varies across iPhone and iPad models.

## UIKit interop
- When accessing UIKit types from handlers or platform code, use
  `MainThread.BeginInvokeOnMainThread` to ensure UI updates run on
  the main thread.
- Prefer `PlatformView` properties on handlers over direct
  `UIView` manipulation when possible.

## Platform-specific adjustments
- Use compiler directives (`#if IOS || MACCATALYST`) only in
  platform-shared files. Code under `Platforms/iOS/` or
  `Platforms/MacCatalyst/` doesn't need them.
- Register platform services in `MauiProgram.cs` using
  `ConfigureLifecycleEvents` for iOS-specific lifecycle hooks.
```

### UI test writing

Create `.github/instructions/uitests.instructions.md` to guide AI assistants when writing or modifying tests:

```markdown
---
applyTo: "**/*Test*/**,**/*test*/**"
---

# UI test conventions

## Test structure
- Use the Arrange-Act-Assert pattern for all tests.
- Name test methods using the pattern: `MethodName_Scenario_ExpectedResult`.
- One assertion per test method when possible.

## Appium tests
- Use `FindElement` with `By.Id` using AutomationId values set in XAML.
- Add explicit waits with `WebDriverWait` instead of `Thread.Sleep`.
- Use the page object pattern to encapsulate screen interactions.

## Device test infrastructure
- Tests must run on both Android and iOS targets.
- Use conditional attributes (`[Trait("Platform", "iOS")]`) to mark
  platform-specific tests.
- Store test app bundle identifiers and paths in a shared
  configuration file, not inline in test code.
```

### XAML patterns

Create `.github/instructions/xaml.instructions.md` for guidance when editing XAML files and their code-behind:

```markdown
---
applyTo: "**/*.xaml,**/*.xaml.cs"
---

# XAML conventions

## Data binding
- Use `x:DataType` on every XAML page and template for compiled
  bindings. This enables compile-time checking and improves
  performance.
- Bind to view model properties, not code-behind properties.
- Use `CommunityToolkit.Mvvm` source generators for
  `ObservableProperty` and `RelayCommand`.

## Resources and styles
- Define reusable colors and styles in `Resources/Styles/`.
- Use `StaticResource` for values that don't change at runtime and
  `DynamicResource` for theme-aware values.
- Follow the naming convention: `PrimaryColor`, `HeaderStyle`,
  `DefaultSpacing`.

## Layout
- Prefer `Grid` and `VerticalStackLayout`/`HorizontalStackLayout`
  over nested `StackLayout` for better performance.
- Always set `AutomationId` on interactive controls to support
  UI testing.
```

### Android handler customization

Create `.github/instructions/android-handlers.instructions.md` for Android platform code:

```markdown
---
applyTo: "**/Platforms/Android/**,**/*.android.cs"
---

# Android platform guidance

## Handlers
- Customize controls by modifying the handler mapper in
  `MauiProgram.cs` rather than creating custom renderers.
- Access the native Android view through `PlatformView` on the
  handler.
- Call `Invalidate()` on the handler when the cross-platform
  property changes need to trigger a native view update.

## Android lifecycle
- Use `ConfigureLifecycleEvents` in `MauiProgram.cs` to hook into
  `OnCreate`, `OnResume`, and `OnDestroy`.
- Request runtime permissions through `Permissions.RequestAsync<T>()`
  instead of platform-specific permission APIs.

## Performance
- Avoid allocations in frequently called handler methods.
- Use `RecyclerView` patterns when working with list-heavy native
  views.
```

## AGENTS.md

`AGENTS.md` is a file placed at the root of your repository that provides high-level instructions for AI agents performing autonomous workflows. While `copilot-instructions.md` is optimized for interactive coding assistance, `AGENTS.md` targets agents that work independently, such as those that create pull requests, run builds, or perform multi-step tasks. For example, an AI agent might clone your repository, run `dotnet build` and `dotnet test`, and leave a PR comment summarizing any failures—all guided by the policies in `AGENTS.md`.

### When to use AGENTS.md

Use `AGENTS.md` to define repository-wide policies that autonomous agents should follow:

- **Git workflow** — branch naming conventions, commit message format, when to squash commits.
- **PR requirements** — required reviewers, CI checks that must pass, PR description templates.
- **Build system** — how to build and test the project, which commands to run, expected output.
- **Code standards** — linting rules, formatting requirements, naming conventions.

### Example AGENTS.md for a .NET MAUI project

```markdown
# AGENTS.md

## Build and test
- Build: `dotnet build src/MyApp.sln`
- Test: `dotnet test src/MyApp.sln --no-build`
- All tests must pass before submitting a PR.

## Git workflow
- Create branches from `main` using the format `feature/<description>`
  or `fix/<description>`.
- Write commit messages in imperative mood: "Add feature" not
  "Added feature".
- Squash commits before merging.

## PR requirements
- Include a summary of changes and link to the related issue.
- Add screenshots for any visual changes.
- Ensure CI passes on both Android and iOS targets.

## Code standards
- Run `dotnet format` before committing.
- Follow the existing naming conventions in the codebase.
- Add XML documentation comments to all public APIs.
```

> [!NOTE]
> `AGENTS.md` and `copilot-instructions.md` are complementary. Use both in your project — `copilot-instructions.md` for interactive coding context and `AGENTS.md` for autonomous agent policies. The `AGENTS.md` convention is supported by GitHub Copilot, Claude Code, and other AI coding assistants.

### For contributors

If you're contributing to an open-source project, read the repository's `AGENTS.md` before opening a pull request. It describes the conventions and quality checks that AI agents enforce during PR reviews, such as required CI checks, commit message formats, and code standards. Understanding these policies helps you avoid automated review feedback and ensures your PR aligns with the project's workflow from the start.

## When to use which file

Use the following table to decide where to put your AI guidance:

| Scenario | Recommended file |
|----------|------------------|
| Project overview, build commands, architecture | `copilot-instructions.md` |
| Platform-specific coding patterns (iOS, Android) | `.github/instructions/*.instructions.md` |
| File-type guidance (XAML, tests) | `.github/instructions/*.instructions.md` |
| Repository-wide agent policies | `AGENTS.md` |
| Git workflow and PR requirements | `AGENTS.md` |
| Task-specific guidance (testing, accessibility) | `.github/instructions/*.instructions.md` |

> [!IMPORTANT]
> Custom instruction files are most effective when they are concise and specific. If an instruction file grows beyond a few hundred lines, consider splitting it into multiple files with narrower `applyTo` patterns.

## Tips for effective instruction files

Follow these practices to get the most value from your instruction files:

- **Be specific** — instructions like "use compiled bindings" are more actionable than "follow best practices."
- **Include code patterns** — show the preferred way to write code, not just rules to follow.
- **Update regularly** — review instruction files when your project conventions change.
- **Test with the AI** — ask the AI assistant to write code for a matching file and verify it follows your instructions.

## See also

- [Write a copilot-instructions.md file for .NET MAUI projects](copilot-instructions.md)
- [Best practices for AI-assisted .NET MAUI development](best-practices.md)
