---
title: "Host a Blazor web app in a .NET MAUI app using BlazorWebView"
description: "The .NET MAUI BlazorWebView control enables you to host a Blazor web app in your .NET MAUI app, and integrate the app with device features."
ms.date: 05/16/2022
---

# Host a Blazor web app in a .NET MAUI app using BlazorWebView

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> is a control that enables you to host a Blazor web app in your .NET MAUI app. These apps, known as Blazor Hybrid apps, enable a Blazor web app to be integrated with platform features and UI controls. The <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> control can be added to any page of a .NET MAUI app, and pointed to the root of the Blazor app. The [Razor components](/aspnet/core/blazor/components/) run natively in the .NET process and render web UI to an embedded web view control. In .NET MAUI, Blazor Hybrid apps can run on all the platforms supported by .NET MAUI.

<xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> defines the following properties:

- `HostPage`, of type `string?`, which defines the root page of the Blazor web app.
- `RootComponents`, of type `RootComponentsCollection`, which specifies the collection of root components that can be added to the control.

The `RootComponent` class defines the following properties:

- `Selector`, of type `string?`, which defines the CSS selector string that specifies where in the document the component should be placed.
- `ComponentType`, of type `Type?`, which defines the type of the root component.
- `Parameters`, of type `IDictionary<string, object?>?`, which represents an optional dictionary of parameters to pass to the root component.

In addition, <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> defines the following events:

- `BlazorWebViewInitializing`, with an accompanying `BlazorWebViewInitializingEventArgs` object, which is raised before the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> is initialized. This event enables customization of the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> configuration.
- `BlazorWebViewInitialized`, with an accompanying `BlazorWebViewInitializedEventArgs` object, which is raised after the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> is initialized but before any component has been rendered. This event enables retrieval of the platform-specific web view instance.
- `UrlLoading`, with an accompanying `UrlLoadingEventArgs` object, is raised when a hyperlink is clicked within a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView>. This event enables customization of whether a hyperlink is opened in the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView>, in an external app, or whether the URL loading attempt is cancelled.

Existing [Razor components](/aspnet/core/blazor/components/) can be used in a .NET MAUI Blazor app by moving the code into the app, or by referencing an existing class library or package that contains the component.

For more information about Blazor Hybrid apps, see [ASP.NET Core Blazor Hybrid](/aspnet/core/blazor/hybrid).

> [!NOTE]
> While Visual Studio installs all the required tooling to develop .NET MAUI Blazor apps, end users of .NET MAUI Blazor apps on Windows must install the [WebView2](https://developer.microsoft.com/microsoft-edge/webview2/) runtime.

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
                <RootComponent Selector="app" ComponentType="{x:Type local:Main}" />
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
            builder.Services.AddMauiBlazorWebViewDeveloperTools();
    #endif
            // Register any app services on the IServiceCollection object
            // e.g. builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
    ```

## Inspect a Blazor Hybrid app on Mac Catalyst

To use Safari developer tools to inspect a Blazor Hybrid app on Mac Catalyst requires you to add the `com.apple.security.get-task-allow` key, of type `Boolean`, to the *Entitlements.plist* file of your app for its debug build.

To add a new *Entitlements.plist* file to your .NET MAUI app project, add a new XML file named *Entitlements.plist* to the *Platforms\\MacCatalyst* folder of your app project. Then add the following XML to the file:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>com.apple.security.get-task-allow</key>
    <true/>
</dict>
</plist>
```

To configure your app to consume this entitlements file, add the following `<PropertyGroup>` node to your app's project file as a child of the `<Project>` node:

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst' and '$(Configuration)' == 'Debug'">
    <CodeSignEntitlements>Platforms/MacCatalyst/Entitlements.plist</CodeSignEntitlements>
</PropertyGroup>
```

This configuration ensures that the entitlements file is only processed for debug builds on Mac Catalyst.

For more information about entitlements, see [Entitlements](~/ios/entitlements.md).
