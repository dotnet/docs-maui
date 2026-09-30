---
title: Chat on Apple platforms
description: Plan on-device chat with AppleIntelligenceChatClient, including model availability, privacy, and preview image input.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Chat on Apple platforms

`AppleIntelligenceChatClient` adapts Apple's Foundation Models framework to `Microsoft.Extensions.AI.IChatClient` for on-device chat. Chat requires an eligible Apple Intelligence device and an available model, in addition to a supported OS. See [Apple requirements](../requirements-apple.md) and the [chat feature comparison](feature-comparison.md).

## Handle model availability

Check model availability in the app rather than assuming that a supported OS version means chat is ready. Provide an unavailable state and a retry path for users whose model is not yet available. Keep prompt and response handling on device unless your app explicitly offers and explains a separate provider; the Apple implementation does not automatically fall back to a cloud service.

Pass cancellation tokens for interactive requests, and use [streaming responses](../chat.md#streaming-responses) when the UI should display output incrementally.

## Design tasks for the on-device model

Favor focused tasks such as summarizing, extracting, classifying, or refining text. Keep prompts concise and ground them in relevant app data; handle branching and deterministic decisions in app code. Don't rely on the on-device model for basic math, code generation, or rigorous logical reasoning. See Apple's [Foundation Models task guidance](https://developer.apple.com/documentation/foundationmodels/generating-content-and-performing-tasks-with-foundation-models) and [prompting guidance](https://developer.apple.com/documentation/foundationmodels/prompting-an-on-device-foundation-model).

The native model's context window is 4,096 tokens shared by instructions, conversation history, tool declarations, schemas, and output. `MaxOutputTokens` does not increase that total. Bound or summarize conversation history and split larger tasks; the Essentials.AI adapter does not expose native context-size or token-count APIs. A local oversized-English prompt returned an explicit context-window error; surface this failure rather than silently discarding input. The adapter reconstructs a native session from the `ChatMessage` history on each request, so retain the turns you need in the app and trim them deliberately. See [Apple's context-window guidance](https://developer.apple.com/documentation/foundationmodels/managing-the-context-window).

Keep representative prompt and output fixtures, and reevaluate behavior when the OS or model changes. See [Apple's model-update guidance](https://developer.apple.com/documentation/foundationmodels/updating-prompts-for-new-model-versions).

### Check grounded results

Treat schema-valid output as structured text, not as proof that the answer is supported by the input. Validate extracted facts against your source data and allow an explicit unknown or unavailable state instead of trusting the model to fill gaps. Handle malformed structured responses as errors, not successful extractions.

In one small local run on macOS 26.7, six source-grounded reservation extractions (three prompts in streaming and non-streaming modes) and a supplied-history recall succeeded. Across two runs, however, absent-fact probes invented an answer even in schema-valid JSON, in both response modes. Neither JSON validity nor a successful first run establishes repeatable factual grounding. Evaluate both modes and re-run cases with your own fixtures. See the [retained chat grounding tests](https://github.com/dotnet/maui-labs/blob/d3b6be534a4a09e7804c8762a1d3f52f73a6ff9b/tests/AI/Microsoft.Maui.Essentials.AI.DeviceTests/Tests/MaciOS/AppleIntelligenceChatClientGroundingTests.cs).

An earlier adapter build also produced malformed streamed JSON because its managed chunker inserted primitive syntax inside an open string. This identified adapter defect is being fixed; it isn't evidence that Apple's native guided generation can't produce valid JSON. See the [retained pre-fix results](https://github.com/dotnet/maui-labs/blob/d3b6be534a4a09e7804c8762a1d3f52f73a6ff9b/tests/AI/AppleChatEvaluation/results-before-stream-fix.json).

## Tools and structured responses

Use `AIFunction` tools for actions the model can call; require user approval *inside the tool implementation* before executing side effects. The native adapter executes supported tools and can return `FunctionCallContent` for information; don't assume function-invocation middleware intercepts every execution. For structured output, use a JSON schema, for example via `GetResponseAsync<T>()`, rather than relying on schema-free JSON formatting. See [Tool calling](../chat.md#tool-calling) and [Structured JSON output](../chat.md#structured-json-output).

## Image input in development

> [!IMPORTANT]
> Apple's [image attachment API](https://developer.apple.com/documentation/foundationmodels/attachment) is available in OS 27. Image input through `Microsoft.Maui.Essentials.AI` is being developed in the [draft adapter change in dotnet/maui-labs#405](https://github.com/dotnet/maui-labs/pull/405). It isn't a released capability of the package and hasn't been validated here with live image inference. Building that proposed change requires opt-in Apple 27 target frameworks and Xcode 27. Apple's released OS API, the draft adapter, and published NuGet support are distinct.

The draft change proposes image *input* through `DataContent` (`image/*`) or local-file `UriContent`, including images in message history. It does not add image generation or a cloud fallback. Don't use the draft API with the published package or treat remote image URLs as supported inputs. The adapter also accepts native `CGImage`, `UIImage`, or `NSImage` through `RawRepresentation` for apps that already hold a platform image; portable encoded bytes should still be retained for persistence.

The following sample is **for a build from the draft PR**, with Apple 27 target frameworks and Xcode 27. Check OS and model vision availability in the app before sending image input; the example isn't supported by the released package:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

if (!OperatingSystem.IsIOSVersionAtLeast(27) &&
    !OperatingSystem.IsMacCatalystVersionAtLeast(27) &&
    !OperatingSystem.IsMacOSVersionAtLeast(27))
{
    throw new PlatformNotSupportedException("Image input requires Apple 27 or later.");
}

IChatClient chatClient = new AppleIntelligenceChatClient();
string imagePath = "/path/to/local-image.png";
byte[] pngBytes = await File.ReadAllBytesAsync(imagePath);

var message = new ChatMessage(ChatRole.User,
[
    new TextContent("Describe the objects visible in this image."),
    new DataContent(pngBytes, "image/png"),
]);
var response = await chatClient.GetResponseAsync([message]);
Console.WriteLine(response.Text);
```

For image questions, ask for a specific extraction or classification, provide a schema when structured data helps, and consider cropping to the relevant region. These are [Apple's native multimodal prompting recommendations](https://developer.apple.com/documentation/foundationmodels/analyzing-images-with-multimodal-prompting), not additional APIs in the Essentials.AI adapter. The draft's model-free image conversion tests do not establish live image recognition quality.

## See also

- [Chat client](../chat.md)
- [Chat feature comparison](feature-comparison.md)
- [Apple Intelligence device requirements](https://support.apple.com/en-us/121115)
