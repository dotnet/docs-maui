---
title: Get started with the chat client
description: Learn how to install, register, and use the AppleIntelligenceChatClient to build on-device chat experiences in your .NET MAUI app using Apple Intelligence.
ms.date: 03/11/2026
---

# Get started with the chat client

`AppleIntelligenceChatClient` provides on-device conversational AI using Apple Intelligence. It implements the standard `IChatClient` interface from `Microsoft.Extensions.AI`, so it works with any middleware, logging, or tooling built for that interface.

> [!NOTE]
> Apple Intelligence requires iOS 26.0+, macOS 26.0+, Mac Catalyst 26.0+, or tvOS 26.0+, and supported Apple Silicon hardware. See [Requirements](requirements.md) for details. Android and Windows support are planned for future releases.

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

Create an extension method on `MauiAppBuilder` to register the chat client:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder AddChatServices(this MauiAppBuilder builder)
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
        builder.AddChatServices();
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

## Basic chat

Inject `IChatClient` and call `GetResponseAsync` for a single-turn response:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
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
```

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Streaming responses

Use `GetStreamingResponseAsync` to receive tokens as they are produced, which reduces perceived latency in chat UIs:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
var client = serviceProvider.GetRequiredService<IChatClient>();

await foreach (var update in client.GetStreamingResponseAsync("Tell me about the Eiffel Tower"))
{
    if (update.Text is not null)
        Console.Write(update.Text);
}
```

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Multi-turn conversations

Pass a `List<ChatMessage>` to maintain conversation history across turns:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
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
```

The system message sets the assistant's persona and persists for the entire conversation.

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Tool calling

`AppleIntelligenceChatClient` supports function calling via `AIFunction` tools. Define methods with `[Description]` attributes and register them with `AIFunctionFactory.Create()`:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
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

// Create AIFunction instances and pass via ChatOptions
var options = new ChatOptions
{
    Tools = [AIFunctionFactory.Create(SearchLandmarksAsync)]
};

await foreach (var update in client.GetStreamingResponseAsync(messages, options, cancellationToken))
{
    if (update.Text is not null)
        Console.Write(update.Text);
}
```

> [!IMPORTANT]
> Only `AIFunction` tools are supported by `AppleIntelligenceChatClient`. Other `AITool` subtypes are not supported.

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Structured JSON output

Use the generic `GetResponseAsync<T>()` method to receive a strongly-typed response. The method automatically applies the appropriate JSON schema and deserializes the result:

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
using Microsoft.Extensions.AI;

public record Itinerary(string Destination, string[] Days, string[] Tips);

var itinerary = await client.GetResponseAsync<Itinerary>(
    "Create a 3-day Tokyo itinerary.");

Console.WriteLine($"Destination: {itinerary.Result.Destination}");
```

> [!IMPORTANT]
> Apple Intelligence requires a JSON schema to produce structured output. The `GetResponseAsync<T>()` method provides this automatically. If you construct `ChatOptions` manually, use `ChatResponseFormat.ForJsonSchema<T>(jsonSerializerOptions)` — plain `ChatResponseFormat.Json` without a schema is **not** supported.

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Next steps

- [Text embeddings](getting-started-embeddings.md) — generate and compare text embeddings on-device
- [Agent framework integration](agent-framework.md) — compose multiple agents into multi-stage workflows
- [Feature comparison](feature-comparison.md) — supported features and `ChatOptions` across platforms
- [Requirements](requirements.md) — supported OS versions and device requirements
