---
title: ".NET MAUI Shell pages"
description: "Learn how to customize the appearance of pages in .NET MAUI Shell apps, including setting page colors, disabling the navigation bar, disabling the tab bar, and displaying views in the navigation bar."
ms.date: 08/30/2024
---

# .NET MAUI Shell pages

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-shell)

A <xref:Microsoft.Maui.Controls.ShellContent> object represents the <xref:Microsoft.Maui.Controls.ContentPage> object for each <xref:Microsoft.Maui.Controls.FlyoutItem> or <xref:Microsoft.Maui.Controls.Tab>. When more than one <xref:Microsoft.Maui.Controls.ShellContent> object is present in a <xref:Microsoft.Maui.Controls.Tab> object, the <xref:Microsoft.Maui.Controls.ContentPage> objects are navigable by top tabs. Within a page, you can navigate to additional <xref:Microsoft.Maui.Controls.ContentPage> objects known as detail pages.

In addition, the <xref:Microsoft.Maui.Controls.Shell> class defines attached properties that can be used to configure the appearance of pages in .NET Multi-platform App UI (.NET MAUI) Shell apps. This configuration includes setting page colors, setting the page presentation mode, disabling the navigation bar, disabling the tab bar, and displaying views in the navigation bar.

## Display pages

In .NET MAUI Shell apps, pages are typically created on demand in response to navigation. This creation is accomplished by using the <xref:Microsoft.Maui.Controls.DataTemplate> markup extension to set the `ContentTemplate` property of each <xref:Microsoft.Maui.Controls.ShellContent> object to a <xref:Microsoft.Maui.Controls.ContentPage> object:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    <TabBar>
       <ShellContent Title="Cats"
                     Icon="cat.png"
                     ContentTemplate="{DataTemplate views:CatsPage}" />
       <ShellContent Title="Dogs"
                     Icon="dog.png"
                     ContentTemplate="{DataTemplate views:DogsPage}" />
       <ShellContent Title="Monkeys"
                     Icon="monkey.png"
                     ContentTemplate="{DataTemplate views:MonkeysPage}" />
    </TabBar>
</Shell>
```

In this example, Shell's implicit conversion operators are used to remove the <xref:Microsoft.Maui.Controls.Tab> objects from the visual hierarchy. However, each <xref:Microsoft.Maui.Controls.ShellContent> object is rendered in a tab:

:::image type="content" source="media/pages/three-pages.png" alt-text="Screenshot of a Shell app with three pages.":::

> [!NOTE]
> The `BindingContext` of each <xref:Microsoft.Maui.Controls.ShellContent> object is inherited from the parent <xref:Microsoft.Maui.Controls.Tab> object.

Within each <xref:Microsoft.Maui.Controls.ContentPage> object, additional <xref:Microsoft.Maui.Controls.ContentPage> objects can be navigated to. For more information about navigation, see [.NET MAUI Shell navigation](navigation.md).

## Load pages at app startup

In a Shell app, each <xref:Microsoft.Maui.Controls.ContentPage> object is typically created on demand, in response to navigation. However, it's also possible to create <xref:Microsoft.Maui.Controls.ContentPage> objects at app startup.

> [!WARNING]
> <xref:Microsoft.Maui.Controls.ContentPage> objects that are created at app startup can lead to a poor startup experience.

<xref:Microsoft.Maui.Controls.ContentPage> objects can be created at app startup by setting the `ShellContent.Content` properties to <xref:Microsoft.Maui.Controls.ContentPage> objects:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    <TabBar>
     <ShellContent Title="Cats"
                   Icon="cat.png">
         <views:CatsPage />
     </ShellContent>
     <ShellContent Title="Dogs"
                   Icon="dog.png">
         <views:DogsPage />
     </ShellContent>
     <ShellContent Title="Monkeys"
                   Icon="monkey.png">
         <views:MonkeysPage />
     </ShellContent>
    </TabBar>
</Shell>
```

In this example, `CatsPage`, `DogsPage`, and `MonkeysPage` are all created at app startup, rather than on demand in response to navigation.

> [!NOTE]
> The `Content` property is the content property of the <xref:Microsoft.Maui.Controls.ShellContent> class, and therefore does not need to be explicitly set.

## Set page colors

The <xref:Microsoft.Maui.Controls.Shell> class defines the following attached properties that can be used to set page colors in a Shell app:

- `BackgroundColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the background color in the Shell chrome. The color won't fill in behind the Shell content.
- `DisabledColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the color to shade text and icons that are disabled.
- `ForegroundColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the color to shade text and icons.
- `TitleColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the color used for the title of the current page.
- `UnselectedColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the color used for unselected text and icons in the Shell chrome.

All of these properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which mean that the properties can be targets of data bindings, and styled using XAML styles. In addition, the properties can be set using Cascading Style Sheets (CSS). For more information, see [.NET MAUI Shell specific properties](~/user-interface/styles/css.md#net-maui-shell-specific-properties).

> [!NOTE]
> There are also properties that enable tab colors to be defined. For more information, see [Tab appearance](tabs.md#tab-appearance).

The following XAML shows setting the color properties in a subclassed <xref:Microsoft.Maui.Controls.Shell> class:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       x:Class="Xaminals.AppShell"
       BackgroundColor="#455A64"
       ForegroundColor="White"
       TitleColor="White"
       DisabledColor="#B4FFFFFF"
       UnselectedColor="#95FFFFFF">

</Shell>
```

In this example, the color values are applied to all pages in the Shell app, unless they're overridden at the page level.

Because the color properties are attached properties, they can also be set on individual pages, to set the colors on that page:

```xaml
<ContentPage ...
             Shell.BackgroundColor="Gray"
             Shell.ForegroundColor="White"
             Shell.TitleColor="Blue"
             Shell.DisabledColor="#95FFFFFF"
             Shell.UnselectedColor="#B4FFFFFF">

</ContentPage>
```

Alternatively, the color properties can be set with a XAML style:

```xaml
<Style x:Key="DomesticShell"
       TargetType="Element" >
    <Setter Property="Shell.BackgroundColor"
            Value="#039BE6" />
    <Setter Property="Shell.ForegroundColor"
            Value="White" />
    <Setter Property="Shell.TitleColor"
            Value="White" />
    <Setter Property="Shell.DisabledColor"
            Value="#B4FFFFFF" />
    <Setter Property="Shell.UnselectedColor"
            Value="#95FFFFFF" />
</Style>
```

For more information about XAML styles, see [Style apps using XAML](~/user-interface/styles/xaml.md).

## Set page presentation mode

By default, a small navigation animation occurs when a page is navigated to with the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method. However, this behavior can be changed by setting the `Shell.PresentationMode` attached property on a <xref:Microsoft.Maui.Controls.ContentPage> to one of the `PresentationMode` enumeration members:

- `NotAnimated` indicates that the page will be displayed without a navigation animation.
- `Animated` indicates that the page will be displayed with a navigation animation. This is the default value of the `Shell.PresentationMode` attached property.
- `Modal` indicates that the page will be displayed as a modal page.
- `ModalAnimated` indicates that the page will be displayed as a modal page, with a navigation animation.
- `ModalNotAnimated` indicates that the page will be displayed as a modal page, without a navigation animation.

> [!IMPORTANT]
> The `PresentationMode` type is a flags enumeration. This means that a combination of enumeration members can be applied in code. However, for ease of use in XAML, the `ModalAnimated` member is a combination of the `Animated` and `Modal` members, and the `ModalNotAnimated` member is a combination of the `NotAnimated` and `Modal` members. For more information about flag enumerations, see [Enumeration types as bit flags](/dotnet/csharp/language-reference/builtin-types/enum#enumeration-types-as-bit-flags).

The following XAML example sets the `Shell.PresentationMode` attached property on a <xref:Microsoft.Maui.Controls.ContentPage>:

```xaml
<ContentPage ...
             Shell.PresentationMode="Modal">
    ...             
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.ContentPage> is set to be displayed as a modal page, when the page is navigated to with the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method.

## Enable navigation bar shadow

The `Shell.NavBarHasShadow` attached property, of type `bool`, controls whether the navigation bar has a shadow. By default the value of the property is `true` on Android, and `false` on other platforms.

While this property can be set on a subclassed <xref:Microsoft.Maui.Controls.Shell> object, it can also be set on any pages that want to enable the navigation bar shadow. For example, the following XAML shows enabling the navigation bar shadow from a <xref:Microsoft.Maui.Controls.ContentPage>:

```xaml
<ContentPage ...
             Shell.NavBarHasShadow="true">
    ...
</ContentPage>
```

This results in the navigation bar shadow being enabled.

## Disable the navigation bar

The `Shell.NavBarIsVisible` attached property, of type `bool`, controls if the navigation bar is visible when a page is presented. By default the value of the property is `true`.

While this property can be set on a subclassed <xref:Microsoft.Maui.Controls.Shell> object, it's typically set on any pages that want to make the navigation bar invisible. For example, the following XAML shows disabling the navigation bar from a <xref:Microsoft.Maui.Controls.ContentPage>:

```xaml
<ContentPage ...
             Shell.NavBarIsVisible="false">
    ...
</ContentPage>
```

::: moniker range=">=net-maui-10.0"

## Control NavBar visibility animation (.NET 10)

In .NET 10, the <xref:Microsoft.Maui.Controls.Shell> class adds the `NavBarVisibilityAnimationEnabled` attached property to control whether changes to the navigation bar's visibility are animated. By default, this property is `true`.

You can set this attached property on any <xref:Microsoft.Maui.Controls.Page> to disable or enable the animation when showing or hiding the navigation bar:

```xaml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MyApp.MyPage"
    Shell.NavBarVisibilityAnimationEnabled="False">
    ...
    <!-- Later you can also toggle Shell.NavBarIsVisible to show/hide without animation -->
    <!-- Shell.NavBarIsVisible="False" -->
    ...
</ContentPage>
```

In C#, use the static attached property accessors:

```csharp
// Disable animation for NavBar visibility changes on a page
Shell.SetNavBarVisibilityAnimationEnabled(myPage, false);

// Query the current setting
bool isEnabled = Shell.GetNavBarVisibilityAnimationEnabled(myPage);
```

This setting affects the animation applied when <xref:Microsoft.Maui.Controls.Shell.NavBarIsVisibleProperty?displayProperty=nameWithType> changes on the target element.
::: moniker-end

## Display views in the navigation bar

The `Shell.TitleView` attached property, of type <xref:Microsoft.Maui.Controls.View>, enables any <xref:Microsoft.Maui.Controls.View> to be displayed in the navigation bar.

While this property can be set on a subclassed <xref:Microsoft.Maui.Controls.Shell> object, it can also be set on any pages that want to display a view in the navigation bar. For example, the following XAML shows displaying an <xref:Microsoft.Maui.Controls.Image> in the navigation bar of a <xref:Microsoft.Maui.Controls.ContentPage>:

```xaml
<ContentPage ...>
    <Shell.TitleView>
        <Image Source="logo.png"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
    </Shell.TitleView>
    ...
</ContentPage>
```

> [!IMPORTANT]
> If the navigation bar has been made invisible, with the `NavBarIsVisible` attached property, the title view will not be displayed.

Many views won't appear in the navigation bar unless the size of the view is specified with the <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties or the location of the view is specified with the `HorizontalOptions` and `VerticalOptions` properties.

The `TitleView` attached property can be set to display a layout class that contains multiple views. Similarly, because the <xref:Microsoft.Maui.Controls.ContentView> class ultimately derives from the <xref:Microsoft.Maui.Controls.View> class, the `TitleView` attached property can be set to display a <xref:Microsoft.Maui.Controls.ContentView> that contains a single view.

## Page visibility

Shell respects page visibility, which is set with the `IsVisible` property. When a page's `IsVisible` property is set to `false`, it's not visible in the Shell app and it's not possible to navigate to it.
