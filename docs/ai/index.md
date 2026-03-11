---
title: Microsoft.Maui.Essentials.AI overview
description: Learn how Microsoft.Maui.Essentials.AI brings on-device AI to .NET MAUI apps through standard Microsoft.Extensions.AI interfaces backed by native Apple platform APIs.
ms.date: 03/11/2026
---

# Microsoft.Maui.Essentials.AI overview

`Microsoft.Maui.Essentials.AI` is a cross-platform library that brings on-device AI capabilities to .NET MAUI applications through seamless integration with [Microsoft.Extensions.AI](https://learn.microsoft.com/dotnet/ai/ai-extensions). It surfaces native platform AI frameworks behind the standard `IChatClient` and `IEmbeddingGenerator` interfaces, so you can write portable AI code that runs entirely on-device without sending data to external servers.

> [!NOTE]
> `Microsoft.Maui.Essentials.AI` is designed for all .NET MAUI platforms. **Apple Intelligence** (iOS, macOS, Mac Catalyst, tvOS) is the first implementation available. Android and Windows support are not yet available and will be added in future releases.

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

`Microsoft.Maui.Essentials.AI` is a bridge between the [Microsoft.Extensions.AI abstractions](https://learn.microsoft.com/dotnet/ai/ai-extensions) and native on-device AI frameworks. Rather than calling platform-specific APIs directly, you interact with well-known interfaces (`IChatClient`, `IEmbeddingGenerator<string, Embedding<float>>`) that the library implements on top of the underlying OS frameworks.

### Currently available (Apple platforms)

- **`AppleIntelligenceChatClient`** — wraps Apple's Foundation Models framework (Apple Intelligence) and exposes it as an `IChatClient`. Requires iOS 26.0+, macOS 26.0+, Mac Catalyst 26.0+, or tvOS 26.0+.
- **`NLEmbeddingGenerator`** — wraps Apple's Natural Language framework (`NLEmbedding`) and exposes it as an `IEmbeddingGenerator<string, Embedding<float>>`. Available from iOS 13.0+, macOS 10.15+, Mac Catalyst 13.1+, and tvOS 13.0+.
- **`NLEmbeddingExtensions`** — provides the `AsIEmbeddingGenerator()` extension method on `NLEmbedding` for convenient integration.

### Planned (not yet available)

- **Android** — on-device AI support for Android is planned for a future release.
- **Windows** — on-device AI support for Windows is planned for a future release.

Because all implementations conform to the standard Microsoft.Extensions.AI interfaces, you can substitute any `IChatClient` or `IEmbeddingGenerator` provider without changing your application logic. This makes it straightforward to switch between on-device models or cloud-backed providers.

## Key benefits

- **Unified API**: Program against `IChatClient` and `IEmbeddingGenerator<string, Embedding<float>>`—the same interfaces used across the .NET AI ecosystem.
- **On-device processing**: AI inference runs locally on the device. No network call is required.
- **Privacy-first**: Data never leaves the device and is never sent to external servers.
- **Platform-optimized**: Execution is accelerated by Apple's Neural Engine and hardware-level ML accelerators.
- **Microsoft.Extensions.AI compatible**: Works out-of-the-box with logging middleware, dependency injection, telemetry, and the full breadth of the .NET AI ecosystem.

## What you can build

`Microsoft.Maui.Essentials.AI` provides the primitives for a wide range of on-device AI scenarios:

- **Chat assistants with tool calling** — Build conversational experiences that call application functions using the standard `IChatClient` tool-calling API.
- **Semantic search with on-device text embeddings** — Use `NLEmbeddingGenerator` to embed strings and compare semantic similarity entirely on-device.
- **Multi-agent AI workflows** — Compose multiple `IChatClient` instances using Microsoft.Extensions.AI middleware pipelines.
- **Structured data extraction with JSON schema** — Request structured responses from `AppleIntelligenceChatClient` by specifying a JSON schema in the chat options.

## In this section

| Article | Description |
|---------|-------------|
| [Requirements](requirements.md) | Platform versions, device requirements, and development prerequisites. |
| [Get started](getting-started.md) | Install the package, register services, and write your first AI-powered .NET MAUI app. |
| [Platform APIs](platform-apis.md) | Detailed reference for `AppleIntelligenceChatClient`, `NLEmbeddingGenerator`, and `NLEmbeddingExtensions`. |
| [Agent framework integration](agent-framework.md) | Build multi-agent AI workflows with Microsoft.Agents.AI and Microsoft.Maui.Essentials.AI. |
| [Feature comparison](feature-comparison.md) | Compare supported features and `ChatOptions` across all platforms. |

## See also

- [Microsoft.Extensions.AI overview](https://learn.microsoft.com/dotnet/ai/ai-extensions)
- [IChatClient interface](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.ichatclient)
- [IEmbeddingGenerator interface](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.iembeddinggenerator-2)
- [Apple Intelligence availability](https://support.apple.com/en-us/120898)
