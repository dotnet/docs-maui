---
title: "Custom layouts"
description: "Learn how to create a custom layout to organize page content using a layout that isn't provided by .NET MAUI."
ms.date: 01/22/2024
---

# Custom layouts

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-customlayouts/)

.NET Multi-platform App UI (.NET MAUI) defines multiple layouts classes that each arrange their children in a different way. A layout can be thought of as list of views with rules and properties that define how to arrange those views within the layout. Examples of layouts include <xref:Microsoft.Maui.Controls.Grid>, <xref:Microsoft.Maui.Controls.AbsoluteLayout>, and <xref:Microsoft.Maui.Controls.VerticalStackLayout>.

In .NET MAUI, the layout classes derive from the abstract <xref:Microsoft.Maui.Controls.Layout> class. This class delegates cross-platform layout and measurement to a layout manager class. The <xref:Microsoft.Maui.Controls.Layout> class also contains an overridable <xref:Microsoft.Maui.Controls.Layout.CreateLayoutManager> method that derived layouts can use to specify the layout manager.

Each layout manager class implements the <xref:Microsoft.Maui.Layouts.ILayoutManager> interface, which specifies that <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> and <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementations must be provided:

-  The <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> implementation calls <xref:Microsoft.Maui.IView.Measure%2A> on each <xref:Microsoft.Maui.IView> in the layout, and returns the total size of the layout given the constraints.
- The <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementation determines where each <xref:Microsoft.Maui.IView> should be placed within the bounds of the layout, and calls <xref:Microsoft.Maui.IView.Arrange%2A> on each <xref:Microsoft.Maui.IView> with its appropriate bounds. The return value is the actual size of the layout.

Sometimes it's necessary to organize page content using a layout that isn't provided by .NET MAUI. This can be achieved by writing your own layout logic. However, an understanding of how .NET MAUI's cross-platform layout process works is first required.

## Layout process

.NET MAUI's cross-platform layout process builds on top of the native layout process on each platform. Generally, the layout process is initiated by the native layout system. The cross-platform process runs when a layout or content control initiates it as a result of being measured or arranged by the native layout system.

> [!NOTE]
> Each platform handles layout slightly differently. One of the goal of .NET MAUI's cross-platform layout process is to be as platform-agnostic as possible.

The following diagram shows the process when the native layout system wants to measure a backing view:

:::image type="content" source="media/custom-layout/layout-process.png" alt-text="The layout process in .NET MAUI" border="false":::

<!-- ```mermaid
sequenceDiagram
    participant P as Platform
    participant BV as Backing view
    participant XV as Cross-platform view
    P->>BV: Measure
    BV->>XV: Cross-platform Measure
    Note over XV: Update DesiredSize
    XV->>BV: DesiredSize
    Note over BV: Internal processing
    BV->>P: Size
``` -->

In this example, assume that the cross-platform view being measured is a <xref:Microsoft.Maui.Controls.ContentView> that contains a <xref:Microsoft.Maui.Controls.Label>. A native platform, such as Android, needs to know the size of the <xref:Microsoft.Maui.Controls.ContentView>, given constraints of a width of 100 and a height of 200. The platform calls its `Measure` method on the backing view for the <xref:Microsoft.Maui.Controls.ContentView> (which on Android is a `ContentViewGroup`) with the constraints. The backing view converts the constraints to cross-platform units, if required, and then calls its `CrossPlatformMeasure` method with those constraints to determine how large the <xref:Microsoft.Maui.Controls.Label> should be. The `CrossPlatformMeasure` method is responsible for calling the <xref:Microsoft.Maui.IView.Measure%2A> method on the <xref:Microsoft.Maui.Controls.Label>. The <xref:Microsoft.Maui.Controls.Label> measures its native control, and updates its <xref:Microsoft.Maui.IView.DesiredSize> property based on that measurement. This value is returned to the backing view as the result of the `CrossPlatformMeasure` method. The backing view then does whatever internal processing it needs to do, and returns its measured size to the platform.

### Layout measurement

The process for layout measurement is as described above, except that multiple child views need to be measured:

:::image type="content" source="media/custom-layout/measure-layout.png" alt-text="The process for layout measurement in .NET MAUI" border="false":::

<!-- ```mermaid
sequenceDiagram
    participant P as Platform
    participant BV as Layout backing view
    participant XV as Cross-platform layout
    participant C as Child view
    P->>BV: Measure
    BV->>XV: Cross-Platform Measure
    loop Each child
        XV->>C: Measure
        C->>XV: DesiredSize
    end
    Note over XV: Update DesiredSize
    XV->>BV: DesiredSize
    Note over BV: Internal processing
    BV->>P: Size
``` -->

The process of iterating over the child views and measuring each one is handled by the <xref:Microsoft.Maui.Layouts.ILayoutManager> implementation for each type of layout, inside of the layout manager's <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> method.

### View measurement

When a cross-platform view is measured, it passes the measurement to its native view. This process is shown in the following diagram:

:::image type="content" source="media/custom-layout/measure-layout.png" alt-text="The process for layout measurement in .NET MAUI" border="false":::

<!-- ```mermaid
sequenceDiagram
	participant V as View
	participant H as Handler
	participant NV as Native view
	Note over V: Adjust for margins
	V->>H: GetDesiredSize
	Note over H: Convert to native
	H->>NV: Measure
	NV->>H: Size
	Note over H: Convert to cross-platform
	H->>V: Size
	Note over V: Adjust for margins, record DesiredSize
``` -->

In this example, which builds on the previous example, the <xref:Microsoft.Maui.IView.Measure%2A> method for the <xref:Microsoft.Maui.Controls.Label> takes the constraints it's given by the `CrossPlatformMeasure` method and makes any appropriate adjustments, such as subtracting its margins. It then passes the updated constraints to the <xref:Microsoft.Maui.IViewHandler.GetDesiredSize%2A> method of its handler. The handler is aware of the native control (a <xref:Android.Widget.TextView> on Android), and converts the constraints to native values and calls the native control's version of `Measure`. The handler then takes the return value from the native measurement and converts it back to cross-platform values, if required, and returns it to the <xref:Microsoft.Maui.Controls.Label>. The <xref:Microsoft.Maui.Controls.Label> adjusts the result (for example, by adding back its margins), if required, and then sets the result in its <xref:Microsoft.Maui.IView.DesiredSize> property. It then returns the value as a result of the `Measure` method.

## Create a custom layout

.NET MAUI's layouts have pre-defined layout managers to handle their layout. There are two main approaches to defining your own custom layout logic:

1. Create a custom layout type, which is usually a sub-class of an existing layout type or of <xref:Microsoft.Maui.Controls.Layout>, and override the  <xref:Microsoft.Maui.Controls.Layout.CreateLayoutManager> method in your custom layout type. Then, provide an <xref:Microsoft.Maui.Layouts.ILayoutManager> implementation that contains your custom layout logic. For more information, see [Create a custom layout type](#create-a-custom-layout-type).
1. Implement <xref:Microsoft.Maui.Controls.ILayoutManagerFactory> and register the implementation with the app's service provider. This will replace .NET MAUI's default layout manager with your own. For more information, see [Implement a layout manager factory](#implement-a-layout-manager-factory).

### Create a custom layout type

The process for creating a custom layout type is as follows:

1. Create a class that sub-classes an existing layout type or the <xref:Microsoft.Maui.Controls.Layout> class, and override the  <xref:Microsoft.Maui.Controls.Layout.CreateLayoutManager> method in your custom layout type. For more information, see [Sub-class a layout](#sub-class-a-layout).
1. Create a layout manager class that derives from an existing layout manager, or that implements the <xref:Microsoft.Maui.Layouts.ILayoutManager> interface directly. In your layout manager class, you should:
    1. Add a constructor that accepts and stores an instance of your custom layout type.
    1. Override, or implement, the <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> method to calculate the total size of the layout given its constraints.
    1. Override, or implement, the <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> method to size and position all the children within the layout.

    For more information, see [Create a layout manager](#create-a-layout-manager).

1. Consume your custom layout type. For more information, see [Consume the layout type](#consume-the-layout-type).

An orientation-sensitive `HorizontalWrapLayout` is used to demonstrate this process. `HorizontalWrapLayout` is similar to a <xref:Microsoft.Maui.Controls.HorizontalStackLayout> in that it arranges its children horizontally across the page. However, it wraps the display of children to a new row when it encounters the right edge of its container

> [!NOTE]
> The [sample](/samples/dotnet/maui-samples/userinterface-customlayouts/) defines additional custom layouts that can be used to understand how to write your own custom layout.

### Sub-class a layout

The first step in creating a custom layout type is to sub-class an existing layout type or the <xref:Microsoft.Maui.Controls.Layout> class. Then, override the <xref:Microsoft.Maui.Controls.Layout.CreateLayoutManager> method in your custom layout type, and return a new instance of the layout manager for the custom layout type:

```csharp
using Microsoft.Maui.Layouts;

public class HorizontalWrapLayout : HorizontalStackLayout
{
    protected override ILayoutManager CreateLayoutManager()
    {
        return new HorizontalWrapLayoutManager(this);
    }
}
```

`HorizontalWrapLayout` derives from <xref:Microsoft.Maui.Controls.HorizontalStackLayout> to use its base layout functionality. .NET MAUI layouts delegate cross-platform layout and measurement to a layout manager class. Therefore, the <xref:Microsoft.Maui.Controls.Layout.CreateLayoutManager> override returns a new instance of the `HorizontalWrapLayoutManager` class, which is the layout manager that's discussed in the next section.

### Create a layout manager

A layout manager class is used to perform cross-platform layout and measurement for your custom layout type. It should derive from an existing layout manager, or it should directly implement the  <xref:Microsoft.Maui.Layouts.ILayoutManager> interface. `HorizontalWrapLayoutManager` derives from <xref:Microsoft.Maui.Layouts.HorizontalStackLayoutManager> so that it can use its underlying functionality and access members in its inheritance hierarchy:

```csharp
using Microsoft.Maui.Layouts;
using HorizontalStackLayoutManager = Microsoft.Maui.Layouts.HorizontalStackLayoutManager;

public class HorizontalWrapLayoutManager : HorizontalStackLayoutManager
{
    HorizontalWrapLayout _layout;

    public HorizontalWrapLayoutManager(HorizontalWrapLayout horizontalWrapLayout) : base(horizontalWrapLayout)
    {
        _layout = horizontalWrapLayout;
    }

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
    }

    public override Size ArrangeChildren(Rect bounds)
    {
    }
}
```

The `HorizontalWrapLayoutManager` constructor stores an instance of the `HorizontalWrapLayout` type in a field, so that it can be accessed throughout the layout manager. The layout manager also overrides the <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> and <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> methods from the <xref:Microsoft.Maui.Layouts.HorizontalStackLayoutManager> class. These methods are where you'll define the logic to implement your custom layout.

#### Measure the layout size

The purpose of the <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> implementation is to calculate the total size of the layout given its constraints. It should does this by calling <xref:Microsoft.Maui.IView.Measure%2A> on each <xref:Microsoft.Maui.IView> in the layout. It can then use this data to calculate and return the total size of the layout given its constraints.

The following example shows the <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> implementation for the `HorizontalWrapLayoutManager` class:

```csharp
public override Size Measure(double widthConstraint, double heightConstraint)
{
    var padding = _layout.Padding;

    widthConstraint -= padding.HorizontalThickness;

    double currentRowWidth = 0;
    double currentRowHeight = 0;
    double totalWidth = 0;
    double totalHeight = 0;

    for (int n = 0; n < _layout.Count; n++)
    {
        var child = _layout[n];
        if (child.Visibility == Visibility.Collapsed)
        {
            continue;
        }

        var measure = child.Measure(double.PositiveInfinity, heightConstraint);

        // Will adding this IView put us past the edge?
        if (currentRowWidth + measure.Width > widthConstraint)
        {
            // Keep track of the width so far
            totalWidth = Math.Max(totalWidth, currentRowWidth);
            totalHeight += currentRowHeight;

            // Account for spacing
            totalHeight += _layout.Spacing;

            // Start over at 0
            currentRowWidth = 0;
            currentRowHeight = measure.Height;
        }
        currentRowWidth += measure.Width;
        currentRowHeight = Math.Max(currentRowHeight, measure.Height);

        if (n < _layout.Count - 1)
        {
            currentRowWidth += _layout.Spacing;
        }
    }

    // Account for the last row
    totalWidth = Math.Max(totalWidth, currentRowWidth);
    totalHeight += currentRowHeight;

    // Account for padding
    totalWidth += padding.HorizontalThickness;
    totalHeight += padding.VerticalThickness;

    // Ensure that the total size of the layout fits within its constraints
    var finalWidth = ResolveConstraints(widthConstraint, Stack.Width, totalWidth, Stack.MinimumWidth, Stack.MaximumWidth);
    var finalHeight = ResolveConstraints(heightConstraint, Stack.Height, totalHeight, Stack.MinimumHeight, Stack.MaximumHeight);

    return new Size(finalWidth, finalHeight);
}
```

The `Measure` method enumerates through all of the visible children in the layout, invoking the <xref:Microsoft.Maui.IView.Measure%2A> on each child. It then returns the total size of the layout, taking into account the constraints and the values of the <xref:Microsoft.Maui.Controls.Layout.Padding> and <xref:Microsoft.Maui.Controls.StackBase.Spacing> properties. The <xref:Microsoft.Maui.Layouts.ResolveConstraints> method is called to ensure that the total size of the layout fits within its constraints.

> [!IMPORTANT]
> When enumerating children in the <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> implementation, skip any child whose <xref:Visibility> property is set to `false`. This ensures that the custom layout won't leave space for invisible children.

#### Arrange children in the layout

The purpose of the <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementation is to size and position all of the children within the layout. To determine where each <xref:Microsoft.Maui.IView> should be placed within the bounds of the layout, it should call <xref:Microsoft.Maui.IView.Arrange%2A> on each <xref:Microsoft.Maui.IView> with its appropriate bounds. It should return a value that represents the actual size of the layout.

> [!WARNING]
> Failure to invoke the <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> method on each child in the layout will result in the child never receiving a correct size or position, and hence the child won't become visible on the page.

The following example shows the <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementation for the `HorizontalWrapLayoutManager` class:

```csharp
public override Size ArrangeChildren(Rect bounds)
{
    var padding = Stack.Padding;
    double top = padding.Top + bounds.Top;
    double left = padding.Left + bounds.Left;

    double currentRowTop = top;
    double currentX = left;
    double currentRowHeight = 0;

    double maxStackWidth = currentX;

    for (int n = 0; n < _layout.Count; n++)
    {
        var child = _layout[n];
        if (child.Visibility == Visibility.Collapsed)
        {
            continue;
        }

        if (currentX + child.DesiredSize.Width > bounds.Right)
        {
            // Keep track of our maximum width so far
            maxStackWidth = Math.Max(maxStackWidth, currentX);

            // Move down to the next row
            currentX = left;
            currentRowTop += currentRowHeight + _layout.Spacing;
            currentRowHeight = 0;
        }

        var destination = new Rect(currentX, currentRowTop, child.DesiredSize.Width, child.DesiredSize.Height);
        child.Arrange(destination);

        currentX += destination.Width + _layout.Spacing;
        currentRowHeight = Math.Max(currentRowHeight, destination.Height);
    }

    var actual = new Size(maxStackWidth, currentRowTop + currentRowHeight);

    // Adjust the size if the layout is set to fill its container
    return actual.AdjustForFill(bounds, Stack);
}
```

The `ArrangeChildren` method enumerates through all the visible children in the layout to size and position them within the layout. It does this by invoking <xref:Microsoft.Maui.IView.Arrange%2A> on each child with appropriate bounds, that take into account the <xref:Microsoft.Maui.Controls.Layout.Padding> and <xref:Microsoft.Maui.Controls.StackBase.Spacing> of the underlying layout. It then returns the actual size of the layout. The <xref:Microsoft.Maui.Layouts.LayoutExtensions.AdjustForFill%2A> method is called to ensure that the size takes into account whether the the layout has its <xref:Microsoft.Maui.Controls.IView.HorizontalLayoutAlignment> and <xref:Microsoft.Maui.Controls.IView.VerticalLayoutOptions> properties set to <xref:Microsoft.Maui.Controls.LayoutOptions.Fill?displayProperty=nameWithType>.

> [!IMPORTANT]
> When enumerating children in the <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementation, skip any child whose <xref:Visibility> property is set to `false`. This ensures that the custom layout won't leave space for invisible children.

### Consume the layout type

The `HorizontalWrapLayout` class can be consumed by placing it in a <xref:Microsoft.Maui.Controls.Page> derived type:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:layouts="clr-namespace:CustomLayoutDemos.Layouts"
             x:Class="CustomLayoutDemos.Views.HorizontalWrapLayoutPage"
             Title="Horizontal wrap layout">
    <ScrollView Margin="20">
        <layouts:HorizontalWrapLayout Spacing="20">
            <Image Source="img_0074.jpg"
                   WidthRequest="150" />
            <Image Source="img_0078.jpg"
                   WidthRequest="150" />
            <Image Source="img_0308.jpg"
                   WidthRequest="150" />
            <Image Source="img_0437.jpg"
                   WidthRequest="150" />
            <Image Source="img_0475.jpg"
                   WidthRequest="150" />
            <Image Source="img_0613.jpg"
                   WidthRequest="150" />
            <!-- More images go here -->
        </layouts:HorizontalWrapLayout>
    </ScrollView>
</ContentPage>
```

Controls can be added to the `HorizontalWrapLayout` as required. When the page containing the `HorizontalWrapLayout` appears, the <xref:Microsoft.Maui.Controls.Image> controls are displayed:

:::image type="content" source="media/custom-layout/horizontal-wrap-layout-2-column.png" alt-text="Screenshot of the horizontal wrap layout on a Mac with two columns.":::

The number of columns in each row depends on the photo size, the width of the page, and the number of pixels per device-independent unit:

:::image type="content" source="media/custom-layout/horizontal-wrap-layout-5-column.png" alt-text="Screenshot of the horizontal wrap layout on a Mac with five columns.":::

> [!NOTE]
> Scrolling is supported by wrapping the `HorizontalWrapLayout` in a <xref:Microsoft.Maui.Controls.ScrollView>.

### Implement a layout manager factory

In some scenarios you may find that you want to change the behavior of an existing layout type without having to create a custom layout type. For these scenarios you can create a type that implements <xref:Microsoft.Maui.Controls.ILayoutManagerFactory> and use it to replace .NET MAUI's default layout manager with your own. This enables you to define a new layout manager for an existing layout type, such as providing a custom layout manager for <xref:Microsoft.Maui.Controls.Grid> with different layouts. This can be useful for scenarios where you want to add a new behavior to a layout but don't want to update the type of an existing widely-used layout in your app.

The process for implementing a layout manager factory is:

1. Create a layout manager that derives from one of .NET MAUI's layout manager types. For more information, see [Create a custom layout manager](#create-a-custom-layout-manager).
1. Create a type that implements <xref:Microsoft.Maui.Controls.ILayoutManagerFactory>. For more information, see [Create a layout manager factory](#create-a-layout-manager-factory).
1. Register your layout manager factory with the app's service provider. For more information, see [Register the layout manager factory](#register-the-layout-manager-factory).

#### Create a custom layout manager

The first step in changing the behavior of an existing layout type is to create a custom layout manager that derives from the layout manager for the layout class:

```csharp
using Microsoft.Maui.Layouts;

public class CustomGridLayoutManager : GridLayoutManager
{
    public CustomGridLayoutManager(IGridLayout layout) : base(layout)
    {
    }

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        EnsureRows();
        return base.Measure(widthConstraint, heightConstraint);
    }

    void EnsureRows()
    {
        if (Grid is not Grid grid)
        {
            return;
        }

        // Find the maximum row value from the child views
        int maxRow = 0;
        foreach (var child in grid)
        {
            maxRow = Math.Max(grid.GetRow(child), maxRow);
        }

        // Add more rows if we need them
        for (int n = grid.RowDefinitions.Count; n <= maxRow; n++)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        }
    }
}
```

In this example, `CustomGridLayoutManager` derives from .NET MAUI's <xref:Microsoft.Maui.Layouts.GridLayoutManager> class, and overrides its <xref:Microsoft.Maui.Layouts.GridLayoutManager.Measure%2A> method. This custom layout manager ensures that the <xref:Microsoft.Maui.Controls.Grid.RowDefinitions> for the <xref:Microsoft.Maui.Controls.Grid> includes enough rows to account for each `Grid.Row` attached property set in a child view. Without this modification, the <xref:Microsoft.Maui.Controls.Grid.RowDefinitions> for the <xref:Microsoft.Maui.Controls.Grid> would have to be specified at design time.

#### Create a layout manager factory

The custom layout manager can then be created in a layout manager factory. This is achieved by creating a type that implements the <xref:Microsoft.Maui.Controls.ILayoutManagerFactory> interface:

```csharp
using Microsoft.Maui.Layouts;

public class CustomLayoutManagerFactory : ILayoutManagerFactory
{
    public ILayoutManager CreateLayoutManager(Layout layout)
    {
        if (layout is Grid)
        {
            return new CustomGridLayoutManager(layout as IGridLayout);
        }
        return null;
    }
}
```

In this example, a `CustomGridLayoutManager` instance is returned if the layout is a <xref:Microsoft.Maui.Controls.Grid>.

#### Register the layout manager factory

The layout manager factory should then be registered with your app's service provider in your `MauiProgram` class:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Setup a custom layout manager so the default manager for the Grid can be replaced.
        builder.Services.Add(new ServiceDescriptor(typeof(ILayoutManagerFactory), new CustomLayoutManagerFactory()));

        return builder.Build();
    }
}
```

The overall effect is that when the app renders a <xref:Microsoft.Maui.Controls.Grid> it will use the custom layout manager to ensures that the <xref:Microsoft.Maui.Controls.Grid.RowDefinitions> for the <xref:Microsoft.Maui.Controls.Grid> includes enough rows to account for each `Grid.Row` attached property set in a child view. This ensures that the following <xref:Microsoft.Maui.Controls.Grid> displays correctly, without setting its <xref:Microsoft.Maui.Controls.Grid.RowDefinitions> property:

```xaml
<Grid>
    <Label Text="This Grid demonstrates replacing the LayoutManager for an existing layout type." />
    <Label Grid.Row="1"
           Text="In this case, it's a LayoutManager for Grid which automatically adds enough rows to accommodate the rows specified in the child views' attached properties." />
    <Label Grid.Row="2"
           Text="Notice that the Grid doesn't explicitly specify a RowDefinitions collection." />
    <Label Grid.Row="3"
           Text="In MauiProgram.cs, an instance of an ILayoutManagerFactory has been added that replaces the default GridLayoutManager. The custom manager will automatically add the necessary RowDefinitions at runtime." />
    <Label Grid.Row="5"
           Text="We can even skip some rows, and it will add the intervening ones for us (notice the gap between the previous label and this one)." />
</Grid>
```
