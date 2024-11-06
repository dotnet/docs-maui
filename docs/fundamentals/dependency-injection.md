---
title: Dependency injection in .NET MAUI
description: Learn how to inject dependencies in a .NET MAUI app, to decouple concrete types from the code that depends on these types.
ms.date: 01/15/2024
---

# Dependency injection

.NET Multi-platform App UI (.NET MAUI) provides in-built support for using dependency injection. Dependency injection is a specialized version of the [Inversion of Control](/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion) (IoC) pattern, where the concern being inverted is the process of obtaining the required dependency. With dependency injection, another class is responsible for injecting dependencies into an object at runtime.

Typically, a class constructor is invoked when instantiating an object, and any values that the object needs are passed as arguments to the constructor. This is an example of dependency injection known as *constructor injection*. The dependencies the object needs are injected into the constructor.

> [!NOTE]
> There are also other types of dependency injection, such as *property setter injection* and *method call injection*, but they are less commonly used.

By specifying dependencies as interface types, dependency injection enables decoupling the concrete types from the code that depends on these types. It generally uses a container that holds a list of registrations and mappings between interfaces and abstract types, and the concrete types that implement or extend these types.

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

In this example, the `MainPageViewModel` constructor requires two interface object instances as arguments injected by another class. The only dependency in the `MainPageViewModel` class is on the interface types. Therefore, the `MainPageViewModel` class doesn't have any knowledge of the class that's responsible for instantiating the interface objects.

Similarly, consider the following example that shows a page class that requires a constructor argument:

```csharp
public MainPage(MainPageViewModel viewModel)
{
    InitializeComponent();

    BindingContext = viewModel;
}
```

In this example, the `MainPage` constructor requires a concrete type as an argument that's injected by another class. The only dependency in the `MainPage` class is on the `MainPageViewModel` type. Therefore, the `MainPage` class doesn't have any knowledge of the class that's responsible for instantiating the concrete type.

In both cases, the class that's responsible for instantiating the dependencies, and inserting them into the dependent class, is known as the *dependency injection container*.

Dependency injection containers reduce the coupling between objects by providing a facility to instantiate class instances and manage their lifetime based on the configuration of the container. During object creation, the container injects any dependencies that the object requires. If those dependencies haven't been created, the container creates and resolves their dependencies first.

There are several advantages to using a dependency injection container:

- A container removes the need for a class to locate its dependencies and manage its lifetimes.
- A container allows the mapping of implemented dependencies without affecting the class.
- A container facilitates testability by allowing dependencies to be mocked.
- A container increases maintainability by allowing new classes to be easily added to the app.

In the context of a .NET MAUI app that uses the Model-View-ViewModel (MVVM) pattern, a dependency injection container will typically be used for registering and resolving views, registering and resolving view models, and for registering services and injecting them into view-models. For more information about the MVVM pattern, see [Model-View-ViewModel (MVVM)](/dotnet/architecture/maui/mvvm?toc=/dotnet/maui/toc.json&bc=/dotnet/maui/breadcrumb/toc.json).

There are many dependency injection containers available for .NET. .NET MAUI has in-built support for using <xref:Microsoft.Extensions.DependencyInjection> to manage the instantiation of views, view models, and service classes in an app. <xref:Microsoft.Extensions.DependencyInjection> facilitates building loosely coupled apps, and provides all of the features commonly found in dependency injection containers, including methods to register type mappings and object instances, resolve objects, manage object lifetimes, and inject dependent objects into constructors of objects that it resolves. For more information about <xref:Microsoft.Extensions.DependencyInjection>, see [Dependency injection in .NET](/dotnet/core/extensions/dependency-injection).

At runtime, the container must know which implementation of the dependencies are being requested in order to instantiate them for the requested objects. In the example above, the `ILoggingService` and `ISettingsService` interfaces need to be resolved before the `MainPageViewModel` object can be instantiated. This involves the container performing the following actions:

- Deciding how to instantiate an object that implements the interface. This is known as *registration*. For more information, see [Registration](#registration).
- Instantiating the object that implements the required interface and the `MainPageViewModel` object. This is known as *resolution*. For more information, see [Resolution](#resolution).

Eventually, an app will finish using the `MainPageViewModel` object, and it will become available for garbage collection. At this point, the garbage collector should dispose of any short-lived interface implementations if other classes do not share the same instances.

## Registration

Before dependencies can be injected into an object, the types for the dependencies must first be registered with the container. Registering a type typically involves passing the container a concrete type, or an interface and a concrete type that implements the interface.

There are two main approaches to registering types and objects with the container:

- Register a type or mapping with the container. This is known as transient registration. When required, the container will build an instance of the specified type.
- Register an existing object in the container as a singleton. When required, the container will return a reference to the existing object.

> [!CAUTION]
> Dependency injection containers are not always suitable for a .NET MAUI app. Dependency injection introduces additional complexity and requirements that might not be appropriate or useful to smaller apps. If a class doesn't have any dependencies, or isn't a dependency for other types, it might not make sense to put it in the container. In addition, if a class has a single set of dependencies that are integral to the type and will never change, it might not make sense to put them in the container.

The registration of types requiring dependency injection should be performed in a single method in your app. This method should be invoked early in the app's lifecycle to ensure it's aware of the dependencies between its classes. Apps should typically perform this in the `CreateMauiApp` method in the `MauiProgram` class. The `MauiProgram` class calls into the `CreateMauiApp` method to create a <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object. The <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object has a <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Services> property of type <xref:Microsoft.Extensions.DependencyInjection.IServiceCollection>, which provides a place to register your types, such as views, view-models, and services for dependency injection:

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

        builder.Services.AddTransient<ILoggingService, LoggingService>();
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

Types that are registered with the <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Services> property are provided to the dependency injection container when <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Build?displayProperty=nameWithType> is called.

When registering dependencies, you need to register all dependencies including including any types that require the dependencies. Therefore, if you have a view-model that takes a dependency as a constructor parameter, you need to register the view-model along with all of its dependencies. Similarly, if you have a view that takes a view-model dependency as a constructor parameter, you need to register the view, and the view-model along with all its dependencies.

> [!TIP]
> A dependency injection container is ideal for creating view-model instances. If a view-model has dependencies, it will manage the creation and injection of any required services. Just ensure that you register your view-models and any dependencies that they may have in the `CreateMauiApp` method in the `MauiProgram` class.  

::: moniker range=">=net-maui-9.0"

In a Shell app, you don't need to register your pages with the dependency injection container unless you want to influence the lifetime of the page relative to the container with the [`AddSingleton`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton%2A), [`AddTransient`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient%2A), or [`AddScoped`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped%2A) methods. For more information, see [Dependency lifetime](#dependency-lifetime).

::: moniker-end

### Dependency lifetime

Depending on the needs of your app, you may need to register dependencies with different lifetimes. The following table lists the main methods you can use to register dependencies, and their registration lifetimes:

| Method | Description |
|---------|---------|
| [`AddSingleton<T>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton%2A) | Creates a single instance of the object which will remain for the lifetime of the app. |
| [`AddTransient<T>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient%2A) | Creates a new instance of the object when requested during resolution. Transient objects do not have a pre-defined lifetime, but will typically follow the lifetime of their host. |
| [`AddScoped<T>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped%2A) | Creates an instance of the object that shares the lifetime of its host. When the host goes out of scope, so does its dependency. Therefore, resolving the same dependency multiple times within the same scope yields the same instance, while resolving the same dependency in different scopes will yield different instances. |

> [!NOTE]
> If an object doesn't inherit from an interface, such as a view or view-model, only its concrete type needs to be provided to the [`AddSingleton<T>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton%2A), [`AddTransient<T>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient%2A), or [`AddScoped<T>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped%2A) method.

The `MainPageViewModel` class is used near the app's root and should always be available, so registering it with [`AddSingleton<T>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton%2A) is beneficial. Other view-models may be situationally navigated to or used later in an app. If you have a type that might not always be used, or if it's memory or computationally intensive or requires just-in-time data, it may be a better candidate for [`AddTransient<T>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient%2A) registration.

Another common way to register dependencies is using the [`AddSingleton<TService, TImplementation>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton%2A), [`AddTransient<TService, TImplementation>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient%2A), or [`AddScoped<TService, TImplementation>`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped%2A) methods. These methods take two types - the interface definition and the concrete implementation. This type of registration is best for cases where you are implementing services based on interfaces.

Once all types have been registered, <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Build?displayProperty=nameWithType> should be called to create the <xref:Microsoft.Maui.Hosting.MauiApp> object and populate the dependency injection container with all the registered types.

> [!IMPORTANT]
> Once  <xref:Microsoft.Maui.Hosting.MauiAppBuilder.Build?displayProperty=nameWithType> has been called, the types registered with the dependency injection container will be immutable and no longer can be updated or modified.

### Register dependencies with an extension method

The <xref:Microsoft.Maui.Hosting.MauiApp.CreateBuilder%2A?displayProperty=nameWithType> method creates a <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object that can be used to register dependencies. If your app needs to register many dependencies, you can create extension methods to help provide an organized and maintainable registration workflow:

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
        mauiAppBuilder.Services.AddTransient<ILoggingService, LoggingService>();
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

Generally, when a type is resolved, one of three scenarios happen:

1. If the type hasn't been registered, the container throws an exception.
2. If the type has been registered as a singleton, the container returns the singleton instance. If this is the first time the type is called for, the container creates it if required and maintains a reference to it.
3. If the type has been registered as transient, the container returns a new instance and doesn't maintain a reference to it.

.NET MAUI supports *automatic* and *explicit* dependency resolution. Automatic dependency resolution uses constructor injection without explicitly requesting the dependency from the container. Explicit dependency resolution occurs on demand by explicitly requesting a dependency from the container.

### Automatic dependency resolution

Automatic dependency resolution occurs in apps that use [.NET MAUI Shell](shell/index.md), provided that you've registered the dependency's type and the type that uses the dependency with the dependency injection container.

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

In addition, in a Shell-based app, .NET MAUI will inject dependencies into detail pages that are registered with the <xref:Microsoft.Maui.Controls.Routing.RegisterRoute%2A?displayProperty=nameWithType> method.

### Explicit dependency resolution

A Shell-based app can't use constructor injection when a type only exposes a parameterless constructor. Alternatively, if your app doesn't use Shell then you'll need to use explicit dependency resolution.

The dependency injection container can be explicitly accessed from an <xref:Microsoft.Maui.Controls.Element> through its [`Handler.MauiContext.Service`](xref:Microsoft.Maui.IMauiContext.Services) property, which is of type <xref:System.IServiceProvider>:

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

This approach can be useful if you need to resolve a dependency from an <xref:Microsoft.Maui.Controls.Element>, or from outside the constructor of an <xref:Microsoft.Maui.Controls.Element>. In this example, accessing the dependency injection container in the `HandlerChanged` event handler ensures that a handler has been set for the page, and therefore that the `Handler` property won't be `null`.

> [!WARNING]
> The `Handler` property of your `Element` could be `null`, so be aware that you may need to account for this situation. For more information, see [Handler lifecycle](~/user-interface/handlers/index.md#handler-lifecycle).

In a view-model, the dependency injection container can be explicitly accessed through the [`Handler.MauiContext.Service`](xref:Microsoft.Maui.IMauiContext.Services) property of `Application.Current.MainPage`:

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

A drawback of this approach is that the view-model now has a dependency on the <xref:Microsoft.Maui.Controls.Application> type. However, this drawback can be eliminated by passing an <xref:System.IServiceProvider> argument to the view-model constructor. The <xref:System.IServiceProvider> is resolved through automatic dependency resolution without having to register it with the dependency injection container. With this approach a type and its <xref:System.IServiceProvider> dependency can be automatically resolved provided that the type is registered with the dependency injection container. The <xref:System.IServiceProvider> can then be used for explicit dependency resolution:

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

In addition, an <xref:System.IServiceProvider> instance can be accessed on each platform through the `IPlatformApplication.Current.Services` property.

## Limitations with XAML resources

A common scenario is to register a page with the dependency injection container, and use automatic dependency resolution to inject it into the `App` constructor and set it as the value of the `MainPage` property:

```csharp
public App(MyFirstAppPage page)
{
    InitializeComponent();
    MainPage = page;
}
```

However, in this scenario if `MyFirstAppPage` attempts to access a `StaticResource` that's been declared in XAML in the `App` resource dictionary, a <xref:Microsoft.Maui.Controls.Xaml.XamlParseException> will be thrown with a message similar to `Position {row}:{column}. StaticResource not found for key {key}`. This occurs because the page resolved through constructor injection has been created before the application-level XAML resources have been initialized.

A workaround for this issue is to inject an <xref:System.IServiceProvider> into your `App` class and then use it to resolve the page inside the `App` class:

```csharp
public App(IServiceProvider serviceProvider)
{
    InitializeComponent();
    MainPage = serviceProvider.GetService<MyFirstAppPage>();
}
```

This approach forces the XAML object tree to be created and initialized before the page is resolved.
