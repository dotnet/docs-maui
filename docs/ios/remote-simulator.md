---
title: "Remote iOS Simulator for Windows"
description: "Learn how the remote iOS Simulator for Windows allows you to test your apps on an iOS simulator displayed in Windows alongside Visual Studio 2022."
ms.date: 08/30/2024
---

# Remote iOS Simulator for Windows

The remote iOS Simulator for Windows allows you to test your apps on an iOS simulator displayed in Windows alongside Visual Studio 2022.

## Get started

The remote iOS Simulator for Windows is installed automatically as part of the .NET Multi-platform App UI development workload in Visual Studio 2022. To use it, follow these steps:

1. Launch Visual Studio 2022 and create or load a .NET MAUI app project.
1. In Visual Studio 2022, pair the IDE to a Mac Build host if you haven't previously. For more information, see [Pair to Mac for iOS development](pair-to-mac.md).
1. In the Visual Studio toolbar, use the **Debug Target** drop-down to select **iOS Simulators** and then a specific iOS simulator:

    :::image type="content" source="media/remote-simulator/debug-target.png" alt-text="Visual Studio iOS simulators debug targets.":::

1. In the Visual Studio toolbar, press the green Start button for your chosen iOS simulator:

    :::image type="content" source="media/remote-simulator/chosen-debug-target.png" alt-text="Visual Studio iOS simulator debug target choice.":::

    Visual Studio will build the app, start the remote iOS simulator for Windows, and deploy the app to the simulator:

    :::image type="content" source="media/remote-simulator/remote-simulator.png" alt-text="Visual Studio iOS remote simulator running an app.":::

## Enable the remote iOS simulator for Windows

The remote iOS simulator for Windows is enabled by default. However, if it's been previously disabled it can be enabled in Visual Studio by navigating to **Tools > Options > Xamarin > iOS Settings** and ensuring that **Remote Simulator to Windows** is checked:

:::image type="content" source="media/remote-simulator/enable-remote-simulator.png" alt-text="Visual Studio checkbox to enable the iOS remote simulator.":::

> [!NOTE]
> When the remote simulator is disabled in Visual Studio, debugging a .NET MAUI iOS app will open the iOS Simulator on the connected Mac build host.

## Simulator window toolbar

The toolbar at the top of the simulator's window displays five buttons:

:::image type="content" source="media/remote-simulator/simulator-window-toolbar.png" alt-text="Visual Studio iOS remote simulator for Windows toolbar.":::

The buttons are as follows:

- **Home** – simulates the home button on an iOS device.
- **Lock** – locks the simulator (swipe to unlock).
- **Take Screenshot** – saves a screenshot of the simulator to **\Users\\{User}\Pictures\Xamarin\iOS Simulator**.
- **Settings** – displays keyboard and other settings.
- **Other options** – displays various simulator options such as rotation, and shake gesture.

Clicking the toolbar's **Settings** button (the gear icon) opens the **Settings** window:

:::image type="content" source="media/remote-simulator/settings-window.png" alt-text="iOS remote simulator settings.":::

These settings allow you to enable the hardware keyboard and reset the content and settings for the simulator.

Clicking the toolbar's **Other options** button (the ellipsis icon) reveals additional buttons such as rotation, shake gestures, and rebooting:

:::image type="content" source="media/remote-simulator/other-options.png" alt-text="iOS remote simulator additional settings.":::

> [!NOTE]
> Right-clicking anywhere in the remote iOS simulator window will display all the toolbar buttons as a context menu.

## Touchscreen support

Many Windows computers have touch screens. Since the remote iOS Simulator for Windows supports touch interactions, you can test your app with the same pinch, swipe, and multi-finger touch gestures that you use with physical iOS devices.

Similarly, the remote iOS Simulator for Windows treats Windows Stylus input as Apple Pencil input.

## Sound handling

Sounds played by the simulator will come from the host Mac's speakers. iOS sounds are not heard on Windows.

## Troubleshooting

In some circumstances, an Xcode configuration problem can result in the remote iOS Simulator for Windows getting stuck in a Connecting to Mac...Checking Server...Connected... loop. When this occurs, you need to remove and reset the Simulators on your Mac build host:

- Ensure that Xamarin Mac Agent (XMA) and Xcode aren't running.
- Delete your *~/Library/Developer/CoreSimulator/Devices* folder.
- Run `killall -9 com.apple.CoreSimulator.CoreSimulatorService`.
- Run `xcrun simctl list devices`.

### Logs

If you experience issues with the remote iOS Simulator, you can view the logs in the following locations:

- **Mac** – `~/Library/Logs/Xamarin/Simulator.Server`
- **Windows** – `%LOCALAPPDATA%\Xamarin\Logs\Xamarin.Simulator`
