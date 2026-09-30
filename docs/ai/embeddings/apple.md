---
title: Text embeddings on Apple platforms
description: Choose Apple Natural Language embedding models and build compatible semantic-search indexes with NLEmbeddingGenerator.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Text embeddings on Apple platforms

Use `NLEmbeddingGenerator` to build on-device semantic search over content such as notes, help articles, and product descriptions. It implements `Microsoft.Extensions.AI.IEmbeddingGenerator<string, Embedding<float>>` using Apple's Natural Language framework. Unlike chat, it doesn't use Foundation Models or require Apple Intelligence.

This article explains how to choose an embedding model and prepare a search index. For the basic API, see [Text embeddings](../embeddings.md).

## Choose a language and model

The default constructor uses an English *sentence* embedding. Sentence models are a good starting point for matching a search query to a phrase or short passage, such as a help article's description.

To use another language, pass an `NLLanguage` to the constructor:

```csharp
using Microsoft.Maui.Essentials.AI;
using NaturalLanguage;

using var germanGenerator = new NLEmbeddingGenerator(NLLanguage.German);
var embeddings = await germanGenerator.GenerateAsync(["Guten Morgen"]);
```

The generator doesn't detect the input language or change models automatically. Available languages depend on the device and OS; check that the requested sentence model is available before enabling search for that language. See [Apple's embedding guidance](https://developer.apple.com/documentation/naturallanguage/finding-similarities-between-pieces-of-text).

### Use a native embedding

For individual-word similarity or query expansion, use a *word* embedding. Word models have a fixed vocabulary and aren't a substitute for sentence models when searching passages.

You can pass a native `NLEmbedding` to the generator or wrap it with `AsIEmbeddingGenerator()` in the `Microsoft.Extensions.AI` namespace. This example assumes an English word model is available:

```csharp
using Microsoft.Extensions.AI;
using NaturalLanguage;

using NLEmbedding nativeEmbedding = NLEmbedding.GetWordEmbedding(NLLanguage.English)!;
using IEmbeddingGenerator<string, Embedding<float>> generator =
    nativeEmbedding.AsIEmbeddingGenerator();
var embeddings = await generator.GenerateAsync(["coffee", "tea"]);
```

Native model lookup can return `null` when the model isn't available. The generator borrows the native instance, so keep it alive until the generator is disposed. The `using` declarations dispose the generator first.

Use the same kind of model for your indexed content and queries. A short query doesn't mean you should switch a sentence index to a word model.

## Keep indexes compatible

An embedding is useful for search only when it's compared with vectors from the same embedding space. Generate indexed content and search queries with the same language, model type, and revision. Equal vector lengths alone don't make two models compatible.

Store the configured language, word-or-sentence model type, model revision, and vector dimensions with your index. Keep the configured language even if the native model doesn't report one. The provider ID `natural-language` isn't a complete model identifier. Rebuild the index when you change the model, rather than comparing new query vectors with old document vectors.

For multilingual content, keep separate indexes for language-specific models and route each query to the appropriate one. Don't assume that different language models produce directly comparable vectors.

## Prepare content for search

Split long content into records that each cover one useful idea. For example, a help article about account settings might have separate records for changing an email address and resetting a password. This lets a query match the relevant passage instead of an entire article.

Start with sentence or paragraph boundaries. Include a heading when a passage needs context, and overlap neighboring passages when an idea spans a boundary. There isn't one chunk size that works best for all content; compare a few approaches with realistic queries and the results you expect them to find.

Store each vector with its source text and a stable record ID so you can display the result and open the original content. Generate embeddings when content is added or changed, not every time a user searches.

The generator doesn't split text or normalize it for you. Keep bulk indexing off the UI thread: calling an async method doesn't guarantee that all native work runs in the background.

### Check generated vectors

A model can return no vector for an input, and the generator represents that result as an empty vector. Handle it before adding the record to your index or calculating similarity. Also reject non-finite or all-zero vectors and mismatched dimensions.

A valid vector doesn't establish that every detail of a long document is represented. Check search quality using queries for details in different parts of your content, not just its title or opening sentence.

## Search locally

Embed the user's query with the same model used to build the index, then rank the stored vectors by cosine similarity. The score helps order results; it isn't a probability or a percentage of correctness. See the [semantic similarity search example](../embeddings.md#semantic-similarity-search).

Review the returned passages for common queries, ambiguous wording, and queries with no relevant answer. If your app uses a score threshold, choose it from those examples rather than treating a fixed score as universally meaningful.

## See also

- [Text embeddings](../embeddings.md)
- [Embedding feature comparison](feature-comparison.md)
- [Apple requirements](../requirements-apple.md)
