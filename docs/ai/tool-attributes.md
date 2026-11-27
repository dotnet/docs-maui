---
title: Source-generated AI tools
description: Use Microsoft.Maui.AI.Attributes to turn .NET methods and properties into Microsoft.Extensions.AI tools at compile time, with dependency injection and approval support.
ms.date: 11/27/2026
ms.topic: how-to
ai-usage: ai-assisted
---

# Source-generated AI tools

`Microsoft.Maui.AI.Attributes` turns methods and property accessors in your app into [Microsoft.Extensions.AI](/dotnet/ai/microsoft-extensions-ai) tools at compile time. Mark a member with `[ExportAIFunction]`, and a source generator creates an `AIFunction` for it. You don't call `AIFunctionFactory.Create` for each tool, and the generated tools don't use reflection to invoke your methods.

The generated tools are standard `AITool` instances, so you can pass them to any `IChatClient` that supports tool calling, including the on-device chat clients in [Microsoft.Maui.Essentials.AI](chat.md#tool-calling) and cloud providers.

> [!IMPORTANT]
> `Microsoft.Maui.AI.Attributes` is experimental and subject to change.

## Install the package

The package targets .NET 10 and later. It works in .NET MAUI apps and in any other .NET project, and doesn't depend on `Microsoft.Maui.Essentials.AI`:

```dotnetcli
dotnet add package Microsoft.Maui.AI.Attributes --version 0.1.0-preview.14.26573.4
```

> [!WARNING]
> The source generator is missing from package versions `0.1.0-preview.15` through at least `0.1.0-preview.16.26575.3`. With those versions, the project fails to build with error CS0534 because the generated tool contexts don't exist. Use `0.1.0-preview.14.26573.4` until a later version includes the generator again. To check a version, confirm that the package contains an `analyzers/dotnet/cs` folder.

## Export methods as tools

Add `[ExportAIFunction]` to a public method to export it as a tool. Add `[Description]` to the method and its parameters to tell the model what the tool does and what each argument means:

```csharp
using System.ComponentModel;
using Microsoft.Maui.AI.Attributes;

public class PlantCatalogService
{
    [Description("Searches the plant catalog by name.")]
    [ExportAIFunction("search_plants")]
    public List<PlantInfo> SearchPlants(
        [Description("Optional text to filter plant names by.")] string? query = null)
    {
        // Search the catalog.
        return [];
    }

    [Description("Removes a plant from the garden.")]
    [ExportAIFunction("remove_plant", ApprovalRequired = true)]
    public string RemovePlant([Description("The plant name.")] string name)
    {
        // Remove the plant.
        return $"Removed {name}.";
    }
}
```

The tool name defaults to the method name. Pass a name to the attribute, as in `[ExportAIFunction("search_plants")]`, to set a different name. Set `ApprovalRequired = true` to [require approval](#require-approval) before the tool runs.

Methods can be instance or static, and can be synchronous or return `Task`, `Task<T>`, `ValueTask`, or `ValueTask<T>`. Parameters with C# default values are optional in the tool's JSON schema. Generic methods and methods with `ref`, `out`, or `in` parameters aren't supported and produce error `MAUIAI004`.

Instance methods run on an instance of their type that's resolved from dependency injection, so register that type in your service collection. Static methods are called directly. A static method that doesn't use [service parameters](#bind-parameters-from-services) doesn't need a service provider at all.

## Export properties as tools

Add `[ExportAIFunction]` to a property's `get` or `set` accessor to let the model read or change app state, such as a view mode or preference:

```csharp
public class GardenSettings
{
    public string ViewMode
    {
        [ExportAIFunction("get_view_mode")]
        [Description("Gets the current view mode.")]
        get;

        [ExportAIFunction("set_view_mode")]
        [Description("Sets the view mode to 'list' or 'grid'.")]
        set;
    } = "list";
}
```

A getter becomes a tool with no arguments. A setter becomes a tool with a required `value` argument.

## Group tools into a context

Create a `partial` class that derives from `AIToolContext`, and add an `[AIToolSource]` attribute for each type that contributes tools:

```csharp
using Microsoft.Maui.AI.Attributes;

[AIToolSource(typeof(PlantCatalogService))]
[AIToolSource(typeof(GardenSettings))]
public partial class GardenTools : AIToolContext
{
}
```

The source generator adds a static `Default` instance to the class and implements its `Tools` property. `Tools` returns the same cached list on every call:

```csharp
IReadOnlyList<AITool> tools = GardenTools.Default.Tools;
```

To include only some of a type's tools, set `IncludeTools` to the member names. To leave out some tools, set `ExcludeTools`:

```csharp
[AIToolSource(typeof(PlantCatalogService), IncludeTools = [nameof(PlantCatalogService.SearchPlants)])]
public partial class ReadOnlyGardenTools : AIToolContext
{
}
```

If `IncludeTools` lists a name that isn't an exported member, the generator reports warning `MAUIAI006`. If a source type has no exported members, it reports warning `MAUIAI003`.

Use separate contexts to give different features, or different conversations, different sets of tools.

### Use the assembly-wide context

If you don't need to choose which tools to offer, use the context that the generator creates for your whole project. It contains every exported method in the project. Its name is the assembly name with dots removed, followed by `ToolContext`. For example, for an assembly named `GardenApp`:

```csharp
IReadOnlyList<AITool> tools = GardenAppToolContext.Default.Tools;
```

The assembly-wide context is `internal`, so it's available only within its project. It doesn't include tools exported from property accessors. To offer those tools, add their types to a context that you declare.

## Bind parameters from services

By default, each parameter becomes an argument in the tool's JSON schema, and the model supplies its value. To resolve a parameter from dependency injection instead, add `[FromServices]` or `[FromKeyedServices]`. These parameters aren't part of the schema, so the model doesn't see them:

```csharp
using Microsoft.Extensions.DependencyInjection;

public static class WateringTools
{
    [Description("Gets watering advice for a plant.")]
    [ExportAIFunction("get_watering_advice")]
    public static string GetWateringAdvice(
        [Description("The plant name.")] string plant,
        [FromServices] IWeatherService weather,
        CancellationToken cancellationToken)
    {
        // Use the weather service to build advice.
        return $"Water {plant} in the morning.";
    }
}
```

Parameters of these types are also supplied automatically and left out of the schema:

| Parameter | Value |
|-----------|-------|
| `CancellationToken` | The cancellation token for the tool call. |
| `IServiceProvider` | The service provider for the tool call. |
| `AIFunctionArguments` | All arguments for the tool call. |
| `[FromServices] T` | The `T` service, which must be registered. |
| `[FromKeyedServices(key)] T` | The keyed `T` service, which must be registered. |

Interface and abstract parameters that don't have `[FromServices]` or `[FromKeyedServices]` are treated as JSON arguments. If a parameter type can't be represented in JSON, such as a delegate, the generator reports warning `MAUIAI002`.

The generated tools read services from the `AIFunctionArguments.Services` property when they run. They don't create a service scope. If a tool needs services and none are available, it throws an `InvalidOperationException`.

## Use the tools with a chat client

Register the types that provide instance tools and services, and give the chat client a service provider for tool calls. The way to do this depends on the client:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

builder.Services.AddSingleton<PlantCatalogService>();
builder.Services.AddSingleton<GardenSettings>();
builder.Services.AddSingleton<IWeatherService, WeatherService>();

#if IOS || MACCATALYST
if (OperatingSystem.IsIOSVersionAtLeast(26) || OperatingSystem.IsMacCatalystVersionAtLeast(26))
{
    builder.Services.AddChatClient(services =>
        new AppleIntelligenceChatClient(functionInvocationServices: services));
}
#elif ANDROID
if (OperatingSystem.IsAndroidVersionAtLeast(26))
{
    builder.Services.AddChatClient(new GeminiNanoChatClient())
        .UseFunctionInvocation();
}
#endif
```

Apple Intelligence runs tools itself, so pass the service provider to the `AppleIntelligenceChatClient` constructor. On Android, and with cloud providers, `UseFunctionInvocation` runs the tools. `AddChatClient` builds the pipeline with the app's service provider, which `UseFunctionInvocation` passes to each tool.

Then pass the tools with each request:

```csharp
var options = new ChatOptions
{
    Tools = [.. ReadOnlyGardenTools.Default.Tools],
};

ChatResponse response = await chatClient.GetResponseAsync("Find basil in the catalog.", options);
```

This example uses `ReadOnlyGardenTools` because Apple Intelligence doesn't support [approval-required tools](#require-approval). On Android, also set `ChatOptions.ModelId` to a model that supports tools, such as `gemini-nano-preview-fast`. The default model rejects requests that include tools. See [Tool calling on Android](chat/android.md#tool-calling).

You can also add tools that you create with `AIFunctionFactory.Create` to the same list. Tool support differs by platform; Windows AI doesn't support tools. See [Tool calling](chat.md#tool-calling).

## Require approval

When `ApprovalRequired` is `true`, the generated tool is wrapped in an `ApprovalRequiredAIFunction`. When the model calls that tool, `UseFunctionInvocation` doesn't run it. Instead, the response contains a `ToolApprovalRequestContent` item. Keep the response in the conversation history, ask the user, and send a `ToolApprovalResponseContent` in the next request. `UseFunctionInvocation` then runs the tool only if the user approved it. In this example, `page` is the current `Page`:

```csharp
List<ChatMessage> messages = [new(ChatRole.User, "Remove the basil from my garden.")];
var options = new ChatOptions { Tools = [.. GardenTools.Default.Tools] };

ChatResponse response = await chatClient.GetResponseAsync(messages, options);
messages.AddMessages(response);

List<ToolApprovalRequestContent> requests = response.Messages
    .SelectMany(m => m.Contents)
    .OfType<ToolApprovalRequestContent>()
    .ToList();

if (requests.Count > 0)
{
    List<AIContent> answers = [];
    foreach (ToolApprovalRequestContent request in requests)
    {
        string toolName = (request.ToolCall as FunctionCallContent)?.Name ?? "this tool";
        bool approved = await page.DisplayAlertAsync("Confirm", $"Allow {toolName}?", "Allow", "Deny");
        answers.Add(request.CreateResponse(approved));
    }

    messages.Add(new ChatMessage(ChatRole.User, answers));
    response = await chatClient.GetResponseAsync(messages, options);
}
```

> [!WARNING]
> `AppleIntelligenceChatClient` runs tools itself and doesn't support approvals. If you pass it a tool that requires approval, the tool runs without asking the user. Don't give approval-required tools to Apple Intelligence. Instead, have the tool itself ask the user to confirm before it makes a change.

## See also

- [Chat client](chat.md)
- [Agent tools, context, and structured output](agents/tools-and-context.md)
- [Use the IChatClient interface](/dotnet/ai/ichatclient)
