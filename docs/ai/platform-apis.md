---
title: Platform-specific AI APIs
description: Microsoft.Maui.Essentials.AI wraps native Apple AI capabilities behind standard Microsoft.Extensions.AI interfaces. This article describes the available APIs.
ms.date: 05/14/2026
---

# Platform-specific AI APIs

`Microsoft.Maui.Essentials.AI` wraps native Apple AI capabilities behind standard `Microsoft.Extensions.AI` interfaces. This article describes the available APIs and their platform-specific behavior.

> [!IMPORTANT]
> `Microsoft.Maui.Essentials.AI` is experimental and is identified by the diagnostic ID `MAUIAI0001`. To use it, suppress the diagnostic or opt in explicitly.

## Apple platforms (iOS, macOS, Mac Catalyst, tvOS)

Apple provides two distinct AI capabilities through different native frameworks:

- **Apple Intelligence** (Foundation Models framework) — on-device language model, requires iOS 26.0+, macOS 26.0+, Mac Catalyst 26.0+, or tvOS 26.0+.
- **Natural Language embeddings** (Natural Language framework) — text embedding, available from iOS 13.0+, macOS 10.15+, Mac Catalyst 13.1+, and tvOS 13.0+.

---

### AppleIntelligenceChatClient

`AppleIntelligenceChatClient` provides access to the on-device Apple Intelligence language model via the Apple Foundation Models framework. It implements the standard `IChatClient` interface from `Microsoft.Extensions.AI`, so it works with any middleware or tooling that targets that interface.

**Namespace:** `Microsoft.Maui.Essentials.AI`  
**Implements:** `IChatClient`  
**Native framework:** Foundation Models (Apple)  
**Minimum platform version:** iOS 26.0, macOS 26.0, Mac Catalyst 26.0, tvOS 26.0

```csharp
public class AppleIntelligenceChatClient : IChatClient
```

#### Constructors

| Constructor | Description |
|-------------|-------------|
| `AppleIntelligenceChatClient()` | Creates an instance with no logging and no function-invocation services. |
| `AppleIntelligenceChatClient(ILoggerFactory? loggerFactory, IServiceProvider? functionInvocationServices)` | Creates an instance with optional logging and optional service provider used when invoking `AIFunction` tools. |

#### Capabilities

| Feature | Supported | Notes |
|---------|-----------|-------|
| Text generation | ✅ | Via `GetResponseAsync` |
| Streaming | ✅ | Via `GetStreamingResponseAsync` |
| Tool/function calling | ✅ | `AIFunction` tools only |
| Structured JSON output | ✅ | Requires JSON schema via `ChatResponseFormat.ForJsonSchema<T>()` |
| System prompts | ✅ | Via `ChatMessage` with `ChatRole.System` |
| Multi-turn conversations | ✅ | Pass full conversation history |
| Image input | ❌ | Text and function calls only |

#### Supported ChatOptions

The following `ChatOptions` properties are passed through to the underlying Foundation Models session. All other properties are silently ignored.

| Option | Type | Description |
|--------|------|-------------|
| `TopK` | `int?` | Limits the number of token candidates considered at each sampling step. |
| `Seed` | `long?` | Random seed for reproducible outputs. |
| `Temperature` | `float?` | Controls randomness. Lower values (toward 0) produce more deterministic output. |
| `MaxOutputTokens` | `int?` | Maximum number of tokens the model may generate. |
| `ResponseFormat` | `ChatResponseFormat` | Use `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)` to request structured JSON output. |
| `Tools` | `IList<AITool>` | `AIFunction` tools the model may call. Other `AITool` subtypes must be adapted before use. |

> [!IMPORTANT]
> Two constraints apply when using `AppleIntelligenceChatClient`:
>
> 1. **Tool types:** Only `AIFunction` tools are supported. If your tool list contains other `AITool` subtypes, you must adapt them before passing them to this client.
> 2. **Structured JSON output:** You must use `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)` to generate a JSON schema. Using `ChatResponseFormat.Json` (plain JSON mode without a schema) is **not** supported and will throw.

#### Supported message content types

| Content type | Supported |
|--------------|-----------|
| `TextContent` | ✅ |
| `FunctionCallContent` | ✅ |
| `FunctionResultContent` | ✅ |
| `ImageContent` | ❌ |

#### Examples

**Simple chat:**

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

IChatClient client = new AppleIntelligenceChatClient();

var response = await client.GetResponseAsync("Summarize the history of the Eiffel Tower.");
Console.WriteLine(response.Text);
```

**Streaming:**

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

IChatClient client = new AppleIntelligenceChatClient();

await foreach (var update in client.GetStreamingResponseAsync("Tell me a short story."))
{
    Console.Write(update.Text);
}
```

**Structured JSON output:**

```csharp
using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

record WeatherSummary(string Condition, int TemperatureCelsius);

IChatClient client = new AppleIntelligenceChatClient();

var options = new ChatOptions
{
    ResponseFormat = ChatResponseFormat.ForJsonSchema<WeatherSummary>(JsonSerializerOptions.Default)
};

var response = await client.GetResponseAsync(
    "Describe today's weather as JSON.",
    options);

var weather = JsonSerializer.Deserialize<WeatherSummary>(response.Text!);
```

**Tool/function calling:**

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

[Description("Gets the current time in the specified time zone.")]
static string GetCurrentTime(string timeZoneId)
{
    var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz).ToString("T");
}

IChatClient client = new AppleIntelligenceChatClient()
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();

var options = new ChatOptions
{
    Tools = [AIFunctionFactory.Create(GetCurrentTime)]
};

var response = await client.GetResponseAsync(
    "What time is it in Tokyo?",
    options);

Console.WriteLine(response.Text);
```

#### Native architecture

`AppleIntelligenceChatClient` bridges managed C# code to the Apple Foundation Models framework through a Swift interop layer:

```
AppleIntelligenceChatClient (C#)
      ↓ P/Invoke
EssentialsAI (Swift — EssentialsAI.xcodeproj)
  - ChatClientNative
  - AIToolNative protocol
      ↓
Foundation Models (Apple)
```

The Swift layer (`EssentialsAI.xcodeproj`) is compiled into the app and called from C# via P/Invoke. This means no network traffic is involved — all inference runs on-device.

---

### NLEmbeddingGenerator

`NLEmbeddingGenerator` generates text embeddings using the Apple Natural Language framework (`NLEmbedding`). Unlike `AppleIntelligenceChatClient`, it does **not** require Apple Intelligence and is available on older OS versions.

**Namespace:** `Microsoft.Maui.Essentials.AI`  
**Implements:** `IEmbeddingGenerator<string, Embedding<float>>`  
**Native framework:** Natural Language (Apple)  
**Minimum platform version:** iOS 13.0, macOS 10.15, Mac Catalyst 13.1, tvOS 13.0

```csharp
public class NLEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
```

#### Constructors

| Constructor | Description |
|-------------|-------------|
| `NLEmbeddingGenerator()` | Creates an instance using the English-language embedding model. |
| `NLEmbeddingGenerator(NLLanguage language)` | Creates an instance for the specified language. |
| `NLEmbeddingGenerator(NLEmbedding embedding)` | Wraps an existing `NLEmbedding` instance. Use this when you need fine-grained control over the underlying embedding model. |

#### Thread safety

`NLEmbeddingGenerator` uses a `SemaphoreSlim(1, 1)` internally to serialize concurrent `GenerateAsync` calls. This is safe for concurrent use, but only one embedding request runs at a time per instance.

> [!TIP]
> For high-throughput scenarios where you need to embed many strings concurrently, create multiple `NLEmbeddingGenerator` instances and distribute work across them.

#### Examples

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;
using System.Numerics.Tensors;

// Default English embeddings
var generator = new NLEmbeddingGenerator();

// Specific language
var frenchGenerator = new NLEmbeddingGenerator(NLLanguage.French);

// Generate embeddings for multiple strings in one call
var embeddings = await generator.GenerateAsync(["Hello world", "Goodbye world"]);

// Compute cosine similarity between two embeddings
float similarity = TensorPrimitives.CosineSimilarity(
    embeddings[0].Vector.Span,
    embeddings[1].Vector.Span);

Console.WriteLine($"Similarity: {similarity:F4}");
```

---

### NLEmbeddingExtensions

`NLEmbeddingExtensions` provides a convenience extension method to wrap a native `NLEmbedding` instance in the standard `IEmbeddingGenerator<string, Embedding<float>>` interface.

**Namespace:** `Microsoft.Maui.Essentials.AI`

| Method | Description |
|--------|-------------|
| `AsIEmbeddingGenerator(this NLEmbedding embedding)` | Wraps a native `NLEmbedding` as an `IEmbeddingGenerator<string, Embedding<float>>`. |

#### Example

```csharp
using NaturalLanguage;
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

NLEmbedding nativeEmbedding = NLEmbedding.GetEmbedding(NLLanguageKey.English, null)!;
IEmbeddingGenerator<string, Embedding<float>> generator = nativeEmbedding.AsIEmbeddingGenerator();

var embeddings = await generator.GenerateAsync(["sample text"]);
```

---

## See also

- [Feature comparison](feature-comparison.md)
- [Getting started](getting-started.md)
- [Requirements](requirements.md)
- [`IChatClient` (Microsoft.Extensions.AI)](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.ichatclient)
- [`IEmbeddingGenerator<TInput,TEmbedding>` (Microsoft.Extensions.AI)](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.iembeddinggenerator-2)
