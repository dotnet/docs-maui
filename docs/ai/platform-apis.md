---
title: "Platform-specific AI APIs"
description: "Learn about the native AI APIs used by Microsoft.Maui.Essentials.AI on each platform including Apple Intelligence, Google ML Kit, and Windows AI."
ms.date: 02/02/2026
---

# Platform-specific AI APIs

Microsoft.Maui.Essentials.AI wraps platform-native AI capabilities behind standard `Microsoft.Extensions.AI` interfaces. This article describes the underlying APIs used on each platform.

## Apple platforms (iOS, macOS, Mac Catalyst, tvOS)

Apple platforms provide two distinct AI capabilities through different native frameworks.

### AppleIntelligenceChatClient

The `AppleIntelligenceChatClient` class implements `IChatClient` using Apple's **Foundation Models** framework, which provides access to on-device large language models as part of Apple Intelligence.

**Namespace**: `Microsoft.Maui.Essentials.AI`

**Native API**: [Foundation Models framework](https://developer.apple.com/documentation/foundationmodels)

```csharp
public class AppleIntelligenceChatClient : IChatClient
```

#### Capabilities

| Feature | Supported | Notes |
|---------|-----------|-------|
| Text generation | ✅ | Via `GetResponseAsync` |
| Streaming | ✅ | Via `GetStreamingResponseAsync` |
| Tool/function calling | ✅ | Native `AIToolNative` protocol |
| JSON structured output | ✅ | **Requires** JSON schema |
| System prompts | ✅ | Set via `ChatMessage` with `Author.System` |
| Multi-turn conversations | ✅ | Pass conversation history |

#### Supported options

The following `ChatOptions` properties are supported:

| Option | Type | Description |
|--------|------|-------------|
| `TopK` | `int?` | Limits token selection to top K candidates |
| `Seed` | `int?` | Random seed for reproducible outputs |
| `Temperature` | `double?` | Controls randomness (0.0 = deterministic, higher = more random) |
| `MaxOutputTokens` | `int?` | Maximum tokens in the response |
| `ResponseFormat` | `ChatResponseFormat` | Set to `ForJsonSchema<T>()` for structured output |
| `Tools` | `IList<AITool>` | Functions the model can call |

> [!IMPORTANT]
> Unlike cloud-based APIs like OpenAI, Apple Intelligence **requires** a JSON schema when requesting JSON output. Using `ChatResponseFormat.Json` without a schema will fail. Always use `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)`.

#### Example usage

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

// Basic chat
var client = new AppleIntelligenceChatClient();
var response = await client.GetResponseAsync("What is the capital of France?");
Console.WriteLine(response.Text);

// Streaming
await foreach (var update in client.GetStreamingResponseAsync("Tell me a story"))
{
    Console.Write(update.Text);
}

// Structured JSON output
var options = new ChatOptions
{
    ResponseFormat = ChatResponseFormat.ForJsonSchema<WeatherResponse>(jsonOptions)
};
var response = await client.GetResponseAsync(messages, options);
var weather = JsonSerializer.Deserialize<WeatherResponse>(response.Text);
```

#### Native architecture

The implementation uses a Swift bridge layer that communicates with the Foundation Models API:

```
AppleIntelligenceChatClient (C#)
        ↓
    P/Invoke
        ↓
EssentialsAI.xcodeproj (Swift)
    - ChatClientNative
    - AIToolNative protocol
    - ResponseUpdateNative
    - CancellationTokenNative
        ↓
Foundation Models (Apple)
```

For more information about the Foundation Models framework, see [Apple's Foundation Models documentation](https://developer.apple.com/documentation/foundationmodels).

### NLEmbeddingGenerator

The `NLEmbeddingGenerator` class implements `IEmbeddingGenerator<string, Embedding<float>>` using Apple's **Natural Language** framework for generating text embeddings.

**Namespace**: `Microsoft.Maui.Essentials.AI`

**Native API**: [NLEmbedding](https://developer.apple.com/documentation/naturallanguage/nlembedding)

```csharp
public class NLEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
```

#### Capabilities

- Generate sentence-level embeddings for semantic similarity
- Support for multiple languages via `NLLanguage`
- Thread-safe implementation (single-threaded access to underlying `NLEmbedding`)
- Extension method: `embedding.AsIEmbeddingGenerator()`

#### Supported languages

The `NLEmbedding` API supports embeddings for many languages. Common examples include:

| Language | Code |
|----------|------|
| English | `en` |
| Spanish | `es` |
| French | `fr` |
| German | `de` |
| Chinese (Simplified) | `zh-Hans` |
| Japanese | `ja` |

For a complete list, see [NLLanguage](https://developer.apple.com/documentation/naturallanguage/nllanguage).

#### Example usage

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;
using System.Numerics.Tensors;

var generator = new NLEmbeddingGenerator();

// Generate an embedding
var embedding = await generator.GenerateAsync("Hello, world!");
var vector = embedding.Vector; // ReadOnlyMemory<float>

// Semantic similarity search
var queryEmbedding = await generator.GenerateAsync("Find restaurants nearby");
var results = items
    .Select(item => (item, TensorPrimitives.CosineSimilarity(
        queryEmbedding.Vector.Span, 
        item.StoredEmbedding.Vector.Span)))
    .OrderByDescending(x => x.Item2)
    .Take(5);
```

> [!NOTE]
> The `NLEmbeddingGenerator` uses Apple's NaturalLanguage framework, which has been available since iOS 13.0. This is **independent** from Apple Intelligence (iOS 26+) and works on older devices.

For more information, see [Apple's Natural Language documentation](https://developer.apple.com/documentation/naturallanguage).

---

## Android

Android provides AI capabilities through Google's ML Kit GenAI APIs, specifically Gemini Nano for on-device generative AI.

### GeminiNanoChatClient

The `GeminiNanoChatClient` class implements `IChatClient` using Google's **ML Kit GenAI** framework, which provides access to Gemini Nano for on-device text generation.

**Namespace**: `Microsoft.Maui.Essentials.AI`

**Native API**: [ML Kit GenAI](https://developers.google.com/ml-kit/genai) via `Google.MLKit.GenAI.Prompt.IGenerativeModel`

```csharp
public sealed class GeminiNanoChatClient : IChatClient
```

#### Capabilities

| Feature | Supported | Notes |
|---------|-----------|-------|
| Text generation | ✅ | Via `GetResponseAsync` |
| Streaming | ✅ | Via `GetStreamingResponseAsync` |
| System prompts | ✅ | Via `PromptPrefix` |
| Image input | ✅ | Single image via `DataContent` |
| Tool/function calling | ❌ | Not supported by ML Kit |
| JSON structured output | ❌ | Not supported by ML Kit |

#### Supported options

The following `ChatOptions` properties are supported:

| Option | Type | Description |
|--------|------|-------------|
| `Temperature` | `float?` | Controls randomness (0.0 = deterministic, higher = more random) |
| `TopK` | `int?` | Limits token selection to top K candidates |
| `Seed` | `long?` | Random seed for reproducible outputs |
| `Instructions` | `string?` | System instructions via `PromptPrefix` |

#### Example usage

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

// Basic chat
var client = new GeminiNanoChatClient();
var response = await client.GetResponseAsync("What is the capital of France?");
Console.WriteLine(response.Text);

// Streaming
await foreach (var update in client.GetStreamingResponseAsync("Tell me a story"))
{
    Console.Write(update.Text);
}

// With system prompt
var messages = new List<ChatMessage>
{
    new ChatMessage(ChatRole.System, "You are a helpful travel assistant."),
    new ChatMessage(ChatRole.User, "Plan a weekend trip to Tokyo")
};
var response = await client.GetResponseAsync(messages);

// With image input
var imageBytes = await File.ReadAllBytesAsync("photo.jpg");
var messages = new List<ChatMessage>
{
    new ChatMessage(ChatRole.User, [
        new TextContent("What's in this image?"),
        new DataContent(imageBytes, "image/jpeg")
    ])
};
var response = await client.GetResponseAsync(messages);
```

#### Extension method

You can also create a chat client from an existing `IGenerativeModel`:

```csharp
using Google.MLKit.GenAI.Prompt;

IGenerativeModel model = Generation.Instance.Client;
IChatClient chatClient = model.AsIChatClient();
```

> [!NOTE]
> ML Kit GenAI supports only one image per request. If multiple `DataContent` items are provided, an `InvalidOperationException` will be thrown.

#### Related resources

- [ML Kit overview](https://developers.google.com/ml-kit)
- [ML Kit GenAI APIs](https://developers.google.com/ml-kit/genai)
- [Gemini Nano on-device](https://developer.android.com/ai/gemini-nano)
- [Android AI documentation](https://developer.android.com/ai)

---

## Windows

Windows provides AI capabilities through the Windows Copilot Runtime, specifically Phi Silica for on-device text generation and embeddings.

### PhiSilicaChatClient

The `PhiSilicaChatClient` class implements `IChatClient` using the **Windows Copilot Runtime** `LanguageModel` API (Phi Silica) for on-device text generation.

**Namespace**: `Microsoft.Maui.Essentials.AI`

**Native API**: [Windows.AI.Text.LanguageModel](/windows/ai/apis/phi-silica)

**Minimum OS**: Windows 10.0.26100.0 (Windows 11 24H2)

```csharp
[SupportedOSPlatform("windows10.0.26100.0")]
public sealed class PhiSilicaChatClient : IChatClient
```

#### Capabilities

| Feature | Supported | Notes |
|---------|-----------|-------|
| Text generation | ✅ | Via `GetResponseAsync` |
| Streaming | ✅ | Via `GetStreamingResponseAsync` |
| System prompts | ✅ | Via `LanguageModel.CreateContext()` |
| Tool/function calling | ❌ | Not supported |
| JSON structured output | ❌ | Not supported |
| Image input | ❌ | Text only |

#### Supported options

The following `ChatOptions` properties are supported:

| Option | Type | Description |
|--------|------|-------------|
| `Temperature` | `double?` | Controls randomness |
| `TopK` | `int?` | Limits token selection to top K candidates |
| `TopP` | `double?` | Nucleus sampling probability |
| `Instructions` | `string?` | System prompt for context |

#### Example usage

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

// Basic chat
var client = new PhiSilicaChatClient();
var response = await client.GetResponseAsync("What is the capital of France?");
Console.WriteLine(response.Text);

// Streaming
await foreach (var update in client.GetStreamingResponseAsync("Tell me a story"))
{
    Console.Write(update.Text);
}

// With system prompt
var messages = new List<ChatMessage>
{
    new ChatMessage(ChatRole.System, "You are a helpful coding assistant."),
    new ChatMessage(ChatRole.User, "Write a C# function to calculate factorial")
};
var response = await client.GetResponseAsync(messages);
```

#### Extension method

You can create a chat client from an existing `LanguageModel`:

```csharp
using Microsoft.Windows.AI.Text;

LanguageModel model = await LanguageModel.CreateAsync();
IChatClient chatClient = model.AsIChatClient();
```

For more information about Phi Silica, see [Windows Copilot Runtime - Phi Silica](/windows/ai/apis/phi-silica).

### PhiSilicaEmbeddingGenerator

The `PhiSilicaEmbeddingGenerator` class implements `IEmbeddingGenerator<string, Embedding<float>>` using the Windows Copilot Runtime for generating text embeddings.

**Namespace**: `Microsoft.Maui.Essentials.AI`

**Native API**: [Windows.AI.Text.LanguageModel](/windows/ai/apis/phi-silica)

**Minimum OS**: Windows 10.0.26100.0 (Windows 11 24H2)

```csharp
[SupportedOSPlatform("windows10.0.26100.0")]
public sealed class PhiSilicaEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
```

#### Capabilities

- Generate text embeddings for semantic similarity
- Thread-safe (LanguageModel is marked as agile)
- Batch embedding generation

#### Example usage

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;
using System.Numerics.Tensors;

var generator = new PhiSilicaEmbeddingGenerator();

// Generate an embedding
var embeddings = await generator.GenerateAsync(["Hello, world!"]);
var vector = embeddings[0].Vector; // ReadOnlyMemory<float>

// Batch generation
var texts = new[] { "First document", "Second document", "Third document" };
var allEmbeddings = await generator.GenerateAsync(texts);

// Semantic similarity search
var queryEmbeddings = await generator.GenerateAsync(["Find similar documents"]);
var results = storedDocuments
    .Select(doc => (doc, TensorPrimitives.CosineSimilarity(
        queryEmbeddings[0].Vector.Span,
        doc.Embedding.Vector.Span)))
    .OrderByDescending(x => x.Item2)
    .Take(5);
```

#### Extension method

You can create an embedding generator from an existing `LanguageModel`:

```csharp
using Microsoft.Windows.AI.Text;

LanguageModel model = await LanguageModel.CreateAsync();
IEmbeddingGenerator<string, Embedding<float>> generator = model.AsIEmbeddingGenerator();
```

#### Related resources

- [Windows AI overview](/windows/ai/)
- [Windows Copilot Runtime APIs](/windows/ai/apis/)
- [Phi Silica documentation](/windows/ai/apis/phi-silica)
- [Windows ML](/windows/ai/windows-ml/)

---

## Cross-platform pattern

With native AI available on all platforms, you can use the same pattern everywhere:

```csharp
public static MauiAppBuilder ConfigureAI(this MauiAppBuilder builder)
{
#if IOS || MACCATALYST
    // Use on-device Apple Intelligence
    builder.Services.AddSingleton<IChatClient>(sp =>
        new AppleIntelligenceChatClient()
            .AsBuilder()
            .UseLogging(sp.GetRequiredService<ILoggerFactory>())
            .Build());

    builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
        new NLEmbeddingGenerator()
            .AsBuilder()
            .UseLogging(sp.GetRequiredService<ILoggerFactory>())
            .Build());
#elif ANDROID
    // Use on-device Gemini Nano
    builder.Services.AddSingleton<IChatClient>(sp =>
        new GeminiNanoChatClient()
            .AsBuilder()
            .UseLogging(sp.GetRequiredService<ILoggerFactory>())
            .Build());
    // Note: Embeddings not available on Android natively
#elif WINDOWS
    // Use on-device Phi Silica
    builder.Services.AddSingleton<IChatClient>(sp =>
        new PhiSilicaChatClient()
            .AsBuilder()
            .UseLogging(sp.GetRequiredService<ILoggerFactory>())
            .Build());

    builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
        new PhiSilicaEmbeddingGenerator()
            .AsBuilder()
            .UseLogging(sp.GetRequiredService<ILoggerFactory>())
            .Build());
#endif
    return builder;
}
```

This pattern ensures your app uses on-device AI processing on all supported platforms.

## See also

- [Feature comparison](feature-comparison.md)
- [Requirements](requirements.md)
- [Getting started](getting-started.md)
