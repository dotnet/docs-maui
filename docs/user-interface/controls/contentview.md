---
title: "ContentView"
description: "The .NET MAUI ContentView class can be used to create a custom control."
ms.date: 08/30/2024
---

# ContentView

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.ContentView> is a control that enables the creation of custom, reusable controls.

The <xref:Microsoft.Maui.Controls.ContentView> class defines a `Content` property, of type <xref:Microsoft.Maui.Controls.View>, which represents the content of the <xref:Microsoft.Maui.Controls.ContentView>. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.ContentView> class derives from the `TemplatedView` class, which defines the <xref:Microsoft.Maui.Controls.ControlTemplate> bindable property, of type <xref:Microsoft.Maui.Controls.ControlTemplate>, which defines the appearance of the control. For more information about the <xref:Microsoft.Maui.Controls.ControlTemplate> property, see [Customize appearance with a ControlTemplate](#customize-appearance-with-a-controltemplate).

> [!NOTE]
> A <xref:Microsoft.Maui.Controls.ContentView> can only contain a single child.

## Create a custom control

The <xref:Microsoft.Maui.Controls.ContentView> class offers little functionality by itself but can be used to create a custom control. The process for creating a custom control is to:

1. Create a class that derives from the <xref:Microsoft.Maui.Controls.ContentView> class.
1. Define any control properties or events in the code-behind file for the custom control.
1. Define the UI for the custom control.

This article demonstrates how to create a `CardView` control, which is a UI element that displays an image, title, and description in a card-like layout.

### Create a ContentView-derived class

A <xref:Microsoft.Maui.Controls.ContentView>-derived class can be created using the ContentView item template in Visual Studio. This template creates a XAML file in which the UI for the custom control can be defined, and a code-behind file in which any control properties, events, and other logic can be defined.

### Define control properties

Any control properties, events, and other logic should be defined in the code-behind file for the <xref:Microsoft.Maui.Controls.ContentView>-derived class.

The `CardView` custom control defines the following properties:

- `CardTitle`, of type `string`, which represents the title shown on the card.
- `CardDescription`, of type `string`, which represents the description shown on the card.
- `IconImageSource`, of type <xref:Microsoft.Maui.Controls.ImageSource>, which represents the image shown on the card.
- `IconBackgroundColor`, of type <xref:Microsoft.Maui.Graphics.Color>, which represents the background color for the image shown on the card.
- `BorderColor`, of type <xref:Microsoft.Maui.Graphics.Color>, which represents the color of the card border, image border, and divider line.
- `CardColor`, of type <xref:Microsoft.Maui.Graphics.Color>, which represents the background color of the card.

Each property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> instance.

The following example shows the `CardTitle` bindable property in the code-behind file for the `CardView` class:

```csharp
public partial class CardView : ContentView
{
    public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(CardView), string.Empty);

    public string CardTitle
    {
        get => (string)GetValue(CardView.CardTitleProperty);
        set => SetValue(CardView.CardTitleProperty, value);
    }
    // ...

    public CardView()
    {
        InitializeComponent();
    }
}
```

For more information about <xref:Microsoft.Maui.Controls.BindableProperty> objects, see [Bindable properties](~/fundamentals/bindable-properties.md).

### Define the UI

The custom control UI can be defined in the XAML file for the <xref:Microsoft.Maui.Controls.ContentView>-derived class, which uses a <xref:Microsoft.Maui.Controls.ContentView> as the root element of the control:

```XAML
<ContentView ...
             x:Name="this"
             x:Class="CardViewDemo.Controls.CardView">
    <Border BindingContext="{x:Reference this}"
            BackgroundColor="{Binding CardColor}"
            Stroke="{Binding BorderColor}"
            ...>
        <Grid>
            ...
            <Border Stroke="{Binding BorderColor, FallbackValue='Black'}"
                    BackgroundColor="{Binding IconBackgroundColor, FallbackValue='Grey'}"
                   ...>
                <Image Source="{Binding IconImageSource}"
                       .. />
            </Border>
            <Label Text="{Binding CardTitle, FallbackValue='Card Title'}"
                   ... />
            <BoxView BackgroundColor="{Binding BorderColor, FallbackValue='Black'}"
                     ... />
            <Label Text="{Binding CardDescription, FallbackValue='Card description text.'}"
                   ... />
        </Grid>
    </Border>
</ContentView>
```

The <xref:Microsoft.Maui.Controls.ContentView> element sets the `x:Name` property to `this`, which can be used to access the object bound to the `CardView` instance. Elements in the layout set bindings on their properties to values defined on the bound object. For more information about data binding, see [Data binding](~/fundamentals/data-binding/index.md).

> [!NOTE]
> The `FallbackValue` property in the `Binding` expression provides a default value in case the binding is `null`.

## Instantiate a custom control

A reference to the custom control namespace must be added to the page that instantiates the custom control. Once the reference has been added the `CardView` can be instantiated, and its properties defined:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:CardViewDemo.Controls"
             x:Class="CardViewDemo.CardViewXamlPage">
   <ScrollView>
       <StackLayout>
           <controls:CardView BorderColor="DarkGray"
                              CardTitle="Slavko Vlasic"
                              CardDescription="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla elit dolor, convallis non interdum."
                              IconBackgroundColor="SlateGray"
                              IconImageSource="user.png" />
            <!-- More CardView objects -->
       </StackLayout>
   </ScrollView>
</ContentPage>                   
```

The following screenshot shows multiple `CardView` objects:

:::image type="content" source="media/contentview/cardview-list.png" alt-text="Screenshot of CardView objects.":::

## Customize appearance with a ControlTemplate

A custom control that derives from the <xref:Microsoft.Maui.Controls.ContentView> class can define its UI using XAML or code, or may not define its UI at all. A <xref:Microsoft.Maui.Controls.ControlTemplate> can be used to override the control's appearance, regardless of how that appearance is defined.

For example, a `CardView` layout might occupy too much space for some use cases. A <xref:Microsoft.Maui.Controls.ControlTemplate> can be used to override the `CardView` layout to provide a more compact view, suitable for a condensed list:

```xaml
<ContentPage.Resources>
    <ResourceDictionary>
        <ControlTemplate x:Key="CardViewCompressed">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="100" />
                </Grid.RowDefinitions>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="100" />
                    <ColumnDefinition Width="100*" />
                </Grid.ColumnDefinitions>
                <Image Source="{TemplateBinding IconImageSource}"
                       BackgroundColor="{TemplateBinding IconBackgroundColor}"
                       WidthRequest="100"
                       HeightRequest="100"
                       Aspect="AspectFill"
                       HorizontalOptions="Center"
                       VerticalOptions="Center" />
                <StackLayout Grid.Column="1">
                    <Label Text="{TemplateBinding CardTitle}"
                           FontAttributes="Bold" />
                    <Label Text="{TemplateBinding CardDescription}" />
                </StackLayout>
            </Grid>
        </ControlTemplate>
    </ResourceDictionary>
</ContentPage.Resources>
```

Data binding in a <xref:Microsoft.Maui.Controls.ControlTemplate> uses the `TemplateBinding` markup extension to specify bindings. The <xref:Microsoft.Maui.Controls.ControlTemplate> property can then be set to the defined <xref:Microsoft.Maui.Controls.ControlTemplate> object, by using its `x:Key` value. The following example shows the <xref:Microsoft.Maui.Controls.ControlTemplate> property set on a `CardView` instance:

```xaml
<controls:CardView ControlTemplate="{StaticResource CardViewCompressed}" />
```

The following screenshot shows a standard `CardView` instance, and multiple `CardView` instances whose control templates have been overridden:

:::image type="content" source="media/contentview/cardview-controltemplates.png" alt-text="Screenshot of CardView overridden with a ControlTemplate.":::

For more information about control templates, see [Control templates](~/fundamentals/controltemplate.md).
