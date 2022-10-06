---
title: "Display a menu bar in a .NET MAUI desktop app"
description: "Learn how to add a .NET MAUI menu bar at the top of a desktop app."
ms.date: 10/05/2022
---

# Display a menu bar in a .NET MAUI desktop app

A .NET Multi-platform App UI (.NET MAUI) menu bar is a container that presents a set of menus in a horizontal row, at the top of a desktop app.

Each top-level menu in the menu bar is represented by a `MenuBarItem` object. `MenuBarItem` defines the following properties:

- `Text`, of type `string`, defines the menu text.
- `IsEnabled`, of type `boolean`, specifies whether the menu is enabled. The default value of this property is `true`.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

An item in a menu is represented by a `MenuFlyoutItem`, and a sub menu for a menu is represented by a `MenuFlyoutSubItem`. `MenuFlyoutSubItem` derives from `MenuFlyoutItem`, which in turn derives from `MenuItem`. `MenuItem` defines multiple properties that enable the appearance and behavior of a menu item to be specified. <!-- For more information, see [Menu items](). -->

::: moniker range=">=net-maui-7.0"
In addition, a horizontal line that separates items in a menu is represented by a `MenuFlyoutSeparator`.
::: moniker-end

## Create menu bar items

`MenuBarItem` objects can be added to the `MenuBarItems` collection, of type `IList<MenuBarItem>`, on a `ContentPage`. .NET MAUI desktop apps will display a menu bar, containing menu items, when they are added to any `ContentPage` that's hosted in a `NavigationPage` or a Shell app.

The following example shows a `ContentPage` that defines menu bar items:

::: moniker range="=net-maui-6.0"
```xaml
<ContentPage ...>
    <ContentPage.MenuBarItems>
        <MenuBarItem Text="File">
            <MenuFlyoutItem Text="Exit"
                            Command="{Binding ExitCommand}" />
        </MenuBarItem>
        <MenuBarItem Text="Locations">
            <MenuFlyoutSubItem Text="Change Location">
                <MenuFlyoutItem Text="Redmond, USA"
                                Command="{Binding ChangeLocationCommand}"
                                CommandParameter="Redmond" />
                <MenuFlyoutItem Text="London, UK"
                                Command="{Binding ChangeLocationCommand}"
                                CommandParameter="London" />
                <MenuFlyoutItem Text="Berlin, DE"
                                Command="{Binding ChangeLocationCommand}"
                                CommandParameter="Berlin"/>
            </MenuFlyoutSubItem>           
            <MenuFlyoutItem Text="Add Location"
                            Command="{Binding AddLocationCommand}" />                         
        </MenuBarItem>
        <MenuBarItem Text="View">
            <MenuFlyoutItem Text="Refresh"
                            Command="{Binding RefreshCommand}" />
            <MenuFlyoutItem Text="Change Theme"
                            Command="{Binding ChangeThemeCommand}" />
        </MenuBarItem>
    </ContentPage.MenuBarItems>
</ContentPage>
```

This example defines three top-level menus. Each top-level menu has menu items, and the second top-level menu has a sub-menu:

:::image type="content" source="media/menubar/menubar-net6.png" alt-text="Screenshot of menu bar in .NET 6.":::

::: moniker-end

::: moniker range=">=net-maui-7.0"
```xaml
<ContentPage ...>
    <ContentPage.MenuBarItems>
        <MenuBarItem Text="File">
            <MenuFlyoutItem Text="Exit"
                            Command="{Binding ExitCommand}" />
        </MenuBarItem>
        <MenuBarItem Text="Locations">
            <MenuFlyoutSubItem Text="Change Location">
                <MenuFlyoutItem Text="Redmond, USA"
                                Command="{Binding ChangeLocationCommand}"
                                CommandParameter="Redmond" />
                <MenuFlyoutItem Text="London, UK"
                                Command="{Binding ChangeLocationCommand}"
                                CommandParameter="London" />
                <MenuFlyoutItem Text="Berlin, DE"
                                Command="{Binding ChangeLocationCommand}"
                                CommandParameter="Berlin"/>
            </MenuFlyoutSubItem>
            <MenuFlyoutSeparator />            
            <MenuFlyoutItem Text="Add Location"
                            Command="{Binding AddLocationCommand}" />
            <MenuFlyoutItem Text="Edit Location"
                            Command="{Binding EditLocationCommand}" />
            <MenuFlyoutItem Text="Remove Location"
                            Command="{Binding RemoveLocationCommand}" />                            
        </MenuBarItem>
        <MenuBarItem Text="View">
            <MenuFlyoutItem Text="Refresh"
                            Command="{Binding RefreshCommand}" />
            <MenuFlyoutItem Text="Change Theme"
                            Command="{Binding ChangeThemeCommand}" />
        </MenuBarItem>
    </ContentPage.MenuBarItems>
</ContentPage>
```

This example defines three top-level menus. Each top-level menu has menu items, and the second top-level menu has a sub-menu and a separator:

:::image type="content" source="media/menubar/menubar-net7.png" alt-text="Screenshot of menu bar in .NET 7.":::

::: moniker-end

In this example, each `MenuFlyoutItem` defines a menu item that executes an `ICommand` when selected.

## Mac Catalyst limitations

.NET MAUI Mac Catalyst apps are limited to 50 menu items. Attempting to add more than 50 menu items to a Mac Catalyst app will result in an exception being thrown.

Additional menu items, beyond the 50 limit, can be added to a menu bar by adding the following code to your `AppDelegate` class:

```csharp
[Export("MenuItem50: ")]
internal void MenuItem50(UICommand uICommand)
{
    uICommand.SendClicked();
}
```
