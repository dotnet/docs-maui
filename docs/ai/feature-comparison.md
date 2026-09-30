---
title: Feature comparison
description: Find platform and capability comparisons for chat and text embeddings in Microsoft.Maui.Essentials.AI.
ms.date: 09/30/2026
---

# Feature comparison

`Microsoft.Maui.Essentials.AI` exposes on-device AI through the `Microsoft.Extensions.AI` interfaces. Each feature comparison contrasts what the abstraction can represent (including features a hosted provider might implement) with what the current Apple implementation actually supports. It isn't an iOS-versus-macOS comparison. Chat and embeddings use different Apple frameworks and have different requirements:

| Feature | Implementation | Compare capabilities | Apple guidance |
|---------|----------------|----------------------|----------------|
| [Chat](chat.md) | `AppleIntelligenceChatClient` (Foundation Models) | [Chat feature comparison](chat/feature-comparison.md) | [Chat on Apple platforms](chat/apple.md) |
| [Text embeddings](embeddings.md) | `NLEmbeddingGenerator` (Natural Language) | [Embeddings feature comparison](embeddings/feature-comparison.md) | [Embeddings on Apple platforms](embeddings/apple.md) |

Apple Intelligence availability for chat does not determine Natural Language embedding availability. Check the [Apple requirements](requirements-apple.md) for setup and device prerequisites.

> [!IMPORTANT]
> `Microsoft.Maui.Essentials.AI` is experimental and identified by diagnostic `MAUIAI0001`. See [Get started](getting-started.md#suppress-the-experimental-warning) for how to suppress it.

## Chat client

See [chat feature comparison](chat/feature-comparison.md) for platform availability, supported content, and options.

### Platform availability

See [chat on Apple platforms](chat/apple.md) for device and model availability guidance.

### Chat capabilities

See [chat feature comparison](chat/feature-comparison.md#abstraction-versus-apple-implementation).

### Supported ChatOptions

See [chat feature comparison](chat/feature-comparison.md#chat-options).

### Supported message content types

See [chat feature comparison](chat/feature-comparison.md#message-content).

## Embedding generator

See [embeddings feature comparison](embeddings/feature-comparison.md) for platform and model availability.

### Platform availability

See [embeddings feature comparison](embeddings/feature-comparison.md#platform-and-model-availability).

### Embedding capabilities

See [embeddings feature comparison](embeddings/feature-comparison.md#abstraction-versus-apple-implementation).

## Platforms

### Apple (iOS/Mac Catalyst)

See [chat on Apple platforms](chat/apple.md) and [embeddings on Apple platforms](embeddings/apple.md) for feature-specific guidance.

#### NLEmbeddingExtensions

See [using a native embedding](embeddings/apple.md#use-a-native-embedding).

#### Multiple languages

See [choose-a-language-and-model](embeddings/apple.md#choose-a-language-and-model).

## See also

- [Get started](getting-started.md)
- [Requirements](requirements-apple.md)
- [Use the IChatClient interface](/dotnet/ai/ichatclient)
- [Use the IEmbeddingGenerator interface](/dotnet/ai/iembeddinggenerator)
