---
title: Write a copilot-instructions.md file for .NET MAUI projects
description: Learn how to create an effective .github/copilot-instructions.md file that gives GitHub Copilot context about your .NET MAUI project structure, platform targets, and coding conventions.
ms.topic: how-to
ms.date: 03/22/2026
no-loc: [".NET MAUI"]
---

# Write a copilot-instructions.md file for .NET MAUI projects

GitHub Copilot reads a `.github/copilot-instructions.md` file from your repository to understand your project's context. For .NET MAUI projects, this file is especially valuable because MAUI apps have cross-platform complexity that AI assistants frequently get wrong — platform-specific file naming conventions, conditional compilation symbols, and multi-target build commands all benefit from explicit documentation.

When you provide a well-written instructions file, Copilot generates code that follows your project's actual patterns instead of guessing. This means fewer corrections for platform-specific code, correct build commands on the first try, and suggestions that match your MVVM framework, dependency injection setup, and navigation model.

> [!TIP]
> The `.github/copilot-instructions.md` file is automatically loaded by GitHub Copilot in Visual Studio Code, Visual Studio, and the GitHub Copilot CLI. You don't need to reference it manually.

## Create the file

Create a file named `copilot-instructions.md` in the `.github` directory at the root of your repository:

```
your-maui-project/
├── .github/
│   └── copilot-instructions.md    ← Create this file
├── Platforms/
├── Resources/
├── MauiProgram.cs
└── YourApp.csproj
```

If the `.github` directory doesn't exist, create it. This directory is also used for GitHub Actions workflows, issue templates, and other GitHub configuration files.

## Essential sections for .NET MAUI projects

The following sections cover what to include in your instructions file. Adapt each section to match your specific project.

### Project overview

Start with the fundamentals: framework version, target platforms, architecture pattern, and app type. This helps Copilot understand the overall shape of your project.

```markdown
## Project Overview

This is a .NET MAUI app targeting .NET 9, built with Shell navigation.
Targets: Android, iOS, Mac Catalyst, Windows.
Uses MVVM with CommunityToolkit.Mvvm.
UI is built with XAML and code-behind files.
Dependency injection is configured in MauiProgram.cs.
```

If your app uses Blazor Hybrid, mention that explicitly — the code patterns differ significantly from pure XAML apps:

```markdown
## Project Overview

This is a .NET MAUI Blazor Hybrid app targeting .NET 9.
Targets: Android, iOS, Mac Catalyst, Windows.
UI is built with Razor components in the /Components directory.
Native MAUI pages are used for platform integration only.
```

### Build and run commands

.NET MAUI projects use target framework monikers (TFMs) to build for specific platforms. Include the exact commands so Copilot can suggest the correct build invocation:

```markdown
## Build Commands

- All platforms: `dotnet build`
- Android: `dotnet build -f net9.0-android -t:Install`
- iOS Simulator: `dotnet build -f net9.0-ios -t:Run`
- Mac Catalyst: `dotnet build -f net9.0-maccatalyst`
- Windows: `dotnet build -f net9.0-windows10.0.19041.0`

## Run Commands

- Android emulator: `dotnet build -f net9.0-android -t:Run`
- iOS Simulator: `dotnet build -f net9.0-ios -t:Run -p:_DeviceName=:v2:udid=SIMULATOR_UDID`
- Mac Catalyst: `dotnet build -f net9.0-maccatalyst -t:Run`
- Windows: `dotnet run -f net9.0-windows10.0.19041.0`
```

> [!NOTE]
> Replace `net9.0` in the examples above with your target .NET version (for example, `net10.0`). Copilot reads your `.github/copilot-instructions.md` file automatically in Visual Studio Code, Visual Studio, and the GitHub Copilot CLI — no manual configuration is required.

> [!NOTE]
> The Windows TFM requires a Windows SDK version suffix (for example, `net9.0-windows10.0.19041.0`). If your project targets a different SDK version, update the TFM to match. Check your `.csproj` file's `TargetFrameworks` value for the exact version.

### Platform-specific code organization

This is the most critical section for .NET MAUI projects. AI assistants frequently generate platform-specific code incorrectly because the file naming and compilation rules are unintuitive.

```markdown
## Platform-Specific Code

### File naming conventions
- Files ending in `.android.cs` compile only for Android
- Files ending in `.ios.cs` compile for BOTH iOS AND Mac Catalyst
- Files ending in `.maccatalyst.cs` compile ONLY for Mac Catalyst
- Files ending in `.windows.cs` compile only for Windows

### IMPORTANT: iOS and Mac Catalyst overlap
Both `.ios.cs` and `.maccatalyst.cs` files compile for Mac Catalyst targets.
There is NO precedence mechanism — if both exist with the same partial class,
you will get compilation errors on Mac Catalyst.

### Platform folders
- `Platforms/Android/` — Android-specific code (Activities, Services)
- `Platforms/iOS/` — iOS-specific code (AppDelegate, SceneDelegate)
- `Platforms/MacCatalyst/` — Mac Catalyst code (AppDelegate, SceneDelegate)
- `Platforms/Windows/` — Windows code (App.xaml.cs, Package.appxmanifest)
- `Platforms/Tizen/` — Tizen code (if targeting Tizen)

### Conditional compilation symbols
Use these preprocessor directives for platform-specific code in shared files:
- `#if ANDROID` — Android only
- `#if IOS` — iOS only (does NOT include Mac Catalyst)
- `#if MACCATALYST` — Mac Catalyst only
- `#if WINDOWS` — Windows only
- `#if IOS || MACCATALYST` — Both Apple mobile platforms
```

> [!WARNING]
> Files ending in `.ios.cs` compile for **both** iOS **and** Mac Catalyst targets. Files ending in `.maccatalyst.cs` compile **only** for Mac Catalyst. This means that when you build for Mac Catalyst, both `.ios.cs` and `.maccatalyst.cs` files are compiled — there is no precedence mechanism that picks one over the other. If both files define the same partial class members, you get duplicate-definition compilation errors on Mac Catalyst. This is the single most common source of AI-generated build errors in .NET MAUI projects. Always include this information in your instructions file.

### Project structure

Document your directory layout so Copilot understands where to place new files:

````markdown
## Project Structure

```
src/MyApp/
├── App.xaml(.cs)              — Application entry, resource loading
├── AppShell.xaml(.cs)         — Shell navigation structure
├── MauiProgram.cs             — DI container, service registration
├── Models/                    — Data models and DTOs
├── ViewModels/                — MVVM view models
├── Views/                     — XAML pages and controls
├── Services/                  — Business logic and data access
├── Converters/                — IValueConverter implementations
├── Handlers/                  — Custom handler implementations
├── Resources/
│   ├── Fonts/                 — Custom fonts (.ttf, .otf)
│   ├── Images/                — SVG and PNG images
│   ├── Raw/                   — Raw assets (JSON, HTML)
│   └── Styles/                — XAML resource dictionaries
└── Platforms/                 — Platform-specific code
```
````

### Testing patterns

Describe your test setup so Copilot can generate tests that match your infrastructure:

```markdown
## Testing

- Unit tests: `tests/MyApp.Tests/` using xUnit
- Run tests: `dotnet test tests/MyApp.Tests/`
- UI tests: `tests/MyApp.UITests/` using Appium with .NET
- Mocking framework: NSubstitute
- View model tests should not reference any MAUI types directly
- Use `IDispatcher` abstraction instead of `MainThread.InvokeOnMainThreadAsync`
```

### NuGet packages and dependencies

List key packages so Copilot understands what APIs are available:

```markdown
## Key NuGet Packages

- `CommunityToolkit.Mvvm` (v8.4.0) — Source generators for MVVM:
  [ObservableProperty], [RelayCommand], ObservableObject
- `CommunityToolkit.Maui` (v10.0.0) — Extra controls, converters, behaviors
- `Microsoft.Extensions.Http` — Typed HttpClient via DI
- `SQLite-net-pcl` — Local database with SQLiteAsyncConnection
- `SkiaSharp.Views.Maui.Controls` — 2D graphics rendering

Do NOT suggest packages we don't use. If a task requires a new package,
mention it explicitly before adding it.
```

### Coding conventions

Document your team's patterns so generated code is consistent:

```markdown
## Coding Conventions

- Use file-scoped namespaces
- Use primary constructors for dependency injection
- ViewModels inherit from ObservableObject (CommunityToolkit.Mvvm)
- Use [ObservableProperty] instead of manual INotifyPropertyChanged
- Use [RelayCommand] instead of manual ICommand implementations
- XAML pages use x:DataType for compiled bindings
- All async methods should use ConfigureAwait(false) except in UI code
- Services are registered in MauiProgram.cs using extension methods
- Use SemanticProperties for accessibility on all interactive elements
```

## .NET MAUI-specific tips

Beyond the sections above, consider documenting these patterns that AI assistants frequently handle incorrectly.

### Handlers vs. renderers

.NET MAUI uses the handler architecture, not the legacy Xamarin.Forms renderer pattern:

```markdown
## Platform Customization

This project uses the .NET MAUI handler architecture. Do NOT use or suggest
Xamarin.Forms custom renderers. When customizing native controls:

1. Use handler mapping in MauiProgram.cs:
   `handlers.AddHandler<MyControl, MyHandler>()`
2. Or use handler lifecycle methods:
   `Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(...)`
```

### Migrating from Xamarin.Forms

If your project was migrated from Xamarin.Forms, document migration-specific constraints in your instructions file so AI assistants don't suggest legacy patterns:

```markdown
## Migration Constraints (Xamarin.Forms to .NET MAUI)

This project was migrated from Xamarin.Forms. Do NOT use or suggest:
- Custom renderers (use handlers instead)
- `Device.RuntimePlatform` checks (use `DeviceInfo.Platform` or conditional compilation)
- `DependencyService` (use Microsoft.Extensions.DependencyInjection)
- `MessagingCenter` (use WeakReferenceMessenger from CommunityToolkit.Mvvm)

### Handlers vs. renderers quick reference
- Before (Xamarin.Forms): `[assembly: ExportRenderer(...)]` + subclass `ViewRenderer<TView, TNativeView>`
- After (.NET MAUI): Configure handler mappings in `MauiProgram.cs`:
  `Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) => { ... });`
```

### App entry point

```markdown
## App Entry Point

MauiProgram.cs is the single entry point. Platform-specific startup code goes in:
- Android: Platforms/Android/MainApplication.cs
- iOS: Platforms/iOS/AppDelegate.cs
- Mac Catalyst: Platforms/MacCatalyst/AppDelegate.cs
- Windows: Platforms/Windows/App.xaml.cs
```

### XAML and resource dictionaries

```markdown
## XAML Patterns

- All styles are defined in Resources/Styles/Styles.xaml
- App-wide resources are in App.xaml
- Use StaticResource for styles, DynamicResource for theme-aware values
- Custom colors are defined in Resources/Styles/Colors.xaml
- Use compiled bindings with x:DataType on every page
```

### Shell navigation

If your app uses Shell, document the routing pattern:

```markdown
## Navigation

This app uses Shell navigation. Routes are registered in AppShell.xaml.cs:
- `Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));`
- Navigate with: `await Shell.Current.GoToAsync(nameof(DetailPage));`
- Pass parameters with: `await Shell.Current.GoToAsync($"{nameof(DetailPage)}?id={itemId}");`
- Receive parameters with [QueryProperty] attribute or IQueryAttributable
```

## Template

Use the following template as a starting point for your own `.github/copilot-instructions.md` file. Remove sections that don't apply and add details specific to your project.

```markdown
# Copilot Instructions for [Your App Name]

## Project Overview

This is a .NET MAUI app targeting .NET 9.
Architecture: MVVM with CommunityToolkit.Mvvm.
Navigation: Shell-based.
Target platforms: Android, iOS, Mac Catalyst, Windows.

## Build Commands

- All platforms: `dotnet build`
- Android: `dotnet build -f net9.0-android`
- iOS Simulator: `dotnet build -f net9.0-ios`
- Mac Catalyst: `dotnet build -f net9.0-maccatalyst`
- Windows: `dotnet build -f net9.0-windows10.0.19041.0`

## Platform-Specific Code

### File naming
- `.android.cs` → Android only
- `.ios.cs` → iOS AND Mac Catalyst
- `.maccatalyst.cs` → Mac Catalyst only
- `.windows.cs` → Windows only

IMPORTANT: `.ios.cs` files compile for Mac Catalyst too.
Do not create both `.ios.cs` and `.maccatalyst.cs` for the same partial class.

### Conditional compilation
- `#if ANDROID`, `#if IOS`, `#if MACCATALYST`, `#if WINDOWS`
- `#if IOS || MACCATALYST` for both Apple platforms

### Platform folders
- `Platforms/Android/` — Android entry points, services
- `Platforms/iOS/` — iOS AppDelegate, scene configuration
- `Platforms/MacCatalyst/` — Mac Catalyst AppDelegate
- `Platforms/Windows/` — Windows App.xaml.cs

## Project Structure

- `MauiProgram.cs` — App entry point, DI registration
- `App.xaml(.cs)` — Application lifecycle
- `AppShell.xaml(.cs)` — Navigation structure
- `Models/` — Data models
- `ViewModels/` — ObservableObject-based view models
- `Views/` — XAML pages
- `Services/` — Business logic, data access
- `Resources/Styles/` — Colors.xaml, Styles.xaml

## Key Packages

- CommunityToolkit.Mvvm — [ObservableProperty], [RelayCommand]
- CommunityToolkit.Maui — Additional controls and converters

## Coding Conventions

- File-scoped namespaces
- Primary constructors for DI
- [ObservableProperty] for bindable properties
- [RelayCommand] for commands
- Compiled bindings with x:DataType
- SemanticProperties on interactive elements

## Testing

- Framework: xUnit
- Run: `dotnet test`
- ViewModels must not depend on MAUI types directly

## App Entry Point

- Shared: MauiProgram.cs
- Android: Platforms/Android/MainApplication.cs
- iOS: Platforms/iOS/AppDelegate.cs
- Mac Catalyst: Platforms/MacCatalyst/AppDelegate.cs
- Windows: Platforms/Windows/App.xaml.cs

## XAML Patterns

- Styles: Resources/Styles/Styles.xaml
- Colors: Resources/Styles/Colors.xaml
- Use StaticResource for styles
- Use DynamicResource for theme-aware values

## Navigation

- Shell-based navigation
- Register routes in AppShell.xaml.cs
- Navigate: `await Shell.Current.GoToAsync(nameof(Page))`
- Parameters: use [QueryProperty] or IQueryAttributable
```

## See also

- [Custom instruction files and AGENTS.md](custom-instructions.md)
- [Best practices for AI-assisted .NET MAUI development](best-practices.md)
- [.NET Agent Skills for .NET MAUI development](skills.md)
