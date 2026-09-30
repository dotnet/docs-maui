---
title: Chat on Apple platforms
description: Plan on-device chat with AppleIntelligenceChatClient, including model availability, privacy, and Apple 27 image input.
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

In one small local run on macOS 26.7, six source-grounded reservation extractions (three prompts in streaming and non-streaming modes) and a supplied-history recall succeeded. In the final two-run evaluation with the [streaming correction](https://github.com/dotnet/maui-labs/pull/606), 18/18 structured outputs parsed, and two oversized prompts produced the expected context errors. However, only 16/20 ground-truth assertions passed: two departure fields were wrong, and two missing-fact answers were invented. These small fixtures aren't a general accuracy benchmark. Neither JSON validity nor a successful first run establishes repeatable factual grounding; evaluate both modes with your own data. See the [retained chat grounding tests](https://github.com/dotnet/maui-labs/blob/69e0e28e6219f9f5ac7f100a60e50844d6b45e17/tests/AI/Microsoft.Maui.Essentials.AI.DeviceTests/Tests/MaciOS/AppleIntelligenceChatClientGroundingTests.cs) and [post-fix results](https://github.com/dotnet/maui-labs/blob/69e0e28e6219f9f5ac7f100a60e50844d6b45e17/tests/AI/AppleChatEvaluation/results-after-stream-fix.json).

Use a package version containing that correction; merging the source change doesn't update an already published package. The [pre-fix results](https://github.com/dotnet/maui-labs/blob/69e0e28e6219f9f5ac7f100a60e50844d6b45e17/tests/AI/AppleChatEvaluation/results-before-stream-fix.json) document a managed chunker defect, not a limitation of Apple's native guided generation.

## Tools and structured responses

Use `AIFunction` tools for actions the model can call; require user approval *inside the tool implementation* before executing side effects. The native adapter executes supported tools and can return `FunctionCallContent` for information; don't assume function-invocation middleware intercepts every execution. For structured output, use a JSON schema, for example via `GetResponseAsync<T>()`, rather than relying on schema-free JSON formatting. See [Tool calling](../chat.md#tool-calling) and [Structured JSON output](../chat.md#structured-json-output).

## Image input on Apple 27+

> [!IMPORTANT]
> Image input requires Apple 27 or later, an available vision-capable Apple Intelligence model, and a `Microsoft.Maui.Essentials.AI` package version containing [the image-input adapter](https://github.com/dotnet/maui-labs/pull/405). Building for Apple 27 requires Apple 27 target frameworks and Xcode 27. Apple's [image attachment API](https://developer.apple.com/documentation/foundationmodels/attachment), the adapter source, and a released NuGet package are separate availability milestones. The image conversion tests don't establish live vision inference quality; no live OS 27 vision result was measured here.

The adapter accepts image *input* through `DataContent` (`image/*`) or local-file `UriContent`, including images in message history. It doesn't add image generation or a cloud fallback; remote image URLs aren't supported. Apps that already hold a platform image can also pass a native `CGImage`, `UIImage`, or `NSImage` through `RawRepresentation`. Retain portable encoded bytes for persistence and cross-platform consumers; the adapter preserves image orientation and can round-trip images in conversation history.

The following sample requires a package version with image-input support. Check OS and model vision availability in the app before sending an image:

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

For image questions, ask for a specific extraction or classification, provide a schema when structured data helps, and consider cropping to the relevant region. These are [Apple's native multimodal prompting recommendations](https://developer.apple.com/documentation/foundationmodels/analyzing-images-with-multimodal-prompting), not additional APIs in the Essentials.AI adapter.

## See also

- [Chat client](../chat.md)
- [Chat feature comparison](feature-comparison.md)
- [Apple Intelligence device requirements](https://support.apple.com/en-us/121115)
