---
ms.topic: include
ms.date: 09/19/2025
---

> [!WARNING]
> On Windows, apps using WebView2-based controls that are installed to the `Program Files` directory may fail to render content properly. This occurs because WebView2 attempts to write its cache and user data files to the app's installation directory, which has restricted write permissions in `Program Files`. To resolve this issue, set the `WEBVIEW2_USER_DATA_FOLDER` environment variable before any WebView control is initialized:
>
> ```csharp
> #if WINDOWS
> var userDataFolder = Path.Combine(FileSystem.AppDataDirectory, "WebView2");
> Environment.SetEnvironmentVariable("WEBVIEW2_USER_DATA_FOLDER", userDataFolder);
> #endif
> ```
>
> Place this code in your `App.xaml.cs` constructor or in `Platforms\Windows\App.xaml.cs` before any WebView control is created. This directs WebView2 to use a writable location in the user's AppData directory instead of the restricted Program Files location.
