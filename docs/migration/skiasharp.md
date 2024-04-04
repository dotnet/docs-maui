---
title: "Reuse SkiaSharp code in .NET MAUI"
description: "Learn how to reuse your SkiaSharp code from a Xamarin.Forms app in a .NET MAUI app."
ms.date: 01/09/2024
---

# Reuse SkiaSharp code in .NET MAUI

SkiaSharp is a 2D graphics system for .NET and C# that draws 2D vector graphics, bitmaps, and text. It's powered by the open-source Skia graphics engine that's used extensively in Google products. You can reuse SkiaSharp code from your Xamarin.Forms apps in your .NET Multi-platform App UI (.NET MAUI) apps with some minor updates.

To reuse your SkiaSharp code from a Xamarin.Forms app in a .NET MAUI app, you must:

> [!div class="checklist"]
>
> - Remove the Xamarin.Forms SkiaSharp NuGet packages from your project, and add the .NET MAUI SkiaSharp NuGet packages to your project.
> - Update namespaces.
> - Initialize SkiaSharp.

## Add NuGets

SkiaSharp for .NET MAUI is packaged as a series of NuGet packages. After you've migrated your Xamarin.Forms app to a .NET MAUI app, you should remove all the existing SkiaSharp NuGet packages from your app. Then, use the NuGet package manager to search for the [SkiaSharp.Views.Maui.Controls](https://www.nuget.org/packages/SkiaSharp.Views.Maui.Controls/) NuGet package and add it to your project. This will also install dependent SkiaSharp packages.

## Update namespaces

Xamarin.Forms apps that use SkiaSharp typically use types from the <xref:SkiaSharp> namespace and the <xref:SkiaSharp.Views.Forms> namespace. In SkiaSharp for .NET MAUI, you'll continue to use the <xref:SkiaSharp> namespace but the types that were in the <xref:SkiaSharp.Views.Forms> namespace have moved into the <xref:SkiaSharp.Views.Maui> and <xref:SkiaSharp.Views.Maui.Controls> namespaces.

The following table shows the namespaces you'll need to use to build your SkiaSharp code in a .NET MAUI app:

| .NET MAUI namespace | Details |
| --------- | ------- |
| <xref:SkiaSharp> | Contains all the SkiaSharp classes, structures, and enumerations. |
| <xref:SkiaSharp.Views.Maui> | Contains types to support touch interactions, and event arguments. |
| <xref:SkiaSharp.Views.Maui.Controls> | Contains the <xref:SkiaSharp.Views.Maui.Controls.SKCanvasView> class, which derives from the .NET MAUI <xref:Microsoft.Maui.Controls.View> class and hosts your SkiaSharp graphics output. Also contains different `ImageSource` classes. |
| <xref:SkiaSharp.Views.Maui.Controls.Hosting> | Contains the <xref:SkiaSharp.Views.Maui.Controls.Hosting.AppHostBuilderExtensions.UseSkiaSharp%2A> method that's used to initialize SkiaSharp in your .NET MAUI app. For more information, see [Initialize SkiaSharp](#initialize-skiasharp). |

## Initialize SkiaSharp

Initialize SkiaSharp in your app by calling the <xref:SkiaSharp.Views.Maui.Controls.Hosting.AppHostBuilderExtensions.UseSkiaSharp%2A> method on the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object in your `MauiProgram` class:

```csharp
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MyMauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        #if DEBUG
        builder.Logging.AddDebug();
        #endif

        return builder.Build();
    }
}
```

> [!NOTE]
> Calling the <xref:SkiaSharp.Views.Maui.Controls.Hosting.AppHostBuilderExtensions.UseSkiaSharp%2A> method requires you to add a `using` directive for the `SkiaSharp.Views.Maui.Controls.Hosting` namespace.
