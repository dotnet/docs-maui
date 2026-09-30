---
title: Text embedding feature comparison
description: Compare the Microsoft.Extensions.AI embedding abstraction with the Apple NLEmbeddingGenerator implementation.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Text embedding feature comparison

`Microsoft.Extensions.AI.IEmbeddingGenerator<string, Embedding<float>>` lets an app request embeddings without committing to a model provider. `NLEmbeddingGenerator` is the `Microsoft.Maui.Essentials.AI` implementation backed by Apple's Natural Language framework (`NLEmbedding`), *not* Foundation Models or Apple Intelligence. The comparison below starts with capabilities expressed by the abstraction, then describes the Apple implementation; other providers may handle the same capabilities differently.

## Abstraction versus Apple implementation

| Capability | `IEmbeddingGenerator` abstraction | Apple `NLEmbeddingGenerator` |
|------------|-----------------------------------|------------------------------|
| Generate vectors | `GenerateAsync` returns typed embeddings for supplied strings | Embeds strings with a native `NLEmbedding` |
| Batch inputs | `GenerateAsync` accepts multiple strings in one request | Accepts multiple strings; concurrent requests are serialized per instance |
| Model selection | Generation options can carry a model ID; provider interpretation varies | The native model is chosen when constructing the generator, not switched per request |
| Vector output | Each embedding exposes a float vector | Returns vectors from the selected native word or sentence model; model, language, revision, and dimensions must match the search index |

The interface doesn't make vectors from different providers or models comparable. For Apple-specific model selection and persistent search indexes, see [Embeddings on Apple platforms](apple.md).

## Platform and model availability

| Platform | Native word embedding API introduced | Native sentence embedding API introduced |
|----------|--------------------------------------|------------------------------------------|
| iOS | 13 | 14 |
| macOS | 10.15 | 11 |
| Mac Catalyst | 13.1 | 14 |

These native API introduction versions are **not** guarantees of shipped-package support or model availability for a given language. The default `NLEmbeddingGenerator` requests the English *sentence* model, not the older word model. While [MAUI Labs managed target settings](https://github.com/dotnet/maui-labs/blob/main/Directory.Build.props) specify iOS/Mac Catalyst 15 and macOS 14, its native bridge project specifies iOS/macOS 26 deployment targets. Older-runtime loading and model availability haven't been established; this evaluation exercised macOS 26.7 only. The [current package source](https://github.com/dotnet/maui-labs/tree/main/src/AI) targets iOS, macOS, and Mac Catalyst, not tvOS. Android and Windows don't have an `NLEmbeddingGenerator` implementation.

The default Apple model is an English sentence embedding. You can request another available sentence language or supply a native embedding, including a word model. See [Embeddings on Apple platforms](apple.md) for index compatibility and missing vectors, and [Text embeddings](../embeddings.md) for interface-based examples.

For Apple's native API availability, see [`wordEmbedding(for:)`](https://developer.apple.com/documentation/naturallanguage/nlembedding/wordembedding(for:)) and [`sentenceEmbedding(for:)`](https://developer.apple.com/documentation/naturallanguage/nlembedding/sentenceembedding(for:)).

## See also

- [Apple requirements](../requirements-apple.md)
- [AI feature comparison](../feature-comparison.md)
