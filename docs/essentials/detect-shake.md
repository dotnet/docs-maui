---
title: "Detect Shake"
description: "The Accelerometer class in Microsoft.Maui.Essentials enables you to detect a shake movement of the device."
ms.date: 05/28/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Detect Shake

The `[Accelerometer](accelerometer.md)` class lets you monitor the device's accelerometer sensor, which indicates the acceleration of the device in three-dimensional space. Additionally, it enables you to register for events when the user shakes the device.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Detect Shake

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To detect a shake of the device you must use the Accelerometer functionality by calling the `Start` and `Stop` methods to listen for changes to the acceleration and to detect a shake. Any time a shake is detected a `ShakeDetected` event will fire. It is recommended to use `Game` or faster for the `SensorSpeed`. Here is sample usage:

```csharp

public class DetectShakeTest
{
    // Set speed delay for monitoring changes.
    SensorSpeed speed = SensorSpeed.Game;

    public DetectShakeTest()
    {
        // Register for reading changes, be sure to unsubscribe when finished
        Accelerometer.ShakeDetected  += Accelerometer_ShakeDetected ;
    }

    void Accelerometer_ShakeDetected (object sender, EventArgs e)
    {
        // Process shake event
    }

    public void ToggleAccelerometer()
    {
        try
        {
            if (Accelerometer.IsMonitoring)
              Accelerometer.Stop();
            else
              Accelerometer.Start(speed);
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

## Implementation Details

The detect shake API uses raw readings from the accelerometer to calculate acceleration. It uses a simple queue mechanism to detect if 3/4ths of the recent accelerometer events occurred in the last half second. Acceleration is calculated by adding the square of the X, Y, and Z readings from the accelerometer and comparing it to a specific threashold.

## API

- [Accelerometer source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Accelerometer)
<!-- - [Accelerometer API documentation](xref:Microsoft.Maui.Essentials.Accelerometer)-->
