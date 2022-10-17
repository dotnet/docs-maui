---
title: "Publish a .NET MAUI app for Windows"
description: "Learn how to package and publish a Windows .NET MAUI app."
ms.date: 10/12/2022
---

# Publish a .NET MAUI app for Windows

> [!div class="op_single_selector"]
>
> - [Publish for Android](../../android/deployment/overview.md)
> - [Publish for iOS](../../ios/deployment/overview.md)
> - [Publish for macOS](../../macos/deployment/overview.md)
> - [Publish for Windows](overview.md)

When distributing your .NET Multi-platform App UI (.NET MAUI) app for Windows, you can publish the app and its dependencies to a folder for deployment to another system. Publishing a .NET MAUI app for Windows creates an MSIX app package, which has numerous benefits for the users installing your app. For more information about the benefits of MSIX, see [What is MSIX?](/windows/msix/overview).

.NET MAUI currently only allows publishing an MSIX package. You can't yet publish a Windows executable file for distribution.

## Configuration

The MSIX package is configured by the _Platforms\\Windows\\Package.appxmanifest_ (the manifest) file in your project. The manifest is used by the MSIX installer, the Microsoft store, and by Windows, to configure and display your app. .NET MAUI does use some shared settings across platforms, such as the app name and icon, which is set in the manifest at build-time. Besides those few settings, you'll need to edit the manifest to configure the app package to create a nice installer experience. The Microsoft Store has its own requirements, set in the manifest, when submitting your app.

You can use the Manifest Designer feature of Visual Studio to visually edit the _Package.appxmanifest_ file, which affects how the app is displayed in the Microsoft Store and in Windows. You can also edit the _Package.appxmanifest_ file using the XML editor.

- To use the Manifest Designer, find the **Solution Explorer** pane, then right-click **Platforms\\Windows\\Package.appxmanifest** > **Properties**.
- To use the XML editor, find the **Solution Explorer** pane, then right-click **Platforms\\Windows\\Package.appxmanifest** > **View Code**.

> [!IMPORTANT]
> The Manifest Designer for .NET MAUI projects can't edit app capabilities. For the time being, you'll need to use the XML editor.

For more information on specific app manifest settings, see [App manifest schema reference](/uwp/schemas/appxpackage/uapmanifestschema/root-elements).

## Publish your app

.NET MAUI can use Visual Studio for publishing, but also supports publishing through the `dotnet` command-line interface (CLI) for Continuous Integration (CI) scenarios.

<!-- - [Publish an app to the Microsoft Store](publish-visual-studio-store.md)-->
- [Publish to a folder with Visual Studio](publish-visual-studio-folder.md)
- [Publish to a folder with the CLI](publish-cli.md)
