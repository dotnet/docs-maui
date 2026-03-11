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

## Register services

Register the AI services in your `MauiProgram.cs` using a helper extension method. The registration is platform-specific because each platform provides its own underlying implementation.

### Define the extension method

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

Create an extension method on `MauiAppBuilder`. This example registers both the chat client and the embedding generator:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Maui.Essentials.AI;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder AddAIServices(this MauiAppBuilder builder)
    {
        // Chat client: requires iOS 26.0+ / macOS 26.0+
        builder.Services.AddSingleton<AppleIntelligenceChatClient>();
        builder.Services.AddChatClient(sp =>
            sp.GetRequiredService<AppleIntelligenceChatClient>()
              .AsBuilder()
              .UseLogging()
              .Build(sp));

        // Embedding generator: available on iOS 13.0+ / macOS 10.15+
        builder.Services.AddSingleton<NLEmbeddingGenerator>();
        builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
            sp.GetRequiredService<NLEmbeddingGenerator>()
              .AsBuilder()
              .UseLogging(sp.GetRequiredService<ILoggerFactory>())
              .Build());

        return builder;
    }
}
```

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available. See [Android requirements](requirements-android.md) for details.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available. See [Windows requirements](requirements-windows.md) for details.

---
<!-- markdownlint-enable MD025 -->

### Call from MauiProgram.cs

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

#if IOS || MACCATALYST
        builder.AddAIServices();
#endif

        return builder.Build();
    }
}
```

# [Android](#tab/android)

Android support for `Microsoft.Maui.Essentials.AI` is not yet available.

# [Windows](#tab/windows)

Windows support for `Microsoft.Maui.Essentials.AI` is not yet available.

---
<!-- markdownlint-enable MD025 -->

## Next steps

- [Chat](chat.md) — use `IChatClient` for conversational AI, tool calling, and structured output
- [Text embeddings](embeddings.md) — use `IEmbeddingGenerator` for semantic search and similarity
- [Requirements](requirements-apple.md) — supported OS versions and device requirements
