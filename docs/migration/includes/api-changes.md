---
ms.topic: include
ms.date: 08/30/2023
---

## API changes

Some APIs have changed in the move from Xamarin.Forms to .NET MAUI. This is multiple reasons including removing duplicate functionality caused by Xamarin.Essentials becoming part of .NET MAUI, and ensuring that APIs follow .NET naming guidelines. The following sections discuss these changes.

### Color changes

In Xamarin.Forms, the `Xamarin.Forms.Color` struct lets you construct <xref:Microsoft.Maui.Graphics.Color> objects using `double` values, and provides named colors, such as `Xamarin.Forms.Color.AliceBlue`. In .NET MAUI, this functionality has been separated into the <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> class, and the <xref:Microsoft.Maui.Graphics.Colors?displayProperty=fullName> class.

The <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> class, in the <xref:Microsoft.Maui.Graphics> namespace, lets you construct <xref:Microsoft.Maui.Graphics.Color> objects using `float` values, `byte` values, and `int` values. The <xref:Microsoft.Maui.Graphics.Colors?displayProperty=fullName> class, which is also in the <xref:Microsoft.Maui.Graphics> namespace, largely provides the same named colors. For example, use <xref:Microsoft.Maui.Graphics.Colors.AliceBlue?displayProperty=nameWithType> to specify the `AliceBlue` color.

The following table shows the API changes between the `Xamarin.Forms.Color` struct and the <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> class:

| Xamarin.Forms API | .NET MAUI API | Comment |
| ----------------- | ------------- | ------- |
| `Xamarin.Forms.Color.R` | <xref:Microsoft.Maui.Graphics.Color.Red?displayProperty=fullName> | |
| `Xamarin.Forms.Color.G` | <xref:Microsoft.Maui.Graphics.Color.Green?displayProperty=fullName> | |
| `Xamarin.Forms.Color.B` | <xref:Microsoft.Maui.Graphics.Color.Blue?displayProperty=fullName> | |
| `Xamarin.Forms.Color.A` | <xref:Microsoft.Maui.Graphics.Color.Alpha?displayProperty=fullName> | |
| `Xamarin.Forms.Color.Hue` | <xref:Microsoft.Maui.Graphics.Color.GetHue%2A?displayProperty=fullName> | Xamarin.Forms property replaced with a method in .NET MAUI. |
| `Xamarin.Forms.Color.Saturation` | <xref:Microsoft.Maui.Graphics.Color.GetSaturation%2A?displayProperty=fullName> | Xamarin.Forms property replaced with a method in .NET MAUI. |
| `Xamarin.Forms.Color.Luminosity` | <xref:Microsoft.Maui.Graphics.Color.GetLuminosity%2A?displayProperty=fullName> | Xamarin.Forms property replaced with a method in .NET MAUI. |
| `Xamarin.Forms.Color.Default` | | No .NET MAUI equivalent. Instead, <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> objects default to `null`. |
| `Xamarin.Forms.Color.Accent` |  | No .NET MAUI equivalent. |
| `Xamarin.Forms.Color.FromHex` | <xref:Microsoft.Maui.Graphics.Color.FromArgb%2A?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.FromHex%2A?displayProperty=fullName> is obsolete and will be removed in a future release. |

In addition, all of the numeric values in a <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> are `float`, rather than `double` as used in `Xamarin.Forms.Color`.

> [!NOTE]
> Unlike Xamarin.Forms, a <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> doesn't have an implicit conversion to <xref:System.Drawing.Color?displayProperty=fullName>.

### Layout changes

The following table lists the layout APIs that have been removed in the move from Xamarin.Forms to .NET MAUI:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms API | .NET MAUI API | Comments |
> | ----------------- | ------------- | -------- |
> | `Xamarin.Forms.AbsoluteLayout.IAbsoluteList<T>.Add` |  | The `Add` overload that accepts 3 arguments isn't present in .NET MAUI. |
> | `Xamarin.Forms.Grid.IGridList<T>.AddHorizontal` |  | No .NET MAUI equivalent. |
> | `Xamarin.Forms.Grid.IGridList<T>.AddVertical` |  | No .NET MAUI equivalent. |
> | `Xamarin.Forms.RelativeLayout` | <xref:Microsoft.Maui.Controls.Compatibility.RelativeLayout?displayProperty=fullName> | In .NET MAUI, `RelativeLayout` only exists as a compatibility control for users migrating from Xamarin.Forms. Use <xref:Microsoft.Maui.Controls.Grid> instead, or add the `xmlns` for the compatibility namespace. |

In addition, adding children to a layout in code in Xamarin.Forms is accomplished by adding the children to the layout's `Children` collection:

```csharp
Grid grid = new Grid();
grid.Children.Add(new Label { Text = "Hello world" });
```

In .NET MAUI, the <xref:Microsoft.Maui.Controls.Layout.Children> collection is for internal use by .NET MAUI and shouldn't be manipulated directly. Therefore, in code children should be added directly to the layout:

```csharp
Grid grid = new Grid();
grid.Add(new Label { Text = "Hello world" });
```

> [!IMPORTANT]
> Any `Add` layout extension methods, such as <xref:Microsoft.Maui.Controls.GridExtensions.Add%2A?displayProperty=nameWithType>, are invoked on the layout rather than the layouts <xref:Microsoft.Maui.Controls.Layout.Children> collection.

You may notice when running your upgraded .NET MAUI app that layout behavior is different. For more information, see [Layout behavior changes from Xamarin.Forms](../layouts.md).

### Custom layout changes

The process for creating a custom layout in Xamarin.Forms involves creating a class that derives from `Layout<View>`, and overriding the `VisualElement.OnMeasure` and `Layout.LayoutChildren` methods. For more information, see [Create a custom layout in Xamarin.Forms](/xamarin/xamarin-forms/user-interface/layouts/custom).

In .NET MAUI, the layout classes derive from the abstract <xref:Microsoft.Maui.Controls.Layout> class. This class delegates cross-platform layout and measurement to a layout manager class. Each layout manager class implements the <xref:Microsoft.Maui.Layouts.ILayoutManager> interface, which specifies that <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> and <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementations must be provided:

- The <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> implementation calls <xref:Microsoft.Maui.IView.Measure%2A?displayProperty=nameWithType> on each view in the layout, and returns the total size of the layout given the constraints.
- The <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementation determines where each view should be placed within the bounds of the layout, and calls <xref:Microsoft.Maui.IView.Arrange%2A> on each view with its appropriate bounds. The return value is the actual size of the layout.

For more information, see [Custom layouts](~/user-interface/layouts/custom.md).

### Device changes

Xamarin.Forms has a `Xamarin.Forms.Device` class that helps you to interact with the device and platform the app is running on. The equivalent class in .NET MAUI, <xref:Microsoft.Maui.Controls.Device?displayProperty=fullName>, is deprecated and its functionality is replaced by multiple types.

The following table shows the .NET MAUI replacements for the functionality in the `Xamarin.Forms.Device` class:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms API | .NET MAUI API | Comments |
> | ----------------- | ------------- | -------- |
> | `Xamarin.Forms.Device.Android` | <xref:Microsoft.Maui.Devices.DevicePlatform.Android?displayProperty=fullName> |  |
> | `Xamarin.Forms.Device.iOS` | <xref:Microsoft.Maui.Devices.DevicePlatform.iOS?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.GTK` |  | No .NET MAUI equivalent. |
> | `Xamarin.Forms.Device.macOS` | | No .NET MAUI equivalent. Instead, use <xref:Microsoft.Maui.Devices.DevicePlatform.MacCatalyst?displayProperty=fullName>. |
> | `Xamarin.Forms.Device.Tizen` | <xref:Microsoft.Maui.Devices.DevicePlatform.Tizen?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.UWP` | <xref:Microsoft.Maui.Devices.DevicePlatform.WinUI?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.WPF` |  |  No .NET MAUI equivalent. |
> | `Xamarin.Forms.Device.Flags` | | No .NET MAUI equivalent. |
> | `Xamarin.Forms.Device.FlowDirection` | <xref:Microsoft.Maui.ApplicationModel.AppInfo.RequestedLayoutDirection?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.Idiom` | <xref:Microsoft.Maui.Devices.DeviceInfo.Idiom?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.IsInvokeRequired` | <xref:Microsoft.Maui.Dispatching.Dispatcher.IsDispatchRequired?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.OS` | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.RuntimePlatform` | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.BeginInvokeOnMainThread` | <xref:Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread%2A?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.GetMainThreadSynchronizationContextAsync` | <xref:Microsoft.Maui.ApplicationModel.MainThread.GetMainThreadSynchronizationContextAsync%2A?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.GetNamedColor` | | No .NET MAUI equivalent. |
> | `Xamarin.Forms.Device.GetNamedSize` | | No .NET MAUI equivalent.|
> | `Xamarin.Forms.Device.Invalidate`  | <xref:Microsoft.Maui.Controls.VisualElement.InvalidateMeasure%2A?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.InvokeOnMainThreadAsync` | <xref:Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync%2A?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.OnPlatform` | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.OpenUri` | <xref:Microsoft.Maui.ApplicationModel.Launcher.OpenAsync%2A?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.SetFlags` | | No .NET MAUI equivalent. |
> | `Xamarin.Forms.Device.SetFlowDirection` | <xref:Microsoft.Maui.Controls.Window.FlowDirection?displayProperty=fullName> | |
> | `Xamarin.Forms.Device.StartTimer` | <xref:Microsoft.Maui.Dispatching.DispatcherExtensions.StartTimer%2A?displayProperty=fullName> or <xref:Microsoft.Maui.Dispatching.Dispatcher.DispatchDelayed%2A?displayProperty=fullName> | |

### Map changes

In Xamarin.Forms, the `Map` control and associated types are in the `Xamarin.Forms.Maps` namespace. In .NET MAUI, this functionality has moved to the <xref:Microsoft.Maui.Controls.Maps> and <xref:Microsoft.Maui.Maps> namespaces. Some properties have been renamed and some types have been replaced with equivalent types from Xamarin.Essentials.

The following table shows the .NET MAUI replacements for the functionality in the `Xamarin.Forms.Maps` namespace:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms API | .NET MAUI API | Comment |
> | ----------------- | ------------- | ------- |
> | `Xamarin.Forms.Maps.Map.HasScrollEnabled` | <xref:Microsoft.Maui.Controls.Maps.Map.IsScrollEnabled?displayProperty=fullName> |  |
> | `Xamarin.Forms.Maps.Map.HasZoomEnabled` | <xref:Microsoft.Maui.Controls.Maps.Map.IsZoomEnabled?displayProperty=fullName> |  |
> | `Xamarin.Forms.Maps.Map.TrafficEnabled` | <xref:Microsoft.Maui.Controls.Maps.Map.IsTrafficEnabled?displayProperty=fullName> |  |
> | `Xamarin.Forms.Maps.Map.MoveToLastRegionOnLayoutChange` |  | No .NET MAUI equivalent. |
> | `Xamarin.Forms.Maps.Pin.Id` | <xref:Microsoft.Maui.Controls.Maps.Pin.MarkerId?displayProperty=fullName> |  |
> | `Xamarin.Forms.Maps.Pin.Position` | <xref:Microsoft.Maui.Controls.Maps.Pin.Location?displayProperty=fullName> |  |
> | `Xamarin.Forms.Maps.MapClickedEventArgs.Position` | <xref:Microsoft.Maui.Controls.Maps.MapClickedEventArgs.Location?displayProperty=fullName> |  |
> | `Xamarin.Forms.Maps.Position` | <xref:Microsoft.Maui.Devices.Sensors.Location?displayProperty=fullName> | Members of type `Xamarin.Forms.Maps.Position` have changed to the <xref:Microsoft.Maui.Devices.Sensors.Location?displayProperty=fullName> type. |
> | `Xamarin.Forms.Maps.Geocoder` | <xref:Microsoft.Maui.Devices.Sensors.Geocoding?displayProperty=fullName> | Members of type `Xamarin.Forms.Maps.Geocoder` have changed to the <xref:Microsoft.Maui.Devices.Sensors.Geocoding?displayProperty=fullName> type. |

.NET MAUI has two `Map` types - <xref:Microsoft.Maui.Controls.Maps.Map?displayProperty=fullName> and <xref:Microsoft.Maui.ApplicationModel.Map?displayProperty=fullName>. Because the <xref:Microsoft.Maui.ApplicationModel> namespace is one of .NET MAUI's `global using` directives, when using the <xref:Microsoft.Maui.Controls.Maps.Map?displayProperty=fullName> control from code you'll have to fully qualify your `Map` usage or use a [using alias](/dotnet/csharp/language-reference/keywords/using-directive#using-alias).

In XAML, an `xmlns` namespace definition should be added for the <xref:Microsoft.Maui.Controls.Maps.Map> control. While this isn't required, it prevents a collision between the `Polygon` and `Polyline` types, which exist in both the <xref:Microsoft.Maui.Controls.Maps> and <xref:Microsoft.Maui.Controls.Shapes> namespaces. For more information, see [Display a map](~/user-interface/controls/map.md#display-a-map).

### Other changes

A small number of other APIs have been consolidated in the move from Xamarin.Forms to .NET MAUI. The following table shows these changes:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms API | .NET MAUI API | Comments |
> | ----------------- | ------------- | -------- |
> | `Xamarin.Forms.Application.Properties` | <xref:Microsoft.Maui.Storage.Preferences?displayProperty=fullName> |  |
> | `Xamarin.Forms.Button.Image` | <xref:Microsoft.Maui.Controls.Button.ImageSource?displayProperty=fullName> |  |
> | `Xamarin.Forms.Frame.OutlineColor` | <xref:Microsoft.Maui.Controls.Frame.BorderColor?displayProperty=fullName> |  |
> | `Xamarin.Forms.IQueryAttributable.ApplyQueryAttributes` | <xref:Microsoft.Maui.Controls.IQueryAttributable.ApplyQueryAttributes%2A?displayProperty=fullName> | In Xamarin.Forms, the `ApplyQueryAttributes` method accepts an `IDictionary<string, string>` argument. In .NET MAUI, the `ApplyQueryAttributes` method accepts an `IDictionary<string, object>` argument.  |
> | `Xamarin.Forms.MenuItem.Icon` | <xref:Microsoft.Maui.Controls.MenuItem.IconImageSource?displayProperty=fullName> | `Xamarin.Forms.MenuItem.Icon` is the base class for `Xamarin.Forms.ToolbarItem`, and so `ToolbarItem.Icon` becomes `ToolbarItem.IconImageSource`. |
> | `Xamarin.Forms.OrientationStateTrigger.Orientation` | <xref:Microsoft.Maui.Controls.OrientationStateTrigger.Orientation?displayProperty=fullName> | In Xamarin.Forms, the `OrientationStateTrigger.Orientation` property is of type `Xamarin.Forms.Internals.DeviceOrientation`. In .NET MAUI, the `OrientationStateTrigger.Orientation` property is of type <xref:Microsoft.Maui.Devices.DisplayOrientation>. |
> | `Xamarin.Forms.OSAppTheme` | <xref:Microsoft.Maui.ApplicationModel.AppTheme?displayProperty=fullName> |  |
> | `Xamarin.Forms.Span.ForegroundColor` | <xref:Microsoft.Maui.Controls.Span.TextColor?displayProperty=fullName> |  |
> | `Xamarin.Forms.ToolbarItem.Name` | <xref:Microsoft.Maui.Controls.MenuItem.Text?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.MenuItem.Text?displayProperty=fullName> is the base class for <xref:Microsoft.Maui.Controls.ToolbarItem?displayProperty=fullName>, and so `ToolbarItem.Name` becomes `ToolbarItem.Text`. |

In addition, in Xamarin.Forms, the `Page.OnAppearing` override is called on Android when an app is backgrounded and then brought to the foreground. However, this override isn't called on iOS and Windows in the same scenario. In .NET MAUI, the <xref:Microsoft.Maui.Controls.Page.OnAppearing> override isn't called on any platforms when an app is backgrounded and then brought to the foreground. Instead, you should listen to lifecycle events on <xref:Microsoft.Maui.Controls.Window> to be notified when an app returns to the foreground. For more information, see [.NET MAUI windows](~/fundamentals/windows.md).

### Native forms changes

[Native forms](/xamarin/xamarin-forms/platform/native-forms) in Xamarin.Forms has become native embedding in .NET MAUI, and uses a different initialization approach and different extension methods to convert cross-platform controls to their native types. For more information, see [Native embedding](~/platform-integration/native-embedding.md).
