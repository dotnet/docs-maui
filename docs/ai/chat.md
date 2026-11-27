---
title: Chat client
description: Use IChatClient from Microsoft.Maui.Essentials.AI for on-device chat, streaming, multi-turn conversations, tool calling, and structured output in .NET MAUI.
ms.date: 11/27/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Chat client

This article shows how to use [`IChatClient`](/dotnet/ai/ichatclient) in a .NET MAUI app after you [register the chat client](getting-started.md#register-the-chat-client). The same code runs on every platform, but supported options and content depend on the platform's model. See the [chat feature comparison](chat/feature-comparison.md), and the [Apple](chat/apple.md) and [Android](chat/android.md) guidance.

## Basic chat

Call `GetResponseAsync` for a single response:

```csharp
using Microsoft.Extensions.AI;

IChatClient chatClient = // resolved from DI

var response = await chatClient.GetResponseAsync("What is .NET MAUI?");
Console.WriteLine(response.Text);
```

> [!TIP]
> On Android, set `ChatOptions.MaxOutputTokens`. Otherwise, the default Gemini Nano model stops after about 40 tokens. See [Set the output length](chat/android.md#set-the-output-length).

## Streaming responses

Use `GetStreamingResponseAsync` to display text as it's generated:

```csharp
await foreach (var update in chatClient.GetStreamingResponseAsync("Explain dependency injection in simple terms."))
{
    Console.Write(update.Text);
}
```

## Multi-turn conversations

The on-device clients don't store conversation state. Pass the conversation history with each request:

```csharp
List<ChatMessage> messages =
[
    new(ChatRole.System, "You are a helpful travel assistant."),
    new(ChatRole.User, "What's special about Tokyo?"),
];

var response = await chatClient.GetResponseAsync(messages);
Console.WriteLine(response.Text);
messages.AddMessages(response);

messages.Add(new(ChatRole.User, "What's the best time to visit?"));
var followUp = await chatClient.GetResponseAsync(messages);
Console.WriteLine(followUp.Text);
```

On-device models have small context windows. Keep only the history the current task needs. A request that exceeds the context window fails rather than being truncated.

## Tool calling

Define functions with `[Description]` attributes, wrap them with `AIFunctionFactory.Create`, and pass them in `ChatOptions.Tools`. The model decides when to call each tool. For more information, see [Tool calling](/dotnet/ai/ichatclient#tool-calling).

```csharp
using System.ComponentModel;
using Microsoft.Extensions.AI;

var options = new ChatOptions
{
    Tools = [AIFunctionFactory.Create(GetWeather)],
};

var response = await chatClient.GetResponseAsync("What's the weather in Paris?", options);
Console.WriteLine(response.Text);

[Description("Gets the current weather for a city.")]
static string GetWeather([Description("The city name.")] string city) => $"Sunny, 22°C in {city}";
```

Tool support differs by platform:

- **Apple**: Apple Intelligence runs `AIFunction` tools itself.
- **Android**: Gemini Nano returns tool calls, and `UseFunctionInvocation` runs them. Tools require a model that supports them, such as `gemini-nano-preview-fast`. See [Tool calling on Android](chat/android.md#tool-calling).
- **Windows**: Tools aren't supported.

To generate tools from attributed methods and properties at compile time instead of calling `AIFunctionFactory.Create`, see [Source-generated AI tools](tool-attributes.md).

Only `AIFunction` tools are supported. Set `ChatOptions.ToolMode` to `ChatToolMode.None` to turn off tools for a request.

## Structured output

Use `GetResponseAsync<T>` to generate a JSON schema from a type and deserialize the response. For trimmed and Native AOT apps, pass source-generated `JsonSerializerOptions`:

```csharp
using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;

var response = await chatClient.GetResponseAsync<Itinerary>(
    "Create a 3-day Tokyo itinerary.",
    AppJsonContext.Default.Options);

Console.WriteLine(response.Result.Destination);
foreach (var day in response.Result.Days)
{
    Console.WriteLine(day);
}

public sealed record Itinerary(string Destination, string[] Days, string[] Tips);

[JsonSerializable(typeof(Itinerary))]
internal partial class AppJsonContext : JsonSerializerContext;
```

The on-device models constrain generation to the schema, so a JSON schema is required. If you set `ChatOptions.ResponseFormat` yourself, use `ChatResponseFormat.ForJsonSchema<T>()`. Schema-free `ChatResponseFormat.Json` isn't supported. Each platform supports a different subset of JSON Schema and rejects schemas it can't represent.

A structured response can match the schema and still contain incorrect values. Validate important fields before your app acts on them.

## See also

- [Chat feature comparison](chat/feature-comparison.md)
- [Chat on Apple platforms](chat/apple.md)
- [Chat on Android](chat/android.md)
- [Use the IChatClient interface](/dotnet/ai/ichatclient)
