---
title: Get started with text embeddings
description: Learn how to install, register, and use NLEmbeddingGenerator to generate and compare on-device text embeddings in your .NET MAUI app using Apple's Natural Language framework.
ms.date: 03/11/2026
---

# Get started with text embeddings

`NLEmbeddingGenerator` converts strings into float vectors using Apple's **Natural Language** framework (`NLEmbedding`). It implements `IEmbeddingGenerator<string, Embedding<float>>` from `Microsoft.Extensions.AI`. The embeddings capture the semantic meaning of sentences and are well-suited for semantic search, clustering, and retrieval-augmented generation (RAG).

> [!NOTE]
> `NLEmbeddingGenerator` is available on iOS 13.0+, macOS 10.15+, Mac Catalyst 13.1+, and tvOS 13.0+. It does **not** require Apple Intelligence or an OS version of 26. Android and Windows support are planned for future releases.

## Installation

Add the NuGet package to your .NET MAUI project:

```dotnetcli
dotnet add package Microsoft.Maui.Essentials.AI
```

Or via PackageReference:

```xml
<PackageReference Include="Microsoft.Maui.Essentials.AI" Version="10.0.*-*" />
```

Suppress the experimental warning project-wide:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);MAUIAI0001</NoWarn>
</PropertyGroup>
```

## Register AI services

### Extension method (recommended)

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

Create an extension method on `MauiAppBuilder` to register the embedding generator:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder AddEmbeddingServices(this MauiAppBuilder builder)
    {
        // Register the Natural Language embedding generator
        builder.Services.AddSingleton<NLEmbeddingGenerator>();

        // Register IEmbeddingGenerator with logging middleware
        builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
        {
            var embeddings = sp.GetRequiredService<NLEmbeddingGenerator>();
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            return embeddings.AsBuilder()
                .UseLogging(loggerFactory)
                .Build();
        });

        return builder;
    }
}
```

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

### Extension method on NLEmbedding (alternative)

If you already have a native `NLEmbedding` instance, use the `AsIEmbeddingGenerator()` extension from the `Microsoft.Extensions.AI` namespace:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
using NaturalLanguage;
using Microsoft.Extensions.AI; // required for AsIEmbeddingGenerator()

NLEmbedding nativeEmbedding = NLEmbedding.GetSentenceEmbedding(NLLanguage.English)!;
IEmbeddingGenerator<string, Embedding<float>> generator = nativeEmbedding.AsIEmbeddingGenerator();
```

> [!NOTE]
> `AsIEmbeddingGenerator()` is an extension method on `NLEmbedding` provided by `NLEmbeddingExtensions`, which lives in the `Microsoft.Extensions.AI` namespace. Add `using Microsoft.Extensions.AI;` to resolve it.

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

### Call from MauiProgram.cs

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

#if IOS || MACCATALYST
        builder.AddEmbeddingServices();
#endif

        return builder.Build();
    }
}
```

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Generate embeddings

Inject `IEmbeddingGenerator<string, Embedding<float>>` and call `GenerateAsync`:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
using Microsoft.Extensions.AI;

var generator = serviceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

// Embed one or more strings in a single call
var embeddings = await generator.GenerateAsync(["Hello, world!"]);
ReadOnlyMemory<float> vector = embeddings[0].Vector;

Console.WriteLine($"Embedding dimensions: {vector.Length}");
```

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Semantic similarity search

Use cosine similarity to find the most semantically similar items in a collection:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
using System.Numerics.Tensors;
using Microsoft.Extensions.AI;

var generator = serviceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

// Embed the user's query
var queryEmbedding = await generator.GenerateAsync(["Find restaurants nearby"]);

// Compare against pre-embedded documents and rank by similarity
var results = documents
    .Select(doc => (doc, similarity: TensorPrimitives.CosineSimilarity(
        queryEmbedding[0].Vector.Span,
        doc.Embedding.Vector.Span)))
    .OrderByDescending(x => x.similarity)
    .Take(5)
    .ToList();

foreach (var (doc, score) in results)
    Console.WriteLine($"{doc.Title} ({score:P1})");
```

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Multiple languages

`NLEmbeddingGenerator` accepts a language parameter to use the embedding model for a specific language:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
using NaturalLanguage;
using Microsoft.Maui.Essentials.AI;

// Default: English
var englishGenerator = new NLEmbeddingGenerator();

// Specific language
var frenchGenerator = new NLEmbeddingGenerator(NLLanguage.French);
```

Throws `NotSupportedException` if sentence embeddings are not available for the requested language.

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Next steps

- [Chat client](getting-started-chat.md) — build on-device conversational AI with Apple Intelligence
- [Agent framework integration](agent-framework.md) — compose multiple agents into multi-stage workflows
- [Feature comparison](feature-comparison.md) — supported features and embedding options across platforms
- [Requirements](requirements.md) — supported OS versions and device requirements
