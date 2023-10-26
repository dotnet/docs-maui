---
title: "Keyboard accelerators"
description: "Learn how to define keyboard accelerators on Mac Catalyst and Windows so that menu items can be invoked through keyboard shortcuts."
ms.date: 10/25/2023
monikerRange: ">=net-maui-8.0"
---

# Keyboard accelerators

Keyboard accelerators are keyboard shortcuts that improve the usability and accessibility of your .NET Multi-platform App UI (.NET MAUI) apps on Mac Catalyst and Windows by providing an intuitive way for users to invoke common actions or commands without navigating the app UI directly.

A keyboard accelerator is composed of two components:

- Modifiers, which include Shift, Ctrl, and Alt.
- Keys, which include all alphanumeric keys.

In .NET MAUI, keyboard accelerators are associated with commands exposed in menus, and should be specified with the menu item. Specifically, .NET MAUI keyboard accelerators can be attached to menu items in the menu bar on Mac Catalyst and Windows, and menu items in context menus on Windows. For more information about menu bars, see [Display a menu bar in a .NET MAUI desktop app](menu-bar.md). For more information about context menus, see [Display a context menu in a .NET MAUI desktop app](context-menu.md).

The following screenshots show menu bar items and context menu items that include keyboard accelerators:

:::image type="content" source="media/keyboard-accelerators/menubar.png" alt-text="Screenshot of menu bar items that include keyboard accelerators.":::
:::image type="content" source="media/keyboard-accelerators/context-menu.png" alt-text="Screenshot of context menu items that include keyboard accelerators.":::

A keyboard accelerator is represented by the `KeyboardAccelerator` class, which represents a shortcut key for a <xref:Microsoft.Maui.Controls.MenuFlyoutItem>. The `KeyboardAccelerator` class defines the following properties:

- `Modifiers`, of type `KeyboardAcceleratorModifiers`, which represents the modifier value, such as Ctrl or Shift, for the keyboard shortcut.
- `Key`, of type `string?`, which represents the key value for the keyboard shortcut.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings.

The `KeyboardAcceleratorModifiers` enumeration defines the following members that be used as values for the `Modifiers` property:

- `None`, which indicates no modifier.
- `Shift`, which indicates the Shift modifier on Mac Catalyst and Windows.
- `Ctrl`, which indicates the Control modifier on Mac Catalyst and Windows.
- `Alt`, which indicates the Option modifier on Mac Catalyst, and the Menu modifier on Windows.
- `Cmd`, which indicates the Command modifier on Mac Catalyst.
- `Windows`, which indicates the Windows modifier on Windows.

> [!IMPORTANT]
> Keyboard accelerators can be attached to <xref:Microsoft.Maui.Controls.MenuFlyoutItem> object in a <xref:Microsoft.Maui.Controls.MenuBarItem> on Mac Catalyst and Windows, and in a <xref:Microsoft.Maui.Controls.MenuFlyout> on Windows.

<!-- Mac Catalyst support for keyboard accelerators in context menu items is pending. -->

The following table outlines the keyboard accelerator formats .NET MAUI supports:

| Platform | Format |
| -------- | ------ |
| Mac Catalyst | - Keyboard accelerators without a modifier, with a single key. For example, using the F1 to invoke the action associated with a menu item. <br> - Keyboard accelerators with one or more modifiers, with a single key. For example, using CTRL+SHIFT+F or CTRL+F to invoke the action associated with a menu item. |
| Windows | - Keyboard accelerators with and without a modifier, with a single key. For example, using the F1 to invoke the action associated with a menu item. <br> - Keyboard accelerators with one or more modifiers, with a single key. For example, using CTRL+SHIFT+F or CTRL+F to invoke the action associated with a menu item. |

## Create a keyboard accelerator

A `KeyboardAccelerator` can be attached to a <xref:Microsoft.Maui.Controls.MenuFlyoutItem> by adding it to its `KeyboardAccelerators` collection:

<!-- TODO: Why is KeyboardAccelerators a collection? Multiple allowed but only first executed on MacCat, but both executed on Win -->

```xaml
<MenuFlyoutItem Text="Cut"
                Clicked="OnCutMenuFlyoutItemClicked">
    <MenuFlyoutItem.KeyboardAccelerators>
        <KeyboardAccelerator Modifiers="Ctrl"
                             Key="X" />
        <KeyboardAccelerator Modifiers="Alt, Ctrl"
                             Key="X" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
<MenuFlyoutItem Text="Copy"
                Clicked="OnCopyMenuFlyoutItemClicked">
    <MenuFlyoutItem.KeyboardAccelerators>
        <KeyboardAccelerator Modifiers="Ctrl"
                             Key="C" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
<MenuFlyoutItem Text="Paste"
                Clicked="OnPasteMenuFlyoutItemClicked">
    <MenuFlyoutItem.KeyboardAccelerators>
        <KeyboardAccelerator Modifiers="Ctrl"
                             Key="V" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
```

Keyboard accelerators can also be specified in code:

```csharp
cutMenuFlyoutItem.KeyboardAccelerators.Add(new KeyboardAccelerator
{
    Modifiers = KeyboardAcceleratorModifiers.Ctrl,
    Key = "X"
});
```

When a keyboard accelerator modifier and key is pressed, the action associated with the <xref:Microsoft.Maui.Controls.MenuFlyoutItem> is invoked.

> [!IMPORTANT]
> While multiple `KeyboardAccelerator` objects can be added to the `MenuFlyoutItem.KeyboardAccelerators` collection, only the first `KeyboardAccelerator` in the collection will have its shortcut displayed on the <xref:Microsoft.Maui.Controls.MenuFlyoutItem>. On Mac Catalyst, only the first `KeyboardAccelerator` in the collection will cause the action associated with the <xref:Microsoft.Maui.Controls.MenuFlyoutItem> to be invoked. However, on Windows, all of the `KeyboardAccelerator` objects in the `MenuFlyoutItem.KeyboardAccelerators` collection will cause the <xref:Microsoft.Maui.Controls.MenuFlyoutItem> action to be invoked,

### Multiple modifiers

<!-- Todo: what about Mac Cat? -->

Multiple modifiers can be specified on a `KeyboardAccelerator`:

```xaml
<MenuFlyoutItem Text="Refresh"
                Command="{Binding RefreshCommand}">
    <MenuFlyoutItem.KeyboardAccelerators>
        <KeyboardAccelerator Modifiers="Shift,Ctrl"
                             Key="R" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
```

The equivalent C# code is:

```csharp
myMenuFlyoutItem.KeyboardAccelerators.Add(new KeyboardAccelerator
{
    Modifiers = KeyboardAcceleratorModifiers.Shift | KeyboardAcceleratorModifiers.Ctrl,
    Key = "R"
});
```

## Specify keyboard accelerators per platform

In addition, different keyboard accelerator modifiers and keys can be specified per platform with the [`OnPlatform`](xref:Microsoft.Maui.Controls.Xaml.OnPlatformExtension) markup extension:

```xaml
<MenuFlyoutItem Text="Change Theme"
                Command="{Binding ChangeThemeCommand}">
    <MenuFlyoutItem.KeyboardAccelerators>
        <KeyboardAccelerator Modifiers="{OnPlatform WinUI=Windows, MacCatalyst=Cmd}"
                             Key="{OnPlatform WinUI=C, MacCatalyst=T}" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
```

The equivalent C# code is:

```csharp
KeyboardAcceleratorModifiers modifier = KeyboardAcceleratorModifiers.None;
string key = string.Empty;

if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
{
    modifier = KeyboardAcceleratorModifiers.Windows;
    key = "C";
}
else if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
{
    modifier = KeyboardAcceleratorModifiers.Cmd;
    key = "T";
}
myMenuFlyoutItem.KeyboardAccelerators.Add(new KeyboardAccelerator
{
    Modifiers = modifier,
    Key = key
});
```

> [!NOTE]
> Keyboard accelerators aren't displayed for <xref:Microsoft.Maui.Controls.MenuFlyoutSubItem> objects, but do work on Windows.

## Special keys

On Windows, special keys can be specified via a constant name or with an integer. For a list of constants and integers, see the table in <xref:Windows.System.VirtualKey>.

> [!IMPORTANT]
> On Windows, single key accelerators (all alphanumeric and punctuation keys, Delete, F2, Spacebar, Esc, Multimedia Key) and multi-key accelerators (Ctrl+Shift+M) are supported. However, Gamepad virtual keys aren't supported.

On Mac Catalyst, special keys can be specified via a constant. For a list of constants that represent the text input strings that correspond to special nonvisible keys, see [Input strings for special keys](https://developer.apple.com/documentation/uikit/uikeycommand/input_strings_for_special_keys?language=objc) on developer.apple.com.

```xaml
<MenuFlyoutItem Text="Help"
                Command="{Binding HelpCommand}">
    <MenuFlyoutItem.KeyboardAccelerators>
        <!-- Alternatively, 112 can be used to specify F1 on Windows -->
        <KeyboardAccelerator Modifiers="None"
                             Key="{OnPlatform WinUI=F1, MacCatalyst=UIKeyInputF1}" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
```

In this example the keyboard accelerator is the F1 key, which is specified via a constant on both platforms. On Windows, it could also be specified by the integer 112.

## Localize a keyboard acclerator

Keyboard accelerator keys can be localized with a standard .NET resource file. The localized key can then be retrieved by using the `x:Static` markup extension to access the localized key value:

```xaml
<MenuFlyoutItem Text="Cut"
                Clicked="OnCutMenuFlyoutItemClicked">
    <MenuFlyoutItem.KeyboardAccelerators>
        <KeyboardAccelerator Modifiers="Ctrl"
                             Key="{x:Static local:AppResources.CutAcceleratorKey}" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
```

For more information, see [Localization](~/fundamentals/localization.md).

## Disable a keyboard accelerator

When a <xref:Microsoft.Maui.Controls.MenuFlyoutItem> is disabled, the associated accelerator is also disabled:

```xaml
<MenuFlyoutItem Text="Cut"
                Clicked="OnCutMenuFlyoutItemClicked"
                IsEnabled="false">
    <MenuFlyoutItem.KeyboardAccelerators>
        <KeyboardAccelerator Modifiers="Ctrl"
                             Key="X" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
```

In this example, because the `IsEnabled` property of the <xref:Microsoft.Maui.Controls.MenuFlyoutItem> is set to `false`, the associated CTRL+X keyboard accelerator can't be invoked.

----

https://learn.microsoft.com/en-us/windows/apps/design/input/keyboard-accelerators
