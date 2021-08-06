---
title: ".NET MAUI installation"
description: "Installation instructions for .NET MAUI."
ms.date: 07/28/2021
---

# .NET MAUI installation

> [!IMPORTANT]
> These requirements will change as new preview releases of Visual Studio and .NET MAUI are released.

To create .NET Multi-platform App UI (.NET MAUI) apps, you currently require .NET 6 Preview 6 with .NET MAUI and the platform SDKs for Android, iOS, macOS, tvOS, and Mac Catalyst.

To create .NET MAUI apps in Visual Studio, you'll also need [Visual Studio 2022 Preview 2](https://visualstudio.microsoft.com/vs/preview/vs2022/) with the following workloads installed:

- Mobile development with .NET
- Universal Windows Platform development
- Desktop development with C++
- .NET Desktop Development
- ASP.NET and web development (required for Blazor Desktop and the `BlazorWebView` control)

:::image type="content" source="installation-images/vs-workloads.png" alt-text="Visual Studio workload.":::

In addition, you must currently install the following Visual Studio extension to create apps that target Windows UI Library (WinUI) 3:

- [Single-project MSIX Packaging Tools](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

For more information about the required workloads and components for WinUI 3 development, see [Required workloads and components](/windows/apps/project-reunion/set-up-your-development-environment#required-workloads-and-components).

To use the `WebView` or `BlazorWebView` controls on Windows you need to install the WebView2 package:

- [Microsoft Edge WebView2 installer](https://developer.microsoft.com/microsoft-edge/webview2/)

## Install .NET 6 Preview 6

In the .NET CLI, run the following command to install the .NET MAUI workloads:

```dotnetcli
dotnet workload install maui
```

To verify your development environment, and install any missing components, use the [maui-check](https://github.com/Redth/dotnet-maui-check) utility. For acquiring and installing .NET SDKs, `maui-check` uses the same workload commands described in the [release notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-maui.md). Install the `maui-check` utility using the following .NET CLI command:

```dotnetcli
dotnet tool install -g redth.net.maui.check
```

Then, run `maui-check`:

```dotnetcli
maui-check
```

If any tools and SDKs required by .NET MAUI are missing, `maui-check` will install them. The example below shows the output generated if the tools and SDKs required by .NET MAUI are already installed:

```dotnetcli
     | \ | | | ____| |_   _|   |  \/  |    / \    | | | | |_ _|
     |  \| | |  _|     | |     | |\/| |   / _ \   | | | |  | |
  _  | |\  | | |___    | |     | |  | |  / ___ \  | |_| |  | |
 (_) |_| \_| |_____|   |_|     |_|  |_| /_/   \_\  \___/  |___|

 .NET MAUI Check v0.6.1.0
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

> Visual Studio 17.0.0-pre.2.0 Checkup...
  - 17.0.0-pre.2.0 - C:\Program Files\Microsoft Visual Studio\2022\Preview

> Android SDK Checkup...
  - emulator (30.7.5)
  - build-tools;30.0.2 (30.0.2)
  - platforms;android-30 (3)
  - system-images;android-30;google_apis_playstore;x86 (9)
  - platform-tools (31.0.2)

> Android Emulator Checkup...
  - Emulator: pixel_2_r_11_0_-_api_30 found.

> .NET SDK Checkup...
  - 2.1.700 - C:\Program Files\dotnet\sdk\2.1.700
  - 2.1.816 - C:\Program Files\dotnet\sdk\2.1.816
  - 3.1.410 - C:\Program Files\dotnet\sdk\3.1.410
  - 5.0.101 - C:\Program Files\dotnet\sdk\5.0.101
  - 5.0.104 - C:\Program Files\dotnet\sdk\5.0.104
  - 5.0.204 - C:\Program Files\dotnet\sdk\5.0.204
  - 6.0.100-preview.6.21355.2 - C:\Program Files\dotnet\sdk\6.0.100-preview.6.21355.2

> .NET SDK - Workload Deduplication Checkup...

> Edge WebView2 Checkup...
  - Found Edge WebView2 version 91.0.864.67

> .NET SDK - Workloads (6.0.100-preview.6.21355.2) Checkup...
  - microsoft-android-sdk-full (Microsoft.NET.Sdk.Android.Manifest-6.0.100 : 30.0.100-preview.6.62) installed.
  - microsoft-ios-sdk-full (Microsoft.NET.Sdk.iOS.Manifest-6.0.100 : 15.0.100-preview.6.63) installed.
  - microsoft-maccatalyst-sdk-full (Microsoft.NET.Sdk.MacCatalyst.Manifest-6.0.100 : 15.0.100-preview.6.63) installed.
  - microsoft-tvos-sdk-full (Microsoft.NET.Sdk.tvOS.Manifest-6.0.100 : 15.0.100-preview.6.63) installed.
  - microsoft-macos-sdk-full (Microsoft.NET.Sdk.macOS.Manifest-6.0.100 : 12.0.100-preview.6.63) installed.
  - maui (Microsoft.NET.Sdk.Maui.Manifest-6.0.100 : 6.0.100-preview.6.1003) installed.
  - microsoft-net-runtime-android (microsoft.net.workload.mono.toolchain.manifest-6.0.100 : 6.0.0-preview.6.21352.12)
installed.
────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────

- Congratulations, everything looks great!
```
