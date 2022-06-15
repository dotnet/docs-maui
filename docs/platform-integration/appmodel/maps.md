---
title: "Map"
description: "Learn how to use the .NET MAUI Map class in the Microsoft.Maui.ApplicationModel namespace. This class enables an application to open the installed map application to a specific location or place mark."
ms.date: 05/23/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Map

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IMap` interface. This interface enables an application to open the installed map application to a specific location or place mark. A default implementation of the `IMap` interface is exposed through the `Map.Default` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `IMap` and `Map` types are available in the `Microsoft.Maui.ApplicationModel` namespace.

## Using the map

The map functionality works by calling the `Map.OpenAsync` method, and passing either an instance of the `Location` or `Placemark` type. The following example opens the installed map app at a specific GPS location:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="navigate_building":::

> [!TIP]
> The `Location` and `Placemark` types are in the `Microsoft.Maui.Devices.Sensors` namespace.

When you use a `Placemark` to open the map, more information is required. The information helps the map app search for the place you're looking for. The following information is required:

- `CountryName`
- `AdminArea`
- `Thoroughfare`
- `Locality`

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="navigate_building_placemark":::

## Extension methods

As long as the `Microsoft.Maui.Devices.Sensors` namespace is imported, which a new .NET MAUI project automatically does, you can use the built-in extension method `OpenMapsAsync` to open the map:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="navigate_building_placemark_extension":::

## Add navigation

When you open the map, you can calculate a route from the device's current location to the specified location. Pass the `MapLaunchOptions` type to the `Map.OpenAsync` method, specifying the navigation mode. The following example opens the map app and specifies a driving navigation mode:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="navigate_building_driving":::

## Platform differences

This section describes the platform-specific differences with the maps API.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

`NavigationMode` supports Bicycling, Driving, and Walking.

Android uses the `geo:` Uri scheme to launch the maps application on the device. This may prompt the user to select from an existing app that supports this Uri scheme. Google Maps supports this scheme.

# [iOS](#tab/ios)

`NavigationMode` supports Driving, Transit, and Walking.

# [Windows](#tab/windows)

`NavigationMode` supports Driving, Transit, and Walking.

-----
<!-- markdownlint-enable MD025 -->
