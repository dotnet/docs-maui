---
title: "Enable the safe area layout guide on iOS"
description: "How to control the iOS safe area layout guide in .NET MAUI, and .NET 10 guidance for modern safe area behavior."
ms.date: 08/18/2025
---

# Enable the safe area layout guide on iOS

By default, .NET Multi-platform App UI (.NET MAUI) apps automatically position page content on an area of the screen that is safe for all devices. This is known as the safe area layout guide, and ensures that content isn't clipped by rounded device corners, the home indicator, or the sensor housing on some iPhone models.

::: moniker range=">=net-maui-10.0"

> [!IMPORTANT]
> In .NET 10, MAUI continues to honor the iOS safe area by default. Prefer the `ISafeAreaView` pattern to opt a layout into drawing behind obstructions, and use page safe area insets when you need to react to device changes. The `iOS Page.UseSafeArea` platform-specific remains available for existing code, but for new apps we recommend the approach below.

## Modern safe area guidance (.NET 10+)

- To allow content to extend into the unsafe regions, set `IgnoreSafeArea` on any layout that implements <xref:Microsoft.Maui.ISafeAreaView>:

    ```xaml
    <Grid IgnoreSafeArea="True">
            <!-- content can extend into areas behind toolbars, cutouts, etc. -->
    </Grid>
    ```

- To read the current safe area insets on a Page at runtime, use the iOS-specific attached property via the platform configuration APIs:

    ```csharp
    using Microsoft.Maui.Controls.PlatformConfiguration;
    using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

    var insets = On<iOS>().SafeAreaInsets(); // returns Microsoft.Maui.Thickness
    ```

> [!TIP]
> Safe area insets can change at runtime (rotation, status bar changes). Handle layout updates accordingly.

::: moniker-end

::: moniker range="<=net-maui-9.0"

This iOS platform-specific enables the safe area layout guide, if it's previously been disabled, and is consumed in XAML by setting the `Page.UseSafeArea` attached property to `true`:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Page.UseSafeArea="True">
    <StackLayout>
        ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

On<iOS>().SetUseSafeArea(true);
```

The `Page.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Page.SetUseSafeArea` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, controls whether the safe area layout guide is disabled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Layout> class defines a <xref:Microsoft.Maui.Controls.Layout.IgnoreSafeArea> property that ensures that content is positioned on an area of the screen that is safe for all iOS devices. This property can be set to `true` on any layout class, such as a <xref:Microsoft.Maui.Controls.Grid> or <xref:Microsoft.Maui.Controls.StackLayout>, to perform the equivalent of this platform-specific.

::: moniker-end
