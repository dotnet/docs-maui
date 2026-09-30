---
title: Microsoft.Maui.Essentials.AI overview
description: Learn how Microsoft.Maui.Essentials.AI brings on-device AI to .NET MAUI apps through standard Microsoft.Extensions.AI interfaces backed by native platform AI frameworks.
ms.date: 09/30/2026
---

# Microsoft.Maui.Essentials.AI overview

`Microsoft.Maui.Essentials.AI` is a cross-platform library that brings on-device AI capabilities to .NET MAUI applications through seamless integration with [Microsoft.Extensions.AI](/dotnet/ai/ai-extensions). It surfaces native platform AI frameworks behind the standard `IChatClient` and `IEmbeddingGenerator` interfaces, so you can write portable AI code that runs entirely on-device without sending data to external servers.

> [!NOTE]
> `Microsoft.Maui.Essentials.AI` currently provides chat through Apple Intelligence and embeddings through Apple's separate Natural Language framework. Android and Windows implementations are not yet available. See the [feature comparisons](feature-comparison.md) for per-feature support.

> [!IMPORTANT]
> This API is experimental and is subject to change. Using it produces diagnostic warning **MAUIAI0001**. You must suppress this warning to use the library.
>
> To suppress the warning project-wide, add the following to your `.csproj` file:
>
> ```xml
> <PropertyGroup>
>   <NoWarn>$(NoWarn);MAUIAI0001</NoWarn>
> </PropertyGroup>
> ```
>
> To suppress the warning for a specific file or block of code, use a pragma:
>
> ```csharp
> #pragma warning disable MAUIAI0001
> // Your code using Microsoft.Maui.Essentials.AI
> #pragma warning restore MAUIAI0001
> ```

## What is Microsoft.Maui.Essentials.AI?

`Microsoft.Maui.Essentials.AI` is a bridge between the [Microsoft.Extensions.AI abstractions](/dotnet/ai/ai-extensions) and native on-device AI frameworks. Rather than calling platform-specific APIs directly, you interact with well-known interfaces (`IChatClient`, `IEmbeddingGenerator<string, Embedding<float>>`) that the library implements on top of the underlying OS frameworks.

### Currently available (Apple platforms)

- **Chat client** — `AppleIntelligenceChatClient` implements `IChatClient` using Apple's Foundation Models framework (Apple Intelligence). Supports eligible devices on iOS 26.0+, macOS 26.0+, and Mac Catalyst 26.0+. See the [chat feature comparison](chat/feature-comparison.md).
- **Embedding generator** — `NLEmbeddingGenerator` implements `IEmbeddingGenerator<string, Embedding<float>>` using Apple's Natural Language framework, independently of Apple Intelligence. The API and individual models have different availability; see the [embedding feature comparison](embeddings/feature-comparison.md).

### Planned (not yet available)

- **Android** — on-device AI support for Android is planned for a future release.
- **Windows** — on-device AI support for Windows is planned for a future release.

The standard Microsoft.Extensions.AI interfaces let applications choose another `IChatClient` or `IEmbeddingGenerator` provider. Changing an embedding provider or model also requires rebuilding stored vector indexes; different models don't necessarily share a vector space.

## Key benefits

- **Unified API**: Program against `IChatClient` and `IEmbeddingGenerator<string, Embedding<float>>`—the same interfaces used across the .NET AI ecosystem.
- **On-device processing**: AI inference runs locally on the device. No network call is required.
- **Privacy-first**: The Apple implementations process prompts and embeddings on device; an app using other providers must account for their data handling separately.
- **Platform-optimized**: Each platform implementation is accelerated by native hardware-level ML capabilities.
- **Microsoft.Extensions.AI compatible**: Works out-of-the-box with logging middleware, dependency injection, telemetry, and the full breadth of the .NET AI ecosystem.

## What you can build

`Microsoft.Maui.Essentials.AI` provides the primitives for a wide range of on-device AI scenarios:

- **Chat assistants with tool calling** — Build conversational experiences that call application functions using the standard `IChatClient` [tool-calling API](/dotnet/ai/ichatclient#tool-calling).
- **Semantic search with on-device text embeddings** — Use `IEmbeddingGenerator` to embed strings and compare semantic similarity entirely on-device.
- **Multi-agent AI workflows** — Compose multiple `IChatClient` instances using [Microsoft.Extensions.AI middleware pipelines](/dotnet/ai/ai-extensions#middleware).
- **Structured data extraction with JSON schema** — Request structured, strongly-typed responses by specifying a JSON schema in the chat options.

## In this section

| Article | Description |
|---------|-------------|
| [Get started](getting-started.md) | Install the package and register the chat client and embedding generator services. |
| [Chat](chat.md) | Use `IChatClient` for basic chat, streaming, multi-turn conversations, tool calling, and structured output. See the [chat feature comparison](chat/feature-comparison.md) and [Apple chat guidance](chat/apple.md). |
| [Text embeddings](embeddings.md) | Use `IEmbeddingGenerator` to generate on-device text embeddings for semantic search and similarity. See the [embedding feature comparison](embeddings/feature-comparison.md) and [Apple embedding guidance](embeddings/apple.md). |
| [Agent framework integration](agent-framework.md) | Build multi-agent AI workflows with Microsoft.Agents.AI and Microsoft.Maui.Essentials.AI. |
| [Feature comparison](feature-comparison.md) | Navigate to feature-specific support and capabilities. |
| [Requirements](requirements-apple.md) | Apple OS and model prerequisites for chat and text embeddings. |

## See also

- [Microsoft.Extensions.AI overview](/dotnet/ai/ai-extensions)
- [Use the IChatClient interface](/dotnet/ai/ichatclient)
- [Use the IEmbeddingGenerator interface](/dotnet/ai/iembeddinggenerator)
- [IChatClient API reference](/dotnet/api/microsoft.extensions.ai.ichatclient)
- [IEmbeddingGenerator API reference](/dotnet/api/microsoft.extensions.ai.iembeddinggenerator-2)
- [Apple Intelligence device requirements](https://support.apple.com/en-us/121115)
