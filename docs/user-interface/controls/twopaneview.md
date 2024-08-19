---
title: "TwoPaneView foldable and large-screen layout control"
description: "Learn how to use the TwoPaneView control to create adaptive layouts that work on phones, tablets, desktop, and foldable devices."
author: conceptdev
ms.author: crdun
ms.date: 08/30/2024
---

# .NET MAUI TwoPaneView layout

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-controls-twopaneview/)

The <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> class represents a container with two views that size and position content in the available space, either side-by-side or top-to-bottom. <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> inherits from <xref:Microsoft.Maui.Controls.Grid> so the easiest way to think about these properties is as if they are being applied to a grid.

:::image type="content" source="media/twopaneview/foldable-maui-app.png" alt-text="Surface Duo dual-screen emulator showing a basic TwoPaneView test app":::

The layout control is provided by the [Microsoft.Maui.Controls.Foldable NuGet package](https://www.nuget.org/packages/Microsoft.Maui.Controls.Foldable/).

## Foldable device support overview

Foldable devices include the Microsoft Surface Duo and Android devices from other manufacturers. They bridge the gap between phones and larger screens like tablets and desktops because apps might need to adjust to a variety of screen sizes and orientations on the same device, including adapting to a hinge or fold in the screen.

Visit the [dual-screen developer docs](/dual-screen/) for more information about building apps that target foldable devices, including [design patterns and user experiences](/dual-screen/design/). There is also a [Surface Duo emulator](/dual-screen/android/emulator/) you can download for Windows, Mac, and Linux.

> [!IMPORTANT]
> The <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> control only adapts to Android foldable devices that support the Jetpack Window Manager API provided by Google (such as Microsoft Surface Duo).
>
> On all other platforms and devices (i.e. other Android devices, iOS, macOS, Windows) it acts like a configurable and responsive split view that can dynamically show one or two panes, proportionally sized on the screen.

## Add and configure the Foldable support NuGet

1. Open the **NuGet Package Manager** dialog for your solution.
2. Under the **Browse** tab, search for `Microsoft.Maui.Controls.Foldable`.
3. Install the `Microsoft.Maui.Controls.Foldable` package to your solution.
4. Add the `UseFoldable()` initialization method (and namespace) call to the project's `MauiApp` class, in the `CreateMauiApp` method:

    ```csharp
    using Microsoft.Maui.Foldable; // ADD THIS NAMESPACE
    ...
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        ...
        builder.UseFoldable(); // ADD THIS LINE TO THE TEMPLATE
        return builder.Build();
    }
    ```

    The `UseFoldable()` initialization is required for the app to be able to detect changes in the app's state, such as being spanned across a fold.

5. Update the `[Activity(...)]` attribute on the `MainActivity` class in *Platforms/Android*, so that it includes _all_ these `ConfigurationChanges` options:

    ```csharp
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize
        | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode
    ```

    These values are required so that configuration changes and span state can be more reliably reported for reliable dual-screen support.

## Set up TwoPaneView

To add the <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> layout to your page:

1. Add a `foldable` namespace alias for the Foldable NuGet:

    ```xaml
    xmlns:foldable="clr-namespace:Microsoft.Maui.Controls.Foldable;assembly=Microsoft.Maui.Controls.Foldable"
    ```

2. Add the <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> as the root element on the page, and add controls to `Pane1` and `Pane2`:

    ```xaml
    <foldable:TwoPaneView x:Name="twoPaneView">
        <foldable:TwoPaneView.Pane1
            BackgroundColor="#dddddd">
            <Label
                Text="Hello, .NET MAUI!"
                SemanticProperties.HeadingLevel="Level1"
                FontSize="32"
                HorizontalOptions="Center" />
        </foldable:TwoPaneView.Pane1>
        <foldable:TwoPaneView.Pane2>
            <StackLayout BackgroundColor="{AppThemeBinding Light={StaticResource Secondary}, Dark={StaticResource Primary}}">
                <Label Text="Pane2 StackLayout"/>
            </StackLayout>
        </foldable:TwoPaneView.Pane2>
    </foldable:TwoPaneView>
    ```

## Understand TwoPaneView modes

Only one of these modes can be active:

- `SinglePane` only one pane is currently visible.
- `Wide` the two panes are laid out horizontally. One pane is on the left and the other is on the right. When on two screens this is the mode when the device is portrait.
- `Tall` the two panes are laid out vertically. One pane is on top and the other is on bottom. When on two screens this is the mode when the device is landscape.

### Control TwoPaneView when it's only on one screen

The following properties apply when the <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> is occupying a single screen:

- `MinTallModeHeight` indicates the minimum height the control must be to enter `Tall` mode.
- `MinWideModeWidth` indicates the minimum width the control must be to enter `Wide` mode.
- `Pane1Length` sets the width of Pane1 in `Wide` mode, the height of `Pane1` in `Tall` mode, and has no effect in `SinglePane` mode.
- `Pane2Length` sets the width of `Pane2` in `Wide` mode, the height of `Pane2` in `Tall` mode, and has no effect in `SinglePane` mode.

> [!IMPORTANT]
> If the <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> is spanned across a hinge or fold these properties have no effect.

### Properties that apply when on one screen or two

The following properties apply when the <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> is occupying a single screen or two screens:

- `TallModeConfiguration` indicates, when in `Tall` mode, the Top/Bottom arrangement or if you only want a single pane visible as defined by the `TwoPaneViewPriority`.
- `WideModeConfiguration` indicates, when in `Wide` mode, the Left/Right arrangement or if you only want a single pane visible as defined by the `TwoPaneViewPriority`.
- `PanePriority` determines whether to show `Pane1` or `Pane2` if in `SinglePane` mode.

## Troubleshooting

If the <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> layout isn't working as expected, double-check the set-up instructions on this page. Omitting or misconfiguring the `UseFoldable()` method or the `ConfigurationChanges` attribute values are common causes of errors.
