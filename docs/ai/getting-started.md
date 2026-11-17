---
title: Get started with Microsoft.Maui.Essentials.AI
description: Install Microsoft.Maui.Essentials.AI, register the on-device chat client and embedding generator, and check model readiness in your .NET MAUI app.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Get started with Microsoft.Maui.Essentials.AI

This article shows how to install the package, register services, and check that the on-device model is ready. Once services are registered, see [Chat](chat.md) and [Text embeddings](embeddings.md) for usage examples.

## Install the package

Add the NuGet package to your .NET MAUI project:

```dotnetcli
dotnet add package Microsoft.Maui.Essentials.AI --prerelease
```

Before you build, check the [requirements](requirements.md) for each platform your app targets. Android apps must set the minimum version to API 26, and Windows apps must be MSIX-packaged and declare the `systemAIModels` capability.

## Suppress the experimental warning

`Microsoft.Maui.Essentials.AI` is experimental, and using it produces diagnostic warning `MAUIAI0001`. Suppress the warning project-wide in your `.csproj` file:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);MAUIAI0001</NoWarn>
</PropertyGroup>
```

## Register the chat client

Register the client for each platform as an `IChatClient`. `AddChatClient` returns a builder, so you can add [Microsoft.Extensions.AI middleware](/dotnet/ai/ichatclient#functionality-pipelines) such as logging:

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
        if (OperatingSystem.IsIOSVersionAtLeast(26) || OperatingSystem.IsMacCatalystVersionAtLeast(26))
        {
            builder.Services.AddChatClient(new AppleIntelligenceChatClient())
                .UseLogging();
        }
#elif WINDOWS
        if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 26100))
        {
            builder.Services.AddChatClient(new WindowsAIChatClient())
                .UseLogging();
        }
#elif ANDROID
        builder.Services.AddChatClient(new GeminiNanoChatClient())
            .UseFunctionInvocation()
            .UseLogging();
#endif

        return builder.Build();
    }
}
```

On Android, `UseFunctionInvocation` runs the tool calls that Gemini Nano returns. Apple Intelligence runs tools itself, and Windows doesn't support tools. See [Tool calling](chat.md#tool-calling).

## Check model readiness

The interface doesn't report whether the on-device model is ready, so each client provides a static `GetModelStatusAsync` method. Check the status before you enable chat, and explain to users when it's unavailable. Requests fail when the model isn't ready; they never prepare or download a model, and they never fall back to a cloud service.

<!-- markdownlint-disable MD025 -->
# [iOS/Mac Catalyst](#tab/macios)

The OS manages Apple Intelligence models, so there's no download API:

```csharp
ChatModelStatus status = await AppleIntelligenceChatClient.GetModelStatusAsync();
if (status.Availability != ChatModelAvailability.Available)
{
    // Apple Intelligence is unsupported, turned off, or still preparing its model.
}
```

# [Windows](#tab/windows)

Call `EnsureReadyAsync` to prepare or download the model:

```csharp
ChatModelStatus status = await WindowsAIChatClient.GetModelStatusAsync();
if (status.Availability == ChatModelAvailability.NotReady)
{
    await WindowsAIChatClient.EnsureReadyAsync(cancellationToken);
}
```

# [Android](#tab/android)

Download the model only after your app or the user decides to:

```csharp
ChatModelStatus status = await GeminiNanoChatClient.GetModelStatusAsync(cancellationToken);
if (status.Availability == ChatModelAvailability.NotReady)
{
    await GeminiNanoChatClient.PrepareModelAsync(cancellationToken);
    status = await GeminiNanoChatClient.GetModelStatusAsync(cancellationToken);
}
```

To report download progress or select a model, see [Chat on Android](chat/android.md).

---
<!-- markdownlint-enable MD025 -->

`status.Capabilities` lists the features the model supports, such as `ChatModelCapability.ToolCalling`, `ChatModelCapability.StructuredOutput`, and `ChatModelCapability.ImageInput`. Use it to enable or hide features in your UI.

## Register the embedding generator

Text embeddings are available on Apple platforms. Register `NLEmbeddingGenerator` as an `IEmbeddingGenerator`:

```csharp
#if IOS || MACCATALYST
builder.Services.AddEmbeddingGenerator(new NLEmbeddingGenerator())
    .UseLogging();
#endif
```

## Next steps

- [Chat](chat.md)
- [Text embeddings](embeddings.md)
- [Chat feature comparison](chat/feature-comparison.md)

## See also

- [Microsoft.Extensions.AI overview](/dotnet/ai/microsoft-extensions-ai)
- [`AddChatClient` API reference](/dotnet/api/microsoft.extensions.dependencyinjection.chatclientbuilderservicecollectionextensions.addchatclient)
- [`AddEmbeddingGenerator` API reference](/dotnet/api/microsoft.extensions.dependencyinjection.embeddinggeneratorbuilderservicecollectionextensions.addembeddinggenerator)
