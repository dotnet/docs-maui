---
title: "Create a .NET MAUI Shell app"
description: "Learn how to create a .NET MAUI Shell app, and how to describe the visual hierarchy of the app in the AppShell class."
ms.date: 08/30/2024
---

# Create a .NET MAUI Shell app

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-shell)

A .NET Multi-platform App UI (.NET MAUI) Shell app can be created with the **.NET MAUI App** project template, and then by describing the visual hierarchy of the app in the `AppShell` class.

For a step-by-step walkthrough of how to create a Shell app, see [Create a .NET MAUI app](../../tutorials/notes-app.md).

## Describe the visual hierarchy of the app

The visual hierarchy of a .NET MAUI Shell app is described in the subclassed <xref:Microsoft.Maui.Controls.Shell> class, which the project template names `AppShell`. A subclassed <xref:Microsoft.Maui.Controls.Shell> class consists of three main hierarchical objects:

1. <xref:Microsoft.Maui.Controls.FlyoutItem> or <xref:Microsoft.Maui.Controls.TabBar>. A <xref:Microsoft.Maui.Controls.FlyoutItem> represents one or more items in the flyout, and should be used when the navigation pattern for the app requires a flyout. A <xref:Microsoft.Maui.Controls.TabBar> represents the bottom tab bar, and should be used when the navigation pattern for the app begins with bottom tabs and doesn't require a flyout. Every <xref:Microsoft.Maui.Controls.FlyoutItem> object or <xref:Microsoft.Maui.Controls.TabBar> object is a child of the <xref:Microsoft.Maui.Controls.Shell> object.
1. <xref:Microsoft.Maui.Controls.Tab>, which represents grouped content, navigable by bottom tabs. Every <xref:Microsoft.Maui.Controls.Tab> object is a child of a <xref:Microsoft.Maui.Controls.FlyoutItem> object or <xref:Microsoft.Maui.Controls.TabBar> object.
1. <xref:Microsoft.Maui.Controls.ShellContent>, which represents the <xref:Microsoft.Maui.Controls.ContentPage> objects for each tab. Every <xref:Microsoft.Maui.Controls.ShellContent> object is a child of a <xref:Microsoft.Maui.Controls.Tab> object. When more than one <xref:Microsoft.Maui.Controls.ShellContent> object is present in a <xref:Microsoft.Maui.Controls.Tab>, the objects will be navigable by top tabs.

These objects don't represent any user interface, but rather the organization of the app's visual hierarchy. Shell will take these objects and produce the navigation user interface for the content.

The following XAML shows an example of a subclassed <xref:Microsoft.Maui.Controls.Shell> class:

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:Xaminals.Views"
       x:Class="Xaminals.AppShell">
    ...
    <FlyoutItem FlyoutDisplayOptions="AsMultipleItems">
        <Tab Title="Domestic"
             Icon="paw.png">
            <ShellContent Title="Cats"
                          Icon="cat.png"
                          ContentTemplate="{DataTemplate views:CatsPage}" />
            <ShellContent Title="Dogs"
                          Icon="dog.png"
                          ContentTemplate="{DataTemplate views:DogsPage}" />
        </Tab>
        <!--
        Shell has implicit conversion operators that enable the Shell visual hierarchy to be simplified.
        This is possible because a subclassed Shell object can only ever contain a FlyoutItem object or a TabBar object,
        which can only ever contain Tab objects, which can only ever contain ShellContent objects.

        The implicit conversion automatically wraps the ShellContent objects below in Tab objects.
        -->
        <ShellContent Title="Monkeys"
                      Icon="monkey.png"
                      ContentTemplate="{DataTemplate views:MonkeysPage}" />
        <ShellContent Title="Elephants"
                      Icon="elephant.png"
                      ContentTemplate="{DataTemplate views:ElephantsPage}" />
        <ShellContent Title="Bears"
                      Icon="bear.png"
                      ContentTemplate="{DataTemplate views:BearsPage}" />
    </FlyoutItem>
    ...
</Shell>
```

When run, this XAML displays the `CatsPage`, because it's the first item of content declared in the subclassed <xref:Microsoft.Maui.Controls.Shell> class:

:::image type="content" source="media/create/cats.png" alt-text="Screenshot of a Shell app":::

Pressing the hamburger icon, or swiping from the left, displays the flyout:

:::image type="content" source="media/create/flyout.png" alt-text="Screenshot of a Shell flyout.":::

Multiple items are displayed on the flyout because the <xref:Microsoft.Maui.Controls.FlyoutDisplayOptions> property is set to `AsMultipleItems`. For more information, see [Flyout display options](flyout.md#flyout-display-options).

> [!IMPORTANT]
> In a Shell app, pages are created on demand in response to navigation. This is accomplished by using the <xref:Microsoft.Maui.Controls.DataTemplate> markup extension to set the `ContentTemplate` property of each <xref:Microsoft.Maui.Controls.ShellContent> object to a <xref:Microsoft.Maui.Controls.ContentPage> object.
