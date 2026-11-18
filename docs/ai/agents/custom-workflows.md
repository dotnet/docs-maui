---
title: Custom workflow graphs
description: Connect on-device agents and your own code in a custom Microsoft Agent Framework workflow graph with executors, edges, and conditional routing in .NET MAUI.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Custom workflow graphs

The [built-in workflow patterns](workflows.md) pass a conversation between agents. When you need typed data between steps, steps written in plain C#, or branches that depend on a result, build your own graph with `WorkflowBuilder`.

A custom workflow has two parts:

- **Executors** are the steps. Each executor receives a typed message, does its work (calling an agent or running your own code), and sends a typed result onward.
- **Edges** connect the executors. An edge can have a condition, so the workflow can branch.

This article builds a trip planner that extracts the request, plans an itinerary, and translates it only when the user asked for another language.

## Define the messages

Use records for the data that flows between executors:

```csharp
public sealed record TripRequest(string Destination, int Days, string Language);

public sealed record Itinerary(string Text, string Language);
```

## Create executors

Derive from `Executor<TInput, TOutput>` and override `HandleAsync`. The value you return is sent to the connected executors. This executor uses an agent's [structured output](tools-and-context.md#get-structured-output) to turn the user's text into a `TripRequest`:

```csharp
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;

internal sealed class ExtractRequestExecutor(AIAgent agent)
    : Executor<string, TripRequest>("ExtractRequest")
{
    public override async ValueTask<TripRequest> HandleAsync(
        string message, IWorkflowContext context, CancellationToken cancellationToken = default)
    {
        AgentResponse<TripRequest> response =
            await agent.RunAsync<TripRequest>(message, cancellationToken: cancellationToken);
        return response.Result;
    }
}

internal sealed class PlanExecutor(AIAgent agent)
    : Executor<TripRequest, Itinerary>("Plan")
{
    public override async ValueTask<Itinerary> HandleAsync(
        TripRequest request, IWorkflowContext context, CancellationToken cancellationToken = default)
    {
        AgentResponse response = await agent.RunAsync(
            $"Plan {request.Days} days in {request.Destination}.", cancellationToken: cancellationToken);
        return new Itinerary(response.Text, request.Language);
    }
}
```

An executor at the end of the graph derives from `Executor<TInput>` and calls `YieldOutputAsync` to return a result to the caller:

```csharp
internal sealed class TranslateExecutor(AIAgent agent) : Executor<Itinerary>("Translate")
{
    public override async ValueTask HandleAsync(
        Itinerary itinerary, IWorkflowContext context, CancellationToken cancellationToken = default)
    {
        AgentResponse response = await agent.RunAsync(
            $"Translate into {itinerary.Language}:\n{itinerary.Text}", cancellationToken: cancellationToken);
        await context.YieldOutputAsync(response.Text, cancellationToken);
    }
}

internal sealed class OutputExecutor() : Executor<Itinerary>("Output")
{
    public override ValueTask HandleAsync(
        Itinerary itinerary, IWorkflowContext context, CancellationToken cancellationToken = default) =>
        context.YieldOutputAsync(itinerary.Text, cancellationToken);
}
```

`OutputExecutor` doesn't call the model; it's plain C#. Use code instead of an agent for any step that has an exact answer, such as validation, calculations, or saving data.

## Connect the executors

Pass the first executor to `WorkflowBuilder`, and then add edges. `AddSwitch` checks its cases in order and sends the message to the first match, or to the default:

```csharp
var extract = new ExtractRequestExecutor(extractAgent);
var plan = new PlanExecutor(plannerAgent);
var translate = new TranslateExecutor(translatorAgent);
var output = new OutputExecutor();

Workflow workflow = new WorkflowBuilder(extract)
    .AddEdge(extract, plan)
    .AddSwitch(plan, cases => cases
        .AddCase(
            message => message is Itinerary itinerary &&
                !string.Equals(itinerary.Language, "English", StringComparison.OrdinalIgnoreCase),
            translate)
        .WithDefault(output))
    .WithOutputFrom(translate, output)
    .Build();
```

`WithOutputFrom` lists the executors whose outputs are returned to the caller.

## Run the workflow

Run the workflow with the input type of the first executor, and then watch for `WorkflowOutputEvent`:

```csharp
await using StreamingRun run = await InProcessExecution.RunStreamingAsync(
    workflow, "Plan 3 days in Kyoto, in French.");
await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

await foreach (WorkflowEvent evt in run.WatchStreamAsync())
{
    if (evt is WorkflowOutputEvent output)
    {
        Console.WriteLine(output.As<string>());
    }
}
```

Pass a `CancellationToken` from the UI to every agent call, so users can cancel a long-running workflow.

## See also

- [Multi-agent workflows](workflows.md)
- [On-device agents](index.md)
- [Agent Framework executors](/agent-framework/workflows/executors)
- [Agent Framework edges](/agent-framework/workflows/edges)
