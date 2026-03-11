---
title: Feature comparison
description: Compares the AI features available across platforms supported by Microsoft.Maui.Essentials.AI.
ms.date: 05/14/2026
---

# Feature comparison

This page compares the AI features available across all platforms targeted by `Microsoft.Maui.Essentials.AI`.

> [!NOTE]
> `Microsoft.Maui.Essentials.AI` is designed as a cross-platform library. Apple Intelligence is the first implementation available. Android and Windows support are not yet available.

> [!IMPORTANT]
> `Microsoft.Maui.Essentials.AI` is experimental and is identified by the diagnostic ID `MAUIAI0001`. To use it, suppress the diagnostic or opt in explicitly.

## Chat client

| Platform | Minimum version | Chat client class | Notes |
|----------|-----------------|-------------------|-------|
| iOS | 26.0+ | `AppleIntelligenceChatClient` | Requires Apple Intelligence |
| macOS | 26.0+ | `AppleIntelligenceChatClient` | Requires Apple Intelligence |
| Mac Catalyst | 26.0+ | `AppleIntelligenceChatClient` | Requires Apple Intelligence |
| tvOS | 26.0+ | `AppleIntelligenceChatClient` | Requires Apple Intelligence |
| Android | Not available | — | Planned for a future release |
| Windows | Not available | — | Planned for a future release |

## Embedding generator

| Platform | Minimum version | Embedding class | Notes |
|----------|-----------------|-----------------|-------|
| iOS | 13.0+ | `NLEmbeddingGenerator` | Does not require Apple Intelligence |
| macOS | 10.15+ | `NLEmbeddingGenerator` | Does not require Apple Intelligence |
| Mac Catalyst | 13.1+ | `NLEmbeddingGenerator` | Does not require Apple Intelligence |
| tvOS | 13.0+ | `NLEmbeddingGenerator` | Does not require Apple Intelligence |
| Android | Not available | — | Planned for a future release |
| Windows | Not available | — | Planned for a future release |

## Chat capabilities

| Feature | iOS | macOS | Mac Catalyst | tvOS | Android | Windows |
|---------|-----|-------|--------------|------|---------|---------|
| Text generation | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | Not available | Not available |
| Streaming responses | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | Not available | Not available |
| Tool/function calling | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | Not available | Not available |
| Structured JSON output | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | Not available | Not available |
| System prompts | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | Not available | Not available |
| Multi-turn conversations | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | Not available | Not available |
| Image input | ❌ | ❌ | ❌ | ❌ | Not available | Not available |

## Embedding capabilities

| Feature | iOS | macOS | Mac Catalyst | tvOS | Android | Windows |
|---------|-----|-------|--------------|------|---------|---------|
| Text embeddings | ✅ 13.0+ | ✅ 10.15+ | ✅ 13.1+ | ✅ 13.0+ | Not available | Not available |
| Multiple languages | ✅ | ✅ | ✅ | ✅ | Not available | Not available |
| Custom embedding model | ✅ | ✅ | ✅ | ✅ | Not available | Not available |

## Supported ChatOptions

`ChatOptions` properties honored by the Apple Intelligence chat client. Options marked ❌ are silently ignored on Apple. Android and Windows chat clients are not yet available.

| Option | iOS | macOS | Mac Catalyst | tvOS | Android | Windows |
|--------|-----|-------|--------------|------|---------|---------|
| `Temperature` | ✅ | ✅ | ✅ | ✅ | Not available | Not available |
| `TopK` | ✅ | ✅ | ✅ | ✅ | Not available | Not available |
| `Seed` | ✅ | ✅ | ✅ | ✅ | Not available | Not available |
| `MaxOutputTokens` | ✅ | ✅ | ✅ | ✅ | Not available | Not available |
| `ResponseFormat` (JSON schema) | ✅ | ✅ | ✅ | ✅ | Not available | Not available |
| `Tools` (`AIFunction`) | ✅ | ✅ | ✅ | ✅ | Not available | Not available |
| `TopP` | ❌ | ❌ | ❌ | ❌ | Not available | Not available |
| `FrequencyPenalty` | ❌ | ❌ | ❌ | ❌ | Not available | Not available |
| `PresencePenalty` | ❌ | ❌ | ❌ | ❌ | Not available | Not available |

> [!NOTE]
> `ResponseFormat` only supports `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)`. Using `ChatResponseFormat.Json` (plain JSON mode without a schema) is not supported.

## Classes and interfaces

| Class / Interface | iOS | macOS | Mac Catalyst | tvOS | Android | Windows |
|-------------------|-----|-------|--------------|------|---------|---------|
| `AppleIntelligenceChatClient` | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | ✅ 26.0+ | Not available | Not available |
| `NLEmbeddingGenerator` | ✅ 13.0+ | ✅ 10.15+ | ✅ 13.1+ | ✅ 13.0+ | Not available | Not available |
| `NLEmbeddingExtensions` | ✅ 13.0+ | ✅ 10.15+ | ✅ 13.1+ | ✅ 13.0+ | Not available | Not available |

## See also

- [Requirements](requirements.md)
- [Platform APIs](platform-apis.md)
- [Getting started](getting-started.md)
