---
title: Text embeddings
description: Use IEmbeddingGenerator from Microsoft.Maui.Essentials.AI to generate on-device text embeddings for semantic search and similarity in .NET MAUI.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Text embeddings

This page shows how to use [`IEmbeddingGenerator<string, Embedding<float>>`](/dotnet/ai/iembeddinggenerator) in a .NET MAUI app once services are registered. For setup and registration, see [Get started](getting-started.md). For platform and model availability, see the [embedding feature comparison](embeddings/feature-comparison.md). For model selection and search practices, see [Embeddings on Apple platforms](embeddings/apple.md).

The `IEmbeddingGenerator` interface is part of `Microsoft.Extensions.AI`. The examples use the interface directly, but available models, languages, and vector compatibility depend on the provider. See the [embedding feature comparison](embeddings/feature-comparison.md).

## Generate embeddings

Call `GenerateAsync` with a list of strings to produce embedding vectors:

```csharp
using Microsoft.Extensions.AI;

IEmbeddingGenerator<string, Embedding<float>> generator = // resolved from DI

string text = "Hello, world!";
var embeddings = await generator.GenerateAsync([text]);
ReadOnlyMemory<float> vector = embeddings[0].Vector;
Console.WriteLine($"Dimensions: {vector.Length}");
```

You can embed multiple strings in a single call:

```csharp
var embeddings = await generator.GenerateAsync(
    ["Hello, world!", "Goodbye, world!", "Good morning!"]);

for (int i = 0; i < embeddings.Count; i++)
    Console.WriteLine($"[{i}] dimensions: {embeddings[i].Vector.Length}");
```

## Semantic similarity search

Use cosine similarity to rank items by how semantically close they are to a query:

```csharp
using System.Numerics.Tensors;
using Microsoft.Extensions.AI;

IEmbeddingGenerator<string, Embedding<float>> generator = // resolved from DI

string query = "Find nearby restaurants";
var queryEmbeddings = await generator.GenerateAsync([query]);

// Compare against validated, pre-embedded documents and rank by similarity
var results = documents
    .Select(doc => (doc, score: TensorPrimitives.CosineSimilarity(
        queryEmbeddings[0].Vector.Span,
        doc.Embedding.Span)))
    .OrderByDescending(x => x.score)
    .Take(5);

foreach (var (doc, score) in results)
    Console.WriteLine($"{doc.Title} — similarity: {score:F3}");
```

The example assumes a usable query vector and an index that was validated when built or loaded. Before comparing vectors, reject empty, non-finite, zero, or mismatched-dimension vectors, and confirm the index uses the same language, model, and revision as the query. Don't silently rank incompatible vectors. The score ranks comparable vectors; it isn't a probability. See [Embeddings on Apple platforms](embeddings/apple.md#keep-indexes-compatible) before building a persistent search index.

## See also

- [Chat client](chat.md)
- [Agent framework integration](agent-framework.md)
- [Embedding feature comparison](embeddings/feature-comparison.md)
- [Embeddings on Apple platforms](embeddings/apple.md)
- [Use the IEmbeddingGenerator interface](/dotnet/ai/iembeddinggenerator)
- [Microsoft.Extensions.AI overview](/dotnet/ai/ai-extensions)
