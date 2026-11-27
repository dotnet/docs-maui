---
title: "Element interaction and automation"
description: "Learn how to interact with UI elements in a running .NET MAUI app using DevFlow, including tapping, filling text, scrolling, navigating, and setting properties."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# Element interaction and automation

DevFlow provides `maui devflow ui` commands for interacting with UI elements in a running .NET MAUI app, including tapping, filling text, scrolling, navigating, and setting properties.

> [!IMPORTANT]
> DevFlow is experimental and might change between releases. The commands in this article were validated against `Microsoft.Maui.Cli` and `Microsoft.Maui.DevFlow.Agent` version `0.1.0-preview.14.26567.2`. Run `maui devflow ui --help` to see the commands available in your installed version.

## Target an app

All interaction commands run against the DevFlow agent in your running app. If only one app with the DevFlow agent is running, DevFlow targets it automatically. If several apps are connected, list them and pass the port of the app you want to target with `--agent-port` (or `-ap`):

```console
maui devflow list
maui devflow ui status --agent-port 10224
```

Replace `10224` with the port that `maui devflow list` shows for your app. The examples in this article omit `--agent-port` for brevity.

> [!TIP]
> Each CLI invocation uses its own control session. After a command changes the app, a second modifying command that runs within about 10 seconds might fail. To run several modifying commands back to back, use the `batch` command. For more information, see [Run several commands in sequence](#run-several-commands-in-sequence).

## Find elements

Most interaction commands target an element by its element ID, or resolve the element for you with `--automationId`, `--type`, or `--text`. Set `AutomationId` on your .NET MAUI controls to make them easy to find:

```xml
<Button AutomationId="LoginButton" Text="Log in" />
```

Use the `ui query` command to find elements and see their element IDs:

```console
maui devflow ui query --automationId "LoginButton"
maui devflow ui query --type Button
maui devflow ui query --selector "Button:visible"
```

In the output, the element ID is shown in square brackets. For elements that have an `AutomationId`, the element ID is the `AutomationId` value. Other elements get a generated ID, such as `794bdb55f8ca`, that you can pass to commands that require an element ID.

The `ui query` command returns all matches. For commands such as `ui tap`, `ui fill`, and `ui clear`, when several elements match `--automationId`, `--type`, or `--text`, add `--index` to select a match by its zero-based position.

## Tap an element

Use the `ui tap` command to simulate a tap on a UI element:

```console
maui devflow ui tap --automationId "LoginButton"
```

You can also pass an element ID directly:

```console
maui devflow ui tap LoginButton
```

The tap triggers any associated event handlers or commands. To tap a specific point inside an element, such as content drawn on a canvas, add `--x` and `--y` in device-independent units relative to the element's top-left corner.

## Fill text

Use the `ui fill` command to enter text into controls such as `Entry` and `Editor`. Pass the text as the last argument:

```console
maui devflow ui fill --automationId "UsernameEntry" "testuser@example.com"
```

This replaces the existing text with the specified value. To remove the text, use the `ui clear` command:

```console
maui devflow ui clear --automationId "UsernameEntry"
```

## Scroll

Use the `ui scroll` command to scroll content. Pass element IDs with `--element`. For elements that have an `AutomationId`, pass the `AutomationId` value.

To scroll an element into view, pass the element you want to see:

```console
maui devflow ui scroll --element "BottomLabel"
```

To scroll by a specific amount, pass `--dx` for horizontal scrolling or `--dy` for vertical scrolling. A positive `--dy` value scrolls toward the end of the content, and a negative value scrolls back toward the start:

```console
maui devflow ui scroll --element "MainScrollView" --dy 500
maui devflow ui scroll --element "MainScrollView" --dy -300
```

For a `CollectionView` or `ListView`, scroll to an item by index with `--item-index`. Use `--group-index` for grouped collections and `--position` to choose where the item ends up (`MakeVisible`, `Start`, `Center`, or `End`):

```console
maui devflow ui scroll --element "ItemsCollection" --item-index 20 --position Center
```

For swipe, pan, pinch, and other touch gestures, use the `ui gesture` command. For more information, run `maui devflow ui gesture --help`.

## Navigate

Use the `ui navigate` command to navigate in apps that use `Shell`. Pass a Shell route as the argument:

```console
maui devflow ui navigate "//settings"
```

The route is passed to `Shell.Current.GoToAsync`, so you can use absolute routes, such as `//settings`, registered relative routes, such as `details`, and `..` to go back. For more information about route syntax, see [.NET MAUI Shell navigation](../../fundamentals/shell/navigation.md).

> [!NOTE]
> The `ui navigate` command requires a `Shell`-based app. It doesn't support apps that use `NavigationPage` without `Shell`.

## Get and set properties

Use the `ui property` command to read a property value from an element, and the `ui set-property` command to change it at runtime. Both commands take an element ID and a property name. The `ui set-property` command also takes the new value:

```console
maui devflow ui property StatusLabel Text
maui devflow ui set-property StatusLabel Text "Updated text"
maui devflow ui set-property StatusLabel TextColor Red
```

Setting properties is useful for testing different visual states or verifying layout behavior without rebuilding the app. Changes are applied immediately to the running app. They aren't persisted and are lost when the app restarts.

To verify a property value, use the `ui assert` command. It exits with an error when the value doesn't match:

```console
maui devflow ui assert --automationId "StatusLabel" Text "Updated text"
```

## Run several commands in sequence

When you run separate commands that change the app, such as `tap`, `fill`, or `navigate`, a command might fail with the message "Another DevFlow session is driving this app" if the previous command ran less than about 10 seconds earlier. Wait at least 10 seconds and run the command again, or use the `batch` command to run a sequence of `ui` commands in one session.

The `batch` command reads one command per line from standard input. In Bash or zsh:

```bash
printf 'ui fill --automationId UsernameEntry testuser\nui tap --automationId LoginButton\nui navigate //settings\n' | maui devflow batch --human
```

In PowerShell:

```powershell
"ui fill --automationId UsernameEntry testuser", "ui tap --automationId LoginButton", "ui navigate //settings" | maui devflow batch --human
```
 Omit `--human` to get JSON Lines output for scripts and tools.

## See also

- [Visual tree inspection and screenshots](visual-tree-screenshots.md)
- [DevFlow overview](index.md)
