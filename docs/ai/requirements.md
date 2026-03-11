---
title: Requirements for Microsoft.Maui.Essentials.AI
description: Review the platform versions, device capabilities, and development prerequisites needed to use Microsoft.Maui.Essentials.AI in a .NET MAUI application.
ms.date: 05/14/2026
---

# Requirements for Microsoft.Maui.Essentials.AI

`Microsoft.Maui.Essentials.AI` surfaces native platform AI frameworks behind standard Microsoft.Extensions.AI interfaces. It is designed as a cross-platform library. Apple Intelligence is the first platform implementation available; Android and Windows support are planned for future releases. This page describes the requirements for the currently available platforms.

## Quick reference

| Feature | iOS | macOS | Mac Catalyst | tvOS | Android | Windows |
|---------|-----|-------|--------------|------|---------|---------|
| Chat client | 26.0+ | 26.0+ | 26.0+ | 26.0+ | Not available | Not available |
| Embedding generator | 13.0+ | 10.15+ | 13.1+ | 13.0+ | Not available | Not available |

## Apple platforms

### AppleIntelligenceChatClient

`AppleIntelligenceChatClient` implements `IChatClient` using Apple's **Foundation Models** framework, which is part of Apple Intelligence. It reports provider metadata of `provider="apple"` and `modelId="apple-intelligence"`.

#### Minimum OS versions

| Platform | Minimum version | Notes |
|----------|-----------------|-------|
| iOS | 26.0 | iPhone with Apple Intelligence support |
| macOS | 26.0 | Mac with Apple Silicon |
| Mac Catalyst | 26.0 | Mac with Apple Silicon |
| tvOS | 26.0 | Apple TV with A-series chip |

#### Device requirements

Apple Intelligence requires capable hardware. The following devices are supported:

- **iPhone**: iPhone 15 Pro, iPhone 15 Pro Max, or any iPhone 16 series device or later.
- **iPad**: iPad Pro with M1 chip or later; iPad Air with M1 chip or later.
- **Mac**: Any Mac with Apple Silicon (M1, M2, M3, or M4 series).
- **Apple TV**: Apple TV 4K (3rd generation) or later.

All devices must have **Apple Intelligence enabled** in Settings. On iOS and iPadOS, go to **Settings > Apple Intelligence & Siri**. On macOS, go to **System Settings > Apple Intelligence & Siri**.

> [!NOTE]
> Apple Intelligence must be enabled in device settings before `AppleIntelligenceChatClient` can be used. Required language models are downloaded automatically by the OS once Apple Intelligence is enabled; no manual download step is needed in your app.

For a full list of supported devices and regions, see [Apple Intelligence availability](https://support.apple.com/en-us/120898).

### NLEmbeddingGenerator

`NLEmbeddingGenerator` implements `IEmbeddingGenerator<string, Embedding<float>>` using Apple's **Natural Language** framework (`NLEmbedding`). The `NLEmbeddingExtensions.AsIEmbeddingGenerator()` extension method provides a convenient way to obtain an instance from an existing `NLEmbedding`.

Unlike `AppleIntelligenceChatClient`, `NLEmbeddingGenerator` does **not** require Apple Intelligence or any specific hardware. It has been available since iOS 13 and is widely supported.

#### Minimum OS versions

| Platform | Minimum version |
|----------|-----------------|
| iOS | 13.0 |
| macOS | 10.15 |
| Mac Catalyst | 13.1 |
| tvOS | 13.0 |

## Development requirements

The following tools and packages are required to develop with `Microsoft.Maui.Essentials.AI`:

| Requirement | Apple (iOS/macOS) | Android | Windows |
|-------------|-------------------|---------|---------|
| .NET SDK | .NET 10 or later | .NET 10 or later | .NET 10 or later |
| .NET MAUI workload | `dotnet workload install maui` | `dotnet workload install maui` | `dotnet workload install maui` |
| Platform tooling | Xcode 26 or later | Android SDK | Windows 10 SDK |
| NuGet package | `Microsoft.Maui.Essentials.AI` | Not available | Not available |

## Android

Android support for `Microsoft.Maui.Essentials.AI` is not yet available. This section will document requirements when Android support is added.

## Windows

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available. This section will document requirements when Windows support is added.

### Experimental API

`Microsoft.Maui.Essentials.AI` is an experimental API. Using any type from this package produces diagnostic warning **MAUIAI0001** at compile time. You must explicitly suppress this warning.

To suppress the warning for the entire project, add the following to your `.csproj` file:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);MAUIAI0001</NoWarn>
</PropertyGroup>
```

To suppress the warning for a specific file or block of code, use a pragma:

```csharp
#pragma warning disable MAUIAI0001
// Your code using Microsoft.Maui.Essentials.AI
#pragma warning restore MAUIAI0001
```

## See also

- [Microsoft.Maui.Essentials.AI overview](index.md)
- [Apple Intelligence availability](https://support.apple.com/en-us/120898)
- [Microsoft.Extensions.AI overview](https://learn.microsoft.com/dotnet/ai/ai-extensions)
