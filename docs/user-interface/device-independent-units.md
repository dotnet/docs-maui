---
title: "Device-independent units"
description: "Learn about device-independent units in .NET MAUI and how they relate to platform-specific unit systems like iOS points and Android density-independent pixels."
ms.date: 12/19/2024
---

# Device-independent units

.NET Multi-platform App UI (.NET MAUI) uses **device-independent units** as its universal measurement system across all platforms. This unit system provides a consistent way to specify sizes, positions, margins, padding, and other measurements that automatically adapt to different screen densities and device types.

## What are device-independent units?

Device-independent units (DIUs) are a virtual unit of measurement that represents a consistent physical size across different devices and platforms, regardless of screen density or resolution. One device-independent unit is designed to represent approximately 1/160th of an inch (0.15875 mm) of physical screen space.

The key characteristics of device-independent units include:

- **Platform consistency**: The same value produces visually similar results across iOS, Android, Windows, and macOS
- **Density independence**: Automatically scales based on screen density to maintain physical size
- **Developer simplicity**: Use a single unit system instead of platform-specific calculations

## Relationship to platform-specific units

Device-independent units in .NET MAUI correspond directly to the native unit systems used by each platform:

### iOS - Points

On iOS, device-independent units map to **points** (pt):
- 1 device-independent unit = 1 iOS point
- Points automatically scale based on device pixel density
- iPhone standard density: 1 point = 1 pixel
- iPhone Retina displays: 1 point = 2-4 pixels (depending on the device)

### Android - Density-independent pixels

On Android, device-independent units map to **density-independent pixels** (dp or dip):
- 1 device-independent unit = 1 Android dp
- Based on 160 DPI as the baseline density
- Automatically scales: `pixels = dp × (dpi / 160)`
- Common scale factors: 1x (mdpi), 1.5x (hdpi), 2x (xhdpi), 3x (xxhdpi), 4x (xxxhdpi)

### Windows - Device-independent pixels

On Windows, device-independent units map to **device-independent pixels**:
- 1 device-independent unit = 1 Windows device-independent pixel
- Based on 96 DPI as the baseline
- Scales with system DPI settings and display scale factor

### macOS - Points

On macOS, device-independent units map to **points**:
- 1 device-independent unit = 1 macOS point
- Similar to iOS, points scale based on display density
- Standard displays: 1 point = 1 pixel
- Retina displays: 1 point = 2 pixels

## Usage in .NET MAUI

Device-independent units are used throughout .NET MAUI for various measurements:

### Layout and positioning

```xaml
<!-- Margin using device-independent units -->
<Label Text="Hello World" Margin="10,20,10,5" />

<!-- Width and height using device-independent units -->
<BoxView WidthRequest="100" HeightRequest="50" />

<!-- Padding using device-independent units -->
<StackLayout Padding="15">
    <Label Text="Content with padding" />
</StackLayout>
```

### Font sizes

```xaml
<!-- Font size in device-independent units -->
<Label Text="Large text" FontSize="24" />
```

The equivalent C# code:

```csharp
var label = new Label 
{ 
    Text = "Large text", 
    FontSize = 24  // 24 device-independent units
};
```

### Border and stroke widths

```xaml
<!-- Border width in device-independent units -->
<Border StrokeThickness="2">
    <Label Text="Bordered content" />
</Border>
```

### Animation and transforms

```csharp
// Translate by 100 device-independent units
await image.TranslateTo(100, 100, 1000);

// Scale maintains proportions across devices
await button.ScaleTo(1.5, 250);
```

## Practical examples

### Creating consistent spacing

```xaml
<StackLayout Spacing="16">
    <Label Text="Item 1" />
    <Label Text="Item 2" />
    <Label Text="Item 3" />
</StackLayout>
```

This creates 16 device-independent units of space between items, which translates to:
- **iOS**: 16 points
- **Android**: 16 dp (approximately 1/10th inch on most devices)
- **Windows**: 16 device-independent pixels

### Responsive design with consistent measurements

```xaml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="60" />  <!-- Header: 60 DIUs -->
        <RowDefinition Height="*" />   <!-- Content: Remaining space -->
        <RowDefinition Height="80" />  <!-- Footer: 80 DIUs -->
    </Grid.RowDefinitions>
    
    <!-- Header with consistent height across platforms -->
    <Label Grid.Row="0" Text="Header" FontSize="18" />
    
    <!-- Main content -->
    <ScrollView Grid.Row="1">
        <StackLayout Padding="20">  <!-- 20 DIU padding -->
            <Label Text="Content" />
        </StackLayout>
    </ScrollView>
    
    <!-- Footer with consistent height -->
    <Button Grid.Row="2" Text="Action" Margin="10" />
</Grid>
```

## Best practices

### Use appropriate values

Choose device-independent unit values that work well across platforms:

```xaml
<!-- Good: Common spacing values -->
<StackLayout Spacing="8">   <!-- Small spacing -->
<StackLayout Spacing="16">  <!-- Medium spacing -->
<StackLayout Spacing="24">  <!-- Large spacing -->

<!-- Good: Touch-friendly button sizes -->
<Button WidthRequest="120" HeightRequest="44" />

<!-- Good: Readable font sizes -->
<Label FontSize="14" />  <!-- Body text -->
<Label FontSize="18" />  <!-- Subtitle -->
<Label FontSize="24" />  <!-- Title -->
```

### Consider minimum touch targets

Ensure interactive elements meet platform guidelines:

```xaml
<!-- Minimum recommended button size -->
<Button Text="Tap me" 
        WidthRequest="44" 
        HeightRequest="44" />
```

This ensures the button meets accessibility guidelines across all platforms (approximately 44 points on iOS, 48 dp on Android).

### Avoid platform-specific conversions

Let .NET MAUI handle the platform-specific scaling automatically:

```csharp
// Good: Use device-independent units directly
myView.Margin = new Thickness(16);

// Avoid: Manual platform-specific calculations
// Don't manually convert between pixels and DIUs
```

## Related concepts

- **Screen density**: The number of pixels per inch (PPI/DPI) of a display
- **Scale factor**: The multiplier used to convert device-independent units to physical pixels
- **Viewport**: The visible area of the app, measured in device-independent units
- **Physical pixels**: The actual dots of color on the screen hardware

## See also

- [Align and position controls](align-position.md)
- [Layouts](layouts/index.md)
- [Fonts](fonts.md)
- [Unit converters](../platform-integration/device-media/unit-converters.md)