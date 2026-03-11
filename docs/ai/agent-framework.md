---
title: Microsoft Agent Framework integration
description: Learn how to compose multiple AI agents into multi-stage workflows that run entirely on-device using Microsoft.Agents.AI and Microsoft.Maui.Essentials.AI.
ms.date: 03/11/2026
ms.topic: conceptual
---

# Microsoft Agent Framework integration

> [!IMPORTANT]
> Multi-agent workflows with `Microsoft.Maui.Essentials.AI` currently require Apple Intelligence, which is available on iOS, macOS, Mac Catalyst, and tvOS. Android and Windows support are not yet available.

## Overview

The Microsoft Agent Framework ([Microsoft.Agents.AI](https://www.nuget.org/packages/Microsoft.Agents.AI)) enables you to compose multiple AI agents into structured workflows, where each agent handles one specialized task. When combined with `Microsoft.Maui.Essentials.AI`, you can build sophisticated multi-stage AI pipelines that run entirely on-device using Apple Intelligence.

Rather than writing a single large prompt that tries to do everything, the agent framework lets you decompose complex tasks into a graph of focused agents. The output of one agent becomes the input of the next, and conditional routing lets the workflow adapt to intermediate results.

As a concrete example, the [LocalChatClientWithAgents](https://github.com/dotnet/maui-samples) sample implements a 4-stage travel itinerary workflow:

1. **Travel Planner** — Extracts destination, day count, and target language from the user's free-text request.
1. **Researcher** — Uses retrieval-augmented generation (RAG) to find relevant landmark data for the destination.
1. **Itinerary Planner** — Calls tools to discover points of interest and produces a structured JSON itinerary.
1. **Translator** — Conditionally translates the itinerary when the target language is not English.

## Key concepts

| Concept | Description |
|---|---|
| Agent | An AI-powered component with a specific role, set of instructions, and optional tools or context providers. |
| Executor | A processing unit that wraps an agent and transforms typed input to typed output. |
| Workflow | A directed graph of executors connected by edges. The workflow determines execution order and routing. |
| Edge | A connection between two executors. Edges are unconditional or conditional (switch/case). |
| Context Provider | Injects contextual information—such as RAG search results—into an agent's prompt at runtime. |

## Installation

Add the required NuGet packages to your `.csproj` file:

```xml
<PackageReference Include="Microsoft.Maui.Essentials.AI" Version="10.0.*-*" />
<PackageReference Include="Microsoft.Extensions.AI" Version="10.3.0" />
<PackageReference Include="Microsoft.Agents.AI" Version="1.0.0-rc3" />
<PackageReference Include="Microsoft.Agents.AI.Hosting" Version="1.0.0-preview.260304.1" />
<PackageReference Include="Microsoft.Agents.AI.Workflows" Version="1.0.0-rc3" />
<PackageReference Include="Microsoft.Agents.AI.Workflows.Generators" Version="1.0.0-rc3" />
```

Because `Microsoft.Maui.Essentials.AI` is currently experimental, suppress the diagnostic warning to keep your build clean:

```xml
<NoWarn>$(NoWarn);MAUIAI0001</NoWarn>
```

## Register AI services

Register `AppleIntelligenceChatClient` and expose it through the `Microsoft.Extensions.AI` `IChatClient` abstraction. Registering a **keyed service** lets each agent in the workflow resolve its own `IChatClient` instance independently.

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
using Microsoft.Maui.Essentials.AI;
using Microsoft.Extensions.AI;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder ConfigureAIServices(this MauiAppBuilder builder)
    {
        // Register the Apple Intelligence chat client
        builder.Services.AddSingleton<AppleIntelligenceChatClient>();

        // Register as the default IChatClient
        builder.Services.AddChatClient(sp =>
        {
            var appleClient = sp.GetRequiredService<AppleIntelligenceChatClient>();
            return appleClient.AsBuilder()
                .UseLogging()
                .Build(sp);
        });

        // Register as a keyed service so agents can resolve it by name
        builder.Services.AddKeyedChatClient("local-model", sp =>
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

> [!NOTE]
> The `"local-model"` key is a convention used in the sample. You can use any string key that makes sense for your application.

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Define workflow models

Each stage of a workflow passes a strongly typed result to the next stage. Defining explicit record types makes each agent's contract clear and enables the framework to serialize results between agents.

```csharp
#if IOS || MACCATALYST
// Raw text request from the user — the entry point to the workflow
public record TravelRequest(string UserInput);

// Output of the Travel Planner agent
public record TravelPlanResult(
    [property: DisplayName("destinationName")]
    string DestinationName,
    [property: DisplayName("dayCount")]
    int DayCount,
    [property: DisplayName("language")]
    string Language);

// Output of the Researcher agent
public record ResearchResult(
    string? DestinationName,
    string? DestinationDescription,
    int DayCount,
    string Language);

// Output of the Itinerary Planner agent (and final output after optional translation)
public record ItineraryResult(string ItineraryJson, string TargetLanguage);
#endif
```

## Create agents

### Simple agent with instructions only

The simplest form of an agent is registered with `AddAIAgent()`, supplying a name, a system prompt, and the keyed service that provides the `IChatClient`:

```csharp
#if IOS || MACCATALYST
using Microsoft.Agents.AI.Hosting;

// In your workflow registration code:
builder.AddAIAgent(
    name: "travel-planner-agent",
    instructions: """
        Extract ONLY these 3 values from the user's travel request:
        - destinationName: the place the user wants to visit
        - dayCount: how many days the trip should last
        - language: the language in which to generate the itinerary (default to "English" if not specified)
        Respond with a JSON object containing exactly those fields and no other text.
        """,
    chatClientServiceKey: "local-model");
#endif
```

### Agent with a RAG context provider

A **context provider** retrieves relevant data at runtime and injects it into the agent's prompt. Use this pattern when an agent needs grounding information (such as landmark descriptions) that is not baked into the model:

```csharp
#if IOS || MACCATALYST
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;

builder.AddAIAgent("researcher-agent", (sp, name) =>
{
    var chatClient = sp.GetRequiredKeyedService<IChatClient>("local-model");

    // Create a text-search provider that retrieves landmark data
    var searchProvider = CreateLandmarkSearchProvider(sp);

    return chatClient.AsAIAgent(
        new ChatClientAgentOptions
        {
            Name = name,
            ChatOptions = new ChatOptions
            {
                Instructions = """
                    Select the best matching destination from the provided candidates.
                    Return detailed information that will help plan a day-by-day itinerary.
                    """,
            },
            AIContextProviders = [searchProvider],
        },
        loggerFactory: sp.GetService<ILoggerFactory>());
});
#endif
```

### Agent with tool calling

Agents can invoke tools—arbitrary .NET methods decorated with `AIFunctionFactory.Create`—to retrieve live data during generation:

```csharp
#if IOS || MACCATALYST
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;

builder.AddAIAgent("itinerary-planner-agent", (sp, name) =>
{
    var chatClient = sp.GetRequiredKeyedService<IChatClient>("local-model");

    // Wrap a static async method as an AI tool
    var findPoiTool = AIFunctionFactory.Create(
        FindPointsOfInterestAsync,
        name: "findPointsOfInterest");

    return chatClient.AsAIAgent(
        new ChatClientAgentOptions
        {
            Name = name,
            ChatOptions = new ChatOptions
            {
                Instructions = """
                    Use the findPointsOfInterest tool to discover real places near the destination.
                    Build a day-by-day itinerary from the results.
                    """,
                // Request structured JSON output validated against the Itinerary schema
                ResponseFormat = ChatResponseFormat.ForJsonSchema<Itinerary>(),
                Tools = [findPoiTool],
            },
        },
        loggerFactory: sp.GetService<ILoggerFactory>(),
        services: sp);
});
#endif
```

## Build the workflow

Connect executors into a directed graph using `WorkflowBuilder`. Use `AddEdge` for unconditional transitions and `AddSwitch` for conditional branching:

```csharp
#if IOS || MACCATALYST
using Microsoft.Agents.AI.Workflows;

// Executors are resolved from DI after all agents are registered
var workflow = new WorkflowBuilder(travelPlannerExecutor)
    // Travel Planner → Researcher (always)
    .AddEdge(travelPlannerExecutor, researcherExecutor)
    // Researcher → Itinerary Planner (always)
    .AddEdge(researcherExecutor, itineraryPlannerExecutor)
    // Itinerary Planner → Translator (if language ≠ English) or Output (otherwise)
    .AddSwitch(itineraryPlannerExecutor, switch_ => switch_
        .AddCase<ItineraryResult>(
            condition: r => r is not null &&
                            !string.Equals(r.TargetLanguage, "English", StringComparison.OrdinalIgnoreCase),
            next: translatorExecutor)
        .WithDefault(outputExecutor))
    // Translator → Output (always, when translation path is taken)
    .AddEdge(translatorExecutor, outputExecutor)
    .Build();
#endif
```

> [!TIP]
> `AddSwitch` evaluates cases in the order they are added. The first case whose condition returns `true` is taken; `WithDefault` handles all other situations.

## Run the workflow

### Non-streaming

For short workflows where you only need the final result:

```csharp
#if IOS || MACCATALYST
var result = await workflowAgent.RunAsync<ItineraryResult>(
    input: userInput,
    cancellationToken: cancellationToken);

// result.ItineraryJson contains the final JSON (translated if applicable)
var itinerary = JsonSerializer.Deserialize<Itinerary>(result.ItineraryJson, jsonOptions);
#endif
```

### Streaming

For long-running generation, use `RunStreamingAsync` to receive incremental updates as each agent produces output. This lets you update the UI in real time instead of waiting for the entire workflow to finish:

```csharp
#if IOS || MACCATALYST
using Microsoft.Agents.AI.Workflows;

// The update record emitted for each streaming chunk
public record ItineraryStreamUpdate
{
    // Set when an agent transitions (e.g., "Researching destination…")
    public string? StatusMessage { get; init; }

    // Set when a new partial itinerary can be rendered (progressively decoded from streaming JSON)
    public Itinerary? PartialItinerary { get; init; }
}

public async IAsyncEnumerable<ItineraryStreamUpdate> StreamItineraryAsync(
    string input,
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    var deserializer = new StreamingJsonDeserializer<Itinerary>(jsonOptions);

    await foreach (var update in workflowAgent.RunStreamingAsync(input, cancellationToken: cancellationToken))
    {
        if (update.RawRepresentation is ExecutorStatusEvent statusEvent)
        {
            // Provides agent-transition messages such as "Researching destination…"
            yield return new ItineraryStreamUpdate { StatusMessage = statusEvent.StatusMessage };
        }
        else if (update.RawRepresentation is ItineraryTextChunkEvent textChunk)
        {
            // Incrementally deserialize streaming JSON for progressive rendering
            var partial = deserializer.ProcessChunk(textChunk.TextChunk);
            yield return new ItineraryStreamUpdate { PartialItinerary = partial };
        }
    }
}
#endif
```

`RunStreamingAsync` emits two kinds of events:

| Event type | When emitted | Typical use |
|---|---|---|
| `ExecutorStatusEvent` | When an agent starts or finishes | Display progress messages to the user. |
| `ItineraryTextChunkEvent` | As the final agent generates text | Stream content into the UI progressively. |

## Best practices

- **Keep agents focused.** Each agent should do one thing well. A narrow system prompt produces more reliable, predictable output than a broad one that tries to cover multiple tasks.
- **Use strong typing between agents.** Defining explicit record types for each agent's output makes it easy to reason about what data flows through the workflow and simplifies debugging.
- **Prefer streaming for long-running workflows.** Multi-agent pipelines can take several seconds to complete. Use `RunStreamingAsync` and surface `ExecutorStatusEvent` messages so users know the app is working.
- **Pass `CancellationToken` throughout.** Pass the token from the originating UI gesture (such as a button tap) all the way through `RunStreamingAsync` and into any tool implementations so the entire pipeline can be cancelled cleanly.
- **Validate structured output.** When an agent is configured with `ResponseFormat = ChatResponseFormat.ForJsonSchema<T>()`, deserialize the output and handle schema mismatches gracefully rather than assuming the JSON is always valid.

## See also

- [Microsoft.Maui.Essentials.AI overview](index.md)
- [Get started with Microsoft.Maui.Essentials.AI](getting-started.md)
- [Platform APIs](platform-apis.md)
- [Feature comparison](feature-comparison.md)
