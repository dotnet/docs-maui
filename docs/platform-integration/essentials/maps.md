---
title: "Xamarin.Essentials Map"
description: "The Map class in Microsoft.Maui.Essentials enables an application to open the installed map application to a specific location or placemark."
ms.date: 05/26/2020
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Map

The `Map` class enables an application to open the installed map application to a specific location or placemark.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Map

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The Map functionality works by calling the `OpenAsync` method with the `Location` or `Placemark` to open with optional `MapLaunchOptions`.

```csharp
public class MapTest
{
    public async Task NavigateToBuilding25()
    {
        var location = new Location(47.645160, -122.1306032);
        var options =  new MapLaunchOptions { Name = "Microsoft Building 25" };

        try
        {
            await Map.OpenAsync(location, options);
        }
        catch (Exception ex)
        {
            // No map application available to open
        }
    }
}
```

When opening with a `Placemark`, the following information is required:

- `CountryName`
- `AdminArea`
- `Thoroughfare`
- `Locality`

```csharp
public class MapTest
{
    public async Task NavigateToBuilding25()
    {
        var placemark = new Placemark
            {
                CountryName = "United States",
                AdminArea = "WA",
                Thoroughfare = "Microsoft Building 25",
                Locality = "Redmond"
            };
        var options =  new MapLaunchOptions { Name = "Microsoft Building 25" };

        try
        {
            await Map.OpenAsync(placemark, options);
        }
        catch (Exception ex)
        {
            // No map application available to open or placemark can not be located
        }
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

## Directions Mode

If you call `OpenMapAsync` without any `MapLaunchOptions`, the map will launch to the location specified. Optionally, you can have a navigation route calculated from the device's current position. This is accomplished by setting the `NavigationMode` on the `MapLaunchOptions`:

```csharp
public class MapTest
{
    public async Task NavigateToBuilding25()
    {
        var location = new Location(47.645160, -122.1306032);
        var options =  new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

        await Map.OpenAsync(location, options);
    }
}
```

## Platform differences

# [Android](#tab/android)

- `NavigationMode` supports Bicycling, Driving, and Walking.

# [iOS](#tab/ios)

- `NavigationMode` supports Driving, Transit, and Walking.

# [Windows](#tab/windows)

- `NavigationMode` supports Driving, Transit, and Walking.

--------------

## Platform implementation specifics

# [Android](#tab/android)

Android uses the `geo:` Uri scheme to launch the maps application on the device. This may prompt the user to select from an existing app that supports this Uri scheme.  Xamarin.Essentials is tested with Google Maps, which supports this scheme.

# [iOS](#tab/ios)

No platform-specific implementation details.

# [Windows](#tab/windows)

No platform-specific implementation details.

--------------

## API

- [Map source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Map)
<!-- - [Map API documentation](xref:Microsoft.Maui.Essentials.Map)-->
