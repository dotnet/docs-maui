---
title: Apple requirements for Microsoft.Maui.Essentials.AI
description: Apple platform and model prerequisites for chat and text embeddings in Microsoft.Maui.Essentials.AI.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Apple requirements for Microsoft.Maui.Essentials.AI

`Microsoft.Maui.Essentials.AI` has different requirements for [chat](chat/apple.md) and [text embeddings](embeddings/apple.md). Check the feature-specific availability before enabling either service in an app.

## Chat client

`AppleIntelligenceChatClient` uses Apple's **Foundation Models** framework, which is part of Apple Intelligence.

### Minimum OS versions

| Platform | Minimum version |
|----------|-----------------|
| iOS | 26.0 |
| macOS | 26.0 |
| Mac Catalyst | 26.0 |

The device must also support Apple Intelligence, have it enabled, and have an available model. OS version alone isn't sufficient. See [Chat on Apple platforms](chat/apple.md) for app guidance and [current Apple Intelligence device requirements](https://support.apple.com/en-us/121115) for supported hardware.

## Embedding generator

`NLEmbeddingGenerator` uses Apple's **Natural Language** framework (`NLEmbedding`). It does **not** require Apple Intelligence.

The generator uses an English sentence model by default. Check that a model is available for the language your app needs; support can vary by device and OS. See [Embeddings on Apple platforms](embeddings/apple.md) for model selection and indexing guidance.

Although Apple's native word and sentence embedding APIs are available on older OS versions, the package's native bridge targets Apple 26. Use the package's deployment requirements when configuring your app, not the introduction version of the underlying Natural Language API.

## NuGet package

See [Get started](getting-started.md#install-the-package) to install `Microsoft.Maui.Essentials.AI`. Build with Xcode 26 or later for text chat. Image input requires a package version with image support, Apple 27 target frameworks, and Xcode 27.

## See also

- [Get started](getting-started.md) — install the package and register services
- [Chat client](chat.md) — usage examples
- [Chat feature comparison](chat/feature-comparison.md) — chat support and capabilities
- [Text embeddings](embeddings.md) — usage examples
- [Embedding feature comparison](embeddings/feature-comparison.md) — API and model availability
