---
title: "Display a menu bar in a .NET MAUI desktop app"
description: "Learn how to add a .NET MAUI menu bar at the top of a desktop app."
ms.date: 10/05/2022
---

# Display a menu bar in a .NET MAUI desktop app

A .NET Multi-platform App UI (.NET MAUI) menu bar is a container that presents a set of menus in a horizontal row, at the top of an app on Mac Catalyst and Windows.

Each top-level menu in the menu bar, known as a menu bar item, is represented by a <xref:Microsoft.Maui.Controls.MenuBarItem> object. <xref:Microsoft.Maui.Controls.MenuBarItem> defines the following properties:

- `Text`, of type `string`, defines the menu text.
- `IsEnabled`, of type `boolean`, specifies whether the menu is enabled. The default value of this property is `true`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

A <xref:Microsoft.Maui.Controls.MenuBarItem> can consist of the following children:

- <xref:Microsoft.Maui.Controls.MenuFlyoutItem>, which represents a menu item that can be clicked.
- <xref:Microsoft.Maui.Controls.MenuFlyoutSubItem>, which represents a sub-menu item that can be clicked.
- <xref:Microsoft.Maui.Controls.MenuFlyoutSeparator>, which is a horizontal line that separates items in the menu.

<xref:Microsoft.Maui.Controls.MenuFlyoutSubItem> derives from <xref:Microsoft.Maui.Controls.MenuFlyoutItem>, which in turn derives from  <xref:Microsoft.Maui.Controls.MenuItem>.  <xref:Microsoft.Maui.Controls.MenuItem> defines multiple properties that enable the appearance and behavior of a menu item to be specified. The appearance of a menu item, or sub-item, can be defined by setting the `Text`, and `IconImageSource` properties. The response to a menu item, or sub-item, click can be defined by setting the `Clicked`, `Command`, and `CommandParameter` properties. For more information about menu items, see [Display menu items](menuitem.md).

## Create menu bar items

<xref:Microsoft.Maui.Controls.MenuBarItem> objects can be added to the <xref:Microsoft.Maui.Controls.Page.MenuBarItems> collection, of type `IList<MenuBarItem>`, on a <xref:Microsoft.Maui.Controls.ContentPage>. .NET MAUI desktop apps will display a menu bar, containing menu items, when they are added to any <xref:Microsoft.Maui.Controls.ContentPage> that's hosted in a <xref:Microsoft.Maui.Controls.NavigationPage> or a Shell app.

The following example shows a <xref:Microsoft.Maui.Controls.ContentPage> that defines menu bar items:

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

:::image type="content" source="media/menubar/menubar.png" alt-text="Screenshot of menu bar.":::

> [!NOTE]
> On Mac Catalyst, menu items are added to the system menu bar.

In this example, each <xref:Microsoft.Maui.Controls.MenuFlyoutItem> defines a menu item that executes an <xref:System.Windows.Input.ICommand> when selected.

::: moniker range=">=net-maui-8.0"

Keyboard accelerators can be added to menu items in a menu bar, so that a menu item can be invoked through a keyboard shortcut. For more information, see [Keyboard accelerators](~/user-interface/keyboard-accelerators.md).

::: moniker-end

## Display icons on menu items

<xref:Microsoft.Maui.Controls.MenuFlyoutItem> and <xref:Microsoft.Maui.Controls.MenuFlyoutSubItem> inherit the `IconImageSource` property from  <xref:Microsoft.Maui.Controls.MenuItem>, which enables a small icon to be displayed next to the text for a menu item. This icon can either be an image, or a font icon.

> [!WARNING]
> Mac Catalyst does not support displaying icons on menu items.

The following example shows a menu bar item, where the icons for menu items are defined using font icons:

```xaml
<ContentPage.MenuBarItems>
    <MenuBarItem Text="Media">
        <MenuFlyoutItem Text="Play">
            <MenuFlyoutItem.IconImageSource>
                <FontImageSource Glyph="&#x25B6;"
                                 FontFamily="Arial" />
            </MenuFlyoutItem.IconImageSource>
        </MenuFlyoutItem>
        <MenuFlyoutItem Text="Pause"
                        Clicked="OnPauseClicked">
            <MenuFlyoutItem.IconImageSource>
                <FontImageSource Glyph="&#x23F8;"
                                 FontFamily="Arial" />
            </MenuFlyoutItem.IconImageSource>
        </MenuFlyoutItem>
        <MenuFlyoutItem Text="Stop"
                        Clicked="OnStopClicked">
            <MenuFlyoutItem.IconImageSource>
                <FontImageSource Glyph="&#x23F9;"
                                 FontFamily="Arial" />
            </MenuFlyoutItem.IconImageSource>
        </MenuFlyoutItem>
    </MenuBarItem>
</ContentPage.MenuBarItems>
```

In this example, the menu bar item defines three menu items that display an icon and text on Windows.

For more information about displaying font icons, see [Display font icons](~/user-interface/fonts.md#display-font-icons). For information about adding images to .NET MAUI projects, see [Add images to a .NET MAUI app project](~/user-interface/images/images.md).

::: moniker range="=net-maui-7.0"

## Mac Catalyst limitations

.NET MAUI Mac Catalyst apps are limited to 50 menu items. Attempting to add more than 50 menu items to a Mac Catalyst app will result in an exception being thrown.

Additional menu items, beyond the 50 limit, can be added to a menu bar by adding the following code to your `AppDelegate` class:

```csharp
[Export("MenuItem50:")]
internal void MenuItem50(UICommand uICommand)
{
    uICommand.SendClicked();
}
```

::: moniker-end
