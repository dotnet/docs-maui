---
title: Chat feature comparison
description: Compare the Microsoft.Extensions.AI chat abstraction with the AppleIntelligenceChatClient implementation.
ms.date: 09/30/2026
ms.topic: concept-article
---

# Chat feature comparison

`Microsoft.Extensions.AI.IChatClient` offers a common chat API across providers. `AppleIntelligenceChatClient` is the `Microsoft.Maui.Essentials.AI` implementation using Apple's Foundation Models framework. The comparison below starts with capabilities expressed by the abstraction, then describes the Apple implementation; other providers may handle the same capabilities differently. This isn't an iOS-versus-macOS capability matrix.

## Abstraction versus Apple implementation

| Capability | `IChatClient` abstraction | Apple `AppleIntelligenceChatClient` |
|------------|---------------------------|------------------------------------|
| Text generation and streaming | Provides response and streaming methods | Supports both on device |
| Conversation history | Accepts message history; session persistence is provider-dependent | Reconstructs a native session from supplied history for each request; the app retains and trims turns |
| System instructions | Can represent system messages | Appends system messages as native instructions |
| Tools | Can represent tools and function calls; execution semantics depend on provider and middleware | Supports `AIFunction` tools only; native adapter executes them |
| Structured output | Can request response formats, subject to provider support | Requires a JSON schema; schema-free `ChatResponseFormat.Json` throws |
| Image input | Can represent image-bearing content; provider support varies | Not in the released package; proposed in the draft Apple 27+ adapter change |

For examples, see [Chat client](../chat.md). For supported input types, options, and availability behavior, see [Chat on Apple platforms](apple.md).

## Chat options

| `ChatOptions` capability | Can be expressed through `IChatClient` | Apple adapter behavior |
|--------------------------|----------------------------------|------------------------|
| `Temperature` | Yes | Mapped to native generation options |
| `TopK` | Yes | Mapped to native sampling options |
| `Seed` | Yes | Used with `TopK` random sampling; not applied to greedy sampling |
| `MaxOutputTokens` | Yes | Mapped when greater than zero; not the total context budget |
| `ResponseFormat` | Yes | JSON schema supported; schema-free `ChatResponseFormat.Json` throws |
| `Tools` | Yes | Supports `AIFunction` tools only |
| `ToolMode` | Yes | `None` suppresses tools; other modes aren't explicitly guaranteed |
| `TopP`, `FrequencyPenalty`, `PresencePenalty`, `StopSequences`, `ModelId` | Yes | Not mapped to native options |

When `TopK` is absent, the native adapter uses greedy sampling. Don't rely on unmapped options to constrain output. See [Chat on Apple platforms](apple.md) for the context budget and tool-execution guidance.

## Message content

`TextContent` and function call/result content are supported. Other unsupported content types fail explicitly. Image input is under development in [dotnet/maui-labs#405](https://github.com/dotnet/maui-labs/pull/405), which is not a released `Microsoft.Maui.Essentials.AI` capability. The draft accepts image `DataContent` and local-file `UriContent` on Apple 27+ with a vision-capable model; HTTP URLs aren't accepted. Do not assume that the currently published package supports images or image generation.

## Platform availability

The current package targets iOS, macOS, and Mac Catalyst. Chat requires version 26 or later on those platforms. OS version alone does not guarantee an available model: the device must support Apple Intelligence and its model must be ready. The package doesn't ship tvOS or visionOS targets, even if Apple's native framework supports other platforms. Android and Windows chat implementations aren't available in `Microsoft.Maui.Essentials.AI`. See [Chat on Apple platforms](apple.md) and [Apple requirements](../requirements-apple.md).

## See also

- [Chat client](../chat.md)
- [Chat on Apple platforms](apple.md)
- [AI feature comparison](../feature-comparison.md)
