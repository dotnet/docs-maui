---
title: Chat client
description: Use IChatClient from Microsoft.Maui.Essentials.AI for basic chat, streaming, multi-turn conversations, tool calling, and structured JSON output in .NET MAUI.
ms.date: 03/11/2026
ms.topic: conceptual
---

# Chat client

This page shows how to use `IChatClient` in a .NET MAUI app once services are registered. For setup and registration, see [Get started](getting-started.md). For platform requirements, see [Apple requirements](requirements-apple.md).

The `IChatClient` interface is part of `Microsoft.Extensions.AI`. All examples on this page use the interface directly and work regardless of the underlying platform implementation.

## Basic chat

Inject `IChatClient` and call `GetResponseAsync` for a single-turn response:

```csharp
using Microsoft.Extensions.AI;

public class ChatService(IChatClient chatClient)
{
    public async Task<string> AskAsync(string question)
    {
        var response = await chatClient.GetResponseAsync(question);
        return response.Text;
    }
}
```

## Streaming responses

Use `GetStreamingResponseAsync` to receive tokens incrementally as they are generated. This reduces perceived latency in chat UIs:

```csharp
using Microsoft.Extensions.AI;

public class ChatService(IChatClient chatClient)
{
    public async Task StreamAsync(string prompt, Action<string> onToken)
    {
        await foreach (var update in chatClient.GetStreamingResponseAsync(prompt))
        {
            if (update.Text is not null)
                onToken(update.Text);
        }
    }
}
```

## Multi-turn conversations

Pass the full conversation history as a `List<ChatMessage>` to maintain context across turns:

```csharp
using Microsoft.Extensions.AI;

var messages = new List<ChatMessage>
{
    new ChatMessage(ChatRole.System, "You are a helpful travel assistant."),
    new ChatMessage(ChatRole.User, "What's special about Tokyo?"),
};

var response = await chatClient.GetResponseAsync(messages);
messages.Add(response.Message); // append assistant reply to history

// Continue the conversation
messages.Add(new ChatMessage(ChatRole.User, "What's the best time to visit?"));
var followUp = await chatClient.GetResponseAsync(messages);
Console.WriteLine(followUp.Text);
```

The system message sets the assistant's persona and persists for the entire conversation.

## Tool calling

Define functions with `[Description]` attributes, wrap them with `AIFunctionFactory.Create`, and pass them via `ChatOptions.Tools`. The model decides when to call each tool:

```csharp
using System.ComponentModel;
using Microsoft.Extensions.AI;

[Description("Gets the current weather for a city.")]
static async Task<string> GetWeatherAsync(
    [Description("The city name")] string city)
{
    // call your weather service here
    return $"Sunny, 22°C in {city}";
}

var options = new ChatOptions
{
    Tools = [AIFunctionFactory.Create(GetWeatherAsync)]
};

var response = await chatClient.GetResponseAsync("What's the weather in Paris?", options);
Console.WriteLine(response.Text);
```

> [!IMPORTANT]
> Only `AIFunction` tools are supported. Other `AITool` subtypes are not supported.

## Structured JSON output

Use the generic `GetResponseAsync<T>()` method to receive a strongly-typed response. The method automatically applies the required JSON schema and deserializes the result:

```csharp
using Microsoft.Extensions.AI;

public record Itinerary(string Destination, string[] Days, string[] Tips);

var result = await chatClient.GetResponseAsync<Itinerary>(
    "Create a 3-day Tokyo itinerary.");

Console.WriteLine($"Destination: {result.Result.Destination}");
foreach (var day in result.Result.Days)
    Console.WriteLine(day);
```

> [!IMPORTANT]
> Apple Intelligence requires a JSON schema to produce structured output. The `GetResponseAsync<T>()` method provides this automatically. If you construct `ChatOptions` manually, use `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)`. Plain `ChatResponseFormat.Json` without a schema is **not** supported.

## See also

- [Text embeddings](embeddings.md)
- [Agent framework integration](agent-framework.md)
- [Feature comparison](feature-comparison.md)
