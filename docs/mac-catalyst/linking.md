---
title: "Linking a .NET MAUI Mac Catalyst app"
description: "Learn about the .NET for Mac Catalyst linker, which is used to eliminate unused code from a .NET MAUI Mac Catalyst app in order to reduce its size."
ms.date: 04/11/2023
no-loc: [ ILLink ]
---

# Linking a .NET MAUI Mac Catalyst app

[!INCLUDE [Linker introduction and behavior](../macios/includes/linker-behavior.md)]

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

To configure linker behavior in Visual Studio for Mac:

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**.
1. In the **Project Properties** window, select the **Build > Mac Catalyst > Build** tab.
1. In the **Project Properties** window, ensure the **Configuration** drop-down is set to **Release** and set the **Linker behavior** drop-down to your desired linker behavior:

    :::image type="content" source="media/linking/vsmac.png" alt-text="Screenshot of the linker behavior for Mac Catalyst in Visual Studio for Mac.":::

1. In the **Project Properties** window, click the **OK** button.

[!INCLUDE [Control the linker](../includes/linker-control.md)]
