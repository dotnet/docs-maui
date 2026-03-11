---
title: Apple requirements for Microsoft.Maui.Essentials.AI
description: Platform versions, device requirements, and Apple Intelligence prerequisites for using Microsoft.Maui.Essentials.AI on iOS, macOS, Mac Catalyst, and tvOS.
ms.date: 03/11/2026
ms.topic: conceptual
---

# Apple requirements for Microsoft.Maui.Essentials.AI

This page covers the requirements for using `Microsoft.Maui.Essentials.AI` on Apple platforms (iOS, macOS, Mac Catalyst, and tvOS).

## Chat client

`AppleIntelligenceChatClient` uses Apple's **Foundation Models** framework, which is part of Apple Intelligence.

### Minimum OS versions

| Platform | Minimum version |
|----------|-----------------|
| iOS | 26.0 |
| macOS | 26.0 |
| Mac Catalyst | 26.0 |
| tvOS | 26.0 |

### Device requirements

Apple Intelligence requires capable hardware:

- **iPhone**: iPhone 15 Pro, iPhone 15 Pro Max, or any iPhone 16 series or later.
- **iPad**: iPad Pro with M1 chip or later; iPad Air with M1 chip or later.
- **Mac**: Any Mac with Apple Silicon (M1, M2, M3, or M4 series).
- **Apple TV**: Apple TV 4K (3rd generation) or later.

All devices must have **Apple Intelligence enabled** in Settings:

- **iOS / iPadOS**: Settings → Apple Intelligence & Siri
- **macOS**: System Settings → Apple Intelligence & Siri

> [!NOTE]
> Required language models are downloaded automatically by the OS once Apple Intelligence is enabled. No manual download step is needed in your app.

For a full list of supported devices and regions, see [Apple Intelligence availability](https://support.apple.com/en-us/120898).

## Embedding generator

`NLEmbeddingGenerator` uses Apple's **Natural Language** framework (`NLEmbedding`). It does **not** require Apple Intelligence or any specific hardware.

### Minimum OS versions

| Platform | Minimum version |
|----------|-----------------|
| iOS | 13.0 |
| macOS | 10.15 |
| Mac Catalyst | 13.1 |
| tvOS | 13.0 |

The embedding generator uses Apple's sentence embedding model, which is built into the OS and requires no additional downloads.

## NuGet package

Add the following package to your `.csproj`:

```xml
<PackageReference Include="Microsoft.Maui.Essentials.AI" Version="10.0.*-*" />
```

Xcode 26 or later is required to build for Apple platforms.

## See also

- [Get started](getting-started.md) — install the package and register services
- [Chat client](chat.md) — usage examples
- [Text embeddings](embeddings.md) — usage examples
- [Android requirements](requirements-android.md)
- [Windows requirements](requirements-windows.md)
