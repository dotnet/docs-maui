---
title: "Microsoft.Maui.Essentials.AI overview"
description: "Learn how to use on-device AI capabilities in .NET MAUI apps with Microsoft.Extensions.AI integration for chat, embeddings, and multi-agent workflows."
ms.date: 02/02/2026
---

# Microsoft.Maui.Essentials.AI overview

Microsoft.Maui.Essentials.AI provides native on-device AI capabilities for .NET MAUI applications through a unified API that integrates with [Microsoft.Extensions.AI](/dotnet/ai/ai-extensions). This enables you to leverage platform-specific AI features—such as Apple Intelligence on iOS and macOS—while writing portable code using standard interfaces like `IChatClient` and `IEmbeddingGenerator<string, Embedding<float>>`.

## Architecture

The library follows a layered architecture:

```
┌─────────────────────────────────────────────────────┐
│           Your .NET MAUI Application                │
├─────────────────────────────────────────────────────┤
│         Microsoft.Extensions.AI Abstractions        │
│    (IChatClient, IEmbeddingGenerator<T, TEmbedding>)│
├─────────────────────────────────────────────────────┤
│          Microsoft.Maui.Essentials.AI               │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐   │
│  │    Apple    │ │   Android   │ │   Windows   │   │
│  │Intelligence │ │ GeminiNano  │ │ PhiSilica   │   │
│  │ ChatClient  │ │ ChatClient  │ │ ChatClient  │   │
│  └─────────────┘ └─────────────┘ └─────────────┘   │
│  ┌─────────────┐                 ┌─────────────┐   │
│  │     NL      │                 │ PhiSilica   │   │
│  │  Embedding  │                 │  Embedding  │   │
│  │  Generator  │                 │  Generator  │   │
│  └─────────────┘                 └─────────────┘   │
├─────────────────────────────────────────────────────┤
│              Platform Native APIs                   │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐   │
│  │   Apple     │ │  Google ML  │ │  Windows    │   │
│  │ Foundation  │ │     Kit     │ │  Copilot    │   │
│  │   Models    │ │   GenAI     │ │  Runtime    │   │
│  └─────────────┘ └─────────────┘ └─────────────┘   │
└─────────────────────────────────────────────────────┘
```

## Key benefits

- **Unified API**: Use `IChatClient` and `IEmbeddingGenerator` interfaces across all platforms
- **On-device processing**: AI runs locally without requiring internet connectivity
- **Privacy-first**: Data stays on the device, never sent to external servers
- **Platform-optimized**: Leverages native AI accelerators (Neural Engine, NPU, GPU)
- **Microsoft.Extensions.AI compatible**: Works with the broader .NET AI ecosystem including logging, middleware, and dependency injection

## Available implementations

| Platform | Chat Client | Embeddings | Status |
|----------|-------------|------------|--------|
| iOS/Mac Catalyst | `AppleIntelligenceChatClient` | `NLEmbeddingGenerator` | ✅ Available |
| macOS | `AppleIntelligenceChatClient` | `NLEmbeddingGenerator` | ✅ Available |
| tvOS | `AppleIntelligenceChatClient` | `NLEmbeddingGenerator` | ✅ Available |
| Android | `GeminiNanoChatClient` | — | ✅ Available |
| Windows | `PhiSilicaChatClient` | `PhiSilicaEmbeddingGenerator` | ✅ Available |

## Getting started

Install the NuGet package:

```shell
dotnet add package Microsoft.Maui.Essentials.AI
```

Register the AI services in your `MauiProgram.cs`:

```csharp
using Microsoft.Maui.Essentials.AI;

var builder = MauiApp.CreateBuilder();
builder.UseMauiApp<App>();

#if IOS || MACCATALYST
builder.Services.AddSingleton<IChatClient>(sp =>
    new AppleIntelligenceChatClient()
        .AsBuilder()
        .UseLogging(sp.GetRequiredService<ILoggerFactory>())
        .Build());
#elif ANDROID
builder.Services.AddSingleton<IChatClient>(sp =>
    new GeminiNanoChatClient()
        .AsBuilder()
        .UseLogging(sp.GetRequiredService<ILoggerFactory>())
        .Build());
#elif WINDOWS
builder.Services.AddSingleton<IChatClient>(sp =>
    new PhiSilicaChatClient()
        .AsBuilder()
        .UseLogging(sp.GetRequiredService<ILoggerFactory>())
        .Build());
#endif

return builder.Build();
```

For complete setup instructions and code samples, see [Getting started with MAUI AI](getting-started.md).

## Documentation

| Article | Description |
|---------|-------------|
| [Platform APIs](platform-apis.md) | Detailed reference for platform-specific AI APIs |
| [Feature comparison](feature-comparison.md) | Feature matrix comparing capabilities across platforms |
| [Requirements](requirements.md) | Minimum OS versions, device requirements, and prerequisites |
| [Getting started](getting-started.md) | Step-by-step guide with code samples |
| [Microsoft Agent Framework](agent-framework.md) | Building multi-agent AI workflows |

## External resources

### Microsoft
- [Microsoft.Extensions.AI documentation](/dotnet/ai/ai-extensions)
- [Microsoft.Extensions.AI.Abstractions NuGet](https://www.nuget.org/packages/Microsoft.Extensions.AI.Abstractions)
- [.NET AI samples and tutorials](/dotnet/ai/)

### Apple
- [Apple Intelligence overview](https://developer.apple.com/apple-intelligence/)
- [Foundation Models framework](https://developer.apple.com/documentation/foundationmodels)
- [Natural Language framework](https://developer.apple.com/documentation/naturallanguage)

### Google
- [ML Kit for Android](https://developers.google.com/ml-kit)
- [ML Kit GenAI APIs](https://developers.google.com/ml-kit/genai)
- [Android AI documentation](https://developer.android.com/ai)

### Microsoft Windows
- [Windows AI overview](/windows/ai/)
- [Windows Copilot Runtime](/windows/ai/apis/)
- [ONNX Runtime](https://onnxruntime.ai/)
- [DirectML](/windows/ai/directml/dml)

## See also

- [Microsoft.Extensions.AI announcement](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)
- [Building AI-powered .NET applications](/dotnet/ai/get-started/dotnet-ai-overview)
