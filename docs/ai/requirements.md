---
title: Requirements for Microsoft.Maui.Essentials.AI
description: Review the platform versions and device capabilities needed to use the Microsoft.Maui.Essentials.AI chat client and embedding generator in a .NET MAUI application.
ms.date: 03/11/2026
---

# Requirements for Microsoft.Maui.Essentials.AI

`Microsoft.Maui.Essentials.AI` surfaces native platform AI frameworks behind standard Microsoft.Extensions.AI interfaces. Apple Intelligence is the first platform implementation available; Android and Windows support are planned for future releases.

## Chat client

The `AppleIntelligenceChatClient` implements `IChatClient` using Apple's **Foundation Models** framework, which is part of Apple Intelligence.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Android support for the `Microsoft.Maui.Essentials.AI` chat client is not yet available.

# [iOS/Mac Catalyst](#tab/macios)

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
> Required language models are downloaded automatically by the OS once Apple Intelligence is enabled; no manual download step is needed in your app.

For a full list of supported devices and regions, see [Apple Intelligence availability](https://support.apple.com/en-us/120898).

# [Windows](#tab/windows)

Windows support for the `Microsoft.Maui.Essentials.AI` chat client is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Embedding generator

The `NLEmbeddingGenerator` implements `IEmbeddingGenerator<string, Embedding<float>>` using Apple's **Natural Language** framework (`NLEmbedding`). Unlike the chat client, it does **not** require Apple Intelligence or any specific hardware; it has been available since iOS 13.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Android support for the `Microsoft.Maui.Essentials.AI` embedding generator is not yet available.

# [iOS/Mac Catalyst](#tab/macios)

#### Minimum OS versions

| Platform | Minimum version |
|----------|-----------------|
| iOS | 13.0 |
| macOS | 10.15 |
| Mac Catalyst | 13.1 |
| tvOS | 13.0 |

The embedding generator uses Apple's `NLEmbedding` sentence embedding model, which is built into the OS and does not require any additional downloads or hardware.

# [Windows](#tab/windows)

Windows support for the `Microsoft.Maui.Essentials.AI` embedding generator is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Experimental API

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
