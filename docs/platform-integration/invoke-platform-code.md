---
title: ".NET MAUI invoking platform code"
description: "Learn how to invoke platform code in a .NET MAUI app by using conditional compilation, or by using partial classes and partial methods."
ms.date: 06/28/2022
---

# Invoke platform code

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-invokeplatformcode)

In situations where .NET Multi-platform App UI (.NET MAUI) doesn't provide any APIs for accessing specific platform APIs, you can write your own code to access the required platform APIs. This requires knowledge of [Apple's iOS and MacCatalyst APIs](https://developer.apple.com/documentation/technologies?language=objc), [Google's Android APIs](https://developer.android.com/reference), and [Microsoft's Windows App SDK APIs](/windows/windows-app-sdk/api/winrt).

Platform code can be invoked from cross-platform code by using conditional compilation, or by using partial classes and partial methods.

## Conditional compilation

Platform code can be invoked from cross-platform code by using conditional compilation to target different platforms.

The following example shows the `DeviceOrientation` enumeration, which will be used to specify the orientation of your device:

```csharp
namespace InvokePlatformCodeDemos.Services
{
    public enum DeviceOrientation
    {
        Undefined,
        Landscape,
        Portrait
    }
}
```

Retrieving the orientation of your device requires writing platform code. This can be accomplished by writing a method that uses conditional compilation to target different platforms:

```csharp
#if ANDROID
using Android.Content;
using Android.Views;
using Android.Runtime;
#elif IOS
using UIKit;
#endif

using InvokePlatformCodeDemos.Services;

namespace InvokePlatformCodeDemos.Services.ConditionalCompilation
{
    public class DeviceOrientationService
    {
        public DeviceOrientation GetOrientation()
        {
#if ANDROID
            IWindowManager windowManager = Android.App.Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            SurfaceOrientation orientation = windowManager.DefaultDisplay.Rotation;
            bool isLandscape = orientation == SurfaceOrientation.Rotation90 || orientation == SurfaceOrientation.Rotation270;
            return isLandscape ? DeviceOrientation.Landscape : DeviceOrientation.Portrait;
#elif IOS
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
            bool isPortrait = orientation == UIInterfaceOrientation.Portrait || orientation == UIInterfaceOrientation.PortraitUpsideDown;
            return isPortrait ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
#else
            return DeviceOrientation.Undefined;
#endif
        }
    }
}
```

In this example, platform implementations of the `GetOrientation` method are provided for Android and iOS. On other platforms, `DeviceOrientation.Undefined` is returned. Alternatively, rather than returning `DeviceOrientation.Undefined` you could throw a `PlatformNotSupportedException` that specifies the platforms that implementations are provided for:

```csharp
throw new PlatformNotSupportedException("GetOrientation is only supported on Android and iOS.");
```

The `DeviceOrientationService.GetOrientation` method can then be invoked from cross-platform code by creating an object instance and invoking its operation:

```csharp
using InvokePlatformCodeDemos.Services;
using InvokePlatformCodeDemos.Services.ConditionalCompilation;
...

DeviceOrientationService deviceOrientationService = new DeviceOrientationService();
DeviceOrientation orientation = deviceOrientationService.GetOrientation();
```

At build time the build system uses conditional compilation to target Android and iOS platform code to the correct platform.

For more information about conditional compilation, see [Conditional compilation](/dotnet/csharp/language-reference/preprocessor-directives#conditional-compilation).

## Partial classes and methods

A .NET MAUI app project contains a _Platforms_ folder, with each child folder representing a platform that .NET MAUI can target:

:::image type="content" source="media/invoke-platform-code/platform-folders.png" alt-text="Platform folders screenshot.":::

The folders for each target platform contain platform-specific code that starts the app on each platform, plus any additional platform code you add. At build time, the build system only includes the code from each folder when building for that specific platform. For example, when you build for Android the files in the _Platforms_ > _Android_ folder will be built into the app package, but the files in the other _Platforms_ folders won't be. This approach uses a feature called multi-targeting to target multiple platforms from a single project.

Multi-targeting can be combined with partial classes and partial methods to invoke platform functionality from cross-platform code. The process for doing this is to:

1. Define the cross-platform API as a partial class that defines partial method signatures for any operations you want to invoke on each platform. For more information, see [Define the cross-platform API](#define-the-cross-platform-api).
1. Implement the cross-platform API per platform, by defining the same partial class and the same partial method signatures, while also providing the method implementations. For more information, see [Implement the API per platform](#implement-the-api-per-platform).
1. Invoke the cross-platform API by creating an instance of the partial class and invoking its methods as required. For more information, see [Invoke the cross-platform API](#invoke-the-cross-platform-api).

### Define the cross-platform API

To invoke platform code from cross-platform code, the first step is to define the cross-platform API as a [partial class](/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods#partial-classes) that defines [partial method](/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods#partial-classes) signatures for any operations you want to invoke on each platform.

The following example shows the `DeviceOrientation` enumeration, which will be used to specify the orientation of your device:

```csharp
namespace InvokePlatformCodeDemos.Services
{
    public enum DeviceOrientation
    {
        Undefined,
        Landscape,
        Portrait
    }
}
```

The following example shows a cross-platform API that can be used to retrieve the orientation of a device:

```csharp
namespace InvokePlatformCodeDemos.Services.PartialMethods
{
    public partial class DeviceOrientationService
    {
        public partial DeviceOrientation GetOrientation();
    }
}
```

The partial class is named `DeviceOrientationService`, which includes a partial method named `GetOrientation`. The code file for this class must be outside of the _Platforms_ folder:

:::image type="content" source="media/invoke-platform-code/create-api.png" alt-text="DeviceOrientationService class in the Services folder screenshot.":::

### Implement the API per platform

After defining the cross-platform API, it must be implemented on all platforms you're targeting by defining the same partial class and the same partial method signatures, while also providing the method implementations.

Platform implementations should be placed in the correct _Platforms_ child folders to ensure that the build system only attempts to build platform code when building for the specific platform. The following table lists the default folder locations for platform implementations:

| Platform | Folder |
| -------- | ------ |
| Android | _Platforms_ > _Android_ |
| iOS | _Platforms_ > _iOS_ |
| MacCatalyst | _Platforms_ > _MacCatalyst_ |
| Tizen | _Platforms_ > _Tizen_ |
| Windows | _Platforms_ > _Windows_ |

> [!IMPORTANT]
> Platform implementations must be in the same namespace and same class that the cross-platform API was defined in.

The following screenshot shows the `DeviceOrientationService` classes in the _Android_ and _iOS_ folders:

:::image type="content" source="media/invoke-platform-code/implement-api.png" alt-text="DeviceOrientationService classes in their Platforms folder screenshot.":::

Alternatively, multi-targeting can be performed based on your own filename and folder criteria, rather than using the _Platforms_ folders. For more information, see [Configure multi-targeting](#configure-multi-targeting).

#### Android

The following example shows the implementation of the `GetOrientation` method on Android:

```csharp
using Android.Content;
using Android.Runtime;
using Android.Views;

namespace InvokePlatformCodeDemos.Services.PartialMethods;

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
```

#### iOS

The following example shows the implementation of the `GetOrientation` method on iOS:

```csharp
using UIKit;

namespace InvokePlatformCodeDemos.Services.PartialMethods;

public partial class DeviceOrientationService
{
    public partial DeviceOrientation GetOrientation()
    {
        UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
        bool isPortrait = orientation == UIInterfaceOrientation.Portrait || orientation == UIInterfaceOrientation.PortraitUpsideDown;
        return isPortrait ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
    }
}
```

### Invoke the cross-platform API

After providing the platform implementations, the API can be invoked from cross-platform code by creating an object instance and invoking its operation:

```csharp
using InvokePlatformCodeDemos.Services;
using InvokePlatformCodeDemos.Services.PartialMethods;
...

DeviceOrientationService deviceOrientationService = new DeviceOrientationService();
DeviceOrientation orientation = deviceOrientationService.GetOrientation();
```

At build time the build system will use multi-targeting to combine the cross-platform partial class with the partial class for the target platform, and build it into the app package.

### Configure multi-targeting

.NET MAUI apps can also be multi-targeted based on your own filename and folder criteria. This enables you to structure your .NET MAUI app project so that you don't have to place your platform code into child folders of the _Platforms_ folder.

For example, a standard multi-targeting pattern is to include the platform as an extension in the filename for the platform code. The build system can be configured to combine cross-platform partial classes with platform partial classes based on this pattern:

:::image type="content" source="media/invoke-platform-code/multi-target-filenames.png" alt-text="DeviceOrientationService classes using filename-based multi-targeting.":::

Another standard multi-targeting pattern is to include the platform as a folder name. The build system can be configured to combine cross-platform partial classes with platform partial classes based on this pattern:

:::image type="content" source="media/invoke-platform-code/multi-target-folders.png" alt-text="DeviceOrientationService classes using folder-based multi-targeting.":::

For more information, see [Configure multi-targeting](configure-multi-targeting.md).
