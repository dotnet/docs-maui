---
title: "Native embedding"
description: "Learn how to consume .NET MAUI controls inside .NET for iOS, .NET for Android, and WinUI native apps."
ms.date: 03/11/2024
zone_pivot_groups: devices-deployment
---

# Native embedding

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-nativeembedding)

Typically, a .NET Multi-platform App UI (.NET MAUI) app includes pages that contain layouts, such as <xref:Microsoft.Maui.Controls.Grid>, and layouts that contain views, such as <xref:Microsoft.Maui.Controls.Button>. Pages, layouts, and views all derive from <xref:Microsoft.Maui.Controls.Element>. Native embedding enables any .NET MAUI controls that derive from <xref:Microsoft.Maui.Controls.Element> to be consumed in .NET for Android, .NET for iOS, .NET for Mac Catalyst, and WinUI native apps.

The process for consuming a .NET MAUI control in a native app is as follows:

1. Create extension methods to bootstrap your native embedded app. For more information, see [Create extension methods](#create-extension-methods).
1. Create a .NET MAUI single project that contains your .NET MAUI UI and any dependencies. For more information, see [Create a .NET MAUI single project](#create-a-net-maui-single-project).
1. Create a native app and enable .NET MAUI support in it. For more information, see [Enable .NET MAUI support](#enable-net-maui-support).
1. Initialize .NET MAUI by calling the `UseMauiEmbedding` extension method. For more information, see [Initialize .NET MAUI](#initialize-net-maui).
1. Create the .NET MAUI UI and convert it to the appropriate native type with the `ToPlatformEmbedding` extension method. For more information, see [Consume .NET MAUI controls](#consume-net-maui-controls).

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

## Create a .NET MAUI single project

Before creating a native app that consumes .NET MAUI controls, you should add a .NET MAUI app project to the same solution as the .NET MAUI class library project you created previously. The .NET MAUI app project will store the UI you intend you re-use in your native embedded app. After adding a new .NET MAUI app project to the solution, perform the following steps:

01. Delete the **Properties** folder from the project.
01. Delete the **Platforms** folder from the project.
01. Delete the **Resources/AppIcon** folder from the project.
01. Delete the **Resources/raw** folder from the project.
01. Delete the **Resources/Splash** folder from the project.
01. Delete the `AppShell` class from the project.
01. Modify the `App` class so that it doesn't set the `MainPage` property:

    ```csharp
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
    }
    ```

01. Delete the `MainPage` class from the project.
01. Modify the project file so that the `$(TargetFrameworks)` build property is set to `net8.0`, and the `$(OutputType)` build property is removed:

    ```xml
    <PropertyGroup>
      <TargetFrameworks>net8.0</TargetFrameworks>

      <RootNamespace>MyMauiApp</RootNamespace>
      <UseMaui>true</UseMaui>
      <SingleProject>true</SingleProject>
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>

      ...
    </PropertyGroup>
    ```

01. Modify the `CreateMauiApp` method in the `MauiProgram` class so that it accepts an optional `Action<MauiAppBuilder>` argument that's invoked before the method returns:

    ```csharp
    public static MauiApp CreateMauiApp(Action<MauiAppBuilder>? additional = null)
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        #if DEBUG
            builder.Logging.AddDebug();
        #endif

        additional?.Invoke(builder);
        return builder.Build();
    }
    ```

At this point you should add your required .NET MAUI UI to the project, including any dependencies and resources, and ensure that the project builds correctly.

## Enable .NET MAUI support

To consume .NET MAUI controls that derive from <xref:Microsoft.Maui.Controls.Element> in a .NET for Android, .NET for iOS, .NET for Mac Catalyst, or WinUI app, you should add your native app project to the same solution as the .NET MAUI class library project you created previously. Then you should enable .NET MAUI support in your native app's project file by setting the `$(UseMaui)` and `$(MauiEnablePlatformUsings)` build properties to `true` in the first `<PropertyGroup>` node in the project file:

```xml
<PropertyGroup>
    ...
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>

    <UseMaui>true</UseMaui>
    <MauiEnablePlatformUsings>true</MauiEnablePlatformUsings>  
</PropertyGroup>
```

:::zone pivot="devices-maccatalyst"

For .NET for Mac Catalyst apps, you'll also need to set the `$(SupportedOSPlatformVersion)` build property to a minimum of 14.0:

```xml
<PropertyGroup>
    ...
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>

    <SupportedOSPlatformVersion>14.2</SupportedOSPlatformVersion>
    <UseMaui>true</UseMaui>
    <MauiEnablePlatformUsings>true</MauiEnablePlatformUsings>  
</PropertyGroup>
```

:::zone-end

:::zone pivot="devices-windows"

For WinUI apps, you'll also need to set the `$(EnableDefaultXamlItems)` build property to `false`:

```xml
<PropertyGroup>
    ...
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>

    <UseMaui>true</UseMaui>
    <MauiEnablePlatformUsings>true</MauiEnablePlatformUsings>    
    <EnableDefaultXamlItems>false</EnableDefaultXamlItems>
</PropertyGroup>
```

This will stop you receiving build errors about the `InitializeComponent` method already being defined.

:::zone-end

Then, add `$(PackageReference)` build items to the project file for the `Microsoft.Maui.Controls` and `Microsoft.Maui.Controls.Compatiblity` NuGet packages:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
    <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
</ItemGroup>
```

## Initialize .NET MAUI

.NET MAUI must be initialized before a native app project can construct a .NET MAUI control. Choosing when to initialize it primarily depends on when it's most convenient in your app flow - it could be performed at startup or just before a .NET MAUI control is constructed. The approach outlined here is to initialize .NET MAUI when the app's initial UI is created.

Typically, the pattern for initializing .NET MAUI in a native app project is as follows:

- Create a <xref:Microsoft.Maui.Hosting.MauiApp> object.
- Create a <xref:Microsoft.Maui.MauiContext> object from the <xref:Microsoft.Maui.Hosting.MauiApp> object.

:::zone pivot="devices-android"

On Android, the `OnCreate` override in the `MainActivity` class is typically the place to perform app startup related tasks. The following code example shows .NET MAUI being initialized in the `MainActivity` class:

```csharp
namespace MyNativeEmbeddedApp.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/AppTheme")]
    public class MainActivity : Activity
    {
        public static readonly Lazy<MauiApp> MauiApp = new(() =>
        {
            var mauiApp = MauiProgram.CreateMauiApp(builder =>
            {
                builder.UseMauiEmbedding();
            });
            return mauiApp;
        });

        public static bool UseWindowContext = true;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Ensure .NET MAUI app is built before creating .NET MAUI views
            var mauiApp = MainActivity.MauiApp.Value;

            // Create .NET MAUI context
            var mauiContext = UseWindowContext
                ? mauiApp.CreateEmbeddedWindowContext(this) // Create window context
                : new MauiContext(mauiApp.Services, this);  // Create app context

            ...              
        }
    }
}
```

:::zone-end

:::zone pivot="devices-ios, devices-maccatalyst"

On iOS and Mac Catalyst, the `FinishedLaunching` override in the `AppDelegate` class should be modified to create your main view controller:

```csharp
namespace MyNativeEmbeddedApp.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow? Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            var vc = new MainViewController();
            Window.RootViewController = vc;
            Window.MakeKeyAndVisible();
            return true;
        }
    }
}
```

.NET MAUI can then be initialized in the `ViewDidLoad` method in your main view controller:

```csharp
using Microsoft.Maui.Platform;

namespace MyNativeEmbeddedApp.iOS
{
    public class MainViewController : UIViewController
    {
        UIWindow GetWindow() =>
            View?.Window ??
            ParentViewController?.View?.Window ??
            MainViewController.MauiApp.Value.Services.GetRequiredService<IUIApplicationDelegate>().GetWindow() ??
            UIApplication.SharedApplication.Delegate.GetWindow();

        public static readonly Lazy<MauiApp> MauiApp = new(() =>
        {
            var mauiApp = MauiProgram.CreateMauiApp(builder =>
            {
                builder.UseMauiEmbedding();
            });
            return mauiApp;
        });

        public static bool UseWindowContext = true;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Ensure app is built before creating .NET MAUI views
            var mauiApp = MainViewController.MauiApp.Value;

            // Create .NET MAUI context
            var mauiContext = UseWindowContext
                ? mauiApp.CreateEmbeddedWindowContext(GetWindow()) // Create window context
                : new MauiContext(mauiApp.Services);               // Create app context

            ...
        }
    }
}
```

:::zone-end

:::zone pivot="devices-windows"

On Windows, the `MainWindow` class is typically the place to perform UI related app startup tasks:

```csharp
namespace MyNativeEmbeddedApp.WinUI
{
    public sealed partial class MainWindow : Microsoft.UI.Xaml.Window
    {
        public static readonly Lazy<MauiApp> MauiApp = new(() =>
        {
            var mauiApp = MauiProgram.CreateMauiApp(builder =>
            {
                builder.UseMauiEmbedding();
            });
            return mauiApp;
        });

        public static bool UseWindowContext = true;

        public MainWindow()
        {
            this.InitializeComponent();

            // Ensure .NET MAUI app is built before creating .NET MAUI views
            var mauiApp = MainWindow.MauiApp.Value;

            // Create .NET MAUI context
            var mauiContext = UseWindowContext
                ? mauiApp.CreateEmbeddedWindowContext(this) // Create window context
                : new MauiContext(mauiApp.Services);        // Create app context

            ...
        }
    }
}
```

:::zone-end

In this example, the <xref:Microsoft.Maui.Hosting.MauiApp> object is created using lazy initialization. The `UseMauiEmbedding` extension method is invoked on the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object. Therefore, your native app project should include a reference to the .NET MAUI class library project you created that contains this extension method. A <xref:Microsoft.Maui.MauiContext> object is then created from the <xref:Microsoft.Maui.Hosting.MauiApp> object, with a `bool` determining where the context is scoped from. The <xref:Microsoft.Maui.MauiContext> object will be used when converting .NET MAUI controls to native types.

## Consume .NET MAUI controls

After .NET MAUI has been initialized in your native app, you can add your .NET MAUI UI to your native app's layout. This can be achieved by creating an instance of the UI and converting it to the appropriate native type with the `ToPlatformEmbedded` extension method.

:::zone pivot="devices-android"

On Android, the `ToPlatformEmbedded` extension method converts the .NET MAUI control to an Android <xref:Android.Views.View> object:

```csharp
var mauiView = new MyMauiContent();
Android.Views.View nativeView = mauiView.ToPlatformEmbedded(mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentView>-derived object is converted to an Android <xref:Android.Views.View> object.

> [!NOTE]
> The `ToPlatformEmbedded` extension method is in the .NET MAUI class library you created earlier. Therefore your native app project should include a reference to that project.

The <xref:Android.Views.View> object can then be added to a layout in your native app:

```csharp
rootLayout.AddView(nativeView, new LinearLayout.LayoutParams(MatchParent, WrapContent));
```

:::zone-end

:::zone pivot="devices-ios, devices-maccatalyst"

On iOS and Mac Catalyst, the `ToPlatformEmbedded` extension method converts the .NET MAUI control to a <xref:UIKit.UIView> object:

```csharp
var mauiView = new MyMauiContent();
UIView nativeView = mauiView.ToPlatformEmbedded(mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentView>-derived object is converted to a <xref:UIKit.UIView> object.

> [!NOTE]
> The `ToPlatformEmbedded` extension method is in the .NET MAUI class library you created earlier. Therefore your native app project should include a reference to that project.

The <xref:UIKit.UIView> object can then be added to a view in your view controller:

```csharp
stackView.AddArrangedSubView(nativeView);
```

> [!WARNING]
> It's not currently possible to interact with the .NET MAUI UI on iOS and Mac Catalyst. For more information, see [GitHub issue #19340](https://github.com/dotnet/maui/issues/19340).

In addition, a `ToUIViewController` extension method in .NET MAUI can be used to attempt to convert a .NET MAUI page to a <xref:UIKit.UIViewController>:

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
UIViewController myPageController = myMauiPage.ToUIViewController(mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object is converted to a <xref:UIKit.UIViewController>.

:::zone-end

:::zone pivot="devices-windows"

On Windows, the `ToPlatformEmbedded` extension method converts the .NET MAUI control to a `FrameworkElement` object:

```csharp
var mauiView = new MyMauiContent();
FrameworkElement nativeView = myMauiPage.ToPlatformEmbedded(mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentView>-derived object is converted to a `FrameworkElement` object. The `FrameworkElement` object can then be set as the content of a WinUI page.

The `FrameworkElement` object can then be added to a layout in your native app:

```csharp
stackPanel.Children.Add(nativeView);
```

:::zone-end

> [!IMPORTANT]
> To avoid an error occurring, XAML hot reload should be disabled before running a native embedded app in debug configuration.
