---
title: "Essentials: Accelerometer"
description: "The Accelerometer class in Microsoft.Maui.Essentials lets you monitor the device's accelerometer sensor, which indicates the acceleration of the device in 3D space."
ms.date: 08/02/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials", "A", "G", "X", "Y", "Z"]
---

# Accelerometer

The `Microsoft.Maui.Essentials.Accelerometer` class lets you monitor the device's accelerometer sensor, which indicates the acceleration of the device in 3D space.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Accelerometer

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The accelerometer functionality works by calling the `Start` and `Stop` methods to listen for changes to the acceleration. Any changes are sent back through the `ReadingChanged` event. Here's a sample of using the accelerometer:

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

Accelerometer readings are reported back in **G**. A **G** is a unit of gravitation force equal to the gravity exerted by the earth's gravitational field $(9.81 m/s^2)$.

The coordinate-system is defined relative to the screen of the device in its default orientation. The axes aren't swapped when the device's screen orientation changes.

The **X** axis is horizontal and points to the right, the **Y** axis is vertical and points up and the **Z** axis points towards the outside of the front face of the screen. In this system, coordinates behind the screen have negative **Z** values.

Examples:

- When the device lies flat on a table and is pushed on its left side toward the right, the **X** acceleration value is positive.

- When the device lies flat on a table, the acceleration value is +1.00 G or $(+9.81 m/s^2)$, which correspond to the acceleration of the device $(0 m/s^2)$ minus the force of gravity $(-9.81 m/s^2)$ and normalized as in G.

- When the device lies flat on a table and is pushed toward the sky with an acceleration of **A** $m/s^2$, the acceleration value is equal to $A+9.81$ which corresponds to the acceleration of the device $(+A m/s^2)$ minus the force of gravity $(-9.81 m/s^2)$ and normalized in **G**.

[!INCLUDE [sensor-speed](includes/sensor-speed.md)]

## API

- [Accelerometer source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Accelerometer)
<!-- - [Accelerometer API documentation](xref:Microsoft.Maui.Essentials.Accelerometer)-->
