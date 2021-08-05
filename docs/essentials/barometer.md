---
title: "Essentials: Barometer"
description: "Describes the Barometer class in the Microsoft.Maui.Essentials namespace, which lets you monitor the device's barometer sensor."
ms.date: 08/04/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Barometer

The `Barometer` class lets you monitor the device's barometer sensor, which measures pressure.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Barometer

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The Barometer functionality works by calling the `Start` and `Stop` methods to listen for changes to the barometer's pressure reading. The pressure reading is represented in hectopascals. Any changes are sent back through the `ReadingChanged` event. The following code example demonstrates reading the barometer sensor:

```csharp
public class BarometerTest
{
    // Set speed delay for monitoring changes.
    SensorSpeed speed = SensorSpeed.UI;

    public BarometerTest()
    {
        // Register for reading changes.
        Barometer.ReadingChanged += Barometer_ReadingChanged;
    }

    void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
    {
        var data = e.Reading;
        // Process Pressure
        Console.WriteLine($"Reading: Pressure: {data.PressureInHectopascals} hectopascals");
    }

    public void ToggleBarometer()
    {
        try
        {
            if (Barometer.IsMonitoring)
              Barometer.Stop();
            else
              Barometer.Start(speed);
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Feature not supported on device
        }
        catch (Exception ex)
        {
            // Other error has occurred.
        }
    }
}
```

[!INCLUDE [sensor-speed](includes/sensor-speed.md)]

## Platform specifics

This section describes platform-specific implementation details related to the barometer.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

No platform-specific implementation details.

# [iOS](#tab/ios)

This API uses [CMAltimeter](https://developer.apple.com/documentation/coremotion/cmaltimeter#//apple_ref/occ/cl/CMAltimeter) to monitor pressure changes, which is a hardware feature that was added to iPhone 6 and newer devices. A `FeatureNotSupportedException` will be thrown on devices that don't support the altimeter.

`SensorSpeed` is not used as it is not supported on iOS.

# [Windows](#tab/windows)

No platform-specific implementation details.

-----

<!-- markdownlint-enable MD025 -->

## API

- [Barometer source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Barometer)
<!-- - [Barometer API documentation](xref:Microsoft.Maui.Essentials.Barometer)-->
