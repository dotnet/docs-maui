---
title: "Custom layouts"
description: ""
ms.date: 01/22/2024
---

# Custom layouts

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-customlayouts/)

.NET Multi-platform App UI (.NET MAUI) defines multiple layouts classes that each arrange their children in a different way. A layout can be thought of as list of views with rules and properties that define how to arrange those views within the layout. Examples of layouts include <xref:Microsoft.Maui.Controls.Grid>, <xref:Microsoft.Maui.Controls.AbsoluteLayout>, and <xref:Microsoft.Maui.Controls.VerticalStackLayout>.

In .NET MAUI, the layout classes derive from the abstract <xref:Microsoft.Maui.Controls.Layout> class. This class delegates cross-platform layout and measurement to a layout manager class. The <xref:Microsoft.Maui.Controls.Layout> class also contains an overridable <xref:Microsoft.Maui.Controls.Layout.CreateLayoutManager> method that derived layouts can use to specify the layout manager.

Each layout manager class implements the <xref:Microsoft.Maui.Layouts.ILayoutManager> interface, which specifies that <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> and <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementations must be provided:

-  The <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> implementation should call measure on each <xref:Microsoft.Maui.IView> in the layout, and should return the total size of the layout given the constraints.
- The <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementation should determine where each <xref:Microsoft.Maui.IView> should be placed within the given bounds, and should call <xref:Microsoft.Maui.IView.Arrange%2A> on each <xref:Microsoft.Maui.IView> with its appropriate bounds. The return value should be the actual size of the layout.

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

In this example, assume that the cross-platform view being measured is a <xref:Microsoft.Maui.Controls.ContentView> that contains a <xref:Microsoft.Maui.Controls.Label>. A native platform, such as Android, needs to know the size of the <xref:Microsoft.Maui.Controls.ContentView>, given constraints of a width of 100 and a height of 200. The platform calls its `Measure` method on the backing view for the <xref:Microsoft.Maui.Controls.ContentView> (which on Android is a `ContentViewGroup`) with the constraints. The backing view converts the constraints to cross-platform units, if required, and then calls its `CrossPlatformMeasure` method with those constraints to determine how large the <xref:Microsoft.Maui.Controls.Label> should be. The `CrossPlatformMeasure` method is responsible for calling the `Measure` method on the <xref:Microsoft.Maui.Controls.Label>. The <xref:Microsoft.Maui.Controls.Label> measures its native control, and updates its <xref:Microsoft.Maui.IView.DesiredSize> property based on that measurement. This value is returned to the backing view as the result of the `CrossPlatformMeasure` method. The backing view then does whatever internal processing it needs to do, and returns its measured size to the platform.

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

In this example, which builds on the previous example, the `Measure` method for the <xref:Microsoft.Maui.Controls.Label> takes the constraints it's given by the `CrossPlatformMeasure` method and makes any appropriate adjustments, such as subtracting its margins. It then passes the updated constraints to the <xref:Microsoft.Maui.IViewHandler.GetDesiredSize%2A> method of its handler. The handler is aware of the native control (a <xref:Android.Widget.TextView> on Android), and converts the constraints to native values and calls the native control's version of `Measure`. The handler then takes the return value from the native measurement and converts it back to cross-platform values, if required, and returns it to the <xref:Microsoft.Maui.Controls.Label>. The <xref:Microsoft.Maui.Controls.Label> adjusts the result (for example, by adding back its margins), if required, and then sets the result in its <xref:Microsoft.Maui.IView.DesiredSize> property. It then returns the value as a result of the `Measure` method.

## Create a custom layout

.NET MAUI's layouts have pre-defined layout managers to handle their layout. There are two main approaches to defining your own custom layout logic:

1. Create a custom layout type, which is usually a sub-class of an existing layout type or of <xref:Microsoft.Maui.Controls.Layout>, and override the  <xref:Microsoft.Maui.Controls.Layout.CreateLayoutManager> method in your custom layout type. Then, provide an <xref:Microsoft.Maui.Layouts.ILayoutManager> implementation that contains your custom layout logic. For more information, see [Create a custom layout type](#create-a-custom-layout-type).
1. Implement <xref:Microsoft.Maui.Controls.ILayoutManagerFactory> and register the implementation with the app's service provider. This will replace .NET MAUI's default layout manager with your own. For more information, see [Implement a layout manager factory](#implement-a-layout-manager-factory).

### Create a custom layout type



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

## Notes


Any layout pass should typically call `Measure` before calling `Arrange`. `Measure` may be called multiple times before calling `Arrange`, because a platform may need to perform some speculative measurements before arranging views.

`Arrange` can be called multiple times at different sizes or locations, provided that `Measure` has been called at least once. For example, a desktop app may determine that a window resizing operation requires arranging a view at a different location, but that the changes to the window size could not have affected the measurements of the view.

Each platform generally handles its own optimization of measurement operations. For example, if the cross-platform layer calls `Measure` on an Android view twice in a row with the same `measureSpec` values, the native Android code will return the cached value unless it determines that there's a good reason for the native view to be remeasured.

## Layout implementation

The `ILayout` interface is composed of a few other interfaces. It provides a `ClipsToBounds` property, which determines whether its child views can be displayed outside of its boundaries, or are clipped at the edges.

> [!NOTE]
> The value of `ClipToBounds` is provided by the `IsClippedToBounds` property of `Layout`. For migration convenience from Xamarin.Forms, the `IsClippedToBounds` property defaults to `false`, so .NET MAUI's layouts do not clip by default.

While it's technically possible to implement a cross-platform layout directly in an implementation of `ILayout`, that work is instead delegated to an implementation of `ILayoutManager`.

`ILayoutManager` defines `Measure` and `ArrangeChildren` methods. All of the logic to handle cross-platform layout is contained in the `ILayoutManager` implementation.

---

https://github.com/dotnet/maui/blob/main/docs/design/layout.md
https://learn.microsoft.com/xamarin/xamarin-forms/user-interface/layouts/custom
https://github.com/hartez/CustomLayoutExamples

This sample demonstrates the following custom layouts:

- CascadeLayout, which cascades items from top left to bottom right, similar to using the cascade windows arrangement in an MDI application.
- ColumnLayout, which is similar to the Xamarin.Forms *AndExpand properties. It's a subclass of VerticalStackLayout, which adds a Fill attached property that can be applied to one or more children of the layout. It uses a custom layout manager that converts the VerticalStackLayout into a single-column Grid at runtime. Each VerticalStackLayout child gets its own row in the Grid. The rows are set to a height of Auto, but children marked as Fill receive a row height of * instead.
- ContentColumnLayout, which is a custom layout that displays a header, content, and footer. It subclasses Layout and implements some extra properties and methods from the IGridLayout interface. This enables it to be passed into the GridLayoutManager to handle the layout at runtime.
- HorizontalWrapLayout, which works like a horizontal stack layout, except that instead of extending out as far as it needs to the right, it will wrap to a new row when it encounters the right edge of its container.
- ZStackLayout, which is a variation of a StackLayout that arranges its children on top of each other. All its children are laid out at the origin. The arrangement area's width is determined by the widest child and the height is determined by the tallest child.
