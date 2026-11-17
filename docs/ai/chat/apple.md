---
title: Chat on Apple platforms
description: Plan on-device chat with AppleIntelligenceChatClient, including model availability, prompt design, tools, and image input on Apple platforms.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Chat on Apple platforms

Use `AppleIntelligenceChatClient` to add on-device features such as summarizing notes, extracting details from text, and answering questions about content in your app. It implements `IChatClient` using Apple's Foundation Models framework.

This article explains how to design these features for Apple's on-device model. For the basic API, see [Chat client](../chat.md). To compare platforms, see the [chat feature comparison](feature-comparison.md).

## Handle model availability

Chat requires a supported device with Apple Intelligence turned on and its model ready. A supported OS version alone isn't enough. Check the status before you enable chat, and explain to users when the feature is unavailable:

```csharp
ChatModelStatus status = await AppleIntelligenceChatClient.GetModelStatusAsync();
if (status.Availability != ChatModelAvailability.Available)
{
    // Apple Intelligence is unsupported, turned off, or still preparing its model.
}

bool supportsImages = status.Capabilities?.Contains(ChatModelCapability.ImageInput) == true;
```

The OS manages the model, so there's no API to download it. Requests run on device and don't switch to a cloud service when the model is unavailable. If your app offers another provider, make that choice and its data handling clear to users.

For interactive features, pass cancellation tokens so users can cancel a request, and use [streaming responses](../chat.md#streaming-responses) to display text as it arrives.

## Design tasks for the on-device model

Give the model one focused task and the information it needs to complete it. For example, ask it to extract a reservation number from a confirmation message rather than plan an entire trip in one request. Summarization, classification, text refinement, and creative dialogue are also good starting points.

Keep instructions concise. Supply relevant app content instead of asking the model to guess, and use app code for calculations, branching, and rules that must produce an exact result. Apple's on-device model isn't intended for general-purpose code generation or complex logical reasoning. See Apple's [task guidance](https://developer.apple.com/documentation/foundationmodels/generating-content-and-performing-tasks-with-foundation-models) and [prompting guidance](https://developer.apple.com/documentation/foundationmodels/prompting-an-on-device-foundation-model).

### Manage conversation history

The model's 4,096-token context window includes instructions, message history, tool definitions, schemas, and the response. `MaxOutputTokens` limits the output; it doesn't increase the context window.

Each request creates a native session from the messages you supply, so your app owns the conversation history. Keep only the history needed for the current task. Summarize older turns or split large tasks into smaller requests. If a request exceeds the context window, explain the failure and let the user shorten or restart it. See [Apple's context-window guidance](https://developer.apple.com/documentation/foundationmodels/managing-the-context-window).

### Check grounded results

Structured output makes a response easier to consume, but doesn't make its values correct. For example, a response can match a reservation schema while containing a departure date that isn't in the source message.

Validate important fields against your app's data before you use them. Include an unknown state in your response type so missing information doesn't force a guessed value. Ask users to review generated content before it affects a booking, payment, or other consequential action.

Test representative inputs, missing information, and ambiguous requests, and test again when the OS or model changes. See [Apple's model-update guidance](https://developer.apple.com/documentation/foundationmodels/updating-prompts-for-new-model-versions).

## Tools and structured responses

Expose app functions as `AIFunction` tools when the model needs to look up data or perform an action. Apple Intelligence runs these tools itself, so you don't need `UseFunctionInvocation`. Put permission checks and user approval inside the tool implementation, especially for actions that change data or contact another person.

On version 26, `ChatToolMode.RequireAny` and `ChatToolMode.RequireSpecific` throw an exception. On version 27 or later, they're supported: the model must call a tool first, and can then finish with text.

To return a typed result, use `GetResponseAsync<T>` or set a JSON schema in `ChatOptions.ResponseFormat`. Schemas can use enums, constants, numeric bounds, array bounds, and simple patterns. References and dictionaries aren't supported. See [Tool calling](../chat.md#tool-calling) and [Structured output](../chat.md#structured-output).

## Token usage

On version 27 or later, `ChatResponse.Usage` reports input, output, total, cached-input, and reasoning token counts. Streaming responses end with a `UsageContent` update. On version 26, usage is `null`.

## Image input on Apple 27+

> [!IMPORTANT]
> Image input requires iOS, macOS, or Mac Catalyst 27 or later and a vision-capable Apple Intelligence model. Build with Apple 27 target frameworks and Xcode 27. On version 26, image requests throw an exception.

Attach encoded image bytes with `DataContent` and an `image/*` media type, or refer to a local image file with `UriContent`. Remote image URLs aren't supported. The client accepts images as input; it doesn't generate images.

```csharp
byte[] pngBytes = await File.ReadAllBytesAsync(imagePath);

var message = new ChatMessage(ChatRole.User,
[
    new TextContent("Describe the objects visible in this image."),
    new DataContent(pngBytes, "image/png"),
]);
var response = await chatClient.GetResponseAsync([message]);
Console.WriteLine(response.Text);
```

If your app already has a native image, you can set `RawRepresentation` to a `CGImage`, `UIImage`, or `NSImage`. Keep the encoded bytes too, so you can save the conversation or send it to another provider. Image orientation is preserved, and images can be included in later conversation history.

Ask specific questions, such as extracting a label or classifying an object, rather than requesting an unrestricted description. Crop to the relevant region when it helps focus the task, and test with images that represent your users' devices and content. See [Apple's multimodal prompting recommendations](https://developer.apple.com/documentation/foundationmodels/analyzing-images-with-multimodal-prompting).

## See also

- [Chat client](../chat.md)
- [Chat feature comparison](feature-comparison.md)
- [Requirements](../requirements.md#apple-platforms)
- [Apple Intelligence device requirements](https://support.apple.com/en-us/121115)
