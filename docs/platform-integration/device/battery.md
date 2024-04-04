---
title: "Battery"
description: "Learn how to use the .NET MAUI IBattery interface in the Microsoft.Maui.Devices namespace. You can check the device's battery information and monitor for changes."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Battery

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Devices.IBattery> interface to check the device's battery information and monitor for changes. This interface also provides information about the device's energy-saver status, which indicates if the device is running in a low-power mode.

The default implementation of the `IBattery` interface is available through the <xref:Microsoft.Maui.Devices.Battery.Default?displayProperty=nameWithType> property. Both the `IBattery` interface and `Battery` class are contained in the `Microsoft.Maui.Devices` namespace.

## Get started

To access the **Battery** functionality the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `BatteryStats` permission is required and must be configured in the Android project. You can configure the permission in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attribute after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.BatteryStats)]
  ```

  \- or -

- Update the Android Manifest:

    Open the _Platforms/Android/AndroidManifest.xml_ file and add the following line in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.BATTERY_STATS" />
  ```

  \- or -

- Update the Android Manifest in the manifest editor:

  In Visual Studio double-click on the *Platforms/Android/AndroidManifest.xml* file to open the Android manifest editor. Then, under **Required permissions** check the **BATTERY_STATS** permission. This will automatically update the *AndroidManifest.xml* file.

# [iOS/Mac Catalyst](#tab/macios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Check the battery status

The battery status can be checked by accessing the `Battery.Default` property, which is the default implementation of the `IBattery` interface. This interface defines various properties to provide information about the state of the battery, such as <xref:Microsoft.Maui.Devices.IBattery.ChargeLevel> to read how much battery is left. The `ChargeLevel` property returns a value between **0.0** and **1.0**, indicating the battery's charge level from empty to full, respectively.

The <xref:Microsoft.Maui.Devices.IBattery.BatteryInfoChanged> event is also available, and is raised when the state of the battery changes. The following example demonstrates how to use the monitor the `BatteryInfoChanged` event and report the battery status to <xref:Microsoft.Maui.Controls.Label> controls:

:::code language="csharp" source="../snippets/shared_1/BatteryTestPage.xaml.cs" id="watch_battery":::

## Low-power energy-saver mode

Devices that run on batteries can be put into a low-power energy-saver mode. Sometimes devices are switched into this mode automatically, like when the battery drops below 20% capacity. The operating system responds to energy-saver mode by reducing activities that tend to deplete the battery. Applications can help by avoiding background processing or other high-power activities when energy-saver mode is on.

> [!IMPORTANT]
> Applications should avoid background processing if the device's energy-saver status is on.

The energy-saver status of the device can be read by accessing the <xref:Microsoft.Maui.Devices.IBattery.EnergySaverStatus> property, which is either <xref:Microsoft.Maui.Devices.EnergySaverStatus.On>, <xref:Microsoft.Maui.Devices.EnergySaverStatus.Off>, or <xref:Microsoft.Maui.Devices.EnergySaverStatus.Unknown>. If the status is `On`, the application should avoid background processing or other activities that may consume a lot of power.

The battery raises the <xref:Microsoft.Maui.Devices.IBattery.EnergySaverStatusChanged> event when the battery enters or leaves energy-saver mode.
You can also obtain the current energy-saver status of the device using the `EnergySaverStatus` property:

The following code example monitors the energy-saver status and sets a property accordingly.

:::code language="csharp" source="../snippets/shared_1/BatteryTestPage.xaml.cs" id="energy_saver":::

## Power source

The <xref:Microsoft.Maui.Devices.IBattery.PowerSource> property returns a <xref:Microsoft.Maui.Devices.BatteryPowerSource> enumeration that indicates how the device is being charged, if at all. If it's not being charged, the status is <xref:Microsoft.Maui.Devices.BatteryPowerSource.Battery?displayProperty=nameWithType>. The <xref:Microsoft.Maui.Devices.BatteryPowerSource.AC?displayProperty=nameWithType>, <xref:Microsoft.Maui.Devices.BatteryPowerSource.Usb?displayProperty=nameWithType>, and <xref:Microsoft.Maui.Devices.BatteryPowerSource.Wireless?displayProperty=nameWithType> values indicate that the battery is being charged.

The following code example sets the text of a <xref:Microsoft.Maui.Controls.Label> control based on power source.

:::code language="csharp" source="../snippets/shared_1/BatteryTestPage.xaml.cs" id="charge_mode":::

## Platform differences

This section describes the platform-specific differences with the battery.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->

# [Android](#tab/android)

No platform differences.

# [iOS/Mac Catalyst](#tab/macios)

- APIs don't work in a simulator and you must use a real device.
- Only returns <xref:Microsoft.Maui.Devices.BatteryPowerSource.AC?displayProperty=nameWithType> or <xref:Microsoft.Maui.Devices.BatteryPowerSource.Battery?displayProperty=nameWithType> for <xref:Microsoft.Maui.Devices.IBattery.PowerSource>.

# [Windows](#tab/windows)

- Only returns <xref:Microsoft.Maui.Devices.BatteryPowerSource.AC?displayProperty=nameWithType> or <xref:Microsoft.Maui.Devices.BatteryPowerSource.Battery?displayProperty=nameWithType> for <xref:Microsoft.Maui.Devices.IBattery.PowerSource>.

-----

<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
