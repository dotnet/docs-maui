---
title: "Connect to local web services from Android emulators and iOS simulators"
description: "Learn how a .NET MAUI app running in the Android emulator or iOS simulator can consume a ASP.NET Core web service running locally."
ms.date: 06/19/2024
---

# Connect to local web services from Android emulators and iOS simulators

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/webservices-rest)

Many mobile and desktop apps consume web services. During the software development phase, it's common to deploy a web service locally and consume it from an app running in the Android emulator or iOS simulator. This avoids having to deploy the web service to a hosted endpoint, and enables a straightforward debugging experience because both the app and web service are running locally.

.NET Multi-platform App UI (.NET MAUI) apps that run on Windows or MacCatalyst can consume ASP.NET Core web services that are running locally over HTTP or HTTPS without any additional work, provided that you've [trusted your development certificate](#trust-your-development-certificate). However, additional work is required when the app is running in the Android emulator or iOS simulator, and the process is different depending on whether the web service is running over HTTP or HTTPS.

## Local machine address

The Android emulator and iOS simulator both provide access to web services running over HTTP or HTTPS on your local machine. However, the local machine address is different for each.

### Android

Each instance of the Android emulator is isolated from your development machine network interfaces, and runs behind a virtual router. Therefore, an emulated device can't see your development machine or other emulator instances on the network.

However, the virtual router for each emulator manages a special network space that includes pre-allocated addresses, with the `10.0.2.2` address being an alias to your host loopback interface (127.0.0.1 on your development machine). Therefore, given a local web service that exposes a GET operation via the `/api/todoitems/` relative URI, an app running on the Android emulator can consume the operation by sending a GET request to `http://10.0.2.2:<port>/api/todoitems/` or `https://10.0.2.2:<port>/api/todoitems/`.

### iOS

The iOS simulator uses the host machine network. Therefore, apps running in the simulator can connect to web services running on your local machine via the machines IP address or via the `localhost` hostname. For example, given a local web service that exposes a GET operation via the `/api/todoitems/` relative URI, an app running on the iOS simulator can consume the operation by sending a GET request to `http://localhost:<port>/api/todoitems/` or `https://localhost:<port>/api/todoitems/`.

> [!NOTE]
> When running a .NET MAUI app in the iOS simulator from Windows, the app is displayed in the [remote iOS simulator for Windows](~/ios/remote-simulator.md). However, the app is running on the paired Mac. Therefore, there's no localhost access to a web service running in Windows for an iOS app running on a Mac.

## Local web services running over HTTP

A .NET MAUI app running in the Android emulator or iOS simulator can consume an ASP.NET Core web service that's running locally over HTTP. This can be achieved by configuring your .NET MAUI app project and your ASP.NET Core web service project to allow clear-text HTTP traffic.

In the code that defines the URL of your local web service in your .NET MAUI app, ensure that the web service URL specifies the HTTP scheme, and the correct hostname. The <xref:Microsoft.Maui.Devices.DeviceInfo> class can be used to detect the platform the app is running on. The correct hostname can then be set as follows:

```csharp
public static string BaseAddress =
    DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
public static string TodoItemsUrl = $"{BaseAddress}/api/todoitems/";
```

For more information about the <xref:Microsoft.Maui.Devices.DeviceInfo> class, see [Device information](~/platform-integration/device/information.md).

In addition, to run your app on Android you must add the required network configuration, and to run your app on iOS you must opt-out of Apple Transport Security (ATS). For more information, see [Android network configuration](#android-network-configuration) and [iOS ATS configuration](#ios-ats-configuration).

You must also ensure that your ASP.NET Core web service is configured to allow HTTP traffic. This can be achieved by adding a HTTP profile to the `profiles` section of *launchSettings.json* in your ASP.NET Core web service project:

```json
{
  ...
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "api/todoitems",
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    ...
  }
}
```

A .NET MAUI app running in the Android emulator or iOS simulator can then consume an ASP.NET Core web service that's running locally over HTTP, provided that web service is launched with the `http` profile.

### Android network configuration

There are two main approaches to enabling clear-text local traffic on Android:

- Enable cleartext network traffic for communication with all domains. For more information, see [Enable clear-text network traffic for all domains](#enable-clear-text-network-traffic-for-all-domains).
- Create a network security configuration file that permits cleartext network traffic on the `localhost` domain. For more information, see [Create a network security configuration file](#create-a-network-security-configuration-file).

#### Enable clear-text network traffic for all domains

Clear-text network traffic for all domains can be enabled by setting the `UsesCleartextTraffic` property of the `Application` attribute to `true` in the *Platforms > Android > MainApplication.cs* file in your .NET MAUI app project. This should be wrapped in an `#if DEBUG` to ensure that it isn't accidentally enabled in a production app:

```csharp
#if DEBUG
[Application(UsesCleartextTraffic = true)]
#else
[Application]
#endif
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
```

> [!NOTE]
> The `UsesCleartextTraffic` property is ignored on Android 7.0 (API 24) and higher if a network security config file is present.

#### Create a network security configuration file

A network security configuration file can be created by adding a new XML file named *network_security_config.xml* to the *Platforms\Android\Resources\xml* folder in your .NET MAUI app project. The XML file should specify the following configuration, which enables cleartext network traffic on the `localhost` domain:

```xml
<?xml version="1.0" encoding="utf-8"?>
<network-security-config>
  <domain-config cleartextTrafficPermitted="true">
    <domain includeSubdomains="true">10.0.2.2</domain>
  </domain-config>
</network-security-config>
```

> [!NOTE]
> Ensure that the build action of the *network_security_config.xml* file is set to **AndroidResource**.

Then, configure the **networkSecurityConfig** property on the **application** node in the *Platforms\Android\AndroidManifest.xml* file in your .NET MAUI app project:

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest>
    <application android:networkSecurityConfig="@xml/network_security_config" ...>
        ...
    </application>
</manifest>
```

For more information about network security configuration files, see [Network security configuration](https://developer.android.com/training/articles/security-config) on developer.android.com.

### iOS ATS configuration

To enable clear-text local traffic on iOS you should opt-out of Apple Transport Security (ATS) in your .NET MAUI app. This can be achieved by adding the following configuration to the *Platforms\iOS\Info.plist* file in your .NET MAUI app project:

```xml
<key>NSAppTransportSecurity</key>    
<dict>
    <key>NSAllowsLocalNetworking</key>
    <true/>
</dict>
```

For more information about ATS, see [Preventing Insecure Network Connections](https://developer.apple.com/documentation/security/preventing_insecure_network_connections) on developer.apple.com.

## Local web services running over HTTPS

A .NET MAUI app running in the Android emulator or iOS simulator can consume an ASP.NET Core web service that's running locally over HTTPS. The process to enable this is as follows:

1. Trust the self-signed development certificate on your machine. For more information, see [Trust your development certificate](#trust-your-development-certificate).
1. Specify the address of your local machine. For more information, see [Specify the local machine address](#specify-the-local-machine-address).
1. Bypass the local development certificate security check. For more information, see [Bypass the certificate security check](#bypass-the-certificate-security-check).

Each item will be discussed in turn.

### Trust your development certificate

Installing the .NET Core SDK installs the ASP.NET Core HTTPS development certificate to your local user certificate store. However, while the certificate has been installed, it's not trusted. To trust the certificate, perform the following one-time step to run the dotnet `dev-certs` tool:

```dotnetcli
dotnet dev-certs https --trust
```

The following command provides help on the `dev-certs` tool:

```dotnetcli
dotnet dev-certs https --help
```

Alternatively, when you run an ASP.NET Core 2.1 project (or above), that uses HTTPS, Visual Studio will detect if the development certificate is missing and will offer to install it and trust it.

> [!NOTE]
> The ASP.NET Core HTTPS development certificate is self-signed.

For more information about enabling local HTTPS on your machine, see [Enable local HTTPS](/aspnet/core/getting-started#enable-local-https).

### Specify the local machine address

In the code that defines the URL of your local web service in your .NET MAUI app, ensure that the web service URL specifies the HTTPS scheme, and the correct hostname. The <xref:Microsoft.Maui.Devices.DeviceInfo> class can be used to detect the platform the app is running on. The correct hostname can then be set as follows:

```csharp
public static string BaseAddress =
    DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:5001" : "https://localhost:5001";
public static string TodoItemsUrl = $"{BaseAddress}/api/todoitems/";
```

For more information about the <xref:Microsoft.Maui.Devices.DeviceInfo> class, see [Device information](~/platform-integration/device/information.md).

### Bypass the certificate security check

Attempting to invoke a local secure web service from a .NET MAUI app running in an Android emulator will result in a `java.security.cert.CertPathValidatorException` being thrown, with a message indicating that the trust anchor for the certification path hasn't been found. Similarly, attempting to invoke a local secure web service from a .NET MAUI app running in an iOS simulator will result in an `NSURLErrorDomain` error with a message indicating that the certificate for the server is invalid. These errors occur because the local HTTPS development certificate is self-signed, and self-signed certificates aren't trusted by Android or iOS. Therefore, it's necessary to ignore SSL errors when an app consumes a local secure web service.

This can be accomplished by passing configured versions of the native `HttpMessageHandler` classes to the `HttpClient` constructor, which instruct the `HttpClient` class to trust localhost communication over HTTPS. The `HttpMessageHandler` class is an abstract class, whose implementation on Android is provided by the `AndroidMessageHandler` class, and whose implementation on iOS is provided by the `NSUrlSessionHandler` class.

The following example shows a class that configures the `AndroidMessageHandler` class on Android and the `NSUrlSessionHandler` class on iOS to trust localhost communication over HTTPS:

```csharp
public class HttpsClientHandlerService
{
    public HttpMessageHandler GetPlatformMessageHandler()
    {
#if ANDROID
        var handler = new Xamarin.Android.Net.AndroidMessageHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            if (cert != null && cert.Issuer.Equals("CN=localhost"))
                return true;
            return errors == System.Net.Security.SslPolicyErrors.None;
        };
        return handler;
#elif IOS
        var handler = new NSUrlSessionHandler
        {
            TrustOverrideForUrl = IsHttpsLocalhost
        };
        return handler;
#else
     throw new PlatformNotSupportedException("Only Android and iOS supported.");
#endif
    }

#if IOS
    public bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
    {
        return url.StartsWith("https://localhost");
    }
#endif
}
```

On Android, the `GetPlatformMessageHandler` method returns an `AndroidMessageHandler` object. The `GetPlatformMessageHandler` method sets the `ServerCertificateCustomValidationCallback` property on the `AndroidMessageHandler` object to a callback that ignores the result of the certificate security check for the local HTTPS development certificate.

On iOS, the `GetPlatformMessageHandler` method returns a `NSUrlSessionHandler` object that sets its `TrustOverrideForUrl` property to a delegate named `IsHttpsLocalHost` that matches the signature of the `NSUrlSessionHandler.NSUrlSessionHandlerTrustOverrideForUrlCallback` delegate. The `IsHttpsLocalHost` delegate returns `true` when the URL starts with `https://localhost`.

The resulting `HttpClientHandler` object can then be passed as an argument to the `HttpClient` constructor for debug builds:

```csharp
#if DEBUG
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            HttpClient client = new HttpClient(handler.GetPlatformMessageHandler());
#else
            client = new HttpClient();
#endif
```

A .NET MAUI app running in the Android emulator or iOS simulator can then consume an ASP.NET Core web service that's running locally over HTTPS.
