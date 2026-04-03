---
title: "Element interaction and automation (experimental)"
description: "Learn how to interact with UI elements in a running .NET MAUI app using DevFlow, including tapping, filling text, scrolling, navigating, and mutating properties."
ms.date: 04/03/2026
---

# Element interaction and automation (experimental)

DevFlow provides commands for interacting with UI elements in a running MAUI app, including tapping, filling text, scrolling, navigating, and mutating properties.

> [!IMPORTANT]
> DevFlow is an **experimental** package from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. It is **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. These packages are provided as-is with no guarantees of support, servicing, or updates.

## Tap an element

Use the `interact tap` command to simulate a tap on a UI element. Elements are identified by their `AutomationId`:

```console
maui devflow agent interact tap --automationid "MyButton"
```

Set `AutomationId` on your MAUI controls to make them discoverable by DevFlow:

```xml
<Button AutomationId="MyButton" Text="Click me" />
```

The tap simulates a user interaction on the target element, triggering any associated event handlers or commands.

## Fill text

Use the `interact fill` command to enter text into `Entry` or `Editor` controls:

```console
maui devflow agent interact fill --automationid "UsernameEntry" --text "testuser@example.com"
```

This clears the existing text and enters the specified value, similar to a user typing into the field.

## Scroll

Use the `interact scroll` command to scroll within scrollable containers such as `ScrollView` or `CollectionView`:

```console
maui devflow agent interact scroll --automationid "MyScrollView" --direction down
```

Supported scroll directions include `up`, `down`, `left`, and `right`.

## Navigate

DevFlow supports navigation commands for apps that use `Shell` or `NavigationPage`:

```console
maui devflow agent interact navigate --route "//MainPage/Details"
```

For Shell-based apps, specify the route using Shell URI syntax. For `NavigationPage`-based apps, specify the target page type.

## Mutate properties

You can change element properties at runtime for debugging purposes. This is useful for testing different visual states or verifying layout behavior without rebuilding the app:

```console
maui devflow agent interact mutate --automationid "MyLabel" --property "Text" --value "Updated text"
```

Property mutations are applied immediately to the running app. Changes are not persisted and are lost when the app is restarted.

## See also

- [Visual tree inspection and screenshots](visual-tree-screenshots.md)
- [DevFlow overview](index.md)
