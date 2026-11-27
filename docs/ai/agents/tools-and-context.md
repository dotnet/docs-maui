---
title: Agent tools, context, and structured output
description: Give on-device Microsoft Agent Framework agents tools, app context, retrieval-augmented generation, and typed results in .NET MAUI.
ms.date: 11/27/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Agent tools, context, and structured output

An agent's instructions tell the model what to do. Tools, context providers, and structured output connect the agent to your app's data and code. This article assumes you've [created an agent](index.md#create-an-agent) with `UseProvidedChatClientAsIs = true`.

## Add tools

Tools let the agent call your app's methods. Wrap methods with `AIFunctionFactory.Create` and pass them in `ChatOptions.Tools`:

```csharp
using System.ComponentModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

FavoritesService favorites = // resolved from DI

AIAgent agent = chatClient.AsAIAgent(new ChatClientAgentOptions
{
    Name = "SessionAssistant",
    UseProvidedChatClientAsIs = true,
    ChatOptions = new ChatOptions
    {
        Instructions = """
            You help attendees manage their saved sessions.
            Call get_favorites to read saved sessions and add_favorite to save one.
            Never announce tool calls in text. Either call the tool or answer directly.
            """,
        Tools =
        [
            AIFunctionFactory.Create(favorites.GetFavoritesAsync, name: "get_favorites"),
            AIFunctionFactory.Create(favorites.AddFavoriteAsync, name: "add_favorite"),
        ],
    },
});

public class FavoritesService
{
    [Description("Gets the titles of the user's saved sessions.")]
    public Task<string[]> GetFavoritesAsync() => ...;

    [Description("Saves a session to the user's favorites.")]
    public Task AddFavoriteAsync([Description("The exact session title.")] string title) => ...;
}
```

Tool support depends on the platform:

- **Apple**: Apple Intelligence runs the tools itself.
- **Android**: The registered client runs tools with `UseFunctionInvocation`. Tools require a model that reports `ChatModelCapability.ToolCalling`, such as `gemini-nano-preview-fast`. See [Tool calling on Android](../chat/android.md#tool-calling).
- **Windows**: Tools aren't supported. Use [context providers](#add-context) to supply data instead.

To generate tools from attributed methods at compile time instead of calling `AIFunctionFactory.Create`, see [Source-generated AI tools](../tool-attributes.md).

Small models choose tools more reliably when you:

- Keep the number of tools small, about three to six per agent.
- Use distinct verb-noun names, such as `get_session` and `add_favorite`.
- Use a few simple parameters, such as strings and numbers, rather than nested objects.
- Name each tool in the instructions and say when to call it.

Put permission checks and confirmation inside the tool, especially for tools that change data. Apple Intelligence runs tools natively, so middleware such as tool approval doesn't see the calls.

## Add context

A *context provider* adds information to each request before the agent calls the model. Use one to supply data that the model needs, such as the user's preferences or the current schedule, without a tool call. Derive from `AIContextProvider` and return instructions from `ProvideAIContextAsync`:

```csharp
using Microsoft.Agents.AI;

public sealed class UserContextProvider(UserPreferencesService preferences) : AIContextProvider
{
    protected override async ValueTask<AIContext> ProvideAIContextAsync(
        InvokingContext context,
        CancellationToken cancellationToken = default)
    {
        var city = await preferences.GetSelectedCityAsync();
        var favorites = await preferences.GetFavoriteTitlesAsync();

        return new AIContext
        {
            Instructions = $"""
                Selected city: {city.Name}
                Saved favorites: {(favorites.Length > 0 ? string.Join("; ", favorites) : "None yet")}
                """
        };
    }
}
```

Add the provider to the agent with `AIContextProviders`:

```csharp
AIAgent agent = chatClient.AsAIAgent(new ChatClientAgentOptions
{
    Name = "SessionAssistant",
    UseProvidedChatClientAsIs = true,
    ChatOptions = new ChatOptions { Instructions = "Answer questions about the user's schedule." },
    AIContextProviders = [new UserContextProvider(preferences)],
});
```

The same provider instance serves every session, so don't store per-session state in its fields. Keep the injected text short. Every provider's output counts toward the model's small context window. Filter and summarize data in C# before you inject it; for example, include one-line summaries instead of full descriptions.

### Retrieve relevant content

For retrieval-augmented generation (RAG), use `TextSearchProvider`. It runs your search function before each model call and adds the results to the request:

```csharp
using Microsoft.Agents.AI;

var ragProvider = new TextSearchProvider(
    async (query, cancellationToken) =>
    {
        var matches = await searchService.SearchSessionsAsync(query, top: 3, cancellationToken);
        return matches.Select(session => new TextSearchProvider.TextSearchResult
        {
            SourceName = session.Title,
            SourceLink = session.Id,
            Text = session.Summary,
        });
    },
    new TextSearchProviderOptions
    {
        SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke,
    });
```

Your search function can use any search technology. On Apple platforms, you can rank content with [on-device text embeddings](../embeddings.md#semantic-similarity-search). Return only a few short results.

If you set `TextSearchProviderOptions.CitationsPrompt`, make it conditional, for example "If the user is asking about one of the sessions above, ...". Small models tend to follow a rigid instruction even when the results aren't relevant to the question.

## Get structured output

Use `RunAsync<T>` to get a typed result. For trimmed and Native AOT apps, pass source-generated `JsonSerializerOptions`:

```csharp
using System.Text.Json.Serialization;
using Microsoft.Agents.AI;

AgentResponse<TripPlan> response = await agent.RunAsync<TripPlan>(
    "Plan a 2-day trip to Kyoto.",
    serializerOptions: AppJsonContext.Default.Options);

Console.WriteLine(response.Result.Destination);

public sealed record TripPlan(string Destination, string[] Days);

[JsonSerializable(typeof(TripPlan))]
internal partial class AppJsonContext : JsonSerializerContext;
```

Keep schemas small and flat, and avoid nullable properties where you can. Each platform supports a different subset of JSON Schema. See [Structured output](../chat.md#structured-output).

## See also

- [On-device agents](index.md)
- [Multi-agent workflows](workflows.md)
- [Agent Framework function tools](/agent-framework/agents/tools/function-tools)
- [Agent Framework context providers](/agent-framework/concepts/agents/conversations/context-providers)
- [Agent Framework RAG](/agent-framework/agents/rag)
