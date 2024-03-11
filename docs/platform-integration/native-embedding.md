---
title: "Native embedding"
description: "Learn how to consume .NET MAUI controls inside .NET iOS, .NET Android, and WinUI native apps."
ms.date: 03/11/2024
zone_pivot_groups: devices-deployment
---

# Native embedding

Typically, a .NET Multi-platform App UI (.NET MAUI) app includes pages that contain layouts, such as <xref:Microsoft.Maui.Controls.Grid>, and layouts that contain views, such as <xref:Microsoft.Maui.Controls.Button>. Pages, layouts, and views all derive from <xref:Microsoft.Maui.Controls.Element>. Native embedding enables any .NET MAUI controls that derive from <xref:Microsoft.Maui.Controls.Element> to be consumed in .NET Android, .NET iOS, .NET Mac Catalyst, and WinUI native apps.

The process for consuming a .NET MAUI control in a native app is as follows:

1. Create extension methods to bootstrap your native embedded app. For more information, see [Create extension methods](#create-extension-methods).
1. Enable .NET MAUI support by adding `<UseMaui>true</UseMaui>` to the native app's project file. For more information, see [Enable .NET MAUI support](#enable-net-maui-support).
1. Initialize .NET MAUI by calling the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding%2A> method. For more information, see [Initialize .NET MAUI](#initialize-net-maui).
1. Add your .NET MAUI code, such as code for a page, and any dependencies to a .NET MAUI single project. For more information, see [Add .NET MAUI views](#add-net-maui-views).
1. Create an instance of the .NET MAUI control and convert it to the appropriate native type with the `ToPlatform` extension method. For more information, see [Consume .NET MAUI controls](#consume-net-maui-controls).

> [!NOTE]
> When using native embedding, .NET MAUI's data binding engine still works. However, page navigation must be performed using the native navigation API.

## Create extension methods

Before creating a native app that consumes .NET MAUI controls, you should first create a .NET MAUI class library project and delete the **Platforms** folder and the `Class1` class from it. Then, add a class to it named `EmbeddedExtensions` that contains the following code:

```csharp
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui.Platform;

#if ANDROID
using PlatformView = Android.Views.View;
using PlatformWindow = Android.App.Activity;
using PlatformApplication = Android.App.Application;
#elif IOS || MACCATALYST
using PlatformView = UIKit.UIView;
using PlatformWindow = UIKit.UIWindow;
using PlatformApplication = UIKit.IUIApplicationDelegate;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using PlatformWindow = Microsoft.UI.Xaml.Window;
using PlatformApplication = Microsoft.UI.Xaml.Application;
#endif

namespace Microsoft.Maui.Controls;

public static class EmbeddedExtensions
{
    public static MauiAppBuilder UseMauiEmbedding(this MauiAppBuilder builder, PlatformApplication? platformApplication = null)
    {
#if ANDROID
        platformApplication ??= (Android.App.Application)Android.App.Application.Context;
#elif IOS || MACCATALYST
        platformApplication ??= UIKit.UIApplication.SharedApplication.Delegate;
#elif WINDOWS
        platformApplication ??= Microsoft.UI.Xaml.Application.Current;
#endif

        builder.Services.AddSingleton(platformApplication);
        builder.Services.AddSingleton<EmbeddedPlatformApplication>();
        builder.Services.AddScoped<EmbeddedWindowProvider>();

        // Returning null is acceptable here as the platform window is optional - but we don't know until we resolve it
        builder.Services.AddScoped<PlatformWindow>(svc => svc.GetRequiredService<EmbeddedWindowProvider>().PlatformWindow!);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IMauiInitializeService, EmbeddedInitializeService>());
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(Window), typeof(EmbeddedWindowHandler));
        });

        return builder;
    }

    public static IMauiContext CreateEmbeddedWindowContext(this MauiApp mauiApp, PlatformWindow platformWindow, Window? window = null)
    {
        var windowScope = mauiApp.Services.CreateScope();

#if ANDROID
        var windowContext = new MauiContext(windowScope.ServiceProvider, platformWindow);
#else
        var windowContext = new MauiContext(windowScope.ServiceProvider);
#endif

        window ??= new Window();

        var wndProvider = windowContext.Services.GetRequiredService<EmbeddedWindowProvider>();
        wndProvider.SetWindow(platformWindow, window);
        window.ToHandler(windowContext);

        return windowContext;
    }

    public static PlatformView ToPlatformEmbedded(this IElement element, IMauiContext context)
    {
        var wndProvider = context.Services.GetService<EmbeddedWindowProvider>();
        if (wndProvider is not null && wndProvider.Window is Window wnd && element is VisualElement visual)
            wnd.AddLogicalChild(visual);

        return element.ToPlatform(context);
    }

    private class EmbeddedInitializeService : IMauiInitializeService
    {
        public void Initialize(IServiceProvider services) =>
            services.GetRequiredService<EmbeddedPlatformApplication>();
    }
}
```

These extension methods are in the `Microsoft.Maui.Controls` namespace, and are used to bootstrap your native embedded app on each platform. The extension methods reference `EmbeddedPlatformApplication`, `EmbeddedWindowHandler`, and `EmbeddedWindowProvider` types that you must also add to the .NET MAUI library project.

The following code shows the `EmbeddedPlatformApplication` class, which should be added to the same .NET MAUI library project as the `EmbeddedExtensions` class:

```csharp
#if ANDROID
using PlatformApplication = Android.App.Application;
#elif IOS || MACCATALYST
using PlatformApplication = UIKit.IUIApplicationDelegate;
#elif WINDOWS
using PlatformApplication = Microsoft.UI.Xaml.Application;
#endif

namespace Microsoft.Maui.Controls;

internal class EmbeddedPlatformApplication : IPlatformApplication
{
    private readonly MauiContext rootContext;
    private readonly IMauiContext applicationContext;

    public IServiceProvider Services { get; }
    public IApplication Application { get; }

    public EmbeddedPlatformApplication(IServiceProvider services)
    {
        IPlatformApplication.Current = this;

#if ANDROID
        var platformApp = services.GetRequiredService<PlatformApplication>();
        rootContext = new MauiContext(services, platformApp);
#else
        rootContext = new MauiContext(services);
#endif

        applicationContext = MakeApplicationScope(rootContext);
        Services = applicationContext.Services;
        Application = Services.GetRequiredService<IApplication>();
    }

    private static IMauiContext MakeApplicationScope(IMauiContext rootContext)
    {
        var scopedContext = new MauiContext(rootContext.Services);
        InitializeScopedServices(scopedContext);
        return scopedContext;
    }

    private static void InitializeScopedServices(IMauiContext scopedContext)
    {
        var scopedServices = scopedContext.Services.GetServices<IMauiInitializeScopedService>();

        foreach (var service in scopedServices)
            service.Initialize(scopedContext.Services);
    }
}
```

The following code shows the `EmbeddedWindowHandler` class, which should be added to the same .NET MAUI library project as the `EmbeddedExtensions` class:

```csharp
using Microsoft.Maui.Handlers;

#if ANDROID
using PlatformWindow = Android.App.Activity;
#elif IOS || MACCATALYST
using PlatformWindow = UIKit.UIWindow;
#elif WINDOWS
using PlatformWindow = Microsoft.UI.Xaml.Window;
#endif

namespace Microsoft.Maui.Controls;

internal class EmbeddedWindowHandler : ElementHandler<IWindow, PlatformWindow>, IWindowHandler
{
    public static IPropertyMapper<IWindow, IWindowHandler> Mapper =
        new PropertyMapper<IWindow, IWindowHandler>(ElementHandler.ElementMapper)
        {
        };

    public static CommandMapper<IWindow, IWindowHandler> CommandMapper =
        new CommandMapper<IWindow, IWindowHandler>(ElementHandler.ElementCommandMapper)
        {
        };

    public EmbeddedWindowHandler() : base(Mapper)
    {
    }

    protected override PlatformWindow CreatePlatformElement() =>
        MauiContext!.Services.GetRequiredService<PlatformWindow>() ??
        throw new InvalidOperationException("EmbeddedWindowHandler could not locate a platform window.");
}
```

The following code shows the `EmbeddedWindowProvider` class, which should be added to the same .NET MAUI library project as the `EmbeddedExtensions` class:

```csharp
#if ANDROID
using PlatformWindow = Android.App.Activity;
#elif IOS || MACCATALYST
using PlatformWindow = UIKit.UIWindow;
#elif WINDOWS
using PlatformWindow = Microsoft.UI.Xaml.Window;
#endif

namespace Microsoft.Maui.Controls;

public class EmbeddedWindowProvider
{
    WeakReference<PlatformWindow?>? platformWindow;
    WeakReference<Window?>? window;

    public PlatformWindow? PlatformWindow => Get(platformWindow);
    public Window? Window => Get(window);

    public void SetWindow(PlatformWindow? platformWindow, Window? window)
    {
        this.platformWindow = new WeakReference<PlatformWindow?>(platformWindow);
        this.window = new WeakReference<Window?>(window);
    }

    private static T? Get<T>(WeakReference<T?>? weak) where T : class =>
        weak is not null && weak.TryGetTarget(out var target) ? target : null;
}
```

## Enable .NET MAUI support

To consume .NET MAUI controls that derive from <xref:Microsoft.Maui.Controls.Element> in a .NET Android, .NET iOS, .NET Mac Catalyst, or WinUI app, you must first enable .NET MAUI support in the native app's project file. Enable support by adding `<UseMaui>true</UseMaui>` to the first `<PropertyGroup>` node in the project file:

```xml
<PropertyGroup>
  ...
  <Nullable>enable</Nullable>
  <ImplicitUsings>true</ImplicitUsings>
  <UseMaui>true</UseMaui>
</PropertyGroup>
```

A consequence of doing this is that it replaces the native implicit namespace support with .NET MAUI namespaces, so you'll have to explicitly add `using` statements to your code files for native types.

For WinUI apps, you'll also need to add `<EnableDefaultXamlItems>false</EnableDefaultXamlItems>` to the first `<PropertyGroup>` node in the project file:

```xml
<PropertyGroup>
  ...
  <Nullable>enable</Nullable>
  <ImplicitUsings>true</ImplicitUsings>
  <UseMaui>true</UseMaui>
  <EnableDefaultXamlItems>false</EnableDefaultXamlItems>
</PropertyGroup>
```

This will stop you receiving build errors about the `InitializeComponent` method already being defined.

## Initialize .NET MAUI

.NET MAUI must be initialized before a native app project can construct a .NET MAUI control. Choosing when to initialize it primarily depends on when it's most convenient in your app flow&mdash;it could be performed at startup or just before a .NET MAUI control is constructed.

Typically, the pattern for initializing .NET MAUI in a native app project is to create a <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object, invoke the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding%2A> method on it, and then create a <xref:Microsoft.Maui.Hosting.MauiApp> object by invoking the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Build> method on the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object. In addition, a <xref:Microsoft.Maui.MauiContext> object should then be created from the <xref:Microsoft.Maui.Hosting.MauiApp> object. The <xref:Microsoft.Maui.MauiContext> object will be used when converting .NET MAUI controls to native types.

The examples in the following sections show the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding%2A> method being invoked at app startup.

:::zone pivot="devices-android"

On Android, the `OnCreate` override in the `MainActivity` class is typically the place to perform app startup related tasks. The following code example shows .NET MAUI being initialized in the `MainActivity` class:

```csharp
using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using Microsoft.Maui.Embedding;

namespace Notes.Android;

public class MainActivity : AppCompatActivity
{
    MauiContext _mauiContext;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Perform your normal Android registration

        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.UseMauiEmbedding<Microsoft.Maui.Controls.Application>();
        MauiApp mauiApp = builder.Build();
        _mauiContext = new MauiContext(mauiApp.Services, this);

        ...
    }
}
```

> [!NOTE]
> The call to the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding%2A> method can specify your own <xref:Microsoft.Maui.Controls.Application> derived class, such as `MyApp`. For example, `builder.UseMauiEmbedding<MyApp>();`.

:::zone-end

:::zone pivot="devices-ios, devices-maccatalyst"

On iOS and Mac Catalyst, the `FinishedLaunching` override in the `AppDelegate` class is typically the place to perform app startup related tasks. It's called after the app has launched, and is usually overridden to configure the main window and view controller. The following code example shows .NET MAUI being initialized in the `AppDelegate` class:

```csharp
using Foundation;
using Microsoft.Maui.Embedding;
using UIKit;

namespace Notes.MaciOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    MauiContext _mauiContext;

    public override UIWindow? Window
    {
        get;
        set;
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // Perform your normal iOS registration

        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.UseMauiEmbedding<Microsoft.Maui.Controls.Application>();
        // Register the Window
        builder.Services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(typeof(UIWindow), Window));
        MauiApp mauiApp = builder.Build();
        _mauiContext = new MauiContext(mauiApp.Services);

        ...
        return true;
    }
}
```

> [!NOTE]
> The call to the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding%2A> method can specify your own <xref:Microsoft.Maui.Controls.Application> derived class, such as `MyApp`. For example, `builder.UseMauiEmbedding<MyApp>();`.

:::zone-end

:::zone pivot="devices-windows"

On Windows apps built using WinUI, .NET MAUI can typically be initialized in the native `App` class, `Window` class, or a <xref:Microsoft.Maui.Controls.Page> class. The following code example shows .NET MAUI being initialized in a <xref:Microsoft.Maui.Controls.Page> class:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Embedding;
using Microsoft.Maui.Hosting;
using Microsoft.UI.Xaml.Controls;

namespace Notes.Windows;

public sealed partial class MainPage : Page
{
    public MauiContext _mauiContext;

    public MainPage()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.UseMauiEmbedding<Microsoft.Maui.Controls.Application>();
        MauiApp mauiApp = builder.Build();
        _mauiContext = new MauiContext(mauiApp.Services);

        ...
    }
}
```

> [!NOTE]
> The call to the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding%2A> method can specify your own <xref:Microsoft.Maui.Controls.Application> derived class, such as `MyApp`. For example, `builder.UseMauiEmbedding<MyApp>();`.

:::zone-end

## Add .NET MAUI views

TEXT GOES HERE

## Consume .NET MAUI controls

To consume .NET MAUI types that derive from <xref:Microsoft.Maui.Controls.Element> in native apps, create an instance of the control and convert it to the appropriate native type with the `ToPlatform` extension method.

:::zone pivot="devices-android"

On Android, `ToPlatform` converts the .NET MAUI control to an Android <xref:Android.Views.View> object:

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
Android.Views.View view = myMauiPage.ToPlatform(_mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object is converted to an Android <xref:Android.Views.View> object.

Alternatively, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object can be converted to a `Fragment` with the following `CreateSupportFragment` extension method:

```csharp
using Android.OS;
using Android.Views;
using Microsoft.Maui.Platform;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Notes.Android;

public static class PageExtensions
{
    public static Fragment CreateSupportFragment(this ContentPage view, MauiContext context)
    {
        return new ScopedFragment(view, context);
    }

    internal class ScopedFragment : Fragment
    {
        readonly IMauiContext _mauiContext;

        public IView DetailView { get; private set; }

        public ScopedFragment(IView detailView, IMauiContext mauiContext)
        {
            DetailView = detailView;
            _mauiContext = mauiContext;
        }

        public override global::Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return DetailView.ToPlatform(_mauiContext);
        }
    }
}
```

The `CreateSupportFragment` extension method can be consumed by invoking it on a <xref:Microsoft.Maui.Controls.ContentPage>-derived object:

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
AndroidX.Fragment.App.Fragment notesPageFragment = myMauiPage.CreateSupportFragment(_mauiContext);
```

The resulting `Fragment` can then be managed by Android's `FragmentManager` class.

For more information about Fragments, see [Fragments](https://developer.android.com/guide/fragments) on developer.android.com.

:::zone-end

:::zone pivot="devices-ios, devices-maccatalyst"

On iOS and Mac Catalyst, `ToPlatform` converts the .NET MAUI control to a <xref:UIKit.UIView> object:

```csharp
Button myButton = new Button { Text = ".NET MAUI" };
UIView button = myButton.ToPlatform(_mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.Button> is converted to a <xref:UIKit.UIView> object.

In addition, a `ToUIViewController` extension method can be used to attempt to convert a .NET MAUI control to a <xref:UIKit.UIViewController>:

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
UIViewController myPageController = myMauiPage.ToUIViewController(_mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object is converted to a <xref:UIKit.UIViewController>.

:::zone-end

:::zone pivot="devices-windows"

On Windows, `ToPlatform` converts the .NET MAUI control to a `FrameworkElement` object:

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
FrameworkElement element = myMauiPage.ToPlatform(_mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object is converted to a `FrameworkElement` object. The `FrameworkElement` object can then be set as the content of a WinUI page.

:::zone-end
