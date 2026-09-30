---
title: Text embeddings on Apple platforms
description: Choose Apple Natural Language embedding models and build compatible semantic-search indexes with NLEmbeddingGenerator.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Text embeddings on Apple platforms

`NLEmbeddingGenerator` wraps Apple's Natural Language `NLEmbedding` behind `Microsoft.Extensions.AI.IEmbeddingGenerator<string, Embedding<float>>`. It does not use Apple Intelligence or Foundation Models. The availability of the `NLEmbedding` API and of a particular model are different; consult the [embedding feature comparison](feature-comparison.md) before choosing a deployment target.

## Choose a language and model

The default constructor requests an English *sentence* embedding. To request another language, construct the generator with an `NLLanguage`; check that a sentence model is available for that language and OS before depending on it. The generator does not detect the input language or switch models automatically.

Apple positions *sentence* models for comparing phrases and passages (including FAQ matching and text retrieval), and *word* models for individual-word similarity and query expansion with a fixed vocabulary. For sentence-level search, store and search your own text/vector index rather than relying on native nearest-neighbor vocabulary enumeration. See [Apple's guidance on choosing embeddings](https://developer.apple.com/documentation/naturallanguage/finding-similarities-between-pieces-of-text).

```csharp
using Microsoft.Maui.Essentials.AI;
using NaturalLanguage;

NLEmbedding nativeEmbedding = NLEmbedding.GetSentenceEmbedding(NLLanguage.German)
    ?? throw new NotSupportedException("A German sentence embedding isn't available.");
var germanGenerator = new NLEmbeddingGenerator(nativeEmbedding);
var embeddings = await germanGenerator.GenerateAsync(["Guten Morgen"]);
```

The language constructor (`new NLEmbeddingGenerator(NLLanguage.German)`) also requests a sentence model, but probing first lets the app provide its own unavailable-model message. Language availability isn't universal: on one tested macOS 26.7 host, English, German, and Italian sentence/word models were available, while several other languages returned no model. Check the actual device rather than hard-coding that observation as a support list.

### Use a native embedding

You can supply an existing native `NLEmbedding` (including a word embedding) to the generator, or adapt it with `AsIEmbeddingGenerator()` in the `Microsoft.Extensions.AI` namespace:

```csharp
using Microsoft.Extensions.AI;
using NaturalLanguage;

NLEmbedding? nativeEmbedding = NLEmbedding.GetSentenceEmbedding(NLLanguage.English);
if (nativeEmbedding is not null)
{
    IEmbeddingGenerator<string, Embedding<float>> generator =
        nativeEmbedding.AsIEmbeddingGenerator();
}
```

Choose a model appropriate for the text you're comparing. Don't switch between sentence and word models merely because a query is short: the indexed text and query must use compatible embeddings. `NLContextualEmbedding` is a separate Apple API for feature extraction in tasks such as classification and tagging; it isn't exposed by `NLEmbeddingGenerator` or an automatic semantic-search upgrade. Its [subword-token sequence limit](https://developer.apple.com/documentation/naturallanguage/nlcontextualembedding/maximumsequencelength) must not be applied to `NLEmbedding` sentence models.

## Keep indexes compatible

Create each index with a consistent model, language, and revision, and embed search queries using the same configuration. Record the native model's language, revision, and vector dimension with your index and rebuild the index when its model changes. A general provider identifier such as `natural-language` does not identify an interchangeable vector space. Don't compare vectors from different languages, sentence/word models, or model revisions as if they were in one shared space.

The generator does not automatically chunk or normalize input. Split app content into meaningful, consistent records for the selected model, and store the source text or identifier alongside each vector. If a generated vector is empty, don't insert it into the index or pass it to cosine similarity; report or handle the missing embedding explicitly.

Also reject non-finite and all-zero vectors, and check dimensions before comparing vectors. On the tested host, native sentence embeddings returned no vector for empty or whitespace input; the current wrapper converts a missing native vector into an empty vector. A nonempty vector doesn't prove that all details of a long passage were preserved. No `NLEmbedding` length/token cutoff was established by the local probes; the separate `NLContextualEmbedding` token limit isn't applicable here.

### Evaluate retrieval with your app's content

Use labeled queries and passages from your own domain to compare chunking choices and rank the expected results. Try meaningful sentence or passage boundaries, relevant headings, and overlap where context spans boundaries; don't treat a playground's fixed chunk size as an Apple limit or a universal optimum.

In a **small synthetic local evaluation** on macOS 26.7 (Apple Silicon), an English sentence model (revision 1, 512 dimensions) ranked the intended result first for 20 of 24 fixed query/document pairs and in the top three for 23 of 24. Categories were paraphrase 6/6, app intent 5/6, technical 3/4, and adversarial opposites 6/8 at rank one. For example, the unpaid-invoice query ranked its intended document fourth. These fixtures illustrate what can go wrong, **not** a general accuracy or production-quality benchmark. A native-versus-.NET-wrapper parity check matched vectors on 5/5 dedicated cases on the same host; it doesn't establish behavior on every device, model, or input. The word model there was revision 1 with 300 dimensions and must not share the sentence index. See the [retained corpus, runner, and results](https://github.com/dotnet/maui-labs/tree/d3b6be534a4a09e7804c8762a1d3f52f73a6ff9b/tests/AI/AppleEmbeddingEvaluation) for test inputs and ranks.

In a separate, matched position test, the **same six** synthetic queries were run against nine deliberately repetitive documents with the relevant detail placed early, in the middle, or late. The figures below show top-one retrievals out of six *per placement*, not 18 independent test cases:

| Chunking strategy | Early | Middle | Late |
|-------------------|-------|--------|------|
| Whole document | 4/6 | 5/6 | 2/6 |
| Fixed 180 characters | 5/6 | 6/6 | 6/6 |
| Playground 360 characters | 2/6 | 1/6 | 5/6 |
| Fixed 720 characters | 1/6 | 2/6 | 2/6 |
| One sentence | 5/6 | 5/6 | 5/6 |
| Three sentences, one overlapping | 4/6 | 4/6 | 4/6 |

This limited corpus favors some splits over others but cannot establish a universal best chunk size or a model token limit. Use [the retained inputs, ranks, and results](https://github.com/dotnet/maui-labs/blob/d3b6be534a4a09e7804c8762a1d3f52f73a6ff9b/tests/AI/AppleEmbeddingEvaluation/README.md) to reproduce the example, then measure your own content.

## Search locally

Generate document embeddings when records are ingested or changed, and use the same generator to embed a query. Rank comparable, nonempty vectors using cosine similarity. The similarity score is a ranking measure, **not a probability or percentage of correctness**. See the [semantic similarity search example](../embeddings.md#semantic-similarity-search).

## See also

- [Text embeddings](../embeddings.md)
- [Embedding feature comparison](feature-comparison.md)
- [Apple requirements](../requirements-apple.md)
