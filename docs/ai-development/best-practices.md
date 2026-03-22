---
title: Best practices for AI-assisted .NET MAUI development
description: Learn how to structure your .NET MAUI projects and write code that AI coding assistants can understand, navigate, and extend effectively.
ms.topic: concept
ms.date: 03/22/2026
no-loc: [".NET MAUI"]
---

# Best practices for AI-assisted .NET MAUI development

AI coding assistants work best when your project follows clear, consistent patterns. The way you organize files, name classes, document APIs, and configure builds directly affects the quality of AI-generated code. This article covers practical techniques to help AI assistants understand your .NET MAUI codebase and produce correct, platform-aware code.

## Project structure

A predictable folder layout helps AI assistants locate relevant code and understand the relationships between components. The standard .NET MAUI template provides a solid starting point:

```text
MyApp/
├── Platforms/
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   └── Windows/
├── Resources/
│   ├── Fonts/
│   ├── Images/
│   └── Styles/
├── Views/
├── ViewModels/
├── Models/
├── Services/
├── Converters/
├── App.xaml
├── App.xaml.cs
├── AppShell.xaml
├── MauiProgram.cs
└── MyApp.csproj
```

> [!TIP]
> When an AI assistant asks "where should I put this code?", a well-organized folder structure gives it a clear answer. Consistent structure across your team's projects means the AI learns the pattern once and applies it everywhere.

Follow these guidelines:

- **Keep platform-specific code in `Platforms/` folders.** This is where Android activities, iOS app delegates, and Windows app lifecycle code belong.
- **Group by feature or role.** Use folders like `Views/`, `ViewModels/`, `Services/`, and `Models/` so that AI assistants can infer a file's purpose from its location.
- **Separate shared logic from platform code.** Business logic and shared services should live outside `Platforms/` so AI assistants don't accidentally mix platform-specific APIs into shared code.
- **Use a consistent structure across projects.** If your team has multiple .NET MAUI apps, use the same folder layout in each. AI assistants carry learned patterns between projects.

## Platform-specific code patterns

.NET MAUI targets multiple platforms from a single project. AI assistants need to understand how your code handles platform differences — getting this wrong causes build failures that can be difficult to diagnose.

### File naming conventions

Use platform-specific file name suffixes to compile code only on the target platform:

```text
Services/
├── DeviceService.cs              # Shared interface
├── DeviceService.android.cs      # Android implementation
├── DeviceService.ios.cs          # iOS implementation
├── DeviceService.maccatalyst.cs  # Mac Catalyst implementation
└── DeviceService.windows.cs      # Windows implementation
```

The .NET MAUI build system automatically compiles `.android.cs` files only for Android, `.ios.cs` files only for iOS, and so on.

> [!WARNING]
> The `.ios.cs` and `.maccatalyst.cs` files compile independently — they do **not** share code. AI assistants frequently assume that iOS code also runs on Mac Catalyst because both use Apple frameworks. If your iOS and Mac Catalyst implementations are identical, you must duplicate the code in both files or use a shared base class.

### Partial classes with platform implementations

Partial classes let you split a type across shared and platform-specific files:

```csharp
// Services/DeviceService.cs (shared)
public partial class DeviceService
{
    public partial string GetDeviceIdentifier();
}

// Services/DeviceService.android.cs
public partial class DeviceService
{
    public partial string GetDeviceIdentifier()
    {
        return Android.Provider.Settings.Secure.GetString(
            Android.App.Application.Context.ContentResolver,
            Android.Provider.Settings.Secure.AndroidId) ?? "unknown";
    }
}
```

### Conditional compilation

For small platform differences, use `#if` directives:

```csharp
public string GetDefaultCacheSize()
{
#if ANDROID
    return "150MB";
#elif IOS || MACCATALYST
    return "100MB";
#elif WINDOWS
    return "200MB";
#else
    return "100MB";
#endif
}
```

> [!NOTE]
> Prefer partial classes or platform-specific files over `#if` directives when the platform-specific code is more than a few lines. Large blocks of conditional compilation are harder for AI assistants to reason about because they must mentally track which branches are active for each platform.

### Dependency injection for platform services

Register platform-specific implementations in `MauiProgram.cs`:

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder.UseMauiApp<App>();

    // Register shared services
    builder.Services.AddSingleton<IDeviceService, DeviceService>();
    builder.Services.AddTransient<MainViewModel>();

    return builder.Build();
}
```

Document service registrations in your repository instructions file so AI assistants know which interfaces are available and how they're resolved.

## Documentation that helps AI

AI assistants read your documentation to understand intent, architecture, and constraints. Well-placed documentation dramatically improves code generation quality.

### XML documentation comments

AI assistants read XML documentation comments to understand what a method does, what its parameters mean, and what callers should expect:

```csharp
/// <summary>
/// Synchronizes local data with the remote API.
/// </summary>
/// <remarks>
/// This method is safe to call from any thread. It acquires a lock
/// to prevent concurrent sync operations. On iOS, ensure the app
/// has background fetch capability enabled if calling from a
/// background task.
/// </remarks>
/// <param name="force">
/// When <see langword="true"/>, syncs all records regardless of
/// their last-modified timestamp.
/// </param>
/// <returns>The number of records synchronized.</returns>
/// <exception cref="NetworkException">
/// Thrown when the device is offline or the API is unreachable.
/// </exception>
public async Task<int> SyncDataAsync(bool force = false)
```

> [!TIP]
> Focus XML documentation on public APIs, especially interfaces that have platform-specific implementations. AI assistants use these comments as a specification when generating implementation code.

### README and architecture documentation

Include a `README.md` at the repository root with:

- **Build instructions** for each platform (`dotnet build -f net10.0-android`, `dotnet build -f net10.0-ios`)
- **Architecture overview** describing the patterns used (MVVM, dependency injection, messaging)
- **Key dependencies** and their purpose
- **Platform-specific setup** requirements (Xcode version, Android SDK levels, Windows SDK)

### Architecture Decision Records

For non-obvious design choices, create Architecture Decision Records (ADRs). AI assistants can read these to understand why your code is structured in a particular way:

```markdown
# ADR-003: Use SQLite instead of LiteDB

## Context
We need a local database for offline data storage.

## Decision
Use sqlite-net-pcl because it has better trimmer
compatibility and smaller binary size on iOS.

## Consequences
Must write SQL queries manually instead of using
LINQ-based document queries.
```

### Inline comments for platform workarounds

When you work around platform-specific behavior, explain **why** the workaround exists:

```csharp
// Android: WebView.EvaluateJavaScriptAsync can deadlock if called
// before the page finishes loading. Wait for Navigated event first.
// See: https://github.com/dotnet/maui/issues/12345
webView.Navigated += async (s, e) =>
{
    var result = await webView.EvaluateJavaScriptAsync("getData()");
};
```

## Common pitfalls AI should know about

These are mistakes AI assistants frequently make with .NET MAUI code. Document these patterns in your [repository instructions file](copilot-instructions.md) so the AI avoids them.

### Static fields that call platform APIs

Platform APIs like `FileSystem.AppDataDirectory` aren't available during type initialization. Static readonly fields are evaluated at type load time, which can happen before the .NET MAUI platform is initialized:

```csharp
// ❌ WRONG — evaluated during type initialization, before MAUI platform is ready
private static readonly string CachePath =
    Path.Combine(FileSystem.AppDataDirectory, "cache");

// ✅ CORRECT — lazy evaluation defers the call until first access
private static string? _cachePath;
private static string CachePath =>
    _cachePath ??= Path.Combine(FileSystem.AppDataDirectory, "cache");
```

### Linker and trimmer safety

The .NET trimmer removes code that appears unused at build time. Types accessed only through reflection — such as dependency injection registrations or XAML type references — can be trimmed away:

```xml
<!-- Preserve types that are only used via reflection -->
<ItemGroup>
  <TrimmerRootAssembly Include="MyApp.Services" />
</ItemGroup>
```

Alternatively, use the `[DynamicallyAccessedMembers]` or `[RequiresUnreferencedCode]` attributes to annotate methods that use reflection.

### iOS file system paths

On iOS, `Environment.SpecialFolder.LocalApplicationData` maps to a **cache** directory that the OS can purge at any time. For persistent data, always use the .NET MAUI file system API:

```csharp
// ❌ WRONG on iOS — this path can be purged by the OS
var path = Environment.GetFolderPath(
    Environment.SpecialFolder.LocalApplicationData);

// ✅ CORRECT — guaranteed persistent across app launches
var path = FileSystem.AppDataDirectory;
```

### Android edge-to-edge display

Starting with .NET 10, Android apps default to edge-to-edge display with `SafeAreaEdges` set to `None`. UI elements can render behind the system status bar and navigation bar. Account for safe areas in your layouts:

```xml
<Grid Padding="{OnPlatform Android='0,48,0,0'}">
    <!-- Content that needs to clear the Android status bar -->
</Grid>
```

### XAML Hot Reload limitations

Not all XAML changes apply through Hot Reload. These changes require a full rebuild:

- Adding or removing resource dictionaries
- Changing `x:Class` declarations
- Modifying the class hierarchy
- Adding new custom controls for the first time

> [!IMPORTANT]
> Document XAML Hot Reload limitations in your repository instructions so AI assistants don't suggest iterating on changes that require a full rebuild.

## Build and test configuration

AI assistants need to know how to build and test your project for each platform. Document these details in your `README.md` or repository instructions file.

### Build commands

Provide explicit build commands for each target framework:

```bash
# Build for Android
dotnet build -f net10.0-android

# Build for iOS (requires macOS)
dotnet build -f net10.0-ios

# Build for Windows
dotnet build -f net10.0-windows10.0.19041.0

# Run on a specific platform
dotnet build -f net10.0-android -t:Run
```

### Test configuration

Specify your test framework and how to run tests:

```bash
# Run unit tests
dotnet test MyApp.Tests/MyApp.Tests.csproj

# Run with specific verbosity
dotnet test --verbosity normal --logger "console;verbosity=detailed"
```

List required workloads, emulators, or simulators so AI assistants can troubleshoot build failures:

```bash
# Required workloads
dotnet workload install maui-android maui-ios maui-maccatalyst

# List available Android emulators
emulator -list-avds
```

## Dependency management

### Central Package Management

Use `Directory.Packages.props` to centralize NuGet package versions across your solution. This prevents version conflicts and gives AI assistants a single location to check for available packages:

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="CommunityToolkit.Mvvm" Version="8.4.0" />
    <PackageVersion Include="CommunityToolkit.Maui" Version="10.0.0" />
    <PackageVersion Include="sqlite-net-pcl" Version="1.9.172" />
    <PackageVersion Include="Microsoft.Extensions.Http" Version="10.0.0" />
  </ItemGroup>
</Project>
```

> [!NOTE]
> When using Central Package Management, individual project files use `<PackageReference Include="CommunityToolkit.Mvvm" />` without specifying a version. AI assistants sometimes add version attributes anyway — document this convention in your repository instructions to prevent it.

### Platform-specific packages

Some NuGet packages behave differently per platform or are only available on certain platforms. Document these in your instructions:

```markdown
## Key packages
- **CommunityToolkit.Maui** — cross-platform, but MediaElement
  has different codec support per platform
- **sqlite-net-pcl** — requires SQLitePCLRaw.bundle_green on
  all platforms for native SQLite binaries
- **SkiaSharp** — GPU acceleration available on Android/iOS,
  software rendering on some Windows configurations
```

## .editorconfig and code formatting

AI assistants read `.editorconfig` files and use them to match your code style. Include one in your repository root:

```ini
[*.cs]
indent_style = space
indent_size = 4
charset = utf-8

# Naming conventions
dotnet_naming_rule.private_fields.symbols = private_fields
dotnet_naming_rule.private_fields.style = prefix_underscore
dotnet_naming_rule.private_fields.severity = suggestion

dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private

dotnet_naming_style.prefix_underscore.capitalization = camel_case
dotnet_naming_style.prefix_underscore.required_prefix = _

# Code style
csharp_style_var_for_built_in_types = true:suggestion
csharp_style_expression_bodied_methods = when_on_single_line:suggestion
csharp_prefer_braces = true:suggestion

[*.xaml]
indent_style = space
indent_size = 4
```

> [!TIP]
> If your team uses non-standard naming conventions (such as `m_` prefixes for member fields or `I` prefixes for all interfaces), document these explicitly in your `.editorconfig` and repository instructions. AI assistants default to standard .NET naming conventions and need explicit guidance to deviate.

## See also

- [Repository instructions for .NET MAUI](copilot-instructions.md)
- [Custom instruction files](custom-instructions.md)
- [.NET Agent Skills for .NET MAUI](skills.md)
