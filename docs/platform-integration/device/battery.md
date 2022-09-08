---
title: "Battery"
description: "Learn how to use the .NET MAUI IBattery interface in the Microsoft.Maui.Devices namespace. You can check the device's battery information and monitor for changes."
ms.date: 09/02/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Battery

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IBattery` interface to check the device's battery information and monitor for changes. This interface also provides information about the device's energy-saver status, which indicates if the device is running in a low-power mode.

The default implementation of the `IBattery` interface is available through the `Battery.Default` property. Both the `IBattery` interface and `Battery` class are contained in the `Microsoft.Maui.Devices` namespace.

## Get started

To access the **Battery** functionality the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `Battery` permission is required and must be configured in the Android project. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _AssemblyInfo.cs_ file under the **Properties** folder and add:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.BatteryStats)]
  ```

  \- or -

- Update the Android Manifest:

  In the **Solution Explorer**, open the _AndroidManifest.xml_ file. This is typically located in the **Your-project** > **Platforms** > **Android** folder. Add the following node as a child to the `<manifest>` node:

  ```xml
  <uses-permission android:name="android.permission.BATTERY_STATS" />
  ```

<!-- TODO not yet supported>
  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **Battery** permission. This will automatically update the _AndroidManifest.xml_ file.
-->

# [iOS\macOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Check the battery status

The battery status can be checked by accessing the `Battery.Default` property, which is the default implementation of the `IBattery` interface. This interface defines various properties to provide information about the state of the battery. The `BatteryInfoChanged` event is also available, and is raised when the state of the battery changed.

The following example demonstrates how to use the monitor the `BatteryInfoChanged` event and report the battery status two `Label` controls:

:::code language="csharp" source="../snippets/shared_1/BatteryTestPage.xaml.cs" id="watch_battery":::

The `ChargeLevel` property returns a value between **0.0** and **1.0**, indicating the battery's charge level from empty to full, respectively.

## Low-power energy-saver mode

Devices that run on batteries can be put into a low-power energy-saver mode. Sometimes devices are switched into this mode automatically, like when the battery drops below 20% capacity. The operating system responds to energy-saver mode by reducing activities that tend to deplete the battery. Applications can help by avoiding background processing or other high-power activities when energy-saver mode is on.

> [!IMPORTANT]
> Applications should avoid background processing if the device's energy-saver status is on.

The energy-saver status of the device can be read by accessing the `EnergySaverStatus` property, which is either `On`, `Off`, or `Unknown`. If the status is `On`, the application should avoid background processing or other activities that may consume a lot of power.

The battery will raise the `EnergySaverStatusChanged` event when the battery enters or leaves energy-saver mode.
You can also obtain the current energy-saver status of the device using the `EnergySaverStatus` property:

The following code example monitors the energy-saver status and sets a property accordingly.

:::code language="csharp" source="../snippets/shared_1/BatteryTestPage.xaml.cs" id="energy_saver":::

## Power source

The `PowerSource` property returns a `BatteryPowerSource` enumeration that indicates how the device is being charged, if at all. If it's not being charged, the status will be `Battery`. The `AC`, `Usb`, and `Wireless` values indicate that the battery is being charged.

The following code example sets the text of a `Label` control based on power source.

:::code language="csharp" source="../snippets/shared_1/BatteryTestPage.xaml.cs" id="charge_mode":::

## Platform differences

This section describes the platform-specific differences with the battery.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->

# [Android](#tab/android)

No platform differences.

# [iOS\macOS](#tab/ios)

- APIs won't work in a simulator and you must use a real device.
- Only returns `AC` or `Battery` for `PowerSource`.

# [Windows](#tab/windows)

- Only returns `AC` or `Battery` for `PowerSource`.

-----

<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
