---
title: "Xamarin.Essentials: Accelerometer"
description: "The Accelerometer class in Xamarin.Essentials lets you monitor the device's accelerometer sensor, which indicates the acceleration of the device in three dimensional space."
author: jamesmontemagno
ms.author: jamont
ms.date: 04/02/2019
ms.custom: video
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Accelerometer

The **Accelerometer** class lets you monitor the device's accelerometer sensor, which indicates the acceleration of the device in three-dimensional space.

## Get started

[!include[](~/essentials/includes/get-started.md)]

## Using Accelerometer

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The Accelerometer functionality works by calling the `Start` and `Stop` methods to listen for changes to the acceleration. Any changes are sent back through the `ReadingChanged` event. Here is sample usage:

```csharp

public class AccelerometerTest
{
    // Set speed delay for monitoring changes.
    SensorSpeed speed = SensorSpeed.UI;

    public AccelerometerTest()
    {
        // Register for reading changes, be sure to unsubscribe when finished
        Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
    }

    void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
    {
        var data = e.Reading;
        Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
        // Process Acceleration X, Y, and Z
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

Accelerometer readings are reported back in G. A G is a unit of gravitation force equal to that exerted by the earth's gravitational field (9.81 m/s^2).

The coordinate-system is defined relative to the screen of the phone in its default orientation. The axes are not swapped when the device's screen orientation changes.

The X axis is horizontal and points to the right, the Y axis is vertical and points up and the Z axis points towards the outside of the front face of the screen. In this system, coordinates behind the screen have negative Z values.

Examples:

- When the device lies flat on a table and is pushed on its left side toward the right, the x acceleration value is positive.

- When the device lies flat on a table, the acceleration value is +1.00 G or (+9.81 m/s^2), which correspond to the acceleration of the device (0 m/s^2) minus the force of gravity (-9.81 m/s^2) and normalized as in G.

- When the device lies flat on a table and is pushed toward the sky with an acceleration of A m/s^2, the acceleration value is equal to A+9.81 which corresponds to the acceleration of the device (+A m/s^2) minus the force of gravity (-9.81 m/s^2) and normalized in G.

[!include[](~/essentials/includes/sensor-speed.md)]

## API

- [Accelerometer source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Accelerometer)
- [Accelerometer API documentation](xref:Xamarin.Essentials.Accelerometer)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Accelerometer-XamarinEssentials-API-of-the-Week/player]

[!include[](~/essentials/includes/xamarin-show-essentials.md)]
