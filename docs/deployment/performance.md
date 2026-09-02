---
title: "Improve app performance"
description: "Learn how to increase the performance of .NET MAUI apps by reducing the amount of work being performed by a CPU, and the amount of memory consumed by an app."
ms.date: 11/28/2025
no-loc: [ "Objective-C" ]
---

# Improve app performance

Poor app performance presents itself in many ways. It can make an app seem unresponsive, can cause slow scrolling, and can reduce device battery life. However, optimizing performance involves more than just implementing efficient code. The user's experience of app performance must also be considered. For example, ensuring that operations execute without blocking the user from performing other activities can help to improve the user's experience.

There are many techniques for increasing the performance, and perceived performance, of .NET Multi-platform App UI (.NET MAUI) apps. Collectively these techniques can greatly reduce the amount of work being performed by a CPU, and the amount of memory consumed by an app.

## Use a profiler

When developing an app, it's important to only attempt to optimize code once it has been profiled. Profiling is a technique for determining where code optimizations will have the greatest effect in reducing performance problems. The profiler tracks the app's memory usage, and records the running time of methods in the app. This data helps to navigate through the execution paths of the app, and the execution cost of the code, so that the best opportunities for optimization can be discovered.

.NET MAUI apps can be profiled using `dotnet-trace` on Android, iOS, and Mac, and Windows, and with PerfView on Windows. For more information, see [Profiling .NET MAUI apps](https://github.com/dotnet/maui/wiki/Profiling-.NET-MAUI-Apps).

The following best practices are recommended when profiling an app:

- Avoid profiling an app in a simulator, as the simulator may distort the app performance.
- Ideally, profiling should be performed on a variety of devices, as taking performance measurements on one device won't always show the performance characteristics of other devices. However, at a minimum, profiling should be performed on a device that has the lowest anticipated specification.
- Close all other apps to ensure that the full impact of the app being profiled is being measured, rather than the other apps.

## Use compiled bindings

Compiled bindings improve data binding performance in .NET MAUI apps by resolving binding expressions at compile time, rather than at runtime with reflection. Compiling a binding expression generates compiled code that typically resolves a binding 8-20 times quicker than using a classic binding. For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).

## Reduce unnecessary bindings

Don't use bindings for content that can easily be set statically. There is no advantage in binding data that doesn't need to be bound, because bindings aren't cost efficient. For example, setting `Button.Text = "Accept"` has less overhead than binding <xref:Microsoft.Maui.Controls.Button.Text?displayProperty=nameWithType> to a viewmodel `string` property with value "Accept".

## Choose the correct layout

A layout that's capable of displaying multiple children, but that only has a single child, is wasteful. For example, the following example shows a <xref:Microsoft.Maui.Controls.VerticalStackLayout> with a single child:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <VerticalStackLayout>
        <Image Source="waterfront.jpg" />
    </VerticalStackLayout>
</ContentPage>
```

This is wasteful and the <xref:Microsoft.Maui.Controls.VerticalStackLayout> element should be removed, as shown in the following example:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <Image Source="waterfront.jpg" />
</ContentPage>

```

In addition, don't attempt to reproduce the appearance of a specific layout by using combinations of other layouts, as this results in unnecessary layout calculations being performed. For example, don't attempt to reproduce a <xref:Microsoft.Maui.Controls.Grid> layout by using a combination of <xref:Microsoft.Maui.Controls.HorizontalStackLayout> elements. The following example shows an example of this bad practice:

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

This is wasteful because unnecessary layout calculations are performed. Instead, the desired layout can be better achieved using a <xref:Microsoft.Maui.Controls.Grid>, as shown in the following example:

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

## Optimize image resources

Images are some of the most expensive resources that apps use, and are often captured at high resolutions. While this creates vibrant images full of detail, apps that display such images typically require more CPU usage to decode the image and more memory to store the decoded image. It is wasteful to decode a high resolution image in memory when it will be scaled down to a smaller size for display. Instead, reduce the CPU usage and memory footprint by creating versions of stored images that are close to the predicted display sizes. For example, an image displayed in a list view should most likely be a lower resolution than an image displayed at full-screen.

In addition, images should only be created when required and should be released as soon as the app no longer requires them. For example, if an app is displaying an image by reading its data from a stream, ensure that stream is created only when it's required, and ensure that the stream is released when it's no longer required. This can be achieved by creating the stream when the page is created, or when the <xref:Microsoft.Maui.Controls.Page.Appearing?displayProperty=nameWithType> event fires, and then disposing of the stream when the <xref:Microsoft.Maui.Controls.Page.Disappearing?displayProperty=nameWithType> event fires.

When downloading an image for display with the <xref:Microsoft.Maui.Controls.ImageSource.FromUri(System.Uri)?displayProperty=nameWithType> method, ensure the downloaded image is cached for a suitable amount of time. For more information, see [Image caching](~/user-interface/controls/image.md#image-caching).

## Reduce the number of elements on a page

Reducing the number of elements on a page will make the page render faster. There are two main techniques for achieving this. The first is to hide elements that aren't visible. The <xref:Microsoft.Maui.Controls.VisualElement.IsVisible> property of each element determines whether the element should be visible on screen. If an element isn't visible because it's hidden behind other elements, either remove the element or set its `IsVisible` property to `false`. Setting the `IsVisible` property on an element to `false` retains the element in the visual tree, but excludes it from rendering and layout calculations.

The second technique is to remove unnecessary elements. For example, the following shows a page layout containing multiple <xref:Microsoft.Maui.Controls.Label> elements:

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

## Reduce the size of the app

When .NET MAUI builds your app, a linker called *ILLink* can be used to reduce the overall size of the app. ILLink reduces the size by analyzing the intermediate code produced by the compiler. It removes unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

For more information about configuring the linker behavior, see [Linking an Android app](~/android/linking.md), [Linking an iOS app](~/ios/linking.md), and [Linking a Mac Catalyst app](~/mac-catalyst/linking.md).

## Reduce the app activation period

All apps have an *activation period*, which is the time between when the app is started and when the app is ready to use. This activation period provides users with their first impression of the app, and so it's important to reduce the activation period and the user's perception of it, in order for them to gain a favorable first impression of the app.

Before an app displays its initial UI, it should provide a splash screen to indicate to the user that the app is starting. If the app can't quickly display its initial UI, the splash screen should be used to inform the user of progress through the activation period, to offer reassurance that the app hasn't hung. This reassurance could be a progress bar, or similar control.

During the activation period, apps execute activation logic, which often includes the loading and processing of resources. The activation period can be reduced by ensuring that required resources are packaged within the app, instead of being retrieved remotely. For example, in some circumstances it may be appropriate during the activation period to load locally stored placeholder data. Then, once the initial UI is displayed, and the user is able to interact with the app, the placeholder data can be progressively replaced from a remote source. In addition, the app's activation logic should only perform work that's required to let the user start using the app. This can help if it delays loading additional assemblies, as assemblies are loaded the first time they are used.

## Choose a dependency injection container carefully

Dependency injection containers introduce additional performance constraints into mobile apps. Registering and resolving types with a container has a performance cost because of the container's use of reflection for creating each type, especially if dependencies are being reconstructed for each page navigation in the app. If there are many or deep dependencies, the cost of creation can increase significantly. In addition, type registration, which usually occurs during app startup, can have a noticeable impact on startup time, dependent upon the container being used. For more information about dependency injection in .NET MAUI apps, see [Dependency injection](~/fundamentals/dependency-injection.md).

As an alternative, dependency injection can be made more performant by implementing it manually using factories.

## Create Shell apps

.NET MAUI Shell apps provide an opinionated navigation experience based on flyouts and tabs. If your app user experience can be implemented with Shell, it is beneficial to do so. Shell apps help to avoid a poor startup experience, because pages are created on demand in response to navigation rather than at app startup, which occurs with apps that use a <xref:Microsoft.Maui.Controls.TabbedPage>. For more information, see [Shell overview](~/fundamentals/shell/index.md).

> [!NOTE]
> Converting to Shell is not a universal solution for performance problems. Shell primarily improves startup time by deferring page creation. If your app has other performance issues such as slow rendering or inefficient data binding, those issues should be addressed separately.

## Use CollectionView instead of ListView

<xref:Microsoft.Maui.Controls.CollectionView> is the recommended control for displaying lists of data, as it provides better performance than <xref:Microsoft.Maui.Controls.ListView>. <xref:Microsoft.Maui.Controls.CollectionView> uses a more flexible layout model with better virtualization, which results in smoother scrolling and improved memory usage, particularly for large data sets.

For more information about migrating from <xref:Microsoft.Maui.Controls.ListView> to <xref:Microsoft.Maui.Controls.CollectionView>, see [CollectionView](~/user-interface/controls/collectionview/index.md#move-from-listview-to-collectionview).

> [!WARNING]
> Don't place a <xref:Microsoft.Maui.Controls.CollectionView> or <xref:Microsoft.Maui.Controls.ListView> inside a <xref:Microsoft.Maui.Controls.ScrollView> or a <xref:Microsoft.Maui.Controls.StackLayout>. This prevents the virtualization from working, which results in degraded performance and increased memory usage because all items are rendered at once. Instead, use the built-in header and footer capabilities of these controls if you need additional content above or below the list.

## Use asynchronous programming

The overall responsiveness of your app can be enhanced, and performance bottlenecks often avoided, by using asynchronous programming. In .NET, the [Task-based Asynchronous Pattern (TAP)](/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap) is the recommended design pattern for asynchronous operations. However, incorrect use of the TAP can result in unperformant apps.

### Fundamentals

The following general guidelines should be followed when using the TAP:

- Always `await` asynchronous methods rather than calling them without awaiting (fire-and-forget). Unawaited tasks can hide exceptions, lead to unpredictable behavior, and make debugging difficult. For more information about async/await best practices, see [Asynchronous programming with async and await](/dotnet/csharp/asynchronous-programming/).
- Understand the task lifecycle, which is represented by the `TaskStatus` enumeration. For more information, see [The meaning of TaskStatus](https://devblogs.microsoft.com/pfxteam/the-meaning-of-taskstatus/) and [Task status](/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap#task-status).
- Use the `Task.WhenAll` method to asynchronously wait for multiple asynchronous operations to finish, rather than individually `await` a series of asynchronous operations. For more information, see [Task.WhenAll](/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern#taskwhenall).
- Use the `Task.WhenAny` method to asynchronously wait for one of multiple asynchronous operations to finish. For more information, see [Task.WhenAny](/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern#taskwhenall).
- Execute intensive synchronous CPU operations on the thread pool with the `Task.Run` method. This method is a shortcut for the `TaskFactory.StartNew` method, with the most optimal arguments set. For more information, see [Task.Run](/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern#taskrun).
- Avoid trying to create asynchronous constructors. Instead, use lifecycle events or separate initialization logic to correctly `await` any initialization. For more information, see [Async Constructors](https://blog.stephencleary.com/2013/01/async-oop-2-constructors.html) on blog.stephencleary.com.
- Use the lazy task pattern to avoid waiting for asynchronous operations to complete during app startup. For more information, see [AsyncLazy](https://devblogs.microsoft.com/pfxteam/asynclazyt/).
- Create a task wrapper for existing asynchronous operations, that don't use the TAP, by creating `TaskCompletionSource<T>` objects. These objects gain the benefits of `Task` programmability, and enable you to control the lifetime and completion of the associated `Task`. For more information, see [The Nature of TaskCompletionSource](https://devblogs.microsoft.com/pfxteam/the-nature-of-taskcompletionsourcetresult/).
- Return a `Task` object, instead of returning an awaited `Task` object, when there's no need to process the result of an asynchronous operation. This is more performant due to less context switching being performed.
- Use the Task Parallel Library (TPL) Dataflow library in scenarios such as processing data as it becomes available, or when you have multiple operations that must communicate with each other asynchronously. For more information, see [Dataflow (Task Parallel Library)](/dotnet/standard/parallel-programming/dataflow-task-parallel-library).

### UI

The following guidelines should be followed when using the TAP with UI controls:

- Call an asynchronous version of an API, if it's available. This will keep the UI thread unblocked, which will help to improve the user's experience with the app.
- Update UI elements with data from asynchronous operations on the UI thread, to avoid exceptions being thrown. For information about determining if code is running on the UI thread, see [Create a thread on the UI thread](~/platform-integration/appmodel/main-thread.md).

    > [!IMPORTANT]
    > Any control properties that are updated via data binding will be automatically marshaled to the UI thread.

- <xref:Microsoft.Maui.Controls.CollectionView> will throw an exception if its `ItemsSource` is updated off the UI thread. Don't rely on auto-marshaling behavior for collection updates; instead, explicitly update collections on the UI thread.
- For best performance with <xref:Microsoft.Maui.Controls.CollectionView>, consider using a standard `List<T>` or array when data doesn't change frequently, and only use `ObservableCollection<T>` when you need automatic UI updates for individual item changes. For large data sets, batch update a regular collection and reassign `ItemsSource` rather than making many individual `ObservableCollection<T>` changes.

### Error handling

The following error handling guidelines should be followed when using the TAP:

- Learn about asynchronous exception handling. Unhandled exceptions that are thrown by code that's running asynchronously are propagated back to the calling thread, except in certain scenarios. For more information, see [Exception handling (Task Parallel Library)](/dotnet/standard/parallel-programming/exception-handling-task-parallel-library).
- Avoid creating `async void` methods, and instead create `async Task` methods. These enable easier error-handling, composability, and testability. The exception to this guideline is asynchronous event handlers, which must return `void`. For more information, see [Avoid Async Void](/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming#avoid-async-void).
- Don't mix blocking and asynchronous code by calling the `Task.Wait`, `Task.Result`, or `GetAwaiter().GetResult` methods, as they can result in deadlock occurring. However, if this guideline must be violated, the preferred approach is to call the `GetAwaiter().GetResult` method because it preserves the task exceptions. For more information, see [Async All the Way](/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming#async-all-the-way) and [Task Exception Handling in .NET 4.5](https://devblogs.microsoft.com/pfxteam/task-exception-handling-in-net-4-5/).
- Use the `ConfigureAwait` method whenever possible, to create context-free code. Context-free code has better performance for mobile apps and is a useful technique for avoiding deadlock when working with a partially asynchronous codebase. For more information, see [Configure Context](/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming#configure-context).
- Use *continuation tasks* for functionality such as handling exceptions thrown by the previous asynchronous operation, and canceling a continuation either before it starts or while it is running. For more information, see [Chaining Tasks by Using Continuous Tasks](/dotnet/standard/parallel-programming/chaining-tasks-by-using-continuation-tasks).
- Use an asynchronous <xref:System.Windows.Input.ICommand> implementation when asynchronous operations are invoked from the <xref:System.Windows.Input.ICommand>. This ensures that any exceptions in the asynchronous command logic can be handled. For more information, see [Async Programming: Patterns for Asynchronous MVVM Applications: Commands](/archive/msdn-magazine/2014/april/async-programming-patterns-for-asynchronous-mvvm-applications-commands).

## Delay the cost of creating objects

Lazy initialization can be used to defer the creation of an object until it's first used. This technique is primarily used to improve performance, avoid computation, and reduce memory requirements.

Consider using lazy initialization for objects that are expensive to create in the following scenarios:

- The app might not use the object.
- Other expensive operations must complete before the object is created.

The `Lazy<T>` class is used to define a lazy-initialized type, as shown in the following example:

```csharp
void ProcessData(bool dataRequired = false)
{
    Lazy<double> data = new Lazy<double>(() =>
    {
        return ParallelEnumerable.Range(0, 1000)
                     .Select(d => Compute(d))
                     .Aggregate((x, y) => x + y);
    });

    if (dataRequired)
    {
        if (data.Value > 90)
        {
            ...
        }
    }
}

double Compute(double x)
{
    ...
}
```

Lazy initialization occurs the first time the `Lazy<T>.Value` property is accessed. The wrapped type is created and returned on first access, and stored for any future access.

For more information about lazy initialization, see [Lazy Initialization](/dotnet/framework/performance/lazy-initialization).

## Release IDisposable resources

The `IDisposable` interface provides a mechanism for releasing resources. It provides a `Dispose` method that should be implemented to explicitly release resources. `IDisposable` is not a destructor, and should only be implemented in the following circumstances:

- When the class owns unmanaged resources. Typical unmanaged resources that require releasing include files, streams, and network connections.
- When the class owns managed `IDisposable` resources.

Type consumers can then call the `IDisposable.Dispose` implementation to free resources when the instance is no longer required. There are two approaches for achieving this:

- By wrapping the `IDisposable` object in a `using` statement.
- By wrapping the call to `IDisposable.Dispose` in a `try`/`finally` block.

### IDisposable and dependency injection

When using dependency injection, avoid registering `IDisposable` types as transient services. The dependency injection container doesn't automatically dispose of transient services, which can lead to memory leaks. Instead, register disposable types as scoped or singleton services, which the container will properly dispose. If you must use a transient service that implements `IDisposable`, consider implementing a factory pattern or manually managing the lifetime and disposal of the object. For more information, see [Dependency injection guidelines](/dotnet/core/extensions/dependency-injection-guidelines#recommendations).

### Wrap the IDisposable object in a using statement

The following example shows how to wrap an `IDisposable` object in a `using` statement:

```csharp
public void ReadText(string filename)
{
    string text;
    using (StreamReader reader = new StreamReader(filename))
    {
        text = reader.ReadToEnd();
    }
    ...
}
```

The `StreamReader` class implements `IDisposable`, and the `using` statement provides a convenient syntax that calls the `StreamReader.Dispose` method on the `StreamReader` object prior to it going out of scope. Within the `using` block, the `StreamReader` object is read-only and cannot be reassigned. The `using` statement also ensures that the `Dispose` method is called even if an exception occurs, as the compiler implements the intermediate language (IL) for a `try`/`finally` block.

### Wrap the call to IDisposable.Dispose in a try/finally block

The following example shows how to wrap the call to `IDisposable.Dispose` in a `try`/`finally` block:

```csharp
public void ReadText(string filename)
{
    string text;
    StreamReader reader = null;
    try
    {
        reader = new StreamReader(filename);
        text = reader.ReadToEnd();
    }
    finally
    {
        if (reader != null)
            reader.Dispose();
    }
    ...
}
```

The `StreamReader` class implements `IDisposable`, and the `finally` block calls the `StreamReader.Dispose` method to release the resource. For more information, see [IDisposable Interface](xref:System.IDisposable).

## Unsubscribe from events

To prevent memory leaks, events should be unsubscribed from before the subscriber object is disposed of. Until the event is unsubscribed from, the delegate for the event in the publishing object has a reference to the delegate that encapsulates the subscriber's event handler. As long as the publishing object holds this reference, garbage collection will not reclaim the subscriber object memory.

The following example shows how to unsubscribe from an event:

```csharp
public class Publisher
{
    public event EventHandler MyEvent;

    public void OnMyEventFires()
    {
        if (MyEvent != null)
            MyEvent(this, EventArgs.Empty);
    }
}

public class Subscriber : IDisposable
{
    readonly Publisher _publisher;

    public Subscriber(Publisher publish)
    {
        _publisher = publish;
        _publisher.MyEvent += OnMyEventFires;
    }

    void OnMyEventFires(object sender, EventArgs e)
    {
        Debug.WriteLine("The publisher notified the subscriber of an event");
    }

    public void Dispose()
    {
        _publisher.MyEvent -= OnMyEventFires;
    }
}
```

The `Subscriber` class unsubscribes from the event in its `Dispose` method.

Reference cycles can also occur when using event handlers and lambda syntax, as lambda expressions can reference and keep objects alive. Therefore, a reference to the anonymous method can be stored in a field and used to unsubscribe from the event, as shown in the following example:

```csharp
public class Subscriber : IDisposable
{
    readonly Publisher _publisher;
    EventHandler _handler;

    public Subscriber(Publisher publish)
    {
        _publisher = publish;
        _handler = (sender, e) =>
        {
            Debug.WriteLine("The publisher notified the subscriber of an event");
        };
        _publisher.MyEvent += _handler;
    }

    public void Dispose()
    {
        _publisher.MyEvent -= _handler;
    }
}

```

The `_handler` field maintains the reference to the anonymous method, and is used for event subscription and unsubscribe.

## Avoid strong circular references on iOS and Mac Catalyst

In some situations it's possible to create strong reference cycles that could prevent objects from having their memory reclaimed by the garbage collector. For example, consider the case where an <xref:Foundation.NSObject>-derived subclass, such as a class that inherits from <xref:UIKit.UIView>, is added to an `NSObject`-derived container and is strongly referenced from Objective-C, as shown in the following example:

```csharp
class Container : UIView
{
    public void Poke()
    {
        // Call this method to poke this object
    }
}

class MyView : UIView
{
    Container _parent;

    public MyView(Container parent)
    {
        _parent = parent;
    }

    void PokeParent()
    {
        _parent.Poke();
    }
}

var container = new Container();
container.AddSubview(new MyView(container));
```

When this code creates the `Container` instance, the C# object will have a strong reference to an Objective-C object. Similarly, the `MyView` instance will also have a strong reference to an Objective-C object.

In addition, the call to `container.AddSubview` will increase the reference count on the unmanaged `MyView` instance. When this happens, the .NET for iOS runtime creates a `GCHandle` instance to keep the `MyView` object in managed code alive, because there is no guarantee that any managed objects will keep a reference to it. From a managed code perspective, the `MyView` object would be reclaimed after the <xref:UIKit.UIView.AddSubview(UIKit.UIView)> call were it not for the `GCHandle`.

The unmanaged `MyView` object will have a `GCHandle` pointing to the managed object, known as a *strong link*. The managed object will contain a reference to the `Container` instance. In turn the `Container` instance will have a managed reference to the `MyView` object.

In circumstances where a contained object keeps a link to its container, there are several options available to deal with the circular reference:

- Avoid the circular reference by keeping a weak reference to the container.
- Call `Dispose` on the objects.
- Manually break the cycle by setting the link to the container to `null`.
- Manually remove the contained object from the container.

### Use weak references

One way to prevent a cycle is to use a weak reference from the child to the parent. For example, the above code could be as shown in the following example:

```csharp
class Container : UIView
{
    public void Poke()
    {
        // Call this method to poke this object
    }
}

class MyView : UIView
{
    WeakReference<Container> _weakParent;

    public MyView(Container parent)
    {
        _weakParent = new WeakReference<Container>(parent);
    }

    void PokeParent()
    {
        if (weakParent.TryGetTarget (out var parent))
            parent.Poke();
    }
}

var container = new Container();
container.AddSubview(new MyView container));
```

Here, the contained object will not keep the parent alive. However, the parent keeps the child alive through the call to `container.AddSubView`.

This also happens in iOS APIs that use the delegate or data source pattern, where a peer class contains the implementation. For example, when setting the <xref:UIKit.UITableView.Delegate%2A> property or the <xref:UIKit.UITableView.DataSource*> in the <xref:UIKit.UITableView> class.

In the case of classes that are created purely for the sake of implementing a protocol, for example the <xref:UIKit.IUITableViewDataSource>, what you can do is instead of creating a subclass, you can just implement the interface in the class and override the method, and assign the `DataSource` property to `this`.

### Dispose of objects with strong references

If a strong reference exists and it's difficult to remove the dependency, make a `Dispose` method clear the parent pointer.

For containers, override the `Dispose` method to remove the contained objects, as shown in the following example:

```csharp
class MyContainer : UIView
{
    public override void Dispose()
    {
        // Brute force, remove everything
        foreach (var view in Subviews)
        {
              view.RemoveFromSuperview();
        }
        base.Dispose();
    }
}
```

For a child object that keeps strong reference to its parent, clear the reference to the parent in the `Dispose` implementation:

```csharp
class MyChild : UIView
{
    MyContainer _container;

    public MyChild(MyContainer container)
    {
        _container = container;
    }

    public override void Dispose()
    {
        _container = null;
    }
}
```
