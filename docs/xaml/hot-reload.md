---
title: "XAML Hot Reload for .NET MAUI"
description: "Learn how to reload changes to your .NET MAUI XAML file instantly on your running app, so you don't have to rebuild your .NET MAUI project after every XAML change."
ms.date: 06/23/2022
---

# XAML Hot Reload for .NET MAUI

.NET Multi-platform App UI (.NET MAUI) XAML Hot Reload is a Visual Studio feature that enables you to view the result of XAML changes in your running app, without having to rebuild your project. Without XAML Hot Reload, you have to build and deploy your app every time you want to view the result of a XAML change.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

When your .NET MAUI app is running in debug configuration, with the debugger attached, XAML Hot Reload parses your XAML edits and sends those changes to the running app. It preserves your UI state, since it doesn't recreate the UI for the full page, and updates changed properties on controls affected by edits. In addition, your navigate state and data will be maintained, enabling you to quickly iterate on your UI without losing your location in the app. Therefore, you'll spend less time rebuilding and deploying your apps to validate UI changes.

By default, you don't need to save your XAML file to see the results of your edits. Instead, updates are applied immediately as you type. However, you can change this behavior to update only on file save. This can be accomplished by checking the **Apply XAML Hot Reload on document save** checkbox in the Hot Reload IDE settings available by selecting **Debug > Options> XAML Hot Reload** from the Visual Studio menu bar. Only updating on file save can sometimes be useful if you make bigger XAML updates and don't wish them to be displayed until they are complete.

<!-- XAML Hot Reload supports simultaneous debugging of multiple platforms in Visual Studio. You can deploy an Android, iOS, and WinUI target at the same time to see your changes reflected on all three platforms at once. To debug on multiple platforms, see [How To: Set multiple startup projects](/visualstudio/ide/how-to-set-multiple-startup-projects). -->

<!-- The above is commented out for now for the following reason: In MAUI if you have separate head projects, you could deploy to multiple platforms simultaneously and hot reload would work. But deploying to multiple platforms currently isn't possible with single project. -->

<!-- **Mac** [Set multiple startup projects](/visualstudio/mac/set-startup-projects?view=vsmac-2019) -->

> [!NOTE]
> If you're writing a native UWP or WPF app, without using .NET MAUI, see [What is XAML Hot Reload for WPF and UWP apps?](/visualstudio/xaml-tools/xaml-hot-reload).

<!-- XAML Hot Reload is available in both Visual Studio 2022 and Visual Studio 2022 for Mac. -->

On Windows, XAML Hot Reload is available on Android, iOS, and WinUI on emulators, simulators, and physical devices.

> [!IMPORTANT]
> XAML Hot Reload doesn't reload C# code, including event handlers.

<!-- XAML Hot Reload does work in collaboration with C# Hot Reload, but there's no official doc to link to, to explain the limitations. -->

## Enable XAML Hot Reload

XAML Hot Reload is enabled by default in Visual Studio 2022. If it's been previously disabled, it can be enabled by selecting **Debug > Options> XAML Hot Reload** from the Visual Studio menu bar. Next, in the **Options** dialog box, ensure that the **Enable XAML Hot Reload**, **WinUI (including .NET MAUI)**, and **Android and iOS (.NET MAUI)** options are checked:

:::image type="content" source="media/hot-reload/options.png" alt-text="XAML Hot Reload options for .NET MAUI.":::

<!-- On Mac, check the **Enable Xamarin Hot Reload** checkbox at **Visual Studio** > **Preferences** > **Tools for Xamarin** > **XAML Hot Reload**. -->

Then, on iOS in your build settings, check that the Linker is set to "Don't Link".

## Troubleshooting

The XAML Hot Reload output displays status messages that can help with troubleshooting. These can be displayed by selecting **View > Output** from the Visual Studio menu bar, and then selecting **Xamarin Hot Reload** in the **Show output from:** drop-down.
<!-- **Mac**: hover over **XAML Hot Reload** in the status bar to show that pad -->

If XAML Hot Reload fails to initialize you should ensure that you're using the latest version of .NET MAUI, the latest version of the IDE, and that your iOS linker settings are set to **Don't Link** in the project's build settings.

If nothing happens when saving your XAML file, ensure that XAML Hot Reload is enabled in the IDE. For more information, see [Enable XAML Hot Reload](#enable-xaml-hot-reload).

If you make a change that the XAML Hot Reload parser sees as invalid, it will show the error underlined in the editor and include it in the **Error List** window. Hot Reload errors have an error code starting with "XHR" (for XAML Hot Reload). If there are any such errors on the page, XAML Hot Reload won't apply changes to your running app until the errors have been fixed.

You can't add, remove, or rename files or NuGet packages during a XAML Hot Reload session. If you add or remove a file or NuGet package, rebuild and redeploy your app to continue using XAML Hot Reload.

Disabling XAML compilation with `[XamlCompilation(XamlCompilationOptions.Skip)]` isn't supported and can cause issues with the Live Visual Tree. For more information about Live Visual Tree, see [Inspect the visual tree of a .NET MAUI app](~/user-interface/live-visual-tree.md).
