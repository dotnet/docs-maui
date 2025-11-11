---
title: "Enable the safe area layout guide on iOS"
description: "How to control the iOS safe area layout guide in .NET MAUI, and .NET 10 guidance for modern safe area behavior."
ms.date: 08/18/2025
---

# Enable the safe area layout guide on iOS

By default, .NET Multi-platform App UI (.NET MAUI) apps automatically position page content on an area of the screen that is safe for all devices. This is known as the safe area layout guide, and ensures that content isn't clipped by rounded device corners, the home indicator, or the sensor housing on some iPhone models.

::: moniker range=">=net-maui-10.0"

> [!IMPORTANT]
> In .NET 10, use the <xref:Microsoft.Maui.Controls.ContentPage.SafeAreaEdges> property (also available on <xref:Microsoft.Maui.Controls.Layout>, <xref:Microsoft.Maui.Controls.ScrollView>, <xref:Microsoft.Maui.Controls.ContentView>, and <xref:Microsoft.Maui.Controls.Border>) to precisely control safe area behavior. The iOS-specific `Page.UseSafeArea` remains for existing apps, but new code should prefer `SafeAreaEdges`.

## Modern safe area guidance (.NET 10+)

### Choose the safe area behavior with SafeAreaEdges

`SafeAreaEdges` accepts the following values from <xref:Microsoft.Maui.SafeAreaEdges>: `Default`, `None`, `Container`, `SoftInput`, and `All`.

- Keep content within all safe areas (avoid bars, notch, and keyboard):

    ```xaml
    <ContentPage SafeAreaEdges="All">
            <!-- content stays within safe area -->
    </ContentPage>
    ```

- Draw edge-to-edge (no safe area padding):

    ```xaml
    <ContentPage SafeAreaEdges="None">
            <!-- content can extend behind bars and notch -->
    </ContentPage>
    ```

- Keep out of the keyboard (soft input), but allow under top/bottom bars/notch:

    ```xaml
    <ScrollView SafeAreaEdges="SoftInput">
            <!-- content can go under bars/notch but avoids keyboard overlap -->
    </ScrollView>
    ```

- Keep out of top/bottom bars and notch, but allow under the keyboard:

    ```xaml
    <Grid SafeAreaEdges="Container">
            <!-- content avoids bars/notch; may extend under keyboard -->
    </Grid>
    ```

The default value is `Default`, which maps to edge-to-edge behavior (equivalent to `None`) unless overridden by platform conventions.

You can set `SafeAreaEdges` in C# as well:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls;

// On a page
this.SafeAreaEdges = SafeAreaEdges.All;

// On a layout
myGrid.SafeAreaEdges = SafeAreaEdges.Container;

// Keyboard-aware
myScrollView.SafeAreaEdges = SafeAreaEdges.SoftInput;
```

### Reading the current safe area insets

To read the current safe area insets on a page at runtime, use the iOS-specific configuration API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

var insets = On<iOS>().SafeAreaInsets(); // Microsoft.Maui.Thickness
```

> [!TIP]
> Safe area insets can change at runtime (for example, rotation or status bar changes). Update layout when insets change.

For more information, see:

- [Safe area layout](~/user-interface/safe-area.md)
- [Safe area enhancements in .NET 10](~/whats-new/dotnet-10.md#safearea-enhancements)

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
