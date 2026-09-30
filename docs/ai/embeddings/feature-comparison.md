---
title: Text embedding feature comparison
description: Compare the Microsoft.Extensions.AI embedding abstraction with the Apple NLEmbeddingGenerator implementation.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Text embedding feature comparison

`IEmbeddingGenerator<string, Embedding<float>>` provides a common API for turning text into vectors. `NLEmbeddingGenerator` implements it using Apple's on-device Natural Language framework. The following table shows how this implementation handles the abstraction's capabilities.

## Abstraction versus Apple implementation

| Capability | `IEmbeddingGenerator` | `NLEmbeddingGenerator` |
|------------|-----------------------------------|------------------------------|
| Generate vectors | `GenerateAsync` returns embeddings for supplied strings | Uses the selected native `NLEmbedding` |
| Batch inputs | Accepts multiple inputs per request | Supports multiple strings; requests on one generator run sequentially |
| Model selection | Generation options can specify a model ID | Select the model in the constructor, not per request |
| Vector output | Returns float vectors | Dimensions depend on the selected native model |

Changing providers doesn't make their vectors interchangeable. Generate your queries and indexed content with the same model. See [Embeddings on Apple platforms](apple.md) for model selection and indexing guidance.

## Platform and model availability

The package provides this implementation for iOS, macOS, and Mac Catalyst, not Android, Windows, or tvOS. Natural Language embeddings don't require Apple Intelligence.

The default generator uses an English sentence model. Other languages depend on the models available on the device. You can also supply a native word model. See [Apple requirements](../requirements-apple.md) for deployment requirements and [Embeddings on Apple platforms](apple.md#choose-a-language-and-model) for model selection.

## See also

- [Apple requirements](../requirements-apple.md)
- [AI feature comparison](../feature-comparison.md)
