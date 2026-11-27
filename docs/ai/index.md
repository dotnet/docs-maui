---
title: Microsoft.Maui.Essentials.AI overview
description: Learn how Microsoft.Maui.Essentials.AI brings on-device chat and text embeddings to .NET MAUI apps through Microsoft.Extensions.AI, using Apple Intelligence, Windows AI, and Gemini Nano.
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# Microsoft.Maui.Essentials.AI overview

`Microsoft.Maui.Essentials.AI` brings on-device AI to .NET MAUI apps through [Microsoft.Extensions.AI](/dotnet/ai/microsoft-extensions-ai). It implements the standard `IChatClient` and `IEmbeddingGenerator<string, Embedding<float>>` interfaces on top of each platform's built-in models, so you write portable AI code that runs entirely on the device. Prompts and responses aren't sent to a cloud service.

> [!IMPORTANT]
> `Microsoft.Maui.Essentials.AI` is experimental and subject to change. Using it produces diagnostic warning `MAUIAI0001`, which you must [suppress](getting-started.md#suppress-the-experimental-warning).

## Platform support

| Platform | Chat (`IChatClient`) | Image input | Text embeddings (`IEmbeddingGenerator`) |
|----------|----------------------|-------------|-----------------------------------------|
| iOS and Mac Catalyst 26+ | Apple Intelligence, with tools and structured output | Version 27+ | Apple Natural Language |
| Windows 11 24H2+ | Windows AI on Copilot+ PCs, with structured output | Not supported | Not supported |
| Android API 26+ | Gemini Nano on supported devices, with structured output and tools on capable models | Supported | Not supported |

Each platform also has hardware and model requirements. See [Requirements](requirements.md).

Because the library uses the standard Microsoft.Extensions.AI interfaces, the same code works with middleware such as logging, telemetry, and function invocation, and with libraries built on those interfaces, such as the [Microsoft Agent Framework](agents/index.md). You can also substitute another `IChatClient` or `IEmbeddingGenerator` provider. If your app falls back to a cloud provider, make that choice and its data handling clear to users.

## Next steps

- [Get started](getting-started.md): Install the package, register services, and check model readiness.
- [Chat](chat.md): Use `IChatClient` for chat, streaming, tool calling, and structured output.
- [Text embeddings](embeddings.md): Use `IEmbeddingGenerator` for on-device semantic search.
- [Source-generated AI tools](tool-attributes.md): Turn app methods and properties into tools at compile time with `Microsoft.Maui.AI.Attributes`.
- [Agents](agents/index.md): Build on-device agents and multi-agent workflows with Microsoft Agent Framework.

## See also

- [Microsoft.Extensions.AI overview](/dotnet/ai/microsoft-extensions-ai)
- [Use the IChatClient interface](/dotnet/ai/ichatclient)
- [Use the IEmbeddingGenerator interface](/dotnet/ai/iembeddinggenerator)
