---
title: "Xamarin.Essentials: Magnetometer"
description: "The Magnetometer class in Xamarin.Essentials lets you monitor the device's magnetometer sensor, which indicates the device's orientation relative to Earth's magnetic field."
author: jamesmontemagno
ms.author: jamont
ms.date: 11/04/2018
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Magnetometer

The **Magnetometer** class lets you monitor the device's magnetometer sensor which indicates the device's orientation relative to Earth's magnetic field.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Magnetometer

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The Magnetometer functionality works by calling the `Start` and `Stop` methods to listen for changes to the magnetometer. Any changes are sent back through the `ReadingChanged` event. Here is sample usage:

```csharp

public class MagnetometerTest
{
    // Set speed delay for monitoring changes.
    SensorSpeed speed = SensorSpeed.UI;

    public MagnetometerTest()
    {
        // Register for reading changes.
        Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
    }

    void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
    {
        var data = e.Reading;
        // Process MagneticField X, Y, and Z
        Console.WriteLine($"Reading: X: {data.MagneticField.X}, Y: {data.MagneticField.Y}, Z: {data.MagneticField.Z}");
    }

    public void ToggleMagnetometer()
    {
        try
        {
            if (Magnetometer.IsMonitoring)
              Magnetometer.Stop();
            else
              Magnetometer.Start(speed);
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

All data is returned in µT (microteslas).

[!INCLUDE [sensor-speed](includes/sensor-speed.md)]

## API

- [Magnetometer source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Magnetometer)
- [Magnetometer API documentation](xref:Xamarin.Essentials.Magnetometer)
