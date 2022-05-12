---
title: "Geocoding"
description: "Learn how to use the .NET MAUI Geocoding class in the Microsoft.Maui.Devices.Sensors namespace. This class provides APIs to both geocode a placemark to a positional coordinate, and reverse geocode coordinates to a placemark."
ms.date: 05/12/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices", "Microsoft.Maui.Devices.Sensors"]
---

# Geocoding

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IGeocoding` interface. This interfaces provides APIs to geocode a placemark to a positional coordinates and reverse geocode coordinates to a placemark. The `IGeocoding` interface is exposed through the `Geocoding.Default` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `Geocoding` and `IGeocoding` types are available in the `Microsoft.Maui.Devices.Sensors` namespace.

## Get started

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

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="geocoding_location":::

The altitude isn't always available. If it isn't available, the `Altitude` property might be `null`, or the value might be `0`. If the altitude is available, the value is in meters above sea level.

## Reverse geocoding

Reverse geocoding is the process of getting placemarks for an existing set of coordinates. The following example demonstrates getting placemarks:

:::code language="csharp" source="../snippets/shared_1/SensorsPage.xaml.cs" id="geocoding_reverse":::

## Get the distance between two locations

The `Location` and `LocationExtensions` classes define methods to calculate the distance between two locations. For an example of getting the distance between two locations, see [Distance between two locations](geolocation.md#distance-between-two-locations).
