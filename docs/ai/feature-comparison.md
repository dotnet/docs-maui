---
title: Feature comparison
description: Feature availability comparison for AppleIntelligenceChatClient, NLEmbeddingGenerator, and NLEmbeddingExtensions across all platforms supported by Microsoft.Maui.Essentials.AI.
ms.date: 03/11/2026
---

# Feature comparison

This page documents the APIs provided by `Microsoft.Maui.Essentials.AI` and compares feature availability across all platforms.

> [!NOTE]
> Apple Intelligence is the first implementation available. Android and Windows support are planned for future releases.

> [!IMPORTANT]
> `Microsoft.Maui.Essentials.AI` is experimental and identified by diagnostic `MAUIAI0001`. See [Get started](getting-started.md#suppress-the-experimental-warning) for how to suppress it.

## Chat client

### Platform availability

| Platform | Class | Minimum version |
|----------|-------|-----------------|
| iOS | `AppleIntelligenceChatClient` | 26.0 |
| macOS | `AppleIntelligenceChatClient` | 26.0 |
| Mac Catalyst | `AppleIntelligenceChatClient` | 26.0 |
| tvOS | `AppleIntelligenceChatClient` | 26.0 |

### Chat capabilities

| Feature | iOS / macOS / Mac Catalyst / tvOS |
|---------|-----------------------------------|
| Text generation | ✅ |
| Streaming responses | ✅ |
| Tool / function calling | ✅ (`AIFunction` only) |
| Structured JSON output | ✅ |
| System prompts | ✅ |
| Multi-turn conversations | ✅ |
| Image input | ❌ |

### Supported ChatOptions

`ChatOptions` properties honored by `AppleIntelligenceChatClient`. All other properties are silently ignored.

| Option | iOS / macOS / Mac Catalyst / tvOS |
|--------|-----------------------------------|
| `Temperature` | ✅ |
| `TopK` | ✅ |
| `Seed` | ✅ |
| `MaxOutputTokens` | ✅ |
| `ResponseFormat` (JSON schema) | ✅ |
| `Tools` (`AIFunction`) | ✅ |
| `TopP` | ❌ ignored |
| `FrequencyPenalty` | ❌ ignored |
| `PresencePenalty` | ❌ ignored |

> [!IMPORTANT]
> Two constraints apply when using `AppleIntelligenceChatClient`:
>
> 1. **Tool types:** Only `AIFunction` tools are supported. Other `AITool` subtypes are not supported.
> 2. **Structured JSON output:** Use `GetResponseAsync<T>()` or set `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)` in `ChatOptions`. Plain `ChatResponseFormat.Json` without a schema is **not** supported.

### Supported message content types

| Content type | iOS / macOS / Mac Catalyst / tvOS |
|--------------|-----------------------------------|
| `TextContent` | ✅ |
| `FunctionCallContent` | ✅ |
| `FunctionResultContent` | ✅ |
| `ImageContent` | ❌ |

## Embedding generator

### Platform availability

| Platform | Class | Minimum version |
|----------|-------|-----------------|
| iOS | `NLEmbeddingGenerator` | 13.0 |
| macOS | `NLEmbeddingGenerator` | 10.15 |
| Mac Catalyst | `NLEmbeddingGenerator` | 13.1 |
| tvOS | `NLEmbeddingGenerator` | 13.0 |

### Embedding capabilities

| Feature | iOS / macOS / Mac Catalyst / tvOS |
|---------|-----------------------------------|
| Sentence-level embeddings | ✅ |
| Multiple languages | ✅ |
| Custom `NLEmbedding` instance | ✅ |
| Concurrent requests | ✅ (serialized per instance) |

> [!NOTE]
> `NLEmbeddingGenerator` uses Apple's *sentence* embedding model (`NLEmbedding.GetSentenceEmbedding`), which is optimized for comparing full sentences or short passages rather than individual words. This makes it well-suited for semantic similarity search over descriptive text.

> [!TIP]
> `NLEmbeddingGenerator` serializes concurrent calls with a `SemaphoreSlim(1, 1)`. For high-throughput scenarios, create multiple instances and distribute work across them.

## Platforms

### Apple (iOS/Mac Catalyst)

#### NLEmbeddingExtensions

`NLEmbeddingExtensions` wraps a native `NLEmbedding` as an `IEmbeddingGenerator<string, Embedding<float>>`.

**Namespace:** `Microsoft.Extensions.AI` *(not `Microsoft.Maui.Essentials.AI`)*

> [!NOTE]
> Add `using Microsoft.Extensions.AI;` to your file to call the `AsIEmbeddingGenerator()` extension method.

| Method | Description |
|--------|-------------|
| `AsIEmbeddingGenerator(this NLEmbedding embedding)` | Wraps a native `NLEmbedding` as an `IEmbeddingGenerator<string, Embedding<float>>`. |

```csharp
using NaturalLanguage;
using Microsoft.Extensions.AI;

NLEmbedding nativeEmbedding = NLEmbedding.GetSentenceEmbedding(NLLanguage.English)!;
IEmbeddingGenerator<string, Embedding<float>> generator = nativeEmbedding.AsIEmbeddingGenerator();
```

#### Multiple languages

`NLEmbeddingGenerator` supports multiple languages through Apple's Natural Language framework. Specify a language when constructing the generator:

```csharp
using NaturalLanguage;
using Microsoft.Maui.Essentials.AI;

// Default: English
var englishGenerator = new NLEmbeddingGenerator();

// Specific language
var frenchGenerator = new NLEmbeddingGenerator(NLLanguage.French);
var embeddings = await frenchGenerator.GenerateAsync(["Bonjour le monde"]);
```

Throws `NotSupportedException` if sentence embeddings are not available for the requested language.

## See also

- [Requirements](requirements-apple.md)
- [Chat](chat.md)
- [Text embeddings](embeddings.md)
- [Use the IChatClient interface](/dotnet/ai/ichatclient)
- [Use the IEmbeddingGenerator interface](/dotnet/ai/iembeddinggenerator)
- [`IChatClient` API reference](/dotnet/api/microsoft.extensions.ai.ichatclient)
- [`IEmbeddingGenerator<TInput,TEmbedding>` API reference](/dotnet/api/microsoft.extensions.ai.iembeddinggenerator-2)
