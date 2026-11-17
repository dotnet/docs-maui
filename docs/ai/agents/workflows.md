---
title: Multi-agent workflows
description: Combine on-device agents into sequential, concurrent, and group chat workflows with Microsoft Agent Framework in .NET MAUI.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Multi-agent workflows

A workflow runs several agents in an order that you define. Each agent does one small task, which suits on-device models better than one agent with a long prompt. `AgentWorkflowBuilder` in the `Microsoft.Agents.AI.Workflows` package builds the common patterns:

| Pattern | Builder method | Use it to |
|---------|----------------|-----------|
| Sequential | `BuildSequential` | Pass the conversation from one agent to the next, such as research and then write. |
| Concurrent | `BuildConcurrent` | Run agents on the same input in parallel and collect all their answers. |
| Group chat | `CreateGroupChatBuilderWith` | Let agents take turns in a shared conversation, such as a panel discussion. |

The examples in this article create agents with a helper method. Every agent uses the registered `IChatClient` with `UseProvidedChatClientAsIs = true`, as described in [Create an agent](index.md#create-an-agent):

```csharp
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

AIAgent CreateAgent(string name, string instructions, IList<AITool>? tools = null) =>
    chatClient.AsAIAgent(new ChatClientAgentOptions
    {
        Name = name,
        UseProvidedChatClientAsIs = true,
        ChatOptions = new ChatOptions
        {
            Instructions = instructions,
            Temperature = 0,
            Tools = tools,
        },
    });
```

## Sequential workflows

In a sequential workflow, each agent receives the conversation so far, including the previous agent's response. The following workflow gathers facts with tools, and then writes a summary without tools:

```csharp
AIAgent researcher = CreateAgent(
    "SessionResearcher",
    "Call get_session and get_speaker for the session the user asks about. Write a short factual brief.",
    [getSessionTool, getSpeakerTool]);

AIAgent writer = CreateAgent(
    "TakeawaysWriter",
    "Using the brief, write 3 bullet points about what to expect from the session. Keep each bullet to 1 sentence.");

Workflow workflow = AgentWorkflowBuilder.BuildSequential("SessionDeepDive", [researcher, writer]);
```

Separating tool calls from writing makes both agents more reliable on small models.

## Concurrent workflows

In a concurrent workflow, every agent receives the same input and runs at the same time. The workflow returns all of their responses:

```csharp
AIAgent takeaways = CreateAgent(
    "TakeawaysAgent",
    "Write 3 bullet points about the key takeaways from the session described.");

AIAgent questions = CreateAgent(
    "QuestionsAgent",
    "Write 2 questions the attendee could ask the speaker about the session described.");

Workflow workflow = AgentWorkflowBuilder.BuildConcurrent("SessionAnalysis", [takeaways, questions]);
```

Text from concurrent agents arrives interleaved. Group streamed updates by `AuthorName` or `ExecutorId` before you display them, or show one agent's text live and the others when they finish.

### Combine patterns

A workflow can become an agent with `AsAIAgent`, so you can nest one pattern inside another. The following workflow researches a session, and then runs two writers in parallel:

```csharp
AIAgent analysis = AgentWorkflowBuilder
    .BuildConcurrent("SessionAnalysis", [takeaways, questions])
    .AsAIAgent(name: "SessionAnalysis");

Workflow workflow = AgentWorkflowBuilder.BuildSequential("SessionDeepDive", [researcher, analysis]);
```

## Group chat workflows

In a group chat, agents take turns in one shared conversation, and a manager picks the next speaker. `RoundRobinGroupChatManager` lets each agent speak in order:

```csharp
AIAgent depth = CreateAgent(
    "DepthAdvocate",
    "Suggest 2 sessions that go deepest into the user's topics. Give a 1-sentence reason for each.");

AIAgent breadth = CreateAgent(
    "BreadthAdvocate",
    "Suggest 2 different sessions than DepthAdvocate that broaden the user's perspective.");

AIAgent moderator = CreateAgent(
    "Moderator",
    "List the best 3 or 4 sessions suggested so far, in time order, as bullet points.");

Workflow workflow = AgentWorkflowBuilder
    .CreateGroupChatBuilderWith(agents => new RoundRobinGroupChatManager(agents)
    {
        MaximumIterationCount = 3,
    })
    .AddParticipants(depth, breadth, moderator)
    .Build();
```

Every turn adds to the shared conversation, so keep the number of agents and turns small to stay within the model's context window. To pause for user input, for example to confirm the user's topics before the panel runs, pass a termination function to the `RoundRobinGroupChatManager` constructor that ends the run after the first agent's turn.

## Run a workflow

Run a workflow with `InProcessExecution.RunStreamingAsync`, and then watch its events. `AgentResponseUpdateEvent` carries streamed text from each agent, and `WorkflowOutputEvent` carries the final result:

```csharp
List<ChatMessage> input = [new(ChatRole.User, "Give me a deep dive on the .NET MAUI keynote.")];

await using StreamingRun run = await InProcessExecution.RunStreamingAsync(workflow, input);
await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

await foreach (WorkflowEvent evt in run.WatchStreamAsync())
{
    if (evt is AgentResponseUpdateEvent update)
    {
        Console.Write(update.Update.Text);
    }
    else if (evt is WorkflowOutputEvent output)
    {
        List<ChatMessage> messages = output.As<List<ChatMessage>>()!;
        break;
    }
}
```

Use `update.ExecutorId` to show which agent is working, for example in a progress indicator.

### Use a workflow as a chat client

To plug a workflow into an existing chat UI, convert it to an agent, and then to an `IChatClient`:

```csharp
IChatClient workflowClient = workflow
    .AsAIAgent(name: "SessionDeepDive")
    .AsIChatClient(allowNonChatClientAgents: true);

await foreach (var update in workflowClient.GetStreamingResponseAsync(history))
{
    Console.Write(update.Text);
}
```

Each update's `AuthorName` identifies the agent that wrote it. To continue a multi-turn workflow, such as a group chat that paused for user input, add the response messages to `history` and send them with the next request.

## See also

- [On-device agents](index.md)
- [Custom workflow graphs](custom-workflows.md)
- [Agent Framework workflow orchestrations](/agent-framework/workflows/orchestrations/)
- [Use workflows as agents](/agent-framework/workflows/as-agents)
