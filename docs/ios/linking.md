---
title: "Linking a .NET MAUI iOS app"
description: "Learn about the .NET iOS linker, which is used to eliminate unused code from a .NET MAUI iOS app in order to reduce its size."
ms.date: 04/11/2023
no-loc: [ ILLink ]
---

# Linking a .NET MAUI iOS app

[!INCLUDE [Linker introduction and behavior](../macios/includes/linker-behavior.md)]

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **iOS > Build** tab and set the **Linker behavior** drop-down to your desired linker behavior:

    :::image type="content" source="media/linking/vs.png" alt-text="Screenshot of the linker behavior for iOS in Visual Studio.":::

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**.
1. In the **Project Properties** window, select the **Build > iOS > Build** tab.
1. In the **Project Properties** window, ensure the **Configuration** drop-down is set to **Release** and set the **Linker behavior** drop-down to your desired linker behavior:

    :::image type="content" source="media/linking/vsmac.png" alt-text="Screenshot of the linker behavior for iOS in Visual Studio for Mac.":::

1. In the **Project Properties** window, click the **OK** button.

----

[!INCLUDE [Control the linker](../includes/linker-control.md)]
