---
title: "Control templates"
description: ".NET MAUI control templates define the visual structure of ContentView derived custom controls, and ContentPage derived pages."
ms.date: 02/18/2022
---

# Control templates

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-controltemplates)

.NET Multi-platform App UI (.NET MAUI) control templates enable you to define the visual structure of <xref:Microsoft.Maui.Controls.ContentView> derived custom controls, and <xref:Microsoft.Maui.Controls.ContentPage> derived pages. Control templates separate the user interface (UI) for a custom control, or page, from the logic that implements the control or page. Additional content can also be inserted into the templated custom control, or templated page, at a pre-defined location.

For example, a control template can be created that redefines the UI provided by a custom control. The control template can then be consumed by the required custom control instance. Alternatively, a control template can be created that defines any common UI that will be used by multiple pages in an app. The control template can then be consumed by multiple pages, with each page still displaying its unique content.

## Create a ControlTemplate

The following example shows the code for a `CardView` custom control:

```csharp
public class CardView : ContentView
{
    public static readonly BindableProperty CardTitleProperty =
        BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(CardView), string.Empty);
    public static readonly BindableProperty CardDescriptionProperty =
        BindableProperty.Create(nameof(CardDescription), typeof(string), typeof(CardView), string.Empty);

    public string CardTitle
    {
        get => (string)GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    public string CardDescription
    {
        get => (string)GetValue(CardDescriptionProperty);
        set => SetValue(CardDescriptionProperty, value);
    }
    ...
}
```

The `CardView` class, which derives from the <xref:Microsoft.Maui.Controls.ContentView> class, represents a custom control that displays data in a card-like layout. The class contains properties, which are backed by bindable properties, for the data it displays. However, the `CardView` class does not define any UI. Instead, the UI will be defined with a control template. For more information about creating <xref:Microsoft.Maui.Controls.ContentView> derived custom controls, see [ContentView](~/user-interface/controls/contentview.md).

A control template is created with the <xref:Microsoft.Maui.Controls.ControlTemplate> type. When you create a <xref:Microsoft.Maui.Controls.ControlTemplate>, you combine <xref:Microsoft.Maui.Controls.View> objects to build the UI for a custom control, or page. A <xref:Microsoft.Maui.Controls.ControlTemplate> must have only one <xref:Microsoft.Maui.Controls.View> as its root element. However, the root element usually contains other <xref:Microsoft.Maui.Controls.View> objects. The combination of objects makes up the control's visual structure.

While a <xref:Microsoft.Maui.Controls.ControlTemplate> can be defined inline, the typical approach to declaring a <xref:Microsoft.Maui.Controls.ControlTemplate> is as a resource in a resource dictionary. Because control templates are resources, they obey the same scoping rules that apply to all resources. For example, if you declare a control template in your app-level resource dictionary, the template can be used anywhere in your app. If you define the template in a page, only that page can use the control template. For more information about resources, see [Resource dictionaries](~/fundamentals/resource-dictionaries.md).

The following XAML example shows a <xref:Microsoft.Maui.Controls.ControlTemplate> for `CardView` objects:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             ...>
    <ContentPage.Resources>
      <ControlTemplate x:Key="CardViewControlTemplate">
          <Frame BindingContext="{Binding Source={RelativeSource TemplatedParent}}"
                 BackgroundColor="{Binding CardColor}"
                 BorderColor="{Binding BorderColor}"
                 ...>
              <!-- UI objects that define the CardView visual structure -->
          </Frame>
      </ControlTemplate>
    </ContentPage.Resources>
    ...
</ContentPage>
```

When a <xref:Microsoft.Maui.Controls.ControlTemplate> is declared as a resource, it must have a key specified with the `x:Key` attribute so that it can be identified in the resource dictionary. In this example, the root element of the `CardViewControlTemplate` is a <xref:Microsoft.Maui.Controls.Frame> object. The <xref:Microsoft.Maui.Controls.Frame> object uses the [`RelativeSource`](xref:Microsoft.Maui.Controls.Xaml.RelativeSourceExtension) markup extension to set its <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> to the runtime object instance to which the template will be applied, which is known as the *templated parent*. The <xref:Microsoft.Maui.Controls.Frame> object uses a combination of controls to define the visual structure of a `CardView` object. The binding expressions of these objects resolve against `CardView` properties, due to inheriting the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> from the root <xref:Microsoft.Maui.Controls.Frame> element. For more information about the [`RelativeSource`](xref:Microsoft.Maui.Controls.Xaml.RelativeSourceExtension) markup extension, see [Relative bindings](~/fundamentals/data-binding/relative-bindings.md).

## Consume a ControlTemplate

A <xref:Microsoft.Maui.Controls.ControlTemplate> can be applied to a <xref:Microsoft.Maui.Controls.ContentView> derived custom control by setting its <xref:Microsoft.Maui.Controls.ControlTemplate> property to the control template object. Similarly, a <xref:Microsoft.Maui.Controls.ControlTemplate> can be applied to a <xref:Microsoft.Maui.Controls.ContentPage> derived page by setting its <xref:Microsoft.Maui.Controls.ControlTemplate> property to the control template object. At runtime, when a <xref:Microsoft.Maui.Controls.ControlTemplate> is applied, all of the controls that are defined in the <xref:Microsoft.Maui.Controls.ControlTemplate> are added to the visual tree of the templated custom control, or templated page.

The following example shows the `CardViewControlTemplate` being assigned to the <xref:Microsoft.Maui.Controls.ControlTemplate> property of two `CardView` objects:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:ControlTemplateDemos.Controls"
             ...>
    <StackLayout Margin="30">
        <controls:CardView BorderColor="DarkGray"
                           CardTitle="John Doe"
                           CardDescription="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla elit dolor, convallis non interdum."
                           IconBackgroundColor="SlateGray"
                           IconImageSource="user.png"
                           ControlTemplate="{StaticResource CardViewControlTemplate}" />
        <controls:CardView BorderColor="DarkGray"
                           CardTitle="Jane Doe"
                           CardDescription="Phasellus eu convallis mi. In tempus augue eu dignissim fermentum. Morbi ut lacus vitae eros lacinia."
                           IconBackgroundColor="SlateGray"
                           IconImageSource="user.png"
                           ControlTemplate="{StaticResource CardViewControlTemplate}" />
    </StackLayout>
</ContentPage>
```

In this example, the controls in the `CardViewControlTemplate` become part of the visual tree for each `CardView` object. Because the root <xref:Microsoft.Maui.Controls.Frame> object for the control template sets its <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> to the templated parent, the <xref:Microsoft.Maui.Controls.Frame> and its children resolve their binding expressions against the properties of each `CardView` object.

The following screenshot shows the `CardViewControlTemplate` applied to the the `CardView` objects:

:::image type="content" source="media/controltemplate/relativesource-controltemplate.png" alt-text="Screenshot of two templated CardView objects.":::

> [!IMPORTANT]
> The point in time that a <xref:Microsoft.Maui.Controls.ControlTemplate> is applied to a control instance can be detected by overriding the <xref:Microsoft.Maui.Controls.TemplatedView.OnApplyTemplate%2A> method in the templated custom control, or templated page. For more information, see [Get a named element from a template](#get-a-named-element-from-a-template).

## Pass parameters with TemplateBinding

The [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup extension binds a property of an element that is in a <xref:Microsoft.Maui.Controls.ControlTemplate> to a public property that is defined by the templated custom control or templated page. When you use a [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension), you enable properties on the control to act as parameters to the template. Therefore, when a property on a templated custom control or templated page is set, that value is passed onto the element that has the [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) on it.

> [!IMPORTANT]
> The [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup expression enables the [`RelativeSource`](xref:Microsoft.Maui.Controls.Xaml.RelativeSourceExtension) binding from the previous control template to be removed, and replaces the `Binding` expressions.

The [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup extension defines the following properties:

- <xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension.Path>, of type `string`, the path to the property.
- <xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension.Mode>, of type <xref:Microsoft.Maui.Controls.BindingMode>, the direction in which changes propagate between the *source* and *target*.
- <xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension.Converter>, of type <xref:Microsoft.Maui.Controls.IValueConverter>, the binding value converter.
- <xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension.ConverterParameter>, of type `object`, the parameter to the binding value converter.
- <xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension.StringFormat>, of type `string`, the string format for the binding.

The [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) for the [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup extension is <xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension.Path>. Therefore, the "Path=" part of the markup extension can be omitted if the path is the first item in the [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) expression. For more information about using these properties in a binding expression, see [Data binding](~/fundamentals/data-binding/index.md).

> [!WARNING]
> The [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup extension should only be used within a <xref:Microsoft.Maui.Controls.ControlTemplate>. However, attempting to use a [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) expression outside of a <xref:Microsoft.Maui.Controls.ControlTemplate> will not result in a build error or an exception being thrown.

The following XAML example shows a <xref:Microsoft.Maui.Controls.ControlTemplate> for `CardView` objects, that uses the [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup extension:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             ...>
    <ContentPage.Resources>
        <ControlTemplate x:Key="CardViewControlTemplate">
            <Frame BackgroundColor="{TemplateBinding CardColor}"
                   BorderColor="{TemplateBinding BorderColor}"
                   ...>
                <!-- UI objects that define the CardView visual structure -->                   
            </Frame>
        </ControlTemplate>
    </ContentPage.Resources>
    ...
</ContentPage>
```

In this example, the [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup extension resolves binding expressions against the properties of each `CardView` object. The following screenshot shows the `CardViewControlTemplate` applied to the `CardView` objects:

:::image type="content" source="media/controltemplate/templatebinding-controltemplate.png" alt-text="Screenshot of templated CardView objects.":::

> [!IMPORTANT]
> Using the [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup extension is equivalent to setting the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> of the root element in the template to its templated parent with the [`RelativeSource`](xref:Microsoft.Maui.Controls.Xaml.RelativeSourceExtension) markup extension, and then resolving bindings of child objects with the `Binding` markup extension. In fact, the [`TemplateBinding`](xref:Microsoft.Maui.Controls.Xaml.TemplateBindingExtension) markup extension creates a `Binding` whose `Source` is `RelativeBindingSource.TemplatedParent`.

## Apply a ControlTemplate with a style

Control templates can also be applied with styles. This is achieved by creating an *implicit* or *explicit* style that consumes the <xref:Microsoft.Maui.Controls.ControlTemplate>.

The following XAML example shows an *implicit* style that consumes the `CardViewControlTemplate`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:ControlTemplateDemos.Controls"
             ...>
    <ContentPage.Resources>
        <ControlTemplate x:Key="CardViewControlTemplate">
            ...
        </ControlTemplate>

        <Style TargetType="controls:CardView">
            <Setter Property="ControlTemplate"
                    Value="{StaticResource CardViewControlTemplate}" />
        </Style>
    </ContentPage.Resources>
    <StackLayout Margin="30">
        <controls:CardView BorderColor="DarkGray"
                           CardTitle="John Doe"
                           CardDescription="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla elit dolor, convallis non interdum."
                           IconBackgroundColor="SlateGray"
                           IconImageSource="user.png" />
        ...
    </StackLayout>
</ContentPage>
```

In this example, the *implicit* <xref:Microsoft.Maui.Controls.Style> is automatically applied to each `CardView` object, and sets the <xref:Microsoft.Maui.Controls.ControlTemplate> property of each `CardView` to `CardViewControlTemplate`.

For more information about styles, see [Styles](~/user-interface/styles/xaml.md).

## Redefine a control’s UI

When a <xref:Microsoft.Maui.Controls.ControlTemplate> is instantiated and assigned to the <xref:Microsoft.Maui.Controls.ControlTemplate> property of a <xref:Microsoft.Maui.Controls.ContentView> derived custom control, or a <xref:Microsoft.Maui.Controls.ContentPage> derived page, the visual structure defined for the custom control or page is replaced with the visual structure defined in the <xref:Microsoft.Maui.Controls.ControlTemplate>.

For example, the `CardViewUI` custom control defines its user interface using the following XAML:

```xaml
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="ControlTemplateDemos.Controls.CardViewUI"
             x:Name="this">
    <Frame BindingContext="{x:Reference this}"
           BackgroundColor="{Binding CardColor}"
           BorderColor="{Binding BorderColor}"
           ...>
        <!-- UI objects that define the CardView visual structure -->           
    </Frame>
</ContentView>
```

However, the controls that comprise this UI can be replaced by defining a new visual structure in a <xref:Microsoft.Maui.Controls.ControlTemplate>, and assigning it to the <xref:Microsoft.Maui.Controls.ControlTemplate> property of a `CardViewUI` object:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             ...>
    <ContentPage.Resources>
        <ControlTemplate x:Key="CardViewCompressed">
            <Grid RowDefinitions="100"
                  ColumnDefinitions="100, *">
                <Image Source="{TemplateBinding IconImageSource}"
                       BackgroundColor="{TemplateBinding IconBackgroundColor}"
                       ...>
                <!-- Other UI objects that define the CardView visual structure -->
            </Grid>
        </ControlTemplate>
    </ContentPage.Resources>
    <StackLayout Margin="30">
        <controls:CardViewUI BorderColor="DarkGray"
                             CardTitle="John Doe"
                             CardDescription="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla elit dolor, convallis non interdum."
                             IconBackgroundColor="SlateGray"
                             IconImageSource="user.png"
                             ControlTemplate="{StaticResource CardViewCompressed}" />
        ...
    </StackLayout>
</ContentPage>
```

In this example, the visual structure of the `CardViewUI` object is redefined in a <xref:Microsoft.Maui.Controls.ControlTemplate> that provides a more compact visual structure that's suitable for a condensed list:

:::image type="content" source="media/controltemplate/redefine-controltemplate.png" alt-text="Screenshot of templated CardViewUI objects.":::

## Substitute content into a ContentPresenter

A <xref:Microsoft.Maui.Controls.ContentPresenter> can be placed in a control template to mark where content to be displayed by the templated custom control or templated page will appear. The custom control or page that consumes the control template will then define content to be displayed by the <xref:Microsoft.Maui.Controls.ContentPresenter>. The following diagram illustrates a <xref:Microsoft.Maui.Controls.ControlTemplate> for a page that contains a number of controls, including a <xref:Microsoft.Maui.Controls.ContentPresenter> marked by a blue rectangle:

:::image type="content" source="media/controltemplate/controltemplate.png" alt-text="Control template for a ContentPage." border="false":::

The following XAML shows a control template named `TealTemplate` that contains a <xref:Microsoft.Maui.Controls.ContentPresenter> in its visual structure:

```xaml
<ControlTemplate x:Key="TealTemplate">
    <Grid RowDefinitions="0.1*, 0.8*, 0.1*">
        <BoxView Color="Teal" />
        <Label Margin="20,0,0,0"
               Text="{TemplateBinding HeaderText}"
               ... />
        <ContentPresenter Grid.Row="1" />
        <BoxView Grid.Row="2"
                 Color="Teal" />
        <Label x:Name="changeThemeLabel"
               Grid.Row="2"
               Margin="20,0,0,0"
               Text="Change Theme"
               ...>
            <Label.GestureRecognizers>
                <TapGestureRecognizer Tapped="OnChangeThemeLabelTapped" />
            </Label.GestureRecognizers>
        </Label>
        <controls:HyperlinkLabel Grid.Row="2"
                                 Margin="0,0,20,0"
                                 Text="Help"
                                 Url="https://learn.microsoft.com/dotnet/maui/"
                                 ... />
    </Grid>
</ControlTemplate>
```

The following example shows `TealTemplate` assigned to the <xref:Microsoft.Maui.Controls.ControlTemplate> property of a <xref:Microsoft.Maui.Controls.ContentPage> derived page:

```xaml
<controls:HeaderFooterPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                           xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                           xmlns:controls="clr-namespace:ControlTemplateDemos.Controls"                           
                           ControlTemplate="{StaticResource TealTemplate}"
                           HeaderText="MyApp"
                           ...>
    <StackLayout Margin="10">
        <Entry Placeholder="Enter username" />
        <Entry Placeholder="Enter password"
               IsPassword="True" />
        <Button Text="Login" />
    </StackLayout>
</controls:HeaderFooterPage>
```

At runtime, when `TealTemplate` is applied to the page, the page content is substituted into the <xref:Microsoft.Maui.Controls.ContentPresenter> defined in the control template:

:::image type="content" source="media/controltemplate/tealtemplate-contentpage.png" alt-text="Screenshot of templated page object.":::

## Get a named element from a template

Named elements within a control template can be retrieved from the templated custom control or templated page. This can be achieved with the <xref:Microsoft.Maui.Controls.TemplatedView.GetTemplateChild%2A> method, which returns the named element in the instantiated <xref:Microsoft.Maui.Controls.ControlTemplate> visual tree, if found. Otherwise, it returns `null`.

After a control template has been instantiated, the template's <xref:Microsoft.Maui.Controls.TemplatedView.OnApplyTemplate%2A> method is called. The <xref:Microsoft.Maui.Controls.TemplatedView.GetTemplateChild%2A> method should therefore be called from a <xref:Microsoft.Maui.Controls.TemplatedView.OnApplyTemplate%2A> override in the templated control or templated page.

> [!IMPORTANT]
> The <xref:Microsoft.Maui.Controls.TemplatedView.GetTemplateChild%2A> method should only be called after the <xref:Microsoft.Maui.Controls.TemplatedView.OnApplyTemplate%2A> method has been called.

The following XAML shows a control template named `TealTemplate` that can be applied to <xref:Microsoft.Maui.Controls.ContentPage> derived pages:

```xaml
<ControlTemplate x:Key="TealTemplate">
    <Grid>
        ...
        <Label x:Name="changeThemeLabel"
               Text="Change Theme"
               ...>
            <Label.GestureRecognizers>
                <TapGestureRecognizer Tapped="OnChangeThemeLabelTapped" />
            </Label.GestureRecognizers>
        </Label>
        ...
    </Grid>
</ControlTemplate>
```

In this example, the <xref:Microsoft.Maui.Controls.Label> element is named, and can be retrieved in the code for the templated page. This is achieved by calling the <xref:Microsoft.Maui.Controls.TemplatedView.GetTemplateChild%2A> method from the <xref:Microsoft.Maui.Controls.TemplatedView.OnApplyTemplate%2A> override for the templated page:

```csharp
public partial class AccessTemplateElementPage : HeaderFooterPage
{
    Label themeLabel;

    public AccessTemplateElementPage()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        themeLabel = (Label)GetTemplateChild("changeThemeLabel");
        themeLabel.Text = OriginalTemplate ? "Aqua Theme" : "Teal Theme";
    }
}
```

In this example, the <xref:Microsoft.Maui.Controls.Label> object named `changeThemeLabel` is retrieved once the <xref:Microsoft.Maui.Controls.ControlTemplate> has been instantiated. `changeThemeLabel` can then be accessed and manipulated by the `AccessTemplateElementPage` class. The following screenshot shows that the text displayed by the <xref:Microsoft.Maui.Controls.Label> has been changed:

:::image type="content" source="media/controltemplate/get-named-element.png" alt-text="Screenshot of templated page object that's changed.":::

## Bind to a viewmodel

A <xref:Microsoft.Maui.Controls.ControlTemplate> can data bind to a viewmodel, even when the <xref:Microsoft.Maui.Controls.ControlTemplate> binds to the templated parent (the runtime object instance to which the template is applied).

The following XAML example shows a page that consumes a viewmodel named `PeopleViewModel`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:ControlTemplateDemos"
             xmlns:controls="clr-namespace:ControlTemplateDemos.Controls"
             ...>
    <ContentPage.BindingContext>
        <local:PeopleViewModel />
    </ContentPage.BindingContext>

    <ContentPage.Resources>
        <DataTemplate x:Key="PersonTemplate">
            <controls:CardView BorderColor="DarkGray"
                               CardTitle="{Binding Name}"
                               CardDescription="{Binding Description}"
                               ControlTemplate="{StaticResource CardViewControlTemplate}" />
        </DataTemplate>
    </ContentPage.Resources>

    <StackLayout Margin="10"
                 BindableLayout.ItemsSource="{Binding People}"
                 BindableLayout.ItemTemplate="{StaticResource PersonTemplate}" />
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> of the page is set to a `PeopleViewModel` instance. This viewmodel exposes a `People` collection and an <xref:System.Windows.Input.ICommand> named `DeletePersonCommand`. The <xref:Microsoft.Maui.Controls.StackLayout> on the page uses a bindable layout to data bind to the `People` collection, and the [`ItemTemplate`](xref:Microsoft.Maui.Controls.BindableLayout.ItemTemplateProperty) of the bindable layout is set to the `PersonTemplate` resource. This <xref:Microsoft.Maui.Controls.DataTemplate> specifies that each item in the `People` collection will be displayed using a `CardView` object. The visual structure of the `CardView` object is defined using a <xref:Microsoft.Maui.Controls.ControlTemplate> named `CardViewControlTemplate`:

```xaml
<ControlTemplate x:Key="CardViewControlTemplate">
    <Frame BindingContext="{Binding Source={RelativeSource TemplatedParent}}"
           BackgroundColor="{Binding CardColor}"
           BorderColor="{Binding BorderColor}"
           ...>
        <!-- UI objects that define the CardView visual structure -->           
    </Frame>
</ControlTemplate>
```

In this example, the root element of the <xref:Microsoft.Maui.Controls.ControlTemplate> is a <xref:Microsoft.Maui.Controls.Frame> object. The <xref:Microsoft.Maui.Controls.Frame> object uses the [`RelativeSource`](xref:Microsoft.Maui.Controls.Xaml.RelativeSourceExtension) markup extension to set its <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> to the templated parent. The binding expressions of the <xref:Microsoft.Maui.Controls.Frame> object and its children resolve against `CardView` properties, due to inheriting the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> from the root <xref:Microsoft.Maui.Controls.Frame> element. The following screenshot shows the page displaying the `People` collection:

:::image type="content" source="media/controltemplate/viewmodel-controltemplate.png" alt-text="Screenshot of three templated CardView objects that bind to a viewmodel.":::

While the objects in the <xref:Microsoft.Maui.Controls.ControlTemplate> bind to properties on its templated parent, the <xref:Microsoft.Maui.Controls.Button> within the control template binds to both its templated parent, and to the `DeletePersonCommand` in the viewmodel. This is because the `Button.Command` property redefines its binding source to be the binding context of the ancestor whose binding context type is `PeopleViewModel`, which is the <xref:Microsoft.Maui.Controls.StackLayout>. The `Path` part of the binding expressions can then resolve the `DeletePersonCommand` property. However, the `Button.CommandParameter` property doesn't alter its binding source, instead inheriting it from its parent in the <xref:Microsoft.Maui.Controls.ControlTemplate>. Therefore, the `CommandParameter` property binds to the `CardTitle` property of the `CardView`.

The overall effect of the <xref:Microsoft.Maui.Controls.Button> bindings is that when the <xref:Microsoft.Maui.Controls.Button> is tapped, the `DeletePersonCommand` in the `PeopleViewModel` class is executed, with the value of the `CardName` property being passed to the `DeletePersonCommand`. This results in the specified `CardView` being removed from the bindable layout.

For more information about relative bindings, see [Relative bindings](~/fundamentals/data-binding/relative-bindings.md).
