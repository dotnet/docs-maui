---
title: "OrientationSensor"
description: "The OrientationSensor class lets you monitor the orientation of a device in three-dimensional space."
ms.date: 11/04/2018
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---
# OrientationSensor

The `OrientationSensor` class lets you monitor the orientation of a device in three dimensional space.

> [!NOTE]
> This class is for determining the orientation of a device in 3D space. If you need to determine if the device's video display is in portrait or landscape mode, use the `Orientation` property of the `ScreenMetrics` object available from the [`DeviceDisplay`](device-display.md) class.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using OrientationSensor

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The `OrientationSensor` is enabled by calling the `Start` method to monitor changes to the device's orientation, and disabled by calling the `Stop` method. Any changes are sent back through the `ReadingChanged` event. Here is a sample usage:

```csharp

public class OrientationSensorTest
{
    // Set speed delay for monitoring changes.
    SensorSpeed speed = SensorSpeed.UI;

    public OrientationSensorTest()
    {
        // Register for reading changes, be sure to unsubscribe when finished
        OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
    }

    void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
    {
        var data = e.Reading;
        Console.WriteLine($"Reading: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}, W: {data.Orientation.W}");
        // Process Orientation quaternion (X, Y, Z, and W)
    }

    public void ToggleOrientationSensor()
    {
        try
        {
            if (OrientationSensor.IsMonitoring)
                OrientationSensor.Stop();
            else
                OrientationSensor.Start(speed);
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

`OrientationSensor` readings are reported back in the form of a [`Quaternion`](xref:System.Numerics.Quaternion) that describes the orientation of the device based on two 3D coordinate systems:

The device (generally a phone or tablet) has a 3D coordinate system with the following axes:

- The positive X axis points to the right of the display in portrait mode.
- The positive Y axis points to the top of the device in portrait mode.
- The positive Z axis points out of the screen.

The 3D coordinate system of the Earth has the following axes:

- The positive X axis is tangent to the surface of the Earth and points east.
- The positive Y axis is also tangent to the surface of the Earth and points north.
- The positive Z axis is perpendicular to the surface of the Earth and points up.

The `Quaternion` describes the rotation of the device's coordinate system relative to the Earth's coordinate system.

A `Quaternion` value is very closely related to rotation around an axis. If an axis of rotation is the normalized vector (a<sub>x</sub>, a<sub>y</sub>, a<sub>z</sub>), and the rotation angle is Θ, then the (X, Y, Z, W) components of the quaternion are:

(a<sub>x</sub>·sin(Θ/2), a<sub>y</sub>·sin(Θ/2), a<sub>z</sub>·sin(Θ/2), cos(Θ/2))

These are right-hand coordinate systems, so with the thumb of the right hand pointed in the positive direction of the rotation axis, the curve of the fingers indicate the direction of rotation for positive angles.

Examples:

- When the device lies flat on a table with its screen facing up, with the top of the device (in portrait mode) pointing north, the two coordinate systems are aligned. The `Quaternion` value represents the identity quaternion (0, 0, 0, 1). All rotations can be analyzed relative to this position.

- When the device lies flat on a table with its screen facing up, and the top of the device (in portrait mode) pointing west, the `Quaternion` value is (0, 0, 0.707, 0.707). The device has been rotated 90 degrees around the Z axis of the Earth.

- When the device is held upright so that the top (in portrait mode) points towards the sky, and the back of the device faces north, the device has been rotated 90 degrees around the X axis. The `Quaternion` value is (0.707, 0, 0, 0.707).

- If the device is positioned so its left edge is on a table, and the top points north, the device has been rotated &ndash;90 degrees around the Y axis (or 90 degrees around the negative Y axis). The `Quaternion` value is (0, -0.707, 0, 0.707).

[!INCLUDE [sensor-speed](includes/sensor-speed.md)]

## API

- [OrientationSensor source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/OrientationSensor)
<!-- - [OrientationSensor API documentation](xref:Microsoft.Maui.Essentials.OrientationSensor)-->
