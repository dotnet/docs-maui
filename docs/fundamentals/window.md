---
title: ".NET MAUI Window"
description: "The .NET MAUI Window class provides the ability to create, configure, show, and manage Windows."
ms.date: 04/14/2022
---

# .NET MAUI Window

The .NET Multi-platform App UI (.NET MAUI) `Window` class provides the ability to create, configure, show, and manage Windows.

`Window` defines the following properties:

- `Title`, of type `string`, represents the title of the window.
- `Page`, of type `Page`, indicates the page being displayed by the window. This property is the content property of the `Window` class, and therefore does not need to be explicitly set.
- `FlowDirection`, of type `FlowDirection`, defines the direction in which the UI element of the window are laid out.
- `Overlays`, of type `IReadOnlyCollection<IWindowOverlay>`, represents the collection window overlays.

These properties, with the exception of the `Overlays` property, are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

<!-- Todo: Is/will Title be shown on desktop platforms? -->

The `Window` class defines the following modal navigation events:

- `ModalPopped`, with `ModalPoppedEventArgs`
- `ModalPopping`, with `ModalPoppingEventArgs`
- `ModalPushed`, with `ModalPushedEventArgs`
- `ModalPushing`, with `ModalPushingEventArgs`
- `PopCanceled`

The `Window` class also defines the following lifecycle events:

- `Created`, which is raised when the Window is created.
- `Resumed`, which is raised when the Window is resumed from a sleeping state.
- `Activated`, which is raised when the Window is activated.
- `Deactivated`, which is raised when the Window is deactivated.
- `Stopped`, which is raised when the Window is stopped.
- `Destroying`, which is raised when the Window is destroyed.
- `Backgrounding`, with `BackgroundingEventArgs`, which is raised when the Window is entering a background state.
- `DisplayDensityChanged`, with `DisplayDensityChangedEventArgs`, which is raised when the effective dots per inch (DPI) for the Window has changed.

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

https://devblogs.microsoft.com/dotnet/announcing-dotnet-maui-preview-11/#multi-window-apps
https://github.com/dotnet/maui/pull/2811
https://vladislavantonyuk.azurewebsites.net/articles/.net-maui-multi-window-support


<!-- Todo: Multi-Window support (once added)
           Eventually there'll be a mechanism for getting from a View to a Window -->
