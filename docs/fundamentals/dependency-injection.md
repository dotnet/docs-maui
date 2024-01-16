---
title: Dependency injection in .NET MAUI
description: Learn how to inject dependencies in a .NET MAUI app, to decouple concrete types from the code that depends on these types.
ms.date: 01/15/2024
---

# Dependency injection

.NET Multi-platform App UI (.NET MAUI) provides in-built support for using dependency injection. Dependency injection is a specialized version of the [Inversion of Control](/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion) (IoC) pattern, where the concern being inverted is the process of obtaining the required dependency. With dependency injection, another class is responsible for injecting dependencies into an object at runtime.

Typically, a class constructor is invoked when instantiating an object, and any values that the object needs are passed as arguments to the constructor. This is an example of dependency injection known as *constructor injection*. The dependencies the object needs are injected into the constructor.

By specifying dependencies as interface types, dependency injection enables decoupling the concrete types from the code that depends on these types. It generally uses a container that holds a list of registrations and mappings between interfaces and abstract types, and the concrete types that implement or extend these types.

> [!NOTE]
> There are also other types of dependency injection, such as *property setter injection* and *method call injection*, but they are less commonly used.

There are many dependency injection containers available in .NET. .NET MAUI has in-built support for using <xref:Microsoft.Extensions.DependencyInjection> to manage the instantiation of views, view models, and service classes in an app. <xref:Microsoft.Extensions.DependencyInjection> facilitates building loosely coupled apps, and provides all of the features commonly found in dependency injection containers, including methods to register type mappings and object instances, resolve objects, manage object lifetimes, and inject dependent objects into constructors of objects that it resolves. For more information about <xref:Microsoft.Extensions.DependencyInjection>, see [Dependency injection in .NET](/dotnet/core/extensions/dependency-injection).

## Dependency injection containers

If a class doesn't directly instantiate the objects it needs, another class must take on this responsibility. Consider the following example, which shows a view-model class that requires constructor arguments:

```csharp
public class MainPageViewModel
{
    readonly ILoggingService _loggingService;
    readonly ISettingsService _settingsService;

    public MainPageViewModel(ILoggingService loggingService, ISettingsService settingsService)
    {
        _loggingService = loggingService;
        _settingsService = settingsService;
    }
}
```

In this example, the `MainPageViewModel` constructor receives two interface object instances as arguments injected by another class. The only dependency in the `MainPageViewModel` class is on the interface types. Therefore, the `MainPageViewModel` class doesn't have any knowledge of the class that's responsible for instantiating the interface objects.

Similarly, consider the following example that shows a page class that requires a constructor argument:n:

```csharp
public MainPage(MainPageViewModel viewModel)
{
    InitializeComponent();

    BindingContext = viewModel;
}
```

In this example, the `MainPage` constructor receives a concrete type as an argument that's injected. The only dependency in the `MainPage` class is on the `MainPageViewModel` type. Therefore, the `MainPage` class doesn't have any knowledge of the class that's responsible for instantiating the concrete type.

In both cases, the class that's responsible for instantiating the dependencies, and inserting them into the dependent class, is known as the *dependency injection container*.

Dependency injection containers reduce the coupling between objects by providing a facility to instantiate class instances and manage their lifetime based on the configuration of the container. During object creation, the container injects any dependencies that the object requires into it. If those dependencies have not yet been created, the container creates and resolves their dependencies first.

There are several advantages to using a dependency injection container:

- A container removes the need for a class to locate its dependencies and manage its lifetimes.
- A container allows the mapping of implemented dependencies without affecting the class.
- A container facilitates testability by allowing dependencies to be mocked.
- A container increases maintainability by allowing new classes to be easily added to the app.

In the context of a .NET MAUI app that uses the Model-View-ViewModel (MVVM) pattern, a dependency injection container will typically be used for registering and resolving views, registering and resolving view models, and for registering services and injecting them into view models.

In .NET MAUI, the `MauiProgram` class will call into the `CreateMauiApp` method to create a <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object. The <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object has a <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Services> property of type <xref:Microsoft.Extensions.DependencyInjection.IServiceCollection>, which provides a place to register our components, such as views, view models, and services for dependency injection. Any components registered with the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Services> property will be provided to the dependency injection container when the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Build?displayProperty=nameWithType> method is called.

At runtime, the container must know which implementation of the services are being requested in order to instantiate them for the requested objects. In the example above, the `ILoggingService` and `ISettingsService` interfaces need to be resolved before the `MainPageViewModel` object can be instantiated. This involves the container performing the following actions:

- Deciding how to instantiate an object that implements the interface. This is known as *registration*. For more information, see [Registration](#registration).
- Instantiating the object that implements the required interface and the `MainPageViewModel` object. This is known as *resolution*. For more information, see [Resolution](#resolution).

Eventually, an app will finish using the `MainPageViewModel` object, and it will become available for garbage collection. At this point, the garbage collector should dispose of any short-lived interface implementations if other classes do not share the same instance.

## Registration

Before dependencies can be injected into an object, the types of the dependencies must first be registered with the container. Registering a type involves passing the container an interface and a concrete type that implements the interface.

There are two ways of registering types and objects in the container through code:

- Register a type or mapping with the container. This is known as transient registration. When required, the container will build an instance of the specified type.
- Register an existing object in the container as a singleton. When required, the container will return a reference to the existing object.

> [!NOTE]
> Dependency injection containers are not always suitable. Dependency injection introduces additional complexity and requirements that might not be appropriate or useful to smaller apps. If a class doesn't have any dependencies, or isn't a dependency for other types, it might not make sense to put it in the container. In addition, if a class has a single set of dependencies that are integral to the type and will never change, it might not make sense to put them in the container.

The registration of types requiring dependency injection should be performed in a single method in an app. This method should be invoked early in the app's lifecycle to ensure it is aware of the dependencies between its classes. Apps should typically perform this in the `CreateMauiApp` method in the `MauiProgram` class:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddTransient<ILoggingService, loggingService>();
        builder.Services.AddTransient<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
```

When registering dependencies, you need to register all dependencies including including any types that require the dependencies. Therefore, if you have a view-model that takes a dependency as a constructor parameter, you need to register the view-model along with all its dependencies. Similarly, if you have a view that takes a view-model dependency as a constructor parameter, you need to register the view, and the view-model along with all its dependencies.

> [!TIP]
> The dependency injection container is ideal for creating view-model instances. If a view-model has dependencies, it will manage the creation and injection of any required services. Just ensure that you register your view models and any dependencies that they may have with the `CreateMauiApp` method in the `MauiProgram` class.  

### Dependency lifetime

Depending on the needs of your app, you may need to register dependencies with different lifetimes. The following table provides information on when you may want to choose these different registration lifetimes:

| Method | Description |
|---------|---------|
| `AddSingleton<T>` | Creates a single instance of the object which will be remain for the lifetime of the app. |
| `AddTransient<T>` | Creates a new instance of the object when requested during resolution. Transient objects do not have a pre-defined lifetime, but will typically follow the lifetime of their host. |
| `AddScoped<T>` | Creates an instance of the object that shares the lifetime of its host. When the host goes out of scope, so does its dependency. Therefore, resolving the same dependency multiple times within the same scope yields the same instance, while resolving the same dependency in different scopes will yield different instances. |

> [!NOTE]
> If an object doesn't inherit from an interface, such as a view-model, only it's concrete type needs to be provided to the `AddSingleton<T>` and `AddTransient<T>` methods.

The `MainPageViewModel` class is used near the app's root and should always be available, so registering it with `AddSingleton<T>` is beneficial. Other view-models maybe situationally navigated to or are used later in the app. Suppose you know that you have a type that might not always be used. In that case, if it's memory or computationally intensive or requires just-in-time data, it may be a better candidate for `AddTransient<T>` registration.

Another common way to add services is using the `AddSingleton<TService, TImplementation>`, `AddTransient<TService, TImplementation>`, or `AddScoped<TService, TImplementation>` methods. These methods take two input types: the interface definition and the concrete implementation. This type of registration is best for cases where you are implementing services based on interfaces.

Once all services have been registered, the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Build?displayProperty=nameWithType> method should be called to create the <xref:Microsoft.Maui.Hosting.MauiApp> object and populate the dependency injection container with all the registered services.

> [!IMPORTANT]
> Once the `Build` method has been called, the services registered with the dependency injection container will be immutable and no longer can be updated or modified.

### Register dependencies with an extension method

The `MauiApp.CreateBuilder` method creates a <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object that can be used to register dependencies. If your app needs to register many dependencies, you can create extension methods to help provide an organized and maintainable registration workflow:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
        => MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .Build();

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<ILoggingService, loggingService>();
        mauiAppBuilder.Services.AddTransient<ISettingsService, SettingsService>();

        // More services registered here.

        return mauiAppBuilder;        
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();

        // More view-models registered here.

        return mauiAppBuilder;        
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPage>();

        // More views registered here.

        return mauiAppBuilder;        
    }
}
```

In this example, the three registration extension methods use the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> instance to access the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Services> property to register dependencies.

## Resolution

After a type is registered, it can be resolved or injected as a dependency. When a type is being resolved, and the container needs to create a new instance, it injects any dependencies into the instance.

Generally, when a type is resolved, one of three things happens:

1. If the type hasn't been registered, the container throws an exception.
2. If the type has been registered as a singleton, the container returns the singleton instance. If this is the first time the type is called for, the container creates it if required and maintains a reference to it.
3. If the type has been registered as transient, the container returns a new instance and doesn't maintain a reference to it.

.NET MAUI supports *automatic* and *explicit* dependency resolution. Automatic dependency resolution uses constructor injection without explicitly requesting the dependency from the container. Explicit dependency resolution occurs on demand by explicitly requesting a dependency from the container.

### Automatic dependency resolution

Automatic dependency resolution occurs in apps that use [.NET MAUI Shell](shell/index.md), provided that you've registered the dependency's type as well as the type that uses the dependency.

Shell-based apps will use the dependency injection container to create objects during navigation.

Shell's `Routing.RegisterRoute` method associated a route path to a <xref:Microsoft.Maui.Controls.View>:

```csharp
Routing.RegisterRoute("Filter", typeof(MyItemPage));
```

During Shell-based navigation, .NET MAUI will look for page registrations, and if any are found, it will create that page and inject any dependencies into its constructor:

```csharp
public MainPage(MainPageViewModel viewModel)
{
    InitializeComponent();

    BindingContext = viewModel;
}
```

In this example, the `MainPage` constructor receives a `MainPageViewModel` instance that's injected. In turn, the `MainPageViewModel` instance has `ILoggingService` and `ISettingsService` instances injected:

```csharp
public class MainPageViewModel
{
    readonly ILoggingService _loggingService;
    readonly ISettingsService _settingsService;

    public MainPageViewModel(ILoggingService loggingService, ISettingsService settingsService)
    {
        _loggingService = loggingService;
        _settingsService = settingsService;
    }
}
```

In addition, in a Shell-based app, .NET MAUI will inject dependencies into detail pages that are registered with the `Routing.RegisterRoute` method.

### Explicit dependency resolution

If your type only exposes a parameterless constructor, then Shell-based apps can't inject dependencies for you. Alternatively, if you don't use Shell (for example, implementing navigation manually without using routes) then you'll need to use explicit dependency resolution.

The dependency injection container can be explicitly accessed from an `Element` through its `Handler.MauiContext.Service` property, which is of type <xref:System.IServiceProvider>:

```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        HandlerChanged += OnHandlerChanged;
    }

    void OnHandlerChanged(object sender, EventArgs e)
    {
        BindingContext = Handler.MauiContext.Services.GetService<MainPageViewModel>();
    }
}
```

In this example, accessing the dependency injection container in the `HandlerChanged` event handler ensures that a handler has been set for the page, and therefore that the `Handler` property won't be `null`.

This approach can be useful if you need to resolve a dependency from an `Element`, or from outside the constructor of an `Element`.

> [!CAUTION]
> The `Handler` property of your `Element` could be `null`, so be aware that you may need to account for this situation. For more information, see [Handler lifecycle](~/user-interface/handlers/index.md#handler-lifecycle).

In a view-model, the dependency injection container can be explicitly accessed through the `Handler.MauiContext.Service` property of `Application.Current.MainPage`:

```csharp
public class MainPageViewModel
{
    readonly ILoggingService _loggingService;
    readonly ISettingsService _settingsService;

    public MainPageViewModel()
    {
        _loggingService = Application.Current.MainPage.Handler.MauiContext.Services.GetService<ILoggingService>();
        _settingsService = Application.Current.MainPage.Handler.MauiContext.Services.GetService<ISettingsService>();
    }
}
```

A drawback of this approach is that the view-model now has a dependency on the `Application` and `MainPage` types. However, this drawback can be eliminated by passing an <xref:System.IServiceProvider> argument to the view-model constructor:

```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        HandlerChanged += OnHandlerChanged;
    }

    void OnHandlerChanged(object sender, EventArgs e)
    {
        BindingContext = new MainPageViewModel(Handler.MauiContext.Services);
    }
}
```

In the view-model, the <xref:System.IServiceProvider> instance can then be used for explicit dependency resolution:

```csharp
public class MainPageViewModel
{
    readonly ILoggingService _loggingService;
    readonly ISettingsService _settingsService;

    public MainPageViewModel(IServiceProvider serviceProvider)
    {
        _loggingService = serviceProvider.GetService<ILoggingService>();
        _settingsService = serviceProvider.GetService<ISettingsService>();
    }
}
```

Alternatively, an <xref:System.IServiceProvider> can also be resolved through automatic dependency resolution without having to register it with the dependency injection container. With this approach a type and its <xref:System.IServiceProvider> dependency can be automatically resolved provided that the type is registered with the dependency injection container. The <xref:System.IServiceProvider> can then be used for explicit dependency resolution.

In addition, an <xref:System.IServiceProvider> instance can be accessed through the following native properties:

- Android - `MauiApplication.Current.Services`
- iOS and Mac Catalyst - `MauiUIApplicationDelegate.Current.Services`
- Windows - `MauiWinUIApplication.Current.Services`

## Limitations with XAML resources

A common scenario is to register a page with the dependency injection container, and use automatic dependency resolution to inject it into the `App` class constructor and set it as the value of the `MainPage` property:

```csharp
public App(MyFirstAppPage page)
{
    InitializeComponent();
    MainPage = page;
}
```

However, in this scenario if `MyFirstAppPage` attempts to access a `StaticResource` that's been declared in XAML in the `App` resource dictionary, a `XamlParseException` will be thrown with a message similar to `Position {row}:{column}. StaticResource not found for key {key}`. This occurs because the page resolved through constructor injection has been created before the application-level XAML resources have been initialized.

A workaround for this issue is to inject an <xref:System.IServiceProvider> into your `App` class and then use it to resolve the page inside the `App` class:

```csharp
public App(IServiceProvider serviceProvider)
{
    InitializeComponent();
    MainPage = serviceProvider.GetService<MyFirstAppPage>();
}
```

This approach forces the XAML object tree to be created and initialized before the page is resolved.