---
title: Chat on Apple platforms
description: Plan on-device chat with AppleIntelligenceChatClient, including model availability, privacy, and Apple 27 image input.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Chat on Apple platforms

Use `AppleIntelligenceChatClient` to add on-device features such as summarizing notes, extracting details from text, and answering questions about content in your app. It implements `Microsoft.Extensions.AI.IChatClient` using Apple's Foundation Models framework.

This article explains how to design these features for Apple's on-device model. For basic API examples, see [Chat client](../chat.md). For supported options and content types, see [Chat feature comparison](feature-comparison.md).

## Handle model availability

Chat requires a supported device with Apple Intelligence enabled and its model ready to use. A supported OS version alone isn't enough. Check availability before enabling chat, and explain to users when the feature is unavailable. See [Apple requirements](../requirements-apple.md).

Requests run on device. The client doesn't switch to a cloud service when the local model is unavailable. If your app offers another provider, make that choice and its data handling clear to users.

For interactive features, pass cancellation tokens so users can cancel a request, and use [streaming responses](../chat.md#streaming-responses) to display text as it arrives.

## Design tasks for the on-device model

Give the model one focused task and the information it needs to complete it. For example, ask it to extract a reservation number from a confirmation message rather than plan an entire trip in one request. Summarization, classification, text refinement, and creative dialogue are also suitable starting points.

Keep instructions concise. Supply relevant app content instead of asking the model to guess, and use app code for calculations, branching, and rules that must produce an exact result. Apple's on-device model isn't intended for general-purpose code generation or complex logical reasoning. See Apple's [task guidance](https://developer.apple.com/documentation/foundationmodels/generating-content-and-performing-tasks-with-foundation-models) and [prompting guidance](https://developer.apple.com/documentation/foundationmodels/prompting-an-on-device-foundation-model).

### Manage conversation history

The model's 4,096-token context window includes instructions, message history, tool definitions, schemas, and the response. `MaxOutputTokens` limits the output; it doesn't increase the total context window.

Keep only the history needed for the current task. Summarize older turns or split large tasks into smaller requests rather than sending an ever-growing conversation. If a request exceeds the context window, explain the failure and let the user shorten or restart it.

Each request creates a native session from the messages you supply. Your app owns conversation history and must include the turns it wants the model to remember. See [Apple's context-window guidance](https://developer.apple.com/documentation/foundationmodels/managing-the-context-window).

### Check grounded results

Structured output makes a response easier to consume, but doesn't establish that its values are correct. For example, a response can match a reservation schema while containing a departure date that isn't in the source message.

Validate important fields against your app's data before using them. Include an unknown state in your response design so missing information doesn't require a guessed value. Ask users to review generated content before it affects a booking, payment, or other consequential action.

Try representative inputs, missing information, and ambiguous requests in both streaming and non-streaming workflows. Recheck these scenarios when the OS or model changes. See [Apple's model-update guidance](https://developer.apple.com/documentation/foundationmodels/updating-prompts-for-new-model-versions).

## Tools and structured responses

Expose app functions as `AIFunction` tools when the model needs to look up data or perform an action. The Apple client executes these tools through the native framework. Put permission checks and user approval inside the tool implementation, especially for actions that change data or contact another person; don't rely on middleware to intercept every call.

To return a typed result, use `GetResponseAsync<T>()` or specify a JSON schema in `ChatOptions`. Schema-free `ChatResponseFormat.Json` isn't supported. See [Tool calling](../chat.md#tool-calling) and [Structured JSON output](../chat.md#structured-json-output).

## Image input on Apple 27+

> [!IMPORTANT]
> Image input requires iOS, macOS, or Mac Catalyst 27 or later and an available vision-capable Apple Intelligence model. Use a package version with image-input support and build with Apple 27 target frameworks and Xcode 27.

Attach encoded image bytes with `DataContent` and an `image/*` media type, or refer to a local image with `UriContent`. Remote image URLs aren't supported. The client accepts images as input; it doesn't generate images.

The following example asks about a local image. Check OS and model availability before sending the request:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

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

If your app already has a native image, you can set `RawRepresentation` to a `CGImage`, `UIImage`, or `NSImage`. Keep encoded bytes available for saving the conversation or passing its content to another provider. Image orientation is preserved, and images can be included in subsequent conversation history.

Ask specific questions, such as extracting a label or classifying an object, rather than requesting an unrestricted description. Crop to the relevant region when it helps focus the task, and use a schema when your app needs structured results. Check the responses with images representative of your users' devices and content. See [Apple's multimodal prompting recommendations](https://developer.apple.com/documentation/foundationmodels/analyzing-images-with-multimodal-prompting).

## See also

- [Chat client](../chat.md)
- [Chat feature comparison](feature-comparison.md)
- [Apple Intelligence device requirements](https://support.apple.com/en-us/121115)
