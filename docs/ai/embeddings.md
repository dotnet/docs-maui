---
title: Text embeddings
description: Use IEmbeddingGenerator from Microsoft.Maui.Essentials.AI to generate on-device text embeddings for semantic search and similarity in .NET MAUI.
ms.date: 03/11/2026
ms.topic: conceptual
---

# Text embeddings

This page shows how to use `IEmbeddingGenerator<string, Embedding<float>>` in a .NET MAUI app once services are registered. For setup and registration, see [Get started](getting-started.md). For platform requirements, see [Apple requirements](requirements-apple.md).

The `IEmbeddingGenerator` interface is part of `Microsoft.Extensions.AI`. All examples on this page use the interface directly and work regardless of the underlying platform implementation.

`NLEmbeddingGenerator` uses Apple's **sentence** embedding model (`NLEmbedding.GetSentenceEmbedding`), which is optimized for comparing full sentences and short passages — making it well-suited for semantic search over descriptive text.

## Generate embeddings

Inject `IEmbeddingGenerator<string, Embedding<float>>` and call `GenerateAsync` with a list of strings:

```csharp
using Microsoft.Extensions.AI;

public class EmbeddingService(IEmbeddingGenerator<string, Embedding<float>> generator)
{
    public async Task<ReadOnlyMemory<float>> EmbedAsync(string text)
    {
        var embeddings = await generator.GenerateAsync([text]);
        return embeddings[0].Vector;
    }
}
```

You can embed multiple strings in a single call:

```csharp
var embeddings = await generator.GenerateAsync(
    ["Hello, world!", "Goodbye, world!", "Bonjour le monde"]);

Console.WriteLine($"Dimensions: {embeddings[0].Vector.Length}");
```

## Semantic similarity search

Use cosine similarity to rank items by how semantically close they are to a query:

```csharp
using System.Numerics.Tensors;
using Microsoft.Extensions.AI;

// Embed the user's query
var queryEmbeddings = await generator.GenerateAsync(["Find nearby restaurants"]);

// Compare against pre-embedded documents and rank by similarity
var results = documents
    .Select(doc => (doc, score: TensorPrimitives.CosineSimilarity(
        queryEmbeddings[0].Vector.Span,
        doc.Embedding.Span)))
    .OrderByDescending(x => x.score)
    .Take(5);

foreach (var (doc, score) in results)
    Console.WriteLine($"{doc.Title} — {score:P1}");
```

## See also

- [Chat client](chat.md)
- [Agent framework integration](agent-framework.md)
- [API reference and features](feature-comparison.md)
