---
title: "Safe area layout"
description: "Learn how to control safe area behavior in .NET MAUI using SafeAreaEdges to ensure content is positioned correctly on devices with notches, rounded corners, and other screen features."
ms.date: 10/12/2025
---

# Safe area layout

The safe area is the region of the screen that is guaranteed to be visible and not obscured by device-specific features such as rounded corners, notches, sensor housings, system bars, or on-screen keyboards. .NET Multi-platform App UI (.NET MAUI) provides the `SafeAreaEdges` property to give you precise control over how your content interacts with these safe area regions.

::: moniker range=">=net-maui-10.0"

## SafeAreaEdges property

The <xref:Microsoft.Maui.Controls.ContentPage.SafeAreaEdges> property, available in .NET 10 and later, provides fine-grained control over safe area behavior. This property is available on the following controls:

- <xref:Microsoft.Maui.Controls.ContentPage>
- <xref:Microsoft.Maui.Controls.Layout> (and all derived layouts: <xref:Microsoft.Maui.Controls.Grid>, <xref:Microsoft.Maui.Controls.StackLayout>, <xref:Microsoft.Maui.Controls.AbsoluteLayout>, <xref:Microsoft.Maui.Controls.FlexLayout>, etc.)
- <xref:Microsoft.Maui.Controls.ScrollView>
- <xref:Microsoft.Maui.Controls.ContentView>
- <xref:Microsoft.Maui.Controls.Border>

The `SafeAreaEdges` property accepts values from the <xref:Microsoft.Maui.SafeAreaEdges> enum, which provides granular control over which safe area insets should be respected.

## SafeAreaEdges enum

The <xref:Microsoft.Maui.SafeAreaEdges> enum defines the following values:

| Value | Description |
|-------|-------------|
| `None` | Edge-to-edge content with no safe area padding. Content can extend behind system bars, notches, and the keyboard. |
| `SoftInput` | Respect only the soft input (keyboard) safe area. Content flows under system bars and notches but avoids overlapping the keyboard. |
| `Container` | Respect container safe areas (system bars, notches, rounded corners) but allow content to extend under the keyboard. |
| `Default` | Platform default safe area behavior. Currently maps to edge-to-edge (`None`) behavior unless overridden by platform conventions. |
| `All` | Respect all safe area insets including system bars, notches, rounded corners, and the keyboard. |

## Usage examples

### Edge-to-edge content

To create edge-to-edge content that extends behind system UI elements:

```xaml
<ContentPage SafeAreaEdges="None">
    <Grid>
        <Image Source="background.jpg" 
               Aspect="AspectFill" />
        <VerticalStackLayout Padding="20"
                             VerticalOptions="End">
            <Label Text="Overlay content"
                   TextColor="White"
                   FontSize="24" />
        </VerticalStackLayout>
    </Grid>
</ContentPage>
```

In C#:

```csharp
var page = new ContentPage
{
    SafeAreaEdges = SafeAreaEdges.None,
    Content = new Grid
    {
        Children =
        {
            new Image { Source = "background.jpg", Aspect = Aspect.AspectFill },
            new VerticalStackLayout
            {
                Padding = new Thickness(20),
                VerticalOptions = LayoutOptions.End,
                Children =
                {
                    new Label 
                    { 
                        Text = "Overlay content", 
                        TextColor = Colors.White, 
                        FontSize = 24 
                    }
                }
            }
        }
    }
};
```

### Respect all safe areas

To keep content within all safe areas:

```xaml
<ContentPage SafeAreaEdges="All">
    <VerticalStackLayout Padding="20">
        <Label Text="Safe content area"
               FontSize="18" />
        <Entry Placeholder="Enter text here" />
        <Button Text="Submit" />
    </VerticalStackLayout>
</ContentPage>
```

This ensures your content is never obscured by system bars, notches, or the keyboard.

### Keyboard-aware layouts

For forms and input-focused pages, use `SoftInput` to avoid keyboard overlap while allowing content under system bars:

```xaml
<ContentPage>
    <ScrollView SafeAreaEdges="SoftInput">
        <VerticalStackLayout Padding="20" Spacing="10">
            <Label Text="User Profile" FontSize="24" />
            <Entry Placeholder="Name" />
            <Entry Placeholder="Email" />
            <Entry Placeholder="Phone" />
            <Editor Placeholder="Bio" HeightRequest="100" />
            <Button Text="Save" />
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

### Immersive scrolling content

For scrollable content that should extend edge-to-edge but respect system UI:

```xaml
<ContentPage SafeAreaEdges="Container">
    <ScrollView>
        <VerticalStackLayout Spacing="10">
            <Image Source="header.jpg" 
                   Aspect="AspectFill" 
                   HeightRequest="300" />
            <VerticalStackLayout Padding="20">
                <Label Text="Article Title" FontSize="28" />
                <Label Text="Article content..." />
            </VerticalStackLayout>
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

### Per-layout control

You can set `SafeAreaEdges` on individual layouts within a page:

```xaml
<ContentPage SafeAreaEdges="None">
    <Grid RowDefinitions="Auto,*,Auto">
        <!-- Header extends edge-to-edge -->
        <Grid BackgroundColor="Primary">
            <Label Text="App Header" 
                   TextColor="White"
                   Margin="20,40,20,20" />
        </Grid>
        
        <!-- Main content respects safe areas -->
        <ScrollView Grid.Row="1" 
                    SafeAreaEdges="All">
            <VerticalStackLayout Padding="20">
                <Label Text="Main content" />
            </VerticalStackLayout>
        </ScrollView>
        
        <!-- Footer respects only keyboard -->
        <Grid Grid.Row="2" 
              SafeAreaEdges="SoftInput"
              BackgroundColor="LightGray"
              Padding="20">
            <Entry Placeholder="Type a message..." />
        </Grid>
    </Grid>
</ContentPage>
```

## Platform-specific behavior

### iOS and Mac Catalyst

On iOS and Mac Catalyst:

- Safe area insets include the status bar, navigation bar, tab bar, notch/Dynamic Island, home indicator, and rounded corners
- The `SoftInput` region includes the keyboard when visible
- Safe area insets automatically adjust during device rotation and when system UI visibility changes

### Android

On Android:

- Safe area insets include system bars (status bar, navigation bar), display cutouts (notches), and rounded corners
- The `SoftInput` region includes the soft keyboard
- Behavior can vary based on edge-to-edge display settings and Android version

### Windows

On Windows:

- Safe area insets typically include the title bar
- The `SoftInput` region includes the on-screen keyboard when visible
- Safe areas are generally less prominent but still respected

## Reading safe area insets

To read the current safe area insets on iOS at runtime, use the iOS-specific configuration API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

var insets = On<iOS>().SafeAreaInsets(); // Returns Microsoft.Maui.Thickness
```

> [!TIP]
> Safe area insets can change at runtime due to device rotation, status bar visibility changes, or keyboard appearance. Your layout should respond automatically to these changes.

## Best practices

1. **Choose the right value for your scenario**:
   - Use `All` for critical content like forms that must always be visible
   - Use `None` for immersive experiences like photo viewers or games
   - Use `Container` for scrollable content with fixed headers/footers
   - Use `SoftInput` for input-focused UIs like messaging apps

2. **Test on multiple devices**: Safe area behavior varies significantly across devices. Test on:
   - Devices with notches (iPhone X and later)
   - Devices with rounded corners
   - Tablets in landscape orientation
   - Devices with different aspect ratios

3. **Combine with padding**: `SafeAreaEdges` controls automatic safe area padding. You can still add your own padding for visual spacing:

   ```xaml
   <ContentPage SafeAreaEdges="All">
       <VerticalStackLayout Padding="20">
           <!-- Your content with both safe area and custom padding -->
       </VerticalStackLayout>
   </ContentPage>
   ```

4. **Use per-control settings**: Take advantage of being able to set `SafeAreaEdges` on individual controls to create sophisticated layouts where different sections have different safe area behavior.

## Migration from legacy APIs

If you're migrating from earlier versions of .NET MAUI:

### From iOS-specific Page.UseSafeArea

The iOS-specific `Page.UseSafeArea` property still works but is considered legacy. Migrate to `SafeAreaEdges`:

**Old approach (.NET 9 and earlier):**

```xaml
<ContentPage xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Page.UseSafeArea="True">
    <!-- Content -->
</ContentPage>
```

**New approach (.NET 10+):**

```xaml
<ContentPage SafeAreaEdges="All">
    <!-- Content -->
</ContentPage>
```

### From Layout.IgnoreSafeArea

The `Layout.IgnoreSafeArea` property still works but is less flexible. Migrate to `SafeAreaEdges`:

**Old approach:**

```xaml
<Grid IgnoreSafeArea="True">
    <!-- Content -->
</Grid>
```

**New approach:**

```xaml
<Grid SafeAreaEdges="None">
    <!-- Content -->
</Grid>
```

## See also

- [Safe area enhancements in .NET 10](~/whats-new/dotnet-10.md#safearea-enhancements)
- [iOS safe area layout guide](~/ios/platform-specifics/page-safe-area-layout.md)
- [ContentPage](~/user-interface/pages/contentpage.md)
- [Layouts](~/user-interface/layouts/index.md)

::: moniker-end

::: moniker range="<net-maui-10.0"

Safe area management in .NET MAUI versions earlier than .NET 10 is handled through iOS-specific platform APIs and the `Layout.IgnoreSafeArea` property. For information about these legacy approaches, see [Enable the safe area layout guide on iOS](~/ios/platform-specifics/page-safe-area-layout.md).

::: moniker-end
