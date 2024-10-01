---
title: "Colors"
description: "The .NET MAUI Color class, in the Microsoft.Maui.Graphics namespace, lets you specify colors as RGB values, HSL values, HSV values, or with a color name."
ms.date: 09/30/2024
---

# Colors

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-graphicsview)

The <xref:Microsoft.Maui.Graphics.Color> class, in the <xref:Microsoft.Maui.Graphics> namespace, lets you specify colors as Red-Green-Blue (RGB) values, Hue-Saturation-Luminosity (HSL) values, Hue-Saturation-Value (HSV) values, or with a color name. An Alpha channel is also available to indicate transparency.

<xref:Microsoft.Maui.Graphics.Color> objects can be created with <xref:Microsoft.Maui.Graphics.Color> constructors, which can be used to specify a gray shade, an RGB value, or an RGB value with transparency. Constructor overloads accept `float` values ranging from 0 to 1, `byte`, and `int` values.

> [!NOTE]
> The default <xref:Microsoft.Maui.Graphics.Color> constructor creates a black <xref:Microsoft.Maui.Graphics.Color> object.

You can also use the following static methods to create <xref:Microsoft.Maui.Graphics.Color> objects:

- `Color.FromRgb` from `float` RGB values that range from 0 to 1.
- `Color.FromRgb` from `double` RGB values that range from 0 to 1.
- `Color.FromRgb` from `byte` RGB values that range from 0 to 255.
- `Color.FromInt` from `int` RGB values that range from 0 to 255.
- `Color.FromRgba` from `float` RGBA values that range from 0 to 1.
- `Color.FromRgba` from `double` RGBA values that range from 0 to 1.
- `Color.FromRgba` from `byte` RGBA values that range from 0 to 255.
- `Color.FromRgba` from `int` RGBA values that range from 0 to 255.
- `Color.FromRgba` from a `string`-based hexadecimal value in the form "#RRGGBBAA" or "#RRGGBB" or "#RGBA" or "#RGB", where each letter corresponds to a hexadecimal digit for the alpha, red, green, and blue channels.
- `Color.FromHsla` from `float` HSLA values.
- `Color.FromHsla` from `double` HSLA values.
- `Color.FromHsv` from `float` HSV values that range from 0 to 1.
- `Color.FromHsv` from `int` HSV values that range from 0 to 255.
- `Color.FromHsva` from `float` HSVA values.
- `Color.FromHsva` from `int` HSV values.
- `Color.FromInt` from an `int` value calculated as (B + 256 \* (G + 256 \* (R + 256 \* A))).
- `Color.FromUint` from a `uint` value calculated as (B + 256 \* (G + 256 \* (R + 256 \* A))).
- `Color.FromArgb` from a `string`-based hexadecimal value in the form "#AARRGGBB" or "#RRGGBB" or "#ARGB" or "RGB", where each letter corresponds to a hexadecimal digit for the alpha, red, green, and blue channels.

> [!NOTE]
> In addition to the methods listed above, the <xref:Microsoft.Maui.Graphics.Color> class also has <xref:Microsoft.Maui.Graphics.Color.Parse%2A> and <xref:Microsoft.Maui.Graphics.Color.TryParse%2A> methods that create <xref:Microsoft.Maui.Graphics.Color> objects from `string` arguments.

Once created, a <xref:Microsoft.Maui.Graphics.Color> object is immutable. The characteristics of the color can be obtained from the following `float` fields, that range from 0 to 1:

- <xref:Microsoft.Maui.Graphics.Color.Red>, which represents the red channel of the color.
- <xref:Microsoft.Maui.Graphics.Color.Green>, which represents the green channel of the color.
- <xref:Microsoft.Maui.Graphics.Color.Blue>, which represents the blue channel of the color.
- <xref:Microsoft.Maui.Graphics.Color.Alpha>, which represents the alpha channel of the color.

In addition, the characteristics of the color can be obtained from the following methods:

- <xref:Microsoft.Maui.Graphics.Color.GetHue%2A>, which returns a `float` that represents the hue channel of the color.
- <xref:Microsoft.Maui.Graphics.Color.GetSaturation%2A>, which returns a `float` that represents the saturation channel of the color.
- <xref:Microsoft.Maui.Graphics.Color.GetLuminosity%2A>, which returns a `float` that represents the luminosity channel of the color.

## Named colors

The <xref:Microsoft.Maui.Graphics.Colors> class defines 148 public static read-only fields for common colors, such as `AntiqueWhite`, `MidnightBlue`, and `YellowGreen`.

## Modify a color

The following instance methods modify an existing color to create a new color:

- <xref:Microsoft.Maui.Graphics.Color.AddLuminosity%2A> returns a <xref:Microsoft.Maui.Graphics.Color> by adding the luminosity value to the supplied delta value.
- <xref:Microsoft.Maui.Graphics.Color.GetComplementary%2A> returns the complementary <xref:Microsoft.Maui.Graphics.Color>.
- <xref:Microsoft.Maui.Graphics.Color.MultiplyAlpha%2A> returns a <xref:Microsoft.Maui.Graphics.Color> by multiplying the alpha value by the supplied `float` value.
- <xref:Microsoft.Maui.Graphics.Color.WithAlpha%2A> returns a <xref:Microsoft.Maui.Graphics.Color>, replacing the alpha value with the supplied `float` value.
- <xref:Microsoft.Maui.Graphics.Color.WithHue%2A> returns a <xref:Microsoft.Maui.Graphics.Color>, replacing the hue value with the supplied `float` value.
- <xref:Microsoft.Maui.Graphics.Color.WithLuminosity%2A> returns a <xref:Microsoft.Maui.Graphics.Color>, replacing the luminosity value with the supplied `float` value.
- <xref:Microsoft.Maui.Graphics.Color.WithSaturation%2A> returns a <xref:Microsoft.Maui.Graphics.Color>, replacing the saturation value with the supplied `float` value.

## Conversions

The following instance methods convert a <xref:Microsoft.Maui.Graphics.Color> to an alternative representation:

- <xref:Microsoft.Maui.Graphics.Color.AsPaint%2A> returns a <xref:Microsoft.Maui.Graphics.SolidPaint> object whose <xref:Microsoft.Maui.Graphics.Color> property is set to the color.
- <xref:Microsoft.Maui.Graphics.Color.ToHex%2A> returns a hexadecimal `string` representation of a <xref:Microsoft.Maui.Graphics.Color>.
- <xref:Microsoft.Maui.Graphics.Color.ToArgbHex%2A> returns an ARGB hexadecimal `string` representation of a <xref:Microsoft.Maui.Graphics.Color>.
- <xref:Microsoft.Maui.Graphics.Color.ToRgbaHex%2A> returns an RGBA hexadecimal `string` representation of a <xref:Microsoft.Maui.Graphics.Color>.
- <xref:Microsoft.Maui.Graphics.Color.ToInt%2A> returns an ARGB `int` representation of a <xref:Microsoft.Maui.Graphics.Color>.
- <xref:Microsoft.Maui.Graphics.Color.ToUint%2A> returns an ARGB `uint` representation of a <xref:Microsoft.Maui.Graphics.Color>.
- <xref:Microsoft.Maui.Graphics.Color.ToRgb%2A> converts a <xref:Microsoft.Maui.Graphics.Color> to RGB `byte` values that are returned as `out` arguments.
- <xref:Microsoft.Maui.Graphics.Color.ToRgba%2A> converts a <xref:Microsoft.Maui.Graphics.Color> to RGBA `byte` values that are returned as `out` arguments.
- <xref:Microsoft.Maui.Graphics.Color.ToHsl%2A> converts a <xref:Microsoft.Maui.Graphics.Color> to HSL `float` values that are passed as `out` arguments.

## Examples

In XAML, colors are typically referenced using their named values, or with hexadecimal:

```xaml
<Label Text="Sea color"
       TextColor="Aqua" />
<Label Text="RGB"
       TextColor="#00FF00" />
<Label Text="Alpha plus RGB"
       TextColor="#CC00FF00" />
<Label Text="Tiny RGB"
       TextColor="#0F0" />
<Label Text="Tiny Alpha plus RGB"
       TextColor="#C0F0" />
```

In C#, colors are typically referenced using their named values, or with their static methods:

```csharp
Label red    = new Label { Text = "Red",    TextColor = Colors.Red };
Label orange = new Label { Text = "Orange", TextColor = Color.FromHex("FF6A00") };
Label yellow = new Label { Text = "Yellow", TextColor = Color.FromHsla(0.167, 1.0, 0.5, 1.0) };
Label green  = new Label { Text = "Green",  TextColor = Color.FromRgb (38, 127, 0) };
Label blue   = new Label { Text = "Blue",   TextColor = Color.FromRgba(0, 38, 255, 255) };
Label indigo = new Label { Text = "Indigo", TextColor = Color.FromRgb (0, 72, 255) };
Label violet = new Label { Text = "Violet", TextColor = Color.FromHsla(0.82, 1, 0.25, 1) };
```

The following example uses the `OnPlatform` markup extension to selectively set the color of an <xref:Microsoft.Maui.Controls.ActivityIndicator>:

```xaml
<ActivityIndicator Color="{OnPlatform AliceBlue, iOS=MidnightBlue}"
                   IsRunning="True" />
```

The equivalent C# code is:

```csharp
ActivityIndicator activityIndicator = new ActivityIndicator
{
    Color = DeviceInfo.Platform == DevicePlatform.iOS ? Colors.MidnightBlue : Colors.AliceBlue,
    IsRunning = true
};
```
