---
title: "Inspect the visual tree of a .NET MAUI app"
description: "Learn how to examine a visual tree of the UI elements in your running .NET MAUI app."
ms.date: 08/27/2024
---

# Inspect the visual tree of a .NET MAUI app

.NET Multi-platform App UI (.NET MAUI) Live Visual Tree is a Visual Studio feature that provides a tree view of the UI elements in your running .NET MAUI app.

When your .NET MAUI app is running in debug configuration, with the debugger attached, the Live Visual Tree window can be opened by selecting **Debug > Windows > Live Visual Tree** from the Visual Studio menu bar:

:::image type="content" source="media/live-visual-tree/live-visual-tree.png" alt-text="Screenshot of the Live Visual Tree window in Visual Studio.":::

Provided that Hot Reload is enabled, the **Live Visual Tree** window will display the hierarchy of your app's UI elements regardless of whether the app's UI is built using XAML or C#. However, you will have to disable Just My XAML to display the hierarchy of your app's UI elements for UIs built using C#.

## Just My XAML

The view of the UI elements is simplified by default using a feature called Just My XAML. In Visual Studio, clicking the **Show Just My XAML** button disables the feature and shows all UI elements in the visual tree:

:::image type="content" source="media/live-visual-tree/just-my-xaml.png" alt-text="Screenshot of all UI elements in the Live Visual Tree window.":::

Just My XAML can be permanently disabled by selecting **Debug > Options > XAML Hot Reload** from the Visual Studio menu bar. Next, in the **Options** dialog box, ensure that **Enable Just My XAML in Live Visual Tree** is disabled:

:::image type="content" source="media/live-visual-tree/just-my-xaml-disabled.png" alt-text="Screenshot of Just My XAML button disabled in the Live Visual Tree window in Visual Studio.":::

## Find a UI element

The structure of a XAML UI has a lot of elements that you may not be interested in, and if you don't have a full understanding of the app's source code you might have a difficult time navigating the visual tree to find the UI element that you're looking for. Therefore, on Windows the **Live Visual Tree** window has multiple approaches that let you use the app's UI to help you find the element you want to examine:

- **Select element in the running application**. You can enable this mode by clicking the **Select Element in the Running Application** button in the **Live Visual Tree** toolbar:

  :::image type="content" source="media/live-visual-tree/select-element-in-running-app.png" alt-text="Screenshot of the Select Element in the Running Application button in the Live Visual Tree toolbar.":::

  With this mode enabled, when you can select a UI element in the app the **Live Visual Tree** window automatically updates to show the node in the tree corresponding to that element.

- **Display layout adorners in the running application**. You can enable this mode by clicking the **Display Layout Adorners in the Running Application** button in the **Live Visual Tree** toolbar:

  :::image type="content" source="media/live-visual-tree/display-adorners.png" alt-text="Screenshot of the Display Layout Adorners in the Running Application button in the Live Visual Tree toolbar.":::

  When this mode is enabled, it causes the app window to show horizontal and vertical lines along the bounds of the selected object so you can see what it aligns to, as well as rectangles showing the margins.

- **Preview Selection**. You can enable this mode by clicking the **Preview Selected Item** button in the **Live Visual Tree** toolbar:

  :::image type="content" source="media/live-visual-tree/preview-selection.png" alt-text="Screenshot of the Preview Selected Item button in the Live Visual Tree toolbar.":::

  This mode shows the XAML source code where the element was declared, provided that you have access to the app source code.
