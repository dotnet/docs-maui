---
title: "Visual tree inspection and screenshots"
description: "Learn how to inspect the .NET MAUI visual tree and capture screenshots with DevFlow using the CLI or HTTP API."
ms.date: 04/03/2026
---

# Visual tree inspection and screenshots

DevFlow lets you inspect the complete MAUI visual tree and capture screenshots from any supported platform, either through the CLI or the HTTP API.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## Prerequisites

- The DevFlow agent must be registered in your app. For setup instructions, see the [DevFlow overview](index.md).
- The app must be running on a supported platform with the agent active.

## Inspect the visual tree

Use the `maui devflow MAUI tree` command to dump the full visual tree of the running application:

```console
maui devflow MAUI tree
```

The output displays every element in the visual tree, including:

- Control types (for example, `Button`, `Label`, `StackLayout`)
- Element names and `AutomationId` values
- Bounding rectangles (position and size)
- Property values for each element

This is useful for understanding the current layout, debugging visibility issues, and verifying that elements are positioned correctly.

## Query elements with CSS selectors

DevFlow includes a CSS selector engine that lets you query specific elements in the visual tree. Use selectors to narrow down the elements you're interested in:

| Selector | Description |
|----------|-------------|
| `Button` | All buttons in the visual tree |
| `#MyButton` | Element with `AutomationId` "MyButton" |
| `.MyClass` | Elements with `StyleClass` "MyClass" |
| `StackLayout > Button` | Direct child buttons of a `StackLayout` |

For example, to find all buttons:

```console
maui devflow MAUI tree --query "Button"
```

CSS selectors are also used by the [element interaction](element-interaction.md) commands to identify target elements.

## Take a screenshot

Use the `maui devflow MAUI screenshot` command to capture a PNG screenshot of the running app:

```console
maui devflow MAUI screenshot -o screenshot.png
```

The screenshot is saved to the specified output path. This works on all supported platforms (Android, iOS, Mac Catalyst, and Windows).

## HTTP API

The DevFlow agent exposes HTTP endpoints for programmatic access to the visual tree and screenshot capabilities. When the agent is running, these features are available at the agent's HTTP port (either the default port or a broker-assigned port). This enables integration with custom tooling, test frameworks, and automation scripts.

## See also

- [DevFlow overview](index.md)
- [Element interaction and automation](element-interaction.md)
