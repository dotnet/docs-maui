---
title: "Linking a .NET MAUI Android app"
description: "Learn about the .NET for Android linker, which is used to eliminate unused code from a .NET MAUI Android app in order to reduce its size."
ms.date: 04/23/2023
no-loc: [ ILLink ]
---

# Linking a .NET MAUI Android app

When it builds your app, .NET Multi-platform App UI (.NET MAUI) can use a linker called *ILLink* to reduce the overall size of the app. ILLink reduces the size by analyzing the intermediate code produced by the compiler. It removes unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

## Linker behavior

The linker enables you to trim your .NET MAUI Android apps. When trimming is enabled, the linker leaves your assemblies untouched and reduces the size of the SDK assemblies by removing types and members that your app doesn't use.

Linker behavior can be configured for each build configuration of your app. By default, trimming is disabled for debug builds and enabled for release builds.

> [!WARNING]
> Enabling the linker for your app's debug configuration may hinder your debugging experience, as it may remove property accessors that enable you to inspect the state of your objects.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Android > Options** tab and ensure that trimming is enabled for the release build configuration:

    :::image type="content" source="media/linking/vs.png" alt-text="Screenshot of the linker behavior for Android in Visual Studio.":::

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**.
1. In the **Project Properties** window, select the **Build > Android > Linker** tab.
1. In the **Project Properties** window, ensure the **Configuration** drop-down is set to **Release** and set the **Linker Behavior** drop-down to your desired linker behavior:

    - *Don't link*. Disabling linking ensures assemblies aren't modified.
    - *Link SDK assemblies only*. In this mode, the linker leaves your assemblies untouched and reduces the size of the SDK assemblies by removing types and members that your app doesn't use.
    - *Link all assemblies*. When it links all assemblies, the linker performs additional optimizations to make your app as small as possible. It modifies the intermediate code for your source code, which may break your app if you use features using an approach that the linker's static analysis can't detect. In these cases, you may need to make adjustments to your source code to make your app work correctly.

    :::image type="content" source="media/linking/vsmac.png" alt-text="Screenshot of the linker behavior for Android in Visual Studio for Mac.":::

    > [!WARNING]
    > Linking all assemblies isn't recommended, and can require significant additional work to ensure that your app still works.

1. In the **Project Properties** window, click the **OK** button.

> [!NOTE]
> Visual Studio for Mac sets the `AndroidLinkMode` MSBuild property in your app's project file. `<AndroidLinkMode>None</AndroidLinkMode>` maps to `<PublishTrimmed>false</PublishTrimmed>` and `<AndroidLinkMode>SdkOnly</AndroidLinkMode>` maps to `<PublishTrimmed>true</PublishTrimmed>`.

----

[!INCLUDE [Control the linker](../includes/linker-control.md)]
