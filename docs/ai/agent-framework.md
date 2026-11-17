---
title: Microsoft Agent Framework integration
description: Learn how to compose multiple AI agents into multi-stage workflows that run entirely on-device using Microsoft.Agents.AI and Microsoft.Maui.Essentials.AI.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Microsoft Agent Framework integration

> [!IMPORTANT]
> Each agent can only use the features that the platform's chat client supports. The workflow in this article uses tool calling and structured output, which Apple Intelligence supports and Gemini Nano supports on capable models, such as `gemini-nano-preview-fast`. Windows AI doesn't support tool calling. See the [chat feature comparison](chat/feature-comparison.md).

## Overview

The [Microsoft Agent Framework](/agent-framework/overview/agent-framework-overview) ([Microsoft.Agents.AI](https://www.nuget.org/packages/Microsoft.Agents.AI)) enables you to compose multiple AI agents into structured workflows, where each agent handles one specialized task. When combined with `Microsoft.Maui.Essentials.AI`, you can build multi-stage AI pipelines that run entirely on-device.

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

Install `Microsoft.Maui.Essentials.AI` and suppress its experimental warning as described in [Get started](getting-started.md), then add the Agent Framework packages to your `.csproj` file:

```xml
<PackageReference Include="Microsoft.Extensions.AI" Version="10.3.0" />
<PackageReference Include="Microsoft.Agents.AI" Version="1.0.0-rc3" />
<PackageReference Include="Microsoft.Agents.AI.Hosting" Version="1.0.0-preview.260304.1" />
<PackageReference Include="Microsoft.Agents.AI.Workflows" Version="1.0.0-rc3" />
<PackageReference Include="Microsoft.Agents.AI.Workflows.Generators" Version="1.0.0-rc3" />
```

## Register AI services

Register the platform's chat client as a **keyed service**, so each agent in the workflow can resolve it by name. In `MauiProgram.cs`:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

var builder = MauiApp.CreateBuilder();

#if IOS || MACCATALYST
builder.Services.AddKeyedChatClient("local-model", new AppleIntelligenceChatClient())
    .UseLogging();
#elif ANDROID
builder.Services.AddKeyedChatClient("local-model", new GeminiNanoChatClient())
    .ConfigureOptions(options =>
    {
        options.ModelId ??= "gemini-nano-preview-fast";
        options.MaxOutputTokens ??= 1024;
    })
    .UseFunctionInvocation()
    .UseLogging();
#endif

// Register workflow agents and executors (cross-platform)
builder.AddItineraryWorkflow();

var app = builder.Build();
```

On Android, `ConfigureOptions` selects a model that supports tool calling and sets an output limit for every agent. `UseFunctionInvocation` runs the tool calls that Gemini Nano returns. The `"local-model"` key is a convention used in the sample; you can use any key.

## Define workflow models

Each stage of a workflow passes a strongly typed result to the next stage. Defining explicit record types makes each agent's contract clear and enables the framework to serialize results between agents.

```csharp
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
```

## Create agents

### Simple agent with instructions only

The simplest form of an agent is registered with `AddAIAgent()`, supplying a name, a system prompt, and the keyed service that provides the `IChatClient`:

```csharp
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
```

### Agent with a RAG context provider

A **context provider** retrieves relevant data at runtime and injects it into the agent's prompt. Use this pattern when an agent needs grounding information (such as landmark descriptions) that is not baked into the model:

```csharp
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
```

### Agent with tool calling

Agents can invoke tools—arbitrary .NET methods decorated with `AIFunctionFactory.Create`—to retrieve live data during generation:

```csharp
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
```

## Build the workflow

Connect executors into a directed graph using `WorkflowBuilder`. Use `AddEdge` for unconditional transitions and `AddSwitch` for conditional branching:

```csharp
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
```

> [!TIP]
> `AddSwitch` evaluates cases in the order they are added. The first case whose condition returns `true` is taken; `WithDefault` handles all other situations.

## Run the workflow

### Non-streaming

For short workflows where you only need the final result:

```csharp
var result = await workflowAgent.RunAsync<ItineraryResult>(
    input: userInput,
    cancellationToken: cancellationToken);

// result.ItineraryJson contains the final JSON (translated if applicable)
var itinerary = JsonSerializer.Deserialize<Itinerary>(result.ItineraryJson, jsonOptions);
```

### Streaming

For long-running generation, use `RunStreamingAsync` to receive incremental updates as each agent produces output. This lets you update the UI in real time instead of waiting for the entire workflow to finish:

```csharp
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
- [Get started](getting-started.md)
- [Chat feature comparison](chat/feature-comparison.md)
- [Requirements](requirements.md)
- [Microsoft Agent Framework docs](/agent-framework/overview/agent-framework-overview)
- [Use the IChatClient interface](/dotnet/ai/ichatclient)
