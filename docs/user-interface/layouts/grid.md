---
title: "Grid"
description: "The .NET MAUI Grid is a layout that organizes its children into rows and columns of cells."
ms.date: 08/30/2024
---

# Grid

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-grid)

:::image type="content" source="media/grid/layouts.png" alt-text=".NET MAUI Grid." border="false":::

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Grid>, is a layout that organizes its children into rows and columns, which can have proportional or absolute sizes. By default, a <xref:Microsoft.Maui.Controls.Grid> contains one row and one column. In addition, a <xref:Microsoft.Maui.Controls.Grid> can be used as a parent layout that contains other child layouts.

The <xref:Microsoft.Maui.Controls.Grid> should not be confused with tables, and is not intended to present tabular data. Unlike HTML tables, a <xref:Microsoft.Maui.Controls.Grid> is intended for laying out content. For displaying tabular data, consider using a [ListView](~/user-interface/controls/listview.md) or [CollectionView](~/user-interface/controls/collectionview/index.md).

The <xref:Microsoft.Maui.Controls.Grid> class defines the following properties:

- `Column`, of type `int`, which is an attached property that indicates the column alignment of a view within a parent <xref:Microsoft.Maui.Controls.Grid>. The default value of this property is 0. A validation callback ensures that when the property is set, its value is greater than or equal to 0.
- `ColumnDefinitions`, of type `ColumnDefinitionCollection`, is a list of `ColumnDefinition` objects that define the width of the grid columns.
- `ColumnSpacing`, of type `double`, indicates the distance between grid columns. The default value of this property is 0.
- `ColumnSpan`, of type `int`, which is an attached property that indicates the total number of columns that a view spans within a parent <xref:Microsoft.Maui.Controls.Grid>. The default value of this property is 1. A validation callback ensures that when the property is set, its value is greater than or equal to 1.
- `Row`, of type `int`, which is an attached property that indicates the row alignment of a view within a parent <xref:Microsoft.Maui.Controls.Grid>. The default value of this property is 0. A validation callback ensures that when the property is set, its value is greater than or equal to 0.
- `RowDefinitions`, of type `RowDefinitionCollection`, is a list of `RowDefinition` objects that define the height of the grid rows.
- `RowSpacing`, of type `double`, indicates the distance between grid rows. The default value of this property is 0.
- `RowSpan`, of type `int`, which is an attached property that indicates the total number of rows that a view spans within a parent <xref:Microsoft.Maui.Controls.Grid>. The default value of this property is 1. A validation callback ensures that when the property is set, its value is greater than or equal to 1.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings and styled.

## Rows and columns

By default, a <xref:Microsoft.Maui.Controls.Grid> contains one row and one column:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="GridTutorial.MainPage">
    <Grid Margin="20,35,20,20">
        <Label Text="By default, a Grid contains one row and one column." />
    </Grid>
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.Grid> contains a single child <xref:Microsoft.Maui.Controls.Label> that's automatically positioned in a single location:

:::image type="content" source="media/grid/default.png" alt-text="Default .NET MAUI Grid layout.":::

The layout behavior of a <xref:Microsoft.Maui.Controls.Grid> can be defined with the `RowDefinitions` and `ColumnDefinitions` properties, which are collections of `RowDefinition` and `ColumnDefinition` objects, respectively. These collections define the row and column characteristics of a <xref:Microsoft.Maui.Controls.Grid>, and should contain one `RowDefinition` object for each row in the <xref:Microsoft.Maui.Controls.Grid>, and one `ColumnDefinition` object for each column in the <xref:Microsoft.Maui.Controls.Grid>.

The `RowDefinition` class defines a `Height` property, of type `GridLength`, and the `ColumnDefinition` class defines a `Width` property, of type `GridLength`. The `GridLength` struct specifies a row height or a column width in terms of the `GridUnitType` enumeration, which has three members:

- `Absolute` – the row height or column width is a value in device-independent units (a number in XAML).
- `Auto` – the row height or column width is autosized based on the cell contents (`Auto` in XAML).
- `Star` – leftover row height or column width is allocated proportionally (a number followed by `*` in XAML).

A <xref:Microsoft.Maui.Controls.Grid> row with a `Height` property of `Auto` constrains the height of views in that row in the same way as a vertical <xref:Microsoft.Maui.Controls.StackLayout>. Similarly, a column with a `Width` property of `Auto` works much like a horizontal <xref:Microsoft.Maui.Controls.StackLayout>.

> [!CAUTION]
> Try to ensure that as few rows and columns as possible are set to `Auto` size. Each auto-sized row or column will cause the layout engine to perform additional layout calculations. Instead, use fixed size rows and columns if possible. Alternatively, set rows and columns to occupy a proportional amount of space with the `GridUnitType.Star` enumeration value.

The following XAML shows how to create a <xref:Microsoft.Maui.Controls.Grid> with three rows and two columns:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="GridDemos.Views.XAML.BasicGridPage"
             Title="Basic Grid demo">
   <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="2*" />
            <RowDefinition Height="*" />
            <RowDefinition Height="100" />
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>
        ...
    </Grid>
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.Grid> has an overall height that is the height of the page. The <xref:Microsoft.Maui.Controls.Grid> knows that the height of the third row is 100 device-independent units. It subtracts that height from its own height, and allocates the remaining height proportionally between the first and second rows based on the number before the star. In this example, the height of the first row is twice that of the second row.

The two `ColumnDefinition` objects both set the `Width` to `*`, which is the same as `1*`, meaning that the width of the screen is divided equally beneath the two columns.

> [!IMPORTANT]
> The default value of the `RowDefinition.Height` property is `*`. Similarly, the default value of the `ColumnDefinition.Width` property is `*`. Therefore, it's not necessary to set these properties in cases where these defaults are acceptable.

Child views can be positioned in specific <xref:Microsoft.Maui.Controls.Grid> cells with the `Grid.Column` and `Grid.Row` attached properties. In addition, to make child views span across multiple rows and columns, use the `Grid.RowSpan` and `Grid.ColumnSpan` attached properties.

The following XAML shows the same <xref:Microsoft.Maui.Controls.Grid> definition, and also positions child views in specific <xref:Microsoft.Maui.Controls.Grid> cells:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="GridDemos.Views.XAML.BasicGridPage"
             Title="Basic Grid demo">
   <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="2*" />
            <RowDefinition />
            <RowDefinition Height="100" />
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition />
            <ColumnDefinition />
        </Grid.ColumnDefinitions>
        <BoxView Color="Green" />
        <Label Text="Row 0, Column 0"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <BoxView Grid.Column="1"
                 Color="Blue" />
        <Label Grid.Column="1"
               Text="Row 0, Column 1"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <BoxView Grid.Row="1"
                 Color="Teal" />
        <Label Grid.Row="1"
               Text="Row 1, Column 0"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <BoxView Grid.Row="1"
                 Grid.Column="1"
                 Color="Purple" />
        <Label Grid.Row="1"
               Grid.Column="1"
               Text="Row1, Column 1"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <BoxView Grid.Row="2"
                 Grid.ColumnSpan="2"
                 Color="Red" />
        <Label Grid.Row="2"
               Grid.ColumnSpan="2"
               Text="Row 2, Columns 0 and 1"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
    </Grid>
</ContentPage>
```

> [!NOTE]
> The `Grid.Row` and `Grid.Column` properties are both indexed from 0, and so `Grid.Row="2"` refers to the third row while `Grid.Column="1"` refers to the second column. In addition, both of these properties have a default value of 0, and so don't need to be set on child views that occupy the first row or first column of a <xref:Microsoft.Maui.Controls.Grid>.

In this example, all three <xref:Microsoft.Maui.Controls.Grid> rows are occupied by <xref:Microsoft.Maui.Controls.BoxView> and <xref:Microsoft.Maui.Controls.Label> views. The third row is 100 device-independent units high, with the first two rows occupying the remaining space (the first row is twice as high as the second row). The two columns are equal in width and divide the <xref:Microsoft.Maui.Controls.Grid> in half. The <xref:Microsoft.Maui.Controls.BoxView> in the third row spans both columns:

:::image type="content" source="media/grid/basic.png" alt-text="Basic .NET MAUI Grid layout.":::

In addition, child views in a <xref:Microsoft.Maui.Controls.Grid> can share cells. The order that the children appear in the XAML is the order that the children are placed in the <xref:Microsoft.Maui.Controls.Grid>. In the previous example, the <xref:Microsoft.Maui.Controls.Label> objects are only visible because they are rendered on top of the <xref:Microsoft.Maui.Controls.BoxView> objects. The <xref:Microsoft.Maui.Controls.Label> objects would not be visible if the <xref:Microsoft.Maui.Controls.BoxView> objects were rendered on top of them.

The equivalent C# code is:

```csharp
public class BasicGridPage : ContentPage
{
    public BasicGridPage()
    {
        Grid grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                new RowDefinition(),
                new RowDefinition { Height = new GridLength(100) }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition(),
                new ColumnDefinition()
            }
        };

        // Row 0
        // The BoxView and Label are in row 0 and column 0, and so only need to be added to the
        // Grid to obtain the default row and column settings.
        grid.Add(new BoxView
        {
            Color = Colors.Green
        });
        grid.Add(new Label
        {
            Text = "Row 0, Column 0",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        });

        // This BoxView and Label are in row 0 and column 1, which are specified as arguments
        // to the Add method.
        grid.Add(new BoxView
        {
            Color = Colors.Blue
        }, 1, 0);
        grid.Add(new Label
        {
            Text = "Row 0, Column 1",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 1, 0);

        // Row 1
        // This BoxView and Label are in row 1 and column 0, which are specified as arguments
        // to the Add method overload.
        grid.Add(new BoxView
        {
            Color = Colors.Teal
        }, 0, 1);
        grid.Add(new Label
        {
            Text = "Row 1, Column 0",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 0, 1);

        // This BoxView and Label are in row 1 and column 1, which are specified as arguments
        // to the Add method overload.
        grid.Add(new BoxView
        {
            Color = Colors.Purple
        }, 1, 1);
        grid.Add(new Label
        {
            Text = "Row1, Column 1",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 1, 1);

        // Row 2
        // Alternatively, the BoxView and Label can be positioned in cells with the Grid.SetRow
        // and Grid.SetColumn methods. Here, the Grid.SetColumnSpan method is used to span two columns.
        BoxView boxView = new BoxView { Color = Colors.Red };
        Grid.SetRow(boxView, 2);
        Grid.SetColumnSpan(boxView, 2);
        Label label = new Label
        {
            Text = "Row 2, Column 0 and 1",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        Grid.SetRow(label, 2);
        Grid.SetColumnSpan(label, 2);

        grid.Add(boxView);
        grid.Add(label);

        Title = "Basic Grid demo";
        Content = grid;
    }
}
```

In code, to specify the height of a `RowDefinition` object, and the width of a `ColumnDefinition` object, you use values of the `GridLength` structure, often in combination with the `GridUnitType` enumeration.

> [!NOTE]
> <xref:Microsoft.Maui.Controls.Grid> also defines an <xref:Microsoft.Maui.Controls.GridExtensions.AddWithSpan%2A> extension method that adds a view to the `Grid` at the specified row and column with the specified row and column spans.

### Simplify row and column definitions

In XAML, the row and column characteristics of a <xref:Microsoft.Maui.Controls.Grid> can be specified using a simplified syntax that avoids having to define `RowDefinition` and `ColumnDefinition` objects for each row and column. Instead, the `RowDefinitions` and `ColumnDefinitions` properties can be set to strings containing comma-delimited `GridUnitType` values, from which type converters built into .NET MAUI create `RowDefinition` and `ColumnDefinition` objects:

```xaml
<Grid RowDefinitions="1*, Auto, 25, 14, 20"
      ColumnDefinitions="*, 2*, Auto, 300">
    ...
</Grid>
```

In this example, the <xref:Microsoft.Maui.Controls.Grid> has five rows and four columns. The third, forth, and fifth rows are set to absolute heights, with the second row auto-sizing to its content. The remaining height is then allocated to the first row.

The forth column is set to an absolute width, with the third column auto-sizing to its content. The remaining width is allocated proportionally between the first and second columns based on the number before the star. In this example, the width of the second column is twice that of the first column (because `*` is identical to `1*`).

## Space between rows and columns

By default, <xref:Microsoft.Maui.Controls.Grid> rows and columns have no space between them. This can be changed by setting the `RowSpacing` and `ColumnSpacing` properties, respectively:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="GridDemos.Views.XAML.GridSpacingPage"
             Title="Grid spacing demo">
    <Grid RowSpacing="6"
          ColumnSpacing="6">
        ...
    </Grid>
</ContentPage>
```

This example creates a <xref:Microsoft.Maui.Controls.Grid> whose rows and columns are separated by 6 device-independent units of space:

:::image type="content" source="media/grid/spacing.png" alt-text=".NET MAUI Grid with spacing between cells.":::

> [!TIP]
> The `RowSpacing` and `ColumnSpacing` properties can be set to negative values to make cell contents overlap.

The equivalent C# code is:

```csharp
public class GridSpacingPage : ContentPage
{
    public GridSpacingPage()
    {
        Grid grid = new Grid
        {
            RowSpacing = 6,
            ColumnSpacing = 6,
            ...
        };
        ...

        Content = grid;
    }
}
```

## Alignment

Child views in a <xref:Microsoft.Maui.Controls.Grid> can be positioned within their cells by the `HorizontalOptions` and `VerticalOptions` properties. These properties can be set to the following fields from the `LayoutOptions` struct:

- `Start`
- `Center`
- `End`
- `Fill`

The following XAML creates a <xref:Microsoft.Maui.Controls.Grid> with nine equal-size cells, and places a <xref:Microsoft.Maui.Controls.Label> in each cell with a different alignment:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="GridDemos.Views.XAML.GridAlignmentPage"
             Title="Grid alignment demo">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition />
            <RowDefinition />
            <RowDefinition />
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition />
            <ColumnDefinition />
            <ColumnDefinition />
        </Grid.ColumnDefinitions>

        <BoxView Color="AliceBlue" />
        <Label Text="Upper left"
               HorizontalOptions="Start"
               VerticalOptions="Start" />
        <BoxView Grid.Column="1"
                 Color="LightSkyBlue" />
        <Label Grid.Column="1"
               Text="Upper center"
               HorizontalOptions="Center"
               VerticalOptions="Start"/>
        <BoxView Grid.Column="2"
                 Color="CadetBlue" />
        <Label Grid.Column="2"
               Text="Upper right"
               HorizontalOptions="End"
               VerticalOptions="Start" />
        <BoxView Grid.Row="1"
                 Color="CornflowerBlue" />
        <Label Grid.Row="1"
               Text="Center left"
               HorizontalOptions="Start"
               VerticalOptions="Center" />
        <BoxView Grid.Row="1"
                 Grid.Column="1"
                 Color="DodgerBlue" />
        <Label Grid.Row="1"
               Grid.Column="1"
               Text="Center center"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <BoxView Grid.Row="1"
                 Grid.Column="2"
                 Color="DarkSlateBlue" />
        <Label Grid.Row="1"
               Grid.Column="2"
               Text="Center right"
               HorizontalOptions="End"
               VerticalOptions="Center" />
        <BoxView Grid.Row="2"
                 Color="SteelBlue" />
        <Label Grid.Row="2"
               Text="Lower left"
               HorizontalOptions="Start"
               VerticalOptions="End" />
        <BoxView Grid.Row="2"
                 Grid.Column="1"
                 Color="LightBlue" />
        <Label Grid.Row="2"
               Grid.Column="1"
               Text="Lower center"
               HorizontalOptions="Center"
               VerticalOptions="End" />
        <BoxView Grid.Row="2"
                 Grid.Column="2"
                 Color="BlueViolet" />
        <Label Grid.Row="2"
               Grid.Column="2"
               Text="Lower right"
               HorizontalOptions="End"
               VerticalOptions="End" />
    </Grid>
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.Label> objects in each row are all identically aligned vertically, but use different horizontal alignments. Alternatively, this can be thought of as the <xref:Microsoft.Maui.Controls.Label> objects in each column being identically aligned horizontally, but using different vertical alignments:

:::image type="content" source="media/grid/alignment.png" alt-text="Cell alignment in a .NET MAUI Grid.":::

The equivalent C# code is:

```csharp
public class GridAlignmentPage : ContentPage
{
    public GridAlignmentPage()
    {
        Grid grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition()
            },
            ColumnDefinitions =
            {
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition()
            }
        };

        // Row 0
        grid.Add(new BoxView
        {
            Color = Colors.AliceBlue
        });
        grid.Add(new Label
        {
            Text = "Upper left",
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start
        });

        grid.Add(new BoxView
        {
            Color = Colors.LightSkyBlue
        }, 1, 0);
        grid.Add(new Label
        {
            Text = "Upper center",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start
        }, 1, 0);

        grid.Add(new BoxView
        {
            Color = Colors.CadetBlue
        }, 2, 0);
        grid.Add(new Label
        {
            Text = "Upper right",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start
        }, 2, 0);

        // Row 1
        grid.Add(new BoxView
        {
            Color = Colors.CornflowerBlue
        }, 0, 1);
        grid.Add(new Label
        {
            Text = "Center left",
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center
        }, 0, 1);

        grid.Add(new BoxView
        {
            Color = Colors.DodgerBlue
        }, 1, 1);
        grid.Add(new Label
        {
            Text = "Center center",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 1, 1);

        grid.Add(new BoxView
        {
            Color = Colors.DarkSlateBlue
        }, 2, 1);
        grid.Add(new Label
        {
            Text = "Center right",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center
        }, 2, 1);

        // Row 2
        grid.Add(new BoxView
        {
            Color = Colors.SteelBlue
        }, 0, 2);
        grid.Add(new Label
        {
            Text = "Lower left",
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.End
        }, 0, 2);

        grid.Add(new BoxView
        {
            Color = Colors.LightBlue
        }, 1, 2);
        grid.Add(new Label
        {
            Text = "Lower center",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End
        }, 1, 2);

        grid.Add(new BoxView
        {
            Color = Colors.BlueViolet
        }, 2, 2);
        grid.Add(new Label
        {
            Text = "Lower right",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End
        }, 2, 2);

        Title = "Grid alignment demo";
        Content = grid;
    }
}
```

## Nested Grid objects

A <xref:Microsoft.Maui.Controls.Grid> can be used as a parent layout that contains nested child <xref:Microsoft.Maui.Controls.Grid> objects, or other child layouts. When nesting <xref:Microsoft.Maui.Controls.Grid> objects, the `Grid.Row`, `Grid.Column`, `Grid.RowSpan`, and `Grid.ColumnSpan` attached properties always refer to the position of views within their parent <xref:Microsoft.Maui.Controls.Grid>.

The following XAML shows an example of nesting <xref:Microsoft.Maui.Controls.Grid> objects:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:converters="clr-namespace:GridDemos.Converters"
             x:Class="GridDemos.Views.XAML.ColorSlidersGridPage"
             Title="Nested Grids demo">

    <ContentPage.Resources>
        <converters:DoubleToIntConverter x:Key="doubleToInt" />

        <Style TargetType="Label">
            <Setter Property="HorizontalTextAlignment"
                    Value="Center" />
        </Style>
    </ContentPage.Resources>

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="500" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <BoxView x:Name="boxView"
                 Color="Black" />
        <Grid Grid.Row="1"
              Margin="20">
            <Grid.RowDefinitions>
                <RowDefinition />
                <RowDefinition />
                <RowDefinition />
                <RowDefinition />
                <RowDefinition />
                <RowDefinition />
            </Grid.RowDefinitions>
            <Slider x:Name="redSlider"
                    ValueChanged="OnSliderValueChanged" />
            <Label Grid.Row="1"
                   Text="{Binding Source={x:Reference redSlider},
                                  Path=Value,
                                  Converter={StaticResource doubleToInt},
                                  ConverterParameter=255,
                                  StringFormat='Red = {0}'}" />
            <Slider x:Name="greenSlider"
                    Grid.Row="2"
                    ValueChanged="OnSliderValueChanged" />
            <Label Grid.Row="3"
                   Text="{Binding Source={x:Reference greenSlider},
                                  Path=Value,
                                  Converter={StaticResource doubleToInt},
                                  ConverterParameter=255,
                                  StringFormat='Green = {0}'}" />
            <Slider x:Name="blueSlider"
                    Grid.Row="4"
                    ValueChanged="OnSliderValueChanged" />
            <Label Grid.Row="5"
                   Text="{Binding Source={x:Reference blueSlider},
                                  Path=Value,
                                  Converter={StaticResource doubleToInt},
                                  ConverterParameter=255,
                                  StringFormat='Blue = {0}'}" />
        </Grid>
    </Grid>
</ContentPage>
```

In this example, the root <xref:Microsoft.Maui.Controls.Grid> contains a <xref:Microsoft.Maui.Controls.BoxView> in its first row, and a child <xref:Microsoft.Maui.Controls.Grid> in its second row. The child <xref:Microsoft.Maui.Controls.Grid> contains <xref:Microsoft.Maui.Controls.Slider> objects that manipulate the color displayed by the <xref:Microsoft.Maui.Controls.BoxView>, and <xref:Microsoft.Maui.Controls.Label> objects that display the value of each <xref:Microsoft.Maui.Controls.Slider>:

:::image type="content" source="media/grid/nested.png" alt-text="Nested .NET MAUI Grid objects.":::

> [!IMPORTANT]
> The deeper you nest <xref:Microsoft.Maui.Controls.Grid> objects and other layouts, the more layout calculations will be performed which may impact performance. For more information, see [Choose the correct layout](~/deployment/performance.md#choose-the-correct-layout).

The equivalent C# code is:

```csharp
public class ColorSlidersGridPage : ContentPage
{
    BoxView boxView;
    Slider redSlider;
    Slider greenSlider;
    Slider blueSlider;

    public ColorSlidersGridPage()
    {
        // Create an implicit style for the Labels
        Style labelStyle = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter { Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center }
            }
        };
        Resources.Add(labelStyle);

        // Root page layout
        Grid rootGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { HeightRequest = 500 },
                new RowDefinition()
            }
        };

        boxView = new BoxView { Color = Colors.Black };
        rootGrid.Add(boxView);

        // Child page layout
        Grid childGrid = new Grid
        {
            Margin = new Thickness(20),
            RowDefinitions =
            {
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition()
            }
        };

        DoubleToIntConverter doubleToInt = new DoubleToIntConverter();

        redSlider = new Slider();
        redSlider.ValueChanged += OnSliderValueChanged;
        childGrid.Add(redSlider);

        Label redLabel = new Label();
        redLabel.SetBinding(Label.TextProperty, new Binding("Value", converter: doubleToInt, converterParameter: "255", stringFormat: "Red = {0}", source: redSlider));
        Grid.SetRow(redLabel, 1);
        childGrid.Add(redLabel);

        greenSlider = new Slider();
        greenSlider.ValueChanged += OnSliderValueChanged;
        Grid.SetRow(greenSlider, 2);
        childGrid.Add(greenSlider);

        Label greenLabel = new Label();
        greenLabel.SetBinding(Label.TextProperty, new Binding("Value", converter: doubleToInt, converterParameter: "255", stringFormat: "Green = {0}", source: greenSlider));
        Grid.SetRow(greenLabel, 3);
        childGrid.Add(greenLabel);

        blueSlider = new Slider();
        blueSlider.ValueChanged += OnSliderValueChanged;
        Grid.SetRow(blueSlider, 4);
        childGrid.Add(blueSlider);

        Label blueLabel = new Label();
        blueLabel.SetBinding(Label.TextProperty, new Binding("Value", converter: doubleToInt, converterParameter: "255", stringFormat: "Blue = {0}", source: blueSlider));
        Grid.SetRow(blueLabel, 5);
        childGrid.Add(blueLabel);

        // Place the child Grid in the root Grid
        rootGrid.Add(childGrid, 0, 1);

        Title = "Nested Grids demo";
        Content = rootGrid;
    }

    void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        boxView.Color = new Color(redSlider.Value, greenSlider.Value, blueSlider.Value);
    }
}
```
