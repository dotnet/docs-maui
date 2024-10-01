---
title: "Theme an app"
description: "Theming can be implemented in .NET MAUI apps by creating a ResourceDictionary for each theme, and then loading the resources with the DynamicResource markup extension."
ms.date: 09/30/2024
---

# Theme an app

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-theming)

.NET Multi-platform App UI (.NET MAUI) apps can respond to style changes dynamically at runtime by using the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension. This markup extension is similar to the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension, in that both use a dictionary key to fetch a value from a <xref:Microsoft.Maui.Controls.ResourceDictionary>. However, while the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension performs a single dictionary lookup, the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension maintains a link to the dictionary key. Therefore, if the value associated with the key is replaced, the change is applied to the <xref:Microsoft.Maui.Controls.VisualElement>. This enables runtime theming to be implemented in .NET MAUI apps.

The process for implementing runtime theming in a .NET MAUI app is as follows:

1. Define the resources for each theme in a <xref:Microsoft.Maui.Controls.ResourceDictionary>. For more information, see [Define themes](#define-themes).
1. Set a default theme in the app's *App.xaml* file. For more information, see [Set a default theme](#set-a-default-theme).
1. Consume theme resources in the app, using the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension. For more information, see [Consume theme resources](#consume-theme-resources).
1. Add code to load a theme at runtime. For more information, see [Load a theme at runtime](#load-a-theme-at-runtime).

> [!IMPORTANT]
> Use the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension if your app doesn’t need to change themes dynamically at runtime. If you anticipate switching themes while the app is running, use the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension, which enables resources to be updated at runtime.

The following screenshot shows themed pages, with the iOS app using a light theme and the Android app using a dark theme:

:::image type="content" source="media/theming/main-page-both-themes.png" alt-text="Screenshot of the main page of a themed app.":::

> [!NOTE]
> Changing a theme at runtime requires the use of XAML or C# style definitions, and isn't possible using CSS.

.NET MAUI also has the ability to respond to system theme changes. The system theme may change for a variety of reasons, depending on the device configuration. This includes the system theme being explicitly changed by the user, it changing due to the time of day, and it changing due to environmental factors such as low light. For more information, see [Respond to system theme changes](system-theme-changes.md).

## Define themes

A theme is defined as a collection of resource objects stored in a <xref:Microsoft.Maui.Controls.ResourceDictionary>.

The following example shows a <xref:Microsoft.Maui.Controls.ResourceDictionary> for a light theme named `LightTheme`:

```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                    x:Class="ThemingDemo.LightTheme">
    <Color x:Key="PageBackgroundColor">White</Color>
    <Color x:Key="NavigationBarColor">WhiteSmoke</Color>
    <Color x:Key="PrimaryColor">WhiteSmoke</Color>
    <Color x:Key="SecondaryColor">Black</Color>
    <Color x:Key="PrimaryTextColor">Black</Color>
    <Color x:Key="SecondaryTextColor">White</Color>
    <Color x:Key="TertiaryTextColor">Gray</Color>
    <Color x:Key="TransparentColor">Transparent</Color>
</ResourceDictionary>
```

The following example shows a <xref:Microsoft.Maui.Controls.ResourceDictionary> for a dark theme named `DarkTheme`:

```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                    x:Class="ThemingDemo.DarkTheme">
    <Color x:Key="PageBackgroundColor">Black</Color>
    <Color x:Key="NavigationBarColor">Teal</Color>
    <Color x:Key="PrimaryColor">Teal</Color>
    <Color x:Key="SecondaryColor">White</Color>
    <Color x:Key="PrimaryTextColor">White</Color>
    <Color x:Key="SecondaryTextColor">White</Color>
    <Color x:Key="TertiaryTextColor">WhiteSmoke</Color>
    <Color x:Key="TransparentColor">Transparent</Color>
</ResourceDictionary>
```

Each <xref:Microsoft.Maui.Controls.ResourceDictionary> contains <xref:Microsoft.Maui.Graphics.Color> resources that define their respective themes, with each <xref:Microsoft.Maui.Controls.ResourceDictionary> using identical key values. For more information about resource dictionaries, see [Resource Dictionaries](~/fundamentals/resource-dictionaries.md).

> [!IMPORTANT]
> A code behind file is required for each <xref:Microsoft.Maui.Controls.ResourceDictionary>, which calls the `InitializeComponent` method. This is necessary so that a CLR object representing the chosen theme can be created at runtime.

## Set a default theme

An app requires a default theme, so that controls have values for the resources they consume. A default theme can be set by merging the theme's <xref:Microsoft.Maui.Controls.ResourceDictionary> into the app-level <xref:Microsoft.Maui.Controls.ResourceDictionary> that's defined in *App.xaml*:

```xaml
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="ThemingDemo.App">
    <Application.Resources>
        <ResourceDictionary Source="Themes/LightTheme.xaml" />
    </Application.Resources>
</Application>
```

For more information about merging resource dictionaries, see [Merged resource dictionaries](~/fundamentals/resource-dictionaries.md#merge-resource-dictionaries).

## Consume theme resources

When an app wants to consume a resource that's stored in a <xref:Microsoft.Maui.Controls.ResourceDictionary> that represents a theme, it should do so with the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension. This ensures that if a different theme is selected at runtime, the values from the new theme will be applied.

The following example shows three styles from that can be applied to all <xref:Microsoft.Maui.Controls.Label> objects in app:

```xaml
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="ThemingDemo.App">
    <Application.Resources>

        <Style x:Key="LargeLabelStyle"
               TargetType="Label">
            <Setter Property="TextColor"
                    Value="{DynamicResource SecondaryTextColor}" />
            <Setter Property="FontSize"
                    Value="30" />
        </Style>

        <Style x:Key="MediumLabelStyle"
               TargetType="Label">
            <Setter Property="TextColor"
                    Value="{DynamicResource PrimaryTextColor}" />
            <Setter Property="FontSize"
                    Value="25" />
        </Style>

        <Style x:Key="SmallLabelStyle"
               TargetType="Label">
            <Setter Property="TextColor"
                    Value="{DynamicResource TertiaryTextColor}" />
            <Setter Property="FontSize"
                    Value="15" />
        </Style>

    </Application.Resources>
</Application>
```

These styles are defined in the app-level resource dictionary, so that they can be consumed by multiple pages. Each style consumes theme resources with the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension.

These styles are then consumed by pages:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:ThemingDemo"
             x:Class="ThemingDemo.UserSummaryPage"
             Title="User Summary"
             BackgroundColor="{DynamicResource PageBackgroundColor}">
    ...
    <ScrollView>
        <Grid>
            <Grid.RowDefinitions>
                <RowDefinition Height="200" />
                <RowDefinition Height="120" />
                <RowDefinition Height="70" />
            </Grid.RowDefinitions>
            <Grid BackgroundColor="{DynamicResource PrimaryColor}">
                <Label Text="Face-Palm Monkey"
                       VerticalOptions="Center"
                       Margin="15"
                       Style="{StaticResource MediumLabelStyle}" />
                ...
            </Grid>
            <StackLayout Grid.Row="1"
                         Margin="10">
                <Label Text="This monkey reacts appropriately to ridiculous assertions and actions."
                       Style="{StaticResource SmallLabelStyle}" />
                <Label Text="  &#x2022; Cynical but not unfriendly."
                       Style="{StaticResource SmallLabelStyle}" />
                <Label Text="  &#x2022; Seven varieties of grimaces."
                       Style="{StaticResource SmallLabelStyle}" />
                <Label Text="  &#x2022; Doesn't laugh at your jokes."
                       Style="{StaticResource SmallLabelStyle}" />
            </StackLayout>
            ...
        </Grid>
    </ScrollView>
</ContentPage>
```

When a theme resource is consumed directly, it should be consumed with the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension. However, when a style that uses the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension is consumed, it should be consumed with the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension.

For more information about styling, see [Style apps using XAML](~/user-interface/styles/xaml.md). For more information about the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension, see [Dynamic styles](~/user-interface/styles/xaml.md#dynamic-styles).

## Load a theme at runtime

When a theme is selected at runtime, an app should:

1. Remove the current theme from the app. This is achieved by clearing the `MergedDictionaries` property of the app-level <xref:Microsoft.Maui.Controls.ResourceDictionary>.
2. Load the selected theme. This is achieved by adding an instance of the selected theme to the `MergedDictionaries` property of the app-level <xref:Microsoft.Maui.Controls.ResourceDictionary>.

Any <xref:Microsoft.Maui.Controls.VisualElement> objects that set properties with the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension will then apply the new theme values. This occurs because the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension maintains a link to dictionary keys. Therefore, when the values associated with keys are replaced, the changes are applied to the <xref:Microsoft.Maui.Controls.VisualElement> objects.

In the sample application, a theme is selected via a modal page that contains a <xref:Microsoft.Maui.Controls.Picker>. The following code shows the `OnPickerSelectionChanged` method, which is executed when the selected theme changes:

The following example shows removing the current theme and loading a new theme:

```csharp
ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
if (mergedDictionaries != null)
{
    mergedDictionaries.Clear();
    mergedDictionaries.Add(new DarkTheme());
}
```
