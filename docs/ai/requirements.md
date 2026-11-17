---
title: Requirements for Microsoft.Maui.Essentials.AI
description: Platform, device, and project requirements for on-device chat and text embeddings with Microsoft.Maui.Essentials.AI on Apple platforms, Windows, and Android.
ms.date: 11/17/2026
ms.topic: concept-article
ai-usage: ai-assisted
---

# Requirements for Microsoft.Maui.Essentials.AI

`Microsoft.Maui.Essentials.AI` requires .NET 10 and the matching .NET MAUI workloads. Each platform also has its own OS, hardware, and model requirements. A supported OS version alone doesn't mean a model is available, so always [check the model status](getting-started.md#check-model-readiness) before you enable chat.

| Platform | Chat | Text embeddings |
|----------|------|-----------------|
| iOS and Mac Catalyst | `AppleIntelligenceChatClient`, version 26 or later | `NLEmbeddingGenerator`, iOS 13 or Mac Catalyst 13.1 or later |
| Windows | `WindowsAIChatClient`, Windows 11 24H2 or later | Not supported |
| Android | `GeminiNanoChatClient`, API 26 or later | Not supported |

The package also supports native macOS 26 or later for chat, and macOS 10.15 or later for embeddings.

## Apple platforms

Chat uses Apple's Foundation Models framework, which is part of Apple Intelligence. The device must support Apple Intelligence, and the user must enable it. For supported hardware, see [Apple Intelligence device requirements](https://support.apple.com/en-us/121115).

Build with Xcode 26 or later. Image input, token usage, and explicit tool modes also require version 27 or later at runtime, Apple 27 target frameworks, and Xcode 27.

Text embeddings use Apple's Natural Language framework and don't require Apple Intelligence. The embedding model for the requested language must be installed on the device. See [Text embeddings on Apple platforms](embeddings/apple.md).

## Windows

Chat uses the on-device Windows App SDK `LanguageModel` (Phi Silica). It requires:

- Windows 11 24H2 (build 26100) or later on supported Copilot+ PC hardware.
- An MSIX-packaged app that declares the `systemAIModels` capability.
- A compatible Windows App SDK runtime.
- For Phi Silica access, a [Limited Access Feature unlock token](/windows/ai/apis/phi-silica).

Keep the app packaged in your project file:

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">
  <WindowsPackageType>MSIX</WindowsPackageType>
</PropertyGroup>
```

Declare the capability in `Platforms/Windows/Package.appxmanifest`:

```xml
<Package ...
         xmlns:systemai="http://schemas.microsoft.com/appx/manifest/systemai/windows10"
         IgnorableNamespaces="uap rescap systemai">
  ...
  <Capabilities>
    <rescap:Capability Name="runFullTrust" />
    <systemai:Capability Name="systemAIModels" />
  </Capabilities>
</Package>
```

## Android

Chat uses Gemini Nano through Google's ML Kit GenAI Prompt API. It requires a device with AICore support for Gemini Nano. Preview models, which include the models that support tool calling, also require enrollment in the AICore Developer Preview. The Android emulator doesn't include AICore models.

Set the Android minimum version to API 26 or later in your project file:

```xml
<SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">26.0</SupportedOSPlatformVersion>
```

The model might need to be downloaded before first use. Your app starts the download explicitly. See [Chat on Android](chat/android.md#check-and-download-the-model).

## See also

- [Get started](getting-started.md)
- [Chat feature comparison](chat/feature-comparison.md)
- [Text embedding feature comparison](embeddings/feature-comparison.md)
