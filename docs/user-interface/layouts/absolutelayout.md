---
title: "AbsoluteLayout"
description: "The .NET MAUI AbsoluteLayout is used to position and size elements using explicit values, or values proportional to the size of the layout."
ms.date: 11/29/2025
---

# AbsoluteLayout

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-absolutelayout)

:::image type="content" source="media/absolutelayout/layouts.png" alt-text=".NET MAUI AbsoluteLayout." border="false":::

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.AbsoluteLayout> is used to position and size children using explicit values. By default, the position is specified by the top-left corner of the child relative to the top-left corner of the <xref:Microsoft.Maui.Controls.AbsoluteLayout>, in device-independent units. <xref:Microsoft.Maui.Controls.AbsoluteLayout> also implements a proportional positioning and sizing feature that positions children based on the available space within the layout. In addition, unlike some other layout classes, <xref:Microsoft.Maui.Controls.AbsoluteLayout> is able to position children so that they overlap.

An <xref:Microsoft.Maui.Controls.AbsoluteLayout> should be regarded as a special-purpose layout to be used only when you can impose a size on children, or when the element's size doesn't affect the positioning of other children.

The <xref:Microsoft.Maui.Controls.AbsoluteLayout> class defines the following properties:

- `LayoutBounds`, of type `Rect`, which is an attached property that represents the position and size of a child. The default value of this property is (0,0,AutoSize,AutoSize).
- `LayoutFlags`, of type `AbsoluteLayoutFlags`, which is an attached property that indicates whether properties of the layout bounds used to position and size the child are interpreted proportionally. The default value of this property is `AbsoluteLayoutFlags.None`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings and styled. For more information about attached properties, see [.NET MAUI Attached Properties](~/fundamentals/attached-properties.md).

## Position and size children

The position and size of children in an <xref:Microsoft.Maui.Controls.AbsoluteLayout> is defined by setting the `AbsoluteLayout.LayoutBounds` attached property of each child, using absolute values or proportional values. Absolute and proportional values can be mixed for children when the position should scale, but the size should stay fixed, or vice versa. For information about absolute values, see [Absolute positioning and sizing](#absolute-positioning-and-sizing). For information about proportional values, see [Proportional positioning and sizing](#proportional-positioning-and-sizing).

The `AbsoluteLayout.LayoutBounds` attached property can be set using two formats:

- `x, y`. With this format, the `x` and `y` values indicate the position of the child relative to its parent. The child is unconstrained and sizes itself.
- `x, y, width, height`. With this format, the `x` and `y` values indicate the position of the child relative to its parent, while the `width` and `height` values indicate the child's size.

When using absolute values, `x` and `y` specify the position of the top-left corner of the child in device-independent units. When using proportional values, `x` and `y` are calculated as `(parentDimension - childDimension) * proportionalValue`. This means a proportional position of (0.5, 0.5) centers the child within the <xref:Microsoft.Maui.Controls.AbsoluteLayout>, because the child is placed at half of the remaining space after accounting for its size.

To specify that a child sizes itself horizontally or vertically, or both, set the `width` and/or `height` values to the `AbsoluteLayout.AutoSize` property. However, overuse of this property can harm application performance, as it causes the layout engine to perform additional layout calculations.

> [!IMPORTANT]
> The `HorizontalOptions` and `VerticalOptions` properties have no effect on children of an <xref:Microsoft.Maui.Controls.AbsoluteLayout>.

## Absolute positioning and sizing

By default, an <xref:Microsoft.Maui.Controls.AbsoluteLayout> positions and sizes children using absolute values, specified in device-independent units, which explicitly define where children should be placed in the layout. This is achieved by adding children to an <xref:Microsoft.Maui.Controls.AbsoluteLayout> and setting the `AbsoluteLayout.LayoutBounds` attached property on each child to absolute position and/or size values.

> [!WARNING]
> Using absolute values for positioning and sizing children can be problematic, because different devices have different screen sizes and resolutions. Therefore, the coordinates for the center of the screen on one device may be offset on other devices.

The following XAML shows an <xref:Microsoft.Maui.Controls.AbsoluteLayout> whose children are positioned using absolute values:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AbsoluteLayoutDemos.Views.XAML.StylishHeaderDemoPage"
             Title="Stylish header demo">
    <AbsoluteLayout Margin="20">
        <BoxView Color="Silver"
                 AbsoluteLayout.LayoutBounds="0, 10, 200, 5" />
        <BoxView Color="Silver"
                 AbsoluteLayout.LayoutBounds="0, 20, 200, 5" />
        <BoxView Color="Silver"
                 AbsoluteLayout.LayoutBounds="10, 0, 5, 65" />
        <BoxView Color="Silver"
                 AbsoluteLayout.LayoutBounds="20, 0, 5, 65" />
        <Label Text="Stylish Header"
               FontSize="24"
               AbsoluteLayout.LayoutBounds="30, 25" />
    </AbsoluteLayout>
</ContentPage>

```

In this example, the position of each <xref:Microsoft.Maui.Controls.BoxView> object is defined using the first two absolute values that are specified in the `AbsoluteLayout.LayoutBounds` attached property. The size of each <xref:Microsoft.Maui.Controls.BoxView> is defined using the third and forth values. The position of the <xref:Microsoft.Maui.Controls.Label> object is defined using the two absolute values that are specified in the `AbsoluteLayout.LayoutBounds` attached property. Size values are not specified for the <xref:Microsoft.Maui.Controls.Label>, and so it's unconstrained and sizes itself. In all cases, the absolute values represent device-independent units.

The following screenshot shows the resulting layout:

:::image type="content" source="media/absolutelayout/absolute-values.png" alt-text="Children placed in an AbsoluteLayout using absolute values.":::

The equivalent C# code is shown below:

```csharp
public class StylishHeaderDemoPage : ContentPage
{
    public StylishHeaderDemoPage()
    {
        AbsoluteLayout absoluteLayout = new AbsoluteLayout
        {
            Margin = new Thickness(20)
        };

        absoluteLayout.Add(new BoxView
        {
            Color = Colors.Silver
        }, new Rect(0, 10, 200, 5));
        absoluteLayout.Add(new BoxView
        {
            Color = Colors.Silver
        }, new Rect(0, 20, 200, 5));
        absoluteLayout.Add(new BoxView
        {
            Color = Colors.Silver
        }, new Rect(10, 0, 5, 65));
        absoluteLayout.Add(new BoxView
        {
            Color = Colors.Silver
        }, new Rect(20, 0, 5, 65));

        absoluteLayout.Add(new Label
        {
            Text = "Stylish Header",
            FontSize = 24
        }, new Point(30,25));                     

        Title = "Stylish header demo";
        Content = absoluteLayout;
    }
}
```

In this example, the position and size of each <xref:Microsoft.Maui.Controls.BoxView> is defined using a `Rect` object. The position of the <xref:Microsoft.Maui.Controls.Label> is defined using a `Point` object. The C# example uses the following `Add` extension methods to add children to the `AbsoluteLayout`:

```csharp
using Microsoft.Maui.Layouts;

namespace Microsoft.Maui.Controls
{
    public static class AbsoluteLayoutExtensions
    {
        public static void Add(this AbsoluteLayout absoluteLayout, IView view, Rect bounds, AbsoluteLayoutFlags flags = AbsoluteLayoutFlags.None)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            if (bounds.IsEmpty)
                throw new ArgumentNullException(nameof(bounds));

            absoluteLayout.Add(view);
            absoluteLayout.SetLayoutBounds(view, bounds);
            absoluteLayout.SetLayoutFlags(view, flags);
        }

        public static void Add(this AbsoluteLayout absoluteLayout, IView view, Point position)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            if (position.IsEmpty)
                throw new ArgumentNullException(nameof(position));

            absoluteLayout.Add(view);
            absoluteLayout.SetLayoutBounds(view, new Rect(position.X, position.Y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }
    }
}
```

In C#, it's also possible to set the position and size of a child of an <xref:Microsoft.Maui.Controls.AbsoluteLayout> after it has been added to the layout, using the `AbsoluteLayout.SetLayoutBounds` method. The first argument to this method is the child, and the second is a `Rect` object.

> [!NOTE]
> An <xref:Microsoft.Maui.Controls.AbsoluteLayout> that uses absolute values can position and size children so that they don't fit within the bounds of the layout.

## Proportional positioning and sizing

An <xref:Microsoft.Maui.Controls.AbsoluteLayout> can position and size children using proportional values. This is achieved by adding children to the <xref:Microsoft.Maui.Controls.AbsoluteLayout> and by setting the `AbsoluteLayout.LayoutBounds` attached property on each child to proportional position and/or size values in the range 0-1. Position and size values are made proportional by setting the `AbsoluteLayout.LayoutFlags` attached property on each child.

The `AbsoluteLayout.LayoutFlags` attached property, of type `AbsoluteLayoutFlags`, allows you to set a flag that indicates that the layout bounds position and size values for a child are proportional to the size of the <xref:Microsoft.Maui.Controls.AbsoluteLayout>. When laying out a child, <xref:Microsoft.Maui.Controls.AbsoluteLayout> scales the position and size values appropriately, to any device size.

The `AbsoluteLayoutFlags` enumeration defines the following members:

- `None`, indicates that values will be interpreted as absolute. This is the default value of the `AbsoluteLayout.LayoutFlags` attached property.
- `XProportional`, indicates that the `x` value will be interpreted as proportional, while treating all other values as absolute.
- `YProportional`, indicates that the `y` value will be interpreted as proportional, while treating all other values as absolute.
- `WidthProportional`, indicates that the `width` value will be interpreted as proportional, while treating all other values as absolute.
- `HeightProportional`, indicates that the `height` value will be interpreted as proportional, while treating all other values as absolute.
- `PositionProportional`, indicates that the `x` and `y` values will be interpreted as proportional, while the size values are interpreted as absolute.
- `SizeProportional`, indicates that the `width` and `height` values will be interpreted as proportional, while the position values are interpreted as absolute.
- `All`, indicates that all values will be interpreted as proportional.

> [!TIP]
> The `AbsoluteLayoutFlags` enumeration is a `Flags` enumeration, which means that enumeration members can be combined. This is accomplished in XAML with a comma-separated list, and in C# with the bitwise OR operator.

For example, if you use the `SizeProportional` flag and set the width of a child to 0.25 and the height to 0.1, the child will be one-quarter of the width of the <xref:Microsoft.Maui.Controls.AbsoluteLayout> and one-tenth the height. The `PositionProportional` flag is similar. A position of (0,0) puts the child in the upper-left corner, while a position of (1,1) puts the child in the lower-right corner, and a position of (0.5,0.5) centers the child within the <xref:Microsoft.Maui.Controls.AbsoluteLayout>.

The following XAML shows an <xref:Microsoft.Maui.Controls.AbsoluteLayout> whose children are positioned using proportional values:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AbsoluteLayoutDemos.Views.XAML.ProportionalDemoPage"
             Title="Proportional demo">
    <AbsoluteLayout>
        <BoxView Color="Blue"
                 AbsoluteLayout.LayoutBounds="0.5,0,100,25"
                 AbsoluteLayout.LayoutFlags="PositionProportional" />
        <BoxView Color="Green"
                 AbsoluteLayout.LayoutBounds="0,0.5,25,100"
                 AbsoluteLayout.LayoutFlags="PositionProportional" />
        <BoxView Color="Red"
                 AbsoluteLayout.LayoutBounds="1,0.5,25,100"
                 AbsoluteLayout.LayoutFlags="PositionProportional" />
        <BoxView Color="Black"
                 AbsoluteLayout.LayoutBounds="0.5,1,100,25"
                 AbsoluteLayout.LayoutFlags="PositionProportional" />
        <Label Text="Centered text"
               AbsoluteLayout.LayoutBounds="0.5,0.5,110,25"
               AbsoluteLayout.LayoutFlags="PositionProportional" />
    </AbsoluteLayout>
</ContentPage>
```

In this example, each child is positioned using proportional values but sized using absolute values. This is accomplished by setting the `AbsoluteLayout.LayoutFlags` attached property of each child to `PositionProportional`. The first two values that are specified in the `AbsoluteLayout.LayoutBounds` attached property, for each child, define the position using proportional values. The size of each child is defined with the third and forth absolute values, using device-independent units.

The following screenshot shows the resulting layout:

:::image type="content" source="media/absolutelayout/proportional-position.png" alt-text="Children placed in an AbsoluteLayout using proportional position values.":::

The equivalent C# code is shown below:

```csharp
public class ProportionalDemoPage : ContentPage
{
    public ProportionalDemoPage()
    {
        BoxView blue = new BoxView { Color = Colors.Blue };
        AbsoluteLayout.SetLayoutBounds(blue, new Rect(0.5, 0, 100, 25));
        AbsoluteLayout.SetLayoutFlags(blue, AbsoluteLayoutFlags.PositionProportional);

        BoxView green = new BoxView { Color = Colors.Green };
        AbsoluteLayout.SetLayoutBounds(green, new Rect(0, 0.5, 25, 100));
        AbsoluteLayout.SetLayoutFlags(green, AbsoluteLayoutFlags.PositionProportional);

        BoxView red = new BoxView { Color = Colors.Red };
        AbsoluteLayout.SetLayoutBounds(red, new Rect(1, 0.5, 25, 100));
        AbsoluteLayout.SetLayoutFlags(red, AbsoluteLayoutFlags.PositionProportional);

        BoxView black = new BoxView { Color = Colors.Black };
        AbsoluteLayout.SetLayoutBounds(black, new Rect(0.5, 1, 100, 25));
        AbsoluteLayout.SetLayoutFlags(black, AbsoluteLayoutFlags.PositionProportional);

        Label label = new Label { Text = "Centered text" };
        AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 110, 25));
        AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);

        Title = "Proportional demo";
        Content = new AbsoluteLayout
        {
            Children =  { blue, green, red, black, label }
        };
    }
}
```

In this example, the position and size of each child is set with the `AbsoluteLayout.SetLayoutBounds` method. The first argument to the method is the child, and the second is a `Rect` object. The position of each child is set with proportional values, while the size of each child is set with absolute values, using device-independent units.

> [!NOTE]
> An <xref:Microsoft.Maui.Controls.AbsoluteLayout> that uses proportional values can position and size children so that they don't fit within the bounds of the layout by using values outside the 0-1 range.
