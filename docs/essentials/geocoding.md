---
title: "Xamarin.Essentials: Geocoding"
description: "The Geocoding class in Xamarin.Essentials provides APIs to both geocode a placemark to a positional coordinates and reverse geocode coordinates to a placemark."
author: jamesmontemagno
ms.author: jamont
ms.date: 05/28/2019
ms.custom: video
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Geocoding

The **Geocoding** class provides APIs to geocode a placemark to a positional coordinates and reverse geocode coordinates to a placemark.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

To access the **Geocoding** functionality the following platform specific setup is required.

# [Android](#tab/android)

No additional setup required.

# [iOS](#tab/ios)

No additional setup required.

# [UWP](#tab/uwp)

A Bing Maps API key is required to use geocoding functionality. Sign up for a free [Bing Maps](https://www.bingmapsportal.com/) account. Under **My account > My keys** create a new key and fill out information based on your application type (which should be **Public Windows App (UWP, 8.x, and earlier)** for UWP apps).

Early on in your application's life before calling any **Geocoding** methods set the API key (which is only available on UWP):

```csharp
Platform.MapServiceToken = "YOUR-KEY-HERE";
```

-----

## Using Geocoding

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

Getting [location](xref:Xamarin.Essentials.Location) coordinates for an address:

```csharp
try
{
    var address =  "Microsoft Building 25 Redmond WA USA";
    var locations = await Geocoding.GetLocationsAsync(address);

    var location = locations?.FirstOrDefault();
    if (location != null)
    {
        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
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

The altitude isn't always available. If it is not available, the `Altitude` property might be `null` or the value might be zero. If the altitude is available, the value is in meters above sea level.

## Using Reverse Geocoding

Reverse geocoding is the process of getting [placemarks](xref:Xamarin.Essentials.Placemark) for an existing set of coordinates:

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

## Distance between Two Locations

The [`Location`](xref:Xamarin.Essentials.Location) and [`LocationExtensions`](xref:Xamarin.Essentials.LocationExtensions) classes define methods to calculate the distance between two locations. See the article [**Xamarin.Essentials: Geolocation**](geolocation.md#calculate-distance) for an example.

## API

- [Geocoding source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Geocoding)
- [Geocoding API documentation](xref:Xamarin.Essentials.Geocoding)
