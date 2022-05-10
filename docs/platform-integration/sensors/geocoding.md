---
title: "Geocoding"
description: "Learn how to use the .NET MAUI Geocoding class in the Microsoft.Maui.Essentials namespace. This class provides APIs to both geocode a placemark to a positional coordinate, and reverse geocode coordinates to a placemark."
ms.date: 05/28/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Geocoding

The `Geocoding` class provides APIs to geocode a placemark to a positional coordinates and reverse geocode coordinates to a placemark.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

To access the **Geocoding** functionality the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No setup is required.

# [iOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

A Bing Maps API key is required to use geocoding functionality. Sign up for a free [Bing Maps](https://www.bingmapsportal.com/) account. Under **My account** > **My keys**, create a new key and fill out information based on your application type, which should be **Public Windows App (UWP, 8.x, and earlier)**.

Early on in your application's life before calling any `Geocoding` methods, set the API key, which is only available on Windows:

```csharp
Platform.MapServiceToken = "YOUR-KEY-HERE";
```

-----
<!-- markdownlint-enable MD025 -->

## Use geocoding

The following example demonstrates how to get the location coordinates for an address:

```csharp
try
{
    var address = "Microsoft Building 25 Redmond WA USA";
    var locations = await Geocoding.GetLocationsAsync(address);

    var location = locations?.FirstOrDefault();

    if (location != null)
        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
}
catch (FeatureNotSupportedException fnsEx)
{
    // Feature not supported on device
}
catch (Exception ex)
{
    // Handle exception that may have occurred in geocoding
}
```

The altitude isn't always available. If it isn't available, the `Altitude` property might be `null`, or the value might be `0`. If the altitude is available, the value is in meters above sea level.

## Reverse geocoding

Reverse geocoding is the process of getting placemarks for an existing set of coordinates. The following example demonstrates getting placemarks:

```csharp
try
{
    var lat = 47.673988;
    var lon = -122.121513;

    var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);

    var placemark = placemarks?.FirstOrDefault();
                
    if (placemark != null)
    {
        var geocodeAddress =
            $"AdminArea:       {placemark.AdminArea}\n" +
            $"CountryCode:     {placemark.CountryCode}\n" +
            $"CountryName:     {placemark.CountryName}\n" +
            $"FeatureName:     {placemark.FeatureName}\n" +
            $"Locality:        {placemark.Locality}\n" +
            $"PostalCode:      {placemark.PostalCode}\n" +
            $"SubAdminArea:    {placemark.SubAdminArea}\n" +
            $"SubLocality:     {placemark.SubLocality}\n" +
            $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
            $"Thoroughfare:    {placemark.Thoroughfare}\n";

        Console.WriteLine(geocodeAddress);
    }
}
catch (FeatureNotSupportedException fnsEx)
{
    // Feature not supported on device
}
catch (Exception ex)
{
    // Handle exception that may have occurred in geocoding
}
```

## Get the distance between two locations

The `Location` and `LocationExtensions` classes define methods to calculate the distance between two locations. For an example of getting the distance between two locations, see [Distance between two locations](geolocation.md#distance-between-two-locations).
