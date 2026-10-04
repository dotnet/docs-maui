---
title: Chat feature comparison
description: Compare the Microsoft.Extensions.AI chat abstraction with the AppleIntelligenceChatClient implementation.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Chat feature comparison

When you use `IChatClient`, your app has a common API for sending messages, streaming responses, and calling tools. The provider determines which capabilities and options are supported. The following tables show how `AppleIntelligenceChatClient` implements that API using Apple's on-device Foundation Models framework.

## Abstraction versus Apple implementation

| Capability | `IChatClient` | `AppleIntelligenceChatClient` |
|------------|---------------------------|------------------------------------|
| Text generation and streaming | Provides response and streaming methods | Supports both on device |
| Conversation history | Accepts a sequence of messages | Uses the history supplied with each request; your app retains the conversation |
| System instructions | Accepts system messages | Uses them as native instructions |
| Tools | Accepts tool definitions and represents function calls | Supports `AIFunction` tools, executed by the native framework |
| Structured output | Accepts a response format | Requires a JSON schema |
| Image input | Accepts image content | Supported on Apple 27+ with an available vision-capable model |

For examples, see [Chat client](../chat.md). For supported input types, options, and availability behavior, see [Chat on Apple platforms](apple.md).

## Chat options

The following `ChatOptions` properties are available through the abstraction. Their effect depends on the provider:

| `ChatOptions` property | Apple implementation |
|------------------------|----------------------|
| `Temperature` | Supported |
| `TopK` | Supported |
| `Seed` | Used with `TopK` random sampling |
| `MaxOutputTokens` | Limits output when greater than zero; doesn't increase the context window |
| `ResponseFormat` | JSON schema supported; schema-free `ChatResponseFormat.Json` isn't supported |
| `Tools` | Supports `AIFunction` tools |
| `ToolMode` | `None` disables tools; required-tool modes aren't enforced |
| `TopP`, `FrequencyPenalty`, `PresencePenalty`, `StopSequences`, `ModelId` | Not supported |

Without `TopK`, the client uses greedy sampling and doesn't apply `Seed`. Unsupported options aren't applied to generation. See [Chat on Apple platforms](apple.md) for guidance on prompts, conversation history, and tool execution.

## Message content

The client supports `TextContent`, `FunctionCallContent`, and `FunctionResultContent`. For images on Apple 27+, use `DataContent` with an image media type or `UriContent` pointing to a local file. Remote image URLs and other content types aren't supported. See the [image-input example](apple.md#image-input-on-apple-27).

## Platform availability

Chat is available on supported iOS, macOS, and Mac Catalyst devices running version 26 or later. Image input requires version 27 or later and a vision-capable model. The device must have Apple Intelligence enabled and the model ready to use.

The package doesn't provide chat implementations for Android, Windows, tvOS, or visionOS. See [Apple requirements](../requirements-apple.md) for setup.

## See also

- [Chat client](../chat.md)
- [Chat on Apple platforms](apple.md)
- [AI feature comparison](../feature-comparison.md)
