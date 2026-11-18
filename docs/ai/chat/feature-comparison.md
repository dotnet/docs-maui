---
title: Chat feature comparison
description: Compare the AppleIntelligenceChatClient, WindowsAIChatClient, and GeminiNanoChatClient implementations of IChatClient in Microsoft.Maui.Essentials.AI.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Chat feature comparison

`IChatClient` gives your app one API for chat on every platform. Each platform's on-device model determines which capabilities, options, and content are supported. Unsupported features either throw an exception or, for optional settings, are ignored. No client switches to a cloud model.

| Platform | Client | Model |
|----------|--------|-------|
| iOS, Mac Catalyst, and macOS | `AppleIntelligenceChatClient` | Apple Intelligence (Foundation Models) |
| Windows | `WindowsAIChatClient` | Phi Silica (Windows AI `LanguageModel`) |
| Android | `GeminiNanoChatClient` | Gemini Nano (ML Kit GenAI Prompt API) |

For device and project requirements, see [Requirements](../requirements.md).

## Capabilities

| Capability | Apple | Windows | Android |
|------------|-------|---------|---------|
| Text and streaming | Supported | Supported | Supported |
| Conversation history | Supplied with each request | Supplied with each request | Supplied with each request |
| Tool calling | `AIFunction` tools, run by Apple Intelligence | Not supported | `AIFunction` tools on capable models, run by `UseFunctionInvocation` |
| Tool modes | `Auto` and `None`; `RequireAny` and `RequireSpecific` on version 27+ | `Auto` and `None` | `Auto` and `None` |
| Structured output | JSON schema | JSON schema | JSON schema with an object root, on capable models |
| Image input | Version 27+ with a vision-capable model | Not supported | In-memory images |
| Reasoning output | Not supported | Not supported | Full output, on capable models |
| Token usage | Input, output, total, cached, and reasoning counts on version 27+ | Not reported | Input count only |
| Model selection | Single system model | Single system model | Five Gemini Nano model IDs |
| Model readiness | `GetModelStatusAsync` | `GetModelStatusAsync` and `EnsureReadyAsync` | `GetModelStatusAsync`, `PrepareModelAsync`, and `DownloadModelAsync` |

Each client's `GetModelStatusAsync` method reports the capabilities that the current model supports. `ConversationId`, `ContinuationToken`, and `AllowBackgroundResponses` aren't supported and throw an exception.

On Windows, a structured response can arrive as one final streaming update. On Android, when a request includes tools, text arrives at the end of each model turn rather than incrementally.

## Chat options

| `ChatOptions` property | Apple | Windows | Android |
|------------------------|-------|---------|---------|
| `Temperature` | Supported | Supported | Supported |
| `TopK` | Supported | Supported | Supported |
| `TopP` | Supported; `TopK` takes precedence | Supported | Ignored |
| `Seed` | Supported with `TopK` or `TopP` | Ignored | Supported |
| `MaxOutputTokens` | Supported | Ignored | Supported; set it for longer responses |
| `ResponseFormat` | JSON schema only | JSON schema only | JSON schema only |
| `Tools` and `ToolMode` | See [Capabilities](#capabilities) | `None` and `Auto`; no tools are registered | See [Capabilities](#capabilities) |
| `Reasoning` | Ignored | Ignored | `Output = ReasoningOutput.Full` |
| `ModelId` | Ignored | Ignored | Selects a model |

Without `TopK` or `TopP`, Apple Intelligence uses greedy sampling. `FrequencyPenalty`, `PresencePenalty`, and `StopSequences` are ignored on every platform.

## Message content

| Content | Apple | Windows | Android |
|---------|-------|---------|---------|
| `TextContent` | Supported | Supported | Supported |
| `FunctionCallContent` and `FunctionResultContent` | Supported | Supported in history | Supported |
| `DataContent` with an image | Version 27+ | Not supported | Supported |
| `UriContent` with an image | Local files on version 27+ | Not supported | Not supported |

Unsupported standard content, such as audio, PDF files, or remote image URLs, throws `NotSupportedException`. Custom `AIContent` types are skipped.

## See also

- [Chat client](../chat.md)
- [Chat on Apple platforms](apple.md)
- [Chat on Android](android.md)
- [Requirements](../requirements.md)
