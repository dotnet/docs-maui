---
title: "Xamarin.Essentials: Barometer"
description: "The Barometer class in Xamarin.Essentials lets you monitor the device's barometer sensor, which measures pressure."
author: jamesmontemagno
ms.author: jamont
ms.date: 11/04/2018
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Barometer

The **Barometer** class lets you monitor the device's barometer sensor, which measures pressure.

## Get started

[!include[](~/essentials/includes/get-started.md)]

## Using Barometer

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The Barometer functionality works by calling the `Start` and `Stop` methods to listen for changes to the barometer's pressure reading in hectopascals. Any changes are sent back through the `ReadingChanged` event. Here is sample usage:

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

[!include[](~/essentials/includes/sensor-speed.md)]

## Platform Implementation Specifics

# [Android](#tab/android)

No platform-specific implementation details.

# [iOS](#tab/ios)

This API uses [CMAltimeter](https://developer.apple.com/documentation/coremotion/cmaltimeter#//apple_ref/occ/cl/CMAltimeter) to monitor pressure changes, which is a hardware feature that was added to iPhone 6 and newer devices. A `FeatureNotSupportedException` will be thrown on devices that don't support the altimeter.

`SensorSpeed` is not used as it is not supported on iOS.

# [UWP](#tab/uwp)

No platform-specific implementation details.

-----

## API

- [Barometer source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Barometer)
- [Barometer API documentation](xref:Xamarin.Essentials.Barometer)
