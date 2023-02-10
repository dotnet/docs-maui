---
title: "Use effects in .NET MAUI"
description: "Learn how to adapt Xamarin.Forms effects to work in a .NET MAUI app."
ms.date: 2/10/2023
---

# Use effects in .NET MAUI

While there are many benefits to using .NET Multi-platform App UI (.NET MAUI) handlers to customize controls, it's still possible to use Xamarin.Forms effects in .NET MAUI apps. For more information about effects, see [Xamarin.Forms effects](/xamarin/xamarin-forms/app-fundamentals/effects/).

The process for migrating a Xamarin.Forms effect to .NET MAUI is to:

1. Remove the effect attributes from your effect class. For more information, see [Remove the effect attributes](#remove-the-effect-attributes).
1. Modify the effect `using` directives. For more information, see [Modify using directives](#modify-using-directives).
1. Add the effect code into the appropriate location in your .NET MAUI app project. For more information, see [Add the effect code](#add-the-effect-code).
1. Register the effect. For more information, see [Register the effect](#register-the-effect).
1. Consume your .NET MAUI effect. For more information, see [Consume the effect](#consume-the-effect).

## Remove the effect attributes

Any <xref:Xamarin.Forms.ResolutionGroupNameAttribute> and <xref:Xamarin.Forms.ExportEffectAttribute> attributes should be removed from your effect classes.

## Modify using directives

Any references to the `Xamarin.Forms` and `Xamarin.Forms.Platform.*` namespaces should be removed from your effect classes.

In addition, you should add a `using` directive to your effect class for the `Microsoft.Maui.Controls.Platform` namespace.

## Add the effect code

If you're using a .NET MAUI multi-targeted project, your effect code should be combined into a single file and placed anywhere outside the *Platforms** folder. This requires you to combine your <xref:Microsoft.Maui.Controls.RoutingEffect> implementation and <xref:Microsoft.Maui.Controls.Platform.PlatformEffect> implementations into a single file, using conditional compilation around platform code. However, if your solution has separate projects per-platform, then you should move the platform-specific effect files into the corresponding projects.

In .NET MAUI, the <xref:Microsoft.Maui.Controls.RoutingEffect> class is in the `Microsoft.Maui.Controls` namespace. This namespace is one of .NET MAUI's implicit `global using` directives, and so you don't need to add a `using` directive for it. However, the <xref:Microsoft.Maui.Controls.Platform.PlatformEffect> class is in the `Microsoft.Maui.Controls.Platform` namespace, for which you must add a `using` directive.

The following code example shows a `FocusRoutingEffect` class, and it's platform implementations, combined into a single file:

```csharp
using Microsoft.Maui.Controls.Platform;

namespace MyMauiApp.Effects;

internal class FocusRoutingEffect : RoutingEffect
{
}

#if ANDROID
internal class FocusPlatformEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        // Customize the control here
    }

    protected override void OnDetached()
    {
        // Cleanup the control customization here
    }
}
#elif IOS
internal class FocusPlatformEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        // Customize the control here
    }

    protected override void OnDetached()
    {
        // Cleanup the control customization here
    }
}
#elif WINDOWS
internal class FocusPlatformEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        // Customize the control here
    }

    protected override void OnDetached()
    {
        // Cleanup the control customization here
    }
}
#endif
```

## Register the effect

In your .NET MAUI app project, open *MauiProgram.cs* and call <xref:Microsoft.Maui.Controls.Hosting.AppHostBuilderExtensions.ConfiureEffects%2A> on the `MauiAppBuilder` object in the `CreateMauiApp` method:

```csharp
public static MauiApp CreateMauiApp()
{
  var builder = MauiApp.CreateBuilder();
  builder
    .UseMauiApp<App>()
    .ConfigureFonts(fonts =>
    {
      fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
      fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    })
    .ConfigureEffects(effects =>
    {
      effects.Add<FocusRoutingEffect, FocusPlatformEffect>();
    });

  return builder.Build();
}
```

The effect is registered with <xref:Microsoft.Maui.Controls.Hosting.AppHostBuilderExtensions.ConfiureEffects%2A>, whose `configureDelegate` registers the <xref:Microsoft.Maui.Controls.Platform.PlatformEffect> implementation against its <xref:Microsoft.Maui.Controls.RoutingEffect> implementation.

## Consume the effect

The effect can then be consumed in a .NET MAUI app by adding it to the <xref:Microsoft.Maui.Controls.Element.Effects> collection of a control:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyMauiApp.Effects"
             x:Class="MyMauiApp.MainPage">
    <VerticalStackLayout>
        <Entry Text="Enter your text">
            <Entry.Effects>
                <local:FocusRoutingEffect />
            </Entry.Effects>
        </Entry>
    </VerticalStackLayout>
</ContentPage>
```
