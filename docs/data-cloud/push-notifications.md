---
title: "Push notifications"
description: ""
ms.date: 07/08/2024
---

# Push notifications

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/webservices-pushnotifications)


## Prerequisites

## Setup push notification services

## Setup Azure Notification Hub

## Create an ASP.NET Core Web API backend app

In this section you'll create an ASP.NET Core Web API backend to handle [device registration](/azure/notification-hubs/notification-hubs-push-notification-registration-management#what-is-device-registration) and sending notifications to the .NET MAUI app.

### Create a web API project

1. In Visual Studio, create a **ASP.NET Core Web API** project:

IMAGE GOES HERE

1. In the **Configure your new project** dialog, name the project **PushNotificationsAPI**.

1. In the **Additional information** dialog ensure that the **Configure for HTTPS** and **Use controllers** checkboxes are enabled:

IMAGE GOES HERE

1. Once the project has been created, press <keybd>F5</keybd> to run the project.

    The app is currently configured to use the `WeatherForecastController` as the `launchUrl`, which is set in the *Properties\launchSettings.json* file. The app will launch in a web browser, and will display some JSON data.

    > [!IMPORTANT]
    > When you run an ASP.NET Core project that uses HTTPS, Visual Studio will detect if the ASP.NET Core HTTPS development certificate is installed to your local user certificate store, and will offer to install it and trust it if it's missing.

1. Close the web browser.

1. In **Solution Explorer**, expand the **Controllers** folder and delete *WeatherForecastController.cs*.
1. In **Solution Explorer**, in the root of the project, delete *WeatherForecast.cs*.
1. Open a command window, and navigate to the directory that contains the project file. Then, run the following commands:

    ```dotnetcli
    dotnet user-secrets init
    dotnet user-secrets set "NotificationHub:Name" <value>
    dotnet user-secrets set "NotificationHub:ConnectionString" "<value>"
    ```

    Replace the placeholder values with your own Azure Notification Hub name and connection string values. These can be found at the following locations in your Azure Notification Hub:

    | Configuration value | Location |
    | ------------------- | -------- |
    | `NotificationHub:Name` | See *Name* in the **Essentials** summary at the top of the **Overview** page. |
    | `NotificationHub:ConnectinString` | See *DefaultFullSharedAccessSignature** in the **Access Policies** page.|

    This sets up local configuration values using the [Secret Manager tool](/aspnet/core/security/app-secrets?tabs=windows#secret-manager). This decouples your Azure Notification Hub secrets from the Visual Studio solution, to ensure that they don't end up in source control.

    > [!TIP]
    > For production scenarios, consider a service such as [Azure KeyVault](https://azure.microsoft.com/products/key-vault/) to securely store the connection string.

### Authenticate clients with an API key

While an API key isn't as secure as a token, it will suffice for this tutorial, and be easily configured via the [ASP.NET Middleware](/aspnet/core/fundamentals/middleware):

1. Open a command window, and navigate to the directory that contains the project file. Then, run the following commands:

    ```dotnetcli
    dotnet user-secrets set "Authentication:ApiKey" <value>
    ```

    Replace the placeholder value with your API key, which can be any value.

1. In Visual Studio, add a new folder named **Authentication** to your project, and then add a new class named `ApiKeyAuthOptions` to the *Authentication* folder. Then replace the code in the *ApiKeyAuthOptions.cs* file with the following code:

    ```csharp
    using Microsoft.AspNetCore.Authentication;

    namespace PushNotificationsAPI.Authentication;

    public class ApiKeyAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKey";
        public string Scheme => DefaultScheme;
        public string ApiKey { get; set; }
    }
    ```

1. In Visual Studio, add a new class named `ApiKeyAuthHandler` to the *Authentication* folder. Then replace the code in the *ApiKeyAuthHandler.cs* file with the following code:

    ```csharp
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Options;
    using System.Security.Claims;
    using System.Text.Encodings.Web;

    namespace PushNotificationsAPI.Authentication;

    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthOptions>
    {
        const string ApiKeyIdentifier = "apikey";

        public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string key = string.Empty;

            if (Request.Headers[ApiKeyIdentifier].Any())
            {
                key = Request.Headers[ApiKeyIdentifier].FirstOrDefault();
            }
            else if (Request.Query.ContainsKey(ApiKeyIdentifier))
            {
                if (Request.Query.TryGetValue(ApiKeyIdentifier, out var queryKey))
                    key = queryKey;
            }

            if (string.IsNullOrWhiteSpace(key))
                return Task.FromResult(AuthenticateResult.Fail("No api key provided"));

            if (!string.Equals(key, Options.ApiKey, StringComparison.Ordinal))
                return Task.FromResult(AuthenticateResult.Fail("Invalid api key."));

            var identities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity("ApiKeyIdentity")
            };

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
    ```

    An [authentication handler](/aspnet/core/security/authentication#authentication-handler) is a type that implements the behavior of a scheme, which in this case is a custom API key scheme.

1. In Visual Studio, add a new class named `AuthenticationBuilderExtensions` to the *Authentication* folder. Then replace the code in the *AuthenticationBuilderExtensions.cs* file with the following code:

    ```csharp
    using Microsoft.AspNetCore.Authentication;

    namespace PushNotificationsAPI.Authentication;

    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeyAuth(this AuthenticationBuilder builder, Action<ApiKeyAuthOptions> configureOptions)
        {
            return builder
                .AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>(
                ApiKeyAuthOptions.DefaultScheme,
                configureOptions);
        }
    }
    ```

    This extension method will be used to simplify the middleware configuration code in *Program.cs*.

1. In Visual Studio, open *Program.cs* and update the code to configure the API key authentication below the call to the `builder.Services.AddControllers` method:

    ```csharp
    using PushNotificationsAPI.Authentication;

    builder.Services.AddControllers();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = ApiKeyAuthOptions.DefaultScheme;
        options.DefaultChallengeScheme = ApiKeyAuthOptions.DefaultScheme;
    }).AddApiKeyAuth(builder.Configuration.GetSection("Authentication").Bind);
    ```    

1. In *Program.cs*, update the code below the `// Configure the HTTP request pipeline` comment to call the `UseRouting`, `UseAuthentication`, and `MapControllers` extension methods:

    ```csharp
    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
    ```

    The `UseAuthentication` extension method registers the middleware that uses the previously registered authentication scheme. `UseAuthentication` must be called before any middleware that depends on users being authenticated.

### Add dependencies and configure services

ASP.NET Core supports the [dependency injection](/aspnet/core/fundamentals/dependency-injection) software design pattern, which is a technique for achieving [inversion of control](/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion) between classes and their dependencies.

Azure Notification Hubs can be accessed through the [Microsoft.Azure.NotificationHubs](https://www.nuget.org/packages/Microsoft.Azure.NotificationHubs/) NuGet package, with calls into your Azure Notification Hub being encapsulated within a service:

1. In Visual Studio, add the [Microsoft.Azure.NotificationHubs](https://www.nuget.org/packages/Microsoft.Azure.NotificationHubs/) NuGet package to your project.
1. In Visual Studio, add a new folder named **Models** to your project, and then add a new class named `PushTemplates` to the *Models* folder. Then replace the code in the *PushTemplates.cs* file with the following code:

    ```csharp
    namespace PushNotificationsAPI.Models;

    public class PushTemplates
    {
        public class Generic
        {
            public const string Android = "{ \"message\" : { \"notification\" : { \"title\" : \"PushDemo\", \"body\" : \"$(alertMessage)\"}, \"data\" : { \"action\" : \"$(alertAction)\" } } }";
            public const string iOS = "{ \"aps\" : {\"alert\" : \"$(alertMessage)\"}, \"action\" : \"$(alertAction)\" }";
        }

        public class Silent
        {
            public const string Android = "{ \"message\" : { \"data\" : {\"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\"} } }";
            public const string iOS = "{ \"aps\" : {\"content-available\" : 1, \"apns-priority\": 5, \"sound\" : \"\", \"badge\" : 0}, \"message\" : \"$(alertMessage)\", \"action\" : \"$(alertAction)\" }";
        }
    }
    ```

    The `PushTemplates` class contains tokenized notification payloads for generic and silent push notifications. These payloads are defined outside of the [installation](/azure/notification-hubs/notification-hubs-templates-cross-platform-push-messages) to allow experimentation without having to update existing installations via the service. Handling changes to installations in this way is out of scope for this article. In product scenarios, consider using [custom templates](/azure/notification-hubs/notification-hubs-templates-cross-platform-push-messages).

1. In Visual Studio, add a new class named `DeviceInstallation` to the *Models* folder. Then replace the code in the *DeviceInstallation.cs* file with the following code:

    ```csharp
    using System.ComponentModel.DataAnnotations;

    namespace PushNotificationsAPI.Models;

    public class DeviceInstallation
    {
        [Required]
        public string InstallationId { get; set; }

        [Required]
        public string Platform { get; set; }

        [Required]
        public string PushChannel { get; set; }

        public IList<string> Tags { get; set; } = Array.Empty<string>();
    }
    ```

1. In Visual Studio, add a new class named `NotificationRequest` to the *Models* folder. Then replace the code in the *NotificationRequest.cs* file with the following code:

    ```csharp
    namespace PushNotificationsAPI.Models;

    public class NotificationRequest
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public bool Silent { get; set; }
    }
    ```

1. In Visual Studio, add a new class named `NotificationHubOptions` to the *Models* folder. Then replace the code in the *NotificationHubOptions.cs* file with the following code:

    ```csharp
    using System.ComponentModel.DataAnnotations;

    namespace PushNotificationsAPI.Models;

    public class NotificationHubOptions
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
    ```

1. In Visual Studio, add a new folder named **Services** to your project, and then add a new interface named `INotificationService` to the *Services* folder. Then replace the code in the *INotificationService.cs* file with the following code:

    ```csharp
    using PushNotificationsAPI.Models;

    namespace PushNotificationsAPI.Services;

    public interface INotificationService
    {
        Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken token);
        Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token);
        Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken token);
    }
    ```

1. In Visual Studio, add a new class named `NotificationHubService` to the *Services* folder. Then replace the code in the *NotificationHubService.cs* file with the following code:

    ```csharp
    using Microsoft.Extensions.Options;
    using Microsoft.Azure.NotificationHubs;
    using PushNotificationsAPI.Models;

    namespace PushNotificationsAPI.Services;

    public class NotificationHubService : INotificationService
    {
        readonly NotificationHubClient _hub;
        readonly Dictionary<string, NotificationPlatform> _installationPlatform;
        readonly ILogger<NotificationHubService> _logger;

        public NotificationHubService(IOptions<NotificationHubOptions> options, ILogger<NotificationHubService> logger)
        {
            _logger = logger;
            _hub = NotificationHubClient.CreateClientFromConnectionString(options.Value.ConnectionString, options.Value.Name);

            _installationPlatform = new Dictionary<string, NotificationPlatform>
            {
                { nameof(NotificationPlatform.Apns).ToLower(), NotificationPlatform.Apns },
                { nameof(NotificationPlatform.FcmV1).ToLower(), NotificationPlatform.FcmV1 }
            };
        }

        public async Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(deviceInstallation?.InstallationId) ||
                string.IsNullOrWhiteSpace(deviceInstallation?.Platform) ||
                string.IsNullOrWhiteSpace(deviceInstallation?.PushChannel))
                return false;

            var installation = new Installation()
            {
                InstallationId = deviceInstallation.InstallationId,
                PushChannel = deviceInstallation.PushChannel,
                Tags = deviceInstallation.Tags
            };

            if (_installationPlatform.TryGetValue(deviceInstallation.Platform, out var platform))
                installation.Platform = platform;
            else
                return false;

            try
            {
                await _hub.CreateOrUpdateInstallationAsync(installation, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(installationId))
                return false;

            try
            {
                await _hub.DeleteInstallationAsync(installationId, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken token)
        {
            if ((notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                (string.IsNullOrWhiteSpace(notificationRequest?.Text)) ||
                string.IsNullOrWhiteSpace(notificationRequest?.Action)))
                return false;

            var androidPushTemplate = notificationRequest.Silent ?
                PushTemplates.Silent.Android :
                PushTemplates.Generic.Android;

            var iOSPushTemplate = notificationRequest.Silent ?
                PushTemplates.Silent.iOS :
                PushTemplates.Generic.iOS;

            var androidPayload = PrepareNotificationPayload(
                androidPushTemplate,
                notificationRequest.Text,
                notificationRequest.Action);

            var iOSPayload = PrepareNotificationPayload(
                iOSPushTemplate,
                notificationRequest.Text,
                notificationRequest.Action);

            try
            {
                if (notificationRequest.Tags.Length == 0)
                {
                    // This will broadcast to all users registered in the notification hub
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, token);
                }
                else if (notificationRequest.Tags.Length <= 20)
                {
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, notificationRequest.Tags, token);
                }
                else
                {
                    var notificationTasks = notificationRequest.Tags
                        .Select((value, index) => (value, index))
                        .GroupBy(g => g.index / 20, i => i.value)
                        .Select(tags => SendPlatformNotificationsAsync(androidPayload, iOSPayload, tags, token));

                    await Task.WhenAll(notificationTasks);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error sending notification");
                return false;
            }
        }

        string PrepareNotificationPayload(string template, string text, string action) => template
            .Replace("$(alertMessage)", text, StringComparison.InvariantCulture)
            .Replace("$(alertAction)", action, StringComparison.InvariantCulture);

        Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, CancellationToken token)
        {
            var sendTasks = new Task[]
            {
                _hub.SendFcmV1NativeNotificationAsync(androidPayload, token),
                _hub.SendAppleNativeNotificationAsync(iOSPayload, token)
            };

            return Task.WhenAll(sendTasks);
        }

        Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, IEnumerable<string> tags, CancellationToken token)
        {
            var sendTasks = new Task[]
            {
                _hub.SendFcmV1NativeNotificationAsync(androidPayload, tags, token),
                _hub.SendAppleNativeNotificationAsync(iOSPayload, tags, token)
            };

            return Task.WhenAll(sendTasks);
        }
    }
    ```

    The tag expression provided to the `SendTemplateNotificationsAsync` method is limited to 20 tags if they only contain ORs. Otherwise they are limited to 6 tags. For more information, see [Routing and Tag Expressions](https://learn.microsoft.com/en-us/previous-versions/azure/azure-services/dn530749(v=azure.100)?f=255&MSPPError=-2147217396).

1. In Visual Studio, open *Program.cs* and update the code to add the `NotificationHubService` as a singleton implementation of `INotificationService` below the call to the `builder.Services.AddAuthentication` method:

    ```csharp
    using PushNotificationsAPI.Authentication;
    using PushNotificationsAPI.Services;
    using PushNotificationsAPI.Models;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = ApiKeyAuthOptions.DefaultScheme;
        options.DefaultChallengeScheme = ApiKeyAuthOptions.DefaultScheme;
    }).AddApiKeyAuth(builder.Configuration.GetSection("Authentication").Bind);

    builder.Services.AddSingleton<INotificationService, NotificationHubService>();
    builder.Services.AddOptions<NotificationHubOptions>()
        .Configure(builder.Configuration.GetSection("NotificationHub").Bind)
        .ValidateDataAnnotations();

    var app = builder.Build();
    ```

### Create the notifications API

1. In Visual Studio, add a new **Controller** named `NotificationsController` to the *Controllers* folder.

    > [!TIP]
    > Choose the **API Controller with read/write actions** template.

1. In the *NotificationsController.cs* file, add the following `using` statements at the top of the file:

    ```csharp
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PushNotificationsAPI.Models;
    using PushNotificationsAPI.Services;
    ```

1. In the *NotificationsController.cs* file, decorate the `NotificationsController` class with the `Authorize` attribute:

    ```csharp
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    ```

1. In the *NotificationsController.cs* file, update the `NotificationsContoller` constructor to accept the registered instance of `INotificationService` as an argument, and assign it to a readonly member:

    ```csharp
    readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    ```

1. In the *NotificationsContoller.cs* file, replace all the methods with the following code:

    ```csharp
    [HttpPut]
    [Route("installations")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
    public async Task<IActionResult> UpdateInstallation(
        [Required] DeviceInstallation deviceInstallation)
    {
        var success = await _notificationService
            .CreateOrUpdateInstallationAsync(deviceInstallation, HttpContext.RequestAborted);

        if (!success)
            return new UnprocessableEntityResult();

        return new OkResult();
    }

    [HttpDelete()]
    [Route("installations/{installationId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
    public async Task<ActionResult> DeleteInstallation(
        [Required][FromRoute] string installationId)
    {
        // Probably want to ensure deletion even if the connection is broken
        var success = await _notificationService
            .DeleteInstallationByIdAsync(installationId, CancellationToken.None);

        if (!success)
            return new UnprocessableEntityResult();

        return new OkResult();
    }

    [HttpPost]
    [Route("requests")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
    public async Task<IActionResult> RequestPush(
        [Required] NotificationRequest notificationRequest)
    {
        if ((notificationRequest.Silent &&
            string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
            (!notificationRequest.Silent &&
            string.IsNullOrWhiteSpace(notificationRequest?.Text)))
            return new BadRequestResult();

        var success = await _notificationService
            .RequestNotificationAsync(notificationRequest, HttpContext.RequestAborted);

        if (!success)
            return new UnprocessableEntityResult();

        return new OkResult();
    }
    ```

1. In the *Properties/launchSettings.json* file, change the `launchUrl` property for each profile from `weatherforecast` to `api/notifications`.

### Create the API app

You'll now create an [API app](https://azure.microsoft.com/products/app-service/api/) in [Azure App Service](/azure/app-service/) to host your backend service. This can be accomplished directly from Visual Studio or Visual Studio Code, with Azure CLI, Azure PowerShell, Azure Developer CLI, and through the Azure Portal. For more information, see [Publish your web app](/azure/app-service/quickstart-dotnetcore?tabs=net80&pivots=development-environment-azure-portal).

To create an API app in the Azure portal:

1. In a web browser, sign into the [Azure portal](https://portal.azure.com).
1. In the Azure portal, click the **Create a resource** button and then search for and choose **API App** before selecting the **Create** button.
1. In the **Create API App** page, update the following fields before selecting the **Create** button:

    | Field | Action |
    | ----- | ------ |
    | Subscription | Choose the same target subscription you created the notification hub in. |
    | Resource Group | Choose the same resource group you created the notification hub in. |
    | Name | Enter a globally unique name. |
    | Runtime stack | Ensure that the latest version of .NET is selected. |

1. Once the **API app** has been provisioned, navigate to the resource.
1. On the **Overview** page, make a note of the default domain value. This URL is your backend endpoint that will be used from your .NET MAUI app. The URL will use the API app name that you specified, with the format `https://<app_name>.azurewebsites.net`.
1. In the app's left menu, select **Settings > Environment variables** and then ensure that the **App settings** tab is selected. Then use the **Add** button to add the following settings:

    | Name | Value |
    | ---- | ----- |
    | Authentication:ApiKey | <api_key_value> |
    | NotificationHub:Name | <hub_name_value> |
    | NotificationHub:ConnectionString | <hub_connection_string_value> |

    > [!IMPORTANT]
    > The `Authentication:ApiKey` application setting has been added for simplicity. For production scenarios, consider a service such as [Azure KeyVault](https://azure.microsoft.com/products/key-vault/) to securely store the connection string.

    Once all of these settings have been entered, select the **Apply** button and then the **Confirm** button.

### Publish the backend service

1. In Visual Studio, right-click your project and select **Publish**.
1. In the **Publish** wizard, select **Azure** and then the **Next** button.
1. In the **Publish** wizard, select **Azure App Service (Windows)** and then the **Next** button.
1. In the **Publish** wizard, follow the authentication flow to connect Visual Studio to your Azure subscription and publish the app.

Visual Studio builds, packages, and publishes the app to Azure, and then launches the app in your default browser.

For more information, see [Publish an ASP.NET web app](/visualstudio/deployment/quickstart-deploy-aspnet-web-app?tabs=azure#publish-to-azure-app-service-on-windows).

> [!TIP]
> You can download a publish profile for your app from the **Overview** page of your API app in the Azure portal and then use the profile in Visual Studio to publish your app.

### Validate the published API

To check that the API app has been published correctly, you should use tooling of your choice to send a `POST` request to the following address:

```
https://<app_name>.azurewebsites.net/api/notifications/requests
```

> [!NOTE]
> The base address is `https://<app_name>.azurewebsites.net`.

Ensure that you configure the request headers to include the key `apikey` and its value, set the body to raw, and use the following placeholder JSON content:

```
{}
```

You should receive a `400 Bad Request` response from the service.

> [!NOTE]
> It's not yet possible to test the API using valid request data since this will require platform-specific information from the .NET MAUI app.

For more information about calling REST APIs, see [Use .http files in Visual Studio](/aspnet/core/test/http-files) and [Test web APIs with the Http Repl](/aspnet/core/web-api/http-repl/?tabs=windows). In Visual Studio Code, [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) can be used to test REST APIs.

## Create a .NET MAUI app

## Configure the Android app

## Configure the iOS app

## Test the app

## Troubleshooting


---

https://learn.microsoft.com/en-us/previous-versions/azure/developer/mobile-apps/notification-hubs-backend-service-xamarin-forms

Android FCM1 setup with ANH - https://medium.com/@pramodyahk/migrating-to-fcm-v1-in-azure-notification-hubs-android-push-notifications-c342d76adfb2

https://firebase.google.com/docs/cloud-messaging/concept-options
https://developer.apple.com/documentation/usernotifications/handling-notifications-and-notification-related-actions#Handle-notifications-while-your-app-runs-in-the-foreground



## iOS

If a notification arrives when your app is running in the foreground, the system delivers that notification directly to your app. Upon receiving a notification, you can use the notification’s payload to do whatever you want. For example, you can update your app’s interface to reflect new information contained in the notification. You can then suppress any scheduled alerts or modify those alerts.


> [!NOTE]
> You would typically perform the registration (and deregisration) actions during the appropriate point in the application lifecycle (or as part of your first-run experience perhaps) without explicit user register/deregister inputs. However, this example will require explicit user input to allow this functionality to be explored and tested more easily. The notification payloads are defined outside of the [Installation](https://docs.microsoft.com/dotnet/api/microsoft.azure.notificationhubs.installation?view=azure-dotnet) to allow experimentation without having to update existing installations via the service. [Custom templates](https://docs.microsoft.com/azure/notification-hubs/notification-hubs-templates-cross-platform-push-messages) would otherwise be ideal.


// FCMv1:
//{
//  "message": {
//    "notification": {
//      "title": "Breaking News",
//      "body": "New news story available."
//    },
//    "data": {
//    "story_id": "story_12345"
//    }
//  }
//}
