---
title: "DevFlow Android setup"
description: "Learn how to configure Android devices and emulators for DevFlow, including ADB port forwarding and Blazor WebView debugging using Chrome DevTools Protocol."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# DevFlow Android setup

This guide covers how to configure Android devices and emulators for DevFlow, including the ADB port forwarding that every DevFlow app needs and Blazor WebView debugging using Chrome DevTools Protocol.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

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

Android devices and emulators run in an isolated network, so DevFlow needs two ADB port mappings that go in opposite directions:

| Connection | Direction | ADB command |
|------------|-----------|-------------|
| Agent in the app registers with the broker | Device → host | `adb reverse tcp:19223 tcp:19223` |
| CLI connects to the agent's HTTP server | Host → device | `adb forward tcp:{port} tcp:{port}` |

Port `19223` is the default broker port. `{port}` is the agent port, shown in the **Port** column of `maui devflow list` output.

### Automatic forwarding

When exactly one Android device or emulator is online, DevFlow adds missing mappings for you, so you don't need to run `adb reverse` or `adb forward` yourself:

1. Start your emulator or connect your device.
1. Run `maui devflow wait`. It adds the broker reverse mapping before it starts waiting. It waits up to 120 seconds by default; use `--timeout <seconds>` for longer builds.
1. Deploy and run your app. When the agent registers, `maui devflow wait` adds the agent forward mapping.
1. Run `maui devflow list` to confirm that the agent appears. This command also adds any missing mappings for registered Android agents.

Other DevFlow commands that connect to an Android agent also add any missing mappings before they connect.

### Multiple devices

If more than one Android device or emulator is online, DevFlow doesn't guess which one to use, and reports the serials you can choose from. Find the serial with `adb devices`, then set the `ANDROID_SERIAL` environment variable. All DevFlow commands that forward ports honor this variable:

```bash
export ANDROID_SERIAL=emulator-5554
maui devflow wait
```

```powershell
$env:ANDROID_SERIAL = "emulator-5554"
maui devflow wait
```

The `maui devflow wait`, `maui devflow list`, and `maui devflow diagnose` commands also accept a `--device` option, which takes precedence over `ANDROID_SERIAL`:

```bash
maui devflow wait --device emulator-5554
```

Other commands that connect to an agent use only `ANDROID_SERIAL` to select the device for forwarding.

### Check forwarding status

Run `maui devflow diagnose` to see the Android forwarding state. When ADB is available and an Android agent is registered, or you pass `--device`, the output includes an `Android forwarding` section. In `--json` output, this is the `android` property. The section reports the selected device, whether the broker reverse mapping is ready, and whether each agent forward is ready. If the app hasn't registered yet, pass `--device` to check the broker mapping:

```bash
maui devflow diagnose --device emulator-5554
```

The `diagnose` command only reports the forwarding state. It doesn't add missing mappings. For each missing mapping, it suggests the `adb` command to run. To repair forwarding, run `maui devflow list` or `maui devflow wait`, or run the suggested commands manually.

### Manual forwarding

To configure the mappings manually, run these commands, replacing `{port}` with the agent port:

```bash
# Let the agent in the app reach the broker on the host
adb reverse tcp:19223 tcp:19223

# Let the CLI on the host reach the agent in the app
adb forward tcp:{port} tcp:{port}
```

If more than one device is online, add `-s <device-serial>` to each command, for example `adb -s emulator-5554 forward tcp:{port} tcp:{port}`.

> [!NOTE]
> ADB removes the mappings when the emulator restarts or the device disconnects. Run `maui devflow list` or `maui devflow wait` again to restore them. The same mappings apply to physical devices connected through USB.

## Troubleshooting

| Issue | Possible cause | Resolution |
|-------|---------------|------------|
| No debuggable WebView found | WebView debugging is not enabled | Ensure `SetWebContentsDebuggingEnabled(true)` is called before the WebView loads. Verify the `#if DEBUG` directive is active for your build configuration. |
| ChromeDriver version mismatch | ChromeDriver version doesn't match device Chrome version | Check the Chrome version on the device (**Settings** > **Apps** > **Chrome**) and download the matching ChromeDriver. |
| Multiple devices connected | DevFlow and ADB can't determine which device to target | Use `adb devices` to list connected devices, then set `ANDROID_SERIAL=<device-serial>` or pass `--device <device-serial>` to `maui devflow wait`, `list`, or `diagnose`. For manual ADB commands, use `adb -s <device-serial>`. |
| Android agent never appears in `maui devflow list` | The broker reverse mapping is missing | Run `maui devflow wait` to add the mapping while you wait for the agent, or run `adb reverse tcp:19223 tcp:19223`. The agent retries the broker connection in the background. To check the mapping, run `maui devflow diagnose --device <device-serial>`. |
| Connection refused after the agent registers | The agent forward mapping is missing | Run `maui devflow list` to add the mapping, or run `adb forward tcp:{port} tcp:{port}` with the port from `maui devflow list`. |
| ADB not found | Platform tools not installed or not on PATH | Install Android SDK Platform Tools and add the directory to your system `PATH`. |

## See also

- [DevFlow overview](index.md)
- [DevFlow broker architecture](broker.md)
