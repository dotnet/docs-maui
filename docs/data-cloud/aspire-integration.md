---
title: ".NET MAUI integration with Aspire"
description: "Learn how to use Aspire to simplify connecting your .NET MAUI app to local web services during development."
ms.date: 10/31/2024
---

# .NET MAUI integration with Aspire

::: moniker range=">=net-maui-10.0"

> [!IMPORTANT]
> This feature is currently in preview. Integration with Visual Studio 2026 is coming but not completely available yet.

Aspire is an opinionated, cloud-ready stack for building observable, production-ready, distributed applications. The .NET MAUI integration with Aspire simplifies the development experience when building mobile and desktop applications that connect to local web services during development.

## What is Aspire?

Aspire provides a consistent, opinionated set of tools and patterns for building and running distributed applications. It's designed to improve the experience of building cloud-native applications by providing:

- **Orchestration**: Simplified management of multiple services and dependencies
- **Components**: Pre-built integrations for common services and platforms
- **Tooling**: Developer dashboard for monitoring and managing services
- **Service discovery**: Automatic configuration for service-to-service communication

For more information about Aspire, see [Aspire documentation](/dotnet/aspire/).

## Benefits of using Aspire with .NET MAUI

Integrating Aspire with your .NET MAUI applications provides several key benefits:

- **Simplified configuration**: Eliminate complex platform-specific networking configuration. No need to manually handle `10.0.2.2` for Android or deal with certificate validation issues.
- **Automatic service discovery**: Your MAUI app automatically discovers and connects to local services without hardcoded URLs.
- **Development tunnels integration**: Built-in support for Dev Tunnels on iOS and Android, making it easy to connect mobile emulators and simulators to local services.
- **Consistent experience**: Use the same patterns and tools across your entire application stack.
- **Observable services**: Monitor your services through the Aspire dashboard during development.

### Comparison with traditional approach

Traditionally, connecting a .NET MAUI app to local web services requires significant manual configuration. You need to:

- Use different URLs for different platforms (localhost, 10.0.2.2, etc.)
- Configure network security settings for Android
- Set up Apple Transport Security (ATS) for iOS
- Handle certificate validation for HTTPS
- Manually manage service URLs in your code

For more information about the traditional approach, see [Connect to local web services](local-web-services.md).

With Aspire integration, these complexities are handled automatically, allowing you to focus on building your application instead of configuring network access.

## Prerequisites

To use Aspire with .NET MAUI, you need:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- [Aspire workload](/dotnet/aspire/fundamentals/setup-tooling)
- A .NET MAUI app targeting .NET 10 or later
- One or more ASP.NET Core web services

To install the Aspire workload, run:

```dotnetcli
dotnet workload install aspire
```

## Getting started

Setting up Aspire integration with your .NET MAUI application involves adding two key projects to your solution:

1. **MAUI Service Defaults project**: Provides default configuration for your MAUI app
2. **App Host project**: Orchestrates your application services and handles service discovery

### Create a MAUI Service Defaults project

The MAUI Service Defaults project contains shared configuration that your MAUI application will use to connect to services. Create this project in your solution directory:

```dotnetcli
dotnet new maui-aspire-servicedefaults -n YourApp.MauiServiceDefaults
```

This project includes:

- Service discovery configuration
- Default resilience patterns
- Telemetry setup
- Platform-specific networking configuration

Add a reference to this project from your .NET MAUI app project:

```dotnetcli
dotnet add YourMauiApp.csproj reference YourApp.MauiServiceDefaults/YourApp.MauiServiceDefaults.csproj
```

### Create an App Host project

The App Host project orchestrates all your application services, including your MAUI app and any backend services. Create this project in your solution directory:

```dotnetcli
dotnet new aspire-apphost -n YourApp.AppHost
```

Add references to your MAUI app and any web service projects:

```dotnetcli
dotnet add YourApp.AppHost.csproj reference YourMauiApp/YourMauiApp.csproj
dotnet add YourApp.AppHost.csproj reference YourWebService/YourWebService.csproj
```

### Configure the App Host

In your App Host project's `Program.cs`, register your MAUI app and web services:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Register your web service
var apiService = builder.AddProject<Projects.YourWebService>("apiservice");

// Register your MAUI app with a reference to the API service
builder.AddProject<Projects.YourMauiApp>("mauiapp")
    .WithReference(apiService);

builder.Build().Run();
```

> [!NOTE]
> Screenshot suggestion: Show the Visual Studio Solution Explorer with the project structure including the MAUI app, Service Defaults, App Host, and Web Service projects.

### Configure your MAUI app

In your .NET MAUI app's `MauiProgram.cs`, configure service defaults and add HTTP clients:

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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

        // Add service defaults
        builder.Services.AddServiceDefaults();

        // Configure HTTP client with service discovery
        builder.Services.AddHttpClient<WeatherApiClient>(client =>
        {
            // Service name matches the name used in App Host
            client.BaseAddress = new Uri("https+http://apiservice");
        });

        return builder.Build();
    }
}
```

The `https+http://` scheme is special syntax that enables both HTTPS and HTTP protocols, with preference for HTTPS. The service name (`apiservice` in this example) must match the name you used when registering the service in your App Host's `Program.cs`.

### Create a service client

Create a typed client to consume your web service:

```csharp
public class WeatherApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherForecast[]?> GetWeatherForecastAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<WeatherForecast[]>(
            "/weatherforecast",
            cancellationToken);
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
```

### Use the client in your app

Inject and use the client in your pages or view models:

```csharp
public partial class MainPage : ContentPage
{
    private readonly WeatherApiClient _weatherClient;

    public MainPage(WeatherApiClient weatherClient)
    {
        _weatherClient = weatherClient;
        InitializeComponent();
    }

    private async void OnGetWeatherClicked(object sender, EventArgs e)
    {
        try
        {
            var forecasts = await _weatherClient.GetWeatherForecastAsync();
            
            if (forecasts != null)
            {
                // Display the weather data
                ResultLabel.Text = $"Retrieved {forecasts.Length} forecasts";
            }
        }
        catch (Exception ex)
        {
            ResultLabel.Text = $"Error: {ex.Message}";
        }
    }
}
```

## Platform-specific considerations

### iOS and Mac Catalyst

On iOS and Mac Catalyst, the Aspire integration works seamlessly when running the app through the App Host. The service discovery configuration automatically provides the correct URLs for connecting to local services.

When using the iOS Simulator or running on a physical device, Dev Tunnels are automatically configured to enable connectivity to services running on your development machine.

> [!NOTE]
> Screenshot suggestion: Show the Aspire dashboard with a MAUI iOS app connected to a web service, displaying the active connections and logs.

### Android

On Android, the Aspire integration handles the platform-specific networking requirements automatically. You no longer need to:

- Configure the special `10.0.2.2` address
- Set up network security configuration files
- Enable clear-text traffic for local development

The integration uses Dev Tunnels to provide secure, reliable connectivity between the Android emulator and your local services.

> [!NOTE]
> Screenshot suggestion: Show the Android emulator running a MAUI app successfully connecting to a local service through the Aspire integration.

#### Dev Tunnels integration

Dev Tunnels provide a secure way to expose your local web services to mobile devices and emulators. The Aspire integration automatically:

- Creates and manages Dev Tunnels for your services
- Configures your MAUI app to use the tunnel URLs
- Handles authentication and connection management

This eliminates the need for complex network configuration and makes it easy to test your app on physical devices.

For more information about Dev Tunnels, see [Dev tunnels documentation](/aspnet/core/test/dev-tunnels).

### Windows

On Windows, local service connectivity works directly through localhost without requiring additional configuration. The Aspire integration provides a consistent API across all platforms, but the underlying implementation on Windows is straightforward.

## Running your application

To run your MAUI app with Aspire integration:

1. Set the App Host project as the startup project in Visual Studio or your preferred IDE
2. Run the solution (F5 or Debug > Start Debugging)
3. The Aspire dashboard will open, showing all registered services
4. Your MAUI app will launch and automatically connect to the configured services

> [!NOTE]
> Screenshot suggestion: Show the Aspire dashboard with multiple services running (MAUI app, web service, database if applicable) with their status indicators and resource usage.

When you run your application through the App Host:

- All services start automatically
- Service discovery is configured
- The dashboard provides real-time monitoring
- Logs from all services are available in one place

### Selecting your platform target

When running through the App Host, you can select which platform to target:

1. In Visual Studio, use the target framework dropdown to select your desired platform (Android, iOS, Windows, etc.)
2. The App Host will launch your MAUI app on the selected platform
3. Service connectivity works automatically on all platforms

## Monitoring and debugging

The Aspire dashboard provides powerful tools for monitoring and debugging your application:

- **Resource view**: See all running services and their status
- **Logs**: View combined logs from all services in one place
- **Traces**: Distributed tracing across services
- **Metrics**: Monitor performance and resource usage

> [!NOTE]
> Screenshot suggestion: Show the Aspire dashboard's traces view, displaying a request flowing from the MAUI app through to a backend service.

## Best practices

When building .NET MAUI apps with Aspire integration:

- **Use typed clients**: Create strongly-typed HTTP clients for each service to improve maintainability
- **Handle errors gracefully**: Network operations can fail; implement proper error handling and retry logic
- **Leverage the dashboard**: Use the Aspire dashboard for debugging and monitoring during development
- **Test on all platforms**: While the integration handles platform differences, always test on your target platforms
- **Follow service defaults**: The service defaults project provides recommended patterns for resilience and telemetry

## Sample application

For a complete working example of .NET MAUI integration with Aspire, see the [AspireWithMaui sample](https://github.com/dotnet/aspire/tree/main/playground/AspireWithMaui) in the Aspire repository.

The sample demonstrates:

- Complete project structure
- Service registration and discovery
- Platform-specific considerations
- Error handling and resilience patterns

## Additional resources

- [Aspire documentation](/dotnet/aspire/)
- [Service discovery in Aspire](/dotnet/aspire/service-discovery/overview)
- [Aspire orchestration](/dotnet/aspire/fundamentals/app-host-overview)
- [Connect to local web services (traditional approach)](local-web-services.md)

::: moniker-end

::: moniker range="<net-maui-10.0"

.NET MAUI integration with Aspire is available in .NET MAUI 10 and later. For information about connecting to local web services in earlier versions, see [Connect to local web services](local-web-services.md).

::: moniker-end
