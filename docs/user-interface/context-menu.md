---
title: "Display a context menu in a .NET MAUI desktop app"
description: "Learn how to add a context menu to a .NET MAUI desktop app."
ms.date: 09/30/2024
---

# Display a context menu in a .NET MAUI desktop app

A context menu, often known as a right-click menu, offers contextual commands that are specific to the control being clicked on. In .NET Multi-platform App UI (.NET MAUI), a context menu can be added to any control that derives from <xref:Microsoft.Maui.Controls.Element>, on Mac Catalyst and Windows. This includes all pages, layouts, and views.

A context menu is defined with a `MenuFlyout`, which can consist of the following children:

- `MenuFlyoutItem`, which represents a menu item that can be clicked.
- `MenuFlyoutSubItem`, which represents a sub-menu item that can be clicked.
- `MenuFlyoutSeparator`, which is a horizontal line that separates items in the menu.

`MenuFlyoutSubItem` derives from `MenuFlyoutItem`, which in turn derives from  <xref:Microsoft.Maui.Controls.MenuItem>.  <xref:Microsoft.Maui.Controls.MenuItem> defines multiple properties that enable the appearance and behavior of a menu item to be specified. The appearance of a menu item, or sub-item, can be defined by setting the `Text`, and `IconImageSource` properties. The response to a menu item, or sub-item, click can be defined by setting the `Clicked`, `Command`, and `CommandParameter` properties. For more information about menu items, see [Display menu items](menuitem.md).

> [!WARNING]
> A context menu on an <xref:Microsoft.Maui.Controls.Entry> is currently unsupported on Mac Catalyst.

## Create context menu items

A `MenuFlyout` object can be added to the `FlyoutBase.ContextFlyout` attached property of any control that derives from <xref:Microsoft.Maui.Controls.Element>. When the user right-clicks on the control, the context menu will appear at the location where the pointer was clicked.

The following example shows a <xref:Microsoft.Maui.Controls.WebView> that defines a context menu:

```xaml
<WebView x:Name="webView"
         Source="https://learn.microsoft.com/dotnet/maui"
         MinimumHeightRequest="400">
    <FlyoutBase.ContextFlyout>
        <MenuFlyout>
            <MenuFlyoutItem Text="Go to docs repo"
                            Clicked="OnWebViewGoToRepoClicked"
                            CommandParameter="docs" />
            <MenuFlyoutItem Text="Go to engineering repo"
                            Clicked="OnWebViewGoToRepoClicked"
                            CommandParameter="eng" />
        </MenuFlyout>
    </FlyoutBase.ContextFlyout>
</WebView>
```

In this example, the context menu defines two menu items:

:::image type="content" source="media/context-menu/webview.png" alt-text="Screenshot of a context menu on a WebView.":::

When a menu item is clicked upon, the `OnWebViewGoToRepoClicked` event handler is executed:

```csharp
void OnWebViewGoToRepoClicked(object sender, EventArgs e)
{
    MenuFlyoutItem menuItem = sender as MenuFlyoutItem;
    string repo = menuItem.CommandParameter as string;
    string url = repo == "docs" ? "docs-maui" : "maui";
    webView.Source = new UrlWebViewSource { Url = $"https://github.com/dotnet/{url}" };
}
```

The `OnWebViewGoToRepoClicked` event handler retrieves the `CommandParameter` property value for the `MenuFlyoutItem` object that was clicked, and uses its value to build the URL that the <xref:Microsoft.Maui.Controls.WebView> navigates to.

> [!WARNING]
> It's not currently possible to add items to, or remove items from, the `MenuFlyout` at runtime.

Keyboard accelerators can be added to context menu items, so that a context menu item can be invoked through a keyboard shortcut. For more information, see [Keyboard accelerators](~/user-interface/keyboard-accelerators.md).

### Create sub-menu items

Sub-menu items can be added to a context menu by adding one or more `MenuFlyoutSubItem` objects to the `MenuFlyout`:

```xaml
<Label x:Name="label"
       Text="Right-click to choose color">
   <FlyoutBase.ContextFlyout>
       <MenuFlyout>
           <MenuFlyoutItem Text="Black"
                           Clicked="OnLabelClicked"
                           CommandParameter="Black" />
           <MenuFlyoutSubItem Text="Light">
               <MenuFlyoutItem Text="Blue"
                               Clicked="OnLabelClicked"
                               CommandParameter="LightBlue" />
               <MenuFlyoutItem Text="Coral"
                               Clicked="OnLabelClicked"
                               CommandParameter="LightCoral" />
               <MenuFlyoutItem Text="Cyan"
                               Clicked="OnLabelClicked"
                               CommandParameter="LightCyan" />
           </MenuFlyoutSubItem>
           <MenuFlyoutSubItem Text="Dark">
               <MenuFlyoutItem Text="Blue"
                               Clicked="OnLabelClicked"
                               CommandParameter="DarkBlue" />
               <MenuFlyoutItem Text="Cyan"
                               Clicked="OnLabelClicked"
                               CommandParameter="DarkCyan" />
               <MenuFlyoutItem Text="Magenta"
                               Clicked="OnLabelClicked"
                               CommandParameter="DarkMagenta" />
           </MenuFlyoutSubItem>
       </MenuFlyout>
   </FlyoutBase.ContextFlyout>
</Label>
```

In this example, the context menu defines a menu item and two sub-menus that each contain three menu items:

:::image type="content" source="media/context-menu/submenus.png" alt-text="Screenshot of a context menu with sub-menu items.":::
<!--
The following example shows the event handler that's executed when a context menu item is clicked:

```csharp
void OnLabelClicked(object sender, EventArgs e)
{
  MenuFlyoutItem menuItem = sender as MenuFlyoutItem;
  string color = menuItem.CommandParameter as string;
  label.TextColor = Color.Parse(color);
}
``` -->

## Display icons on menu items

`MenuFlyoutItem` and `MenuFlyoutSubItem` inherit the `IconImageSource` property from  <xref:Microsoft.Maui.Controls.MenuItem>, which enables a small icon to be displayed next to the text for a context menu item. This icon can either be an image, or a font icon.

> [!WARNING]
> Mac Catalyst does not support displaying icons on context menu items.

The following example shows a context menu, where the icons for menu items are defined using font icons:

```xaml
<Button Text="&#x25B6;&#xFE0F; Play"
        WidthRequest="80">
    <FlyoutBase.ContextFlyout>
        <MenuFlyout>
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
        </MenuFlyout>
    </FlyoutBase.ContextFlyout>
</Button>
```

In this example, the context menu defines two menu items that display an icon and text on Windows:

:::image type="content" source="media/context-menu/icons.png" alt-text="Screenshot of a context menu that uses icons.":::

For more information about displaying font icons, see [Display font icons](~/user-interface/fonts.md#display-font-icons). For information about adding images to .NET MAUI projects, see [Add images to a .NET MAUI app project](~/user-interface/images/images.md).
