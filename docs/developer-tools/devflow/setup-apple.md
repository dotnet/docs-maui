---
title: "DevFlow Apple platforms setup (experimental)"
description: "Learn how to configure Mac Catalyst and iOS for DevFlow Blazor WebView debugging."
ms.date: 04/03/2026
---

# DevFlow Apple platforms setup (experimental)

This guide covers how to configure Mac Catalyst and iOS for DevFlow Blazor WebView debugging.

> [!IMPORTANT]
> DevFlow is an **experimental** package from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. It is **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. These packages are provided as-is with no guarantees of support, servicing, or updates.

## Prerequisites

- **Node.js**: Required to run Appium. Install from [nodejs.org](https://nodejs.org).
- **Appium**: Install globally:

  ```bash
  npm install -g appium
  ```

- **XCUITest driver**: Install the Appium XCUITest driver:

  ```bash
  appium driver install xcuitest
  ```

- **ios-webkit-debug-proxy** (iOS devices only): Required for WebView debugging on physical iOS devices. Install via Homebrew:

  ```bash
  brew install ios-webkit-debug-proxy
  ```

- **Xcode command line tools**: Install with:

  ```bash
  xcode-select --install
  ```

## Enable WKWebView inspection

Starting with iOS 16.4 and macOS 13.3, `WKWebView` instances must be explicitly marked as inspectable. Configure this through a `BlazorWebViewHandler` mapper in your `MauiProgram.cs`:

```csharp
#if DEBUG && (IOS || MACCATALYST)
Microsoft.Maui.Handlers.BlazorWebViewHandler.Mapper.AppendToMapping(
    "WebViewInspectable", (handler, view) =>
    {
        if (OperatingSystem.IsIOSVersionAtLeast(16, 4) ||
            OperatingSystem.IsMacCatalystVersionAtLeast(16, 4))
        {
            handler.PlatformView.Inspectable = true;
        }
    });
#endif
```

> [!NOTE]
> Without this configuration, the WebView won't appear as a debuggable target in Safari or DevFlow.

## iOS device settings

On the iOS device, enable Web Inspector:

1. Open **Settings** > **Safari** > **Advanced**.
1. Enable **Web Inspector**.

## Start Appium

Start Appium in a terminal window before running DevFlow commands that target Apple platforms:

```bash
appium
```

Appium runs on `localhost:4723` by default.

## Troubleshooting

| Issue | Possible cause | Resolution |
|-------|---------------|------------|
| WebView context not found | `WKWebView` is not marked as inspectable | Verify the handler mapper code is running and that the platform version checks pass. Confirm the `#if DEBUG` directive is active. |
| Appium connection failures | Appium is not running or the XCUITest driver is not installed | Start Appium with `appium` and verify the driver is installed with `appium driver list --installed`. |
| ios-webkit-debug-proxy issues | Proxy not installed or device not trusted | Install via `brew install ios-webkit-debug-proxy`. Ensure the device is unlocked and trusts the Mac. |
| Finding device UDID | Need to identify a specific device | Run `xcrun xctrace list devices` or connect the device and check **Finder** > **General** > **Serial Number** (click to reveal UDID). |

## See also

- [DevFlow overview](index.md)
- [DevFlow broker architecture](broker.md)
