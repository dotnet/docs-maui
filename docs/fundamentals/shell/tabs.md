---
title: ".NET MAUI Shell tabs"
description: "Learn how to customize and control a .NET MAUI TabBar, which represents the bottom tab bar in a .NET MAUI Shell app."
ms.date: 11/28/2025
---

# .NET MAUI Shell tabs

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-shell)

The navigation experience provided by .NET Multi-platform App UI (.NET MAUI) Shell is based on flyouts and tabs. The top level of navigation in a Shell app is either a flyout or a bottom tab bar, depending on the navigation requirements of the app. When the navigation experience for an app begins with bottom tabs, the child of the subclassed <xref:Microsoft.Maui.Controls.Shell> object should be a <xref:Microsoft.Maui.Controls.TabBar> object, which represents the bottom tab bar.

A <xref:Microsoft.Maui.Controls.TabBar> object can contain one or more <xref:Microsoft.Maui.Controls.Tab> objects, with each <xref:Microsoft.Maui.Controls.Tab> object representing a tab on the bottom tab bar. Each <xref:Microsoft.Maui.Controls.Tab> object can contain one or more <xref:Microsoft.Maui.Controls.ShellContent> objects, with each <xref:Microsoft.Maui.Controls.ShellContent> object displaying a single <xref:Microsoft.Maui.Controls.ContentPage>. When more than one <xref:Microsoft.Maui.Controls.ShellContent> object is present in a <xref:Microsoft.Maui.Controls.Tab> object, the <xref:Microsoft.Maui.Controls.ContentPage> objects are navigable by top tabs. Within a tab, you can navigate to other <xref:Microsoft.Maui.Controls.ContentPage> objects that are known as detail pages.

> [!IMPORTANT]
> The <xref:Microsoft.Maui.Controls.TabBar> type disables the flyout.

> [!TIP]
> Tabs can be displayed with a flyout by adding multiple <xref:Microsoft.Maui.Controls.ShellContent> objects to a <xref:Microsoft.Maui.Controls.FlyoutItem> object or <xref:Microsoft.Maui.Controls.Tab> object. For more information, see [.NET MAUI Shell flyout](flyout.md#flyout-items).

## Single page

A single page Shell app can be created by adding a <xref:Microsoft.Maui.Controls.Tab> object to a <xref:Microsoft.Maui.Controls.TabBar> object. Within the <xref:Microsoft.Maui.Controls.Tab> object, a <xref:Microsoft.Maui.Controls.ShellContent> object should be set to a <xref:Microsoft.Maui.Controls.ContentPage> object:

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

Shell has implicit-conversion operators that enable the Shell visual hierarchy to be simplified without introducing more views into the visual tree. This simplification is possible because a subclassed <xref:Microsoft.Maui.Controls.Shell> object can only ever contain <xref:Microsoft.Maui.Controls.FlyoutItem> objects or a <xref:Microsoft.Maui.Controls.TabBar> object, which can only ever contain <xref:Microsoft.Maui.Controls.Tab> objects, which can only ever contain <xref:Microsoft.Maui.Controls.ShellContent> objects. These implicit-conversion operators can be used to remove the <xref:Microsoft.Maui.Controls.Tab> objects from the previous example:

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

This implicit conversion automatically wraps the <xref:Microsoft.Maui.Controls.ShellContent> object in a <xref:Microsoft.Maui.Controls.Tab> object, which is wrapped in a <xref:Microsoft.Maui.Controls.TabBar> object.

> [!IMPORTANT]
> In a Shell app, pages are created on demand in response to navigation. This is accomplished by using the <xref:Microsoft.Maui.Controls.DataTemplate> markup extension to set the `ContentTemplate` property of each <xref:Microsoft.Maui.Controls.ShellContent> object to a <xref:Microsoft.Maui.Controls.ContentPage> object.

## Bottom tabs

If there are multiple <xref:Microsoft.Maui.Controls.Tab> objects in a single <xref:Microsoft.Maui.Controls.TabBar> object, <xref:Microsoft.Maui.Controls.Tab> objects are rendered as bottom tabs:

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

The `Title` property, of type `string`, defines the tab title. The `Icon` property, of type <xref:Microsoft.Maui.Controls.ImageSource>, defines the tab icon:

:::image type="content" source="media/tabs/two-page-app-bottom-tabs.png" alt-text="Screenshot of a Shell two page app with bottom tabs.":::

When there are more than five tabs on a <xref:Microsoft.Maui.Controls.TabBar>, a **More** tab appears, which can be used to access the other tabs:

:::image type="content" source="media/tabs/more-tabs.png" alt-text="Screenshot of a Shell app with a More tab.":::

In addition, Shell's implicit conversion operators can be used to remove the <xref:Microsoft.Maui.Controls.ShellContent> and <xref:Microsoft.Maui.Controls.Tab> objects from the previous example:

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

This implicit conversion automatically wraps each <xref:Microsoft.Maui.Controls.ShellContent> object in a <xref:Microsoft.Maui.Controls.Tab> object.

> [!IMPORTANT]
> In a Shell app, pages are created on demand in response to navigation. This is accomplished by using the <xref:Microsoft.Maui.Controls.DataTemplate> markup extension to set the `ContentTemplate` property of each <xref:Microsoft.Maui.Controls.ShellContent> object to a <xref:Microsoft.Maui.Controls.ContentPage> object.

## Bottom and top tabs

When more than one <xref:Microsoft.Maui.Controls.ShellContent> object is present in a <xref:Microsoft.Maui.Controls.Tab> object, a top tab bar is added to the bottom tab, through which the <xref:Microsoft.Maui.Controls.ContentPage> objects are navigable:

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

This code results in the layout shown in the following screenshot:

:::image type="content" source="media/tabs/two-page-app-top-tabs.png" alt-text="Screenshot of a Shell two page app with top and bottom tabs.":::

In addition, Shell's implicit conversion operators can be used to remove the second <xref:Microsoft.Maui.Controls.Tab> object from the previous example:

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

This implicit conversion automatically wraps the third <xref:Microsoft.Maui.Controls.ShellContent> object in a <xref:Microsoft.Maui.Controls.Tab> object.

## Tab appearance

The <xref:Microsoft.Maui.Controls.Shell> class defines the following attached properties that control the appearance of tabs:

- `TabBarBackgroundColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the background color for the tab bar. If the property is unset, the `BackgroundColor` property value is used.
- `TabBarDisabledColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the disabled color for the tab bar. If the property is unset, the `DisabledColor` property value is used.
- `TabBarForegroundColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the foreground color for the tab bar. If the property is unset, the `ForegroundColor` property value is used.
- `TabBarTitleColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the title color for the tab bar. If the property is unset, the `TitleColor` property value is used.
- `TabBarUnselectedColor`, of type <xref:Microsoft.Maui.Graphics.Color>, that defines the unselected color for the tab bar. If the property is unset, the `UnselectedColor` property value is used.

All of these properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings, and styled.

The three properties that most influence the color of a tab are `TabBarForegroundColor`, `TabBarTitleColor`, and `TabBarUnselectedColor`:

- If only the `TabBarTitleColor` property is set then its value will be used to color the title and icon of the selected tab. If `TabBarTitleColor` isn't set then the title color will match the value of the `TabBarForegroundColor` property.
- If the `TabBarForegroundColor` property is set and the `TabBarUnselectedColor` property isn't set then the value of the `TabBarForegroundColor` property will be used to color the title and icon of the selected tab.
- If only the `TabBarUnselectedColor` property is set then its value will be used to color the title and icon of the unselected tab.

For example:

- When the `TabBarTitleColor` property is set to `Green` the title and icon for the selected tab is green, and unselected tabs match system colors.
- When the `TabBarForegroundColor` property is set to `Blue` the title and icon for the selected tab is blue, and unselected tabs match system colors.
- When the `TabBarTitleColor` property is set to `Green` and the `TabBarForegroundColor` property is set to `Blue` the title is green and the icon is blue for the selected tab, and unselected tabs match system colors.
- When the `TabBarTitleColor` property is set to `Green` and the `Shell.ForegroundColor` property is set to `Blue` the title is green and the icon is blue for the selected tab, and unselected tabs match system colors. This occurs because the `Shell.ForegroundColor` property value propagates to the `TabBarForegroundColor` property.
- When the `TabBarTitleColor` property is set to `Green`, the `TabBarForegroundColor` property is set to `Blue`, and the `TabBarUnselectedColor` property is set to `Red`, the title is green and the icon is blue for the selected tab, and unselected tab titles and icons are red.

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

When a Shell app that uses a tab bar is first run, the `Shell.CurrentItem` property is set to the first <xref:Microsoft.Maui.Controls.Tab> object in the subclassed <xref:Microsoft.Maui.Controls.Shell> object. However, the property can be set to another <xref:Microsoft.Maui.Controls.Tab>, as shown in the following example:

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

This example sets the `CurrentItem` property to the <xref:Microsoft.Maui.Controls.ShellContent> object named `dogsItem`, which results in it being selected and displayed. In this example, an implicit conversion is used to wrap each <xref:Microsoft.Maui.Controls.ShellContent> object in a <xref:Microsoft.Maui.Controls.Tab> object.

The equivalent C# code, given a <xref:Microsoft.Maui.Controls.ShellContent> object named `dogsItem`, is:

```csharp
CurrentItem = dogsItem;
```

In this example, the `CurrentItem` property is set in the subclassed <xref:Microsoft.Maui.Controls.Shell> class. Alternatively, the `CurrentItem` property can be set in any class through the `Shell.Current` static property:

```csharp
Shell.Current.CurrentItem = dogsItem;
```

## TabBar and Tab visibility

The tab bar and tabs are visible in Shell apps by default. However, the tab bar can be hidden by setting the `Shell.TabBarIsVisible` attached property to `false`.

While this property can be set on a subclassed <xref:Microsoft.Maui.Controls.Shell> object, it's typically set on any <xref:Microsoft.Maui.Controls.ShellContent> or <xref:Microsoft.Maui.Controls.ContentPage> objects that want to make the tab bar invisible:

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

In addition, <xref:Microsoft.Maui.Controls.Tab> objects can be hidden by setting the `IsVisible` bindable property to `false`:

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
