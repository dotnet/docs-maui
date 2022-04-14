---
title: "RelativeLayout"
description: "The .NET MAUI RelativeLayout is used position and size children relative to properties of the layout or sibling elements."
ms.date: 01/21/2022
---

# RelativeLayout

<!-- Sample link goes here -->

:::image type="content" source="media/relativelayout/layouts.png" alt-text=".NET MAUI RelativeLayout." border="false":::

The .NET Multi-platform App UI (.NET MAUI) `RelativeLayout`, which is available in the `Microsoft.Maui.Controls.Compatibility` namespace, is used to position and size children relative to properties of the layout or sibling elements. This allows UIs to be created that scale proportionally across device sizes. In addition, unlike some other layout classes, `RelativeLayout` is able to position children so that they overlap.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `RelativeLayout` class defines the following properties:

- `XConstraint`, of type `Constraint`, which is an attached property that represents the constraint on the X position of the child.
- `YConstraint`, of type `Constraint`, which is an attached property that represents the constraint on the Y position of the child.
- `WidthConstraint`, of type `Constraint`, which is an attached property that represents the constraint on the width of the child.
- `HeightConstraint`, of type `Constraint`, which is an attached property that represents the constraint on the height of the child.
- `BoundsConstraint`, of type `BoundsConstraint`, which is an attached property that represents the constraint on the position and size of the child. This property can't be easily consumed from XAML.

These properties are backed by `BindableProperty` objects, which means that the properties can be targets of data bindings and styled. <!-- For more information about attached properties, see [.NET MAUI Attached Properties](~/xamarin-forms/xaml/attached-properties.md). -->

The `RelativeLayout` class derives from the `Layout<T>` class, which defines a `Children` property of type `IList<T>`. The `Children` property is the `ContentProperty` of the `Layout<T>` class, and therefore does not need to be explicitly set from XAML.

The width and height of a child in a `RelativeLayout` can also be specified through the child's `WidthRequest` and `HeightRequest` properties, instead of the `WidthConstraint` and `HeightConstraint` attached properties.

> [!TIP]
> Avoid using a `RelativeLayout` whenever possible. It will result in the CPU having to perform significantly more work.

## Constraints

Within a `RelativeLayout`, the position and size of children are specified as constraints using absolute values or relative values. When constraints aren't specified, a child will be positioned in the upper left corner of the layout.

The following table shows how to specify constraints in XAML and C#:

|     | XAML | C# |
| --- | ---- | -- |
| **Absolute values** | Absolute constraints are specified by setting the `RelativeLayout` attached properties to `double` values. | Absolute constraints are specified by the `Constraint.Constant` method, or by using the `Children.Add` overload that requires a `Func<Rect>` argument. |
| **Relative values** | Relative constraints are specified by setting the `RelativeLayout` attached properties to `Constraint` objects that are returned by the `ConstraintExpression` markup extension. | Relative constraints are specified by `Constraint` objects that are returned by methods of the `Constraint` class. |

For more information about specifying constraints using absolute values, see [Absolute positioning and sizing](#absolute-positioning-and-sizing). For more information about specifying constraints using relative values, see [Relative positioning and sizing](#relative-positioning-and-sizing).

In C#, children can be added to `RelativeLayout` by three `Add` overloads. The first overload requires a `Expression<Func<Rect>>` to specify the position and size of a child. The second overload requires optional `Expression<Func<double>>` objects for the `x`, `y`, `width`, and `height` arguments. The third overload requires optional `Constraint` objects for the `x`, `y`, `width`, and `height` arguments.

It's possible to change the position and size of a child in a `RelativeLayout` with the `SetXConstraint`, `SetYConstraint`, `SetWidthConstraint`, and `SetHeightConstraint` methods. The first argument to each of these methods is the child, and the second is a `Constraint` object. In addition, the `SetBoundsConstraint` method can also be used to change the position and size of a child. The first argument to this method is the child, and the second is a `BoundsConstraint` object.

## Absolute positioning and sizing

A `RelativeLayout` can position and size children using absolute values, specified in device-independent units, which explicitly define where children should be placed in the layout. This is achieved by adding children to the `Children` collection of a `RelativeLayout` and setting the `XConstraint`, `YConstraint`, `WidthConstraint`, and `HeightConstraint` attached properties on each child to absolute position and/or size values.

> [!WARNING]
> Using absolute values for positioning and sizing children can be problematic, because different devices have different screen sizes and resolutions. Therefore, the coordinates for the center of the screen on one device may be offset on other devices.

The following XAML shows a `RelativeLayout` whose children are positioned using absolute values:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:compat="clr-namespace:Microsoft.Maui.Controls.Compatibility;assembly=Microsoft.Maui.Controls"
             x:Class="RelativeLayoutDemos.Views.XAML.StylishHeaderDemoPage"
             Title="Stylish header demo">
    <compat:RelativeLayout Margin="20">
        <BoxView Color="Silver"
                 compat:RelativeLayout.XConstraint="0"
                 compat:RelativeLayout.YConstraint="10"
                 compat:RelativeLayout.WidthConstraint="200"
                 compat:RelativeLayout.HeightConstraint="5" />
        <BoxView Color="Silver"
                 compat:RelativeLayout.XConstraint="0"
                 compat:RelativeLayout.YConstraint="20"
                 compat:RelativeLayout.WidthConstraint="200"
                 compat:RelativeLayout.HeightConstraint="5" />
        <BoxView Color="Silver"
                 compat:RelativeLayout.XConstraint="10"
                 compat:RelativeLayout.YConstraint="0"
                 compat:RelativeLayout.WidthConstraint="5"
                 compat:RelativeLayout.HeightConstraint="65" />
        <BoxView Color="Silver"
                 compat:RelativeLayout.XConstraint="20"
                 compat:RelativeLayout.YConstraint="0"
                 compat:RelativeLayout.WidthConstraint="5"
                 compat:RelativeLayout.HeightConstraint="65" />
        <Label Text="Stylish header"
               FontSize="24"
               compat:RelativeLayout.XConstraint="30"
               compat:RelativeLayout.YConstraint="25" />
    </compat:RelativeLayout>
</ContentPage>
```

In this example, the position of each `BoxView` object is defined using the values specified in the `XConstraint` and `YConstraint` attached properties. The size of each `BoxView` is defined using the values specified in the `WidthConstraint` and `HeightConstraint` attached properties. The position of the `Label` object is also defined using the values specified in the `XConstraint` and `YConstraint` attached properties. However, size values are not specified for the `Label`, and so it's unconstrained and sizes itself. In all cases, the absolute values represent device-independent units.

The following screenshot shows the resulting layout:

:::image type="content" source="media/relativelayout/absolute-values.png" alt-text="Screenshot of children placed in a RelativeLayout using absolute values.":::

The equivalent C# code is shown below:

```csharp
public class StylishHeaderDemoPage : ContentPage
{
    public StylishHeaderDemoPage()
    {
        RelativeLayout relativeLayout = new RelativeLayout
        {
            Margin = new Thickness(20)
        };

        relativeLayout.Children.Add(new BoxView
        {
            Color = Colors.Silver
        }, () => new Rect(0, 10, 200, 5));

        relativeLayout.Children.Add(new BoxView
        {
            Color = Colors.Silver
        }, () => new Rect(0, 20, 200, 5));

        relativeLayout.Children.Add(new BoxView
        {
            Color = Colors.Silver
        }, () => new Rect(10, 0, 5, 65));

        relativeLayout.Children.Add(new BoxView
        {
            Color = Colors.Silver
        }, () => new Rect(20, 0, 5, 65));

        relativeLayout.Children.Add(new Label
        {
            Text = "Stylish Header",
            FontSize = 24
        }, Constraint.Constant(30), Constraint.Constant(25));

        Title = "Stylish header demo";
        Content = relativeLayout;
    }
}
```

In this example, `BoxView` objects are added to the `RelativeLayout` using an `Add` overload that requires a `Expression<Func<Rect>>` to specify the position and size of each child. The position of the `Label` is defined using an `Add` overload that requires optional `Constraint` objects, in this case created by the `Constraint.Constant` method.

> [!NOTE]
> A `RelativeLayout` that uses absolute values can position and size children so that they don't fit within the bounds of the layout.

## Relative positioning and sizing

A `RelativeLayout` can position and size children using values that are relative to properties of the layout, or sibling elements. This is achieved by adding children to the `Children` collection of the `RelativeLayout` and setting the `XConstraint`, `YConstraint`, `WidthConstraint`, and `HeightConstraint` attached properties on each child to relative values using `Constraint` objects.

Constraints can be a constant, relative to a parent, or relative to a sibling. The type of constraint is represented by the `ConstraintType` enumeration, which defines the following members:

- `RelativeToParent`, which indicates a constraint that is relative to a parent.
- `RelativeToView`, which indicates a constraint that is relative to a view (or sibling).
- `Constant`, which indicates a constant constraint.

### Constraint markup extension

In XAML, a `Constraint` object can be created by the `ConstraintExpression` markup extension. This markup extension is typically used to relate the position and size of a child within a `RelativeLayout` to its parent, or to a sibling.

The `ConstraintExpression` class defines the following properties:

- `Constant`, of type `double`, which represents the constraint constant value.
- `ElementName`, of type `string`, which represents the name of a source element against which to calculate the constraint.
- `Factor`, of type `double`, which represents the factor by which to scale a constrained dimension, relative to the source element. This property defaults to 1.
- `Property`, of type `string`, which represents the name of the property on the source element to use in the constraint calculation.
- `Type`, of type `ConstraintType`, which represents the type of the constraint.

<!-- For more information about .NET MAUI markup extensions, see [XAML Markup Extensions](~/xaml/markup-extensions/index.md). -->

The following XAML shows a `RelativeLayout` whose children are constrained by the `ConstraintExpression` markup extension:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:compat="clr-namespace:Microsoft.Maui.Controls.Compatibility;assembly=Microsoft.Maui.Controls"
             x:Class="RelativeLayoutDemos.Views.XAML.RelativePositioningAndSizingDemoPage"
             Title="RelativeLayout demo">
    <compat:RelativeLayout>
        <!-- Four BoxView's -->
        <BoxView Color="Red"
                 compat:RelativeLayout.XConstraint="{compat:ConstraintExpression Type=Constant, Constant=0}"
                 compat:RelativeLayout.YConstraint="{compat:ConstraintExpression Type=Constant, Constant=0}" />
        <BoxView Color="Green"
                 compat:RelativeLayout.XConstraint="{compat:ConstraintExpression Type=RelativeToParent, Property=Width, Constant=-40}"
                 compat:RelativeLayout.YConstraint="{compat:ConstraintExpression Type=Constant, Constant=0}" />
        <BoxView Color="Blue"
                 compat:RelativeLayout.XConstraint="{compat:ConstraintExpression Type=Constant, Constant=0}"
                 compat:RelativeLayout.YConstraint="{compat:ConstraintExpression Type=RelativeToParent, Property=Height, Constant=-40}" />
        <BoxView Color="Yellow"
                 compat:RelativeLayout.XConstraint="{compat:ConstraintExpression Type=RelativeToParent, Property=Width, Constant=-40}"
                 compat:RelativeLayout.YConstraint="{compat:ConstraintExpression Type=RelativeToParent, Property=Height, Constant=-40}" />

        <!-- Centered and 1/3 width and height of parent -->
        <BoxView x:Name="oneThird"
                 Color="Silver"
                 compat:RelativeLayout.XConstraint="{compat:ConstraintExpression Type=RelativeToParent, Property=Width, Factor=0.33}"
                 compat:RelativeLayout.YConstraint="{compat:ConstraintExpression Type=RelativeToParent, Property=Height, Factor=0.33}"
                 compat:RelativeLayout.WidthConstraint="{compat:ConstraintExpression Type=RelativeToParent, Property=Width, Factor=0.33}"
                 compat:RelativeLayout.HeightConstraint="{compat:ConstraintExpression Type=RelativeToParent, Property=Height, Factor=0.33}" />

        <!-- 1/3 width and height of previous -->
        <BoxView Color="Black"
                 compat:RelativeLayout.XConstraint="{compat:ConstraintExpression Type=RelativeToView, ElementName=oneThird, Property=X}"
                 compat:RelativeLayout.YConstraint="{compat:ConstraintExpression Type=RelativeToView, ElementName=oneThird, Property=Y}"
                 compat:RelativeLayout.WidthConstraint="{compat:ConstraintExpression Type=RelativeToView, ElementName=oneThird, Property=Width, Factor=0.33}"
                 compat:RelativeLayout.HeightConstraint="{compat:ConstraintExpression Type=RelativeToView, ElementName=oneThird, Property=Height, Factor=0.33}" />
    </compat:RelativeLayout>
</ContentPage>
```

In this example, the position of each `BoxView` object is defined by setting the `XConstraint` and `YConstraint` attached properties. The first `BoxView` has its `XConstraint` and `YConstraint` attached properties set to constants, which are absolute values. The remaining `BoxView` objects all have their position set by using at least one relative value. For example, the yellow `BoxView` object sets the `XConstraint` attached property to the width of its parent (the `RelativeLayout` minus 40). Similarly, this `BoxView` sets the `YConstraint` attached property to the height of its parent minus 40. This ensures that the yellow `BoxView` appears in the lower-right corner of the screen.

> [!NOTE]
> `BoxView` objects that don't specify a size are automatically sized to 40x40 by Xamarin.Forms.

The silver `BoxView` named `oneThird` is positioned centrally, relative to its parent. It's also sized relative to its parent, being one third of its width and height. This is achieved by setting the `XConstraint` and `WidthConstraint` attached properties to the width of the parent (the `RelativeLayout`, multiplied by 0.33). Similarly, the `YConstraint` and `HeightConstraint` attached properties are set to the height of the parent, multiplied by 0.33.

The black `BoxView` is positioned and sized relative to the `oneThird` `BoxView`. This is achieved by setting its `XConstraint` and `YConstraint` attached properties to the `X` and `Y` values, respectively, of the sibling element. Similarly, its size is set to one third of the width and height of its sibling element. This is achieved by setting its `WidthConstraint` and `HeightConstraint` attached properties to the `Width` and `Height` values of the sibling element, respectively, which are then multiplied by 0.33.

The following screenshot shows the resulting layout:

:::image type="content" source="media/relativelayout/relative-values.png" alt-text="Screenshot of children placed in a RelativeLayout using relative values.":::

### Constraint objects

The `Constraint` class defines the following public static methods, which return `Constraint` objects:

- `Constant`, which constrains a child to a size specified with a `double`.
- `FromExpression`, which constrains a child using a lambda expression.
- `RelativeToParent`, which constrains a child relative to its parent's size.
- `RelativeToView`, which constrains a child relative to the size of a view.

In addition, the `BoundsConstraint` class defines a single method, `FromExpression`, which returns a `BoundsConstraint` that constrains a child's position and size with a `Expression<Func<Rect>>`. This method can be used to set the `BoundsConstraint` attached property.

The following C# code shows a `RelativeLayout` whose children are constrained by `Constraint` objects:

```csharp
public class RelativePositioningAndSizingDemoPage : ContentPage
{
    public RelativePositioningAndSizingDemoPage()
    {
        RelativeLayout relativeLayout = new RelativeLayout();

        // Four BoxView's
        relativeLayout.Children.Add(
            new BoxView { Color = Colors.Red },
            Constraint.Constant(0),
            Constraint.Constant(0));

        relativeLayout.Children.Add(
            new BoxView { Color = Colors.Green },
            Constraint.RelativeToParent((parent) =>
            {
                return parent.Width - 40;
            }), Constraint.Constant(0));

        relativeLayout.Children.Add(
            new BoxView { Color = Colors.Blue },
            Constraint.Constant(0),
            Constraint.RelativeToParent((parent) =>
            {
                return parent.Height - 40;
            }));

        relativeLayout.Children.Add(
            new BoxView { Color = Colors.Yellow },
            Constraint.RelativeToParent((parent) =>
            {
                return parent.Width - 40;
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return parent.Height - 40;
            }));

        // Centered and 1/3 width and height of parent
        BoxView silverBoxView = new BoxView { Color = Colors.Silver };
        relativeLayout.Children.Add(
            silverBoxView,
            Constraint.RelativeToParent((parent) =>
            {
                return parent.Width * 0.33;
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * 0.33;
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return parent.Width * 0.33;
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * 0.33;
            }));

        // 1/3 width and height of previous
        relativeLayout.Children.Add(
            new BoxView { Color = Colors.Black },
            Constraint.RelativeToView(silverBoxView, (parent, sibling) =>
            {
                return sibling.X;
            }),
            Constraint.RelativeToView(silverBoxView, (parent, sibling) =>
            {
                return sibling.Y;
            }),
            Constraint.RelativeToView(silverBoxView, (parent, sibling) =>
            {
                return sibling.Width * 0.33;
            }),
            Constraint.RelativeToView(silverBoxView, (parent, sibling) =>
            {
                return sibling.Height * 0.33;
            }));

        Title = "RelativeLayout demo";
        Content = relativeLayout;
    }
}
```

In this example, children are added to the `RelativeLayout` using the `Add` overload that requires an optional `Constraint` object for the `x`, `y`, `width`, and `height` arguments.

> [!NOTE]
> A `RelativeLayout` that uses relative values can position and size children so that they don't fit within the bounds of the layout.
