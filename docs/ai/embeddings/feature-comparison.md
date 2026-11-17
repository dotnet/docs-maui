---
title: Text embedding feature comparison
description: Compare the Microsoft.Extensions.AI embedding abstraction with the Apple NLEmbeddingGenerator implementation in Microsoft.Maui.Essentials.AI.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Text embedding feature comparison

`IEmbeddingGenerator<string, Embedding<float>>` gives your app one API for turning text into vectors. On-device text embeddings are currently available only on Apple platforms. On Windows and Android, use another `IEmbeddingGenerator` provider.

| Platform | Generator | Model |
|----------|-----------|-------|
| iOS 13+, Mac Catalyst 13.1+, and macOS 10.15+ | `NLEmbeddingGenerator` | Apple Natural Language (`NLEmbedding`) |
| Windows | Not supported | Not applicable |
| Android | Not supported | Not applicable |

`NLEmbeddingGenerator` doesn't require Apple Intelligence. For device and project requirements, see [Requirements](../requirements.md).

## Capabilities

| Capability | Apple | Windows | Android |
|------------|-------|---------|---------|
| Generate vectors | Supported | Not supported | Not supported |
| Batch inputs | Multiple strings per request; requests on one generator run sequentially | Not supported | Not supported |
| Model selection | Language or native model, set in the constructor rather than per request | Not supported | Not supported |
| Vector dimensions | Depend on the selected model | Not supported | Not supported |

Vectors from different providers or models aren't interchangeable. Generate queries and indexed content with the same model.

## Models and languages

The default generator uses an English sentence model. Other languages depend on the models installed on the device. You can also supply a native word model. See [Choose a language and model](apple.md#choose-a-language-and-model).

## See also

- [Text embeddings](../embeddings.md)
- [Text embeddings on Apple platforms](apple.md)
- [Requirements](../requirements.md)
