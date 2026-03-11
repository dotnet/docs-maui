---
title: Feature comparison
description: Compares the AI features available across platforms supported by Microsoft.Maui.Essentials.AI.
ms.date: 05/14/2026
---

# Feature comparison

This page compares the AI features available across platforms supported by `Microsoft.Maui.Essentials.AI`.

> [!NOTE]
> Currently, `Microsoft.Maui.Essentials.AI` supports Apple platforms only. Support for additional platforms will be documented as it becomes available.

> [!IMPORTANT]
> `Microsoft.Maui.Essentials.AI` is experimental and is identified by the diagnostic ID `MAUIAI0001`. To use it, suppress the diagnostic or opt in explicitly.

## Chat capabilities

Chat features are provided by `AppleIntelligenceChatClient` and require Apple Intelligence, which is available on iOS 26.0+, macOS 26.0+, Mac Catalyst 26.0+, and tvOS 26.0+.

| Feature | iOS | macOS | Mac Catalyst | tvOS |
|---------|-----|-------|--------------|------|
| Text generation | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ |
| Streaming responses | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ |
| Tool/function calling | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ |
| Structured JSON output | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ |
| System prompts | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ |
| Multi-turn conversations | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ |
| Image input | ❌ | ❌ | ❌ | ❌ |

## Embedding capabilities

Embedding features are provided by `NLEmbeddingGenerator` and use the Apple Natural Language framework. These features do **not** require Apple Intelligence and are available on older OS versions.

| Feature | iOS | macOS | Mac Catalyst | tvOS |
|---------|-----|-------|--------------|------|
| Text embeddings | ✅ 13.0+ | ✅ 10.15+ | ✅ 13.1+ | ✅ 13.0+ |
| Multiple languages | ✅ | ✅ | ✅ | ✅ |
| Custom `NLEmbedding` | ✅ | ✅ | ✅ | ✅ |

## Supported ChatOptions

The following `ChatOptions` properties are honored by `AppleIntelligenceChatClient`. Options marked ❌ are silently ignored.

| Option | iOS | macOS | Mac Catalyst | tvOS |
|--------|-----|-------|--------------|------|
| `Temperature` | ✅ | ✅ | ✅ | ✅ |
| `TopK` | ✅ | ✅ | ✅ | ✅ |
| `Seed` | ✅ | ✅ | ✅ | ✅ |
| `MaxOutputTokens` | ✅ | ✅ | ✅ | ✅ |
| `ResponseFormat` (JSON schema) | ✅ | ✅ | ✅ | ✅ |
| `Tools` (`AIFunction`) | ✅ | ✅ | ✅ | ✅ |
| `TopP` | ❌ | ❌ | ❌ | ❌ |
| `FrequencyPenalty` | ❌ | ❌ | ❌ | ❌ |
| `PresencePenalty` | ❌ | ❌ | ❌ | ❌ |

> [!NOTE]
> `ResponseFormat` only supports `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)`. Using `ChatResponseFormat.Json` (plain JSON mode without a schema) is not supported.

## Classes and interfaces

| Class / Interface | iOS | macOS | Mac Catalyst | tvOS |
|-------------------|-----|-------|--------------|------|
| `AppleIntelligenceChatClient` | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ |
| `NLEmbeddingGenerator` | ✅ 13.0+ | ✅ 10.15+ | ✅ 13.1+ | ✅ 13.0+ |
| `NLEmbeddingExtensions` | ✅ 13.0+ | ✅ 10.15+ | ✅ 13.1+ | ✅ 13.0+ |

## See also

- [Requirements](requirements.md)
- [Platform APIs](platform-apis.md)
- [Getting started](getting-started.md)
