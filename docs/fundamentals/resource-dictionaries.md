---
title: "Resource dictionaries"
description: "Learn how .NET MAUI XAML resources dictionaries enable resources to be shared and reused throughout an app."
ms.date: 04/21/2023
---

# Resource dictionaries

A .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.ResourceDictionary> is a repository for resources that are used by a .NET MAUI app. Typical resources that are stored in a <xref:Microsoft.Maui.Controls.ResourceDictionary> include styles, control templates, data templates, converters, and colors.

XAML resources that are stored in a <xref:Microsoft.Maui.Controls.ResourceDictionary> can be referenced and applied to elements by using the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) or [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension. In C#, resources can also be defined in a <xref:Microsoft.Maui.Controls.ResourceDictionary> and then referenced and applied to elements by using a string-based indexer.

> [!TIP]
> In Visual Studio, a XAML-based <xref:Microsoft.Maui.Controls.ResourceDictionary> file that's backed by a code-behind file can be added to your project by the **.NET MAUI ResourceDictionary (XAML)** item template.

## Create resources

Every <xref:Microsoft.Maui.Controls.VisualElement> derived object has a `Resources` property, which is a <xref:Microsoft.Maui.Controls.ResourceDictionary> that can contain resources. Similarly, an `Application` derived object has a `Resources` property, which is a <xref:Microsoft.Maui.Controls.ResourceDictionary> that can contain resources.

A .NET MAUI app can contain only a single class that derives from `Application`, but often makes use of many classes that derive from <xref:Microsoft.Maui.Controls.VisualElement>, including pages, layouts, and views. Any of these objects can have its `Resources` property set to a <xref:Microsoft.Maui.Controls.ResourceDictionary> containing resources. Choosing where to put a particular <xref:Microsoft.Maui.Controls.ResourceDictionary> impacts where the resources can be used:

- Resources in a <xref:Microsoft.Maui.Controls.ResourceDictionary> that is attached to a view, such as <xref:Microsoft.Maui.Controls.Button> or <xref:Microsoft.Maui.Controls.Label>, can only be applied to that particular object.
- Resources in a <xref:Microsoft.Maui.Controls.ResourceDictionary> attached to a layout, such as <xref:Microsoft.Maui.Controls.StackLayout> or <xref:Microsoft.Maui.Controls.Grid>, can be applied to the layout and all the children of that layout.
- Resources in a <xref:Microsoft.Maui.Controls.ResourceDictionary> defined at the page level can be applied to the page and to all its children.
- Resources in a <xref:Microsoft.Maui.Controls.ResourceDictionary> defined at the application level can be applied throughout the app.

With the exception of implicit styles, each resource in resource dictionary must have a unique string key that's defined with the `x:Key` attribute.

The following XAML shows resources defined in an application level <xref:Microsoft.Maui.Controls.ResourceDictionary> in the **App.xaml** file:

```xaml
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="ResourceDictionaryDemo.App">
    <Application.Resources>

        <Thickness x:Key="PageMargin">20</Thickness>

        <!-- Colors -->
        <Color x:Key="AppBackgroundColor">AliceBlue</Color>
        <Color x:Key="NavigationBarColor">#1976D2</Color>
        <Color x:Key="NavigationBarTextColor">White</Color>
        <Color x:Key="NormalTextColor">Black</Color>

        <!-- Images -->
        <x:String x:Key="BackgroundImage">background</x:String>
        <x:String x:Key="MenuIcon">menu.png</x:String>
        <x:String x:Key="SearchIcon">search.png</x:String>

        <!-- Implicit styles -->
        <Style TargetType="NavigationPage">
            <Setter Property="BarBackgroundColor"
                    Value="{StaticResource NavigationBarColor}" />
            <Setter Property="BarTextColor"
                    Value="{StaticResource NavigationBarTextColor}" />
        </Style>

        <Style TargetType="ContentPage"
               ApplyToDerivedTypes="True">
            <Setter Property="BackgroundColor"
                    Value="{StaticResource AppBackgroundColor}" />
        </Style>

    </Application.Resources>
</Application>
```

In this example, the resource dictionary defines a `Thickness` resource, multiple <xref:Microsoft.Maui.Graphics.Color> resources, and two implicit <xref:Microsoft.Maui.Controls.Style> resources.<!-- For more information about the `App` class, see [.NET MAUI App Class](~/fundamentals/application-class.md).-->

> [!IMPORTANT]
> Inserting resources directly between the `Resources` property-element tags automatically creates a <xref:Microsoft.Maui.Controls.ResourceDictionary> object. However, it's also valid to place all resources between optional <xref:Microsoft.Maui.Controls.ResourceDictionary> tags.

## Consume resources

Each resource has a key that is specified using the `x:Key` attribute, which becomes its dictionary key in the <xref:Microsoft.Maui.Controls.ResourceDictionary>. The key is used to reference a resource from the <xref:Microsoft.Maui.Controls.ResourceDictionary> with the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) or [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) XAML markup extension.

The [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension is similar to the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension in that both use a dictionary key to reference a value from a resource dictionary. However, while the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension performs a single dictionary lookup, the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension maintains a link to the dictionary key. Therefore, if the dictionary entry associated with the key is replaced, the change is applied to the visual element. This enables runtime resource changes to be made in an app. For more information about markup extensions, see [XAML markup extensions](~/xaml/markup-extensions/consume.md).

The following XAML example shows how to consume resources, and also define an additional resource in a <xref:Microsoft.Maui.Controls.StackLayout>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="ResourceDictionaryDemo.MainPage"
             Title="Main page">
    <StackLayout Margin="{StaticResource PageMargin}"
                 Spacing="6">
        <StackLayout.Resources>
            <!-- Implicit style -->
            <Style TargetType="Button">
                <Setter Property="FontSize" Value="14" />
                <Setter Property="BackgroundColor" Value="#1976D2" />
                <Setter Property="TextColor" Value="White" />
                <Setter Property="CornerRadius" Value="5" />
            </Style>
        </StackLayout.Resources>

        <Label Text="This app demonstrates consuming resources that have been defined in resource dictionaries." />
        <Button Text="Navigate"
                Clicked="OnNavigateButtonClicked" />
    </StackLayout>
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.ContentPage> object consumes the implicit style defined in the application level resource dictionary. The <xref:Microsoft.Maui.Controls.StackLayout> object consumes the `PageMargin` resource defined in the application level resource dictionary, while the <xref:Microsoft.Maui.Controls.Button> object consumes the implicit style defined in the <xref:Microsoft.Maui.Controls.StackLayout> resource dictionary. This results in the appearance shown in the following screenshot:

:::image type="content" source="media/resource-dictionaries/consuming.png" alt-text="Consuming resource dictionary resources.":::

> [!IMPORTANT]
> Resources that are specific to a single page shouldn't be included in an application level resource dictionary, as such resources will then be parsed at app startup instead of when required by a page. For more information, see [Reduce the application resource dictionary size](~/deployment/performance.md#reduce-the-application-resource-dictionary-size).

## Resource lookup behavior

The following lookup process occurs when a resource is referenced with the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) or [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension:

- The requested key is checked for in the resource dictionary, if it exists, for the element that sets the property. If the requested key is found, its value is returned and the lookup process terminates.
- If a match isn't found, the lookup process searches the visual tree upwards, checking the resource dictionary of each parent element. If the requested key is found, its value is returned and the lookup process terminates. Otherwise the process continues upwards until the root element is reached.
- If a match isn't found at the root element, the application level resource dictionary is examined.
- If a match still isn't found, a `XamlParseException` is thrown.

Therefore, when the XAML parser encounters a [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) or [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension, it searches for a matching key by traveling up through the visual tree, using the first match it finds. If this search ends at the page and the key still hasn't been found, the XAML parser searches the <xref:Microsoft.Maui.Controls.ResourceDictionary> attached to the `App` object. If the key still isn't found, an exception is thrown.

## Override resources

When resources share keys, resources defined lower in the visual tree will take precedence over those defined higher up. For example, setting an `AppBackgroundColor` resource to `AliceBlue` at the application level will be overridden by a page level `AppBackgroundColor` resource set to `Teal`. Similarly, a page level `AppBackgroundColor` resource will be overridden by a layout or view level `AppBackgroundColor` resource.

## Stand-alone resource dictionaries

A <xref:Microsoft.Maui.Controls.ResourceDictionary> can also be created as a stand-alone XAML file that isn't backed by a code-behind file. To create a stand-alone <xref:Microsoft.Maui.Controls.ResourceDictionary>, add a new <xref:Microsoft.Maui.Controls.ResourceDictionary> file to the project with the **.NET MAUI ResourceDictionary (XAML)** item template and delete its code-behind file. Then, in the XAML file remove the `x:Class` attribute from the <xref:Microsoft.Maui.Controls.ResourceDictionary> tag near the start of the file. In addition, add `<?xaml-comp compile="true" ?>` after the XML header to ensure that the XAML will be compiled.

> [!NOTE]
> A stand-alone <xref:Microsoft.Maui.Controls.ResourceDictionary> must have a build action of **MauiXaml**.

The following XAML example shows a stand-alone <xref:Microsoft.Maui.Controls.ResourceDictionary> named **MyResourceDictionary.xaml**:

```xaml
<?xml version="1.0" encoding="UTF-8" ?>
<?xaml-comp compile="true" ?>
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    <DataTemplate x:Key="PersonDataTemplate">
        <ViewCell>
            <Grid RowSpacing="6"
                  ColumnSpacing="6">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="0.5*" />
                    <ColumnDefinition Width="0.2*" />
                    <ColumnDefinition Width="0.3*" />
                </Grid.ColumnDefinitions>
                <Label Text="{Binding Name}"
                       TextColor="{StaticResource NormalTextColor}"
                       FontAttributes="Bold" />
                <Label Grid.Column="1"
                       Text="{Binding Age}"
                       TextColor="{StaticResource NormalTextColor}" />
                <Label Grid.Column="2"
                       Text="{Binding Location}"
                       TextColor="{StaticResource NormalTextColor}"
                       HorizontalTextAlignment="End" />
            </Grid>
        </ViewCell>
    </DataTemplate>
</ResourceDictionary>
```

In this example, the <xref:Microsoft.Maui.Controls.ResourceDictionary> contains a single resource, which is an object of type <xref:Microsoft.Maui.Controls.DataTemplate>. **MyResourceDictionary.xaml** can be consumed by merging it into another resource dictionary.

<!--
By default, the linker will remove stand-alone XAML files from release builds when the linker behavior is set to link all assemblies. To ensure that stand-alone XAML files remain in a release build:

1. Add a custom `Preserve` attribute to the assembly containing the stand-alone XAML files. For more information, see [Preserving code](~/ios/deploy-test/linker.md).
1. Set the `Preserve` attribute at the assembly level:

    ```csharp
    [assembly:Preserve(AllMembers = true)]
    ```

For more information about linking, see [Linking Xamarin.iOS apps](~/ios/deploy-test/linker.md) and [Linking on Android](~/android/deploy-test/linker.md). -->

## Merge resource dictionaries

Resource dictionaries can be combined by merging one or more <xref:Microsoft.Maui.Controls.ResourceDictionary> objects into another <xref:Microsoft.Maui.Controls.ResourceDictionary>.

### Merge local resource dictionaries

A local <xref:Microsoft.Maui.Controls.ResourceDictionary> file can be merged into another <xref:Microsoft.Maui.Controls.ResourceDictionary> by creating a <xref:Microsoft.Maui.Controls.ResourceDictionary> object whose `Source` property is set to the filename of the XAML file with the resources:

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <!-- Add more resources here -->
        <ResourceDictionary Source="MyResourceDictionary.xaml" />
        <!-- Add more resources here -->
    </ContentPage.Resources>
    ...
</ContentPage>
```

This syntax does not instantiate the `MyResourceDictionary` class. Instead, it references the XAML file. For that reason, when setting the `Source` property, a code-behind file isn't required, and the `x:Class` attribute can be removed from the root tag of the **MyResourceDictionary.xaml** file.

> [!IMPORTANT]
> The `ResourceDictionary.Source` property can only be set from XAML.

### Merge resource dictionaries from other assemblies

A <xref:Microsoft.Maui.Controls.ResourceDictionary> can also be merged into another <xref:Microsoft.Maui.Controls.ResourceDictionary> by adding it into the `MergedDictionaries` property of the <xref:Microsoft.Maui.Controls.ResourceDictionary>. This technique allows resource dictionaries to be merged, regardless of the assembly in which they reside. Merging resource dictionaries from external assemblies requires the <xref:Microsoft.Maui.Controls.ResourceDictionary> to have a build action set to **MauiXaml**, to have a code-behind file, and to define the `x:Class` attribute in the root tag of the file.

> [!WARNING]
> The <xref:Microsoft.Maui.Controls.ResourceDictionary> class also defines a `MergedWith` property. However, this property has been deprecated and should no longer be used.

The following code example shows two resource dictionaries being added to the `MergedDictionaries` collection of a page level <xref:Microsoft.Maui.Controls.ResourceDictionary>:

```xaml
<ContentPage ...
             xmlns:local="clr-namespace:ResourceDictionaryDemo"
             xmlns:theme="clr-namespace:MyThemes;assembly=MyThemes">
    <ContentPage.Resources>
        <ResourceDictionary>
            <!-- Add more resources here -->
            <ResourceDictionary.MergedDictionaries>
                <!-- Add more resource dictionaries here -->
                <local:MyResourceDictionary />
                <theme:DefaultTheme />
                <!-- Add more resource dictionaries here -->
            </ResourceDictionary.MergedDictionaries>
            <!-- Add more resources here -->
        </ResourceDictionary>
    </ContentPage.Resources>
    ...
</ContentPage>
```

In this example, a resource dictionary from the same assembly, and a resource dictionary from an external assembly, are merged into the page level resource dictionary. In addition, you can also add other <xref:Microsoft.Maui.Controls.ResourceDictionary> objects within the `MergedDictionaries` property-element tags, and other resources outside of those tags.

> [!IMPORTANT]
> There can be only one `MergedDictionaries` property-element tag in a <xref:Microsoft.Maui.Controls.ResourceDictionary>, but you can put as many <xref:Microsoft.Maui.Controls.ResourceDictionary> objects in there as required.

When merged <xref:Microsoft.Maui.Controls.ResourceDictionary> resources share identical `x:Key` attribute values, .NET MAUI uses the following resource precedence:

1. The resources local to the resource dictionary.
1. The resources contained in the resource dictionaries that were merged via the `MergedDictionaries` collection, in the reverse order they are listed in the `MergedDictionaries` property.

> [!TIP]
> Searching resource dictionaries can be a computationally intensive task if an app contains multiple, large resource dictionaries. Therefore, to avoid unnecessary searching, you should ensure that each page in an application only uses resource dictionaries that are appropriate to the page.

### Consume a XAML-based resource dictionary from code

Resource dictionaries that are defined in XAML can be consumed in code, provided that the <xref:Microsoft.Maui.Controls.ResourceDictionary> is backed by a code-behind file. In Visual Studio, XAML-based <xref:Microsoft.Maui.Controls.ResourceDictionary> files that are backed by code-behind files can be added to your project by the **.NET MAUI ResourceDictionary (XAML)** item template:

:::image type="content" source="media/resource-dictionaries/resource-dictionaries-code-behind.png" alt-text="Screenshot of resource dictionaries backed by code-behind.":::

XAML-based resource dictionaries that are backed by code-behind files can then be consumed from C# by adding them to the <xref:Microsoft.Maui.Controls.ResourceDictionary.MergedDictionaries> collection of the resource dictionary:

```csharp
Resources.MergedDictionaries.Add(new MyMauiApp.Resources.Styles.MyColors());
Resources.MergedDictionaries.Add(new MyMauiApp.Resources.Styles.MyStyles());
```

## Access resources by key from code

You can access resources in a resource dictionary from code like any other dictionary.

The following example shows how to retrieve and apply a resource from a page's resource dictionary:

```csharp
// Retrieve the Primary color value which is in the page's resource dictionary
var hasValue = Resources.TryGetValue("Primary", out object primaryColor);

if (hasValue)
{
    myLabel.TextColor = (Color)primaryColor;
}
```

This is the recommended approach that ensures that .NET MAUI won't throw a `KeyNotFoundException` if it's unable to retrieve a resource from code. This can occur when a merged resource dictionary is made up of resources defined in a XAML file, and inline resources. For more information, see [GitHub issue #11214](https://github.com/dotnet/maui/pull/11214).

> [!NOTE]
> To retrieve app-wide resources from code, access the `App.Current.Resources` resource dictionary.
