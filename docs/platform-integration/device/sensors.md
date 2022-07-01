---
title: Accessing device sensors overview
description: "Learn how to use and monitor sensors provided by your device, with .NET MAUI. You can monitor the following sensors: accelerometer, barometer, compass, shake, gyroscope, magnetometer, orientation."
ms.topic: overview
ms.date: 05/23/2022
ms.custom: template-overview
show_latex: true
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices", "Microsoft.Maui.Devices.Sensors"]
---

# Accessing device sensors

Devices have all sorts of sensors that are available to you. Some sensors can detect movement, others changes in the environment, such as light. Monitoring and reacting to these sensors makes your app dynamic in adapting to how the device is being used. You can also respond to changes in the sensors and alert the user. This article gives you a brief overview of the common sensors supported by .NET Multi-User Application (.NET MAUI).

Device sensor-related types are available in the `Microsoft.Maui.Devices.Sensors` namespace.

## Sensor speed

Sensor speed sets the speed in which a sensor will return data to your app. When you start a sensor, you provide the desired sensor speed with the `SensorSpeed` enumeration.

- `Fastest`\
Get the sensor data as fast as possible (not guaranteed to return on UI thread).

- `Game`\
Rate suitable for games (not guaranteed to return on UI thread).

- `Default`\
Default rate suitable for screen orientation changes.

- `UI`\
Rate suitable for general user interface.

> [!WARNING]
> Monitoring too many sensors at once may affect the rate sensor data is returned to your app.

### Sensor event handlers

Event handlers added to sensors with either the `Game` or `Fastest` speeds **aren't** guaranteed to run on the UI thread. If the event handler needs to access user-interface elements, use the [`MainThread.BeginInvokeOnMainThread`](../appmodel/main-thread.md) method to run that code on the UI thread.

## Accelerometer

The accelerometer sensor measures the acceleration of the device along its three axes. The data reported by the sensor represents how the user is moving the device.

To start monitoring the accelerometer sensor, call the `IAccelerometer.Start` method. .NET MAUI sends accelerometer data changes to your app by raising the `IAccelerometer.ReadingChanged` event. Use the `IAccelerometer.Stop` method to stop monitoring the sensor. You can detect the monitoring state of the accelerometer with the `IAccelerometer.IsMonitoring` property, which will be `true` if the accelerometer was started and is currently being monitored.

The following code example demonstrates monitoring the accelerometer for changes. The `Accelerometer.Default` instance would be passed to the `ToggleAccelerometer` method:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="toggle_accelerometer":::

Accelerometer readings are reported back in **G**. A **G** is a unit of gravitation force equal to the gravity exerted by the earth's gravitational field $(9.81 m/s^2)$.

The coordinate-system is defined relative to the screen of the device in its default orientation. The axes aren't swapped when the device's screen orientation changes.

The **X** axis is horizontal and points to the right, the **Y** axis is vertical and points up and the **Z** axis points towards the outside of the front face of the screen. In this system, coordinates behind the screen have negative **Z** values.

Examples:

- When the device lies flat on a table and is pushed on its left side toward the right, the **X** acceleration value is positive.

- When the device lies flat on a table, the acceleration value is +1.00 G or $(+9.81 m/s^2)$, which correspond to the acceleration of the device $(0 m/s^2)$ minus the force of gravity $(-9.81 m/s^2)$ and normalized as in G.

<!-- TODO: Why are A and G mentioned here as if they're XYZ properties from the data? -->
- When the device lies flat on a table and is pushed toward the sky with an acceleration of **A** $m/s^2$, the acceleration value is equal to $A+9.81$ which corresponds to the acceleration of the device $(+A m/s^2)$ minus the force of gravity $(-9.81 m/s^2)$ and normalized in **G**.

### Platform-specific information (Accelerometer)

There is no platform-specific information related to the accelerometer sensor.

## Barometer

The barometer sensor measures the ambient air pressure. The data reported by the sensor represents the current air pressure. This data is reported the first time you start monitoring the sensor and then each time the pressure changes.

To start monitoring the barometer sensor, call the `IBarometer.Start` method. .NET MAUI sends air pressure readings to your app by raising the `IBarometer.ReadingChanged` event. Use the `IBarometer.Stop` method to stop monitoring the sensor. You can detect the monitoring state of the barometer with the `IBarometer.IsMonitoring` property, which will be `true` if the barometer is currently being monitored.

The pressure reading is represented in hectopascals.

The following code example demonstrates monitoring the barometer for changes. The `Barometer.Default` instance would be passed to the `ToggleBarometer` method:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="toggle_barometer":::

### Platform-specific information (Barometer)

This section describes platform-specific implementation details related to the barometer sensor.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No platform-specific implementation details.

# [iOS\macOS](#tab/ios)

This API uses [CMAltimeter](https://developer.apple.com/documentation/coremotion/cmaltimeter#//apple_ref/occ/cl/CMAltimeter) to monitor pressure changes, which is a hardware feature that was added to iPhone 6 and newer devices. A `FeatureNotSupportedException` will be thrown on devices that don't support the altimeter, the sensor used to report air pressure.

`SensorSpeed` isn't used by this API, as it isn't supported on iOS.

# [Windows](#tab/windows)

No platform-specific implementation details.

-----
<!-- markdownlint-enable MD025 -->

## Compass

The compass sensor monitors the device's magnetic north heading.

To start monitoring the compass sensor, call the `Compass.Start` method. .NET MAUI raises the `Compass.ReadingChanged` event when the compass heading changes. Use the `Compass.Stop` method to stop monitoring the sensor. You can detect the monitoring state of the compass with the `Compass.IsMonitoring` property, which will be `true` if the compass is currently being monitored.

The following code example demonstrates monitoring the compass for changes. The `Compass.Default` instance would be passed to the `ToggleCompass` method:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="toggle_compass":::

### Platform-specific information (Compass)

This section describes platform-specific implementation details related to the compass feature.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Android doesn't provide an API for retrieving the compass heading. .NET MAUI uses the accelerometer and magnetometer sensors to calculate the magnetic north heading, which is recommended by Google.

In rare instances, you maybe see inconsistent results because the sensors need to be calibrated. Recalibrating the compass on Android varies by phone model and Android version. You'll need to search the internet on how to recalibrate the compass. Here are two links that may help in recalibrating the compass:

- [Google Help Center: Find and improve your location’s accuracy](https://support.google.com/maps/answer/2839911)
- [Stack Exchange Android Enthusiasts: How can I calibrate the compass on my phone?](https://android.stackexchange.com/questions/10145/how-can-i-calibrate-the-compass-on-my-phone)

Running multiple sensors from your app at the same time may impair the sensor speed.

### Lowpass filter

Because of how the Android compass values are updated and calculated, there may be a need to smooth out the values. A _Lowpass filter_ can be applied that averages the sine and cosine values of the angles and can be turned on by using the `Start` method overload, which accepts the `bool applyLowPassFilter` parameter:

```csharp
Compass.Default.Start(SensorSpeed.UI, applyLowPassFilter: true);
```

This is only applied on the Android platform, and the parameter is ignored on iOS and Windows. For more information, see [this GitHub issue comment](https://github.com/xamarin/Essentials/pull/354#issuecomment-405316860).

# [iOS\macOS](#tab/ios)

No platform-specific implementation details.

# [Windows](#tab/windows)

No platform-specific implementation details.

-----
<!-- markdownlint-enable MD025 -->

## Shake

Even though this article is listing **shake** as a sensor, it isn't. The [accelerometer](#accelerometer) is used to detect when the device is shaken.

The detect shake API uses raw readings from the accelerometer to calculate acceleration. It uses a simple queue mechanism to detect if 3/4ths of the recent accelerometer events occurred in the last half second. Acceleration is calculated by adding the square of the X, Y, and Z ($x^2+y^2+z^2$) readings from the accelerometer and comparing it to a specific threshold.

To start monitoring the accelerometer sensor, call the `IAccelerometer.Start` method. When a shake is detected, the `IAccelerometer.ShakeDetected` event is raised.  Use the `IAccelerometer.Stop` method to stop monitoring the sensor. You can detect the monitoring state of the accelerometer with the `IAccelerometer.IsMonitoring` property, which will be `true` if the accelerometer was started and is currently being monitored.

It's recommended to use `Game` or faster for the `SensorSpeed`.

The following code example demonstrates monitoring the accelerometer for the `ShakeDetected` event. The `Accelerometer.Default` instance would be passed to the `ToggleShake` method:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="toggle_shake":::

### Platform-specific information (Shake)

There is no platform-specific information related to the accelerometer sensor.

## Gyroscope

The gyroscope sensor measures the angular rotation speed around the device's three primary axes.

To start monitoring the gyroscope sensor, call the `IGyroscope.Start` method. .NET MAUI sends gyroscope data changes to your app by raising the `IGyroscope.ReadingChanged` event. The data provided by this event is measured in rad/s (radian per second). Use the `IGyroscope.Stop` method to stop monitoring the sensor. You can detect the monitoring state of the gyroscope with the `IGyroscope.IsMonitoring` property, which will be `true` if the gyroscope was started and is currently being monitored.

The following code example demonstrates monitoring the gyroscope. The `Gyroscope.Default` instance would be passed to the `ToggleGyroscope` method:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="toggle_gyroscope":::

### Platform-specific information (Gyroscope)

There is no platform-specific information related to the gyroscope sensor.

## Magnetometer

The magnetometer sensor indicates the device's orientation relative to Earth's magnetic field.

To start monitoring the magnetometer sensor, call the `IMagnetometer.Start` method. .NET MAUI sends magnetometer data changes to your app by raising the `IMagnetometer.ReadingChanged` event. The data provided by this event is measured in $µT$ (microteslas). Use the `IMagnetometer.Stop` method to stop monitoring the sensor. You can detect the monitoring state of the magnetometer with the `IMagnetometer.IsMonitoring` property, which will be `true` if the magnetometer was started and is currently being monitored.

The following code example demonstrates monitoring the magnetometer. The `Magnetometer.Default` instance would be passed to the `ToggleMagnetometer` method:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="toggle_magnetometer":::

### Platform-specific information (Magnetometer)

There is no platform-specific information related to the magnetometer sensor.

## Orientation

The orientation sensor monitors the orientation of a device in 3D space.

> [!NOTE]
> This sensor isn't used for determining if the device's video display is in portrait or landscape mode. Use the `Orientation` property of the `ScreenMetrics` object available from the [`DeviceDisplay`](../device/display.md) class.

To start monitoring the orientation sensor, call the `IOrientationSensor.Start` method. .NET MAUI sends orientation data changes to your app by raising the `IOrientationSensor.ReadingChanged` event. Use the `IOrientationSensor.Stop` method to stop monitoring the sensor. You can detect the monitoring state of the orientation with the `IOrientationSensor.IsMonitoring` property, which will be `true` if the orientation was started and is currently being monitored.

The following code example demonstrates monitoring the orientation sensor. The `OrientationSensor.Default` instance would be passed to the `ToggleOrientation` method:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="toggle_orientation":::

`IOrientationSensor` readings are reported back in the form of a [`Quaternion`](xref:System.Numerics.Quaternion) that describes the orientation of the device based on two 3D coordinate systems:

The device (generally a phone or tablet) has a 3D coordinate system with the following axes:

- The positive X-axis points to the right of the display in portrait mode.
- The positive Y-axis points to the top of the device in portrait mode.
- The positive Z-axis points out of the screen.

The 3D coordinate system of the Earth has the following axes:

- The positive X-axis is tangent to the surface of the Earth and points east.
- The positive Y-axis is also tangent to the surface of the Earth and points north.
- The positive Z-axis is perpendicular to the surface of the Earth and points up.

The `Quaternion` describes the rotation of the device's coordinate system relative to the Earth's coordinate system.

A `Quaternion` value is closely related to rotation around an axis. If an axis of rotation is the normalized vector ($a_x, a_y, a_z$), and the rotation angle is $\theta$, then the (X, Y, Z, W) components of the quaternion are:

$(a_x \times \sin(\theta/2), a_y \times \sin(\theta/2), a_z \times \sin(\theta/2), \cos(\theta/2)$

These are right-hand coordinate systems, so with the thumb of the right hand pointed in the positive direction of the rotation axis, the curve of the fingers indicate the direction of rotation for positive angles.

Examples:

- When the device lies flat on a table with its screen facing up, with the top of the device (in portrait mode) pointing north, the two coordinate systems are aligned. The `Quaternion` value represents the identity quaternion (0, 0, 0, 1). All rotations can be analyzed relative to this position.

- When the device lies flat on a table with its screen facing up, and the top of the device (in portrait mode) pointing west, the `Quaternion` value is (0, 0, 0.707, 0.707). The device has been rotated 90 degrees around the Z axis of the Earth.

- When the device is held upright so that the top (in portrait mode) points towards the sky, and the back of the device faces north, the device has been rotated 90 degrees around the X axis. The `Quaternion` value is (0.707, 0, 0, 0.707).

- If the device is positioned so its left edge is on a table, and the top points north, the device has been rotated -90 degrees around the Y axis (or 90 degrees around the negative Y axis). The `Quaternion` value is (0, -0.707, 0, 0.707).

### Platform-specific information (Orientation)

There is no platform-specific information related to the orientation sensor.
