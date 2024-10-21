---
title: "Linking a .NET MAUI iOS app"
description: "Learn about the .NET for iOS linker, which is used to eliminate unused code from a .NET MAUI iOS app in order to reduce its size."
ms.date: 08/27/2024
no-loc: [ ILLink ]
monikerRange: "=net-maui-8.0"
---

# Linking a .NET MAUI iOS app

[!INCLUDE [Linker introduction and behavior](../macios/includes/linker-behavior.md)]

To configure linker behavior in Visual Studio:

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **iOS > Build** tab and set the **Linker behavior** drop-down to your desired linker behavior:

    :::image type="content" source="media/linking/vs.png" alt-text="Screenshot of the linker behavior for iOS in Visual Studio.":::

[!INCLUDE [Control the linker](../includes/linker-control.md)]
