---
title: "Native embedding"
description: "Learn how to consume .NET MAUI controls inside .NET for iOS, .NET for Android, and WinUI native apps."
ms.date: 10/15/2024
zone_pivot_groups: devices-platforms
---

# Native embedding

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-nativeembedding)

Typically, a .NET Multi-platform App UI (.NET MAUI) app includes pages that contain layouts, such as <xref:Microsoft.Maui.Controls.Grid>, and layouts that contain views, such as <xref:Microsoft.Maui.Controls.Button>. Pages, layouts, and views all derive from <xref:Microsoft.Maui.Controls.Element>. Native embedding enables any .NET MAUI controls that derive from <xref:Microsoft.Maui.Controls.Element> to be consumed in .NET for Android, .NET for iOS, .NET for Mac Catalyst, and WinUI native apps.

The process for consuming a .NET MAUI control in a native app is as follows:

::: moniker range="=net-maui-8.0"

1. Create extension methods to bootstrap your native embedded app. For more information, see [Create extension methods](#create-extension-methods).
1. Create a .NET MAUI single project that contains your .NET MAUI UI and any dependencies. For more information, see [Create a .NET MAUI single project](#create-a-net-maui-single-project).
1. Create a native app and enable .NET MAUI support in it. For more information, see [Enable .NET MAUI support](#enable-net-maui-support).
1. Initialize .NET MAUI by calling the `UseMauiEmbedding` extension method. For more information, see [Initialize .NET MAUI](#initialize-net-maui).
1. Create the .NET MAUI UI and convert it to the appropriate native type with the `ToPlatformEmbedding` extension method. For more information, see [Consume .NET MAUI controls](#consume-net-maui-controls).

::: moniker-end

::: moniker range=">=net-maui-9.0"

1. Create a .NET MAUI single project that contains your .NET MAUI UI and any dependencies. For more information, see [Create a .NET MAUI single project](#create-a-net-maui-single-project).
1. Create a native app and enable .NET MAUI support in it. For more information, see [Enable .NET MAUI support](#enable-net-maui-support).
1. Initialize .NET MAUI by calling the `UseMauiEmbedding` extension method. For more information, see [Initialize .NET MAUI](#initialize-net-maui).
1. Create the .NET MAUI UI and convert it to the appropriate native type with the `ToPlatformEmbedding` extension method. For more information, see [Consume .NET MAUI controls](#consume-net-maui-controls).

::: moniker-end

> [!NOTE]
> When using native embedding, .NET MAUI's data binding engine still works. However, page navigation must be performed using the native navigation API.

::: moniker range="=net-maui-8.0"

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
01. Modify the project file so that the `$(TargetFramework)` build property is set to `net8.0`, and the `$(OutputType)` build property is removed:

    ```xml
    <PropertyGroup>
      <TargetFramework>net8.0</TargetFramework>

      <RootNamespace>MyMauiApp</RootNamespace>
      <UseMaui>true</UseMaui>
      <SingleProject>true</SingleProject>
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>

      ...
    </PropertyGroup>
    ```

    > [!IMPORTANT]
    > Ensure you set the `$(TargetFramework)` build property, not the`$(TargetFrameworks)` build property.

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

::: moniker-end

::: moniker range=">=net-maui-9.0"

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
01. Modify the project file so that the `$(TargetFramework)` build property is set to `net9.0`, and the `$(OutputType)` build property is removed:

    ```xml
    <PropertyGroup>
      <TargetFramework>net9.0</TargetFramework>

      <RootNamespace>MyMauiApp</RootNamespace>
      <UseMaui>true</UseMaui>
      <SingleProject>true</SingleProject>
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>

      ...
    </PropertyGroup>
    ```

    > [!IMPORTANT]
    > Ensure you set the `$(TargetFramework)` build property, not the`$(TargetFrameworks)` build property.

01. In the `MauiProgram` class, create a `CreateMauiApp` overload that accepts an optional `Action<MauiAppBuilder>` argument:

    ```csharp
    public static class MauiProgram
    {
        // Create a MauiApp using the default application.
        public static MauiApp CreateMauiApp(Action<MauiAppBuilder>? additional = null) =>
            CreateMauiApp<App>(additional);

        ...
    }
    ```

01. In the `MauiProgram` class, modify the existing `CreateMauiApp` method to accept a `TApp` generic argument, and accept an optional `Action<MauiAppBuilder>` argument that's invoked before the method returns. In addition, change the call to `UseMauiApp<App>` to `UseMauiEmbeddedApp<TApp>`:

    ```csharp
    public static class MauiProgram
    {
        ...

        // Create a MauiApp using the specified application.
        public static MauiApp CreateMauiApp<TApp>(Action<MauiAppBuilder>? additional = null) where TApp : App
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiEmbeddedApp<TApp>()
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
    }
    ```

You should then add your required .NET MAUI UI to the project, including any dependencies and resources, and ensure that the project builds correctly.

::: moniker-end

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

::: moniker range="=net-maui-8.0"

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

::: moniker-end

::: moniker range=">=net-maui-9.0"

For .NET for Mac Catalyst apps, you'll also need to set the `$(SupportedOSPlatformVersion)` build property to a minimum of 15.0:

```xml
<PropertyGroup>
    ...
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>

    <SupportedOSPlatformVersion>15.0</SupportedOSPlatformVersion>
    <UseMaui>true</UseMaui>
    <MauiEnablePlatformUsings>true</MauiEnablePlatformUsings>  
</PropertyGroup>
```

::: moniker-end

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

::: moniker range="=net-maui-8.0"

Then, add `$(PackageReference)` build items to the project file for the `Microsoft.Maui.Controls` and `Microsoft.Maui.Controls.Compatiblity` NuGet packages:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
    <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
</ItemGroup>
```

::: moniker-end

::: moniker range=">=net-maui-9.0"

Then, add `$(PackageReference)` build items to the project file for the `Microsoft.Maui.Controls` NuGet package:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
</ItemGroup>
```

::: moniker-end

## Initialize .NET MAUI

.NET MAUI must be initialized before a native app project can construct a .NET MAUI control. Choosing when to initialize it primarily depends on when it's most convenient in your app flow - it could be performed at startup or just before a .NET MAUI control is constructed. The approach outlined here is to initialize .NET MAUI when the app's initial UI is created.

Typically, the pattern for initializing .NET MAUI in a native app project is as follows:

- Create a <xref:Microsoft.Maui.Hosting.MauiApp> object.
- Create a <xref:Microsoft.Maui.MauiContext> object from the <xref:Microsoft.Maui.Hosting.MauiApp> object.

:::zone pivot="devices-android"

On Android, the `OnCreate` override in the `MainActivity` class is typically the place to perform app startup related tasks. The following code example shows .NET MAUI being initialized in the `MainActivity` class:

```csharp
namespace MyNativeEmbeddedApp.Droid;

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
```

:::zone-end

:::zone pivot="devices-ios, devices-maccatalyst"

On iOS and Mac Catalyst, the `AppDelegate` class should be modified to return `true` for the `FinishedLaunching` override:

```csharp
namespace MyNativeEmbeddedApp.iOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window { get; set; }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions) => true;
}
```

The `WillConnect` method in the `SceneDelegate` class should then be modified to create your main view controller and set it as the view of the `UINavigationController`:

```csharp
namespace MyNativeEmbeddedApp.iOS;

[Register("SceneDelegate")]
public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
{
    [Export("window")]
    public UIWindow? Window { get; set; }

    [Export("scene:willConnectToSession:options:")]
    public void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
    {
        if (scene is not UIWindowScene windowScene)
            return;

        Window = new UIWindow(windowScene);

        var mainVC = new MainViewController();
        var navigationController = new UINavigationController(mainVC);
        navigationController.NavigationBar.PrefersLargeTitles = true;

        Window.RootViewController = navigationController;
        Window.MakeKeyAndVisible();
    }

    /// ...
}
```

Then, in the XML editor, open the **Info.plist** file and add the following XML to the end of the file:

```xml
<key>UIApplicationSceneManifest</key>
<dict>
  <key>UIApplicationSupportsMultipleScenes</key>
  <true/>
  <key>UISceneConfigurations</key>
  <dict>
    <key>UIWindowSceneSessionRoleApplication</key>
    <array>
      <dict>
        <key>UISceneConfigurationName</key>
        <string>Default Configuration</string>
        <key>UISceneDelegateClassName</key>
        <string>SceneDelegate</string>
      </dict>
    </array>
  </dict>
</dict>
```

.NET MAUI can then be initialized in the `ViewDidLoad` method in your main view controller:

```csharp
using Microsoft.Maui.Platform;

namespace MyNativeEmbeddedApp.iOS;

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
```

:::zone-end

:::zone pivot="devices-windows"

On Windows, the `MainWindow` class is typically the place to perform UI related app startup tasks:

```csharp
namespace MyNativeEmbeddedApp.WinUI;

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

::: moniker range="=net-maui-8.0"

> [!NOTE]
> The `ToPlatformEmbedded` extension method is in the .NET MAUI class library you created earlier. Therefore your native app project should include a reference to that project.

::: moniker-end

::: moniker range=">=net-maui-9.0"

> [!NOTE]
> The `ToPlatformEmbedded` extension method is in the <xref:Microsoft.Maui.Controls.Embedding> namespace. Therefore your native app project should include a `using` statement for that namespace.

::: moniker-end

The <xref:Android.Views.View> object can then be added to a layout in your native app:

```csharp
rootLayout.AddView(nativeView, new LinearLayout.LayoutParams(MatchParent, WrapContent));
```

:::zone-end

:::zone pivot="devices-ios, devices-maccatalyst"

On iOS and Mac Catalyst, the `ToPlatformEmbedded` extension method converts the .NET MAUI control to a <xref:UIKit.UIView> object:

::: moniker range="=net-maui-8.0"

```csharp
var mauiView = new MyMauiContent();
UIView nativeView = mauiView.ToPlatformEmbedded(mauiContext);
nativeView.WidthAnchor.ConstraintEqualTo(View.Frame.Width).Active = true;
nativeView.HeightAnchor.ConstraintEqualTo(500).Active = true;
```

In this example, a <xref:Microsoft.Maui.Controls.ContentView>-derived object is converted to a <xref:UIKit.UIView> object and then width and height constraints are set on it to allow interaction.

> [!NOTE]
> The `ToPlatformEmbedded` extension method is in the .NET MAUI class library you created earlier. Therefore your native app project should include a reference to that project.

The <xref:UIKit.UIView> object can then be added to a view in your view controller:

```csharp
stackView.AddArrangedSubView(nativeView);
```

::: moniker-end

::: moniker range=">=net-maui-9.0"

```csharp
var mauiView = new MyMauiContent();
UIView nativeView = mauiView.ToPlatformEmbedded(mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentView>-derived object is converted to a <xref:UIKit.UIView> object.

> [!NOTE]
> The `ToPlatformEmbedded` extension method is in the <xref:Microsoft.Maui.Controls.Embedding> namespace. Therefore your native app project should include a `using` statement for that namespace.

The <xref:UIKit.UIView> object can then be added to a view in your view controller:

```csharp
stackView.AddArrangedSubView(new ContainerView(nativeView));
```

`ContainerView` is a custom type that wraps the .NET MAUI view to ensure that it's sized correctly. This is achieved by redirecting `IntrinsicContentSize` to the .NET MAUI view's `SizeThatFits`:

```csharp
class ContainerView : UIView
{
    public ContainerView(UIView view)
    {
        AddSubview(view);
    }

    public override CGSize IntrinsicContentSize =>
        SizeThatFits(new CGSize(nfloat.MaxValue, nfloat.MaxValue));

    public override CGSize SizeThatFits(CGSize size) =>
        Subviews?.FirstOrDefault()?.SizeThatFits(size) ?? CGSize.Empty;

    public override void LayoutSubviews()
    {
        if (Subviews?.FirstOrDefault() is { } view)
            view.Frame = Bounds;
    }

    public override void SetNeedsLayout()
    {
        base.SetNeedsLayout();
          InvalidateIntrinsicContentSize();
    }
}
```

::: moniker-end

In addition, a `ToUIViewController` extension method in .NET MAUI can be used to attempt to convert a .NET MAUI page to a <xref:UIKit.UIViewController>:

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
UIViewController myPageController = myMauiPage.ToUIViewController(mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object is converted to a <xref:UIKit.UIViewController>.

:::zone-end

:::zone pivot="devices-windows"

On Windows, the `ToPlatformEmbedded` extension method converts the .NET MAUI control to a <xref:Microsoft.UI.Xaml.FrameworkElement> object:

```csharp
var mauiView = new MyMauiContent();
FrameworkElement nativeView = myMauiPage.ToPlatformEmbedded(mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentView>-derived object is converted to a <xref:Microsoft.UI.Xaml.FrameworkElement> object. The <xref:Microsoft.UI.Xaml.FrameworkElement> object can then be set as the content of a WinUI page.

The <xref:Microsoft.UI.Xaml.FrameworkElement> object can then be added to a layout in your native app:

```csharp
stackPanel.Children.Add(nativeView);
```

:::zone-end

::: moniker range="=net-maui-8.0"

> [!IMPORTANT]
> To avoid an error occurring, XAML hot reload should be disabled before running a native embedded app in debug configuration.

## Support XAML hot reload

XAML hot reload isn't supported in native embedded apps. However, you can still use XAML hot reload to quickly iterate on your .NET MAUI UI by creating a .NET MAUI app that consumes the .NET MAUI UI.

To view your .NET MAUI UI with XAML hot reload:

1. In the project containing your .NET MAUI UI, update the `MauiProgram` class to add a `CreateMauiApp` overload, and modify the existing `CreateMauiApp` method to accept a generic argument:

    ```csharp
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp(Action<MauiAppBuilder>? additional = null) =>
            CreateMauiApp<App>(additional);

        public static MauiApp CreateMauiApp<TApp>(Action<MauiAppBuilder>? additional = null) where TApp : App
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<TApp>()
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
    }
    ```

1. In the project containing your .NET MAUI UI, convert each resource dictionary from a stand-alone XAML file to a resource dictionary that's backed by a code-behind file.
1. In the project containing your .NET MAUI UI, update your resource dictionary instantiation, typically in *App.xaml*, so that the `Source` property also specifies the assembly that contains the resource dictionary:

    ```xaml
    <ResourceDictionary Source="Resources/Styles/Colors.xaml;assembly=NativeEmbeddingDemo" />
    <ResourceDictionary Source="Resources/Styles/Styles.xaml;assembly=NativeEmbeddingDemo" />
    ```

1. Create a new .NET MAUI app and add it to the solution containing your .NET MAUI UI project and native embedded apps.
1. In your .NET MAUI app project, add a reference to the project that contains your .NET MAUI UI.
1. In your .NET MAUI app project, delete any **Resource** child folders where the resource is provided by your .NET MAUI UI project. For example, if your .NET MAUI UI project contains **Resources > Fonts**, **Resources > Images**, and **Resources > Styles** folders, these folders should be deleted from the .NET MAUI app you've just created. This enables your .NET MAUI app to consume the resources from the project containing your .NET MAUI UI.
1. In your .NET MAUI app, update your `App` class so that it derives from the `App` class in your .NET MAUI UI project:

    ```xaml
    <myMauiUIProject:App xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                         xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                         xmlns:myMauiUIProject="clr-namespace:NativeEmbeddingDemo;assembly=NativeEmbeddingDemo"
                         x:Class="TestHarnessApp.TestApp">
        <myMauiUIProject:App.Resources>
            <!-- App specific resources go here -->
        </myMauiUIProject:App.Resources>
    </myMauiUIProject:App>
    ```

    Then update the code-behind file for the `App` class so that it derives from the `App` class in your .NET MAUI UI project, and loads any XAML resources from this project:

    ```csharp
    public partial class TestApp : myMauiUIProject.App
    {
        public TestApp()
        {
            var baseResources = Resources;
            InitializeComponent();
            Resources.MergedDictionaries.Add(baseResources);
            MainPage = new HostPage();
        }
    }
    ```

1. In your .NET MAUI app, add a page that displays the UI from the project containing your .NET MAUI UI:

    ```xaml
    <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                 xmlns:myMauiUIProject="clr-namespace:NativeEmbeddingDemo;assembly=NativeEmbeddingDemo"
                 x:Class="TestHarnessApp.HostPage"
                 Title="HostPage">
        <myMauiUIProject:MyMauiContent />
    </ContentPage>
    ```

1. In your .NET MAUI app, update the `MauiProgram` class to call the `CreateMauiApp` method in the project containing your .NET MAUI UI:

    ```csharp
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp() =>
            NativeEmbeddingDemo.MauiProgram.CreateMauiApp<TestApp>(builder =>
            {
                // Add any test harness configuration such as service stubs or mocks.
            });
    }
    ```

You should now be able to run your .NET MAUI app project on each platform and use XAML hot reload to iterate on your .NET MAUI UI.

For an example of this approach, see the [sample app](/samples/dotnet/maui-samples/platformintegration-nativeembedding).

::: moniker-end
