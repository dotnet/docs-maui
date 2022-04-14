---
title: ".NET MAUI Window"
description: "The .NET MAUI Window class provides the ability to create, configure, show, and manage Windows."
ms.date: 04/14/2022
---

# .NET MAUI Window

The .NET Multi-platform App UI (.NET MAUI) `Window` class provides the ability to create, configure, show, and manage Windows.

`Window` defines the following properties:

- `Title`, of type `string`, which represents the title of the window.
- `Page`, of type `Page`,  . This property is the content property of the `Window` class, and therefore does not need to be explicitly set.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

<!-- Todo: Is/will Title be shown on desktop platforms? -->

The `Window` class defines the following modal navigation events:

- `ModalPopped`, with `ModalPoppedEventArgs`
- `ModalPopping`, with `ModalPoppingEventArgs`
- `ModalPushed`, with `ModalPushedEventArgs`
- `ModalPushing`, with `ModalPushingEventArgs`
- `PopCanceled`

For more information about the model navigation events, see XXXXXXXXXXX.

The `Window` class also defines the following lifecycle events:

- `Created`
- `Resumed`
- `Activated`
- `Deactivated`
- `Stopped`
- `Destroying`

For more information about the lifecycle events, and their associated overrides, see [App lifecycle](~/fundamentals/app-lifecycle.md).

## Create a Window

By default, .NET MAUI creates a `Window` object when you set the `MainPage` property to a `Page` object in your `App` class. However, you can also override the `CreateWindow` method in your `App` class to create a `Window` object:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;

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
using Microsoft.Maui.Controls;

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

> [!WARNING]
> An `InvalidOperationException` will be thrown if the `App.MainPage` property is set and the `CreateWindow` method creates a `Window` object using the override that accepts a `Page` argument.

Regardless of how your `Window` object is created, it will be the parent of the root page in your app, and the parent of the `Window` object is the `Application` object.

<!-- Todo: Multi-Window support (once added)
           Eventually there'll be a mechanism for getting from a View to a Window -->
