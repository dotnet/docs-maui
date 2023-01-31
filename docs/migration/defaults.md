---
title: "Default values changes from Xamarin.Forms"
description: "Learn about the default values that have changed between Xamarin.Forms and .NET MAUI."
ms.date: 1/31/2023
---

# Default values changes from Xamarin.Forms

Xamarin.Forms uses arbitrary default values for some property values, such as padding, margins, and spacing. .NET Multi-platform App UI (.NET MAUI) changes these arbitrary property values to zero.

The .NET MAUI project template includes resource dictionaries that provide default styles for most controls. It's recommended you take a similar approach in your apps, by modifying or inheriting from these [resource dictionaries](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles).

To preserve the Xamarin.Forms default values in projects that don't set explicit values, add implicit styles to your project. For more information about implicit styles, see [Implicit styles](~/user-interface/styles/xaml.md#implicit-styles).

The following table lists the property values that have changed betwen Xamarin.Forms and .NET MAUI:

| Property | Xamarin.Forms value | .NET MAUI value |
| --- | --- | --- |
| `Grid.ColumnSpacing` | 6 | 0 |
| `Grid.RowSpacing` | 6 | 0 |
| `StackLayout.Spacing` | 6 | 0 |
