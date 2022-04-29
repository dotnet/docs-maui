---
title: "Geolocation"
description: "Learn how to use the .NET MAUI Geolocation class in the Microsoft.Maui.Essentials namespace. This class provides API to retrieve the device's current geolocation coordinates."
ms.date: 03/13/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Geolocation

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Geolocation` class. This class provides APIs to retrieve the device's current geolocation coordinates.

## Get started

[!INCLUDE [get-started](../essentials/includes/get-started.md)]

[!INCLUDE [essentials-namespace](../essentials/includes/essentials-namespace.md)]

To access the **Geolocation** functionality, the following platform-specific setup is required:

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

**Coarse and Fine Location** permissions are required and must be configured in the Android project. Additionally, if your app targets Android 5.0 (API level 21) or higher, you must declare that your app uses the hardware features in the manifest file. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _AssemblyInfo.cs_ file under the **Properties** folder and add:
  
  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
  [assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
  [assembly: UsesFeature("android.hardware.location", Required = false)]
  [assembly: UsesFeature("android.hardware.location.gps", Required = false)]
  [assembly: UsesFeature("android.hardware.location.network", Required = false)]
  ```

  If your application is targeting Android 10 - Q (API Level 29 or higher) and is requesting **LocationAlways**, you must also add this permission request:

  ```csharp
  [assembly: UsesPermission(Manifest.Permission.AccessBackgroundLocation)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
  <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
  <uses-feature android:name="android.hardware.location" android:required="false" />
  <uses-feature android:name="android.hardware.location.gps" android:required="false" />
  <uses-feature android:name="android.hardware.location.network" android:required="false" />
  ```

  If your application is targeting Android 10 - Q (API Level 29 or higher) and is requesting **LocationAlways**, you must also add this permission request:

  ```xml
  <uses-permission android:name="android.permission.ACCESS_BACKGROUND_LOCATION" />
  ```

  \- or -

- Use the Android project properties:
  
  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **ACCESS_COARSE_LOCATION** and **ACCESS_FINE_LOCATION** permissions. This will automatically update the _AndroidManifest.xml_ file.

> [!TIP]
> Be sure to read the [Android documentation on background location updates](https://developer.android.com/training/location/permissions), as there are many restrictions that need to be considered.

[!INCLUDE [android-permissions](includes/android-permissions.md)]

# [iOS](#tab/ios)

In your _Info.plist_ file, add the following keys:

```xml
<key>NSLocationWhenInUseUsageDescription</key>
<string>Fill in a reason why your app needs access to location.</string>
```

The `<string>` element is the reason the app is requesting access to location information. This text is shown to the user.

An alternative to editing the _Info.plist_ file directly is opening the plist editor. In the editor you can add the **Privacy - Location When In Use Usage Description** property, and fill in a value to display to the user.

# [Windows](#tab/windows)

<!-- TODO: Check if this is still supported in WinUI3 -->
In the _Package.appxmanifest_, under **Capabilities**, ensure that the **Location** capability is checked.

-----
<!-- markdownlint-enable MD025 -->

## Get the last known location

The device may have cached the most recent location of the device. Use the `Geolocation.GetLastKnownLocationAsync` method to access the cached location, if available. This is often faster then doing a full location query, but can be less accurate. If no cached location exists, this method returns `null`.

> [!NOTE]
> When necessary, the Geolocation API prompt's the user for permissions.

The following code example demonstrates checking for a cached location:

```csharp
public async Task GetCachedLocation()
{
    try
    {
        Location location = await Geolocation.GetLastKnownLocationAsync();

        if (location != null)
            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
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
```

Not all location values may be available, depending on the device. For example, the `Altitude` property might be `null`, have a value of 0, or have a positive value indicating the meters above sea level. Other values that may not be present include the `Speed` and `Course` properties.

## Get the current location

While checking for the [last known location](#get-the-last-known-location) of the device may be quicker, it can be inaccurate. Use the `Geolocation.GetLocationAsync` method to query the device for the current location. You can configure the accuracy and timeout of the query. It's best to the method overload that uses the `GeolocationRequest` and `CancellationToken` parameters, since it may take some time to get the device's location.

> [!NOTE]
> When necessary, the Geolocation API prompt's the user for permissions.

The following code example demonstrates how to request the device's location, while supporting cancellation:

```csharp
private CancellationTokenSource _cancelTokenSource;
private bool _isCheckingLocation;

public async Task GetCurrentLocation()
{
    try
    {
        _isCheckingLocation = true;

        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                
        _cancelTokenSource = new CancellationTokenSource();

        Location location = await Geolocation.GetLocationAsync(request, _cancelTokenSource.Token);

        if (location != null)
            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
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
    finally
    {
        _isCheckingLocation = false;
    }
}

public void CancelRequest()
{
    if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
        _cancelTokenSource.Cancel();
}
```

Not all location values may be available, depending on the device. For example, the `Altitude` property might be `null`, have a value of 0, or have a positive value indicating the meters above sea level. Other values that may not be present include `Speed` and `Course`.

## Accuracy

The following sections outline the location accuracy distance, per platform:

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

Some devices may return a mock location from the provider or by an application that provides mock locations. You can detect this by using the `IsFromMockProvider` on any `Location`:

```csharp
public async Task CheckMock()
{
    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
    Location location = await Geolocation.GetLocationAsync(request);

    if (location != null && location.IsFromMockProvider)
    {
        // location is from a mock provider
    }
}
```

## Distance between two locations

The `Location.CalculateDistance` method calculates the distance between two geographic locations. This calculated distance doesn't take roads or other pathways into account, and is merely the shortest distance between the two points along the surface of the Earth. This calculation is known as the _great-circle distance_ calculation.

The following code calculates the distance between the United States of America cities of Boston and San Francisco:

```csharp
Location boston = new Location(42.358056, -71.063611);
Location sanFrancisco = new Location(37.783333, -122.416667);
double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
```

The `Location` constructor accepts the latitude and longitude arguments, respectively. Positive latitude values are north of the equator, and positive longitude values are east of the Prime Meridian. Use the final argument to `CalculateDistance` to specify miles or kilometers. The `UnitConverters` class also defines `KilometersToMiles` and `MilesToKilometers` methods for converting between the two units.

## Platform differences

This section describes the platform-specific differences with the geolocation API.

Altitude is calculated differently on each platform.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

On Android, [altitude](https://developer.android.com/reference/android/location/Location#getAltitude()), if available, is returned in meters above the WGS 84 reference ellipsoid. If this location doesn't have an altitude, `0.0` is returned.

# [iOS](#tab/ios)

On iOS, [altitude](https://developer.apple.com/documentation/corelocation/cllocation/1423820-altitude) is measured in meters. Positive values indicate altitudes above sea level, while negative values indicate altitudes below sea level.

# [Windows](#tab/windows)

On Windows, altitude is returned in meters. For more information, see the [AltitudeReferenceSystem](/uwp/api/windows.devices.geolocation.geopoint.altitudereferencesystem#Windows_Devices_Geolocation_Geopoint_AltitudeReferenceSystem) reference documentation.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## API

- [Geolocation source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Geolocation)
<!-- - [Geolocation API documentation](xref:Microsoft.Maui.Essentials.Geolocation)-->
