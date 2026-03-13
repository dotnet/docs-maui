---
title: Text embeddings
description: Use IEmbeddingGenerator from Microsoft.Maui.Essentials.AI to generate on-device text embeddings for semantic search and similarity in .NET MAUI.
ms.date: 03/11/2026
ms.topic: conceptual
---

# Text embeddings

This page shows how to use [`IEmbeddingGenerator<string, Embedding<float>>`](/dotnet/ai/iembeddinggenerator) in a .NET MAUI app once services are registered. For setup and registration, see [Get started](getting-started.md). For platform requirements, see [Requirements](requirements-apple.md).

The `IEmbeddingGenerator` interface is part of `Microsoft.Extensions.AI`. All examples on this page use the interface directly and work regardless of the underlying platform implementation.

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
    ["Hello, world!", "Goodbye, world!", "Bonjour le monde"]);

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
- [Feature comparison](feature-comparison.md)
- [Use the IEmbeddingGenerator interface](/dotnet/ai/iembeddinggenerator)
- [Microsoft.Extensions.AI overview](/dotnet/ai/ai-extensions)
