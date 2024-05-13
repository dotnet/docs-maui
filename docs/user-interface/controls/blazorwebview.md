---
title: "Host a Blazor web app in a .NET MAUI app using BlazorWebView"
description: "The .NET MAUI BlazorWebView control enables you to host a Blazor web app in your .NET MAUI app, and integrate the app with device features."
ms.date: 05/03/2024
---

# Host a Blazor web app in a .NET MAUI app using BlazorWebView

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> is a control that enables you to host a Blazor web app in your .NET MAUI app. These apps, known as Blazor Hybrid apps, enable a Blazor web app to be integrated with platform features and UI controls. The <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> control can be added to any page of a .NET MAUI app, and pointed to the root of the Blazor app. The [Razor components](/aspnet/core/blazor/components/) run natively in the .NET process and render web UI to an embedded web view control. In .NET MAUI, Blazor Hybrid apps can run on all the platforms supported by .NET MAUI.

<xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> defines the following properties:

- <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.HostPage>, of type `string?`, which defines the root page of the Blazor web app.
- <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.RootComponents>, of type `RootComponentsCollection`, which specifies the collection of root components that can be added to the control.
- <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.StartPath>, of type `string`, which defines the path for initial navigation within the Blazor navigation context when the Blazor component is finished loading.

The <xref:Microsoft.AspNetCore.Components.WebView.Maui.RootComponent> class defines the following properties:

- <xref:Microsoft.AspNetCore.Components.WebView.Maui.RootComponent.Selector>, of type `string?`, which defines the CSS selector string that specifies where in the document the component should be placed.
- <xref:Microsoft.AspNetCore.Components.WebView.Maui.RootComponent.ComponentType>, of type `Type?`, which defines the type of the root component.
- <xref:Microsoft.AspNetCore.Components.WebView.Maui.RootComponent.Parameters>, of type `IDictionary<string, object?>?`, which represents an optional dictionary of parameters to pass to the root component.

In addition, <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> defines the following events:

- <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.BlazorWebViewInitializing>, with an accompanying `BlazorWebViewInitializingEventArgs` object, which is raised before the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> is initialized. This event enables customization of the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> configuration.
- <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.BlazorWebViewInitialized>, with an accompanying `BlazorWebViewInitializedEventArgs` object, which is raised after the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> is initialized but before any component has been rendered. This event enables retrieval of the platform-specific web view instance.
- <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.UrlLoading>, with an accompanying `UrlLoadingEventArgs` object, is raised when a hyperlink is clicked within a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView>. This event enables customization of whether a hyperlink is opened in the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView>, in an external app, or whether the URL loading attempt is cancelled.

Existing [Razor components](/aspnet/core/blazor/components/) can be used in a .NET MAUI Blazor app by moving the code into the app, or by referencing an existing class library or package that contains the component. For more information, see [Reuse Razor components in ASP.NET Core Blazor Hybrid](/aspnet/core/blazor/hybrid/reuse-razor-components).

Browser developer tools can be used to inspect .NET MAUI Blazor apps. For more information, see [Use browser developer tools with ASP.NET Core Blazor Hybrid](/aspnet/core/blazor/hybrid/developer-tools).

> [!NOTE]
> While Visual Studio installs all the required tooling to develop .NET MAUI Blazor apps, end users of .NET MAUI Blazor apps on Windows must install the [WebView2](https://developer.microsoft.com/microsoft-edge/webview2/) runtime.

For more information about Blazor Hybrid apps, see [ASP.NET Core Blazor Hybrid](/aspnet/core/blazor/hybrid).

## Create a .NET MAUI Blazor app

A .NET MAUI Blazor app can be created in Visual Studio by the **.NET MAUI Blazor app** template:

:::image type="content" source="media/blazorwebview/project-template.png" alt-text=".NET MAUI Blazor app project template screenshot.":::

This project template creates a multi-targeted .NET MAUI Blazor app that can be deployed to Android, iOS, macOS, and Windows. For step-by-step instructions on creating a .NET MAUI Blazor app, see [Build a .NET MAUI Blazor app](/aspnet/core/blazor/hybrid/tutorials/maui).

The <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> created by the project template is defined in *MainPage.xaml*, and points to the root of the Blazor app:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:BlazorWebViewDemo"
             x:Class="BlazorWebViewDemo.MainPage"
             BackgroundColor="{DynamicResource PageBackgroundColor}">

    <BlazorWebView HostPage="wwwroot/index.html">
        <BlazorWebView.RootComponents>
            <RootComponent Selector="#app" ComponentType="{x:Type local:Main}" />
        </BlazorWebView.RootComponents>
    </BlazorWebView>

</ContentPage>
```

The root [Razor component](/aspnet/core/blazor/components/) for the app is in *Main.razor*, which Razor compiles into a type named `Main` in the application's root namespace. The rest of the [Razor components](/aspnet/core/blazor/components/) are in the *Pages* and *Shared* project folders, and are identical to the components used in the default Blazor web template. Static web assets for the app are in the *wwwroot* folder.

## Add a BlazorWebView to an existing app

The process to add a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> to an existing .NET MAUI app is as follows:

1. Add the Razor SDK, `Microsoft.NET.Sdk.Razor` to your project by editing its first line of the CSPROJ project file:

    ```xml
    <Project Sdk="Microsoft.NET.Sdk.Razor">
    ```

    The Razor SDK is required to build and package projects containing Razor files for Blazor projects.

1. Add the root [Razor component](/aspnet/core/blazor/components/) for the app to the project.
1. Add your [Razor components](/aspnet/core/blazor/components/) to project folders named *Pages* and *Shared*.
1. Add your static web assets to a project folder named *wwwroot*.
1. Add any optional *_Imports.razor* files to your project.
1. Add a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> to a page in your .NET MAUI app, and point it to the root of the Blazor app:

    ```xaml
    <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                 xmlns:local="clr-namespace:MyBlazorApp"
                 x:Class="MyBlazorApp.MainPage">

        <BlazorWebView HostPage="wwwroot/index.html">
            <BlazorWebView.RootComponents>
                <RootComponent Selector="#app" ComponentType="{x:Type local:Main}" />
            </BlazorWebView.RootComponents>
        </BlazorWebView>

    </ContentPage>
    ```

1. Modify the `CreateMauiApp` method of your `MauiProgram` class to register the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> control for use in your app. To do this, on the `IServiceCollection` object, call the `AddMauiBlazorWebView` method to add component web view services to the services collection:

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
                });

            builder.Services.AddMauiBlazorWebView();
    #if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    #endif
            // Register any app services on the IServiceCollection object
            // e.g. builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
    ```

## Access scoped services from native UI

<xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> has a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.TryDispatchAsync%2A> method that can call a specified `Action<ServiceProvider>` asynchronously and pass in the scoped services available in Razor components. This enables code from the native UI to access scoped services such as <xref:Microsoft.AspNetCore.Components.NavigationManager>:

```csharp
private async void OnMyMauiButtonClicked(object sender, EventArgs e)
{
    var wasDispatchCalled = await blazorWebView.TryDispatchAsync(sp =>
    {
        var navMan = sp.GetRequiredService<NavigationManager>();
        navMan.CallSomeNavigationApi(...);
    });

    if (!wasDispatchCalled)
    {
        // Consider what to do if it the dispatch fails - that's up to your app to decide.
    }
}
```

## Diagnosing issues

<xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> has built-in logging that can help you diagnose issues in your Blazor Hybrid app. There are two steps to enable this logging:

1. Enable <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> and related components to log diagnostic information.
1. Configure a logger to write the log output to where you can view it.

For more information about logging, see [Logging in C# and .NET](/dotnet/core/extensions/logging).

### Enable BlazorWebView logging

All logging configuration can be performed as part of service registration in the dependency injection system. To enable maximum logging for <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> and related components under the <xref:Microsoft.AspNetCore.Components.WebView?displayProperty=fullName> namespace, add the following code to where your app's services are registered:

```csharp
services.AddLogging(logging =>
{
    logging.AddFilter("Microsoft.AspNetCore.Components.WebView", LogLevel.Trace);
});
```

Alternatively, to enable maximum logging for every component that uses <xref:Microsoft.Extensions.Logging?displayProperty=fullName>, you could use the following code:

```csharp
services.AddLogging(logging =>
{
    logging.SetMinimumLevel(LogLevel.Trace);
});
```

### Configure logging output and viewing the output

After configuring components to write log information you need to configure where the loggers should write the logs to, and then view the log output.

The **Debug** logging providers write the output using `Debug` statements, and the output can be viewed from Visual Studio.

To configure the **Debug** logging provider, first add a reference in your project to the [`Microsoft.Extensions.Logging.Debug`](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Debug) NuGet package. Then, register the provider inside the call to <xref:Microsoft.Extensions.DependencyInjection.LoggingServiceCollectionExtensions.AddLogging%2A> that you added in the previous step by calling the <xref:Microsoft.Extensions.Logging.DebugLoggerFactoryExtensions.AddDebug%2A> extension method:

```csharp
services.AddLogging(logging =>
{
    logging.AddFilter("Microsoft.AspNetCore.Components.WebView", LogLevel.Trace);
    logging.AddDebug();
});
```

When you run the app from Visual Studio (with debugging enabled), you can view the debug output in Visual Studio's **Output** window.

## Play inline video on iOS

To play inline video in a Blazor hybrid app on iOS, in a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView>, you should:

- Set the <xref:Microsoft.AspNetCore.Components.WebView.UrlLoadingEventArgs.UrlLoadingStrategy> property to `OpenInWebView`. This can be accomplished in the event handler for the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.UrlLoading> event:

    ```csharp
    private void BlazorUrlLoading(object? sender, UrlLoadingEventArgs e)
    {
    #if IOS
        e.UrlLoadingStrategy = UrlLoadingStrategy.OpenInWebView;
    #endif
    }
    ```

- Ensure that the `AllowsInlineMediaPlayback` property in a `Configuration` object is set to `true`. This can be accomplished in the event handler for the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.BlazorWebViewInitializing> event:

    ```csharp
    private void BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs e)
    {
    #if IOS
        e.Configuration.AllowsInlineMediaPlayback = true;
    #endif
    }
    ```
