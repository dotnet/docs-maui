---
title: ".NET MAUI Shell tabs"
description: "Learn how to customize and control a .NET MAUI TabBar, which represents the bottom tab bar in a .NET MAUI Shell app."
ms.date: 04/07/2022
---

# .NET MAUI Shell tabs

The navigation experience provided by .NET Multi-platform App UI (.NET MAUI) Shell is based on flyouts and tabs. The top level of navigation in a Shell app is either a flyout or a bottom tab bar, depending on the navigation requirements of the app. When the navigation experience for an app begins with bottom tabs, the child of the subclassed `Shell` object should be a `TabBar` object, which represents the bottom tab bar.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

A `TabBar` object can contain one or more `Tab` objects, with each `Tab` object representing a tab on the bottom tab bar. Each `Tab` object can contain one or more `ShellContent` objects, with each `ShellContent` object displaying a single `ContentPage`. When more than one `ShellContent` object is present in a `Tab` object, the `ContentPage` objects will be navigable by top tabs. Within a tab, additional `ContentPage` objects that are known as detail pages, can be navigated to.

> [!IMPORTANT]
> The `TabBar` type disables the flyout.

## Single page

A single page Shell app can be created by adding a `Tab` object to a `TabBar` object. Within the `Tab` object, a `ShellContent` object should be set to a `ContentPage` object:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    <TabBar>
       <Tab>
           <ShellContent ContentTemplate="{DataTemplate views:CatsPage}" />
       </Tab>
    </TabBar>
</Shell>
```

This example results in the following single page app:

:::image type="content" source="media/tabs/single-page-app.png" alt-text="Screenshot of a Shell single page app.":::

Shell has implicit conversion operators that enable the Shell visual hierarchy to be simplified, without introducing additional views into the visual tree. This is possible because a subclassed `Shell` object can only ever contain `FlyoutItem` objects or a `TabBar` object, which can only ever contain `Tab` objects, which can only ever contain `ShellContent` objects. These implicit conversion operators can be used to remove the `Tab` objects from the previous example:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    <Tab>
        <ShellContent ContentTemplate="{DataTemplate views:CatsPage}" />
    </Tab>
</Shell>
```

This implicit conversion automatically wraps the `ShellContent` object in a `Tab` object, which is wrapped in a `TabBar` object.

> [!IMPORTANT]
> In a Shell app, pages are created on demand in response to navigation. This is accomplished by using the `DataTemplate` markup extension to set the `ContentTemplate` property of each `ShellContent` object to a `ContentPage` object.

## Bottom tabs

`Tab` objects are rendered as bottom tabs, provided that there are multiple `Tab` objects in a single `TabBar` object:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    <TabBar>
       <Tab Title="Cats"
            Icon="cat.png">
           <ShellContent ContentTemplate="{DataTemplate views:CatsPage}" />
       </Tab>
       <Tab Title="Dogs"
            Icon="dog.png">
           <ShellContent ContentTemplate="{DataTemplate views:DogsPage}" />
       </Tab>
    </TabBar>
</Shell>
```

The `Title` property, of type `string`, defines the tab title. The `Icon` property, of type `ImageSource`, defines the tab icon:

:::image type="content" source="media/tabs/two-page-app-bottom-tabs.png" alt-text="Screenshot of a Shell two page app with bottom tabs.":::

When there are more than five tabs on a `TabBar`, a **More** tab will appear, which can be used to access the additional tabs:

:::image type="content" source="media/tabs/more-tabs.png" alt-text="Screenshot of a Shell app with a More tab.":::

In addition, Shell's implicit conversion operators can be used to remove the `ShellContent` and `Tab` objects from the previous example:

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
    </TabBar>
</Shell>
```

This implicit conversion automatically wraps each `ShellContent` object in a `Tab` object.

> [!IMPORTANT]
> In a Shell app, pages are created on demand in response to navigation. This is accomplished by using the `DataTemplate` markup extension to set the `ContentTemplate` property of each `ShellContent` object to a `ContentPage` object.

## Bottom and top tabs

When more than one `ShellContent` object is present in a `Tab` object, a top tab bar is added to the bottom tab, through which the `ContentPage` objects are navigable:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    <TabBar>
       <Tab Title="Domestic"
            Icon="paw.png">
           <ShellContent Title="Cats"
                         ContentTemplate="{DataTemplate views:CatsPage}" />
           <ShellContent Title="Dogs"
                         ContentTemplate="{DataTemplate views:DogsPage}" />
       </Tab>
       <Tab Title="Monkeys"
            Icon="monkey.png">
           <ShellContent ContentTemplate="{DataTemplate views:MonkeysPage}" />
       </Tab>
    </TabBar>
</Shell>
```

This results in the layout shown in the following screenshot:

:::image type="content" source="media/tabs/two-page-app-top-tabs.png" alt-text="Screenshot of a Shell two page app with top and bottom tabs.":::

In addition, Shell's implicit conversion operators can be used to remove the second `Tab` object from the previous example:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    <TabBar>
       <Tab Title="Domestic"
            Icon="paw.png">
           <ShellContent Title="Cats"
                         Icon="cat.png"
                         ContentTemplate="{DataTemplate views:CatsPage}" />
           <ShellContent Title="Dogs"
                         Icon="dog.png"
                         ContentTemplate="{DataTemplate views:DogsPage}" />
       </Tab>
       <ShellContent Title="Monkeys"
                     Icon="monkey.png"
                     ContentTemplate="{DataTemplate views:MonkeysPage}" />
    </TabBar>
</Shell>
```

This implicit conversion automatically wraps the third `ShellContent` object in a `Tab` object.

## Tab appearance

The `Shell` class defines the following attached properties that control the appearance of tabs:

- `TabBarBackgroundColor`, of type `Color`, that defines the background color for the tab bar. If the property is unset, the `BackgroundColor` property value is used.
- `TabBarDisabledColor`, of type `Color`, that defines the disabled color for the tab bar. If the property is unset, the `DisabledColor` property value is used.
- `TabBarForegroundColor`, of type `Color`, that defines the foreground color for the tab bar. If the property is unset, the `ForegroundColor` property value is used.
- `TabBarTitleColor`, of type `Color`, that defines the title color for the tab bar. If the property is unset, the `TitleColor` property value will be used.
- `TabBarUnselectedColor`, of type `Color`, that defines the unselected color for the tab bar. If the property is unset, the `UnselectedColor` property value is used.

All of these properties are backed by `BindableProperty` objects, which means that the properties can be targets of data bindings, and styled.

The following example shows a XAML style that sets different tab bar color properties:

```xaml
<Style TargetType="TabBar">
    <Setter Property="Shell.TabBarBackgroundColor"
            Value="CornflowerBlue" />
    <Setter Property="Shell.TabBarTitleColor"
            Value="Black" />
    <Setter Property="Shell.TabBarUnselectedColor"
            Value="AntiqueWhite" />
</Style>
```

In addition, tabs can also be styled using Cascading Style Sheets (CSS). For more information, see [.NET MAUI Shell specific properties](~/user-interface/styles/css.md#net-maui-shell-specific-properties).

## Tab selection

When a Shell app that uses a tab bar is first run, the `Shell.CurrentItem` property will be set to the first `Tab` object in the subclassed `Shell` object. However, the property can be set to another `Tab`, as shown in the following example:

```xaml
<Shell ...
       CurrentItem="{x:Reference dogsItem}">
    <TabBar>
        <ShellContent Title="Cats"
                      Icon="cat.png"
                      ContentTemplate="{DataTemplate views:CatsPage}" />
        <ShellContent x:Name="dogsItem"
                      Title="Dogs"
                      Icon="dog.png"
                      ContentTemplate="{DataTemplate views:DogsPage}" />
    </TabBar>
</Shell>
```

This example sets the `CurrentItem` property to the `ShellContent` object named `dogsItem`, which results in it being selected and displayed. In this example, an implicit conversion is used to wrap each `ShellContent` object in a `Tab` object.

The equivalent C# code, given a `ShellContent` object named `dogsItem`, is:

```csharp
CurrentItem = dogsItem;
```

In this example, the `CurrentItem` property is set in the subclassed `Shell` class. Alternatively, the `CurrentItem` property can be set in any class through the `Shell.Current` static property:

```csharp
Shell.Current.CurrentItem = dogsItem;
```

## TabBar and Tab visibility

The tab bar and tabs are visible in Shell apps by default. However, the tab bar can be hidden by setting the `Shell.TabBarIsVisible` attached property to `false`.

While this property can be set on a subclassed `Shell` object, it's typically set on any `ShellContent` or `ContentPage` objects that want to make the tab bar invisible:

```xaml
<TabBar>
   <Tab Title="Domestic"
        Icon="paw.png">
       <ShellContent Title="Cats"
                     ContentTemplate="{DataTemplate views:CatsPage}" />
       <ShellContent Shell.TabBarIsVisible="false"
                     Title="Dogs"
                     ContentTemplate="{DataTemplate views:DogsPage}" />
   </Tab>
   <Tab Title="Monkeys"
        Icon="monkey.png">
       <ShellContent ContentTemplate="{DataTemplate views:MonkeysPage}" />
   </Tab>
</TabBar>
```

In this example, the tab bar is hidden when the upper **Dogs** tab is selected.

In addition, `Tab` objects can be hidden by setting the `IsVisible` bindable property to `false`:

```xaml
<TabBar>
    <ShellContent Title="Cats"
                  Icon="cat.png"
                  ContentTemplate="{DataTemplate views:CatsPage}" />
    <ShellContent Title="Dogs"
                  Icon="dog.png"
                  ContentTemplate="{DataTemplate views:DogsPage}"
                  IsVisible="False" />
    <ShellContent Title="Monkeys"
                  Icon="monkey.png"
                  ContentTemplate="{DataTemplate views:MonkeysPage}" />
</TabBar>
```

In this example, the second tab is hidden.
