---
title: "Microsoft Agent Framework integration"
description: "Learn how to build multi-agent AI workflows in .NET MAUI apps using Microsoft.Agents.AI with Microsoft.Maui.Essentials.AI."
ms.date: 02/02/2026
---

# Microsoft Agent Framework integration

This article explains how to build sophisticated multi-agent AI workflows in .NET MAUI applications using the Microsoft Agent Framework (`Microsoft.Agents.AI`) together with `Microsoft.Maui.Essentials.AI`.

## Overview

The Microsoft Agent Framework enables you to compose multiple AI agents into workflows where each agent handles a specific task. This is particularly useful for complex scenarios like:

- **Travel planning**: Research → Itinerary generation → Translation
- **Content creation**: Research → Draft → Edit → Format
- **Data processing**: Extract → Transform → Analyze → Summarize

## Key concepts

### Agents and executors

| Concept | Description |
|---------|-------------|
| **Agent** | An AI-powered component that performs a specific task |
| **Executor** | A processing unit that transforms input to output (`Executor<TIn, TOut>`) |
| **Workflow** | A composition of executors connected by edges |
| **Edge** | A connection between executors (can be unconditional or conditional) |

### Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                      Workflow Orchestration                      │
│                      (Microsoft.Agents.AI)                       │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐      │
│  │ Agent 1 │ →  │ Agent 2 │ →  │ Agent 3 │ →  │ Agent N │      │
│  │Research │    │ Plan    │    │Translate│    │ Output  │      │
│  └─────────┘    └─────────┘    └─────────┘    └─────────┘      │
├─────────────────────────────────────────────────────────────────┤
│               Microsoft.Extensions.AI (IChatClient)              │
├─────────────────────────────────────────────────────────────────┤
│              Microsoft.Maui.Essentials.AI                        │
│              (AppleIntelligenceChatClient, etc.)                 │
└─────────────────────────────────────────────────────────────────┘
```

## Getting started

### Installation

Add the required packages:

```shell
dotnet add package Microsoft.Maui.Essentials.AI
dotnet add package Microsoft.Agents.AI
```

### Define workflow models

Create models for data passed between agents:

```csharp
// Input to the workflow
public record TravelRequest(
    string Destination,
    int DurationDays,
    string Preferences,
    string? TargetLanguage = null);

// Research results from first agent
public record ResearchData(
    string Destination,
    List<string> Attractions,
    List<string> Restaurants,
    List<string> Tips,
    string WeatherInfo);

// Itinerary from planning agent
public record Itinerary(
    string Destination,
    int DurationDays,
    List<ItineraryDay> Days);

public record ItineraryDay(
    int DayNumber,
    string Theme,
    List<Activity> Activities);

public record Activity(
    string Name,
    string Description,
    string Time,
    string Location,
    string Type);

// Final output
public record TravelPlan(
    Itinerary Itinerary,
    string Language,
    bool WasTranslated);
```

### Create executor classes

Each executor handles one stage of the workflow:

#### Research executor

```csharp
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

public class ResearcherExecutor : Executor<TravelRequest, ResearchData>
{
    private readonly IChatClient _chatClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ResearcherExecutor(IChatClient chatClient)
    {
        _chatClient = chatClient;
        _jsonOptions = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        };
    }

    public override async Task<ResearchData> ExecuteAsync(
        TravelRequest input, 
        CancellationToken cancellationToken = default)
    {
        var prompt = $"""
            Research travel information for {input.Destination}.
            Find:
            - Top attractions and things to do
            - Recommended restaurants
            - Local tips and customs
            - Weather information for the travel period
            
            User preferences: {input.Preferences}
            """;

        var options = new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<ResearchData>(_jsonOptions)
        };

        var response = await _chatClient.GetResponseAsync(prompt, options, cancellationToken);
        return JsonSerializer.Deserialize<ResearchData>(response.Text, _jsonOptions)!;
    }
}
```

#### Itinerary planner executor

```csharp
public class ItineraryPlannerExecutor : Executor<(TravelRequest Request, ResearchData Research), Itinerary>
{
    private readonly IChatClient _chatClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ItineraryPlannerExecutor(IChatClient chatClient)
    {
        _chatClient = chatClient;
        _jsonOptions = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        };
    }

    public override async Task<Itinerary> ExecuteAsync(
        (TravelRequest Request, ResearchData Research) input,
        CancellationToken cancellationToken = default)
    {
        var prompt = $"""
            Create a detailed {input.Request.DurationDays}-day itinerary for {input.Request.Destination}.
            
            Available attractions: {string.Join(", ", input.Research.Attractions)}
            Recommended restaurants: {string.Join(", ", input.Research.Restaurants)}
            Local tips: {string.Join(", ", input.Research.Tips)}
            Weather: {input.Research.WeatherInfo}
            
            User preferences: {input.Request.Preferences}
            
            Create a day-by-day plan with specific activities, times, and locations.
            """;

        var options = new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<Itinerary>(_jsonOptions)
        };

        var response = await _chatClient.GetResponseAsync(prompt, options, cancellationToken);
        return JsonSerializer.Deserialize<Itinerary>(response.Text, _jsonOptions)!;
    }
}
```

#### Translator executor (conditional)

```csharp
public class TranslatorExecutor : Executor<(Itinerary Itinerary, string TargetLanguage), Itinerary>
{
    private readonly IChatClient _chatClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public TranslatorExecutor(IChatClient chatClient)
    {
        _chatClient = chatClient;
        _jsonOptions = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        };
    }

    public override async Task<Itinerary> ExecuteAsync(
        (Itinerary Itinerary, string TargetLanguage) input,
        CancellationToken cancellationToken = default)
    {
        var itineraryJson = JsonSerializer.Serialize(input.Itinerary, _jsonOptions);
        
        var prompt = $"""
            Translate this travel itinerary to {input.TargetLanguage}.
            Keep the JSON structure exactly the same, only translate the text content.
            
            {itineraryJson}
            """;

        var options = new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<Itinerary>(_jsonOptions)
        };

        var response = await _chatClient.GetResponseAsync(prompt, options, cancellationToken);
        return JsonSerializer.Deserialize<Itinerary>(response.Text, _jsonOptions)!;
    }
}
```

### Build the workflow

Compose executors into a workflow using `WorkflowBuilder`:

```csharp
using Microsoft.Agents.AI;

public class TravelPlanningWorkflow
{
    private readonly ResearcherExecutor _researcher;
    private readonly ItineraryPlannerExecutor _planner;
    private readonly TranslatorExecutor _translator;

    public TravelPlanningWorkflow(IChatClient chatClient)
    {
        _researcher = new ResearcherExecutor(chatClient);
        _planner = new ItineraryPlannerExecutor(chatClient);
        _translator = new TranslatorExecutor(chatClient);
    }

    public async Task<TravelPlan> ExecuteAsync(
        TravelRequest request,
        CancellationToken cancellationToken = default)
    {
        // Step 1: Research
        var research = await _researcher.ExecuteAsync(request, cancellationToken);

        // Step 2: Plan itinerary
        var itinerary = await _planner.ExecuteAsync((request, research), cancellationToken);

        // Step 3: Translate if needed (conditional)
        var wasTranslated = false;
        if (!string.IsNullOrEmpty(request.TargetLanguage) && 
            request.TargetLanguage.ToLower() != "english")
        {
            itinerary = await _translator.ExecuteAsync(
                (itinerary, request.TargetLanguage), 
                cancellationToken);
            wasTranslated = true;
        }

        return new TravelPlan(itinerary, request.TargetLanguage ?? "English", wasTranslated);
    }
}
```

### Use the workflow

```csharp
public class TravelPlannerViewModel : ObservableObject
{
    private readonly TravelPlanningWorkflow _workflow;

    [ObservableProperty]
    private TravelPlan? _plan;

    [ObservableProperty]
    private bool _isGenerating;

    [ObservableProperty]
    private string _currentStage = string.Empty;

    public TravelPlannerViewModel(IChatClient chatClient)
    {
        _workflow = new TravelPlanningWorkflow(chatClient);
    }

    [RelayCommand]
    private async Task GeneratePlanAsync(CancellationToken cancellationToken)
    {
        IsGenerating = true;
        
        try
        {
            var request = new TravelRequest(
                Destination: "Tokyo, Japan",
                DurationDays: 5,
                Preferences: "Cultural experiences, food tours, avoiding crowds",
                TargetLanguage: "Japanese");

            Plan = await _workflow.ExecuteAsync(request, cancellationToken);
        }
        finally
        {
            IsGenerating = false;
        }
    }
}
```

## Workflow events and progress

Track workflow progress with events:

```csharp
public interface IWorkflowEvents
{
    event EventHandler<WorkflowStageEventArgs>? StageStarted;
    event EventHandler<WorkflowStageEventArgs>? StageCompleted;
    event EventHandler<WorkflowErrorEventArgs>? StageError;
}

public record WorkflowStageEventArgs(string StageName, int StageNumber, int TotalStages);
public record WorkflowErrorEventArgs(string StageName, Exception Error);

public class TravelPlanningWorkflowWithEvents : IWorkflowEvents
{
    public event EventHandler<WorkflowStageEventArgs>? StageStarted;
    public event EventHandler<WorkflowStageEventArgs>? StageCompleted;
    public event EventHandler<WorkflowErrorEventArgs>? StageError;

    private readonly ResearcherExecutor _researcher;
    private readonly ItineraryPlannerExecutor _planner;
    private readonly TranslatorExecutor _translator;

    public TravelPlanningWorkflowWithEvents(IChatClient chatClient)
    {
        _researcher = new ResearcherExecutor(chatClient);
        _planner = new ItineraryPlannerExecutor(chatClient);
        _translator = new TranslatorExecutor(chatClient);
    }

    public async Task<TravelPlan> ExecuteAsync(
        TravelRequest request,
        CancellationToken cancellationToken = default)
    {
        var needsTranslation = !string.IsNullOrEmpty(request.TargetLanguage) && 
                              request.TargetLanguage.ToLower() != "english";
        var totalStages = needsTranslation ? 3 : 2;

        // Stage 1: Research
        StageStarted?.Invoke(this, new("Research", 1, totalStages));
        var research = await _researcher.ExecuteAsync(request, cancellationToken);
        StageCompleted?.Invoke(this, new("Research", 1, totalStages));

        // Stage 2: Plan
        StageStarted?.Invoke(this, new("Itinerary Planning", 2, totalStages));
        var itinerary = await _planner.ExecuteAsync((request, research), cancellationToken);
        StageCompleted?.Invoke(this, new("Itinerary Planning", 2, totalStages));

        // Stage 3: Translate (conditional)
        var wasTranslated = false;
        if (needsTranslation)
        {
            StageStarted?.Invoke(this, new("Translation", 3, totalStages));
            itinerary = await _translator.ExecuteAsync(
                (itinerary, request.TargetLanguage!), 
                cancellationToken);
            StageCompleted?.Invoke(this, new("Translation", 3, totalStages));
            wasTranslated = true;
        }

        return new TravelPlan(itinerary, request.TargetLanguage ?? "English", wasTranslated);
    }
}
```

### Display progress in UI

```csharp
public class TravelPlannerPageViewModel : ObservableObject
{
    private readonly TravelPlanningWorkflowWithEvents _workflow;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    [ObservableProperty]
    private double _progress;

    public TravelPlannerPageViewModel(IChatClient chatClient)
    {
        _workflow = new TravelPlanningWorkflowWithEvents(chatClient);
        _workflow.StageStarted += OnStageStarted;
        _workflow.StageCompleted += OnStageCompleted;
    }

    private void OnStageStarted(object? sender, WorkflowStageEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            StatusMessage = $"Step {e.StageNumber}/{e.TotalStages}: {e.StageName}...";
        });
    }

    private void OnStageCompleted(object? sender, WorkflowStageEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Progress = (double)e.StageNumber / e.TotalStages;
        });
    }
}
```

## Streaming with agents

Combine streaming responses with agent workflows:

```csharp
public class StreamingItineraryPlannerExecutor
{
    private readonly IChatClient _chatClient;

    public StreamingItineraryPlannerExecutor(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async IAsyncEnumerable<string> ExecuteStreamingAsync(
        TravelRequest request,
        ResearchData research,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var prompt = $"""
            Create a detailed {request.DurationDays}-day itinerary for {request.Destination}.
            
            Available attractions: {string.Join(", ", research.Attractions)}
            User preferences: {request.Preferences}
            
            Format as a readable day-by-day narrative.
            """;

        await foreach (var update in _chatClient.GetStreamingResponseAsync(prompt, cancellationToken: cancellationToken))
        {
            if (update.Text is not null)
            {
                yield return update.Text;
            }
        }
    }
}
```

## Best practices

### 1. Define clear agent responsibilities

Each agent should have a single, well-defined responsibility:

```csharp
// Good: Single responsibility
public class ResearcherExecutor { /* Only researches */ }
public class PlannerExecutor { /* Only plans */ }
public class TranslatorExecutor { /* Only translates */ }

// Avoid: Multiple responsibilities in one agent
public class DoEverythingExecutor { /* Researches, plans, and translates */ }
```

### 2. Use typed inputs and outputs

Strong typing helps catch errors at compile time:

```csharp
// Good: Strongly typed
public class ResearcherExecutor : Executor<TravelRequest, ResearchData>

// Avoid: Weakly typed
public class ResearcherExecutor : Executor<string, object>
```

### 3. Handle cancellation throughout

Pass cancellation tokens through the entire workflow:

```csharp
public override async Task<TOutput> ExecuteAsync(
    TInput input,
    CancellationToken cancellationToken = default)
{
    cancellationToken.ThrowIfCancellationRequested();
    
    var response = await _chatClient.GetResponseAsync(
        prompt, 
        options, 
        cancellationToken);
    
    // ...
}
```

### 4. Implement error recovery

Add retry logic and fallbacks:

```csharp
public async Task<Itinerary> ExecuteWithRetryAsync(
    TravelRequest request,
    int maxRetries = 3,
    CancellationToken cancellationToken = default)
{
    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            return await ExecuteAsync(request, cancellationToken);
        }
        catch (Exception ex) when (attempt < maxRetries)
        {
            await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
        }
    }
    
    throw new InvalidOperationException("Failed after max retries");
}
```

## External resources

- [Microsoft.Extensions.AI documentation](https://learn.microsoft.com/dotnet/ai/ai-extensions)
- [Microsoft Agents SDK](https://github.com/microsoft/agents)
- [AutoGen for .NET](https://microsoft.github.io/autogen-for-net/)
- [Semantic Kernel](https://learn.microsoft.com/semantic-kernel/)

## See also

- [Getting started](getting-started.md)
- [Platform APIs](platform-apis.md)
- [Feature comparison](feature-comparison.md)
