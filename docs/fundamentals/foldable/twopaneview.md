---
title: "TwoPaneView foldable and large-screen layout control"
description: "Learn how to use the SemanticProperties class in a .NET MAUI app, so that a screen reader can speak about the user interface elements on a page."
monikerRange: ">= net-maui-7.0"
ms.date: 11/01/2022
---
# .NET MAUI TwoPaneView layout

[![Browse sample.](~/media/code-sample.png) Browse the sample](https://github.com/conceptdev/dotnet-maui-samples/blob)

The `TwoPaneView` class represents a container with two views that size and position content in the available space, either side-by-side or top-to-bottom. `TwoPaneView` inherits from `Grid` so the easiest way to think about these properties is as if they are being applied to a grid.

is provided by the [Microsoft.Maui.Controls.Foldable NuGet package](https://www.nuget.org/packages/Microsoft.Maui.Controls.Foldable/).

> [!IMPORTANT]
> The `TwoPaneView` control only adapts to Android foldable devices that support the Jetpack Window Manager API provided by Google (such as Microsoft Surface Duo). On all other platforms and devices it acts like a configurable and responsive split view that can dynamically show one or two panes, proportionally sized on the screen.

## Set up TwoPaneView

To add the TwoPaneView layout to your page:

1. Add a `foldable` namespace alias for the Foldable NuGet:

    ```xaml
    xmlns:foldable="clr-namespace:Microsoft.Maui.Controls.Foldable;assembly=Microsoft.Maui.Controls.Foldable"
    ```

2. Add the `TwoPaneView` as the root element on the page, and add controls to `Pane1` and `Pane2`:

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

## Control TwoPaneView when it's only on one screen

The following properties apply when the `TwoPaneView` is occupying a single screen:

- `MinTallModeHeight` indicates the minimum height the control must be to enter tall mode.
- `MinWideModeWidth` indicates the minimum width the control must be to enter wide mode.
- `Pane1Length` sets the width of Pane1 in Wide mode, the height of Pane1 in Tall mode, and has no effect in SinglePane mode.
- `Pane2Length` sets the width of Pane2 in Wide mode, the height of Pane2 in Tall mode, and has no effect in SinglePane mode.

> [!IMPORTANT]
> If the `TwoPaneView` is spanned across a hinge or fold these properties have no effect.

## Properties that apply when on one screen or two

The following properties apply when the `TwoPaneView` is occupying a single screen or two screens:

- `TallModeConfiguration` indicates, when in tall mode, the Top/Bottom arrangement or if you only want a single pane visible as defined by the TwoPaneViewPriority.
- `WideModeConfiguration` indicates, when in wide mode, the Left/Right arrangement or if you only want a single pane visible as defined by the TwoPaneViewPriority.
- `PanePriority` determines whether to show Pane1 or Pane2 if in SinglePane mode.
