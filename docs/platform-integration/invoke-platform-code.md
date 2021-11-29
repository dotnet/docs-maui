---
title: ".NET MAUI invoking platform code"
description: "Learn how to invoke platform code in a .NET MAUI app, by combining multi-targeting with partial classes and partial methods."
ms.date: 11/29/2021
---

# Invoke platform code

.NET Multi-platform App UI (.NET MAUI) apps can combine multi-targeting with partial classes and partial methods to invoke platform code from cross-platform code.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

In a .NET MAUI app, the app project contains a **Platforms** folder:

:::image type="content" source="media/invoke-platform-code/platform-folders.png" alt-text="Platform folders screenshot.":::

Each child of the **Platforms** folder represents a platform that .NET MAUI can target, and each child folder contains platform-specific code. At build time, the compiler only includes the code from each folder when building for that specific platform. For example, when you build for Android the files in the **Platforms** > **Android** folder will be built into the app package, but the files in the other **Platform** folders won't be. This approach uses [multi-targeting](/dotnet/standard/library-guidance/cross-platform-targeting#multi-targeting) to combine partial classes and partial methods to invoke native platform functionality from cross-platform code. Alternatively, multi-targeting can be performed based on your own filename and folder criteria, rather than using the **Platforms** folders. For more information, see [Configure multi-targeting](#configure-multi-targeting).

The process for invoking platform code from cross-platform code is to:

1. Define the cross-platform API as a partial class that defines partial method signatures for any operations you want to invoke on each platform. For more information, see [Define the cross-platform API](#define-the-cross-platform-api).
1. Implement the cross-platform API per platform, by defining the same partial class and the same partial method signatures, but also provide the method implementations. For more information, see [Implement the API per platform](#implement-the-api-per-platform).
1. Invoke the cross-platform API by creating an instance of the partial class and invoking its methods as required. For more information, see [Invoke the cross-platform API](#invoke-the-cross-platform-api).

## Define the cross-platform API

The first step in being able to invoke platform code from cross-platform code is to define the cross-platform API as a [partial class](/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods#partial-classes) that defines [partial method](/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods#partial-classes) signatures for any operations you want to invoke on each platform.

The following example shows a cross-platform API that can be used to retrieve the orientation of a device:

```csharp
namespace CallPlatformCodeDemos.Services
{
    public enum DeviceOrientation
    {
        Undefined,
        Landscape,
        Portrait
    }

    public partial class DeviceOrientationService
    {
        public partial DeviceOrientation GetOrientation();
    }
}
```

## Implement the API per platform

After defining the cross-platform API, it must be implemented on your required platforms by defining the same partial class and the same partial method signatures, but also providing the method implementations.

The following table lists the default folder locations for platform implementations:

| Platform | Folder |
| -------- | ------ |
| Android | **Platforms** > **Android** |
| iOS | **Platforms** > **iOS** |
| MacCatalyst | **Platforms** > **MacCatalyst** |
| Windows | **Platforms** > **Windows** |

> [!IMPORTANT]
> Platform implementations must occur in the same namespace and same class that the cross-platform API was defined in.

Alternatively, multi-targeting can be performed based on your own filename and folder criteria, rather than using the **Platforms** folders. For more information, see [Configure multi-targeting](#configure-multi-targeting).

The following screenshot shows the `DeviceOrientationService` classes in the Android and iOS **Platform** folders:

:::image type="content" source="media/invoke-platform-code/implement-api.png" alt-text="DeviceOrientationService classes in their Platforms folder screenshot.":::

### Android

The following example shows the implementation of the `GetOrientation` method on Android:

```csharp
using Android.Content;
using Android.Runtime;
using Android.Views;

namespace CallPlatformCodeDemos.Services
{
    public partial class DeviceOrientationService
    {
        public partial DeviceOrientation GetOrientation()
        {
            IWindowManager windowManager = Android.App.Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            SurfaceOrientation orientation = windowManager.DefaultDisplay.Rotation;
            bool isLandscape = orientation == SurfaceOrientation.Rotation90 || orientation == SurfaceOrientation.Rotation270;
            return isLandscape ? DeviceOrientation.Landscape : DeviceOrientation.Portrait;
        }
    }
}
```

### iOS

The following example shows the implementation of the `GetOrientation` method on iOS:

```csharp
using UIKit;

namespace CallPlatformCodeDemos.Services
{
    public partial class DeviceOrientationService
    {
        public partial DeviceOrientation GetOrientation()
        {
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
            bool isPortrait = orientation == UIInterfaceOrientation.Portrait || orientation == UIInterfaceOrientation.PortraitUpsideDown;
            return isPortrait ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
        }
    }
}
```

## Invoke the cross-platform API

After providing the platform implementations, the cross-platform API can be invoked by creating an object and invoking its operation:

```csharp
using CallPlatformCodeDemos.Services;
using Microsoft.Maui.Controls;
...

DeviceOrientationService deviceOrientationService = new DeviceOrientationService();
DeviceOrientation orientation = deviceOrientationService.GetOrientation();
```

At build time, the compiler will combine the cross-platform partial class with the partial class for the target platform and build it into the app package.

## Configure multi-targeting

.NET MAUI apps can also be multi-targeted based on your own filename and folder criteria. This enables you to structure your .NET MAUI app project so that you don't have to place your platform code into sub-folders of the **Platforms** folder.

For example, a standard multi-targeting pattern is to include the platform in the filename containing the platform code. The build system can configured to use this pattern to combine cross-platform partial classes with platform partial classes that include the platform's name:

:::image type="content" source="media/invoke-platform-code/multi-target-filenames.png" alt-text="DeviceOrientationService classes using filename-based multi-targeting.":::

Another standard multi-targeting pattern is to include the platform in a folder name. The build system can then be configured to use this pattern to combine cross-platform partial classes with platform partial classes that are located in specific folders:

:::image type="content" source="media/invoke-platform-code/multi-target-folders.png" alt-text="DeviceOrientationService classes using folder-based multi-targeting.":::

<!-- For more information, see [Configure multi-targeting](configure-multi-targeting.md).-->
