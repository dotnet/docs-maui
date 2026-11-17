---
title: Chat on Android
description: Use GeminiNanoChatClient for on-device chat with Gemini Nano on Android, including model download, model selection, tool calling, and structured output.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Chat on Android

`GeminiNanoChatClient` implements `IChatClient` using Gemini Nano, through Google's ML Kit GenAI Prompt API and the device's AICore service. Requests run on device. The client never downloads a model during a request and never falls back to a cloud service.

This article covers Android-specific behavior. For the basic API, see [Chat client](../chat.md). To compare platforms, see the [chat feature comparison](feature-comparison.md). For device and project requirements, see [Requirements](../requirements.md#android).

## Check and download the model

AICore manages Gemini Nano, and the model might not be on the device yet. Android API 26 alone doesn't mean the model is available. Check the status before you enable chat:

```csharp
using Microsoft.Maui.Essentials.AI;

ChatModelStatus status = await GeminiNanoChatClient.GetModelStatusAsync(cancellationToken);
```

| `status.Availability` | Meaning |
|-----------------------|---------|
| `Available` | The model is ready. |
| `NotReady` | The model can be downloaded. |
| `Downloading` | A download is in progress. |
| `Unavailable` | The device or AICore doesn't support the model. |
| `Unknown` | AICore reported an unrecognized status. |

Status checks and chat requests never start a download. Download the model only after your app or the user decides to. `PrepareModelAsync` waits for the download to finish:

```csharp
if (status.Availability == ChatModelAvailability.NotReady)
{
    await GeminiNanoChatClient.PrepareModelAsync(cancellationToken);
    status = await GeminiNanoChatClient.GetModelStatusAsync(cancellationToken);
}
```

To show progress, enumerate `DownloadModelAsync` instead:

```csharp
await foreach (var update in GeminiNanoChatClient.DownloadModelAsync(cancellationToken))
{
    Console.WriteLine($"{update.State}: {update.BytesDownloaded} of {update.BytesToDownload}");
}

status = await GeminiNanoChatClient.GetModelStatusAsync(cancellationToken);
```

Check the status again after a download. A completed download doesn't guarantee the model is ready. Canceling stops your app's request, but AICore might continue a download that it shares with other apps.

## Choose a model

Set `ChatOptions.ModelId` to select a Gemini Nano model for a request. Pass the same ID to `GetModelStatusAsync`, `PrepareModelAsync`, and `DownloadModelAsync`.

| Model ID | Release stage | Preference |
|----------|---------------|------------|
| `gemini-nano` or `null` (default) | Stable | Full |
| `gemini-nano-full` | Stable | Full |
| `gemini-nano-fast` | Stable | Fast |
| `gemini-nano-preview-full` | Preview | Full |
| `gemini-nano-preview-fast` | Preview | Fast |

AICore resolves each ID to a model on the device. For example, the default stable model is currently Nano V3. Responses report the resolved model name, such as `nano-v4-fast`, rather than the ID you requested. Preview models depend on the device and on enrollment in the AICore Developer Preview. Unknown IDs throw an exception.

Capabilities depend on the resolved model. Check `status.Capabilities` for `ChatModelCapability.ToolCalling`, `ChatModelCapability.StructuredOutput`, and `ChatModelCapability.Reasoning` before you use those features:

```csharp
var status = await GeminiNanoChatClient.GetModelStatusAsync("gemini-nano-preview-fast", cancellationToken);
bool supportsTools = status.Capabilities?.Contains(ChatModelCapability.ToolCalling) == true;
```

## Set the output length

`MaxOutputTokens` has no default. When it isn't set, the default Nano V3 model stops after about 40 tokens. Set `MaxOutputTokens` for anything longer than a short answer:

```csharp
var options = new ChatOptions { MaxOutputTokens = 512 };
var response = await chatClient.GetResponseAsync("Summarize this article: ...", options);
```

A response that reaches the limit ends with `ChatFinishReason.Length`.

The client also maps `Temperature` (0 through 1), `TopK`, and `Seed`. To stream the model's reasoning on a model that reports `ChatModelCapability.Reasoning`, set `Reasoning = new() { Output = ReasoningOutput.Full }`. Reasoning effort levels and summaries aren't supported.

## Tool calling

Tools require a model that reports `ChatModelCapability.ToolCalling`, such as `gemini-nano-preview-fast`. The default Nano V3 model doesn't support tools, and a request with tools fails on it.

The client returns the model's tool calls rather than running them. Add `UseFunctionInvocation()` to run the calls and send the results back to the model:

```csharp
using System.ComponentModel;
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

IChatClient chatClient = new GeminiNanoChatClient()
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();

var options = new ChatOptions
{
    ModelId = "gemini-nano-preview-fast",
    MaxOutputTokens = 256,
    Tools = [AIFunctionFactory.Create(GetWeather)],
};

var response = await chatClient.GetResponseAsync("What's the weather in Paris?", options);
Console.WriteLine(response.Text);

[Description("Gets the current weather for a city.")]
static string GetWeather([Description("The city name.")] string city) => $"Sunny, 22°C in {city}";
```

`ChatToolMode.Auto` and `ChatToolMode.None` are supported. `ChatToolMode.RequireAny` and `ChatToolMode.RequireSpecific` aren't supported and throw an exception. Tool parameter schemas can't use `$ref`, `const`, or dictionary types.

When a streaming request includes tools and no response schema, text arrives at the end of each model turn rather than incrementally, so raw function-call tokens never reach your UI. With a response schema, the final JSON still streams incrementally. Reasoning streams in both cases.

## Structured output

Structured output requires a model that reports `ChatModelCapability.StructuredOutput`. Use `GetResponseAsync<T>`, or set `ChatOptions.ResponseFormat` to `ChatResponseFormat.ForJsonSchema<T>()`. AICore constrains generation to the schema. For trimmed and Native AOT apps, pass source-generated `JsonSerializerOptions`:

```csharp
using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;

var response = await chatClient.GetResponseAsync<WeatherReport>(
    "Return location Seattle, temperature 72, and condition sunny.",
    AppJsonContext.Default.Options,
    new ChatOptions { MaxOutputTokens = 256 });

Console.WriteLine(response.Result.Location);

public sealed record WeatherReport(string Location, int Temperature, string Condition);

[JsonSerializable(typeof(WeatherReport))]
internal partial class AppJsonContext : JsonSerializerContext;
```

Keep the following behavior in mind:

- The schema root must be an object. `GetResponseAsync<T>` automatically wraps other types, such as arrays.
- The client returns the generated JSON unchanged and doesn't validate it. If generation reaches `MaxOutputTokens`, the JSON is incomplete, so set a large enough limit.
- Schema-free `ChatResponseFormat.Json` and `GetResponseAsync<T>` with `useJsonSchemaResponseFormat: false` throw `NotSupportedException`.
- Structured output works with streaming, tools, and reasoning. With tools, the model first decides whether to call a tool, and then generates its final answer under the schema. This adds one extra inference to the final turn.

The client supports these JSON Schema keywords and rejects others, such as `anyOf`, `const`, `minLength`, and `maxLength`, rather than ignoring them:

| Type | Supported keywords |
|------|--------------------|
| Any | `type` (one type, optionally with `null`), `description`, `title`, local `$ref`, `$defs` |
| `object` | `properties`, `required`, `additionalProperties` (Boolean only) |
| `array` | `items`, `minItems`, `maxItems` |
| `string` | `enum`, `format`, `pattern` |
| `integer`, `number` | `minimum`, `maximum` |

AICore enforces types, `enum`, `required`, `minimum`, `maximum`, `minItems`, and `maxItems` during generation. It doesn't enforce `pattern` or `format`.

## Image input

Add images to `User` or `Assistant` messages as `DataContent` with an `image/*` media type:

```csharp
byte[] pngBytes = await File.ReadAllBytesAsync(imagePath);

var message = new ChatMessage(ChatRole.User,
[
    new TextContent("Describe the objects in this image."),
    new DataContent(pngBytes, "image/png"),
]);
var response = await chatClient.GetResponseAsync([message], new ChatOptions { MaxOutputTokens = 256 });
```

Only in-memory images are supported. `UriContent` throws `NotSupportedException`, and empty or undecodable image data throws `ArgumentException`.

## Token usage

Responses report input tokens in `ChatResponse.Usage.InputTokenCount`. Output and total token counts aren't available and are `null`.

## See also

- [Chat client](../chat.md)
- [Chat feature comparison](feature-comparison.md)
- [Requirements](../requirements.md#android)
- [Gemini Nano with ML Kit GenAI Prompt API](https://developers.google.com/ml-kit/genai/prompt/android)
