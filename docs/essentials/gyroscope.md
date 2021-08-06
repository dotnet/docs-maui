---
title: "Gyroscope"
description: "The Gyroscope class in Microsoft.Maui.Essentials lets you monitor the device's gyroscope sensor, which measures rotation around the device's three primary axes."
ms.date: 11/04/2018
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Gyroscope

The `Gyroscope` class lets you monitor the device's gyroscope sensor which is the rotation around the device's three primary axes.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Gyroscope

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The Gyroscope functionality works by calling the `Start` and `Stop` methods to listen for changes to the gyroscope. Any changes are sent back through the `ReadingChanged` event in rad/s. Here is sample usage:

```csharp

public class GyroscopeTest
{
    // Set speed delay for monitoring changes.
    SensorSpeed speed = SensorSpeed.UI;

    public GyroscopeTest()
    {
        // Register for reading changes.
        Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
    }

    void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
    {
        var data = e.Reading;
        // Process Angular Velocity X, Y, and Z reported in rad/s
        Console.WriteLine($"Reading: X: {data.AngularVelocity.X}, Y: {data.AngularVelocity.Y}, Z: {data.AngularVelocity.Z}");
    }

    public void ToggleGyroscope()
    {
        try
        {
            if (Gyroscope.IsMonitoring)
              Gyroscope.Stop();
            else
              Gyroscope.Start(speed);
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

## API

- [Gyroscope source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Gyroscope)
<!-- - [Gyroscope API documentation](xref:Microsoft.Maui.Essentials.Gyroscope)-->
