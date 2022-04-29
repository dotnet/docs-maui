---
title: "Map"
description: "Learn how to use the .NET MAUI Map class in the Microsoft.Maui.Essentials namespace. This class enables an application to open the installed map application to a specific location or placemark."
ms.date: 08/24/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Map

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Map` class. This class enables an application to open the installed map application to a specific location or placemark.

## Get started

[!INCLUDE [get-started](../essentials/includes/get-started.md)]

[!INCLUDE [essentials-namespace](../essentials/includes/essentials-namespace.md)]

## Using the map

The map functionality works by calling the `Map.OpenAsync` method, and passing either an instance of the `Location` or `Placemark` type. The following example opens the installed map app at a specific GPS location:

```csharp
public async Task NavigateToBuilding25()
{
    var location = new Location(47.645160, -122.1306032);
    var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

    try
    {
        await Map.OpenAsync(location, options);
    }
    catch (Exception ex)
    {
        // No map application available to open
    }
}
```

When using a `Placemark`, more information is required. The information helps the map app search for the place you're looking for. The following information is required:

- `CountryName`
- `AdminArea`
- `Thoroughfare`
- `Locality`

```csharp
public async Task NavigateToBuilding25()
{
    var placemark = new Placemark
    {
        CountryName = "United States",
        AdminArea = "WA",
        Thoroughfare = "Microsoft Building 25",
        Locality = "Redmond"
    };
    var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

    try
    {
        await Map.OpenAsync(placemark, options);
    }
    catch (Exception ex)
    {
        // No map application available to open or placemark can not be located
    }
}
```

## Extension Methods

If you already have a reference to a `Location` or `Placemark`, you can use the built-in extension method `OpenMapAsync` with optional `MapLaunchOptions`:

```csharp
public class MapTest
{
    public async Task OpenPlacemarkOnMap(Placemark placemark)
    {
        try
        {
            await placemark.OpenMapAsync();
        }
        catch (Exception ex)
        {
            // No map application available to open
        }
    }
}
```

## Add navigation

When you open the map, you can calculate a route from the device's current location to the specified location. Pass the `MapLaunchOptions` type to the `Map.OpenAsync` method, specifying the navigation mode. The following example opens the map app and specifies a driving navigation mode:

```csharp
public async Task NavigateToBuilding25()
{
    var location = new Location(47.645160, -122.1306032);
    var options =  new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

    await Map.OpenAsync(location, options);
}
```

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

## API

- [Map source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Map)
<!-- - [Map API documentation](xref:Microsoft.Maui.Essentials.Map)-->
