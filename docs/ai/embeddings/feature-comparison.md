---
title: Text embedding feature comparison
description: Compare the Microsoft.Extensions.AI embedding abstraction with the Apple NLEmbeddingGenerator implementation in Microsoft.Maui.Essentials.AI.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Text embedding feature comparison

`IEmbeddingGenerator<string, Embedding<float>>` provides a common API for turning text into vectors. `NLEmbeddingGenerator` implements it using Apple's on-device Natural Language framework. Windows and Android don't have an on-device embedding generator.

| Capability | `IEmbeddingGenerator` | `NLEmbeddingGenerator` |
|------------|-----------------------|------------------------|
| Generate vectors | `GenerateAsync` returns embeddings for supplied strings | Uses the selected native `NLEmbedding` |
| Batch inputs | Accepts multiple inputs per request | Supports multiple strings; requests on one generator run sequentially |
| Model selection | Generation options can specify a model ID | Select the language or model in the constructor, not per request |
| Vector output | Returns float vectors | Dimensions depend on the selected native model |

Vectors from different providers or models aren't interchangeable. Generate queries and indexed content with the same model.

## Platform and model availability

`NLEmbeddingGenerator` is available on iOS 13, Mac Catalyst 13.1, and macOS 10.15 or later. It doesn't require Apple Intelligence.

The default generator uses an English sentence model. Other languages depend on the models installed on the device. You can also supply a native word model. See [Choose a language and model](apple.md#choose-a-language-and-model).

## See also

- [Text embeddings](../embeddings.md)
- [Text embeddings on Apple platforms](apple.md)
- [Requirements](../requirements.md)
