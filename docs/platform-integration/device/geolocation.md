---
title: "Geolocation"
description: "Learn how to use the .NET MAUI IGeolocation interface in the Microsoft.Maui.Devices.Sensors namespace. This interface provides API to retrieve the device's current geolocation coordinates."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices", "Microsoft.Maui.Devices.Sensors"]
---

# Geolocation

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Devices.Sensors.IGeolocation> interface. This interface provides APIs to retrieve the device's current geolocation coordinates.

The default implementation of the `IGeolocation` interface is available through the <xref:Microsoft.Maui.Devices.Sensors.Geolocation.Default?displayProperty=nameWithType> property. Both the `IGeolocation` interface and `Geolocation` class are contained in the `Microsoft.Maui.Devices.Sensors` namespace.

## Get started

To access the **Geolocation** functionality, the following platform-specific setup is required:

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Coarse *or* fine location permissions, or both, must be specified and should be configured in the Android project.

Additionally, if your app targets Android 5.0 (API level 21) or higher, you must declare that your app uses the hardware features in the manifest file. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attributes after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
  [assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
  [assembly: UsesFeature("android.hardware.location", Required = false)]
  [assembly: UsesFeature("android.hardware.location.gps", Required = false)]
  [assembly: UsesFeature("android.hardware.location.network", Required = false)]
  ```

  If your application is targeting Android 10 - Q (API Level 29 or higher) and is requesting `LocationAlways`, you must also add this permission request:

  ```csharp
  [assembly: UsesPermission(Manifest.Permission.AccessBackgroundLocation)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
  <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
  <uses-feature android:name="android.hardware.location" android:required="false" />
  <uses-feature android:name="android.hardware.location.gps" android:required="false" />
  <uses-feature android:name="android.hardware.location.network" android:required="false" />
  ```

  If your application is targeting Android 10 - Q (API Level 29 or higher) and is requesting `LocationAlways`, you must also add this permission request:

  ```xml
  <uses-permission android:name="android.permission.ACCESS_BACKGROUND_LOCATION" />
  ```

  \- or -

- Update the Android Manifest in the manifest editor:

  In Visual Studio double-click on the *Platforms/Android/AndroidManifest.xml* file to open the Android manifest editor. Then, under **Required permissions** check the permissions listed above. This will automatically update the *AndroidManifest.xml* file.

> [!TIP]
> Be sure to read the [Android documentation on background location updates](https://developer.android.com/training/location/permissions), as there are many restrictions that need to be considered.

# [iOS/Mac Catalyst](#tab/macios)

In the _Platforms/iOS/Info.plist_ and _Platforms/MacCatalyst/Info.plist_ files, add the following keys and values:

```xml
<key>NSLocationWhenInUseUsageDescription</key>
<string>Fill in a reason why your app needs access to location.</string>
```

The `<string>` element is the reason the app is requesting access to location information. This text is shown to the user.

An alternative to editing the _Platforms/iOS/Info.plist_ and _Platforms/MacCatalyst/Info.plist_ files directly is opening the plist editor. In the editor you can add the **Privacy - Location When In Use Usage Description** property, and fill in a value to display to the user.

### Full accuracy location permission

If you're going to request full accuracy with the <xref:Microsoft.Maui.Devices.Sensors.GeolocationRequest.RequestFullAccuracy?displayProperty=nameWithType> property, add the following dictionary to the _Platforms/iOS/Info.plist_ and _Platforms/MacCatalyst/Info.plist_ files:

```xml
<key>NSLocationTemporaryUsageDescriptionDictionary</key>
<dict>
  <key>TemporaryFullAccuracyUsageDescription</key>
  <string>Fill in a reason why your app needs full accuracy</string>
</dict>
```

The `<string>` element is the reason the app is requesting access to location information with full accuracy. This text is shown to the user.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Get the last known location

The device may have cached the most recent location of the device. Use the <xref:Microsoft.Maui.Devices.Sensors.IGeolocation.GetLastKnownLocationAsync> method to access the cached location, if available. This is often faster than doing a full location query, but can be less accurate. If no cached location exists, this method returns `null`.

> [!NOTE]
> When necessary, the Geolocation API prompts the user for permissions.

The following code example demonstrates checking for a cached location:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="geolocation_cached":::

Depending on the device, not all location values may be available. For example, the <xref:Microsoft.Maui.Devices.Sensors.Location.Altitude> property might be `null`, have a value of 0, or have a positive value indicating the meters above sea level. Other values that may not be present include the <xref:Microsoft.Maui.Devices.Sensors.Location.Speed> and <xref:Microsoft.Maui.Devices.Sensors.Location.Course> properties.

## Get the current location

While checking for the [last known location](#get-the-last-known-location) of the device may be quicker, it can be inaccurate. Use the <xref:Microsoft.Maui.Devices.Sensors.IGeolocation.GetLocationAsync%2A> method to query the device for the current location. You can configure the accuracy and timeout of the query. It's best to the method overload that uses the <xref:Microsoft.Maui.Devices.Sensors.GeolocationRequest> and <xref:System.Threading.CancellationToken> parameters, since it may take some time to get the device's location.

> [!NOTE]
> When necessary, the Geolocation API prompts the user for permissions.

The following code example demonstrates how to request the device's location, while supporting cancellation:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="geolocation_get":::

Not all location values may be available, depending on the device. For example, the <xref:Microsoft.Maui.Devices.Sensors.Location.Altitude> property might be `null`, have a value of 0, or have a positive value indicating the meters above sea level. Other values that may not be present include <xref:Microsoft.Maui.Devices.Sensors.Location.Speed> and <xref:Microsoft.Maui.Devices.Sensors.Location.Course>.

> [!WARNING]
> <xref:Microsoft.Maui.Devices.Sensors.IGeolocation.GetLocationAsync%2A> can return `null` in some scenarios. This indicates that the underlying platform is unable to obtain the current location.

::: moniker range=">=net-maui-8.0"

## Listen for location changes

In addition to querying the device for the current location, you can listen for location changes while an app is in the foreground.

To check to see if the app is currently listening for location changes, there's a <xref:Microsoft.Maui.Devices.Sensors.Geolocation.IsListeningForeground> property you can query. Once you're ready to start listening for location changes you should call the <xref:Microsoft.Maui.Devices.Sensors.Geolocation.StartListeningForegroundAsync%2A> method. This method starts listening for location updates and raises the <xref:Microsoft.Maui.Devices.Sensors.Geolocation.LocationChanged> event when the location changes, provided that the app is in the foreground. The <xref:Microsoft.Maui.Devices.Sensors.GeolocationLocationChangedEventArgs> object that accompanies this event has a <xref:Microsoft.Maui.Devices.Sensors.GeolocationLocationChangedEventArgs.Location> property, of type <xref:Microsoft.Maui.Devices.Sensors.Location>, that represents the new location that's been detected.

> [!NOTE]
> When necessary, the Geolocation API prompts the user for permissions.

The following code example demonstrates how to listen for a location change, and how to process the changed location:

```csharp
async void OnStartListening()
{
    try
    {
        Geolocation.LocationChanged += Geolocation_LocationChanged;
        var request = new GeolocationListeningRequest((GeolocationAccuracy)Accuracy);
        var success = await Geolocation.StartListeningForegroundAsync(request);

        string status = success
            ? "Started listening for foreground location updates"
            : "Couldn't start listening";
    }
    catch (Exception ex)
    {
        // Unable to start listening for location changes
    }
}

void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
{
    // Process e.Location to get the new location
}
```

Error handling can be implemented by registering an event handler for the <xref:Microsoft.Maui.Devices.Sensors.Geolocation.ListeningFailed> event. The <xref:Microsoft.Maui.Devices.Sensors.GeolocationListeningFailedEventArgs> object that accompanies this event has an <xref:Microsoft.Maui.Devices.Sensors.GeolocationListeningFailedEventArgs.Error> property, of type <xref:Microsoft.Maui.Devices.Sensors.GeolocationError>, that indicates why listening failed. When the <xref:Microsoft.Maui.Devices.Sensors.Geolocation.ListeningFailed> event is raised, listening for further location changes stops and no further <xref:Microsoft.Maui.Devices.Sensors.Geolocation.LocationChanged> events are raised.

To stop listening for location changes, call the <xref:Microsoft.Maui.Devices.Sensors.Geolocation.StopListeningForeground%2A> method:

```csharp
void OnStopListening()
{
    try
    {
        Geolocation.LocationChanged -= Geolocation_LocationChanged;
        Geolocation.StopListeningForeground();
        string status = "Stopped listening for foreground location updates";
    }
    catch (Exception ex)
    {
        // Unable to stop listening for location changes
    }
}
```

> [!NOTE]
> The <xref:Microsoft.Maui.Devices.Sensors.Geolocation.StopListeningForeground%2A> method has no effect when the app isn't listening for location changes.

::: moniker-end

## Accuracy

The following sections outline the location accuracy distance, per platform:

> [!IMPORTANT]
> iOS has some limitations regarding accuracy. For more information, see the [Platform differences](#platform-differences) section.

### Lowest

| Platform | Distance (in meters) |
|----------|----------------------|
| Android  | 500                  |
| iOS      | 3000                 |
| Windows  | 1000 - 5000          |

### Low

| Platform | Distance (in meters) |
|----------|----------------------|
| Android  | 500                  |
| iOS      | 1000                 |
| Windows  | 300 - 3000           |

### Medium (Default)

| Platform | Distance (in meters) |
|----------|----------------------|
| Android  | 100 - 500            |
| iOS      | 100                  |
| Windows  | 30-500               |

### High

| Platform | Distance (in meters) |
|----------|----------------------|
| Android  | 0 - 100              |
| iOS      | 10                   |
| Windows  | <= 10                |

### Best

| Platform | Distance (in meters) |
|----------|----------------------|
| Android  | 0 - 100              |
| iOS      | ~0                   |
| Windows  | <= 10                |

## Detecting mock locations

Some devices may return a mock location from the provider or by an application that provides mock locations. You can detect this by using the <xref:Microsoft.Maui.Devices.Sensors.Location.IsFromMockProvider> on any <xref:Microsoft.Maui.Devices.Sensors.Location>:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="geolocation_ismock":::

## Distance between two locations

The <xref:Microsoft.Maui.Devices.Sensors.Location.CalculateDistance%2A> method calculates the distance between two geographic locations. This calculated distance doesn't take roads or other pathways into account, and is merely the shortest distance between the two points along the surface of the Earth. This calculation is known as the _great-circle distance_ calculation.

The following code calculates the distance between the United States of America cities of Boston and San Francisco:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="geolocation_distance":::

The <xref:Microsoft.Maui.Devices.Sensors.Location.%23ctor(System.Double,System.Double,System.Double)> constructor accepts the latitude and longitude arguments, respectively. Positive latitude values are north of the equator, and positive longitude values are east of the Prime Meridian. Use the final argument to `CalculateDistance` to specify miles or kilometers. The <xref:Microsoft.Maui.Media.UnitConverters> class also defines <xref:Microsoft.Maui.Media.UnitConverters.KilometersToMiles%2A> and <xref:Microsoft.Maui.Media.UnitConverters.MilesToKilometers%2A> methods for converting between the two units.

## Platform differences

This section describes the platform-specific differences with the geolocation API.

Altitude is calculated differently on each platform.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

On Android, [altitude](https://developer.android.com/reference/android/location/Location#getAltitude()), if available, is returned in meters above the WGS 84 reference ellipsoid. If this location doesn't have an altitude, `0.0` is returned.

The <xref:Microsoft.Maui.Devices.Sensors.Location.ReducedAccuracy?displayProperty=nameWithType> property is only used by iOS and returns `false` on all other platforms.

# [iOS/Mac Catalyst](#tab/macios)

On iOS, [altitude](https://developer.apple.com/documentation/corelocation/cllocation/1423820-altitude) is measured in meters. Positive values indicate altitudes above sea level, while negative values indicate altitudes below sea level.

Starting with iOS 14, the user may restrict your app from detecting a location with full accuracy. The <xref:Microsoft.Maui.Devices.Sensors.Location.ReducedAccuracy?displayProperty=nameWithType> property indicates whether or not the location is using reduced accuracy. To request full accuracy, set the <xref:Microsoft.Maui.Devices.Sensors.GeolocationRequest.RequestFullAccuracy?displayProperty=nameWithType> property to `true`:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="geolocation_request_full":::

# [Windows](#tab/windows)

On Windows, altitude is returned in meters. <!-- For more information, see the [AltitudeReferenceSystem](/uwp/api/windows.devices.geolocation.geopoint.altitudereferencesystem#Windows_Devices_Geolocation_Geopoint_AltitudeReferenceSystem) reference documentation.-->

The <xref:Microsoft.Maui.Devices.Sensors.Location.ReducedAccuracy?displayProperty=nameWithType> property is only used by iOS and returns `false` on all other platforms.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
