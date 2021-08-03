---
title: "Xamarin.Essentials: Geolocation"
description: "This document describes the Geolocation class in Xamarin.Essentials, which provides APIs to retrieve the device's current geolocation coordinates."
author: jamesmontemagno
ms.custom: video
ms.author: jamont
ms.date: 03/13/2019
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Geolocation

The **Geolocation** class provides APIs to retrieve the device's current geolocation coordinates.

## Get started

[!include[](~/essentials/includes/get-started.md)]

To access the **Geolocation** functionality, the following platform-specific setup is required:

# [Android](#tab/android)

Coarse and Fine Location permissions are required and must be configured in the Android project. Additionally, if your app targets Android 5.0 (API level 21) or higher, you must declare that your app uses the hardware features in the manifest file. This can be added in the following ways:

Open the **AssemblyInfo.cs** file under the **Properties** folder and add:

```csharp
[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]
```

Or update the Android manifest:

Open the **AndroidManifest.xml** file under the **Properties** folder and add the following inside of the **manifest** node:

```xml
<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-feature android:name="android.hardware.location" android:required="false" />
<uses-feature android:name="android.hardware.location.gps" android:required="false" />
<uses-feature android:name="android.hardware.location.network" android:required="false" />
```

Or right-click on the Android project and open the project's properties. Under **Android Manifest** find the **Required permissions:** area and check the **ACCESS_COARSE_LOCATION** and **ACCESS_FINE_LOCATION** permissions. This will automatically update the **AndroidManifest.xml** file.

If your application is targeting Android 10 - Q (API Level 29 or higher) and is requesting **LocationAlways**, you must also add the following permission into **AssemblyInfo.cs**:

```csharp
[assembly: UsesPermission(Manifest.Permission.AccessBackgroundLocation)]
```

Or directly into your **AndroidManifest.xml**:

```xml
<uses-permission android:name="android.permission.ACCESS_BACKGROUND_LOCATION" />
```

If it recommended to read [Android documentation on background location updates](https://developer.android.com/training/location/permissions) as there are many restrictions that need to be considered.

[!include[](~/essentials/includes/android-permissions.md)]

# [iOS](#tab/ios)

Your app's **Info.plist** must contain the `NSLocationWhenInUseUsageDescription` key in order to access the device’s location.

Open the plist editor and add the **Privacy - Location When In Use Usage Description** property and fill in a value to display to the user.

Or manually edit the file and add the following and update the rationale:

```xml
<key>NSLocationWhenInUseUsageDescription</key>
<string>Fill in a reason why your app needs access to location.</string>
```

# [UWP](#tab/uwp)

You must set the `Location` permission for the application. This can be done by opening the **Package.appxmanifest** and selecting the **Capabilities** tab and checking **Location**.

-----

## Using Geolocation

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The Geolocation API will also prompt the user for permissions when necessary.

You can get the last known [location](xref:Xamarin.Essentials.Location) of the device by calling the `GetLastKnownLocationAsync` method. This is often faster then doing a full query, but can be less accurate and may return `null` if no cached location exists.

```csharp
try
{
    var location = await Geolocation.GetLastKnownLocationAsync();

    if (location != null)
    {
        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
    }
}
catch (FeatureNotSupportedException fnsEx)
{
    // Handle not supported on device exception
}
catch (FeatureNotEnabledException fneEx)
{
    // Handle not enabled on device exception
}
catch (PermissionException pEx)
{
    // Handle permission exception
}
catch (Exception ex)
{
    // Unable to get location
}
```

To query the current device's [location](xref:Xamarin.Essentials.Location) coordinates, the `GetLocationAsync` can be used. It is best to pass in a full `GeolocationRequest` and `CancellationToken` since it may take some time to get the device's location.

```csharp
CancellationTokenSource cts;

async Task GetCurrentLocation()
{
    try
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
        cts = new CancellationTokenSource();
        var location = await Geolocation.GetLocationAsync(request, cts.Token);

        if (location != null)
        {
            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
        }
    }
    catch (FeatureNotSupportedException fnsEx)
    {
        // Handle not supported on device exception
    }
    catch (FeatureNotEnabledException fneEx)
    {
        // Handle not enabled on device exception
    }
    catch (PermissionException pEx)
    {
        // Handle permission exception
    }
    catch (Exception ex)
    {
        // Unable to get location
    }
}

protected override void OnDisappearing()
{
    if (cts != null && !cts.IsCancellationRequested)
        cts.Cancel();
    base.OnDisappearing();
}
```

Note all values may be available due to how each device queries geolocation through different providers. For example, the `Altitude` property might be `null`, have a value of 0, or have a positive value, which is in meters above sea level. Other values that may not be present include `Speed` and `Course`.

## Geolocation Accuracy

The following table outlines accuracy per platform:

### Lowest

| Platform | Distance (in meters) |
| --- | --- |
| Android | 500 |
| iOS | 3000 |
| UWP | 1000 - 5000 |

### Low

| Platform | Distance (in meters) |
| --- | --- |
| Android | 500 |
| iOS | 1000 |
| UWP | 300 - 3000 |

### Medium (Default)

| Platform | Distance (in meters) |
| --- | --- |
| Android | 100 - 500 |
| iOS | 100 |
| UWP | 30-500 |

### High

| Platform | Distance (in meters) |
| --- | --- |
| Android | 0 - 100 |
| iOS | 10 |
| UWP | <= 10 |

### Best

| Platform | Distance (in meters) |
| --- | --- |
| Android | 0 - 100 |
| iOS | ~0 |
| UWP | <= 10 |

<a name="calculate-distance"></a>

## Detecting Mock Locations
Some devices may return a mock location from the provider or by an application that provides mock locations. You can detect this by using the `IsFromMockProvider` on any [`Location`](xref:Xamarin.Essentials.Location).

```csharp
var request = new GeolocationRequest(GeolocationAccuracy.Medium);
var location = await Geolocation.GetLocationAsync(request);

if (location != null)
{
    if(location.IsFromMockProvider)
    {
        // location is from a mock provider
    }
}
```

## Distance between Two Locations

The [`Location`](xref:Xamarin.Essentials.Location) and [`LocationExtensions`](xref:Xamarin.Essentials.LocationExtensions) classes define `CalculateDistance` methods that allow you to calculate the distance between two geographic locations. This calculated distance does not take roads or other pathways into account, and is merely the shortest distance between the two points along the surface of the Earth, also known as the _great-circle distance_ or colloquially, the distance "as the crow flies."

Here's an example:

```csharp
Location boston = new Location(42.358056, -71.063611);
Location sanFrancisco = new Location(37.783333, -122.416667);
double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
```

The `Location` constructor has latitude and longitude arguments in that order. Positive latitude values are north of the equator, and positive longitude values are east of the Prime Meridian. Use the final argument to `CalculateDistance` to specify miles or kilometers. The `UnitConverters` class also defines `KilometersToMiles` and `MilesToKilometers` methods for converting between the two units.

## Platform Differences

Altitude is calculated differently on each platform.

# [Android](#tab/android)

On Android, [altitude](https://developer.android.com/reference/android/location/Location#getAltitude()), if available, is returned in meters above the WGS 84 reference ellipsoid. If this location does not have an altitude then 0.0 is returned.

# [iOS](#tab/ios)

On iOS, [altitude](https://developer.apple.com/documentation/corelocation/cllocation/1423820-altitude) is measured in meters. Positive values indicate altitudes above sea level, while negative values indicate altitudes below sea level.

# [UWP](#tab/uwp)

On UWP, altitude is returned in meters. See the [AltitudeReferenceSystem](/uwp/api/windows.devices.geolocation.geopoint.altitudereferencesystem#Windows_Devices_Geolocation_Geopoint_AltitudeReferenceSystem) documentation for more information.

-----

## API

- [Geolocation source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Geolocation)
- [Geolocation API documentation](xref:Xamarin.Essentials.Geolocation)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Geolocation-XamarinEssentials-API-of-the-Week/player]

[!include[](~/essentials/includes/xamarin-show-essentials.md)]
