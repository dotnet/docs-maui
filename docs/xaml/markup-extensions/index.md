---
title: "XAML markup extensions overview"
description: ".NET MAUI XAML markup extensions extend the power and flexibility of XAML by allowing element attributes to be set from sources other than literal text strings."
ms.date: 01/27/2022
---

# Markup extensions

.NET Multi-platform App UI (.NET MAUI) XAML markup extensions extend the power and flexibility of XAML by allowing element attributes to be set from sources other than literal text strings.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

For example, you typically set the `Color` property of `BoxView` like this:

```xaml
<BoxView Color="Blue" />
```

Alternatively, you can set it to a hexadecimal RGB color value:

```xaml
<BoxView Color="#FF0080" />
```

In either case, the text string set to the `Color` attribute is converted to a `Color` value by the .NET MAUI `ColorTypeConverter` class.

You might prefer instead to set the `Color` attribute from a value stored in a resource dictionary, or from the value of a static property of a class that you've created, or from a property of type `Color` of another element on the page, or constructed from separate hue, saturation, and luminosity values. All these options are possible using XAML markup extensions.

A markup extension is a different way to express an attribute of an element. .NET MAUI XAML markup extensions are usually identifiable by an attribute value that is enclosed in curly braces:

```xaml
<BoxView Color="{StaticResource themeColor}" />
```

Any attribute value in curly braces is *always* a XAML markup extension. However, XAML markup extensions can also be referenced without the use of curly braces.
