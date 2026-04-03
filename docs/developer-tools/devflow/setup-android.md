---
title: "DevFlow Android setup (experimental)"
description: "Learn how to configure Android devices and emulators for DevFlow Blazor WebView debugging using Chrome DevTools Protocol."
ms.date: 04/03/2026
---

# DevFlow Android setup (experimental)

This guide covers how to configure Android devices and emulators for DevFlow Blazor WebView debugging using Chrome DevTools Protocol.

> [!IMPORTANT]
> DevFlow is an **experimental** package from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. It is **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. These packages are provided as-is with no guarantees of support, servicing, or updates.

## Prerequisites

- **ADB (Android Debug Bridge)**: Included with Android SDK Platform Tools. Ensure `adb` is available on your system `PATH`.
- **ChromeDriver**: Must match the Chrome version installed on the target device or emulator. Download from [ChromeDriver downloads](https://chromedriver.chromium.org/downloads).

## Enable USB debugging

To enable USB debugging on a physical Android device:

1. Open **Settings** > **About phone**.
1. Tap **Build number** seven times to enable Developer Options.
1. Go back to **Settings** > **Developer Options**.
1. Enable **USB Debugging**.
1. Connect the device to your development machine via USB and authorize the connection when prompted.

## Enable WebView debugging

The Android WebView must be configured to allow debugging. Choose one of the following options:

### Option 1: In MainActivity.cs

Add the following to your `MainActivity.cs`:

```csharp
#if DEBUG
Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif
```

### Option 2: Via BlazorWebViewHandler mapper

Configure the handler in your `MauiProgram.cs`:

```csharp
#if DEBUG && ANDROID
Microsoft.Maui.Handlers.BlazorWebViewHandler.Mapper.AppendToMapping(
    "WebViewDebugging", (handler, view) =>
    {
        Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
    });
#endif
```

## ADB port forwarding

Android emulators run in an isolated network. Use `adb reverse` to forward ports from the emulator to the host machine:

```bash
# Forward the broker port
adb reverse tcp:19223 tcp:19223

# Forward the agent port (replace {port} with the assigned port)
adb reverse tcp:{port} tcp:{port}
```

> [!NOTE]
> Run these commands each time the emulator restarts. For physical devices connected via USB, the same `adb reverse` commands apply.

## Troubleshooting

| Issue | Possible cause | Resolution |
|-------|---------------|------------|
| No debuggable WebView found | WebView debugging is not enabled | Ensure `SetWebContentsDebuggingEnabled(true)` is called before the WebView loads. Verify the `#if DEBUG` directive is active for your build configuration. |
| ChromeDriver version mismatch | ChromeDriver version doesn't match device Chrome version | Check the Chrome version on the device (**Settings** > **Apps** > **Chrome**) and download the matching ChromeDriver. |
| Multiple devices connected | ADB can't determine which device to target | Use `adb devices` to list connected devices, then specify the target with `adb -s <device-serial> reverse ...`. |
| ADB not found | Platform tools not installed or not on PATH | Install Android SDK Platform Tools and add the directory to your system `PATH`. |

## See also

- [DevFlow overview](index.md)
- [DevFlow broker architecture](broker.md)
