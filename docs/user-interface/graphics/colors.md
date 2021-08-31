---
title: "Colors"
description: "The .NET MAUI Color class lets you specify colors as RGB values, HSL values, HSV values, or with a color name."
ms.date: 08/31/2021
---

# Colors

<!-- Sample link goes here -->

The `Color` class lets you specify colors as Red-Green-Blue (RGB) values, Hue-Saturation-Luminosity (HSL) values, Hue-Saturation-Value (HSV) values, or with a color name. An Alpha channel is also available to indicate transparency.

`Color` objects can be created with the `Color` constructors, which can be used to specify a gray shade, an RGB value, or an RGB value with transparency. In all cases, arguments are `float` values ranging from 0 to 1.

> [!IMPORTANT]
> The default `Color` constructor creates a black `Color` object.

You can also use static methods to create `Color` objects:

- `Color.FromRgb` for `float` RGB values from 0 to 1.
- `Color.FromRgb` for `double` RGB values from 0 to 1.
- `Color.FromRgb` for `byte` RGB values from 0 to 255.
- `Color.FromRgba` for `float` RGA values, with transparency, from 0 to 1.
- `Color.FromRgba` for `double` RGB values, with transparency, from 0 to 1.
- `Color.FromRgba` for `byte` RGB values, with transparency, from 0 to 255.
- `Color.FromRgba` for a `string`-based hexadecimal value in the form "#RRGGBBAA" or "#RRGGBB" or "#RGBA" or "#RGB", where each letter corresponds to a hexadecimal digit for the alpha, red, green, and blue channels.
- `Color.FromHsla` for `float` HSL values, with transparency.
- `Color.FromHsla` for `double` HSL values, with transparency.
- `Color.FromHsv` for `float` HSV values from 0 to 1.
- `Color.FromHsv` for `int` HSV values from 0 to 255.
- `Color.FromHsva` for `float` HSV values, with transparency.
- `Color.FromHsva` for `int` HSV values, with transparency.
- `Color.FromInt` for an `int` value calculated as (B + 256 \* (G + 256 \* (R + 256 \* A))).
- `Color.FromUint` for a `uint` value calculated as (B + 256 \* (G + 256 \* (R + 256 \* A))).
- `Color.FromArgb` for a `string`-based hexadecimal value in the form "#AARRGGBB" or "#RRGGBB" or "#ARGB" or "RGB", where each letter corresponds to a hexadecimal digit for the alpha, red, green, and blue channels.

> [!NOTE]
> In addition to the methods listed above, the `Color` class also has `Parse` and `TryParse` methods that create `Color` objects from `string` arguments.

Once created, a `Color` object is immutable. The characteristics of the color can be obtained from the following properties:

- `R`, which represents the red channel of the color.
- `G`, which represents the green channel of the color.
- `B`, which represents the blue channel of the color.
- `A`, which represents the alpha channel of the color.

These properties are all `float` values ranging from 0 to 1.

In addition, the characteristics of the color can be obtained from the following methods:

- `GetHue`, which returns a `float` that represents the hue channel of the color.
- `GetSaturation`, which returns a `float` that represents the saturation channel of the color.
- `Luminosity`, which returns a `float` that represents the luminosity channel of the color.

## Named colors

The `Colors` class defines 148 public static read-only fields for common colors, such as `AntiqueWhite`, `MidnightBlue`, and `YellowGreen`.

## Modify a color

The following instance methods modify an existing color to create a new color:

- `AddLuminosity` returns a `Color` by modifying the luminosity by the supplied delta.
- `GetComplementary` returns the complementary `Color`.
- `MultiplyAlpha` returns a `Color` by modifying the alpha, multiplying it by the supplied `float` value.
- `WithAlpha` returns a `Color`, replacing the alpha with the supplied `float` value.
- `WithHue` returns a `Color`, replacing the hue with the supplied `float` value.
- `WithLuminosity` returns a `Color`, replacing the luminosity with the supplied `float` value.
- `WithSaturation` returns a `Color`, replacing the saturation with the supplied `float` value.

## Conversions

The following instance methods convert a `Color` to an alternative representation:

- `ToHex` returns a hexadecimal `string` representation of a `Color`.
- `ToArgbHex` returns an ARGB hexadecimal `string` representation of a `Color`.
- `ToRgbaHex` returns an RGBA hexadecimal `string` representation of a `Color`.
- `ToInt` returns an ARGB `int` representation of a `Color`.
- `ToUint` returns an ARGB `uint` representation of a `Color`.
- `ToRgb` converts a `Color` to RGB `byte` values that are returned as `out` arguments.
- `ToRgba` converts a `Color` to RGBA `byte` values that are returned as `out` arguments.
- `ToHsl` converts a `Color` to HSL `float` values that are passed as `out` arguments.

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

The following example uses the `OnPlatform` markup extension to selectively set the color of an `ActivityIndicator`:

```xaml
<ActivityIndicator Color="{OnPlatform AliceBlue, iOS=MidnightBlue}"
                   IsRunning="True" />
```

The equivalent C# code is:

```csharp
ActivityIndicator activityIndicator = new ActivityIndicator
{
    Color = Device.RuntimePlatform == Device.iOS ? Colors.MidnightBlue : Colors.AliceBlue,
    IsRunning = true
};
```
