---
title: Get started with Microsoft.Maui.Essentials.AI
description: Install the Microsoft.Maui.Essentials.AI package and register the chat client and embedding generator services in your .NET MAUI app.
ms.date: 03/11/2026
ms.topic: conceptual
---

# Get started with Microsoft.Maui.Essentials.AI

This page covers installation and service registration. Once services are registered, see [Chat](chat.md) and [Text embeddings](embeddings.md) for usage examples.

## Install the package

Add the NuGet package to your .NET MAUI project:

```dotnetcli
dotnet add package Microsoft.Maui.Essentials.AI
```

Or add the `PackageReference` directly to your `.csproj`:

```xml
<PackageReference Include="Microsoft.Maui.Essentials.AI" Version="10.0.*-*" />
```

## Suppress the experimental warning

`Microsoft.Maui.Essentials.AI` is experimental (diagnostic ID `MAUIAI0001`). Suppress the warning project-wide in your `.csproj`:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);MAUIAI0001</NoWarn>
</PropertyGroup>
```

## Register the chat client

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

Register `AppleIntelligenceChatClient` as a singleton, then expose it as `IChatClient` using `AddChatClient`:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

#if IOS || MACCATALYST
        // Register the platform-specific implementation
        builder.Services.AddSingleton<AppleIntelligenceChatClient>();

        // Register as IChatClient with middleware
        builder.Services.AddChatClient(sp =>
            sp.GetRequiredService<AppleIntelligenceChatClient>()
              .AsBuilder()
              .UseLogging()
              .Build(sp));
#endif

        return builder.Build();
    }
}
```

---
<!-- markdownlint-enable MD025 -->

## Register the embedding generator

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

Register `NLEmbeddingGenerator` as a singleton, then expose it as `IEmbeddingGenerator` using `AddEmbeddingGenerator`:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

#if IOS || MACCATALYST
        // Register the platform-specific implementation
        builder.Services.AddSingleton<NLEmbeddingGenerator>();

        // Register as IEmbeddingGenerator with middleware
        builder.Services.AddEmbeddingGenerator(sp =>
            sp.GetRequiredService<NLEmbeddingGenerator>()
              .AsBuilder()
              .UseLogging()
              .Build(sp));
#endif

        return builder.Build();
    }
}
```

---
<!-- markdownlint-enable MD025 -->

## Next steps

- [Chat](chat.md) — use `IChatClient` for conversational AI, tool calling, and structured output
- [Text embeddings](embeddings.md) — use `IEmbeddingGenerator` for semantic search and similarity
- [Requirements](requirements-apple.md) — supported OS versions and device requirements
