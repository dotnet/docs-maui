---
title: "Xamarin.Essentials Color Converters"
description: "The ColorConverters class in Xamarin.Essentials provides several helper methods and extension methods to work with System.Drawing.Color."
author: jamesmontemagno
ms.author: jamont
ms.date: 01/06/2020
ms.custom: video
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Color Converters

The **ColorConverters** class in Xamarin.Essentials provides several helper methods for System.Drawing.Color.

## Get started

[!include[](~/essentials/includes/get-started.md)]

## Using Color Converters

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

When working with `System.Drawing.Color` you can use the built in converters of Xamarin.Forms to create a color from Hsl, Hex, or UInt.

```csharp
var blueHex = ColorConverters.FromHex("#3498db");
var blueHsl = ColorConverters.FromHsl(204, 70, 53);
var blueUInt = ColorConverters.FromUInt(3447003);
```

## Using Color Extensions

Extension methods on `System.Drawing.Color` enable you to apply different properties:

```csharp
var blue = ColorConverters.FromHex("#3498db");

// Multiplies the current alpha by 50%
var blueWithAlpha = blue.MultiplyAlpha(.5f);
```

There are several other extension methods including:

- GetComplementary
- MultiplyAlpha
- ToUInt
- WithAlpha
- WithHue
- WithLuminosity
- WithSaturation

## Using Platform Extensions

Additionally, you can convert System.Drawing.Color to the platform specific color structure. These methods can only be called from the iOS, Android, and UWP projects.

```csharp
var system = System.Drawing.Color.FromArgb(255, 52, 152, 219);

// Extension to convert to Android.Graphics.Color, UIKit.UIColor, or Windows.UI.Color
var platform = system.ToPlatformColor();
```

```csharp
var platform = new Android.Graphics.Color(52, 152, 219, 255);

// Back to System.Drawing.Color
var system = platform.ToSystemColor();
```

The `ToSystemColor` method applies to Android.Graphics.Color, UIKit.UIColor, and Windows.UI.Color.

## API

- [Color Converters source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Types/ColorConverters.shared.cs)
- [Color Converters API documentation](xref:Xamarin.Essentials.ColorConverters)
- [Color Extensions source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Types/ColorConverters.shared.cs)
- [Color Extensions API documentation](xref:Xamarin.Essentials.ColorExtensions)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Color-Converters-XamarinEssentials-API-of-the-Week/player]

[!include[](~/essentials/includes/xamarin-show-essentials.md)]
