---
title: On-device agents with Microsoft Agent Framework
description: Build on-device AI agents in .NET MAUI with Microsoft Agent Framework and the Microsoft.Maui.Essentials.AI chat clients.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# On-device agents with Microsoft Agent Framework

[Microsoft Agent Framework](/agent-framework/overview/) builds AI agents and multi-agent workflows on top of `IChatClient`. Because the `Microsoft.Maui.Essentials.AI` chat clients implement `IChatClient`, your agents can run entirely on the device.

An *agent* combines a chat client with instructions, and optionally tools and context. A *workflow* connects several agents, or agents and your own code, in an explicit order.

## Choose an approach

| Use | When |
|-----|------|
| `IChatClient` directly | One prompt produces the result you need. See [Chat](../chat.md). |
| An agent | The task is conversational, or the model needs tools or extra context to answer. |
| A workflow | The task has distinct steps that different agents handle, or you need control over the order. See [Multi-agent workflows](workflows.md). |

If you can write a function to handle a task, do that instead of using an agent.

## Install the packages

Add the Agent Framework packages to your .NET MAUI project. You need `Microsoft.Agents.AI.Workflows` only for workflows:

```dotnetcli
dotnet add package Microsoft.Agents.AI
dotnet add package Microsoft.Agents.AI.Workflows
```

Then [register the chat client](../getting-started.md#register-the-chat-client) for each platform.

## Create an agent

Create an agent from the registered `IChatClient` with `AsAIAgent`:

```csharp
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

IChatClient chatClient = // resolved from DI

AIAgent agent = chatClient.AsAIAgent(new ChatClientAgentOptions
{
    Name = "TravelAssistant",
    UseProvidedChatClientAsIs = true,
    ChatOptions = new ChatOptions
    {
        Instructions = "You are a travel assistant. Answer in 2 or 3 short sentences.",
        Temperature = 0,
    },
});

AgentResponse response = await agent.RunAsync("What should I pack for Tokyo in spring?");
Console.WriteLine(response.Text);
```

Set `UseProvidedChatClientAsIs` to `true`. By default, an agent adds its own function-invocation middleware to the chat client. Apple Intelligence already runs tools itself, and the Android registration already includes `UseFunctionInvocation`, so the client you registered is ready to use as is.

### Stream the response

Use `RunStreamingAsync` to display text as it's generated:

```csharp
await foreach (AgentResponseUpdate update in agent.RunStreamingAsync("Suggest a walking route in Kyoto."))
{
    Console.Write(update.Text);
}
```

### Continue a conversation

An `AgentSession` keeps the conversation history between runs:

```csharp
AgentSession session = await agent.CreateSessionAsync();

await agent.RunAsync("I'm visiting Tokyo for three days.", session);
AgentResponse followUp = await agent.RunAsync("What should I see on the first day?", session);
Console.WriteLine(followUp.Text);
```

On-device models have small context windows, so start a new session when the user starts a new task.

## Platform support

Agents and workflows run on every platform that has an on-device chat client. Each agent can use only the features that the platform's model supports:

| Feature | Apple | Windows | Android |
|---------|-------|---------|---------|
| Agents and workflows | Supported | Supported | Supported |
| Tools | Supported | Not supported | On models with `ChatModelCapability.ToolCalling` |
| Structured output | Supported | Supported | On models with `ChatModelCapability.StructuredOutput` |

For details, see the [chat feature comparison](../chat/feature-comparison.md).

## Design for on-device models

On-device models are smaller than cloud models. These practices make agents more reliable:

- **Give each agent one focused task.** Split a large task across several agents rather than writing one long prompt.
- **Supply context instead of asking the model to find it.** Inject small, relevant data with a [context provider](tools-and-context.md#add-context) rather than relying on several rounds of tool calls.
- **Separate gathering from writing.** Let one agent call tools to collect facts, then pass its summary to tool-free agents that write the result.
- **Control length in the instructions.** Ask for a specific number of bullet points or sentences. On Android, set a generous `MaxOutputTokens`, such as 1,024, because a tight limit cuts responses off mid-sentence, and the default Gemini Nano model stops after about 40 tokens when it's unset.
- **Use `Temperature = 0`** for predictable answers.

## Next steps

- [Agent tools, context, and structured output](tools-and-context.md)
- [Multi-agent workflows](workflows.md)
- [Custom workflow graphs](custom-workflows.md)

## See also

- [Microsoft Agent Framework overview](/agent-framework/overview/)
- [Chat client](../chat.md)
