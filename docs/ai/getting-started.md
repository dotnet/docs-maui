---
title: Get started with Microsoft.Maui.Essentials.AI
description: Learn how to install and use Microsoft.Maui.Essentials.AI to integrate on-device AI capabilities such as chat and text embeddings into your .NET MAUI app.
ms.date: 05/14/2026
---

# Get started with Microsoft.Maui.Essentials.AI

`Microsoft.Maui.Essentials.AI` provides on-device AI capabilities for .NET MAUI apps, including chat completions via Apple Intelligence and text embeddings via the Natural Language framework.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [.NET MAUI workload](https://learn.microsoft.com/dotnet/maui/get-started/installation)
- For Apple platforms: Xcode 26 or later
- A supported Apple device or simulator (see [Requirements](requirements.md))

## Installation

Add the NuGet package to your .NET MAUI project:

### .NET CLI

```dotnetcli
dotnet add package Microsoft.Maui.Essentials.AI
```

### PackageReference

```xml
<PackageReference Include="Microsoft.Maui.Essentials.AI" Version="10.0.*-*" />
```

### Suppress the experimental warning

`Microsoft.Maui.Essentials.AI` is experimental (diagnostic ID `MAUIAI0001`). Suppress the warning project-wide in your `.csproj`:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);MAUIAI0001</NoWarn>
</PropertyGroup>
```

Alternatively, suppress it inline around specific code:

```csharp
#pragma warning disable MAUIAI0001
// ... your experimental AI code
#pragma warning restore MAUIAI0001
```

## Register AI services

The recommended approach is to register AI services in a dedicated extension method and call it from `MauiProgram.cs`.

### Extension method (recommended)

Create an extension method on `MauiAppBuilder` to register all required services:

```csharp
#if IOS || MACCATALYST
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder AddAppleIntelligenceServices(this MauiAppBuilder builder)
    {
        // Register the base Apple Intelligence client
        builder.Services.AddSingleton<AppleIntelligenceChatClient>();

        // Register IChatClient with logging middleware
        builder.Services.AddChatClient(sp =>
        {
            var appleClient = sp.GetRequiredService<AppleIntelligenceChatClient>();
            return appleClient.AsBuilder()
                .UseLogging()
                .Build(sp);
        });

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
#endif
```

Key registration details:

- `AppleIntelligenceChatClient` is registered as a singleton and wraps Apple Intelligence on-device inference.
- `IChatClient` is registered using the `.AsBuilder().UseLogging().Build(sp)` middleware pattern, which adds structured logging to every request.
- `NLEmbeddingGenerator` is registered as a singleton and wraps the Natural Language framework. It does **not** require Apple Intelligence and is available on iOS 13.0+.
- `IEmbeddingGenerator<string, Embedding<float>>` is registered with logging middleware for observability.

### Call from MauiProgram.cs

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

#if IOS || MACCATALYST
        builder.AddAppleIntelligenceServices();
#else
        throw new PlatformNotSupportedException(
            "Apple Intelligence is only supported on iOS and macCatalyst.");
#endif

        return builder.Build();
    }
}
```

> [!NOTE]
> Apple Intelligence chat (`AppleIntelligenceChatClient`) requires iOS 26.0+, macOS 26.0+, or macCatalyst 26.0+. Text embeddings (`NLEmbeddingGenerator`) are available from iOS 13.0+ and macOS 10.15+.

## Basic chat

Inject `IChatClient` and call `GetResponseAsync` for a single-turn response:

```csharp
#if IOS || MACCATALYST
using Microsoft.Extensions.AI;

public class MyService
{
    private readonly IChatClient _chatClient;

    public MyService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> AskAsync(string question)
    {
        var response = await _chatClient.GetResponseAsync(question);
        return response.Text;
    }
}
#endif
```

## Streaming responses

Use `GetStreamingResponseAsync` to receive tokens as they are produced:

```csharp
#if IOS || MACCATALYST
var client = serviceProvider.GetRequiredService<IChatClient>();

await foreach (var update in client.GetStreamingResponseAsync("Tell me about the Eiffel Tower"))
{
    if (update.Text is not null)
        Console.Write(update.Text);
}
#endif
```

Streaming is useful for displaying responses incrementally in a chat UI, reducing perceived latency.

## Multi-turn conversations

Pass a `List<ChatMessage>` to maintain conversation history across turns:

```csharp
#if IOS || MACCATALYST
using Microsoft.Extensions.AI;

var messages = new List<ChatMessage>
{
    new ChatMessage(ChatRole.System, "You are a helpful travel assistant."),
    new ChatMessage(ChatRole.User, "What's special about Tokyo?"),
};

var response = await client.GetResponseAsync(messages);
messages.Add(response.Message); // Append assistant reply to history

// Continue the conversation
messages.Add(new ChatMessage(ChatRole.User, "What's the best time to visit?"));
var followUp = await client.GetResponseAsync(messages);
Console.WriteLine(followUp.Text);
#endif
```

The system message sets the assistant's persona and persists for the entire conversation.

## Tool calling

`AppleIntelligenceChatClient` supports function calling via `AIFunction` tools. Define tool methods with `[Description]` attributes and register them with `AIFunctionFactory.Create()`:

```csharp
#if IOS || MACCATALYST
using System.ComponentModel;
using Microsoft.Extensions.AI;

// Define tool methods with descriptions
[Description("Search for travel destinations by a natural language query.")]
async Task<string> SearchLandmarksAsync(
    [Description("A natural language search query")] string query,
    [Description("Maximum number of results to return")] int maxResults = 5)
{
    var results = await dataService.SearchLandmarksAsync(query, maxResults);
    return string.Join("\n", results.Select(r => $"{r.Name}: {r.Description}"));
}

[Description("Get the current weather for a location.")]
async Task<string> GetWeatherAsync(
    [Description("The city or location name")] string location)
{
    var weather = await weatherService.GetWeatherAsync(location);
    return $"{weather.Temperature}°C, {weather.Condition}";
}

// Create AIFunction instances from the methods
var tools = new List<AITool>
{
    AIFunctionFactory.Create(SearchLandmarksAsync),
    AIFunctionFactory.Create(GetWeatherAsync),
};

// Pass tools via ChatOptions
var options = new ChatOptions { Tools = tools };
await foreach (var update in client.GetStreamingResponseAsync(messages, options, cancellationToken))
{
    if (update.Text is not null)
        Console.Write(update.Text);
}
#endif
```

> [!IMPORTANT]
> Only `AIFunction` tools are supported by `AppleIntelligenceChatClient`. Other `AITool` subtypes are not supported and will be ignored or cause an error.

## Structured JSON output

To receive a strongly-typed JSON response, use `ChatResponseFormat.ForJsonSchema<T>()` with a `JsonSerializerOptions` instance:

```csharp
#if IOS || MACCATALYST
using System.Text.Json;
using Microsoft.Extensions.AI;

public record Itinerary(string Destination, string[] Days, string[] Tips);

var serializerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

var chatOptions = new ChatOptions
{
    ResponseFormat = ChatResponseFormat.ForJsonSchema<Itinerary>(serializerOptions)
};

var response = await client.GetResponseAsync(
    "Create a 3-day Tokyo itinerary in JSON format.", chatOptions);

var itinerary = JsonSerializer.Deserialize<Itinerary>(response.Text!, serializerOptions);
Console.WriteLine($"Destination: {itinerary!.Destination}");
#endif
```

> [!IMPORTANT]
> Apple Intelligence requires a JSON schema to produce structured output. Using `ChatResponseFormat.Json` without a schema is **not** supported. Always use `ChatResponseFormat.ForJsonSchema<T>(serializerOptions)`.

## Text embeddings

`NLEmbeddingGenerator` converts strings into float vectors using Apple's Natural Language **sentence** embedding model (`NLEmbedding.GetSentenceEmbedding`). These vectors capture the semantic meaning of full sentences or short passages and can be used for semantic search, clustering, and retrieval-augmented generation (RAG).

```csharp
#if IOS || MACCATALYST
using System.Numerics.Tensors;
using Microsoft.Extensions.AI;

var generator = serviceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

// Generate an embedding for a single string
var embeddings = await generator.GenerateAsync(["Hello, world!"]);
ReadOnlyMemory<float> vector = embeddings[0].Vector;

// Semantic similarity search over a collection of pre-embedded documents
var queryEmbedding = await generator.GenerateAsync(["Find restaurants nearby"]);

var results = documents
    .Select(doc => (doc, similarity: TensorPrimitives.CosineSimilarity(
        queryEmbedding[0].Vector.Span,
        doc.Embedding.Vector.Span)))
    .OrderByDescending(x => x.similarity)
    .Take(5)
    .ToList();

foreach (var (doc, score) in results)
    Console.WriteLine($"{doc.Title} ({score:P1})");
#endif
```

> [!NOTE]
> `NLEmbeddingGenerator` is available on iOS 13.0+ and macOS 10.15+. It does **not** require Apple Intelligence or a minimum OS version of 26.

## Next steps

- [Requirements](requirements.md) — supported OS versions and device requirements
- [Platform APIs](platform-apis.md) — detailed API reference for `AppleIntelligenceChatClient` and `NLEmbeddingGenerator`
- [Agent framework integration](agent-framework.md) — building multi-step agents with tool calling
- [Feature comparison](feature-comparison.md) — comparing capabilities across platforms
