---
title: "Native embedding"
description: "Learn how to consume .NET MAUI controls inside .NET for iOS, .NET for Android, and WinUI native apps."
ms.date: 11/07/2022
---

# Native embedding

Typically, a .NET Multi-platform App UI (.NET MAUI) app includes pages that contains layouts, such as <xref:Microsoft.Maui.Controls.Grid>, and layouts that contain views, such as <xref:Microsoft.Maui.Controls.Button>. Pages, layout, and views, all derive from <xref:Microsoft.Maui.Controls.Element>. Native embedding enables any .NET MAUI controls that derive from <xref:Microsoft.Maui.Controls.Element> to be consumed in .NET for iOS, .NET for Android, and WinUI native apps.

The process for consuming a .NET MAUI control in a native app is as follows:

1. Enable .NET MAUI support by adding `<UseMaui>true</UseMaui>` to the native app's project file.
1. Initialize .NET MAUI by calling the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding*> method.
1. Add your .NET MAUI code, such as a page's, and any dependencies, to the native project.
1. Create an instance of the .NET MAUI control and convert it to the appropriate native type with the `ToPlatform` extension method.

> [!NOTE]
> When using native embedding, .NET MAUI's data binding engine still works. However, page navigation must be performed using the native navigation API.

## Enable .NET MAUI support

To consume .NET MAUI controls that derive from <xref:Microsoft.Maui.Controls.Element> in a .NET for iOS, .NET for Android, or WinUI app, you must first enable .NET MAUI support in the native app's project file. This can be accomplished by adding `<UseMaui>true</UseMaui>` to the first `<PropertyGroup>` node in the *.csproj*:

```xml
<PropertyGroup>
  ...
  <Nullable>enable</Nullable>
  <ImplicitUsings>true</ImplicitUsings>
  <UseMaui>true</UseMaui>
</PropertyGroup>
```

A consequence of doing this is that it will replace the native implicit namespace support with .NET MAUI's, and so you'll have to explicitly add `using` statements to your code files for native types.

## Initialize .NET MAUI

.NET MAUI must be initialized before a native app project can construct a .NET MAUI control. Choosing when to do this primarily depends on when it's most convenient in your app flow - it could be performed at startup, or just before a .NET MAUI control is constructed.

Typically, the pattern for initializing .NET MAUI in a native app project is to create a <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object, invoke the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding*> method on it, and then create a <xref:Microsoft.Maui.Hosting.MauiApp> object by invoking the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Build> method on the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object. In addition, a <xref:Microsoft.Maui.MauiContext> object should then be created from the <xref:Microsoft.Maui.Hosting.MauiApp> object. The <xref:Microsoft.Maui.MauiContext> object will be used when converting .NET MAUI controls to native types.

The examples in the following sections show the <xref:Microsoft.Maui.Embedding.AppHostBuilderExtensions.UseMauiEmbedding*> method being invoked at app startup.

### Android

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

### iOS

On iOS, the `FinishedLaunching` override in the `AppDelegate` class is typically the place to perform app startup related tasks. It's called after the app has launched, and is usually overridden to configure the main window and view controller. The following code example shows .NET MAUI being initialized in the `AppDelegate` class:

```csharp
using Foundation;
using Microsoft.Maui.Embedding;
using UIKit;

namespace Notes.iOS;

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

### Windows

On Windows apps built using WinUI, .NET MAUI can typically be initialized in the native `App` class, `Window` class, or a `Page` class. The following code example shows .NET MAUI being initialized in a `Page` class:

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

## Consume .NET MAUI controls

To consume .NET MAUI types that derive from <xref:Microsoft.Maui.Controls.Element> in native apps, create an instance of the control and convert it to the appropriate native type with the `ToPlatform` extension method.

### Android

On Android, `ToPlatform` converts the .NET MAUI control to a <xref:Android.Views.View> object:

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
Android.Views.View view = myMauiPage.ToPlatform(_mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object is converted to a <xref:Android.Views.View> object.

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

### iOS

On iOS, `ToPlatform` converts the .NET MAUI control to a <xref:UIKit.UIView> object:

```csharp
Button myButton = new Button { Text = ".NET MAUI" };
UIView button = myButton.ToPlatform(_mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.Button> is converted to a <xref:UIKit.UIView> object.

In addition, iOS has a `ToUIViewController` extension method that attempts to convert a .NET MAUI control to a <xref:UIKit.UIViewController>:

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
UIViewController myPageController = myMauiPage.ToUIViewController(_mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object is converted to a <xref:UIKit.UIViewController>.

### Windows

On Windows, `ToPlatform` converts the .NET MAUI control to a `FrameworkElement` object.

```csharp
MyMauiPage myMauiPage = new MyMauiPage();
FrameworkElement element = myMauiPage.ToPlatform(_mauiContext);
```

In this example, a <xref:Microsoft.Maui.Controls.ContentPage>-derived object is converted to a `FrameworkElement` object. The `FrameworkElement` object can then be set as the content of a WinUI page.
