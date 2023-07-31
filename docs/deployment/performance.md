---
title: "Improve app performance"
description: "Learn how to increase the performance of .NET MAUI apps by reducing the amount of work being performed by a CPU, and the amount of memory consumed by an app."
ms.date: 08/01/2023
---

<!-- TODO: Add https://learn.microsoft.com/en-us/xamarin/cross-platform/deploy-test/memory-perf-best-practices -->

# Improve app performance

Poor app performance presents itself in many ways. It can make an app seem unresponsive, can cause slow scrolling, and can reduce device battery life. However, optimizing performance involves more than just implementing efficient code. The user's experience of app performance must also be considered. For example, ensuring that operations execute without blocking the user from performing other activities can help to improve the user's experience.

There are many techniques for increasing the performance, and perceived performance, of .NET Multi-platform App UI (.NET MAUI) apps. Collectively these techniques can greatly reduce the amount of work being performed by a CPU, and the amount of memory consumed by an app.

## Use compiled bindings

Compiled bindings improve data binding performance in .NET MAUI apps by resolving binding expressions at compile time, rather than at runtime with reflection. Compiling a binding expression generates compiled code that typically resolves a binding 8-20 times quicker than using a classic binding. For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).

## Reduce unnecessary bindings

Don't use bindings for content that can easily be set statically. There is no advantage in binding data that doesn't need to be bound, because bindings aren't cost efficient. For example, setting `Button.Text = "Accept"` has less overhead than binding [`Button.Text`](xref:Microsoft.Maui.Controls.Button.Text) to a viewmodel `string` property with value "Accept".

## Choose the correct layout

A layout that's capable of displaying multiple children, but that only has a single child, is wasteful. For example, the following example shows a [`VerticalStackLayout`](xref:Microsoft.Maui.Controls.VerticalStackLayout) with a single child:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <VerticalStackLayout>
        <Image Source="waterfront.jpg" />
    </VerticalStackLayout>
</ContentPage>

```

This is wasteful and the [`VerticalStackLayout`](xref:Microsoft.Maui.Controls.VerticalStackLayout) element should be removed, as shown in the following example:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <Image Source="waterfront.jpg" />
</ContentPage>

```

In addition, don't attempt to reproduce the appearance of a specific layout by using combinations of other layouts, as this results in unnecessary layout calculations being performed. For example, don't attempt to reproduce a [`Grid`](xref:Microsoft.Maui.Controls.Grid) layout by using a combination of [`HorizontalStackLayout`](xref:Microsoft.Maui.Controls.HorizontalStackLayout) elements. The following example shows an example of this bad practice:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <VerticalStackLayout>
        <HorizontalStackLayout>
            <Label Text="Name:" />
            <Entry Placeholder="Enter your name" />
        </HorizontalStackLayout>
        <HorizontalStackLayout>
            <Label Text="Age:" />
            <Entry Placeholder="Enter your age" />
        </HorizontalStackLayout>
        <HorizontalStackLayout>
            <Label Text="Occupation:" />
            <Entry Placeholder="Enter your occupation" />
        </HorizontalStackLayout>
        <HorizontalStackLayout>
            <Label Text="Address:" />
            <Entry Placeholder="Enter your address" />
        </HorizontalStackLayout>
    </VerticalStackLayout>
</ContentPage>

```

This is wasteful because unnecessary layout calculations are performed. Instead, the desired layout can be better achieved using a [`Grid`](xref:Microsoft.Maui.Controls.Grid), as shown in the following example:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <Grid ColumnDefinitions="100,*"
          RowDefinitions="30,30,30,30">
        <Label Text="Name:" />
        <Entry Grid.Column="1"
               Placeholder="Enter your name" />
        <Label Grid.Row="1"
               Text="Age:" />
        <Entry Grid.Row="1"
               Grid.Column="1"
               Placeholder="Enter your age" />
        <Label Grid.Row="2"
               Text="Occupation:" />
        <Entry Grid.Row="2"
               Grid.Column="1"
               Placeholder="Enter your occupation" />
        <Label Grid.Row="3"
               Text="Address:" />
        <Entry Grid.Row="3"
               Grid.Column="1"
               Placeholder="Enter your address" />
    </Grid>
</ContentPage>
```

## Optimize layout performance

To obtain the best possible layout performance, follow these guidelines:

- Reduce the depth of layout hierarchies by specifying [`Margin`](xref:Microsoft.Maui.Controls.View.Margin) property values, allowing the creation of layouts with fewer wrapping views. For more information, see [Align and position controls](~/user-interface/align-position.md).
- When using a [`Grid`](xref:Microsoft.Maui.Controls.Grid), try to ensure that as few rows and columns as possible are set to [`Auto`](xref:Microsoft.Maui.GridLength.Auto) size. Each auto-sized row or column will cause the layout engine to perform additional layout calculations. Instead, use fixed size rows and columns if possible. Alternatively, set rows and columns to occupy a proportional amount of space with the [`GridUnitType.Star`](xref:Microsoft.Maui.GridUnitType.Star) enumeration value, provided that the parent tree follows these layout guidelines.
- Don't set the [`VerticalOptions`](xref:Microsoft.Maui.Controls.View.VerticalOptions) and [`HorizontalOptions`](xref:Microsoft.Maui.Controls.View.VerticalOptions) properties of a layout unless required. The default value of [`LayoutOptions.Fill`](xref:Microsoft.Maui.Controls.LayoutOptions.Fill) allows for the best layout optimization. Changing these properties has a cost and consumes memory, even when setting them to the default values.
- When using an [`AbsoluteLayout`](xref:Microsoft.Maui.Controls.AbsoluteLayout), avoid using the [`AbsoluteLayout.AutoSize`](xref:Microsoft.Maui.Controls.AbsoluteLayout.AutoSize) property whenever possible.
- Don't update any [`Label`](xref:Microsoft.Maui.Controls.Label) objects more frequently than required, as the change of size of the label can result in the entire screen layout being re-calculated.
- Don't set the [`Label.VerticalTextAlignment`](xref:Microsoft.Maui.Controls.Label.VerticalTextAlignment) property unless required.
- Set the [`LineBreakMode`](xref:Microsoft.Maui.Controls.Label.LineBreakMode) of any [`Label`](xref:Microsoft.Maui.Controls.Label) objects to [`NoWrap`](xref:Microsoft.Maui.LineBreakMode.NoWrap) whenever possible.

## Use asynchronous programming

The overall responsiveness of your app can be enhanced, and performance bottlenecks often avoided, by using asynchronous programming. In .NET, the [Task-based Asynchronous Pattern (TAP)](/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap) is the recommended design pattern for asynchronous operations. However, incorrect use of the TAP can result in unperformant apps. Therefore, the following guidelines should be followed when using the TAP.

### Fundamentals

- Understand the task lifecycle, which is represented by the `TaskStatus` enumeration. For more information, see [The meaning of TaskStatus](https://devblogs.microsoft.com/pfxteam/the-meaning-of-taskstatus/) and [Task status](/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap#task-status).
- Use the `Task.WhenAll` method to asynchronously wait for multiple asynchronous operations to finish, rather than individually `await` a series of asynchronous operations. For more information, see [Task.WhenAll](/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern#taskwhenall).
- Use the `Task.WhenAny` method to asynchronously wait for one of multiple asynchronous operations to finish. For more information, see [Task.WhenAny](/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern#taskwhenall).
- Use the `Task.Delay` method to produce a `Task` object that finishes after the specified time. This is useful for scenarios such as polling for data, and delaying handling user input for a predetermined time. For more information, see [Task.Delay](/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern#taskdelay).
- Execute intensive synchronous CPU operations on the thread pool with the `Task.Run` method. This method is a shortcut for the `TaskFactory.StartNew` method, with the most optimal arguments set. For more information, see [Task.Run](/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern#taskrun).
- Avoid trying to create asynchronous constructors. Instead, use lifecycle events or separate initialization logic to correctly `await` any initialization. For more information, see [Async Constructors](https://blog.stephencleary.com/2013/01/async-oop-2-constructors.html) on blog.stephencleary.com.
- Use the lazy task pattern to avoid waiting for asynchronous operations to complete during app startup. For more information, see [AsyncLazy](https://devblogs.microsoft.com/pfxteam/asynclazyt/).
- Create a task wrapper for existing asynchronous operations, that don't use the TAP, by creating `TaskCompletionSource<T>` objects. These objects gain the benefits of `Task` programmability, and enable you to control the lifetime and completion of the associated `Task`. For more information, see [The Nature of TaskCompletionSource](https://devblogs.microsoft.com/pfxteam/the-nature-of-taskcompletionsourcetresult/).

- Return a `Task` object, instead of returning an awaited `Task` object, when there's no need to process the result of an asynchronous operation. This is more performant due to less context switching being performed.
- Use the Task Parallel Library (TPL) Dataflow library in scenarios such as processing data as it becomes available, or when you have multiple operations that must communicate with each other asynchronously. For more information, see [Dataflow (Task Parallel Library)](/dotnet/standard/parallel-programming/dataflow-task-parallel-library).

### UI

- Call an asynchronous version of an API, if it's available. This will keep the UI thread unblocked, which will help to improve the user's experience with the app.
- Update UI elements with data from asynchronous operations on the UI thread, to avoid exceptions being thrown. However, updates to the `ListView.ItemsSource` property will automatically be marshaled to the UI thread. For information about determining if code is running on the UI thread, see [Create a thread on the UI thread](~/platform-integration/appmodel/main-thread.md).

    > [!IMPORTANT]
    > Any control properties that are updated via data binding will be automatically marshaled to the UI thread.

### Error handling

- Learn about asynchronous exception handling. Unhandled exceptions that are thrown by code that's running asynchronously are propagated back to the calling thread, except in certain scenarios. For more information, see [Exception handling (Task Parallel Library)](/dotnet/standard/parallel-programming/exception-handling-task-parallel-library).
- Avoid creating `async void` methods, and instead create `async Task` methods. These enable easier error-handling, composability, and testability. The exception to this guideline is asynchronous event handlers, which must return `void`. For more information, see [Avoid Async Void](/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming#avoid-async-void).
- Don't mix blocking and asynchronous code by calling the `Task.Wait`, `Task.Result`, or `GetAwaiter().GetResult` methods, as they can result in deadlock occurring. However, if this guideline must be violated, the preferred approach is to call the `GetAwaiter().GetResult` method because it preserves the task exceptions. For more information, see [Async All the Way](/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming#async-all-the-way) and [Task Exception Handling in .NET 4.5](https://devblogs.microsoft.com/pfxteam/task-exception-handling-in-net-4-5/).
- Use the `ConfigureAwait` method whenever possible, to create context-free code. Context-free code has better performance for mobile apps and is a useful technique for avoiding deadlock when working with a partially asynchronous codebase. For more information, see [Configure Context](/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming#configure-context).
- Use *continuation tasks* for functionality such as handling exceptions thrown by the previous asynchronous operation, and canceling a continuation either before it starts or while it is running. For more information, see [Chaining Tasks by Using Continuous Tasks](/dotnet/standard/parallel-programming/chaining-tasks-by-using-continuation-tasks).
- Use an asynchronous `ICommand` implementation when asynchronous operations are invoked from the `ICommand`. This ensures that any exceptions in the asynchronous command logic can be handled. For more information, see [Async Programming: Patterns for Asynchronous MVVM Applications: Commands](/archive/msdn-magazine/2014/april/async-programming-patterns-for-asynchronous-mvvm-applications-commands).

## Choose a dependency injection container carefully

Dependency injection containers introduce additional performance constraints into mobile apps. Registering and resolving types with a container has a performance cost because of the container's use of reflection for creating each type, especially if dependencies are being reconstructed for each page navigation in the app. If there are many or deep dependencies, the cost of creation can increase significantly. In addition, type registration, which usually occurs during app startup, can have a noticeable impact on startup time, dependent upon the container being used.

As an alternative, dependency injection can be made more performant by implementing it manually using factories.

## Create Shell apps

.NET MAUI Shell apps provide an opinionated navigation experience based on flyouts and tabs. If your app user experience can be implemented with Shell, it is beneficial to do so. Shell apps help to avoid a poor startup experience, because pages are created on demand in response to navigation rather than at app startup, which occurs with apps that use a [`TabbedPage'](xref:Microsoft.Maui.Controls.TabbedPage). For more information, see [Shell overview](~/fundamentals/shell/index.md).

## Optimize ListView performance

When using [`ListView`](xref:Microsoft.Maui.Controls.ListView), there are a number of user experiences that should be optimized:

- *Initialization* – the time interval starting when the control is created, and ending when items are shown on screen.
- *Scrolling* – the ability to scroll through the list and ensure that the UI doesn't lag behind touch gestures.
- *Interaction* for adding, deleting, and selecting items.

The [`ListView`](xref:Microsoft.Maui.Controls.ListView) control requires an app to supply data and cell templates. How this is achieved will have a large impact on the performance of the control. For more information, see [Cache data](~/user-interface/controls/listview.md#cache-data).

## Optimize image resources

Displaying image resources can greatly increase an app's memory footprint. Therefore, they should only be created when required and should be released as soon as the app no longer requires them. For example, if an app is displaying an image by reading its data from a stream, ensure that stream is created only when it's required, and ensure that the stream is released when it's no longer required. This can be achieved by creating the stream when the page is created, or when the [`Page.Appearing`](xref:Microsoft.Maui.Controls.Page.Appearing) event fires, and then disposing of the stream when the [`Page.Disappearing`](xref:Microsoft.Maui.Controls.Page.Disappearing) event fires.

When downloading an image for display with the [`ImageSource.FromUri`](xref:Microsoft.Maui.Controls.ImageSource.FromUri(System.Uri)) method, ensure the downloaded image is cached for a suitable amount of time. For more information, see [Image caching](~/user-interface/controls/image.md#image-caching).

## Reduce the visual tree size

Reducing the number of elements on a page will make the page render faster. There are two main techniques for achieving this. The first is to hide elements that aren't visible. The [`IsVisible`](xref:Microsoft.Maui.Controls.IsVisible) property of each element determines whether the element should be part of the visual tree or not. Therefore, if an element isn't visible because it's hidden behind other elements, either remove the element or set its `IsVisible` property to `false`.

The second technique is to remove unnecessary elements. For example, the following shows a page layout containing multiple [`Label`](xref:Microsoft.Maui.Controls.Label) elements:

```xaml
<VerticalStackLayout>
    <VerticalStackLayout Padding="20,20,0,0">
        <Label Text="Hello" />
    </VerticalStackLayout>
    <VerticalStackLayout Padding="20,20,0,0">
        <Label Text="Welcome to the App!" />
    </VerticalStackLayout>
    <VerticalStackLayout Padding="20,20,0,0">
        <Label Text="Downloading Data..." />
    </VerticalStackLayout>
</VerticalStackLayout>
```

The same page layout can be maintained with a reduced element count, as shown in the following example:

```xaml
<VerticalStackLayout Padding="20,35,20,20"
                     Spacing="25">
    <Label Text="Hello" />
    <Label Text="Welcome to the App!" />
    <Label Text="Downloading Data..." />
</VerticalStackLayout>
```

## Reduce the application resource dictionary size

Any resources that are used throughout the app should be stored in the app's resource dictionary to avoid duplication. This will help to reduce the amount of XAML that has to be parsed throughout the app. The following example shows the `HeadingLabelStyle` resource, which is used app wide, and so is defined in the app's resource dictionary:

```xaml
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.App">
     <Application.Resources>
        <Style x:Key="HeadingLabelStyle"
               TargetType="Label">
            <Setter Property="HorizontalOptions"
                    Value="Center" />
            <Setter Property="FontSize"
                    Value="Large" />
            <Setter Property="TextColor"
                    Value="Red" />
        </Style>
     </Application.Resources>
</Application>
```

However, XAML that's specific to a page shouldn't be included in the app's resource dictionary, as the resources will then be parsed at app startup instead of when required by a page. If a resource is used by a page that's not the startup page, it should be placed in the resource dictionary for that page, therefore helping to reduce the XAML that's parsed when the app starts. The following example shows the `HeadingLabelStyle` resource, which is only on a single page, and so is defined in the page's resource dictionary:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <ContentPage.Resources>
        <Style x:Key="HeadingLabelStyle"
                TargetType="Label">
            <Setter Property="HorizontalOptions"
                    Value="Center" />
            <Setter Property="FontSize"
                    Value="Large" />
            <Setter Property="TextColor"
                    Value="Red" />
        </Style>
    </ContentPage.Resources>
    ...
</ContentPage>
```

For more information about app resources, see [Style apps using XAML](~/user-interface/styles/xaml.md).
