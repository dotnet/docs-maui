---
title: ".NET MAUI windows"
description: "Learn how to use the .NET MAUI Window class to create, configure, show, and manage multi-window apps."
ms.date: 10/04/2022
---

# .NET MAUI windows

The .NET Multi-platform App UI (.NET MAUI) `Window` class provides the ability to create, configure, show, and manage multiple windows.

`Window` defines the following properties:

- `Title`, of type `string`, represents the title of the window.
- `Page`, of type `Page`, indicates the page being displayed by the window. This property is the content property of the `Window` class, and therefore does not need to be explicitly set.
- `FlowDirection`, of type `FlowDirection`, defines the direction in which the UI element of the window are laid out.
- `Overlays`, of type `IReadOnlyCollection<IWindowOverlay>`, represents the collection of window overlays.

These properties, with the exception of the `Overlays` property, are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

<!-- Todo: Is/will Title be shown on desktop platforms? -->

The `Window` class defines the following modal navigation events:

- `ModalPopped`, with `ModalPoppedEventArgs`, which is raised when a view has been popped modally.
- `ModalPopping`, with `ModalPoppingEventArgs`, which is raised when a view is modally popped.
- `ModalPushed`, with `ModalPushedEventArgs`, which is raised after a view has been pushed modally.
- `ModalPushing`, with `ModalPushingEventArgs`, which is raised when a view is modally pushed.
- `PopCanceled`, which is raised when a modal pop is cancelled.

The `Window` class also defines the following lifecycle events:

- `Created`, which is raised when the Window is created.
- `Resumed`, which is raised when the Window is resumed from a sleeping state.
- `Activated`, which is raised when the Window is activated.
- `Deactivated`, which is raised when the Window is deactivated.
- `Stopped`, which is raised when the Window is stopped.
- `Destroying`, which is raised when the Window is destroyed.
- `Backgrounding`, with an accompanying `BackgroundingEventArgs` object, which is raised on iOS and Mac Catalyst when the Window is closed or enters a background state. This event can be used to persist any `string` state to the `State` property of the `BackgroundingEventArgs` object, which the OS will preserve until it's time to resume the window. When the window is resumed the state is provided via the `IActivationState` argument to the `CreateWindow` method.
- `DisplayDensityChanged`, with an accompanying `DisplayDensityChangedEventArgs` object, which is raised on Android and Windows when the effective dots per inch (DPI) for the window has changed.

For more information about the lifecycle events, and their associated overrides, see [App lifecycle](app-lifecycle.md).

## Create a Window

By default, .NET MAUI creates a `Window` object when you set the `MainPage` property to a `Page` object in your `App` class. However, you can also override the `CreateWindow` method in your `App` class to create a `Window` object:

```csharp
namespace MyMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);

            // Manipulate Window object

            return window;
        }
    }
}
```

While the `Window` class has a default constructor and a constructor that accepts a `Page` argument, which represents the root page of the app, you can also call the base `CreateWindow` method to return the .NET MAUI created `Window` object.

In addition, you can also create your own `Window`-derived object:

```csharp
namespace MyMauiApp
{
    public class MyWindow : Window
    {
        public MyWindow() : base()
        {
        }

        public MyWindow(Page page) : base(page)
        {
        }

        // Override Window methods
    }
}
```

The `Window`-derived class can then be consumed by creating a `MyWindow` object in the `CreateWindow` override in your `App` class.

Regardless of how your `Window` object is created, it will be the parent of the root page in your app.

## Multi-window support

Multiple windows can be simultaneously opened on Android, iOS on iPad (iPadOS), Mac Catalyst, and Windows. This can be achieved by creating a `Window` object and opening it using the `OpenWindow` method on the `Application` object:

```csharp
Window secondWindow = new Window(new MyPage());
Application.Current.OpenWindow(secondWindow);
```

The `Application.Current.Windows` collection, of type `IReadOnlyList<Window>` maintains references to all `Window` objects that are registered with the `Application` object.

Windows can be closed with the `Application.Current.CloseWindow` method:

```csharp
// Close a specific window
Application.Current.CloseWindow(secondWindow);

// Close the active window
Application.Current.CloseWindow(GetParentWindow());
```

> [!IMPORTANT]
> Multi-window support works on Android and Windows without additional configuration. However, additional configuration is required on iPadOS and Mac Catalyst.

### iPadOS and macOS configuration

To use multi-window support on iPadOS and Mac Catalyst, add a class named `SceneDelegate` to the **Platforms > iOS** and **Platforms > MacCatalyst** folders:

```csharp
using Foundation;
using Microsoft.Maui;
using UIKit;

namespace MyMauiApp;

[Register("SceneDelegate")]
public class SceneDelegate : MauiUISceneDelegate
{
}
```

Then, in the XML editor, open the **Platforms > iOS > Info.plist** file and the **Platforms > MacCatalyst > Info.plist** file and add the following XML to the end of each file:

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
        <string>__MAUI_DEFAULT_SCENE_CONFIGURATION__</string>
        <key>UISceneDelegateClassName</key>
        <string>SceneDelegate</string>
      </dict>
    </array>
  </dict>
</dict>
```

> [!IMPORTANT]
> Multi-window support doesn't work on iOS for iPhone.
