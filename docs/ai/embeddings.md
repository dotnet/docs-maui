---
title: Text embeddings
description: Use IEmbeddingGenerator from Microsoft.Maui.Essentials.AI to generate on-device text embeddings for semantic search and similarity in .NET MAUI.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Text embeddings

This article shows how to use [`IEmbeddingGenerator<string, Embedding<float>>`](/dotnet/ai/iembeddinggenerator) in a .NET MAUI app after you [register the embedding generator](getting-started.md#register-the-embedding-generator). On-device text embeddings are available on Apple platforms. Available models, languages, and vector compatibility depend on the provider. See the [embedding feature comparison](embeddings/feature-comparison.md) and [Text embeddings on Apple platforms](embeddings/apple.md).

## Generate embeddings

Call `GenerateAsync` with a list of strings to produce embedding vectors:

```csharp
using Microsoft.Extensions.AI;

IEmbeddingGenerator<string, Embedding<float>> generator = // resolved from DI

var embeddings = await generator.GenerateAsync(["Hello, world!", "Good morning!"]);
foreach (var embedding in embeddings)
{
    Console.WriteLine($"Dimensions: {embedding.Vector.Length}");
}
```

## Semantic similarity search

Use cosine similarity to rank items by how semantically close they are to a query:

```csharp
using System.Numerics.Tensors;
using Microsoft.Extensions.AI;

var queryEmbeddings = await generator.GenerateAsync(["Find nearby restaurants"]);

// Compare against validated, pre-embedded documents and rank by similarity
var results = documents
    .Select(doc => (doc, score: TensorPrimitives.CosineSimilarity(
        queryEmbeddings[0].Vector.Span,
        doc.Embedding.Span)))
    .OrderByDescending(x => x.score)
    .Take(5);

foreach (var (doc, score) in results)
{
    Console.WriteLine($"{doc.Title}: {score:F3}");
}
```

Compare only vectors from the same language, model, and model revision. Before comparing, reject empty, non-finite, all-zero, or mismatched-dimension vectors. The score ranks comparable vectors; it isn't a probability. Before you build a persistent search index, see [Keep indexes compatible](embeddings/apple.md#keep-indexes-compatible).

## See also

- [Embedding feature comparison](embeddings/feature-comparison.md)
- [Text embeddings on Apple platforms](embeddings/apple.md)
- [Use the IEmbeddingGenerator interface](/dotnet/ai/iembeddinggenerator)
