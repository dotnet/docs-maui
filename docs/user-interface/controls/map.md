---
title: "Map"
description: "Learn how to use the Map control, which is a cross-platform view for displaying and annotating maps. The Map control is available in the Microsoft.Maui.Controls.Maps NuGet package."
ms.date: 08/30/2024
---

# Map

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-map)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Maps.Map> control is a cross-platform view for displaying and annotating maps. The <xref:Microsoft.Maui.Controls.Maps.Map> control uses the native map control on each platform, and is provided by the [Microsoft.Maui.Controls.Maps NuGet package](https://www.nuget.org/packages/Microsoft.Maui.Controls.Maps/).

> [!IMPORTANT]
> The <xref:Microsoft.Maui.Controls.Maps.Map> control isn't supported on Windows due to lack of a map control in WinUI. However, the [CommunityToolkit.Maui.Maps](https://www.nuget.org/packages/CommunityToolkit.Maui.Maps) NuGet package provides access to Bing Maps through a `WebView` on Windows. For more information, see [Get started](/dotnet/communitytoolkit/maui/get-started?tabs=CommunityToolkitMauiMaps).

## Setup

The <xref:Microsoft.Maui.Controls.Maps.Map> control uses the native map control on each platform. This provides a fast, familiar maps experience for users, but means that some configuration steps are needed to adhere to each platforms API requirements.

### Map initialization

The <xref:Microsoft.Maui.Controls.Maps.Map> control is provided by the [Microsoft.Maui.Controls.Maps NuGet package](https://www.nuget.org/packages/Microsoft.Maui.Controls.Maps/), which should be added to your .NET MAUI app project.

After installing the NuGet package, it must be initialized in your app by calling the <xref:Microsoft.Maui.Controls.Hosting.AppHostBuilderExtensions.UseMauiMaps%2A> method on the `MauiAppBuilder` object in the `CreateMauiApp` method of your `MauiProgram` class:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiMaps();

        return builder.Build();
    }
}
```

Once the NuGet package has been added and initialized, <xref:Microsoft.Maui.Controls.Maps.Map> APIs can be used in your project.

### Platform configuration

Additional configuration is required on Android before the map will display. In addition, on iOS, Android, and Mac Catalyst, accessing the user's location requires location permissions to have been granted to your app.

#### iOS and Mac Catalyst

Displaying and interacting with a map on iOS and Mac Catalyst doesn't require any additional configuration. However, to access location services, you should set the required location services requests in **Info.plist**. These will typically be one or more of the following:

- [`NSLocationAlwaysAndWhenInUseUsageDescription`](https://developer.apple.com/documentation/bundleresources/information_property_list/nslocationalwaysandwheninuseusagedescription) – for using location services at all times.
- [`NSLocationWhenInUseUsageDescription`](https://developer.apple.com/library/ios/documentation/General/Reference/InfoPlistKeyReference/Articles/CocoaKeys.html#//apple_ref/doc/uid/TP40009251-SW26) – for using location services when the app is in use.

For more information, see [Choosing the location services authorization to request](https://developer.apple.com/documentation/bundleresources/information_property_list/protected_resources/choosing_the_location_services_authorization_to_request) on developer.apple.com.

The XML representation for these keys in **Info.plist** is shown below. You should update the `string` values to reflect how your app is using the location information:

```xml
<key>NSLocationAlwaysAndWhenInUseUsageDescription</key>
<string>Can we use your location at all times?</string>
<key>NSLocationWhenInUseUsageDescription</key>
<string>Can we use your location when your app is being used?</string>
```

A prompt is then displayed when your app attempts to access the user's location, requesting access:

:::image type="content" source="media/map/permission-ios.png" lightbox="media/map/permission-ios-large.png" alt-text="Screenshot of location permission request on iOS.":::

#### Android

The configuration process for displaying and interacting with a map on Android is to:

1. Get a Google Maps API key and add it to your app manifest.
1. Specify the Google Play services version number in the manifest.
1. [optional] Specify location permissions in the manifest.
1. [optional] Specify the WRITE_EXTERNAL_STORAGE permission in the manifest.

##### Get a Google Maps API key

To use the <xref:Microsoft.Maui.Controls.Maps.Map> control on Android you must generate an API key, which will be consumed by the [Google Maps SDK](https://developers.google.com/maps/documentation/android/) on which the <xref:Microsoft.Maui.Controls.Maps.Map> control relies on Android. To do this, follow the instructions in [Set up in the Google Cloud Console](https://developers.google.com/maps/documentation/android-sdk/cloud-setup) and [Use API Keys](https://developers.google.com/maps/documentation/android-sdk/get-api-key) on developers.google.com.

Once you've obtained an API key it must be added within the `<application>` element of your **Platforms/Android/AndroidManifest.xml** file, by specifying it as the value of the `com.google.android.geo.API_KEY` metadata:

```xml
<application android:allowBackup="true" android:icon="@mipmap/appicon" android:roundIcon="@mipmap/appicon_round" android:supportsRtl="true">
  <meta-data android:name="com.google.android.geo.API_KEY" android:value="PASTE-YOUR-API-KEY-HERE" />
</application>
```

This embeds the API key into the manifest. Without a valid API key the <xref:Microsoft.Maui.Controls.Maps.Map> control will display a blank grid.

> [!NOTE]
> `com.google.android.geo.API_KEY` is the recommended metadata name for the API key. A key with this name can be used to authenticate to multiple Google Maps-based APIs on Android. For backwards compatibility, the `com.google.android.maps.v2.API_KEY` metadata name can be used, but only allows authentication to the Android Maps API v2. An app can only specify one of the API key metadata names.

<!-- For your app package to access Google Maps, you must include SHA-1 fingerprints and package names for every keystore (debug and release) that you use to sign your package. For example, if you use one computer for debug and another computer for generating the release APK, you should include the SHA-1 certificate fingerprint from the debug keystore of the first computer and the SHA-1 certificate fingerprint from the release keystore of the second computer. Also remember to edit the key credentials if the app's **Package Name** changes. -->

##### Specify the Google Play services version number

Add the following declaration within the `<application>` element of **AndroidManifest.xml**:

```xml
<meta-data android:name="com.google.android.gms.version" android:value="@integer/google_play_services_version" />
```

This embeds the version of Google Play services that the app was compiled with, into the manifest.

##### Specify location permissions

If your app needs to access the user's location, you must request permission by adding the `ACCESS_COARSE_LOCATION` or `ACCESS_FINE_LOCATION` permissions (or both) to the manifest, as a child of the `<manifest>` element:

```xml
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
  ...
  <!-- Required to access the user's location -->
  <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
  <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
</manifest>
```

The `ACCESS_COARSE_LOCATION` permission allows the API to use WiFi or mobile data, or both, to determine the device's location. The `ACCESS_FINE_LOCATION` permissions allows the API to use the Global Positioning System (GPS), WiFi, or mobile data to determine a precise a location as possible.

A prompt is then displayed when your app attempts to access the user's location, requesting access:

:::image type="content" source="media/map/permission-android.png" lightbox="media/map/permission-android-large.png" alt-text="Screenshot of location permission request on Android.":::

Alternatively, these permissions can be enabled in Visual Studio's Android manifest editor.

#### Specify the WRITE_EXTERNAL_STORAGE permission

If your app targets API 22 or lower, it will be necessary to add the `WRITE_EXTERNAL_STORAGE` permission to the manifest, as a child of the `<manifest>` element:

```xml
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
```

This is not required if your app targets API 23 or greater.

## Map control

The <xref:Microsoft.Maui.Controls.Maps.Map> class defines the following properties that control map appearance and behavior:

- `IsShowingUser`, of type `bool`, indicates whether the map is showing the user's current location.
- `ItemsSource`, of type `IEnumerable`, which specifies the collection of `IEnumerable` pin items to be displayed.
- `ItemTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, which specifies the <xref:Microsoft.Maui.Controls.DataTemplate> to apply to each item in the collection of displayed pins.
- `ItemTemplateSelector`, of type <xref:Microsoft.Maui.Controls.DataTemplateSelector>, which specifies the <xref:Microsoft.Maui.Controls.DataTemplateSelector> that will be used to choose a <xref:Microsoft.Maui.Controls.DataTemplate> for a pin at runtime.
- `IsScrollEnabled`, of type `bool`, determines whether the map is allowed to scroll.
- `IsTrafficEnabled`, of type `bool`, indicates whether traffic data is overlaid on the map.
- `IsZoomEnabled`, of type `bool`, determines whether the map is allowed to zoom.
- `MapElements`, of type `IList<MapElement>`, represents the list of elements on the map, such as polygons and polylines.
- `MapType`, of type `MapType`, indicates the display style of the map.
- `Pins`, of type `IList<Pin>`, represents the list of pins on the map.
- `VisibleRegion`, of type `MapSpan`, returns the currently displayed region of the map.

These properties, with the exception of the `MapElements`, `Pins`, and `VisibleRegion` properties, are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which mean they can be targets of data bindings.

The <xref:Microsoft.Maui.Controls.Maps.Map> class also defines a `MapClicked` event that's fired when the map is tapped. The `MapClickedEventArgs` object that accompanies the event has a single property named `Location`, of type `Location`. When the event is fired, the `Location` property is set to the map location that was tapped. For information about the `Location` class, see [Location and distance](#location-and-distance).

For information about the `ItemsSource`, `ItemTemplate`, and `ItemTemplateSelector` properties, see [Display a pin collection](#display-a-pin-collection).

### Display a map

A <xref:Microsoft.Maui.Controls.Maps.Map> can be displayed by adding it to a layout or page:

```xaml
<ContentPage ...
             xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps">
    <maps:Map x:Name="map" />
</ContentPage>
```

The equivalent C# code is:

```csharp
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace WorkingWithMaps
{
    public class MapTypesPageCode : ContentPage
    {
        public MapTypesPageCode()
        {
            Map map = new Map();
            Content = map;
        }
    }
}
```

This example calls the default <xref:Microsoft.Maui.Controls.Maps.Map> constructor, which centers the map on Maui, Hawaii::

:::image type="content" source="media/map/map-default.png" alt-text="Screenshot of map control with default location.":::

Alternatively, a `MapSpan` argument can be passed to a <xref:Microsoft.Maui.Controls.Maps.Map> constructor to set the center point and zoom level of the map when it's loaded. For more information, see [Display a specific location on a map](#display-a-specific-location-on-a-map).

> [!IMPORTANT]
> .NET MAUI has two `Map` types - <xref:Microsoft.Maui.Controls.Maps.Map?displayProperty=fullName> and <xref:Microsoft.Maui.ApplicationModel.Map?displayProperty=fullName>. Because the <xref:Microsoft.Maui.ApplicationModel> namespace is one of .NET MAUI's `global using` directives, when using the <xref:Microsoft.Maui.Controls.Maps.Map?displayProperty=fullName> control from code you'll have to fully qualify your `Map` usage or use a [using alias](/dotnet/csharp/language-reference/keywords/using-directive#using-alias).

### Map types

The `Map.MapType` property can be set to a `MapType` enumeration member to define the display style of the map. The `MapType` enumeration defines the following members:

- `Street` specifies that a street map will be displayed.
- `Satellite` specifies that a map containing satellite imagery will be displayed.
- `Hybrid` specifies that a map combining street and satellite data will be displayed.

By default, a <xref:Microsoft.Maui.Controls.Maps.Map> will display a street map if the `MapType` property is undefined. Alternatively, the `MapType` property can be set to one of the `MapType` enumeration members:

```xaml
<maps:Map MapType="Satellite" />
```

The equivalent C# code is:

```csharp
Map map = new Map
{
    MapType = MapType.Satellite
};
```

### Display a specific location on a map

The region of a map to display when a map is loaded can be set by passing a `MapSpan` argument to the <xref:Microsoft.Maui.Controls.Maps.Map> constructor:

```xaml
<ContentPage ...
             xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps"
             xmlns:sensors="clr-namespace:Microsoft.Maui.Devices.Sensors;assembly=Microsoft.Maui.Essentials">
    <maps:Map>
        <x:Arguments>
            <maps:MapSpan>
                <x:Arguments>
                    <sensors:Location>
                        <x:Arguments>
                            <x:Double>36.9628066</x:Double>
                            <x:Double>-122.0194722</x:Double>
                        </x:Arguments>
                    </sensors:Location>
                    <x:Double>0.01</x:Double>
                    <x:Double>0.01</x:Double>
                </x:Arguments>
            </maps:MapSpan>
        </x:Arguments>
    </maps:Map>
</ContentPage>
```

The equivalent C# code is:

```csharp
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
...

Location location = new Location(36.9628066, -122.0194722);
MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
Map map = new Map(mapSpan);
```

This example creates a <xref:Microsoft.Maui.Controls.Maps.Map> object that shows the region that is specified by the `MapSpan` object. The `MapSpan` object is centered on the latitude and longitude represented by a `Location` object, and spans 0.01 latitude and 0.01 longitude degrees. For information about the `Location` class, see [Location and distance](#location-and-distance). For information about passing arguments in XAML, see [Pass arguments in XAML](~/xaml/pass-arguments.md).

The result is that when the map is displayed, it's centered on a specific location, and spans a specific number of latitude and longitude degrees:

:::image type="content" source="media/map/map-region.png" alt-text="Screenshot of map control with specified location.":::

### Create a MapSpan object

There are a number of approaches for creating `MapSpan` objects. A common approach is supply the required arguments to the `MapSpan` constructor. These are a latitude and longitude represented by a `Location` object, and `double` values that represent the degrees of latitude and longitude that are spanned by the `MapSpan`. For information about the `Location` class, see [Location and distance](#location-and-distance).

Alternatively, there are three methods in the `MapSpan` class that return new `MapSpan` objects:

1. `ClampLatitude` returns a `MapSpan` with the same `LongitudeDegrees` as the method's class instance, and a radius defined by its `north` and `south` arguments.
1. `FromCenterAndRadius` returns a `MapSpan` that is defined by its `Location` and `Distance` arguments.
1. `WithZoom` returns a `MapSpan` with the same center as the method's class instance, but with a radius multiplied by its `double` argument.

For information about the `Distance` struct, see [Location and distance](#location-and-distance).

Once a `MapSpan` has been created, the following properties can be accessed to retrieve data about it:

- `Center`, of type `Location`, which represents the location in the geographical center of the `MapSpan`.
- `LatitudeDegrees`, of type `double`, which represents the degrees of latitude that are spanned by the `MapSpan`.
- `LongitudeDegrees`, of type `double`, which represents the degrees of longitude that are spanned by the `MapSpan`.
- `Radius`, of type `Distance`, which represents the `MapSpan` radius.

### Move the map

The `Map.MoveToRegion` method can be called to change the position and zoom level of a map. This method accepts a `MapSpan` argument that defines the region of the map to display, and its zoom level.

The following code shows an example of moving the displayed region on a map:

```csharp
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps.Map;
...

MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.444));
map.MoveToRegion(mapSpan);
```

### Zoom the map

The zoom level of a <xref:Microsoft.Maui.Controls.Maps.Map> can be changed without altering its location. This can be accomplished using the map UI, or programatically by calling the `MoveToRegion` method with a `MapSpan` argument that uses the current location as the `Location` argument:

```csharp
double zoomLevel = 0.5;
double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
if (map.VisibleRegion != null)
{
    map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongDegrees, latlongDegrees));
}
```

In this example, the `MoveToRegion` method is called with a `MapSpan` argument that specifies the current location of the map, via the `Map.VisibleRegion` property, and the zoom level as degrees of latitude and longitude. The overall result is that the zoom level of the map is changed, but its location isn't. An alternative approach for implementing zoom on a map is to use the `MapSpan.WithZoom` method to control the zoom factor.

> [!IMPORTANT]
> Zooming a map, whether via the map UI or programatically, requires that the `Map.IsZoomEnabled` property is `true`. For more information about this property, see [Disable zoom](#disable-zoom).

### Customize map behavior

The behavior of a <xref:Microsoft.Maui.Controls.Maps.Map> can be customized by setting some of its properties, and by handling the `MapClicked` event.

> [!NOTE]
> Additional map behavior customization can be achieved by customizing its handler. For more information, see [Customize controls with handlers](~/user-interface/handlers/customize.md).

#### Show traffic data

The <xref:Microsoft.Maui.Controls.Maps.Map> class defines a `IsTrafficEnabled` property of type `bool`. By default this property is `false`, which indicates that traffic data won't be overlaid on the map. When this property is set to `true`, traffic data is overlaid on the map:

```xaml
<maps:Map IsTrafficEnabled="true" />
```

The equivalent C# code is:

```csharp
Map map = new Map
{
    IsTrafficEnabled = true
};
```

#### Disable scroll

The <xref:Microsoft.Maui.Controls.Maps.Map> class defines a `IsScrollEnabled` property of type `bool`. By default this property is `true`, which indicates that the map is allowed to scroll. When this property is set to `false`, the map will not scroll:

```xaml
<maps:Map IsScrollEnabled="false" />
```

The equivalent C# code is:

```csharp
Map map = new Map
{
    IsScrollEnabled = false
};
```

#### Disable zoom

The <xref:Microsoft.Maui.Controls.Maps.Map> class defines a `IsZoomEnabled` property of type `bool`. By default this property is `true`, which indicates that zoom can be performed on the map. When this property is set to `false`, the map can't be zoomed:

```xaml
<maps:Map IsZoomEnabled="false" />
```

The equivalent C# code is:

```csharp
Map map = new Map
{
    IsZoomEnabled = false
};
```

#### Show the user's location

The <xref:Microsoft.Maui.Controls.Maps.Map> class defines a `IsShowingUser` property of type `bool`. By default this property is `false`, which indicates that the map is not showing the user's current location. When this property is set to `true`, the map shows the user's current location:

```xaml
<maps:Map IsShowingUser="true" />
```

The equivalent C# code is:

```csharp
Map map = new Map
{
    IsShowingUser = true
};
```

> [!IMPORTANT]
> Accessing the user's location requires location permissions to have been granted to the application. For more information, see [Platform configuration](#platform-configuration).

#### Map clicks

The <xref:Microsoft.Maui.Controls.Maps.Map> class defines a `MapClicked` event that's fired when the map is tapped. The `MapClickedEventArgs` object that accompanies the event has a single property named `Location`, of type `Location`. When the event is fired, the `Location` property is set to the map location that was tapped. For information about the `Location` class, see [Location and distance](#location-and-distance).

The following code example shows an event handler for the `MapClicked` event:

```csharp
void OnMapClicked(object sender, MapClickedEventArgs e)
{
    System.Diagnostics.Debug.WriteLine($"MapClick: {e.Location.Latitude}, {e.Location.Longitude}");
}
```

In this example, the `OnMapClicked` event handler outputs the latitude and longitude that represents the tapped map location. The event handler must be registered with the `MapClicked` event:

```xaml
<maps:Map MapClicked="OnMapClicked" />
```

The equivalent C# code is:

```csharp
Map map = new Map();
map.MapClicked += OnMapClicked;
```

## Location and distance

The `Microsoft.Maui.Devices.Sensors` namespace contains a `Location` class that's typically used when positioning a map and its pins. The `Microsoft.Maui.Maps` namespace contains a `Distance` struct that can optionally be used when positioning a map.

### Location

The `Location` class encapsulates a location stored as latitude and longitude values. This class defines the following properties:

- `Accuracy`, of type `double?`, which represents the horizontal accuracy of the `Location`, in meters.
- `Altitude`, of type `double?`, which represents the altitude in meters in a reference system that's specified by the `AltitudeReferenceSystem` property.
- `AltitudeReferenceSystem`, of type `AltitudeReferenceSystem`, which specifies the reference system in which the altitude value is provided.
- `Course`, of type `double?`, which indicates the degrees value relative to true north.
- `IsFromMockProvider`, of type `bool`, which indicates if the location is from the GPS or from a mock location provider.
- `Latitude`, of type `double`, which represents the latitude of the location in decimal degrees.
- `Longitude`, of type `double`, which represents the longitude of the location in decimal degrees.
- `Speed`, of type `double?`, which represents the speed in meters per second.
- `Timestamp`, of type `DateTimeOffset`, which represents the timestamp when the `Location` was created.
- `VerticalAccuracy`, of type `double?`, which specifies the vertical accuracy of the `Location`, in meters.

`Location` objects are created with one of the `Location` constructor overloads, which typically require at a minimum latitude and longitude arguments specified as `double` values:

```csharp
Location location = new Location(36.9628066, -122.0194722);
```

When creating a `Location` object, the latitude value will be clamped between -90.0 and 90.0, and the longitude value will be clamped between -180.0 and 180.0.

> [!NOTE]
> The `GeographyUtils` class has a `ToRadians` extension method that converts a `double` value from degrees to radians, and a `ToDegrees` extension method that converts a `double` value from radians to degrees.

The `Location` class also has `CalculateDistance` methods that calculate the distance between two locations.

### Distance

The `Distance` struct encapsulates a distance stored as a `double` value, which represents the distance in meters. This struct defines three read-only properties:

- `Kilometers`, of type `double`, which represents the distance in kilometers that's spanned by the `Distance`.
- `Meters`, of type `double`, which represents the distance in meters that's spanned by the `Distance`.
- `Miles`, of type `double`, which represents the distance in miles that's spanned by the `Distance`.

`Distance` objects can be created with the `Distance` constructor, which requires a meters argument specified as a `double`:

```csharp
Distance distance = new Distance(1450.5);
```

Alternatively, `Distance` objects can be created with the `FromKilometers`, `FromMeters`, `FromMiles`, and `BetweenPositions` factory methods:

```csharp
Distance distance1 = Distance.FromKilometers(1.45); // argument represents the number of kilometers
Distance distance2 = Distance.FromMeters(1450.5);   // argument represents the number of meters
Distance distance3 = Distance.FromMiles(0.969);     // argument represents the number of miles
Distance distance4 = Distance.BetweenPositions(location1, location2);
```

## Pins

The <xref:Microsoft.Maui.Controls.Maps.Map> control allows locations to be marked with `Pin` objects. A `Pin` is a map marker that opens an information window when tapped:

:::image type="content" source="media/map/pin-and-information-window.png" alt-text="Screenshot of a map pin and its information window.":::

When a `Pin` object is added to the `Map.Pins` collection, the pin is rendered on the map.

The `Pin` class has the following properties:

- `Address`, of type `string`, which typically represents the address for the pin location. However, it can be any `string` content, not just an address.
- <xref:Microsoft.Maui.Controls.Label>, of type `string`, which typically represents the pin title.
- `Location`, of type `Location`, which represents the latitude and longitude of the pin.
- `Type`, of type `PinType`, which represents the type of pin.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means a `Pin` can be the target of data bindings. For more information about data binding `Pin` objects, see [Display a pin collection](#display-a-pin-collection).

In addition, the `Pin` class defines `MarkerClicked` and `InfoWindowClicked` events. The `MarkerClicked` event is fired when a pin is tapped, and the `InfoWindowClicked` event is fired when the information window is tapped. The `PinClickedEventArgs` object that accompanies both events has a single `HideInfoWindow` property, of type `bool`.

### Display a pin

A `Pin` can be added to a <xref:Microsoft.Maui.Controls.Maps.Map> in XAML:

```xaml
<ContentPage ...
             xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps"
             xmlns:sensors="clr-namespace:Microsoft.Maui.Devices.Sensors;assembly=Microsoft.Maui.Essentials">
    <maps:Map x:Name="map">
        <x:Arguments>
            <maps:MapSpan>
                <x:Arguments>
                    <sensors:Location>
                        <x:Arguments>
                            <x:Double>36.9628066</x:Double>
                            <x:Double>-122.0194722</x:Double>
                        </x:Arguments>
                    </sensors:Location>
                    <x:Double>0.01</x:Double>
                    <x:Double>0.01</x:Double>
                </x:Arguments>
            </maps:MapSpan>
        </x:Arguments>
        <maps:Map.Pins>
            <maps:Pin Label="Santa Cruz"
                      Address="The city with a boardwalk"
                      Type="Place">
                <maps:Pin.Location>
                    <sensors:Location>
                        <x:Arguments>
                            <x:Double>36.9628066</x:Double>
                            <x:Double>-122.0194722</x:Double>
                        </x:Arguments>
                    </sensors:Location>
                </maps:Pin.Location>
            </maps:Pin>
        </maps:Map.Pins>
    </maps:Map>
</ContentPage>
```

This XAML creates a <xref:Microsoft.Maui.Controls.Maps.Map> object that shows the region that is specified by the `MapSpan` object. The `MapSpan` object is centered on the latitude and longitude represented by a `Location` object, which extends 0.01 latitude and longitude degrees. A `Pin` object is added to the `Map.Pins` collection, and drawn on the <xref:Microsoft.Maui.Controls.Maps.Map> at the location specified by its `Location` property. For information about the `Location` class, see [Location and distance](#location-and-distance). For information about passing arguments in XAML to objects that lack default constructors, see [Pass arguments in XAML](~/xaml/pass-arguments.md).

The equivalent C# code is:

```csharp
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
...

Map map = new Map
{
  ...
};

Pin pin = new Pin
{
  Label = "Santa Cruz",
  Address = "The city with a boardwalk",
  Type = PinType.Place,
  Location = new Location(36.9628066, -122.0194722)
};
map.Pins.Add(pin);
```

This example code results in a single pin being rendered on a map:

:::image type="content" source="media/map/pin-only.png" alt-text="Screenshot of a map pin.":::

### Interact with a pin

By default, when a `Pin` is tapped its information window is displayed:

:::image type="content" source="media/map/pin-and-information-window.png" alt-text="Screenshot of a map pin and its information window.":::

Tapping elsewhere on the map closes the information window.

The `Pin` class defines a `MarkerClicked` event, which is fired when a `Pin` is tapped. It's not necessary to handle this event to display the information window. Instead, this event should be handled when there's a requirement to be notified that a specific pin has been tapped.

The `Pin` class also defines a `InfoWindowClicked` event that's fired when an information window is tapped. This event should be handled when there's a requirement to be notified that a specific information window has been tapped.

The following code shows an example of handling these events:

```csharp
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
...

Pin boardwalkPin = new Pin
{
    Location = new Location(36.9641949, -122.0177232),
    Label = "Boardwalk",
    Address = "Santa Cruz",
    Type = PinType.Place
};
boardwalkPin.MarkerClicked += async (s, args) =>
{
    args.HideInfoWindow = true;
    string pinName = ((Pin)s).Label;
    await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
};

Pin wharfPin = new Pin
{
    Location = new Location(36.9571571, -122.0173544),
    Label = "Wharf",
    Address = "Santa Cruz",
    Type = PinType.Place
};
wharfPin.InfoWindowClicked += async (s, args) =>
{
    string pinName = ((Pin)s).Label;
    await DisplayAlert("Info Window Clicked", $"The info window was clicked for {pinName}.", "Ok");
};
```

The `PinClickedEventArgs` object that accompanies both events has a single `HideInfoWindow` property, of type `bool`. When this property is set to `true` inside an event handler, the information window will be hidden.

### Pin types

`Pin` objects include a `Type` property, of type `PinType`, which represents the type of pin. The `PinType` enumeration defines the following members:

- `Generic`, represents a generic pin.
- `Place`, represents a pin for a place.
- `SavedPin`, represents a pin for a saved location.
- `SearchResult`, represents a pin for a search result.

However, setting the `Pin.Type` property to any `PinType` member does not change the appearance of the rendered pin. Instead, you must customize the `Pin` handler to customize pin appearance. For more information about handler customization, see [Customize controls with handlers](~/user-interface/handlers/customize.md).

### Display a pin collection

The <xref:Microsoft.Maui.Controls.Maps.Map> class defines the following bindable properties:

- `ItemsSource`, of type `IEnumerable`, which specifies the collection of `IEnumerable` pin items to be displayed.
- `ItemTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, which specifies the <xref:Microsoft.Maui.Controls.DataTemplate> to apply to each item in the collection of displayed pins.
- `ItemTemplateSelector`, of type <xref:Microsoft.Maui.Controls.DataTemplateSelector>, which specifies the <xref:Microsoft.Maui.Controls.DataTemplateSelector> that will be used to choose a <xref:Microsoft.Maui.Controls.DataTemplate> for a pin at runtime.

> [!IMPORTANT]
> The `ItemTemplate` property takes precedence when both the `ItemTemplate` and `ItemTemplateSelector` properties are set.

A <xref:Microsoft.Maui.Controls.Maps.Map> can be populated with pins by using data binding to bind its `ItemsSource` property to an `IEnumerable` collection:

```xaml
<ContentPage ...
             xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps">    
    <Grid>
        ...
        <maps:Map x:Name="map"
                  ItemsSource="{Binding Positions}">
            <maps:Map.ItemTemplate>
                <DataTemplate>
                    <maps:Pin Location="{Binding Location}"
                              Address="{Binding Address}"
                              Label="{Binding Description}" />
                </DataTemplate>    
            </maps:Map.ItemTemplate>
        </maps:Map>
        ...
    </Grid>
</ContentPage>
```

The `ItemsSource` property data binds to the `Positions` property of the connected viewmodel, which returns an `ObservableCollection` of `Position` objects, which is a custom type. Each `Position` object defines `Address` and `Description` properties, of type `string`, and a `Location` property, of type `Location`.

The appearance of each item in the `IEnumerable` collection is defined by setting the `ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate> that contains a `Pin` object that data binds to appropriate properties.

The following screenshot shows a <xref:Microsoft.Maui.Controls.Maps.Map> displaying a `Pin` collection using data binding:

:::image type="content" source="media/map/pins-itemssource.png" alt-text="Screenshot of map with data bound pins.":::

#### Choose item appearance at runtime

The appearance of each item in the `IEnumerable` collection can be chosen at runtime, based on the item value, by setting the `ItemTemplateSelector` property to a <xref:Microsoft.Maui.Controls.DataTemplateSelector>:

```xaml
<ContentPage ...
             xmlns:templates="clr-namespace:WorkingWithMaps.Templates"
             xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps">
    <ContentPage.Resources>
       <templates:MapItemTemplateSelector x:Key="MapItemTemplateSelector">
           <templates:MapItemTemplateSelector.DefaultTemplate>
               <DataTemplate>
                   <maps:Pin Location="{Binding Location}"
                             Address="{Binding Address}"
                             Label="{Binding Description}" />
               </DataTemplate>
           </templates:MapItemTemplateSelector.DefaultTemplate>
           <templates:MapItemTemplateSelector.SanFranTemplate>
               <DataTemplate>
                   <maps:Pin Location="{Binding Location}"
                             Address="{Binding Address}"
                             Label="Xamarin!" />
               </DataTemplate>
           </templates:MapItemTemplateSelector.SanFranTemplate>    
       </templates:MapItemTemplateSelector>
    </ContentPage.Resources>

    <Grid>
        ...
        <maps:Map x:Name="map"
                  ItemsSource="{Binding Positions}"
                  ItemTemplateSelector="{StaticResource MapItemTemplateSelector}">
        ...
    </Grid>
</ContentPage>
```

The following example shows the `MapItemTemplateSelector` class:

```csharp
using WorkingWithMaps.Models;

namespace WorkingWithMaps.Templates;

public class MapItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate DefaultTemplate { get; set; }
    public DataTemplate SanFranTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return ((Position)item).Address.Contains("San Francisco") ? SanFranTemplate : DefaultTemplate;
    }
}
```

The `MapItemTemplateSelector` class defines `DefaultTemplate` and `SanFranTemplate` <xref:Microsoft.Maui.Controls.DataTemplate> properties that are set to different data templates. The `OnSelectTemplate` method returns the `SanFranTemplate`, which displays "Xamarin" as a label when a `Pin` is tapped, when the item has an address that contains "San Francisco". When the item doesn't have an address that contains "San Francisco", the `OnSelectTemplate` method returns the `DefaultTemplate`.

> [!NOTE]
> A use case for this functionality is binding properties of subclassed `Pin` objects to different properties, based on the `Pin` sub-type.

For more information about data template selectors, see [Create a DataTemplateSelector](~/fundamentals/datatemplate.md#create-a-datatemplateselector).

## Polygons, polylines, and circles

`Polygon`, `Polyline`, and `Circle` elements allow you to highlight specific areas on a map. A `Polygon` is a fully enclosed shape that can have a stroke and fill color. A `Polyline` is a line that does not fully enclose an area. A `Circle` highlights a circular area of the map:

:::image type="content" source="media/map/polygon-polyline.png" alt-text="Polygon and polyline on a map."::: :::image type="content" source="media/map/circle.png" alt-text="Circle on a map.":::

The `Polygon`, `Polyline`, and `Circle` classes derive from the `MapElement` class, which exposes the following bindable properties:

- `StrokeColor` is a <xref:Microsoft.Maui.Graphics.Color> object that determines the line color.
- `StrokeWidth` is a `float` object that determines the line width.

The `Polygon` class defines an additional bindable property:

- `FillColor` is a <xref:Microsoft.Maui.Graphics.Color> object that determines the polygon's background color.

In addition, the `Polygon` and `Polyline` classes both define a `GeoPath` property, which is a list of `Location` objects that specify the points of the shape.

The `Circle` class defines the following bindable properties:

- `Center` is a `Location` object that defines the center of the circle, in latitude and longitude.
- `Radius` is a `Distance` object that defines the radius of the circle in meters, kilometers, or miles.
- `FillColor` is a <xref:Microsoft.Maui.Graphics.Color> property that determines the color within the circle perimeter.

### Create a polygon

A `Polygon` object can be added to a map by instantiating it and adding it to the map's `MapElements` collection:

```xaml
<ContentPage ...
             xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps"
             xmlns:sensors="clr-namespace:Microsoft.Maui.Devices.Sensors;assembly=Microsoft.Maui.Essentials">
    <maps:Map>
        <maps:Map.MapElements>
            <maps:Polygon StrokeColor="#FF9900"
                          StrokeWidth="8"
                          FillColor="#88FF9900">
                <maps:Polygon.Geopath>
                    <sensors:Location>
                        <x:Arguments>
                            <x:Double>47.6458676</x:Double>
                            <x:Double>-122.1356007</x:Double>
                        </x:Arguments>
                    </sensors:Location>
                    <sensors:Location>
                        <x:Arguments>
                            <x:Double>47.6458097</x:Double>
                            <x:Double>-122.142789</x:Double>
                        </x:Arguments>
                    </sensors:Location>
                    ...
                </maps:Polygon.Geopath>
            </maps:Polygon>
        </maps:Map.MapElements>
    </maps:Map>
</ContentPage>
```

The equivalent C# code is:

```csharp
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
...

Map map = new Map();

// Instantiate a polygon
Polygon polygon = new Polygon
{
    StrokeWidth = 8,
    StrokeColor = Color.FromArgb("#1BA1E2"),
    FillColor = Color.FromArgb("#881BA1E2"),
    Geopath =
    {
        new Location(47.6368678, -122.137305),
        new Location(47.6368894, -122.134655),
        ...
    }
};

// Add the polygon to the map's MapElements collection
map.MapElements.Add(polygon);
```

The `StrokeColor` and `StrokeWidth` properties are specified to set the polygon's outline. In this example, the `FillColor` property value matches the `StrokeColor` property value but has an alpha value specified to make it transparent, allowing the underlying map to be visible through the shape. The `GeoPath` property contains a list of `Location` objects defining the geographic coordinates of the polygon points. A `Polygon` object is rendered on the map once it has been added to the `MapElements` collection of the <xref:Microsoft.Maui.Controls.Maps.Map>.

> [!NOTE]
> A `Polygon` is a fully enclosed shape. The first and last points will automatically be connected if they do not match.

### Create a polyline

A `Polyline` object can be added to a map by instantiating it and adding it to the map's `MapElements` collection:

```xaml
<ContentPage ...
             xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps"
             xmlns:sensors="clr-namespace:Microsoft.Maui.Devices.Sensors;assembly=Microsoft.Maui.Essentials">
    <maps:Map>
        <maps:Map.MapElements>
            <maps:Polyline StrokeColor="Black"
                           StrokeWidth="12">
                <maps:Polyline.Geopath>
                    <sensors:Location>
                        <x:Arguments>
                            <x:Double>47.6381401</x:Double>
                            <x:Double>-122.1317367</x:Double>
                        </x:Arguments>
                    </sensors:Location>
                    <sensors:Location>
                        <x:Arguments>
                            <x:Double>47.6381473</x:Double>
                            <x:Double>-122.1350841</x:Double>
                        </x:Arguments>
                    </sensors:Location>
                    ...
                </maps:Polyline.Geopath>
            </maps:Polyline>
        </maps:Map.MapElements>
    </maps:Map>
</ContentPage>
```

The equivalent C# code is:

```csharp
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
...

Map map = new Map();

// instantiate a polyline
Polyline polyline = new Polyline
{
    StrokeColor = Colors.Blue,
    StrokeWidth = 12,
    Geopath =
    {
        new Location(47.6381401, -122.1317367),
        new Location(47.6381473, -122.1350841),
        ...
    }
};

// Add the Polyline to the map's MapElements collection
map.MapElements.Add(polyline);
```

The `StrokeColor` and `StrokeWidth` properties are specified to set the line appearance. The `GeoPath` property contains a list of `Location` objects defining the geographic coordinates of the polyline points. A `Polyline` object is rendered on the map once it has been added to the `MapElements` collection of the <xref:Microsoft.Maui.Controls.Maps.Map>.

### Create a circle

A `Circle` object can be added to a map by instantiating it and adding it to the map's `MapElements` collection:

```xaml
<ContentPage ...
             xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps"
             xmlns:sensors="clr-namespace:Microsoft.Maui.Devices.Sensors;assembly=Microsoft.Maui.Essentials">
    <maps:Map>
        <maps:Map.MapElements>
            <maps:Circle StrokeColor="#88FF0000"
                         StrokeWidth="8"
                         FillColor="#88FFC0CB">
                <maps:Circle.Center>
                    <sensors:Location>
                        <x:Arguments>
                            <x:Double>37.79752</x:Double>
                            <x:Double>-122.40183</x:Double>
                        </x:Arguments>
                    </sensors:Location>
                </maps:Circle.Center>
                <maps:Circle.Radius>
                    <maps:Distance>
                        <x:Arguments>
                            <x:Double>250</x:Double>
                        </x:Arguments>
                    </maps:Distance>
                </maps:Circle.Radius>
            </maps:Circle>             
        </maps:Map.MapElements>
    </maps:Map>
</ContentPage>
```

The equivalent C# code is:

```csharp
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

Map map = new Map();

// Instantiate a Circle
Circle circle = new Circle
{
    Center = new Location(37.79752, -122.40183),
    Radius = new Distance(250),
    StrokeColor = Color.FromArgb("#88FF0000"),
    StrokeWidth = 8,
    FillColor = Color.FromArgb("#88FFC0CB")
};

// Add the Circle to the map's MapElements collection
map.MapElements.Add(circle);
```

The location of the `Circle` on the Map is determined by the value of the `Center` and `Radius` properties. The `Center` property defines the center of the circle, in latitude and longitude, while the `Radius` property defines the radius of the circle in meters. The `StrokeColor` and `StrokeWidth` properties are specified to set the circle's outline. The `FillColor` property value specifies the color within the circle perimeter. In this example, both of the color values specify an alpha channel, allowing the underlying map to be visible through the circle. The `Circle` object is rendered on the map once it has been added to the `MapElements` collection of the <xref:Microsoft.Maui.Controls.Maps.Map>.

> [!NOTE]
> The `GeographyUtils` class has a `ToCircumferencePositions` extension method that converts a `Circle` object (that defines `Center` and `Radius` property values) to a list of `Location` objects that make up the latitude and longitude coordinates of the circle perimeter.

## Geocoding and geolocation

The `Geocoding` class, in the `Microsoft.Maui.Devices.Sensors` namespace, can be used to geocode a placemark to positional coordinates and reverse geocode coordinates to a placemark. For more information, see [Geocoding](~/platform-integration/device/geocoding.md).

The `Geolocation` class, in the `Microsoft.Maui.Devices.Sensors` namespace, can be used to retrieve the device's current geolocation coordinates. For more information, see [Geolocation](~/platform-integration/device/geolocation.md).

## Launch the native map app

The native map app on each platform can be launched from a .NET MAUI app by the `Launcher` class. This class enables an app to open another app through its custom URI scheme. The launcher functionality can be invoked with the `OpenAsync` method, passing in a `string` or `Uri` argument that represents the custom URL scheme to open. For more information about the `Launcher` class, see [Launcher](~/platform-integration/appmodel/launcher.md).

> [!NOTE]
> An alternative to using the `Launcher` class is to use <xref:Microsoft.Maui.Controls.Maps.Map> class from the `Microsoft.Maui.ApplicationModel` namespace. For more information, see [Map](~/platform-integration/appmodel/maps.md).

The maps app on each platform uses a unique custom URI scheme. For information about the maps URI scheme on iOS, see [Map Links](https://developer.apple.com/library/archive/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html) on developer.apple.com. For information about the maps URI scheme on Android, see [Maps Developer Guide](https://developer.android.com/guide/components/intents-common.html#Maps) and [Google Maps Intents for Android](https://developers.google.com/maps/documentation/urls/android-intents) on developers.android.com. For information about the maps URI scheme on Windows, see [Launch the Windows Maps app](/windows/uwp/launch-resume/launch-maps-app).

### Launch the map app at a specific location

A location in the native maps app can be opened by adding appropriate query parameters to the custom URI scheme for each map app:

```csharp
if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
{
    // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
    await Launcher.OpenAsync("http://maps.apple.com/?q=394+Pacific+Ave+San+Francisco+CA");
}
else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
{
    // opens the Maps app directly
    await Launcher.OpenAsync("geo:0,0?q=394+Pacific+Ave+San+Francisco+CA");
}
else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
{
    await Launcher.OpenAsync("bingmaps:?where=394 Pacific Ave San Francisco CA");
}
```

This example code results in the native map app being launched on each platform, with the map centered on a pin representing the specified location.

### Launch the map app with directions

The native maps app can be launched displaying directions, by adding appropriate query parameters to the custom URI scheme for each map app:

```csharp
if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
{
    // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
    await Launcher.OpenAsync("http://maps.apple.com/?daddr=San+Francisco,+CA&saddr=cupertino");
}
else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
{
    // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
    await Launcher.OpenAsync("http://maps.google.com/?daddr=San+Francisco,+CA&saddr=Mountain+View");
}
else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
{
    await Launcher.OpenAsync("bingmaps:?rtp=adr.394 Pacific Ave San Francisco CA~adr.One Microsoft Way Redmond WA 98052");
}
```

This example code results in the native map app being launched on each platform, with the map centered on a route between the specified locations.
