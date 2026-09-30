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

The native *word* and *sentence* embedding APIs have different introduction versions. The default generator requests an English sentence model, not the older word model. The managed project and native bridge have separate deployment settings; native API introduction alone doesn't prove the complete package works on an older OS. See the [embedding feature comparison](embeddings/feature-comparison.md#platform-and-model-availability) for native API floors and package caveats, and [Embeddings on Apple platforms](embeddings/apple.md) for model selection and indexing guidance.

## NuGet package

Add the following package to your `.csproj`:

```xml
<PackageReference Include="Microsoft.Maui.Essentials.AI" Version="10.0.50-preview.1.26158.1" />
```

Xcode 26 or later is required to build for Apple platforms.

## See also

- [Get started](getting-started.md) — install the package and register services
- [Chat client](chat.md) — usage examples
- [Chat feature comparison](chat/feature-comparison.md) — chat support and capabilities
- [Text embeddings](embeddings.md) — usage examples
- [Embedding feature comparison](embeddings/feature-comparison.md) — API and model availability
