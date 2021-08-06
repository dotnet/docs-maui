---
title: "Compass"
description: "Describes the Compass class in Microsoft.Maui.Essentials, which lets you monitor the device's magnetic north heading."
ms.date: 11/04/2018
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Compass

The `Compass` class lets you monitor the device's magnetic north heading.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Compass

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The Compass functionality works by calling the `Start` and `Stop` methods to listen for changes to the compass. Any changes are sent back through the `ReadingChanged` event. Here is an example:

```csharp
public class CompassTest
{
    // Set speed delay for monitoring changes.
    SensorSpeed speed = SensorSpeed.UI;

    public CompassTest()
    {
        // Register for reading changes, be sure to unsubscribe when finished
        Compass.ReadingChanged += Compass_ReadingChanged;
    }

    void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
        var data = e.Reading;
        Console.WriteLine($"Reading: {data.HeadingMagneticNorth} degrees");
        // Process Heading Magnetic North
    }

    public void ToggleCompass()
    {
        try
        {
            if (Compass.IsMonitoring)
              Compass.Stop();
            else
              Compass.Start(speed);
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Feature not supported on device
        }
        catch (Exception ex)
        {
            // Some other exception has occurred
        }
    }
}
```

[!INCLUDE [sensor-speed](includes/sensor-speed.md)]

## Platform implementation specifics

# [Android](#tab/android)

Android does not provide an API for retrieving the compass heading. We utilize the accelerometer and magnetometer to calculate the magnetic north heading, which is recommended by Google.

In rare instances, you maybe see inconsistent results because the sensors need to be calibrated, which involves moving your device in a figure-8 motion. The best way of doing this is to open Google Maps, tap on the dot for your location, and select **Calibrate compass**.

Running multiple sensors from your app at the same time may adjust the sensor speed.

## Low Pass Filter

Due to how the Android compass values are updated and calculated there may be a need to smooth out the values. A _Low Pass Filter_ can be applied that averages the sine and cosine values of the angles and can be turned on by using the `Start` method overload, which accepts the `bool applyLowPassFilter` parameter:

```csharp
Compass.Start(SensorSpeed.UI, applyLowPassFilter: true);
```

This is only applied on the Android platform, and the parameter is ignored on iOS and UWP.  More information can be read [here](https://github.com/xamarin/Essentials/pull/354#issuecomment-405316860).

--------------

## API

- [Compass source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Compass)
<!-- - [Compass API documentation](xref:Microsoft.Maui.Essentials.Compass)-->
