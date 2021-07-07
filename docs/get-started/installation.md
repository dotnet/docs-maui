---
title: ".NET MAUI installation"
description: "Installation instructions for .NET MAUI."
ms.date: 06/18/2021
---

# .NET MAUI installation

> [!IMPORTANT]
> These requirements will change as new preview releases of Visual Studio and .NET MAUI are released.

To create .NET Multi-platform App UI (MAUI) apps, you currently require .NET 6 Preview 5 with .NET MAUI and the platform SDKs for Android, iOS, macOS, tvOS, and Mac Catalyst.

To create .NET MAUI apps in Visual Studio, you'll also need [Visual Studio 16.11 Preview 2](https://visualstudio.microsoft.com/vs/preview/) with the following workloads installed:

- Mobile development with .NET
- Universal Windows Platform development
- Desktop development with C++
- .NET Desktop Development
- ASP.NET and web development (required for Blazor Desktop and the BlazorWebView control)

:::image type="content" source="installation-images/vs-workloads.png" alt-text="Visual Studio workload.":::

In addition, you must currently install the following Visual Studio extensions to create apps that target Windows UI Library (WinUI) 3:

- [Project Reunion (Preview)](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftProjectReunionPreview)
- [Single-project MSIX Packaging Tools](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingTools)

For more information about the required workloads and components for WinUI 3 development, see [Required workloads and components](/windows/apps/project-reunion/set-up-your-development-environment#required-workloads-and-components).

To use the WebView or BlazorWebView controls on Windows you need to install the WebView2 package:

- [Microsoft Edge WebView2 installer](https://developer.microsoft.com/microsoft-edge/webview2/)

## Install .NET 6 Preview 5

To verify your development environment, and install any missing components, use the [maui-check](https://github.com/Redth/dotnet-maui-check) utility. Install this utility using the following .NET CLI command:

```dotnetcli
dotnet tool install -g redth.net.maui.check
```

Then, run `maui-check`:

```dotnetcli
maui-check
```

If any tools and SDKs required by .NET MAUI are missing, `maui-check` will install them. The example below shows the output generated if the tools and SDKs required by .NET MAUI are already installed:

```dotnetcli
      _   _   _____   _____     __  __      _      _   _   ___
     | \ | | | ____| |_   _|   |  \/  |    / \    | | | | |_ _|
     |  \| | |  _|     | |     | |\/| |   / _ \   | | | |  | |
  _  | |\  | | |___    | |     | |  | |  / ___ \  | |_| |  | |
 (_) |_| \_| |_____|   |_|     |_|  |_| /_/   \_\  \___/  |___|

* .NET MAUI Check v0.5.6.0 *
────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
This tool will attempt to evaluate your .NET MAUI development environment.
If problems are detected, this tool may offer the option to try and fix them for you, or suggest a way to fix them
yourself.

Thanks for choosing .NET MAUI!
────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
» Synchronizing configuration... ok
» Scheduling appointments... ok

> OpenJDK 11.0 Checkup...
 - 11.0.10 (C:\Program Files\Microsoft\jdk-11.0.10.9-hotspot\bin\..)
 - 1.8.0-25 (C:\Program Files\Android\Jdk\microsoft_dist_openjdk_1.8.0.25)

> Visual Studio 16.10.0 Checkup...
 - 16.9.6
 - 16.11.0-pre.2.0 - C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview
 - 15.9.18

> Android SDK Checkup...
 - emulator (30.6.5)
 - build-tools;30.0.2 (30.0.2)
 - platforms;android-30 (3)
 - system-images;android-30;google_apis;x86 (9)
 - platform-tools (30.0.4)

> Android Emulator Checkup...
 - Emulator: pixel_2_xl_pie_9_0_api_28 found.

> .NET SDK Checkup...
 - 2.1.200 - C:\Program Files\dotnet\sdk\2.1.200
 - 2.1.201 - C:\Program Files\dotnet\sdk\2.1.201
 - 2.1.202 - C:\Program Files\dotnet\sdk\2.1.202
 - 2.1.400 - C:\Program Files\dotnet\sdk\2.1.400
 - 2.1.402 - C:\Program Files\dotnet\sdk\2.1.402
 - 2.1.403 - C:\Program Files\dotnet\sdk\2.1.403
 - 2.1.500 - C:\Program Files\dotnet\sdk\2.1.500
 - 2.1.502 - C:\Program Files\dotnet\sdk\2.1.502
 - 2.1.504 - C:\Program Files\dotnet\sdk\2.1.504
 - 2.1.505 - C:\Program Files\dotnet\sdk\2.1.505
 - 2.1.507 - C:\Program Files\dotnet\sdk\2.1.507
 - 2.1.509 - C:\Program Files\dotnet\sdk\2.1.509
 - 2.1.602 - C:\Program Files\dotnet\sdk\2.1.602
 - 2.1.700 - C:\Program Files\dotnet\sdk\2.1.700
 - 2.1.801 - C:\Program Files\dotnet\sdk\2.1.801
 - 5.0.203 - C:\Program Files\dotnet\sdk\5.0.203
 - 5.0.400-preview.21277.10 - C:\Program Files\dotnet\sdk\5.0.400-preview.21277.10
 - 6.0.100-preview.5.21302.13 - C:\Program Files\dotnet\sdk\6.0.100-preview.5.21302.13

> .NET SDK - Workload Deduplication Checkup...

> .NET SDK - EnableWorkloadResolver.sentinel Checkup...
 - C:\Program Files (x86)\Microsoft Visual
Studio\2019\Preview\MSBuild\Current\Bin\SdkResolvers\Microsoft.DotNet.MSBuildSdkResolver\EnableWorkloadResolver.sentinel
exists.

> .NET SDK - Workloads (6.0.100-preview.5.21302.13) Checkup...
 - microsoft-android-sdk-full (Microsoft.NET.Sdk.Android.Manifest-6.0.100 : 30.0.100-preview.5.28) installed.
 - microsoft-ios-sdk-full (Microsoft.NET.Sdk.iOS.Manifest-6.0.100 : 14.5.100-preview.5.894) installed.
 - microsoft-maccatalyst-sdk-full (Microsoft.NET.Sdk.MacCatalyst.Manifest-6.0.100 : 14.5.100-preview.5.894) installed.
 - microsoft-tvos-sdk-full (Microsoft.NET.Sdk.tvOS.Manifest-6.0.100 : 14.5.100-preview.5.894) installed.
 - microsoft-macos-sdk-full (Microsoft.NET.Sdk.macOS.Manifest-6.0.100 : 11.3.100-preview.5.894) installed.

> .NET SDK - Packs (6.0.100-preview.5.21302.13) Checkup...
 - Microsoft.Maui.Templates (6.0.100-preview.5.794) installed.
 - Microsoft.Android.Sdk (30.0.100-preview.5.28) installed.
 - Microsoft.Android.Sdk.BundleTool (30.0.100-preview.5.28) installed.
 - Microsoft.Android.Ref (30.0.100-preview.5.28) installed.
 - Microsoft.Android.Templates (30.0.100-preview.5.28) installed.
 - Microsoft.iOS.Sdk (14.5.100-preview.5.894) installed.
 - Microsoft.iOS.Windows.Sdk (14.5.100-preview.5.894) installed.
 - Microsoft.iOS.Ref (14.5.100-preview.5.894) installed.
 - Microsoft.iOS.Templates (14.5.100-preview.5.894) installed.
 - Microsoft.MacCatalyst.Sdk (14.5.100-preview.5.894) installed.
 - Microsoft.MacCatalyst.Ref (14.5.100-preview.5.894) installed.
 - Microsoft.MacCatalyst.Templates (14.5.100-preview.5.894) installed.
 - Microsoft.tvOS.Sdk (14.5.100-preview.5.894) installed.
 - Microsoft.tvOS.Ref (14.5.100-preview.5.894) installed.
 - Microsoft.tvOS.Templates (14.5.100-preview.5.894) installed.
 - Microsoft.macOS.Sdk (11.3.100-preview.5.894) installed.
 - Microsoft.macOS.Ref (11.3.100-preview.5.894) installed.
 - Microsoft.macOS.Templates (11.3.100-preview.5.894) installed.
────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────

- Congratulations, everything looks great!
```
