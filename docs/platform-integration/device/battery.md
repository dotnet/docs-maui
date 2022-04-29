---
title: "Battery"
description: "Learn how to use the .NET MAUI Battery class in the Microsoft.Maui.Essentials namespace. You can check the device's battery information and monitor for changes."
ms.date: 08/05/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Battery

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Battery` class to check the device's battery information and monitor for changes. This class also provides information about the device's energy-saver status, which indicates if the device is running in a low-power mode.

> [!IMPORTANT]
> Applications should avoid background processing if the device's energy-saver status is on.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

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

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.BATTERY_STATS" />
  ```

  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **Battery** permission. This will automatically update the _AndroidManifest.xml_ file.

# [iOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Using Battery

The following example demonstrates checking the state of the battery:

```csharp
double level = Battery.ChargeLevel; // returns 0.0 to 1.0 or 1.0 when on AC or no battery.

BatteryState state = Battery.State;

switch (state)
{
    case BatteryState.Charging:
        // Currently charging
        break;
    case BatteryState.Full:
        // Battery is full
        break;
    case BatteryState.Discharging:
    case BatteryState.NotCharging:
        // Currently discharging battery or not being charged
        break;
    case BatteryState.NotPresent:
        // Battery doesn't exist in device (desktop computer)
        break;
    case BatteryState.Unknown:
        // Unable to detect battery state
        break;
}

var source = Battery.PowerSource;

switch (source)
{
    case BatteryPowerSource.Battery:
        // Being powered by the battery
        break;
    case BatteryPowerSource.AC:
        // Being powered by A/C unit
        break;
    case BatteryPowerSource.Usb:
        // Being powered by USB cable
        break;
    case BatteryPowerSource.Wireless:
        // Powered via wireless charging
        break;
    case BatteryPowerSource.Unknown:
        // Unable to detect power source
        break;
}
```

The `Battery` class also provides the `BatteryInfoChanged` event, with which you can detect changes to the battery:

```csharp
public class BatteryTest
{
    public BatteryTest()
    {
        // Register for battery changes, be sure to unsubscribe when needed
        Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;
    }

    void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs   e)
    {
        double level = e.ChargeLevel;
        BatteryState state = e.State;
        BatteryPowerSource source = e.PowerSource;

        Console.WriteLine($"Reading: Level: {level}, State: {state}, Source: {source}");
    }
}
```

Devices that run on batteries can be put into a low-power energy-saver mode. Sometimes devices are switched into this mode automatically, like when the battery drops below 20% capacity. The operating system responds to energy-saver mode by reducing activities that tend to deplete the battery. Applications can help by avoiding background processing or other high-power activities when energy-saver mode is on.

You can also obtain the current energy-saver status of the device using the static `Battery.EnergySaverStatus` property:

```csharp
// Get energy saver status
EnergySaverStatus status = Battery.EnergySaverStatus;
```

This property returns a member of the `EnergySaverStatus` enumeration, which is either `On`, `Off`, or `Unknown`. If the property returns `On`, the application should avoid background processing or other activities that might consume too much power.

The application should also install an event handler. The `Battery` class exposes an event that is triggered when the energy-saver status changes:

```csharp
public class EnergySaverTest
{
    public EnergySaverTest()
    {
        // Subscribe to changes of energy-saver status
        Battery.EnergySaverStatusChanged += OnEnergySaverStatusChanged;
    }

    private void OnEnergySaverStatusChanged(EnergySaverStatusChangedEventArgs e)
    {
        // Process change
        EnergySaverStatus status = e.EnergySaverStatus;
    }
}
```

If the energy-saver status changes to `On`, the application should stop doing background processing. If the status changes to `Unknown` or `Off`, the application can resume background processing.

## Platform differences

This section describes the platform-specific differences with the battery.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->

# [Android](#tab/android)

No platform differences.

# [iOS](#tab/ios)

- APIs won't work in a simulator and you must use a real device.
- Only returns `AC` or `Battery` for `PowerSource`.

# [Windows](#tab/windows)

- Only returns `AC` or `Battery` for `PowerSource`.

-----

<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## API

- [Battery source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Battery)
<!-- - [Battery API documentation](xref:Microsoft.Maui.Essentials.Battery)-->
