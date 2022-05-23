---
title: "Default image directory on Windows"
description: "This article explains how to consume the .NET MAUI Windows platform-specific that defines the directory in the project that image assets will be loaded from."
ms.date: 04/06/2022
---

# Default image directory on Windows

This .NET Multi-platform App UI (.NET MAUI) Windows platform-specific defines the directory in the project that image assets will be loaded from. It's consumed in XAML by setting the `Application.ImageDirectory` to a `string` that represents the project directory that contains image assets:

```xaml
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls"
             ...
             windows:Application.ImageDirectory="Assets">
    ...
</Application>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...
Application.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetImageDirectory("Assets");
```

The `Application.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `Application.SetImageDirectory` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, is used to specify the project directory that images will be loaded from. In addition, the `GetImageDirectory` method can be used to return a `string` that represents the project directory that contains the app image assets.

The result is that all images used in an app will be loaded from the specified project directory.
