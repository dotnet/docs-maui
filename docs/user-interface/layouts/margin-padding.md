---
title: "Margin and padding"
description: "The .NET MAUI Margin and Padding properties control layout behavior when an element is rendered in the user interface."
ms.date: 01/04/2022
---

# Margin and padding

The .NET Multi-platform App UI (.NET MAUI) `Margin` and `Padding` properties control layout behavior when an element is rendered in the user interface. Margin and padding are related layout concepts:

- The `Margin` property represents the distance between an element and its adjacent elements, and is used to control the element's rendering position, and the rendering position of its neighbors. `Margin` values can be specified on layouts and views.
- The `Padding` property represents the distance between an element and its child elements, and is used to separate the control from its own content. `Padding` values can be specified on layouts.

The following diagram illustrates the two concepts:

:::image type="content" source="media/margin-padding/margin-padding.png" alt-text=".NET MAUI margin and padding concepts." border="false":::

> [!NOTE]
> `Margin` values are additive. Therefore, if two adjacent elements specify a margin of 20 device-independent units, the distance between the elements will be 40 device-independent units. In addition, margin and padding are additive when both are applied, in that the distance between an element and any content will be the margin plus padding.

## Specify a Thickness

The `Margin` and `Padding` properties are both of type `Thickness`. There are three possibilities when creating a `Thickness` struct:

- Create a `Thickness` struct defined by a single uniform value. The single value is applied to the left, top, right, and bottom sides of the element.
- Create a `Thickness` struct defined by horizontal and vertical values. The horizontal value is symmetrically applied to the left and right sides of the element, with the vertical value being symmetrically applied to the top and bottom sides of the element.
- Create a `Thickness` struct defined by four distinct values that are applied to the left, top, right, and bottom sides of the element.

The following XAML code example shows all three possibilities:

```xaml
<StackLayout Padding="0,20,0,0">
  <Label Text=".NET MAUI" Margin="20" />
  <Label Text=".NET iOS" Margin="10, 25" />
  <Label Text=".NET Android" Margin="0, 20, 15, 5" />
</StackLayout>
```

The equivalent C# code is shown in the following code example:

```csharp
StackLayout stackLayout = new StackLayout
{
    Padding = new Thickness(0, 20, 0, 0)
};  
stackLayout.Add(new Label { Text = ".NET MAUI", Margin = new Thickness(20) });
stackLayout.Add(new Label { Text = ".NET iOS", Margin = new Thickness(10, 25) });
stackLayout.Add(new Label { Text = ".NET Android", Margin = new Thickness(0, 20, 15, 5) });
```

> [!NOTE]
> `Thickness` values can be negative, which typically clips or overdraws the content.
